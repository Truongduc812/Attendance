using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.ConnectionCommon.MessageBroker
{
	public class BaseMessage
	{
		[JsonProperty("msgId", NullValueHandling = NullValueHandling.Ignore)]
		[NotNull]
		public string MessageId { get; set; } = "";
	}
}
