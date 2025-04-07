using iSoft.Common.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Payloads
{
  public class EnvData
  {
    [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
    public string Key { get; set; }

    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
    public string Value { get; set; }

    public EnvData(string key, string value)
    {
      Key = key;
      Value = value;
    }
    public EnvData()
    {

    }
    public override string ToString()
    {
      return $"{{{Key}: {Value}}}";
    }
  }
}
