using iSoft.ConnectionCommon.MessageQueueNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.TrackDeviceService
{
	public class MessageQueueConfig
	{
		private static Dictionary<string, QueueProperties> dic = new Dictionary<string, QueueProperties>();
		public static Dictionary<string, QueueProperties> GetMessageQueueConfig()
		{
			return dic;
		}
		public static Dictionary<string, QueueProperties> SetQueueConfig(Dictionary<string, QueueProperties> keyValuePairs)
		{
			foreach (var obj in keyValuePairs)
			{
				if (!dic.ContainsKey(obj.Key))
				{
					dic.Add(obj.Key, obj.Value);
				}
				else
				{
					dic[obj.Key] = obj.Value;
				}
			}
			return dic;
		}
		public static QueueProperties GetQueueProperties(string topicName)
		{
			Dictionary<string, QueueProperties> dicQueueProp = GetMessageQueueConfig();
			if (dicQueueProp.ContainsKey(topicName))
			{
				return dicQueueProp[topicName];
			}
			return new QueueProperties(topicName).SetRetryQueue();
		}
	}
}
