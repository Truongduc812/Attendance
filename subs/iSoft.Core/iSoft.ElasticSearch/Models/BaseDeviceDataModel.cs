using iSoft.Common.Utils;
using System;

namespace iSoft.ElasticSearch.Models
{
  public class BaseDeviceDataModel
  {
    //public virtual DateTime Timestamp { get; set; }
    public virtual long ConnectionId { get; set; }
    public virtual DateTime ExecuteAt { get; set; }
    public virtual String MessageId { get; set; }
    public BaseDeviceDataModel()
    {
    }

    public bool MatchValues(BaseDeviceDataModel? model2)
    {
      if (model2 == null)
      {
        return false;
      }
      
      return this.Equals(model2);
    }

  }
}
