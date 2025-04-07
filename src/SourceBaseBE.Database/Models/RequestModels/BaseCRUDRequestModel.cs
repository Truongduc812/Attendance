using iSoft.Database.Entities;
using Microsoft.AspNetCore.Http;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class BaseCRUDRequestModel<TEntity> where TEntity : BaseCRUDEntity, new()
  {
    public long? Id { get; set; }
    public long? Order { get; set; }

    public virtual TEntity GetEntity(TEntity entity)
    {
      if (this.Id != null)
      {
        entity.Id = (long)this.Id;
      }
      entity.Order = Order;
      return entity;
    }

    public virtual Dictionary<string, (string, IFormFile)> GetFiles()
    {
      return null;
    }
  }
}
