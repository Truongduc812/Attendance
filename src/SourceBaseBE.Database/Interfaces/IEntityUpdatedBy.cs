using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceBaseBE.Database.Entities;

namespace SourceBaseBE.Database.Interfaces
{
  public interface IEntityUpdatedBy
  {
    // Id của user cập nhật gần nhất cho đối tượng này (không quan hệ)
    abstract long? UpdatedBy { get; set; }

    //abstract User? UpdatedByUser { get; set; }
  }
}
