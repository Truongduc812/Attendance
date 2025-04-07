using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class DepartmentAdminResponseModel : BaseCRUDResponseModel<DepartmentAdminEntity>
  {
    public long? UserId { get; set; }
    public UserEntity? User { get; set; }
    public long? DepartmentId { get; set; }
    public DepartmentEntity? Department { get; set; }
    public EnumDepartmentAdmin? Role { get; set; }
    public string? Note { get; set; }

    public override object SetData(DepartmentAdminEntity entity)
    {
      base.SetData(entity);
      this.UserId = entity.UserId;
      this.User = entity.User;
      this.DepartmentId = entity.DepartmentId;
      this.Department = entity.Department;
      this.Role = entity.Role;
      this.Note = entity.Note;

      return this;
    }
    public override List<object> SetData(List<DepartmentAdminEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (DepartmentAdminEntity entity in listEntity)
      {
        listRS.Add(new DepartmentAdminResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
