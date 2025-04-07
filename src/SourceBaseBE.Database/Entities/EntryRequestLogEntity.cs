using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities;

public class EntryRequestLogEntity: BaseCRUDEntity
{

  [Required]
  [Column("EntryRequestId")]
  [ForeignKey(nameof(this.EntryRequest))]
  public long? EntryRequestId { get; set; }
  public virtual EntryRequestEntity? EntryRequest { get; set; }

  [Required]
  [Column("ChangedBy")]
  [ForeignKey(nameof(this.ChangedByEmployee))]
  public long? ChangedBy { get; set; }
  public virtual EmployeeEntity? ChangedByEmployee { get; set; }

  [Column("Note")]
  public string? Note { get; set; }

  [Required]
  [Column("Action")]
  [DefaultValue(EnumRegistrationAction.INITIALIZE)]
  public string Action { get; set; }


  [Required]
  [Column("Status")]
  [DefaultValue(EnumRegistrationStatus.INITIALIZED)]
  public string Status { get; set; }
}
