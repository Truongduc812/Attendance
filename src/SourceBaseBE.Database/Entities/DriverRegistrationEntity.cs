using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SourceBaseBE.Database.Entities
{
  [Table("DriverRegistrations")]
  public class DriverRegistrationEntity
  {
    [Key]
    [Column("EntryRequestId", Order = 1)]
    [ForeignKey(nameof(this.EntryRequest))]
    public long EntryRequestId { get; set; }
    public EntryRequestEntity EntryRequest { get; set; }

    [Key]
    [Column("DriverId", Order = 2)]
    [ForeignKey(nameof(this.Driver))]
    public long DriverId { get; set; }
    public EmployeeEntity Driver { get; set; }

    [Column("IsMain", TypeName = "Bool")]
    public bool? IsMain { get; set; }
  }
}
