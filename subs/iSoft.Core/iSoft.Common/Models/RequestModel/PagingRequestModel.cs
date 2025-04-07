
namespace iSoft.Common.Models.RequestModels
{
  public class PagingRequestModel
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
    public virtual string GetKeyCache()
    {
      return $"{this.Page}|{this.PageSize}";
    }
  }
}