using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class AuthTokenResponseModel : BaseCRUDResponseModel<AuthTokenEntity>
  {
    public string AccessToken { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public List<UserEntity>? ListUser { get; set; }

    public override object SetData(AuthTokenEntity entity)
    {
      base.SetData(entity);
      this.AccessToken = entity.AccessToken;
      this.ExpirationTime = entity.ExpirationTime;
      this.ListUser = entity.ListUser;

      return this;
    }
    public override List<object> SetData(List<AuthTokenEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (AuthTokenEntity entity in listEntity)
      {
        listRS.Add(new AuthTokenResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
