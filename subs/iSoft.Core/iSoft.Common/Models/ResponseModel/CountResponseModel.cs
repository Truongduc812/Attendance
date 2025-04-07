using Newtonsoft.Json;

namespace iSoft.Common.Models.ResponseModels
{
  public class CountResponseModel
  {
    [JsonProperty("name")]
    public string? Key { get; set; }

    [JsonProperty("value")]
    public long? Number { get; set; }
 
  }
}
