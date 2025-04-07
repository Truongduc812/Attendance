using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  [Table("EntryTransactionTypes")]
  public class EntryTransactionTypeEntity : BaseCRUDEntity
  {
    public EntryTransactionTypeEntity() {
      EntryRequests = new HashSet<EntryRequestEntity>(); 
    }

    [Required]
    [Column("TypeName")]
    [MaxLength(255)]
    public string? TypeName { get; set; }

    public virtual ICollection<EntryRequestEntity> EntryRequests { get; set; }
  }
}
