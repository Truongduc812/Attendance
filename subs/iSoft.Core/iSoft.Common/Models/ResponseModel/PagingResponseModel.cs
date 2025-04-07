using Newtonsoft.Json;

namespace iSoft.Common.Models.ResponseModels
{

  public class PagingResponseModel
  {

    //[JsonProperty("beginNumber", NullValueHandling = NullValueHandling.Ignore)]
    //public virtual long BeginNumber { get; set; }
    [JsonProperty("totalRecord", NullValueHandling = NullValueHandling.Ignore)]
    public virtual long? TotalRecord { get; set; }

    [JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
    public virtual List<object> ListData { get; set; }

  }
}
