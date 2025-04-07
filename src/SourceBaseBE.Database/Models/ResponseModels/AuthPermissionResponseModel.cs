using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class AuthPermissionResponseModel : BaseCRUDResponseModel<AuthPermissionEntity>
  {
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<AuthGroupEntity>? ListAuthGroup { get; set; }
    public List<UserEntity>? ListUser { get; set; }

    public override object SetData(AuthPermissionEntity entity)
    {
      base.SetData(entity);
      this.Name = entity.Name;
      this.Description = entity.Description;

      if (entity.ListAuthGroup != null)
      {
        this.ListAuthGroup = entity.ListAuthGroup.Select(x => x).ToList();
      }

      if (entity.ListUser != null)
      {
        this.ListUser = entity.ListUser.Select(x => x).ToList();
      }

      return this;
    }
    public override List<object> SetData(List<AuthPermissionEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (AuthPermissionEntity entity in listEntity)
      {
        listRS.Add(new AuthPermissionResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
