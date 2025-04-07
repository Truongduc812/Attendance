using iSoft.Common.Enums;
using iSoft.Common.Utils;
using iSoft.Database.Entities;
using static iSoft.Common.ConstCommon;

namespace iSoft.Database.Models.ResponseModels
{
  public class ISoftProjectResponseModel : BaseCRUDResponseModel<ISoftProjectEntity>
  {
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<UserEntity>? ListUser { get; set; }
    public EnumActiveStatus? Status { get; set; }

    public override object SetData(ISoftProjectEntity entity)
    {
      base.SetData(entity);
      this.Name = entity.Name;
      this.Description = entity.Description;

      if (entity.ListUser != null)
      {
        this.ListUser = entity.ListUser.Select(x => x).ToList();
      }
      this.Status = entity.Status;

      return this;
    }
    public override List<object> SetData(List<ISoftProjectEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (ISoftProjectEntity entity in listEntity)
      {
        listRS.Add(new ISoftProjectResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
