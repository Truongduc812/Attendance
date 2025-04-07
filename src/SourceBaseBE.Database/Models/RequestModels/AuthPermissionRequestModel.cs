using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class AuthPermissionRequestModel : BaseCRUDRequestModel<AuthPermissionEntity>
  {
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<long>? ListAuthGroup { get; set; }
    public List<long>? ListUser { get; set; }

    public override AuthPermissionEntity GetEntity(AuthPermissionEntity entity)
    {
      if (Id != null)
      {
        entity.Id = (long)Id;
      }
      entity.Order = Order;
      entity.Name = this.Name;
      entity.Description = this.Description;

      if (this.ListAuthGroup != null)
      {
        entity.AuthGroupIds = this.ListAuthGroup.Select(x => x).ToList();
      }

      if (this.ListUser != null)
      {
        entity.UserIds = this.ListUser.Select(x => x).ToList();
      }

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
