using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Enums
{
  public enum EnumCRUDType
  {
    None = 0,
    View = 1,
    Create = 2,
    Edit = 3,
    Delete = 4,
    Request = 5,
    Approve = 6,
    AllPermission = 7,
  }
}
