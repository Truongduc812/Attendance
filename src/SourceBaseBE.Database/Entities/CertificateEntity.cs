using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities;

[Table("Certificatess")]
public class CertificateEntity : BaseCRUDEntity
{

  [Required]
  [Column("EditerId")]
  [ForeignKey(nameof(this.Employee))]
  public long? EmployeeId { get; set; }
  public virtual EmployeeEntity? Employee { get; set; }

  [Column("CertificateNumber")]
  [MaxLength(255)]
  public string? CertificateNumber { get; set; }

  [Required]
  [Column("FrontImageFileId")]
  [ForeignKey(nameof(this.FrontImageFile))]
  public long? FrontImageFileId { get; set; }
  public FileEntity? FrontImageFile { get; set; }

  [Required]
  [Column("BackImageFileId")]
  [ForeignKey(nameof(this.BackImageFile))]
  public long? BackImageFileId { get; set; }
  public FileEntity? BackImageFile { get; set; }

  [Column("TypeCertificate")]
  public string TypeCertificate { get; set; }
}
