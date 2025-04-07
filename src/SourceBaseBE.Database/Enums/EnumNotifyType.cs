using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Enums
{
  public enum EnumNotifyType
  {
    [Description("INFORMATION")]
    INFORMATION,
    [Description("WARNING")]
    WARNING,
    [Description("ALARM")]
    ALARM
  }
}
