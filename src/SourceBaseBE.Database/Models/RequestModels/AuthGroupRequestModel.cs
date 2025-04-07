using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class AuthGroupRequestModel : BaseCRUDRequestModel<AuthGroupEntity>
  {
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<long>? ListAuthPermission { get; set; }
    public List<long>? ListUser { get; set; }

    public override AuthGroupEntity GetEntity(AuthGroupEntity entity)
    {
      if (Id != null)
      {
        entity.Id = (long)Id;
      }
      entity.Order = Order;
      entity.Name = this.Name;
      entity.Description = this.Description;

      if (this.ListAuthPermission != null)
      {
        entity.AuthPermissionIds = this.ListAuthPermission.Select(x => x).ToList();
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
