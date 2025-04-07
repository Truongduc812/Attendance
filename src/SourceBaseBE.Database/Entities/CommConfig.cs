using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Entities
{
  public class CommConfig : Entity
  {
    [Required]
    [MaxLength(50)] // Độ dài tối đa cho Requester là 50 ký tự
    public string Type { get; set; }

    [Required]
    [MaxLength(255)] // Độ dài tối đa cho Value là 255 ký tự
    public string Value { get; set; }

    [ForeignKey("CommProtocol")] // Xác định khóa ngoại
    public long CommProtocolId { get; set; }

    [Required]
    public CommProtocol CommProtocol { get; set; }
  }
}
