using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using ConnectionCommon.Connection;
using iSoft.Common.Errors;
using UDF.TrackDeviceService.Services.Interfaces;
using System.Threading.Tasks;
using iSoft.TrackDeviceService.Services.Interfaces;
using ConnectionCommon.MessageBroker;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.EtherCAT;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.MCProtocol;
using iSoft.ConnectionCommon.IndustrialConn.TcpGroup.Modbus;
using iSoft.ConnectionCommon.MessageBroker;
using iSoft.ConnectionCommon.SocketIOs;
using System.Text;
using iSoft.ConnectionCommon.Model;
using Common.Utils;
using iSoft.Common.Util;
using iSoft.ConnectionCommon.MessageQueueNS;
using System.Linq;
using iSoft.ConnectionCommon.iSoft.ConnectionCommon.SocketIOs;

namespace iSoft.TrackDeviceService.Services
{
	public class OutDataService : BaseService, IOutputService
	{
		private Microsoft.Extensions.Logging.ILogger _logger;
		public List<IConnection> Connections { get; set; } = new List<IConnection>();
		public event EventHandler<Error> OnErrorOccure = null;
		private readonly ILoggerFactory loggerFactory;
		private readonly Microsoft.AspNetCore.Hosting.IApplicationLifetime appLifetime;
		private readonly StoreHolderService storeHolderService;
		private readonly RealtimeService realtimeService;
		private readonly MessageQueue messageQueueService;
		private readonly IMQService mQService;
		private bool isRabbitInitDone = false;
		//private readonly RabbitConn
		public OutDataService(ILoggerFactory loggerFactory,
			Microsoft.AspNetCore.Hosting.IApplicationLifetime appLifetime,
			StoreHolderService storeHolderService,
			RealtimeService realtimeService,
			MessageQueue messageQueueService,
			IMQService mQService
			)
		{
			try
			{
				this.loggerFactory = loggerFactory;
				this._logger = loggerFactory.CreateLogger<ReadDeviceService>();
				this.appLifetime = appLifetime;
				this.storeHolderService = storeHolderService;
				this.storeHolderService.OnStoreDataChange += StoreHolderService_OnStoreDataChange;
				this.realtimeService = realtimeService;
				this.messageQueueService = messageQueueService;
				this.mQService = mQService;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}

		private async Task StoreHolderService_OnStoreDataChange(ulong connId, iMagPayloadMessage data)
		{
			if (data == null || !isRabbitInitDone) return;
			Console.WriteLine($"StoreDataChange on ConnId: {connId} with data: {data.ToString()}");
			realtimeService?.RaiseData(data.ToJson(), (int)connId);
			mQService?.RaiseData(data.ToJson(), (int)connId);
			foreach (var topic in messageQueueService.GetPubTopics())
				messageQueueService?.PushMessageAsync(topic, data.ToJson());
		}

		public IConnService SetConnections(List<IConnection> connections)
		{
			this.Connections = connections;
			return this;
		}

		public Task Run()
		{
			throw new NotImplementedException();
		}

		public IConnService SetLogger(ILogger loger)
		{
			this._logger = loger;
			return this;
		}

		public async Task InitAllConnections(List<Connection> conns)
		{
			foreach (var conn in conns)
				AppendAndStartConnection(conn);
		}

		public async Task InitAllConnections(List<IConnection> conns)
		{
			this.Connections = conns;
		}

		public IConnection GetConnectionService(Connection connection)
		{
			switch (connection.ConnType)
			{
				case eConnType.ADS:
					return new EtherCAT()
					{
						Connection = connection,
					};
					break;
				case eConnType.ModbusTCP:
					return new ModbusConn2()
					{
						Connection = connection,
					};
				case eConnType.ModbusRTU:
					return new ModbusConn2()
					{
						Connection = connection,
					};
				case eConnType.MCProtocol:
					return new MCProtocolConn2()
					{
						Connection = connection,
					};
				case eConnType.SLMP:
					return new MCProtocolConn2()
					{
						Connection = connection,
					};
					break;
				case eConnType.Excel:
					break;
				case eConnType.TCP_IP:
					break;
				case eConnType.Rabbit:
					return RabbitConn.GetInstance();
					break;
				case eConnType.Kafka:
					return new KafkaConn()
					{
						Connection = connection,
					};
					break;
				case eConnType.OPC_UA:
					break;
				case eConnType.SerialText:
					break;
				case eConnType.SocketIO:
					return new SocketIoClient(loggerFactory.CreateLogger<SocketIoClient>())
					{
						Connection = connection,
					};
				default:
					return null;
			}
			return null;
		}

		public Task StartAllConnections()
		{
			throw new NotImplementedException();
		}

		public Task StopAll()
		{
			throw new NotImplementedException();
		}

		public void HandleErrorConnection(Exception error)
		{
			throw new NotImplementedException();
		}

		public IConnService AppendConnection(IConnection connection)
		{
			throw new NotImplementedException();
		}

		public Task<IConnService> AppendAndStartConnection(IConnection connection)
		{
			throw new NotImplementedException();
		}

		public async Task<IConnService> AppendAndStartConnection(Connection connection)
		{
			await Task.Run(async () =>
			{
				if (connection.ConnType == eConnType.SocketIO)
				{
					await realtimeService.Init(connection);
					return;
				}
				else if (connection.ConnType == eConnType.MQTT)
				{
					await mQService.Init(connection,this.loggerFactory);
					return;
				}
				else if (connection.ConnType == eConnType.Rabbit)
				{
					var messageQueueConfigs = JsonExtensionUtil.FromJson<List<QueueProperties>>(ConfigUtil.GetAppSetting<object>("AppSettings:MessageQueueConfig").ToString());
					var host = connection.ConnectionConfigs?.Where(x => x.Config.Name == "host")?.FirstOrDefault()?.Value != null ? connection.ConnectionConfigs?.Where(x => x.Config.Name == "host")?.FirstOrDefault()?.Value : "dev.i-soft.com.vn";
					var port = connection.ConnectionConfigs?.Where(x => x.Config.Name == "port")?.FirstOrDefault()?.Value != null ? connection.ConnectionConfigs?.Where(x => x.Config.Name == "port")?.FirstOrDefault()?.Value : "5672";
					var user = connection.ConnectionConfigs?.Where(x => x.Config.Name == "user")?.FirstOrDefault()?.Value != null ? connection.ConnectionConfigs?.Where(x => x.Config.Name == "user")?.FirstOrDefault()?.Value : "guest";
					var pass = connection.ConnectionConfigs?.Where(x => x.Config.Name == "password")?.FirstOrDefault()?.Value != null ? connection.ConnectionConfigs?.Where(x => x.Config.Name == "password")?.FirstOrDefault()?.Value : "iSoft@123";
					await messageQueueService.Init(messageQueueConfigs, new Common.Models.RemoteConfigModels.Subs.RabbitMQConfigModel(host, int.Parse(port), user, pass));
					isRabbitInitDone = true;
				}
				var iConnection = GetConnectionService(connection);
				Connections.Add(iConnection);
				await iConnection.Init(connection);
				await iConnection.Connect();
			});
			return this;
		}
	}
}
