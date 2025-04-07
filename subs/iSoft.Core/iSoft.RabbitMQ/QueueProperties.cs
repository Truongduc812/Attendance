using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.RabbitMQ
{
  public class QueueProperties
  {
    public string QueueName { get; set; }
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
    public long MaxMessageCount { get; set; }
    public int ExpiredTimeInSeconds { get; set; }
    public bool Durable { get; set; }
    public int TimeRetryInSeconds { get; set; }
    public int MaxRetryCount { get; set; }
    public ushort RabbitPrefetchCount { get; set; }
    public bool Deletable { get; set; }
    public QueueProperties(string queueName, string exchangeName)
    {
      QueueName = queueName;
      ExchangeName = exchangeName;
      RoutingKey = queueName;
    }
    public QueueProperties SetExpiredQueue(int expiredTimeInSeconds = 30, ushort rabbitPrefetchCount = 8, bool durable = false, long maxMessageCount = 1000000)
    {
      TimeRetryInSeconds = -1;
      MaxRetryCount = -1;
      ExpiredTimeInSeconds = expiredTimeInSeconds;
      Durable = durable;
      RabbitPrefetchCount = rabbitPrefetchCount;
      MaxMessageCount = maxMessageCount;
      return this;
    }

    public QueueProperties SetRetryQueue(int timeRetryInSeconds = 300, int maxRetryCount = 50, ushort rabbitPrefetchCount = 8, bool durable = true, long maxMessageCount = 1000000)
    {
      TimeRetryInSeconds = timeRetryInSeconds;
      MaxRetryCount = maxRetryCount;
      ExpiredTimeInSeconds = -1;
      Durable = durable;
      RabbitPrefetchCount = rabbitPrefetchCount;
      MaxMessageCount = maxMessageCount;
      return this;
    }

    public QueueProperties SetDeleteMessage(bool deletable)
    {
      Deletable = deletable;
      return this;
    }

    public string GetRetryName()
    {
      return this.QueueName + "_retry";
    }

    public string GetRetryExchangeName()
    {
      return this.QueueName + "_retry_exchange";
    }
  }
}
