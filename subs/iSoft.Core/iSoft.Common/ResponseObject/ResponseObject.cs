using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.ResponseObjectNS
{
  public class ResponseObject
  {

    [JsonProperty("status")]
    public string? Status { get; set; } = null;

    [JsonProperty("code")]
    public string? Code { get; set; } = null;

    [JsonProperty("message")]
    public string? Message { get; set; } = null;

    [JsonProperty("data")]
    public object Data { get; set; } = null;


    public ResponseObject()
    {
    }

    public ResponseObject(string status, object data)
    {
      Status = status;
      Data = data;
    }

    public ResponseObject(string status, string code, string message)
    {
      Status = status;
      Code = code;
      Message = message;
    }

    public enum ResponseStatus
    {
      Success,
      Error
    }
  }
}
