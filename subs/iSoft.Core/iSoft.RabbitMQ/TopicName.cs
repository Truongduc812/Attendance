using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.RabbitMQ
{
	public class TopicName
	{
		public static string TraceDataTopic = "TraceDataTopic";
		public static string SearchDataTopic = "SearchDataTopic";
		public static string RealtimeDataTopic = "RealtimeDataTopic";
		public static string TrackDeviceTopic = "TrackDeviceTopic";
    public static string SyncDataTopic = "SyncDataTopic";
    public static string UpdateWorkingDayTopic = "UpdateWorkingDayTopic";
  }
  public class ExchangeName
  {
    public static string SourceBaseBEEnvEx = "SourceBaseBEEnvEx";
    public static string SourceBaseBESyncDataEx = "SourceBaseBESyncDataEx";
  }
}
