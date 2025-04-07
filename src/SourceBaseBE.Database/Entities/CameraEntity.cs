using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  [Table("Cameras")]
  public class CameraEntity : BaseCRUDEntity
  {
    public CameraEntity() {
      CameraSettings = new HashSet<CameraSettingEntity>();
    }

    [Column("RtspLink", TypeName = "VARCHAR(255)")]
    [MaxLength(255)]
    public string? RtspLink { get; set; }

    [Column("CameraName")]
    [MaxLength(100)]
    public string? CameraName { get; set; }

    [Column("CameraLocation", TypeName = "VARCHAR(255)")]
    [MaxLength(255)]
    public string? CameraLocation { get; set; }

    [Column("FrameRate")]
    public int? FrameRate { get; set; }

    [Column("OutPutWidth")]
    public int? OutPutWidth { get; set; }

    [Column("OutPutHeight")]
    public int? OutPutHeight { get; set; }

    public virtual ICollection<CameraSettingEntity> CameraSettings { get; set; }
  }
}
