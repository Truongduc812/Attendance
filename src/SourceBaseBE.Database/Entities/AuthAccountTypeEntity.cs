using iSoft.Database.Entities;
using SourceBaseBE.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  public class AuthAccountTypeEntity : BaseCRUDEntity
  {
    public AuthAccountTypeEntity()
    {
      Users = new HashSet<UserEntity>();
    }

    public ICollection<UserEntity> Users { get; set; }
  }
}
