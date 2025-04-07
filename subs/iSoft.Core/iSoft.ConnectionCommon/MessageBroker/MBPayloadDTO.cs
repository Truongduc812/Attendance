using ConnectionCommon.Connection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.ConnectionCommon.MessageBroker
{
  public class MBPayloadDTO
  {
    [JsonProperty("params")]
    public ICollection<ConnectionParam>? ConnectionParams { get; set; }
    public static MBPayloadDTO Create(ICollection<ConnectionParam>? ConnectionParams)
    {
      var dto = new MBPayloadDTO
      {
        ConnectionParams = ConnectionParams,
      };
      return dto;
    }
  }
}
