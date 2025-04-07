using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities
{
  [Table("EntryCaptures")]
  [Microsoft.EntityFrameworkCore.Index(nameof(EntryCaptureEntity.EntryRequestId), nameof(EntryCaptureEntity.InOutType),  IsUnique = true)]
  public class EntryCaptureEntity : BaseCRUDEntity
  {
    [Required]
    [Column("EntryRequestId")]
    [ForeignKey(nameof(this.EntryRequest))]
    public long EntryRequestId { get; set; }
    public EntryRequestEntity EntryRequest { get; set; }

    [Required]
    [Column("CapturedBy")]
    [ForeignKey(nameof(this.CapturedByEmployee))]
    public long CapturedBy { get; set; }
    public EmployeeEntity? CapturedByEmployee { get; set; }

    [Required]
    [Column("Status", TypeName = "VARCHAR(10)")]
    [DefaultValue(EnumEntryCaptureStatus.CAPTURED)]
    public string Status { get; set; }

    [Required]
    [Column("MeasuredWeight", TypeName = "REAL")]
    public decimal MeasuredWeight { get; set; }

    [Required]
    [Column("LicensePlateScanned", TypeName = "VARCHAR(20)")]
    public string LicensePlateScanned { get; set; }

    [Required]
    [Column("Status", TypeName = "VARCHAR(10)")]
    [DefaultValue(EnumInOutType.IN)]
    public string InOutType { get; set; }

    [Required]
    [Column("ImageFileId")]
    [ForeignKey(nameof(this.ImageFile))]
    public long ImageFileId { get; set; }
    public FileEntity ImageFile { get; set; }

    [Column("Note", TypeName = "TEXT")]
    public string? Note { get; set; }
  }
}
