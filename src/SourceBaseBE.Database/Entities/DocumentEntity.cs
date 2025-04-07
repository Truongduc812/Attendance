using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;
using System.Runtime.CompilerServices;

namespace SourceBaseBE.Database.Entities
{
  [Table("Documents")]
  public class DocumentEntity : BaseCRUDEntity
  {

    [Column("FilePath", TypeName = "VARCHAR(255)")]
    public string? FilePath { get; set; }

  }
}
