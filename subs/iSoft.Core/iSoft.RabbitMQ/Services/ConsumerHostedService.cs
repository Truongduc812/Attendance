using iSoft.Common.ConfigsNS;
using iSoft.Common.Exceptions;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Utils;
using iSoft.Common;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using iSoft.Common.Cached;
using iSoft.RabbitMQ.Payload;
using static iSoft.Common.Messages;
using iSoft.Common.Enums;
using iSoft.Redis.Services;
using iSoft.Common.Services;
using iSoft.InfluxDB.Services;
using Prometheus;
using iSoft.Common.MetricsNS;

namespace iSoft.RabbitMQ.Services
{
	public class ConsumerHostedService : BackgroundService
	{
		public ILogger _logger;
		public IModel _model;
		public IConnection _connection;
		public QueueProperties _queueProperties;
		public MemCached cached = new MemCached(5);
		public EnumConnectionStatus RabbitMQConnectionStatus = EnumConnectionStatus.None;
		public RabbitMQService _rabbitMQService;

		public ConsumerHostedService(ILogger<ConsumerHostedService> logger, RabbitMQService rabbitMQService, string queueName)
		{
			_logger = logger;
			RabbitMQConnectionStatus = initRabbitMQ(rabbitMQService, queueName);
		}
		public EnumConnectionStatus initRabbitMQ(RabbitMQService rabbitMQService, string queueName)
		{
			try
			{
				try
				{
					if (_model != null && _model.IsOpen)
						_model.Close();
					if (_connection != null && _connection.IsOpen)
						_connection.Close();
				}
				catch (Exception ex) { }

				_logger.LogInformation($"*** TRY CONNECT RABBITMQ *** {CommonConfig.GetConfig().RabbitMQConfig.GetHostName()}");

				_rabbitMQService = rabbitMQService;
				_queueProperties = MessageQueueConfig.GetQueueProperties(queueName);

				_connection = rabbitMQService.CreateChannel();
				_connection.ConnectionShutdown += ConnectionShutdownHandler;
				_connection.CallbackException += ConnectionCallbackException;
				_model = _connection.CreateModel();

				_model.ExchangeDeclare(exchange: _queueProperties.ExchangeName, type: ExchangeType.Fanout, _queueProperties.Durable, false);
				_model.BasicQos(0, _queueProperties.RabbitPrefetchCount, false);

				rabbitMQService.ProcessOneProp(_model, _queueProperties);
				_logger.LogInformation("*** CONNECT RABBITMQ SUCCESS ***");
				return EnumConnectionStatus.Connected;
			}
			catch (Exception ex)
			{
				_logger.LogMsg(Messages.ErrException, ex);
				return EnumConnectionStatus.Error;
			}
		}

		private void ConnectionCallbackException(object? sender, CallbackExceptionEventArgs e)
		{
			throw new NotImplementedException("RabbitMQ ConnectionCallbackException");
		}

		private void ConnectionShutdownHandler(object sender, ShutdownEventArgs e)
		{
			if (e.Initiator == ShutdownInitiator.Application)
			{
				_logger.LogWarning("RabbitMQ connection closed by application.");
			}
			else
			{
				_logger.LogError("Connection closed unexpectedly. Initiator: {0}, Reason: {1}", e.Initiator, e.ReplyText);

				this.RabbitMQConnectionStatus = EnumConnectionStatus.Error;
				while (true)
				{
					if (this.RabbitMQConnectionStatus == EnumConnectionStatus.Connected)
					{
						//ReadMessages(handleMessage).Wait();
						break;
					}
					RabbitMQConnectionStatus = initRabbitMQ(_rabbitMQService, _queueProperties.QueueName);
					Thread.Sleep(10000);
				}
			}
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			Task.Run(() => Init(stoppingToken));
		}
		public virtual async Task Init(CancellationToken stoppingToken)
		{
			while (true)
			{
				try
				{
					if (RabbitMQConnectionStatus == EnumConnectionStatus.Connected)
					{
						await ReadMessages(handleMessage);
						break;
					}
					RabbitMQConnectionStatus = initRabbitMQ(_rabbitMQService, _queueProperties.QueueName);
				}
				catch (Exception ex)
				{
					_logger.LogMsg(Messages.ErrException, ex);
				}
				Thread.Sleep(10000);
			}
		}
		public virtual async Task ReadMessages(Func<DeliveryObj, Task> handleMessageFunction)
		{
			var consumer = new AsyncEventingBasicConsumer(_model);
			consumer.Received += async (ch, e) =>
			{
				await Task.CompletedTask;

				DeliveryObj deliveryObj = new DeliveryObj()
				{
					DeliveryTag = e.DeliveryTag,
					QueueName = _queueProperties.QueueName,
					Exchange = _queueProperties.ExchangeName,
					RoutingKey = _queueProperties.RoutingKey,
					Data = e.Body.ToArray(),
				};
				deliveryObj.model = _model;

				await handleMessageFunction(deliveryObj);
			};
			_model.BasicConsume(_queueProperties.QueueName, false, consumer);
			await Task.CompletedTask;
		}

		public virtual async Task handleMessage(DeliveryObj deliveryMessage)
		{
			try
			{
				this._logger.LogInformation("Func Start");

				this.RemoveSuccessMessage(deliveryMessage);

				this._logger.LogInformation("Success, " + deliveryMessage.ToJson());
			}
			catch (Exception ex)
			{
				this._logger.LogMsg(Messages.ErrException.SetParameters(ex));
			}

		}

		public virtual void RetryMessage(DeliveryObj deliveryMessage)
		{
			string errMessage = "";
			string dataJson = "";
			var message = deliveryMessage.GetData<DevicePayloadMessage>(ref dataJson, ref errMessage);
			if (message == null) return;
			_logger.LogWarning($"RetryMessage, {message?.MessageId}");

			int retryCount = CachedFunc.IsCanRetry(message.MessageId, _queueProperties.TimeRetryInSeconds, _queueProperties.MaxRetryCount);
			if (retryCount >= 0)
			{
				_rabbitMQService.PushMessage(message, true, _queueProperties.GetRetryExchangeName(), _queueProperties.GetRetryName(), true);
				GaugeMetrics.TrackMetricsReceiveMessage(deliveryMessage.QueueName, false, true);
				_model.BasicAck(deliveryMessage.DeliveryTag, false);
			}
			else
			{
				// Stop retry
				GaugeMetrics.TrackMetricsReceiveMessage(deliveryMessage.QueueName, false, true, true);
			}
		}
		public virtual void RemoveSuccessMessage(DeliveryObj deliveryMessage)
		{
			_model.BasicAck(deliveryMessage.DeliveryTag, false);
			GaugeMetrics.TrackMetricsReceiveMessage(deliveryMessage.QueueName, true);
		}
		public virtual void RemoveErrorMessage(DeliveryObj deliveryMessage)
		{
			_model.BasicAck(deliveryMessage.DeliveryTag, false);
			GaugeMetrics.TrackMetricsReceiveMessage(deliveryMessage.QueueName, false, false);
		}
		public override void Dispose()
		{
			try
			{
				if (_model != null && _model.IsOpen)
					_model.Close();
				if (_connection != null && _connection.IsOpen)
					_connection.Close();
				base.Dispose();
			}
			catch (Exception ex)
			{
				_logger.LogMsg(Messages.ErrException, ex);
			}
		}
	}
}
