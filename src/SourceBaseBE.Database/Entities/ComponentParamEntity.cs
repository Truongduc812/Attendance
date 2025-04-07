using iSoft.Common.Enums;
using iSoft.Database.Entities;
using iSoft.DBLibrary.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities 
{
  [Table("ComponentParams")]
  public class ComponentParamEntity : BaseCRUDEntity
  {
    // Noted code: "public override ComponentParamEntity GetEntity(ComponentParamEntity entity)"
    public string Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public string? Value { get; set; } = "";

    [ForeignKey(nameof(ComponentWEntity))]
    public long? ComponentId { get; set; }
    public ComponentWEntity? Component { get; set; }
    [ForeignKey(nameof(PageWEntity))]
    public long? PageId { get; set; }
    public PageWEntity? Page { get; set; }

    [ForeignKey(nameof(ComponentParamMasterEntity))]
    public long? PatternComponentParamMasterId { get; set; }
    public ComponentParamMasterEntity? PatternComponentParamMaster { get; set; }

  }
}
