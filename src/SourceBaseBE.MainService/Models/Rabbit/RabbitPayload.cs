using iSoft.RabbitMQ.Payload;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SourceBaseBE.MainService.Models.Rabbit
{
	public class RabbitPayload : DevicePayloadMessage
	{
		[JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
		public new List<Dictionary<string, object>> Data { get; set; }
	}
}
