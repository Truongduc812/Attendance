using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Enums
{
  public enum EnumDeviceResolution
  {
    None,
    MobileDevice, // 320px — 480px
    IPadTabletDevice, // 481px — 768px
    LaptopDevice, // 769px — 1024px
    DesktopDevice, // 1025px — 1200px
    ExtraLargeScreenDevice, // 1201px and more
    All,
  }
}
