using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  [Table("AreaCodes")]

  [Microsoft.EntityFrameworkCore.Index(nameof(AreaCode), IsUnique = true)]
  public class AreaCodeEntity : BaseCRUDEntity
  {
    public AreaCodeEntity() {
      //AreaRegistrations = new HashSet<AreaRegistrationEntity>();
      Alarms = new HashSet<AlarmEntity>();
      CameraSettings = new HashSet<CameraSettingEntity>();
      Employees = new HashSet<EmployeeEntity>();
      EntryRequests = new HashSet<EntryRequestEntity>();
    }

    [Required]
    [Column("AreaCode")]
    [MaxLength(255)]
    public string? AreaCode { get; set; }

    [Required]
    [Column("AreaName")]
    [MaxLength(255)]
    public string? AreaName { get; set; }

    //public ICollection<AreaRegistrationEntity> AreaRegistrations { get; set; }
    public virtual ICollection<AlarmEntity> Alarms { get; set; }

    public virtual ICollection<CameraSettingEntity> CameraSettings { get; set; }

    public virtual ICollection<EmployeeEntity> Employees { get; set; } 

    public virtual ICollection<EntryRequestEntity> EntryRequests { get; set; }
  }
}
