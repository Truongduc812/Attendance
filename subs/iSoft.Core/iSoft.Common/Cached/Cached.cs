using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Cached
{

  public class MemCached : IDisposable
  {
    private static Dictionary<string, KeyValuePair<object, long>> dicCache = new Dictionary<string, KeyValuePair<object, long>>();
    //private static Cached instance = null;
    //private static object lockObject = new object();
    //public static int _cleanupIntervalInSeconds;
    //private Cached instance = null;
    private object lockObject = new object();
    public int _cleanupIntervalInSeconds;
    private Thread cleanupThread = null;
    public MemCached(int cleanupIntervalInSeconds)
    {
      _cleanupIntervalInSeconds = cleanupIntervalInSeconds;

      cleanupThread = new Thread(CleanupExpiredItems);
      cleanupThread.Start();
    }
    //public static Cached GetInstance(int cleanupIntervalInSeconds)
    //{
    //    lock (lockObject)
    //    {
    //        if (instance == null)
    //        {
    //            instance = new Cached(cleanupIntervalInSeconds);
    //        }
    //    }
    //    return instance;
    //}

    public void AddToCache(string key, object value, int expiredTimeInSeconds)
    {
      lock (dicCache)
      {
        if (dicCache.ContainsKey(key))
        {
          dicCache[key] = new KeyValuePair<object, long>(value, DateTime.Now.Ticks + expiredTimeInSeconds * TimeSpan.TicksPerSecond);
        }
        else
        {
          dicCache.Add(key, new KeyValuePair<object, long>(value, DateTime.Now.Ticks + expiredTimeInSeconds * TimeSpan.TicksPerSecond));
        }
      }
    }

    public object GetFromCache(string key)
    {
      lock (dicCache)
      {
        if (ContainsKey(key))
        {
          return dicCache[key].Key;
        }
      }
      return null;
    }

    public bool IsFirstCheck(string key, object value, int expiredTimeInSeconds)
    {
      if (!ContainsKey(key))
      {
        AddToCache(key, value, expiredTimeInSeconds);
        return true;
      }
      return false;
    }

    public bool IsCanRetry(string key, int expiredTimeInSeconds, int maxRetry)
    {
      //if (GetLock("locked_" + key, 10))
      {
        if (!ContainsKey(key))
        {
          AddToCache(key, 1, expiredTimeInSeconds);
          return true;
        }

        int retry = (int)GetFromCache(key);
        AddToCache(key, retry + 1, expiredTimeInSeconds);

        if (retry < maxRetry)
        {
          return true;
        }

        DeleteKey(key);
        return false;
      }

    }

    public int GetCount()
    {
      return dicCache.Count;
    }
    public bool ContainsKey(string key)
    {
      lock (lockObject)
      {
        if (dicCache.ContainsKey(key))
        {
          long ticks = dicCache[key].Value;
          return DateTime.Now.Ticks < ticks;
        }
      }
      return false;
    }

    //public bool GetLock(string key, int expiredTimeInSeconds)
    //{
    //  lock (lockObject)
    //  {
    //    if (dicCache.ContainsKey(key))
    //    {
    //      long ticks = dicCache[key].Value;
    //      if (DateTime.Now.Ticks < ticks)
    //      {
    //        return false;
    //      }
    //      else
    //      {
    //        AddToCache(key, true, expiredTimeInSeconds);
    //        return true;
    //      }
    //    }
    //    else
    //    {
    //      AddToCache(key, true, expiredTimeInSeconds);
    //      return true;
    //    }
    //  }
    //}

    public void RemoveLock(string key)
    {
      lock (lockObject)
      {
        DeleteKey(key);
      }
    }

    private void CleanupExpiredItems()
    {
      while (true)
      {
        List<string> expiredKeys = new List<string>();

        lock (dicCache)
        {
          foreach (var item in dicCache)
          {
            string key = item.Key;
            long ticks = item.Value.Value;

            if (DateTime.Now.Ticks >= ticks)
            {
              expiredKeys.Add(key);
            }
          }

          foreach (string key in expiredKeys)
          {
            DeleteKey(key);
          }
        }

        Thread.Sleep(_cleanupIntervalInSeconds * 1000);
      }
    }
    private void DeleteKey(string key)
    {
      dicCache.Remove(key);
    }

    public void Dispose()
    {
      if (cleanupThread != null && cleanupThread.IsAlive)
      {
        cleanupThread.Abort();
      }
      if (dicCache != null && dicCache.Count > 0)
      {
        dicCache.Clear();
      }
    }
  }
}
