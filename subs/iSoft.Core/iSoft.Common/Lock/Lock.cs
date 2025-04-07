using iSoft.Common.Cached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Lock
{
  public class Lock
  {
    private static object lockObj = new object();
    public static object lockObj_RunSQLServer = new object();
    public static object lockObj_SaveESLastData = new object();
    public static object lockObj_SaveTraceLastData = new object();

    private static MemCached memCached = new MemCached(1);
    public static object GetLockObject(string key, int expiredTimeInSeconds)
    {
      lock (lockObj)
      {
        if (!memCached.ContainsKey(key))
        {
          memCached.AddToCache(key, new object(), expiredTimeInSeconds);
        }
        return memCached.GetFromCache(key);
      }
    }
  }
}
