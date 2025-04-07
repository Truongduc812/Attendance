using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities;

public class GoodTypeEntity : BaseCRUDEntity
{
  [Required]
  [Column("Name", TypeName = "VARCHAR(255)")]
  public string? Name { get; set; }

  [Required]
  [Column("Category", TypeName = "VARCHAR(255)")]
  public string? Category { get; set; }

  [Column("ProductSKU", TypeName = "VARCHAR(255)")]
  public string? ProductSKU { get; set; }

  [ForeignKey(nameof(Entities.FileEntity))]
  public long? ImageFileId { get; set; }
  public FileEntity? ImageFile { get; set; }

  [Required]
  [Column("Weight")]
  public decimal Weight { get; set; }

  public virtual ICollection<GoodRegistrationEntity> GoodRegistrations { get; set; } = new List<GoodRegistrationEntity>();
}
