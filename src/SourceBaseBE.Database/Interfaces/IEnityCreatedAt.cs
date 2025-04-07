using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Interfaces
{
  public interface IEnityCreatedAt
  {
    [Column(TypeName = "DATETIME")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.fff}")]
    abstract DateTime? CreatedAt { get; set; }
  }
}
