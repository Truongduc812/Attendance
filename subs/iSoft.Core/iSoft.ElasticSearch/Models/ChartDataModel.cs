using iSoft.Common.Utils;
using Newtonsoft.Json;

namespace iSoft.ElasticSearch.Models
{
	public class ChartDataModel
  {
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Id { get; set; }
    [JsonIgnore]
    public DateTime ExecuteAtData {
      get
      {
        return DateTimeUtil.GetDateTimeFromString(this.ExecuteAt, "yyyy-MM-dd HH:mm:ss.fff");
      }
    }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ExecuteAt { get; set; } // yyyy-MM-dd HH:mm:ss.fff
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, object> DicValue = new Dictionary<string, object>();

    public override string ToString()
    {
      return $"{ExecuteAt}: {DicValue.Count}";
    }
  }
}
