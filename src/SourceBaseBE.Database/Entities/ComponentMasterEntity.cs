using iSoft.Database.Entities;
using iSoft.DBLibrary.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities 
{
  [Table("ComponentMasters")]
  public class ComponentMasterEntity : BaseCRUDEntity
  {
    // Noted code: "public override ComponentMasterEntity GetEntity(ComponentMasterEntity entity)"
    public string Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public EnumComponentType? ComponentType { get; set; }
    [ForeignKey(nameof(PageWEntity))]
    public long? PageId { get; set; }
    public PageWEntity? Page { get; set; }

    public long? ParentId { get; set; }
    public virtual ComponentMasterEntity Parent { get; set; }
    public virtual ICollection<ComponentMasterEntity> Children { get; set; }
    public long? RefId { get; set; }
    public virtual ComponentMasterEntity Ref { get; set; }
    public virtual ICollection<ComponentMasterEntity> RefChildren { get; set; }

    public int? DeepLevel { get; set; }
    public bool? HideFlag { get; set; }
    public bool? SpecialComponentFlag { get; set; }
    public EnumComponentPosition? CSSPosition { get; set; }
    public string? CSSTop { get; set; } = "";
    public string? CSSLeft { get; set; } = "";
    public string? CSSRight { get; set; } = "";
    public string? CSSBottom { get; set; } = "";
    public string? CSSHeight { get; set; } = "";
    public string? CSSWidth { get; set; } = "";
    public EnumDeviceResolution? DeviceResolution { get; set; }

  }
}
