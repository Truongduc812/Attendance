using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities
{
  [Table("EntryReports")]
  public class EntryReportEntity : BaseCRUDEntity
  {
    public EntryReportEntity()
    {
      EntryReportLogs = new HashSet<EntryReportLogEntity>();
    }

    [Required]
    [Column("EntryRequestId")]
    [ForeignKey(nameof(this.EntryRequest))]
    public long EntryRequestId { get; set; }
    public EntryRequestEntity EntryRequest { get; set; }

    [Required]
    [Column("ReportBy")]
    [ForeignKey(nameof(this.ReportByEmployee))]
    public long ReportBy { get; set; }
    public EmployeeEntity ReportByEmployee { get; set; }

    [Required]
    [Column("ReportStatus", TypeName = "VARCHAR(20)")]
    [DefaultValue(EnumEntryReportStatus.INITIALIZED)]
    public string ReportStatus { get; set; }

    [Required]
    [Column("MeasuredWeight")]
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

    public virtual ICollection<EntryReportLogEntity> EntryReportLogs { get; set; }
  }
}
