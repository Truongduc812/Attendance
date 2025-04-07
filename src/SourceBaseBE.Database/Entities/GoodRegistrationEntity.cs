using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities;

public class GoodRegistrationEntity : BaseCRUDEntity
{
  [Required]
  [Column("GoodsTypeId")]
  [ForeignKey(nameof(this.GoodsType))]
  public long GoodsTypeId { get; set; }
  public virtual GoodTypeEntity GoodsType { get; set; } = null!;

  [Required]
  [Column("EntryRequestId")]
  [ForeignKey(nameof(this.EntryRequest))]
  public long EntryRequestId { get; set; }
  public virtual EntryRequestEntity EntryRequest { get; set; } = null!;

  [Required]
  [Column("Unit")]
  public string Unit { get; set; }

  [Required]
  [Column("Quantity")]
  public long Quantity { get; set; }
}
