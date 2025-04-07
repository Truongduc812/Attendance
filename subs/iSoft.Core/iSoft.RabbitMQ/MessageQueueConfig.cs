
namespace iSoft.RabbitMQ
{
  public class MessageQueueConfig
  {
    private static Dictionary<string, QueueProperties> dic = new Dictionary<string, QueueProperties>();
    public static Dictionary<string, QueueProperties> GetQueueConfig()
    {
      lock (dic)
      {
        if (!dic.ContainsKey(TopicName.TraceDataTopic))
        {
          dic.Add(TopicName.TraceDataTopic,
                  new QueueProperties(TopicName.TraceDataTopic, ExchangeName.SourceBaseBEEnvEx).SetRetryQueue(300, 50, 8, true));
        }
        if (!dic.ContainsKey(TopicName.SearchDataTopic))
        {
          dic.Add(TopicName.SearchDataTopic,
            new QueueProperties(TopicName.SearchDataTopic, ExchangeName.SourceBaseBEEnvEx).SetRetryQueue(300, 50, 8, true));
        }
        if (!dic.ContainsKey(TopicName.RealtimeDataTopic))
        {
          dic.Add(TopicName.RealtimeDataTopic,
            new QueueProperties(TopicName.RealtimeDataTopic, ExchangeName.SourceBaseBEEnvEx).SetExpiredQueue(2, 8, true));
        }
        if (!dic.ContainsKey(TopicName.UpdateWorkingDayTopic))
        {
          dic.Add(TopicName.UpdateWorkingDayTopic,
            new QueueProperties(TopicName.UpdateWorkingDayTopic, ExchangeName.SourceBaseBEEnvEx).SetRetryQueue(300, 50, 8, true));
        }
      }
      return dic;
    }

    private static Dictionary<string, QueueProperties> dicQueueProp = GetQueueConfig();
    public static QueueProperties GetQueueProperties(string topicName)
    {
      if (dicQueueProp.ContainsKey(topicName))
      {
        return dicQueueProp[topicName];
      }
      throw new NotImplementedException("[GetQueueProperties()]");
    }

    private static Dictionary<string, List<QueueProperties>> dicQueuePropExchange = null;
    public static List<QueueProperties> GetQueuePropertiesExchange(string exchangeName)
    {
      if (dicQueuePropExchange == null)
      {
        dicQueuePropExchange = new Dictionary<string, List<QueueProperties>>();
        foreach (var keyVal in MessageQueueConfig.dicQueueProp)
        {
          if (dicQueuePropExchange.ContainsKey(keyVal.Value.ExchangeName))
          {
            var listQueueProp = dicQueuePropExchange[keyVal.Value.ExchangeName];
            listQueueProp.Add(keyVal.Value);
          }
          else
          {
            var listQueueProp = new List<QueueProperties>();
            listQueueProp.Add(keyVal.Value);
            dicQueuePropExchange.Add(keyVal.Value.ExchangeName, listQueueProp);
          }

          if (keyVal.Value.TimeRetryInSeconds != -1)
          {
            if (dicQueuePropExchange.ContainsKey(keyVal.Value.GetRetryExchangeName()))
            {
              var listQueueProp = dicQueuePropExchange[keyVal.Value.GetRetryExchangeName()];
              listQueueProp.Add(keyVal.Value);
            }
            else
            {
              var listQueueProp = new List<QueueProperties>();
              listQueueProp.Add(keyVal.Value);
              dicQueuePropExchange.Add(keyVal.Value.GetRetryExchangeName(), listQueueProp);
            }
          }
        }
      }
      if (dicQueuePropExchange.ContainsKey(exchangeName))
      {
        return dicQueuePropExchange[exchangeName];
      }
      throw new NotImplementedException("[GetQueuePropertiesExchange()]");
    }
  }
}
