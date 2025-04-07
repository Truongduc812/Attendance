using ConnectionCommon.Connection;
using ConnectionCommon.MessageBroker;
using iSoft.Common;
using iSoft.Common.Cached;
using iSoft.Common.Exceptions;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.ConnectionCommon.MessageBroker;
using iSoft.ConnectionCommon.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static iSoft.Common.Messages;

namespace iSoft.ConnectionCommon.MessageQueueNS
{
    public class MessageQueue : IDisposable
  {
    private static MemCached cache = new MemCached(60);
    private static RabbitConn rabbit = RabbitConn.GetInstance();
    private static Dictionary<string, QueueProperties> dicQueueProperties = new Dictionary<string, QueueProperties>();
    private static Dictionary<string, List<QueueProperties>> dicQueuePropertiesEx = new Dictionary<string, List<QueueProperties>>();

    public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
      {
        builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Warning)
              .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information)
              .AddConsole()
              .AddDebug();
      }
    );

    public static List<ConnectionConfig> GetConfig(ServerConfigModel rabbitMQConfig)
    {
      return new List<ConnectionConfig>()
      {
        new ConnectionConfig()
        {
          Config= new Config() {
            Name="username"
          },
          Value=rabbitMQConfig.Username
        },
        new ConnectionConfig()
        {
          Config= new Config() {
            Name="password"
          },
          Value=rabbitMQConfig.Password
        },
        new ConnectionConfig()
        {
          Config= new Config() {
            Name="host"
          },
          Value=rabbitMQConfig.Address
        },
        new ConnectionConfig()
        {
          Config= new Config() {
            Name="port"
          },
          Value=rabbitMQConfig.Port.ToString()
        },
      };
    }

    public static List<ConnectionConfig> GetConfig(string topicName)
    {
      return new List<ConnectionConfig>()
      {
        new ConnectionConfig()
        {
          Config= new Config() {
            Name="pubqueuename"
          },
          Value=topicName
        },
        new ConnectionConfig()
        {
          Config= new Config() {
            Name="username"
          },
          Value="guest"
        },
        new ConnectionConfig()
        {
          Config= new Config() {
            Name="password"
          },
          Value="iSoft@123"
        },
        new ConnectionConfig()
        {
          Config= new Config() {
            Name="host"
          },
          Value="dev.i-soft.com.vn"
        },
        new ConnectionConfig()
        {
          Config= new Config() {
            Name="port"
          },
          Value="5672"
        },
      };
    }

    public static async Task Init(Dictionary<string, QueueProperties> dic, ServerConfigModel rabbitMQConfig)
    {
      rabbit.ClearChannel();
      rabbit.ClearConnection();
      var connection = new Connection();
      connection.ConnectionConfigs = GetConfig(rabbitMQConfig);
      await rabbit.Init(connection, loggerFactory, rabbitMQConfig);
      dicQueueProperties.Clear();
      dicQueuePropertiesEx.Clear();
      if (dic != null)
      {
        foreach (var kvp in dic)
        {
          if (!dicQueueProperties.ContainsKey(kvp.Key) && kvp.Key != null && kvp.Value != null)
          {
            dicQueueProperties.Add(kvp.Key, kvp.Value);
          }
        }
        foreach (var keyVal in dicQueueProperties)
        {
          if (dicQueuePropertiesEx.ContainsKey(keyVal.Value.ExchangeName))
          {
            var listQueueProp = dicQueuePropertiesEx[keyVal.Value.ExchangeName];
            listQueueProp.Add(keyVal.Value);
          }
          else
          {
            var listQueueProp = new List<QueueProperties>();
            listQueueProp.Add(keyVal.Value);
            dicQueuePropertiesEx.Add(keyVal.Value.ExchangeName, listQueueProp);
          }

          if (keyVal.Value.TimeRetryInSeconds != -1)
          {
            if (dicQueuePropertiesEx.ContainsKey(keyVal.Value.GetRetryExchangeName()))
            {
              var listQueueProp = dicQueuePropertiesEx[keyVal.Value.GetRetryExchangeName()];
              listQueueProp.Add(keyVal.Value);
            }
            else
            {
              var listQueueProp = new List<QueueProperties>();
              listQueueProp.Add(keyVal.Value);
              dicQueuePropertiesEx.Add(keyVal.Value.GetRetryExchangeName(), listQueueProp);
            }
          }
        }
      }
    }

    public static QueueProperties GetQueueProperties(string queueName)
    {
      if (dicQueueProperties.ContainsKey(queueName))
      {
        return dicQueueProperties[queueName];
      }
      throw new NotImplementedException("NotImplementedException [GetQueueProperties()]");
    }

    public static List<QueueProperties> GetQueuePropertiesEx(string exchangeName)
    {
      if (dicQueuePropertiesEx.ContainsKey(exchangeName))
      {
        return dicQueuePropertiesEx[exchangeName];
      }
      throw new NotImplementedException("NotImplementedException [GetQueuePropertiesEx()]");
    }

    public static async Task PushMessageAsync(string exchangeNameOrQueueName, bool persistent, object message, string routingKey = "")
    {
      var logger = loggerFactory.CreateLogger<MessageQueue>();
      try
      {
        var listQueuePropertyies = GetQueuePropertiesEx(exchangeNameOrQueueName);
        var channel = RabbitConn.GetInstance().GetChannel(listQueuePropertyies);
        var body = Encoding.UTF8.GetBytes(message.ToJson());
        var properties = channel.CreateBasicProperties();
        properties.Persistent = persistent;
        channel.BasicPublish(exchange: exchangeNameOrQueueName, routingKey: routingKey, basicProperties: properties, body: body);
      }
      catch (Exception ex)
      {
        logger.LogMsg(ErrException.SetParameters(ex));
        throw ex;
      }
    }
    public static async Task<string> Subscribe(
      string queueName,
      Func<PayloadMessage, Task> handleMessageFunction,
      bool isAutoAck)
    {
      try
      {
        var channel = RabbitConn.GetInstance().GetChannel(GetQueueProperties(queueName));

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (model, e) =>
        {
          var exchange = e.Exchange;
          var routingKey = e.RoutingKey;
          PayloadMessage payloadMessage = new PayloadMessage()
          {
            DeliveryTag = e.DeliveryTag,
            QueueName = queueName,
            Exchange = exchange,
            RoutingKey = routingKey,
            Data = e.Body.ToArray(),
          };
          payloadMessage.channel = channel;
          await handleMessageFunction(payloadMessage);
        };
        string tag = channel.BasicConsume(queue: queueName, autoAck: isAutoAck, consumer: consumer);

        return tag;
      }
      catch (Exception ex)
      {
        throw new BaseException(ex);
      }
    }
    public static void DoNothingMessage(PayloadMessage payload)
    {
    }
    public static void AckMessage(PayloadMessage payload)
    {
      payload.channel.BasicAck(payload.DeliveryTag, false);
    }
    public static async Task RetryMessage(string messageId, PayloadMessage payload, DevicePayloadMessage message)
    {
      var queueProperties = GetQueueProperties(payload.QueueName);
      if (cache.IsCanRetry(messageId.ToString(), queueProperties.MaxRetryCount * queueProperties.TimeRetryInSeconds, queueProperties.MaxRetryCount))
      {
        await MessageQueue.PushMessageAsync(queueProperties.GetRetryExchangeName(), true, message, queueProperties.GetRetryName());
        DeleteMessage(payload);
      }
      else
      {
        if (queueProperties.Deletable)
        {
          DeleteMessage(payload);
        }
        else
        {
          DoNothingMessage(payload);
        }
      }
    }
    public static void DeleteMessage(PayloadMessage payload)
    {
      payload.channel.BasicAck(payload.DeliveryTag, false);
    }

    public void Dispose()
    {
      cache.Dispose();
      //rabbit.Dispose();
    }
  }
}
