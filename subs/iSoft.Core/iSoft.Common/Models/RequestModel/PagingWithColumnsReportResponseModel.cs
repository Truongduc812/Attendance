using Newtonsoft.Json;

namespace iSoft.Common.Models.ResponseModels
{

  public class PagingWithColumnsReportResponseModel : PagingResponseModel
  {
    [JsonProperty("columns", NullValueHandling = NullValueHandling.Ignore)]
    public virtual List<ColumnResponseModel> Columns { get; set; } 
  }
}
