//using Newtonsoft.Json;
//using iSoft.Common.ExtensionMethods;
//using Microsoft.Extensions.Logging;
//using iSoft.Common;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using iSoft.Redis.Services;
//using iSoft.Common.Models.ConfigModel.Subs;
//using Serilog;
//using iSoft.Common.Utils;
//using System.Diagnostics;
//using iSoft.Common.Enums;

//namespace iSoft.Database.CachedFuncNS
//{
//  public class CachedFunc
//  {
//    static object lockObj = new object();
//    static Serilog.ILogger _logger = Log.Logger;
//    static ServerConfigModel? redisConfig = null;
//    static RedisService redisService = new RedisService();
//    static Dictionary<string, bool> dicClearWithAnyChange = new Dictionary<string, bool>();
//    static Dictionary<string, List<string>> dicClearByEntity = new Dictionary<string, List<string>>();
//    public static void SetRedisConfig(ServerConfigModel config)
//    {
//      lock (lockObj)
//      {
//        redisConfig = config;
//      }
//    }
//    public static T? GetRedisData<T>(string redisKey, T? notFoundValue)
//    {
//      try
//      {
//        if (redisService.ConnectionStatus == EnumConnectionStatus.Error)
//        {
//          _logger.Error($"SetRedisData error, {redisKey}");
//          return notFoundValue;
//        }

//        lock (lockObj)
//        {
//          redisService.ConnectRedis(redisConfig);
//        }

//        string data = redisService.GetValue(redisKey);
//        if (string.IsNullOrEmpty(data))
//        {
//          return notFoundValue;
//        }
//        T rs = JsonConvert.DeserializeObject<T>(data);
//        return rs;
//      }
//      catch (Exception ex)
//      {
//        lock (lockObj)
//        {
//          redisService.ConnectionStatus = EnumConnectionStatus.Error;
//        }
//        _logger.LogMsg(Messages.ErrException.SetParameters(nameof(GetRedisData), redisKey, redisConfig?.GetLogStr(), ex));
//        return notFoundValue;
//      }
//    }
//    public static void SetRedisData<T>(string redisKey, T data, long expiredTimeInSeconds)
//    {
//      try
//      {
//        if (redisService.ConnectionStatus == EnumConnectionStatus.Error)
//        {
//          _logger.Error($"SetRedisData error, {redisKey}");
//          return;
//        }

//        lock (lockObj)
//        {
//          redisService.ConnectRedis(redisConfig);
//        }

//        redisService.SetValue(redisKey, data.ToJson(), expiredTimeInSeconds);
//      }
//      catch (Exception ex)
//      {
//        lock (lockObj)
//        {
//          redisService.ConnectionStatus = EnumConnectionStatus.Error;
//        }
//        _logger.LogMsg(Messages.ErrException.SetParameters(nameof(SetRedisData), redisKey, redisConfig?.GetLogStr(), ex));
//      }
//    }
//    public static void SetRedisDataInMilisecond<T>(string redisKey, T data, long expiredTimeInMiliseconds)
//    {
//      try
//      {
//        if (redisService.ConnectionStatus == EnumConnectionStatus.Error)
//        {
//          _logger.Error($"SetRedisData error, {redisKey}");
//          return;
//        }

//        lock (lockObj)
//        {
//          redisService.ConnectRedis(redisConfig);
//        }

//        redisService.SetValueInMilisecond(redisKey, data.ToJson(), expiredTimeInMiliseconds);
//      }
//      catch (Exception ex)
//      {
//        lock (lockObj)
//        {
//          redisService.ConnectionStatus = EnumConnectionStatus.Error;
//        }
//        _logger.LogMsg(Messages.ErrException.SetParameters(nameof(SetRedisDataInMilisecond), redisKey, redisConfig?.GetLogStr(), ex));
//      }
//    }
//    public static EnumConnectionStatus TestConnection()
//    {
//      try
//      {
//        if (redisService.ConnectionStatus == EnumConnectionStatus.Error)
//        {

//          lock (lockObj)
//          {
//            redisService.ConnectRedis(redisConfig, true);
//          }

//          return redisService.ConnectionStatus;
//        }
//      }
//      catch (Exception ex)
//      {
//        //_logger.LogMsg(Messages.ErrException.SetParameters(nameof(TestConnection), ex));
//      }
//      return redisService.ConnectionStatus;
//    }
//    public static void ClearRedisByKey(string redisKey)
//    {
//      try
//      {
//        if (redisService.ConnectionStatus == EnumConnectionStatus.Error)
//        {
//          return;
//        }

//        lock (lockObj)
//        {
//          redisService.ConnectRedis(redisConfig);
//        }

//        redisService.DeleteValue(redisKey);
//      }
//      catch (Exception ex)
//      {
//        _logger.LogMsg(Messages.ErrException.SetParameters(nameof(ClearRedisByKey), redisKey, redisConfig?.GetLogStr(), ex));
//      }
//    }
//    public static void ClearRedisByEntity(string entityName)
//    {
//      try
//      {
//        if (redisService.ConnectionStatus == EnumConnectionStatus.Error)
//        {
//          return;
//        }
//        lock (lockObj)
//        {
//          redisService.ConnectRedis(redisConfig);

//          if (dicClearByEntity.ContainsKey(entityName))
//          {
//            var listKey = dicClearByEntity[entityName];
//            foreach (var key in listKey)
//            {
//              redisService.DeleteValue(key);
//            }
//            dicClearByEntity.Remove(entityName);
//          }

//          foreach (var keyVal in dicClearWithAnyChange)
//          {
//            redisService.DeleteValue(keyVal.Key);
//          }
//          dicClearWithAnyChange.Clear();
//        }
//      }
//      catch (Exception ex)
//      {
//        _logger.LogMsg(Messages.ErrException.SetParameters(nameof(ClearRedisByEntity), entityName, redisConfig?.GetLogStr(), ex));
//      }
//    }
//    //public static void ClearRedisAll()
//    //{
//    //  try
//    //  {
//    //    redisService.ConnectRedis(redisConfig);
//    //    foreach(var keyVal in dicClearByEntity)
//    //    {
//    //      var listKey = keyVal.Value;
//    //      foreach (var key in listKey)
//    //      {
//    //        redisService.DeleteValue(key);
//    //      }
//    //    }
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    _logger.LogMsg(Messages.ErrException.SetParameters(nameof(ClearRedisAll), ex));
//    //  }
//    //}

//    public static void AddEntityCacheKey(string entityName, string cacheKey, bool isClearWithAnyChange)
//    {
//      try
//      {
//        if (redisService.ConnectionStatus == EnumConnectionStatus.Error)
//        {
//          return;
//        }

//        lock (lockObj)
//        {
//          if (dicClearByEntity.ContainsKey(entityName))
//          {
//            dicClearByEntity[entityName].Add(cacheKey);
//          }
//          else
//          {
//            dicClearByEntity.Add(entityName, new List<string>() { cacheKey });
//          }

//          if (isClearWithAnyChange)
//          {
//            if (!dicClearWithAnyChange.ContainsKey(cacheKey))
//            {
//              dicClearWithAnyChange.Add(cacheKey, true);
//            }
//          }
//          else
//          {

//          }
//        }
//      }
//      catch (Exception ex)
//      {
//        _logger.LogMsg(Messages.ErrException.SetParameters(nameof(AddEntityCacheKey), entityName, cacheKey, isClearWithAnyChange, redisConfig.GetLogStr(), ex));
//      }
//    }

//    public static bool RequireLockAndRetry(string keyLock, int expiredTimeInMilisecond, int retryNumber, int sleepInMiliseconds)
//    {
//      //Debug.WriteLine("RequireLockAndRetry, retryNumber: " + retryNumber+$" {sleepInMiliseconds}");
//      if (retryNumber < 0)
//      {
//        return false;
//      }

//      string? lockId = GetRedisData<string>(keyLock, null);
//      if (lockId != null)
//      {
//        Thread.Sleep(sleepInMiliseconds);
//        //Debug.WriteLine("Return false: " + retryNumber + $" {sleepInMiliseconds}");
//        return RequireLockAndRetry(keyLock, expiredTimeInMilisecond, retryNumber - 1, sleepInMiliseconds);
//      }

//      string newLockId = StringUtil.GenerateRandomKeyWithDateTime();
//      SetRedisDataInMilisecond<string>(keyLock, newLockId, expiredTimeInMilisecond);

//      lockId = GetRedisData<string>(keyLock, null);
//      if (lockId != newLockId)
//      {
//        Thread.Sleep(sleepInMiliseconds);
//        //Debug.WriteLine("Return false: " + retryNumber + $" {sleepInMiliseconds}");
//        return RequireLockAndRetry(keyLock, expiredTimeInMilisecond, retryNumber - 1, sleepInMiliseconds);
//      }

//      //Debug.WriteLine("Locked: " + keyLock);
//      return true;
//    }
//    public static void UnLock(string keyLock)
//    {
//      //Debug.WriteLine("Unlocked: " + keyLock);
//      ClearRedisByKey(keyLock);
//    }

//    public static bool IsCanRetry(string messageId, int expiredTimeInSeconds, int maxRetry)
//    {
//      string key = $"IsCanRetry_{messageId}";
//      int? retriedCount = GetRedisData<int?>(key, null);
//      if (retriedCount == null)
//      {
//        SetRedisData<int>(key, 1, expiredTimeInSeconds);
//      }
//      else
//      {
//        SetRedisData<int>(key, retriedCount.Value + 1, expiredTimeInSeconds);
//      }
//      if (retriedCount < maxRetry)
//      {
//        return true;
//      }
//      ClearRedisByKey(key);
//      return false;
//    }
//  }
//}
