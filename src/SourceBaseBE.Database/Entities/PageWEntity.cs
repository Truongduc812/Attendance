using iSoft.Database.Entities;
using iSoft.DBLibrary.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities 
{
  [Table("PageWs")]
  public class PageWEntity : BaseCRUDEntity
  {
    // Noted code: "public override PageWEntity GetEntity(PageWEntity entity)"
    public string Name { get; set; } = "";
    public string? Alias { get; set; } = "";
    public string? Description { get; set; } = "";
    public EnumPageType? PageType { get; set; }
    [ForeignKey(nameof(PageMasterEntity))]
    public long? PatternPageMasterId { get; set; }
    public PageMasterEntity? PatternPageMaster { get; set; }
    [ForeignKey(nameof(UserEntity))]
    public long? OwnerId { get; set; }
    public UserEntity? Owner { get; set; }

    public long? ParentId { get; set; }
    public virtual PageWEntity? Parent { get; set; }
    public virtual ICollection<PageWEntity>? Children { get; set; }

  }
}
