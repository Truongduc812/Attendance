using iSoft.Common.ExtensionMethods;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace iSoft.RabbitMQ.Payload
{
    public class DevicePayloadMessage : BaseMessage
    {
        [JsonProperty("connId", NullValueHandling = NullValueHandling.Ignore)]
        public long ConnectionId { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public  Dictionary<string, object> Data { get; set; }

        [JsonProperty("exeAt", NullValueHandling = NullValueHandling.Ignore)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.fff}")]
        public DateTime ExecuteAt { get; set; }
        public override string ToString()
        {
            //string arrEnvStr = string.Join(", ", Data.ConvertAll(x => x.ToString()).ToArray());
            return $"{{MessageId: {MessageId}, ConnectionId: {ConnectionId}, ExecuteAt: {ExecuteAt.ToString("yyyy-MM-dd HH:mm:ss.fff")}, Data: [{Data.ToJson()}]}}";
        }
    }
}
