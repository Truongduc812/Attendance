using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
  public class WorkshopEntity:BaseCRUDEntity, IEntityCategory
  {
    public long FactoryId { get; set; }
    public FactoryEntity Factory { get; set; }

    public IEnumerable<LineEntity> Lines { get; set; }
    public string? Category { get; set; }
  }
}
