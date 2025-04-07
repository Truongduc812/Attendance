using iSoft.Database.Entities;
using iSoft.DBLibrary.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities 
{
  [Table("ComponentWs")]
  public class ComponentWEntity : BaseCRUDEntity
  {
    // Noted code: "public override ComponentWEntity GetEntity(ComponentWEntity entity)"
    public string Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public EnumComponentType? ComponentType { get; set; }

    [ForeignKey(nameof(PageWEntity))]
    public long? PageId { get; set; }
    public PageWEntity? Page { get; set; }

    public long? ParentId { get; set; }
    public virtual ComponentWEntity Parent { get; set; }
    public virtual ICollection<ComponentWEntity> Children { get; set; }
    public long? RefId { get; set; }
    public virtual ComponentWEntity Ref { get; set; }
    public virtual ICollection<ComponentWEntity> RefChildren { get; set; }

    public int? DeepLevel { get; set; }
    public bool? HideFlag { get; set; }
    public bool? SpecialComponentFlag { get; set; }
    public EnumComponentPosition? CSSPosition { get; set; }
    [ForeignKey(nameof(ComponentMasterEntity))]
    public long? PatternComponentMasterId { get; set; }
    public ComponentMasterEntity? PatternComponentMaster { get; set; }
    public string? CSSTop { get; set; } = "";
    public string? CSSLeft { get; set; } = "";
    public string? CSSRight { get; set; } = "";
    public string? CSSBottom { get; set; } = "";
    public string? CSSHeight { get; set; } = "";
    public string? CSSWidth { get; set; } = "";
    public EnumDeviceResolution? DeviceResolution { get; set; }
  }
}
