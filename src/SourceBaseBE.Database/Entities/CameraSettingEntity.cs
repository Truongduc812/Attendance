using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.DBLibrary.Entities;

namespace SourceBaseBE.Database.Entities
{
  [Table("CameraSettings")]
  public class CameraSettingEntity
  {
    [Key]
    [Column("CameraId", Order = 1)]
    [ForeignKey(nameof(this.Camera))]
    public long? CameraId { get; set; }
    public CameraEntity? Camera { get; set; }

    [Key]
    [Column("AreaCodeId", Order = 2)]
    [ForeignKey(nameof(this.AreaCode))]
    public long? AreaCodeId { get; set; }
    public AreaCodeEntity? AreaCode { get; set; }

    [Required]
    [ForeignKey(nameof(this.FilePath))]
    [Column("FilePathId")]
    public long? FilePathId { get; set; }
    public FileEntity? FilePath { get; set; }

    [Required]
    [Column("StartTime", TypeName = "TIMESTAMP")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? StartTime { get; set; }

    [Required]
    [Column("EndTime", TypeName = "TIMESTAMP")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? EndTime { get; set; }
  }
}
