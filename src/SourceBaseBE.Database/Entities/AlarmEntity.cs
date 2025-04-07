using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;
using System.Runtime.CompilerServices;

namespace SourceBaseBE.Database.Entities
{
  [Table("Alarms")]
  public class AlarmEntity : BaseCRUDEntity
  {
    [Required]
    [Column("AlarmTypeId")]
    [ForeignKey(nameof(this.AlarmType))]
    public long? AlarmTypeId { get; set; }
    public AlarmTypeEntity? AlarmType { get; set; }

    [Required]
    [Column("EntryRequestId")]
    [ForeignKey(nameof(this.EntryRequest))]
    public long? EntryRequestId { get; set; }
    public EntryRequestEntity? EntryRequest { get; set; }

    [Required]
    [Column("AlarmAreaId")]
    [ForeignKey(nameof(this.AreaCode))]
    public long? AlarmAreaId { get; set; }
     public AreaCodeEntity? AreaCode { get; set; }

    [Required]
    [Column("AlarmName", TypeName = "VARCHAR(255)")]
    public string? AlarmName { get; set; }

    [Required]
    [Column("AlarmTime", TypeName = "TIMESTAMP")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? AlarmTime { get; set; }

    [Column("FilePath", TypeName = "VARCHAR(255)")]
    public string? FilePath { get; set; }

  }
}
