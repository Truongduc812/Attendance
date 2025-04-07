using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Interfaces
{
  public interface IEnityDeletedFlag
  {
    [DefaultValue(false)]
    abstract bool DeletedFlag { get; set; }
  }
}
