using iSoft.Common.Enums;
using iSoft.Common.Utils;
using iSoft.Database.Entities;
using Microsoft.AspNetCore.Http;
using static iSoft.Common.ConstCommon;

namespace iSoft.Database.Models.RequestModels
{
  public class ISoftProjectRequestModel : BaseCRUDRequestModel<ISoftProjectEntity>
  {
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<long>? ListUser { get; set; }
    public EnumActiveStatus? Status { get; set; }

    public override ISoftProjectEntity GetEntity(ISoftProjectEntity entity)
    {
      if (Id != null)
      {
        entity.Id = (long)Id;
      }
      entity.Order = Order;
      entity.Name = this.Name;
      entity.Description = this.Description;

      if (this.ListUser != null)
      {
        entity.UserIds = this.ListUser.Select(x => x).ToList();
      }
      entity.Status = this.Status;

      return entity;
    }

    public override Dictionary<string, (string, IFormFile)> GetFiles()
    {
      Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
      
/*[GEN-17]*/
      return dicRS;
    }
  }
}
