using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class AuthTokenRequestModel : BaseCRUDRequestModel<AuthTokenEntity>
  {
    public string AccessToken { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public List<UserEntity>? ListUser { get; set; }

    public override AuthTokenEntity GetEntity(AuthTokenEntity entity)
    {
      if (Id != null)
      {
        entity.Id = (long)Id;
      }
      entity.Order = Order;
      entity.AccessToken = this.AccessToken;
      entity.ExpirationTime = this.ExpirationTime;
      entity.ListUser = this.ListUser;

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
