using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Entities
{
  public class CommProtocol : Entity
  {
    public CommProtocol()
    {
      CommConfigs = new HashSet<CommConfig>();
    }

    [Required]
    public CommConnection Connection { get; set; }

    //[InverseProperty("CommProtocol")] // Xác định mối quan hệ ngược với tên navigation property trong lớp CommConfig
    public ICollection<CommConfig> CommConfigs { get; set; }
  }
}