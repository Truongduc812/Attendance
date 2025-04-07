using Newtonsoft.Json;
using System.Collections.Generic;

namespace iSoft.Database.Models
{
  public class PagingModel
  {
    public class PagingRequest
    {
      public int Page { get; set; }

      public int PageSize { get; set; }

      public int GetSkip()
      {
        return (Page - 1) * PageSize;
      }
      public int GetLimit()
      {
        return PageSize;
      }
      public int GetBeginNumber()
      {
        return (Page - 1) * PageSize + 1;
      }
    }

    public class PagingResponse<T>
    {
      [JsonProperty("datas")]
      public List<T>? Datas { get; set; } = new List<T>();

      [JsonProperty("page")]
      public int Page { get; set; } = 1;

      [JsonProperty("pageSize")]
      public int PageSize { get; set; } = 0;

      [JsonProperty("totalPage")]
      public int TotalPage { get; set; } = 0;
    }
  }
}
