using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Utils
{
  public class NumberUtil
  {
    private static Random ran;
    static NumberUtil()
    {
      ran = new Random();
    }
    public static int GetRandomInt(int min, int max)
    {
      return ran.Next(min, max + 1);
    }
    public static long GetRandomLong(long min, long max)
    {
      return ran.NextInt64(min, max + 1);
    }
    public static double GetRandomDouble(double min, double max)
    {
      return min + (ran.NextDouble() * (max - min));
    }
  }
}
