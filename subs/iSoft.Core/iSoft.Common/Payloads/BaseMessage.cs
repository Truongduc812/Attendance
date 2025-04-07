using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Payloads
{
	public class BaseMessage
	{
		//[JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
		[NotNull]
		public string MessageId { get; set; } = "";
	}
}
