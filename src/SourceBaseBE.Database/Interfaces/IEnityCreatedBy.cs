using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceBaseBE.Database.Entities;

namespace SourceBaseBE.Database.Interfaces
{
  public interface IEnityCreatedBy
  {
    // Id của user cập tạo ra record này (không quan hệ)
    abstract long? CreatedBy { get; set; }

    //abstract User? CreatedByUser { get; set; }
  }
}
