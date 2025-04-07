using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Enums
{
  public enum EnumActiveStatus
  {
    Actived = 1,
    Disabled = 2,
  }
  public class EnumEmployeeStatusStr
  {
    public static string Active = "Active";
    public static string InActive = "In-active";
    public static string GetFromCode(EnumEmployeeStatus? st)
    {
      switch(st)
      {
        case EnumEmployeeStatus.Actived:
          return EnumEmployeeStatusStr.Active;
        case EnumEmployeeStatus.InActive:
          return EnumEmployeeStatusStr.InActive;
      }
      return "";
    }
  }
  public enum EnumEmployeeStatus
  {
    Actived = 1,
    InActive = 2,
  }
}
