using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class DepartmentAdminModel 
  {
    public long? UserId { get; set; }
    public UserEntity? User { get; set; }
    public long? DepartmentId { get; set; }
    public DepartmentEntity? Department { get; set; }
    public EnumDepartmentAdmin? Role { get; set; }
    public string? Note { get; set; } 
  }
}
