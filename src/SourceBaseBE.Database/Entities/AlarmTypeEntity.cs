using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  [Table("AlarmTypes")]
  public class AlarmTypeEntity : BaseCRUDEntity
  {
    public AlarmTypeEntity() {
      Alarms = new HashSet<AlarmEntity>();
    }

    [Required]
    [Column("AlarmTypeName", TypeName = "VARCHAR(255)")]
    public string? AlarmTypeName { get; set; }
    public virtual ICollection<AlarmEntity> Alarms { get; set; }
  }
}
