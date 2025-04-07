

using iSoft.Common.ExtensionMethods;
using iSoft.Common.MetricsNS;
using iSoft.Common.Models;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Common.Services;
using iSoft.InfluxDB.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using System.Web;
using static iSoft.Common.Messages;

namespace iSoft.RabbitMQ.Services
{
  public class RabbitMQService
  {
    private ServerConfigModel _serverConfig;
    public static string GetConnectionString(ServerConfigModel config)
    {
      var UserName = HttpUtility.UrlEncode(config.Username);
      var Password = HttpUtility.UrlEncode(config.Password);
      var Host = HttpUtility.UrlEncode(config.Address);
      var Port = HttpUtility.UrlEncode(config.Port.ToString());

      string rs = $"amqp://{UserName}:{Password}@{Host}:{Port}";
      return rs;
    }
    public RabbitMQService(ServerConfigModel serverConfig)
    {
      _serverConfig = serverConfig;
    }
    public IConnection CreateChannel()
    {
      ConnectionFactory connection = new ConnectionFactory();
      connection.DispatchConsumersAsync = true;
      connection.RequestedHeartbeat = TimeSpan.FromSeconds(0);

      //var UserName = HttpUtility.UrlEncode(_serverConfig.Username);
      //var Password = HttpUtility.UrlEncode(_serverConfig.Password);
      //var Host = HttpUtility.UrlEncode(_serverConfig.Address);
      //var Port = HttpUtility.UrlEncode(_serverConfig.Port.ToString());
      connection.Uri = new Uri(GetConnectionString(_serverConfig));
      //connection.ClientProvidedName = this.Connection.DisplayName;

      var channel = connection.CreateConnection();
      return channel;
    }
    //public IModel GetChannel(List<QueueProperties> listProp)
    //{
    //  lock (dicChannel)
    //  {
    //    if (!dicChannel.ContainsKey(listProp[0].ExchangeName))
    //    {
    //      var connection = factory.CreateConnection();
    //      listConnection.Add(connection);
    //      IModel model = connection.CreateModel();
    //      model.ExchangeDeclare(exchange: listProp[0].ExchangeName, type: ExchangeType.Fanout, listProp[0].Durable);
    //      model.BasicQos(0, listProp[0].RabbitPrefetchCount, false);
    //      Dictionary<string, bool> dicQueueName = new Dictionary<string, bool>();
    //      foreach (var prop in listProp)
    //      {
    //        ProcessOneProp(model, prop);
    //        dicQueueName.Add(prop.QueueName, true);
    //      }

    //      dicChannel.Add(listProp[0].ExchangeName, new PairObj2<IModel, Dictionary<string, bool>>(model, dicQueueName));
    //      return model;
    //    }
    //    else
    //    {
    //      IModel model = dicChannel[listProp[0].ExchangeName].obj1;
    //      return model;
    //    }
    //  }
    //}
    public void ProcessOneProp(IModel channel, QueueProperties prop)
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
          channel.ExchangeDeclare(exchange: prop.QueueName, type: ExchangeType.Direct, prop.Durable, false);
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
          channel.ExchangeDeclare(exchange: prop.QueueName, type: ExchangeType.Direct, prop.Durable, false);
          channel.QueueBind(queue: prop.QueueName, exchange: prop.QueueName, routingKey: prop.RoutingKey, null);
        }
      }
    }
    public void PushMessage(object message, bool persistent, string exchangeNameOrQueueName, string routingKey = "", bool isRetry = false)
    {
      try
      {
        using var connection = CreateChannel();
        using var model = connection.CreateModel();
        var body = Encoding.UTF8.GetBytes(message.ToJson());
        var properties = model.CreateBasicProperties();
        properties.Persistent = persistent;
        model.BasicPublish(exchangeNameOrQueueName,
                             routingKey,
                             basicProperties: properties,
                             body: body);
        if (!isRetry)
        {
          //_influxDBService.TrackMetricsPushMessage(exchangeNameOrQueueName, true);
          GaugeMetrics.TrackMetricsPushMessage(exchangeNameOrQueueName, true);
        }
      }
      catch (Exception ex)
      {
        if (!isRetry)
        {
          //_influxDBService.TrackMetricsPushMessage(exchangeNameOrQueueName, false);
          GaugeMetrics.TrackMetricsPushMessage(exchangeNameOrQueueName, false);
        }
        throw ex;
      }
    }



  }
}