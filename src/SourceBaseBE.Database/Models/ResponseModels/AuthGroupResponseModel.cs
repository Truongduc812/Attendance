using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class AuthGroupResponseModel : BaseCRUDResponseModel<AuthGroupEntity>
  {
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<AuthPermissionEntity>? ListAuthPermission { get; set; }
    public List<UserEntity>? ListUser { get; set; }

    public override object SetData(AuthGroupEntity entity)
    {
      base.SetData(entity);
      this.Name = entity.Name;
      this.Description = entity.Description;

      if (entity.ListAuthPermission != null)
      {
        this.ListAuthPermission = entity.ListAuthPermission.Select(x => x).ToList();
      }

      if (entity.ListUser != null)
      {
        this.ListUser = entity.ListUser.Select(x => x).ToList();
      }

      return this;
    }
    public override List<object> SetData(List<AuthGroupEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (AuthGroupEntity entity in listEntity)
      {
        listRS.Add(new AuthGroupResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
