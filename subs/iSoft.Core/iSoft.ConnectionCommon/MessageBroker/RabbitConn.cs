using ConnectionCommon.Connection;
using iSoft.Common;
using iSoft.Common.Cached;
using iSoft.Common.Exceptions;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.ConnectionCommon;
using iSoft.ConnectionCommon.MessageBroker;
using iSoft.ConnectionCommon.MessageQueueNS;
using iSoft.ConnectionCommon.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;
using static iSoft.Common.Messages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConnectionCommon.MessageBroker
{
    public class RabbitConn : Connection.IConnection
  {
    #region Fields
    #region PubFields
    string pubExchangeName = "Demo Track device";
    string pubRoutingKey = "demo-routing";
    string pubQueueName = "Demo Queue";
    string pubExChangetype = "Direct";
    private IModel pubChannel;
    #endregion
    #region SubFields
    string subExchangeName = "Demo Track device";
    string subRoutingKey = "demo-routing";
    string subQueueName = "Demo Queue";
    string subExChangetype = "Direct";
    private IModel subChannel;
    static Dictionary<string, PairObj2<IModel, Dictionary<string, bool>>> dicChannel = new Dictionary<string, PairObj2<IModel, Dictionary<string, bool>>>();
    static List<RabbitMQ.Client.IConnection> listConnection = new List<RabbitMQ.Client.IConnection>();
    private static RabbitConn instance = null;
    private static object lockObject = new object();
    private RabbitConn() { }
    public static RabbitConn GetInstance()
    {
      lock (lockObject)
      {
        if (instance == null)
        {
          instance = new RabbitConn();
        }
      }
      return instance;
    }

    string comsumeTag;
    AsyncEventingBasicConsumer subConsumer;
    #endregion

    #endregion

    ConnectionFactory factory = new ConnectionFactory() { DispatchConsumersAsync = true, RequestedHeartbeat = TimeSpan.Zero };

    private Func<BaseMessage, Task<bool>> callback;
    private Func<PayloadMessage, Task> callback2;
    public bool IsConnect { get; set; }
    public Connection.Connection Connection { get; set; }
    private ILogger<RabbitConn> logger;

    public async Task<bool> Connect()
    {
      return true;
    }

    public Task<bool> Disconnect()
    {
      throw new NotImplementedException();
    }

    public async Task Init(Connection.Connection connection, ILoggerFactory loggerFactory, ServerConfigModel rabbitMQConfig)
    {
      try
      {
        this.logger = loggerFactory.CreateLogger<RabbitConn>();

        this.Connection = connection;
        var UserName = HttpUtility.UrlEncode(LoadConfigOrDefault("username", rabbitMQConfig.Username));
        var Password = HttpUtility.UrlEncode(LoadConfigOrDefault("password", rabbitMQConfig.Password));
        var Host = HttpUtility.UrlEncode(LoadConfigOrDefault("host", rabbitMQConfig.Address));
        var Port = HttpUtility.UrlEncode(LoadConfigOrDefault("port", rabbitMQConfig.Port.ToString()));
        pubExchangeName = LoadConfigOrDefault("pubexchangename", "demo-pub-exchange");
        pubRoutingKey = LoadConfigOrDefault("pubroutingkey", "demo-pub-routingkey");
        pubQueueName = LoadConfigOrDefault("pubqueuename", "demo-pub-queuename");
        pubExChangetype = LoadConfigOrDefault("pubexchangetype", "direct");
        subExchangeName = LoadConfigOrDefault("subexchangename", "demo-sub-exchange");
        subRoutingKey = LoadConfigOrDefault("subroutingkey", "demo-sub-routingkey");
        subQueueName = LoadConfigOrDefault("subqueuename", "demo-sub-queuename");
        subExChangetype = LoadConfigOrDefault("subexchangetype", "direct");
        factory.Uri = new Uri($"amqp://{UserName}:{Password}@{Host}:{Port}");
        factory.ClientProvidedName = this.Connection.DisplayName;
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }
    private string LoadConfigOrDefault(string proName, string defaultValue)
    {
      string ret = null;
      if (this.Connection.ConnectionConfigs != null)
      {
        ret = this.Connection.ConnectionConfigs.FirstOrDefault(s => s.Config.Name == $"{proName}")?.Value;
        if (string.IsNullOrEmpty(ret))
        {
          ret = defaultValue;
        }
      }
      return ret;
    }
    public Task<int[]> ReadAsync()
    {
      throw new NotImplementedException();
    }

    public Task<T> ReadAsync<T>() where T : class
    {
      throw new Exception();
    }

    private async void Consumer_Received(object? sender, BasicDeliverEventArgs e)
    {
      //Message errMessage = null;
      //string funcName = "Consumer_Received";
      //string msg = "";
      //try
      //{
      //	msg = Encoding.UTF8.GetString(e.Body.ToArray());
      //	var payload = JsonExtensionUtil.FromJson<MessageBrokerPayload>(msg);
      //	var result = await callback(DevicePayloadMessage.Create(this.Connection.Id, payload));
      //	if (subChannel.IsClosed)
      //	{
      //		await StartRead(this.callback);
      //		return;
      //	}
      //	if (result)
      //	{
      //		subChannel.BasicAck(e.DeliveryTag, false);
      //	}
      //	else
      //	{
      //		subChannel.BasicNack(e.DeliveryTag, false, true);
      //	}
      //}
      //catch (JsonReaderException ex)
      //{
      //	errMessage = Messages.ErrBaseException.SetParameters(msg, ex);
      //	this.logger.LogMsg(errMessage);
      //	subChannel.BasicReject(e.DeliveryTag, false);
      //}
      //catch (DBException ex)
      //{
      //	errMessage = Messages.ErrDBException.SetParameters(msg, ex);
      //	this.logger.LogMsg(errMessage);
      //	subChannel.BasicNack(e.DeliveryTag, false, true);
      //}
      //catch (BaseException ex)
      //{
      //	errMessage = Messages.ErrBaseException.SetParameters(msg, ex);
      //	this.logger.LogMsg(errMessage);
      //	subChannel.BasicNack(e.DeliveryTag, false, true);
      //}
      //catch (Exception ex)
      //{
      //	errMessage = Messages.ErrException.SetParameters(msg, ex);
      //	this.logger.LogMsg(errMessage);
      //	subChannel.BasicNack(e.DeliveryTag, false, true);
      //}
    }

    private async void Consumer_Received2(object? sender, BasicDeliverEventArgs e)
    {
      try
      {
        PayloadMessage payloadMessage = new PayloadMessage()
        {
          DeliveryTag = e.DeliveryTag,
          Exchange = e.Exchange,
          RoutingKey = e.RoutingKey,
          Data = e.Body.ToArray(),
        };
        await callback2(payloadMessage);
      }
      catch (JsonReaderException ex)
      {
        this.logger.LogMsg(Messages.ErrBaseException.SetParameters(ex));
      }
      catch (DBException ex)
      {
        this.logger.LogMsg(Messages.ErrDBException.SetParameters(ex));
      }
      catch (BaseException ex)
      {
        this.logger.LogMsg(Messages.ErrBaseException.SetParameters(ex));
      }
      catch (Exception ex)
      {
        this.logger.LogMsg(Messages.ErrException.SetParameters(ex));
      }
    }
    public void Dispose()
    {

    }
    public async Task SendAsync<T>(T data) where T : class
    {
      var dat = data as MessageBrokerPayload;
      if (dat == null) return;
      RabbitMQ.Client.IConnection cnn = factory.CreateConnection();
      this.pubChannel = cnn.CreateModel();
      pubChannel.ExchangeDeclare(pubExchangeName, pubExChangetype);
      var message = Encoding.UTF8.GetBytes(JsonExtensionUtil.ToJson(dat));
      pubChannel.BasicPublish(pubExchangeName, pubRoutingKey, null, message);
      pubChannel.Close();
      cnn.Close();
    }

    public async Task StartRead(Func<BaseMessage, Task<bool>> callback, Action<Exception> callbackException)
    {

      this.callback = callback;
      if (this.callback == null)
        throw new Exception("callback not right type of handler");
      RabbitMQ.Client.IConnection cnn = factory.CreateConnection();
      subChannel = cnn.CreateModel();
      subChannel.ExchangeDeclare(subExchangeName, subExChangetype);
      subChannel.QueueDeclare(subQueueName, false, false, false, null);
      subChannel.QueueBind(subQueueName, subExchangeName, subRoutingKey, null);
      subChannel.BasicQos(0, 8, false);
      subConsumer = new AsyncEventingBasicConsumer(subChannel);
      subConsumer.Received += SubConsumer_Received; ;
      comsumeTag = subChannel.BasicConsume(subQueueName, false, subConsumer);
    }

    public IModel GetChannel(QueueProperties prop)
    {
      lock (dicChannel)
      {
        IModel channel;
        if (!dicChannel.ContainsKey(prop.ExchangeName))
        {
          var connection = factory.CreateConnection();
          listConnection.Add(connection);
          channel = connection.CreateModel();
          channel.ExchangeDeclare(exchange: prop.ExchangeName, type: ExchangeType.Fanout, prop.Durable);
          channel.BasicQos(0, prop.RabbitPrefetchCount, false);
          dicChannel.Add(
            prop.ExchangeName,
            new PairObj2<IModel, Dictionary<string, bool>>(
                channel,
                new Dictionary<string, bool>() {
                    { prop.QueueName, true }
                  }
            )
          );
          ProcessOneProp(channel, prop);
        }
        else
        {
          channel = dicChannel[prop.ExchangeName].obj1;
        }

        if (!dicChannel[prop.ExchangeName].obj2.ContainsKey(prop.QueueName))
        {
          ProcessOneProp(channel, prop);

          dicChannel[prop.ExchangeName].obj2.Add(prop.QueueName, true);

          return channel;
        }
        else
        {
          return channel;
        }
      }
    }

    public IModel GetChannel(List<QueueProperties> listProp)
    {
      lock (dicChannel)
      {
        if (!dicChannel.ContainsKey(listProp[0].ExchangeName))
        {
          var connection = factory.CreateConnection();
          listConnection.Add(connection);
          IModel channel = connection.CreateModel();
          channel.ExchangeDeclare(exchange: listProp[0].ExchangeName, type: ExchangeType.Fanout, listProp[0].Durable);
          channel.BasicQos(0, listProp[0].RabbitPrefetchCount, false);
          Dictionary<string, bool> dicQueueName = new Dictionary<string, bool>();
          foreach (var prop in listProp)
          {
            ProcessOneProp(channel, prop);
            dicQueueName.Add(prop.QueueName, true);
          }

          dicChannel.Add(listProp[0].ExchangeName, new PairObj2<IModel, Dictionary<string, bool>>(channel, dicQueueName));
          return channel;
        }
        else
        {
          IModel channel = dicChannel[listProp[0].ExchangeName].obj1;
          return channel;
        }
      }
    }

    private void ProcessOneProp(IModel channel, QueueProperties prop)
    {
      if (prop.TimeRetryInSeconds != -1)
      {
        string retryQueueName = prop.GetRetryName();
        string retryExchange = prop.GetRetryExchangeName();

        // Declare the original queue
        channel.QueueDeclare(queue: prop.QueueName,
                            durable: prop.Durable,
                            exclusive: false,
                            autoDelete: false,
                            arguments: new Dictionary<string, object>
                                      {
                                              { "x-max-length", prop.MaxMessageCount },
                                              //{ "x-dead-letter-exchange", retryExchange },
                                      });

        // Declare the Exchange by Myself
        if (prop.QueueName != prop.ExchangeName)
        {
          channel.ExchangeDeclare(exchange: prop.QueueName, type: ExchangeType.Direct, prop.Durable);
          channel.QueueBind(queue: prop.QueueName, exchange: prop.QueueName, routingKey: prop.RoutingKey, null);
        }

        // Declare the retry queue
        channel.ExchangeDeclare(exchange: retryExchange, type: ExchangeType.Direct, prop.Durable);
        channel.QueueDeclare(queue: retryQueueName,
                            durable: prop.Durable,
                            exclusive: false,
                            autoDelete: false,
                            arguments: new Dictionary<string, object>
                                      {
                                              { "x-max-length", prop.MaxMessageCount },
                                              { "x-dead-letter-exchange", prop.QueueName },
                                              { "x-dead-letter-routing-key", prop.RoutingKey },
                                              { "x-message-ttl", 1000 * prop.TimeRetryInSeconds },
                                      });
        channel.QueueBind(queue: prop.QueueName, exchange: prop.ExchangeName, routingKey: prop.RoutingKey, null);
        channel.QueueBind(queue: retryQueueName, exchange: retryExchange, routingKey: retryQueueName, null);
      }
      else
      {
        channel.QueueDeclare(queue: prop.QueueName,
                            durable: prop.Durable,
                            exclusive: false,
                            autoDelete: false,
                            arguments: new Dictionary<string, object>
                                      {
                                              { "x-max-length", prop.MaxMessageCount },
                                              { "x-message-ttl", 1000 * prop.ExpiredTimeInSeconds },
                                      });
        channel.QueueBind(queue: prop.QueueName, exchange: prop.ExchangeName, routingKey: prop.RoutingKey, null);

        // Declare the Exchange by Myself
        if (prop.QueueName != prop.ExchangeName)
        {
          channel.ExchangeDeclare(exchange: prop.QueueName, type: ExchangeType.Direct, prop.Durable);
          channel.QueueBind(queue: prop.QueueName, exchange: prop.QueueName, routingKey: prop.RoutingKey, null);
        }
      }
    }

    public void ClearChannel()
    {
      foreach (var keyVal in dicChannel)
      {
        try
        {
          keyVal.Value.obj1.Close();
        }
        catch { }
      }
      dicChannel.Clear();
    }

    public void ClearConnection()
    {
      foreach (var conn in listConnection)
      {
        try
        {
          conn.Close();
        }
        catch { }
      }
      listConnection.Clear();
    }

    private async Task SubConsumer_Received(object sender, BasicDeliverEventArgs @event)
    {
      Consumer_Received(sender, @event);
    }

    private async Task SubConsumer_Received2(object sender, BasicDeliverEventArgs @event)
    {
      Consumer_Received2(sender, @event);
    }

    public async Task StopRead()
    {
      if (subChannel == null) return;
      subChannel.BasicCancel(comsumeTag);
      subChannel.Close();
    }

    public async Task CheckConnectionLoop()
    {
    }

    public Task<int[]> ReadDevice(string deviceId, int quantity = 1)
    {
      throw new NotImplementedException();
    }

    public Task<T> ReadDevice<T>(string deviceId, int quantity = 1)
    {
      throw new NotImplementedException();
    }
  }

}
