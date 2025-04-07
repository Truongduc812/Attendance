using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.CheckPerTime
{
  public class CheckPerTime
  {
    private int hour { get; set; }
    private int minute { get; set; }
    private int second { get; set; }

    public CheckPerTime()
    {
      var curDateTime = DateTime.Now;
      this.hour = curDateTime.Hour;
      this.minute = curDateTime.Minute;
      this.second = curDateTime.Second;
    }
    public CheckPerTime(DateTime curDateTime)
    {
      this.hour = curDateTime.Hour;
      this.minute = curDateTime.Minute;
      this.second = curDateTime.Second;
    }

    public bool PerSeconds(int seconds)
    {
      return this.second % seconds == 0;
    }
    public bool PerMinutes(int minutes)
    {
      return this.minute % minutes == 0 && this.second == 0;
    }
    public bool PerHours(int hours)
    {
      return this.hour % hours == 0 && this.minute == 0 && this.second == 0;
    }
  }
}
