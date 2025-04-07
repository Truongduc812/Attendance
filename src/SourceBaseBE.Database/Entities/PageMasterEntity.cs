using iSoft.Database.Entities;
using iSoft.DBLibrary.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities 
{
  [Table("PageMasters")]
  public class PageMasterEntity : BaseCRUDEntity
  {
    // Noted code: "public override PageMasterEntity GetEntity(PageMasterEntity entity)"
    public string Name { get; set; } = "";
    public string? Alias { get; set; } = "";
    public string? Description { get; set; } = "";
    public EnumPageType? PageType { get; set; }

    public long? ParentId { get; set; }
    public virtual PageMasterEntity Parent { get; set; }
    public virtual ICollection<PageMasterEntity> Children { get; set; }
  }
}
