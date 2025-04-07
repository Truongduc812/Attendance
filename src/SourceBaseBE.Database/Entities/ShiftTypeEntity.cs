using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  public class ShiftTypeEntity : BaseCRUDEntity
  {
    public ShiftTypeEntity()
    {
      Shifts = new HashSet<ShiftEntity>();
    }
    public ICollection<ShiftEntity> Shifts { get; set; }
  }
}