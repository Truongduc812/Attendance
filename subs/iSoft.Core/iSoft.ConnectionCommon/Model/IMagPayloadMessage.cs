using iSoft.Common.ExtensionMethods;
using iSoft.ConnectionCommon.MessageBroker;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace iSoft.ConnectionCommon.Model
{
  public class DevicePayloadMessage : BaseMessage
  {
    [JsonProperty("connId", NullValueHandling = NullValueHandling.Ignore)]
    public long ConnectionId { get; set; }

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, object> Data { get; set; }

    [JsonProperty("exeAt", NullValueHandling = NullValueHandling.Ignore)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.fff}")]
    public DateTime ExecuteAt { get; set; }

    public static DevicePayloadMessage Create(long connId, List<EnvData> envDatas, DateTime dateTime)
    {
      throw new NotImplementedException("List<EnvData> -> Dictionary<string, object>");
      //var ret = new DevicePayloadMessage()
      //{
      //    ConnectionId = connId,
      //    Data = envDatas,
      //    ExecuteAt = dateTime
      //};
      //return ret;
    }
    public override string ToString()
    {
      //string arrEnvStr = string.Join(", ", Data.ConvertAll(x => x.ToString()).ToArray());
      return $"{{MessageId: {this.MessageId}, ConnectionId: {ConnectionId}, ExecuteAt: {ExecuteAt.ToString("yyyy-MM-dd HH:mm:ss.fff")}, Data: [{Data.ToJson()}]}}";
    }
  }
}
