using Newtonsoft.Json;

namespace iSoft.Common.Models.ResponseModels
{

  public class PagingWithColumnsResponseModel : PagingResponseModel
  {
    [JsonProperty("columns", NullValueHandling = NullValueHandling.Ignore)]
    public virtual List<ColumnResponseModel> Columns { get; set; }

    [JsonProperty("counts", NullValueHandling = NullValueHandling.Ignore)]
    public virtual List<CountResponseModel> Counts { get; set; }

  }
}
