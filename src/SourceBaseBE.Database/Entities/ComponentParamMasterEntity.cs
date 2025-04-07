using iSoft.Common.Enums;
using iSoft.Database.Entities;
using iSoft.DBLibrary.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Entities 
{
  [Table("ComponentParamMasters")]
  public class ComponentParamMasterEntity : BaseCRUDEntity
  {
    // Noted code: "public override ComponentParamMasterEntity GetEntity(ComponentParamMasterEntity entity)"
    public string Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public EnumDataType? DataType { get; set; }

    [ForeignKey(nameof(ComponentWEntity))]
    public long? ComponentId { get; set; }
    public ComponentWEntity? Component { get; set; }
    [ForeignKey(nameof(PageWEntity))]
    public long? PageId { get; set; }
    public PageWEntity? Page { get; set; }
  }
}
