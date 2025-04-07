using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities;

public class IdentityCardEntity : BaseCRUDEntity
{
  public IdentityCardEntity()
  {
    Employees = new HashSet<EmployeeEntity>();
  }

  [Required]
  [Column("CiNumber")]
  [MaxLength(255)]
  public string? CiNumber { get; set; }

  [Required]
  [Column("FrontImageFileId")]
  [ForeignKey(nameof(this.FrontImageFile))]
  public long? FrontImageFileId { get; set; }
  public FileEntity FrontImageFile {  get; set; }

  [Required]
  [Column("BackImageFileId")]
  [ForeignKey(nameof(this.BackImageFile))]
  public long? BackImageFileId { get; set; }
  public FileEntity BackImageFile { get; set; }

  public virtual ICollection<EmployeeEntity> Employees { get; set; }
}
