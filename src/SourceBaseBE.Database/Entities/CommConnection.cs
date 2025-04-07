using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SourceBaseBE.Database.Entities
{
  public class CommConnection : Entity
  {
    public CommConnection()
    {
      Parameters = new HashSet<ParameterEntity>();
    }

    [ForeignKey("CommProtocol")] // Xác định khóa ngoại
    public long CommProtocolId { get; set; }

    [Required]
    public CommProtocol CommProtocol { get; set; }

    public ICollection<ParameterEntity> Parameters { get; set; }
  }
}
