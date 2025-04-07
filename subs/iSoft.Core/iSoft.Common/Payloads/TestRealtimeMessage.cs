using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Payloads
{
	public class TestRealtimeMessage : BaseMessage
  {
    [JsonProperty("ConnectionId", NullValueHandling = NullValueHandling.Ignore)]
    public long? ConnectionId { get; set; }


    [JsonProperty("DicVal", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, object> DicVal = new Dictionary<string, object>();


    [JsonProperty("ExecuteAt", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime ExecuteAt { get; set; }
  }
}
