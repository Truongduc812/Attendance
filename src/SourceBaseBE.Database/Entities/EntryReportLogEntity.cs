using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities
{
  [Table("EntryReportLogs")]
  public class EntryReportLogEntity : BaseCRUDEntity
  {
    [Required]
    [Column("EntryReportId")]
    [ForeignKey(nameof(this.EntryReport))]
    public long EntryReportId { get; set; }
    public virtual EntryReportEntity EntryReport { get; set; }

    [Required]
    [Column("ChangedBy")]
    [ForeignKey(nameof(this.ChangedByEmployee))]
    public long? ChangedBy { get; set; }
    public virtual EmployeeEntity? ChangedByEmployee { get; set; }

    [Column("Note")]
    public string? Note { get; set; }

    [Required]
    [Column("Action")]
    [DefaultValue(EnumEntryReportAction.INITIALIZE)]
    public string Action { get; set; }

    [Required]
    [Column("Status")]
    [DefaultValue(EnumEntryReportStatus.INITIALIZED)]
    public string Status { get; set; }
  }
}