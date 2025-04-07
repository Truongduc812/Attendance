using iSoft.Common.Enums;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Common.Utils;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace iSoft.Redis.Services
{
  public class RedisService : IDisposable
  {
    ConnectionMultiplexer redis;
    IDatabase db;
    public EnumConnectionStatus ConnectionStatus = EnumConnectionStatus.None;
    public string _connectionString;
    public static string GetConnectionString(ServerConfigModel config)
    {
      return $"{config.Address}:{config.Port}, ConnectTimeout = 1000";
    }
    public void ConnectRedis(ServerConfigModel redisConfig, bool forceFlag = false)
    {
      this._connectionString = GetConnectionString(redisConfig);
      if (redisConfig == null)
      {
        throw new BaseException($"ConnectRedis error, config = null");
      }

      if (this.ConnectionStatus == EnumConnectionStatus.Error && forceFlag == false)
      {
        throw new BaseException($"ConnectRedis error, ConnectionStatus = Error");
      }

      if (ConnectionStatus != EnumConnectionStatus.Connected)
      {
        try
        {
          redis = ConnectionMultiplexer.Connect(this._connectionString);
          db = redis.GetDatabase();
          ConnectionStatus = EnumConnectionStatus.Connected;
        }
        catch (Exception ex)
        {
          this.ConnectionStatus = EnumConnectionStatus.Error;
          throw new BaseException(ex);
        }
      }
    }
    public void CloseRedis()
    {
      try
      {
        redis.Close();
        redis = null;
        db = null;
        ConnectionStatus = EnumConnectionStatus.Disconnected;
      }
      catch (Exception ex)
      {
        ConnectionStatus = EnumConnectionStatus.Error;
      }
    }

    /// <summary>
    /// SetValue
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiredTimeInSeconds"></param>
    /// <returns>true: ok, false: error</returns>
    public bool SetValue(string key, string value, long expiredTimeInSeconds)
    {
      if (db != null)
      {
        var data = EncodeUtil.CompressString(value);
        return db.StringSet(key, data, TimeSpan.FromSeconds(expiredTimeInSeconds));
      }
      return false;
    }

    /// <summary>
    /// SetValueInMilisecond
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiredTimeInMiliseconds"></param>
    /// <returns>true: ok, false: error</returns>
    public bool SetValueInMilisecond(string key, string value, long expiredTimeInMiliseconds)
    {
      if (db != null)
      {
        var data = EncodeUtil.CompressString(value);
        return db.StringSet(key, data, TimeSpan.FromMilliseconds(expiredTimeInMiliseconds));
      }
      return false;
    }

    /// <summary>
    /// GetValue
    /// </summary>
    /// <param name="key"></param>
    /// <returns>Return null if not exists</returns>
    public string? GetValue(string key)
    {
      if (db != null)
      {
        var compressedValue = db.StringGet(key);
        if (compressedValue.IsNullOrEmpty)
        {
          return null;
        }
        var compressedBytes = (byte[])compressedValue;
        return EncodeUtil.DecompressString(compressedBytes);
      }
      return null;
    }

    /// <summary>
    /// DeleteValue
    /// </summary>
    /// <param name="key"></param>
    /// <returns>true: deleted, false: Key does not exist or was not deleted.</returns>
    public bool DeleteValue(string key)
    {
      if (db != null)
      {
        return db.KeyDelete(key);
      }
      return false;
    }

    public void Dispose()
    {
      CloseRedis();
    }
  }
}