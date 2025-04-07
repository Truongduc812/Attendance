using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  [Table("Guests")]
  public class GuestEntity : BaseCRUDEntity
  {
    [Required]
    [Column("EntryRequestId")]
    [ForeignKey(nameof(this.EntryRequest))]
    public long? EntryRequestId { get; set; }
    public EntryRequestEntity? EntryRequest { get; set; }

    [Required]
    [Column("FullName", TypeName = "VARCHAR(50)")]
    public string? FullName { get; set; }

    [Required]
    [Column("CiNumber", TypeName = "VARCHAR(50)")]
    public string? CiNumber { get; set; }

    [Column("DrivingLicense", TypeName = "VARCHAR(50)")]
    public string? DrivingLicense { get; set; }

    [Column("DateOfBirth", TypeName = "DATE")]
    public DateOnly? DateOfBirth { get; set; }
  }
}
