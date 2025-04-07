using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  public class DataTypeEntity : BaseCRUDEntity
  {
    public DataTypeEntity()
    {
      Parameters = new HashSet<ParameterEntity>();
    }
    public ICollection<ParameterEntity> Parameters { get; set; }
  }
}
