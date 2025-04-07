using Confluent.Kafka;
using ConnectionCommon.Connection;
using ConnectionCommon.MessageBroker;
using iSoft.Common.Models.ConfigModel.Subs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace iSoft.ConnectionCommon.MessageBroker
{
    public class KafkaConn : IConnection
  {

    public bool IsConnect { get; set; }
    private ILogger<KafkaConn> logger;
    #region Producer Fields
    string pubTopic;
    ProducerConfig proConfig;
    int proTimeout;
    #endregion
    #region Consumer Fields
    string subTopic;
    ConsumerConfig consumConfig;
    bool cancelledConsume;
    int consumTimeout = 3000;
    #endregion
    public Connection Connection { get; set; }

    public Task CheckConnectionLoop()
    {
      throw new NotImplementedException();
    }

    public Task<bool> Connect()
    {
      throw new NotImplementedException();
    }

    public Task<bool> Disconnect()
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    public async Task Init(Connection connection)
    {
      this.Connection = connection;
      try
      {


        this.Connection = connection;
        var Host = HttpUtility.UrlEncode(LoadConfigOrDefault("host", "dev.i-soft.com.vn"));
        var Port = HttpUtility.UrlEncode(LoadConfigOrDefault("port", "9092"));
        var GroupId = LoadConfigOrDefault("groupid", "test");
        pubTopic = LoadConfigOrDefault("pubtopic", "pubTopic");
        subTopic = LoadConfigOrDefault("subtopic", "pubTopic");
        consumTimeout = int.Parse(LoadConfigOrDefault("consumetimeout", "3000"));
        proTimeout = int.Parse(LoadConfigOrDefault("producertimeout", "1000"));
        proConfig = new ProducerConfig
        {
          BootstrapServers = $"{Host}:{Port}",
          EnableIdempotence = true,
          MessageTimeoutMs = proTimeout,
          Partitioner= Partitioner.Random
        };
        consumConfig = new ConsumerConfig
        {
          BootstrapServers = $"{Host}:{Port}",
          GroupId = GroupId,
          AutoOffsetReset = AutoOffsetReset.Earliest
        };

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

      }
      if (string.IsNullOrEmpty(ret))
      {
        ret = defaultValue;
      }
      return ret;
    }
    public async Task Init(Connection connection, ILoggerFactory loggerFactory, ServerConfigModel rabbitMQConfig)
    {
      this.logger = loggerFactory.CreateLogger<KafkaConn>();
      await Init(connection);
    }

    public Task<int[]> ReadAsync()
    {
      throw new NotImplementedException();
    }

    public Task<T> ReadAsync<T>() where T : class
    {
      throw new NotImplementedException();
    }

    public async Task SendAsync<T>(T data) where T : class
    {
      try
      {
        using (var producer = new ProducerBuilder<Null, string>(proConfig).Build())
        {
          var result = await producer.ProduceAsync(pubTopic, new Message<Null, string> { Value = "a log message" });
        }
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }

    public async Task StartRead(Func<BaseMessage, Task<bool>> callback, Action<Exception> callbackException)
    {
      using (var consumer = new ConsumerBuilder<Ignore, string>(consumConfig).Build())
      {
        consumer.Subscribe(subTopic);

        while (!cancelledConsume)
        {
          var consumeResult = consumer.Consume();

          // handle consumed message.
          //await callback(ReceivePayload.CreateReceivePayload(this.Connection, consumeResult.Message));
        }

        consumer.Close();
      }
    }

    public async Task StopRead()
    {
      cancelledConsume = true;
    }

    Task IConnection.CheckConnectionLoop()
    {
      throw new NotImplementedException();
    }

    Task<bool> IConnection.Connect()
    {
      throw new NotImplementedException();
    }

    Task<bool> IConnection.Disconnect()
    {
      throw new NotImplementedException();
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
