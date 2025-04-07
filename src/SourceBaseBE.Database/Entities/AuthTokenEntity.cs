using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  public class AuthTokenEntity : BaseCRUDEntity
  {
    public string AccessToken { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public List<UserEntity>? ListUser { get; set; }
  }
}
