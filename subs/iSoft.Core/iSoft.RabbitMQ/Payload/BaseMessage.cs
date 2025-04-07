using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace iSoft.RabbitMQ.Payload
{
    public class BaseMessage
    {
        [JsonProperty("msgId", NullValueHandling = NullValueHandling.Ignore)]
        [NotNull]
        public string MessageId { get; set; } = "";
    }
}
