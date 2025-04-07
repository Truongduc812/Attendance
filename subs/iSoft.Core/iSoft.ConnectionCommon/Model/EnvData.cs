using iSoft.Common.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.ConnectionCommon.Model
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

    public double GetValue()
    {
      bool convertResult = double.TryParse(this.Value, out double doubleVal);
      if (convertResult)
      {
        return doubleVal;
      };
      throw new BaseException($"Convert string to double error, input: {this.Value}");
    }
    public override string ToString()
    {
      return $"{{{Key}: {Value}}}";
    }
  }
}
