using iSoft.Common;
using iSoft.Common.Cached;
using iSoft.Common.Enums;
using iSoft.Common.Lock;
using iSoft.Common.Utils;
using iSoft.DBLibrary.DBConnections.Interfaces;
using System.Data;
using SourceBaseBE.CommonFunc.EnvConfigData;
using iSoft.ElasticSearch.Services;
using System.Text.RegularExpressions;
using iSoft.Common.Enums.DBProvider;
//using iSoft.ConnectionCommon.Model;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models.ConfigModel.Subs;
using Serilog;
using iSoft.Redis.Services;
using iSoft.Common.Exceptions;
using iSoft.RabbitMQ.Payload;

namespace SourceBaseBE.CommonFunc.DataService
{
  public class ImportDataFactory
  {
    private MemCached cached = new MemCached(60);
    private readonly ILogger _logger = Log.Logger;
    private IDbConnection conn = null;
    private IDBConnectionCustom connCus = null;
    private ESRestfullAPIService esAPIService = null;

    public List<EnvConfigModel> arrExampleEnv;

    public Dictionary<string, EnvConfigModel> dicEnvKey2Config = new Dictionary<string, EnvConfigModel>();
    //Dictionary<string, object> DicEnvKey2Value = new Dictionary<string, object>();

    Dictionary<string, LastData> dicTableName2LastData = new Dictionary<string, LastData>();
    Dictionary<string, LastData> dicIndexName2LastData = new Dictionary<string, LastData>();
    private static object lockObj1 = new object();
    public ImportDataFactory(IDbConnection conn,
                            IDBConnectionCustom connCus,
                            List<EnvConfigModel> arrExampleEnv,
                            ServerConfigModel esConfig)
    {
      this.conn = conn;
      this.connCus = connCus;
      this.arrExampleEnv = arrExampleEnv;

      foreach (var envObj in this.arrExampleEnv)
      {
        if (!this.dicEnvKey2Config.ContainsKey(envObj.GetKey()))
        {
          this.dicEnvKey2Config.Add(envObj.GetKey(), envObj);
        }
        else
        {
          this._logger.Warning($"[ImportDataFactory] EnviromentVarName is duplicate: {envObj.EnviromentVarName}");
        }
      }
      this.esAPIService = new ESRestfullAPIService(esConfig);
    }
    public void ImportTraceData(DevicePayloadMessage message, EnumDBProvider databaseType)
    {
      saveTraceData(message, databaseType);
    }

    private void saveTraceData(DevicePayloadMessage message, EnumDBProvider databaseType)
    {
      // message = PreProcessMessage(message);

      Dictionary<string, TraceDataTableObj> listTable = getListTraceDataTableObj(message);

      string keyLock = $"saveTraceData_{message.MessageId}";
      if (CachedFunc.RequireLockAndRetry(keyLock, 1000, 12, 8))
      {
        try
        {
          lock (Lock.lockObj_RunSQLServer)
          {
            //IDbTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            foreach (var key in listTable.Keys)
            {
              var val = listTable[key];
              try
              {
                //if (!cachedLock.ContainsKey($"log_trace_message_seconds_30m_{val.TableName}"))
                //{
                //  cachedLock.AddToCache($"log_trace_message_seconds_30m_{val.TableName}", true, 60 * 30);
                //  this._logger.LogMsg(Messages.IBegin_0_1, $"SaveTraceData, {val.TableName}", message.MessageId + ", message: " + message.ToJson());
                //}
                //else
                {
                  if (!cached.ContainsKey($"log_trace_message_seconds_5m_{val.TableName}"))
                  {
                    cached.AddToCache($"log_trace_message_seconds_5m_{val.TableName}", true, 60 * 5);
                    this._logger.LogMsg(Messages.IBegin_0_1, $"[tracedata5m], {val.TableName}", message.MessageId + ", values: " + val.DicEnvKey2Value.ToJson());
                  }
                }
                var ret = val.SaveTraceData(this.conn, /*transaction,*/ this.connCus, ref dicTableName2LastData, databaseType);
                if (ret == EnumFuncResult.Error)
                {
                  //try
                  //{
                  //  transaction.Rollback();
                  //}
                  //catch { }
                  return;
                }
              }
              catch (Exception ex)
              {
                //try
                //{
                //  transaction.Rollback();
                //  _logger.LogMsg(Messages.ErrDBException, "", $"[saveTraceData] transaction.Rollback(); at {val.TableName}");
                //}
                //catch { }
                foreach (var key2 in listTable.Keys)
                {
                  var val2 = listTable[key2];
                  if (val2.isExistsTable(conn, databaseType) == EnumFuncResult.Ok)
                  {
                    if (!dicTableName2LastData.ContainsKey(val2.TableName))
                    {
                      dicTableName2LastData.Add(val2.TableName, new LastData());
                    }
                  }
                  else
                  {
                    dicTableName2LastData.Remove(val2.TableName);
                  }
                }

                throw ex;
              }
            }
            //transaction.Commit();
          }
        }
        finally
        {
          CachedFunc.UnLock(keyLock);
        }
      }
      else
      {
        this._logger.LogInformation($"[saveTraceData] Check locked item (Duplicate), MessageId: {message.MessageId}");
        throw new RetryException($"[saveTraceData] Check locked item (Duplicate), MessageId: {message.MessageId}");
      }
    }
    public async Task ImportESData(DevicePayloadMessage message, string esSubfix = "")
    {
      await pushESData(message, esSubfix);
    }

    private async Task pushESData(DevicePayloadMessage message, string esSubfix = "")
    {
      message = PreProcessMessage(message);
      Dictionary<string, TraceDataTableObj> listTable = getListTraceDataTableObj(message, esSubfix);
      foreach (var keyVal in listTable)
      {
        //if (!cachedLock.ContainsKey($"log_es_message_seconds_10m_{keyVal.Value.TableName}"))
        //{
        //  cachedLock.AddToCache($"log_es_message_seconds_10m_{keyVal.Value.TableName}", true, 60 * 10);
        //  this._logger.LogMsg(Messages.IBegin_0_1, $"SaveESData 10m, {keyVal.Value.TableName}", message.MessageId + ", message: " + message.ToJson());
        //}
        //else
        {
          if (!cached.ContainsKey($"log_es_message_seconds_5m_{keyVal.Value.TableName}"))
          {
            cached.AddToCache($"log_es_message_seconds_5m_{keyVal.Value.TableName}", true, 60 * 5);
            this._logger.LogMsg(Messages.IBegin_0_1, $"[searchdata5m], {keyVal.Value.TableName}", message.MessageId + ", values: " + keyVal.Value.DicEnvKey2Value.ToJson());
          }
        }
        var ret = await keyVal.Value.SaveESData(this.esAPIService, dicIndexName2LastData);
        if (ret == EnumFuncResult.Error)
        {
          _logger.LogInformation($"[pushESData] SaveESData error, {message.ToJson()}");
        }
      }
    }

    private DevicePayloadMessage PreProcessMessage(DevicePayloadMessage message)
    {
      //EnvData env = null;
      //Dictionary<string, EnvData> dicEnvKey2Env = new Dictionary<string, EnvData>();

      //for (int i = 0; i < message.Data.Count; i++)
      //{
      //  env = message.Data[i];
      //  string fixedEnvKey = StringUtil.ConvertToESField(env.Key);
      //  if (!dicEnvKey2Env.ContainsKey(fixedEnvKey))
      //  {
      //    dicEnvKey2Env.Add(fixedEnvKey, message.Data[i]);
      //  }
      //}
      
      //// gvl_env_factories_1__workshops_1__lines_1__machines_1__conveyors_1__motors_1__current_in_a
      //message = PassingDataEnvByRegex(message, dicEnvKey2Env, @"[a-zA-Z0-9_]*_motors_[\d]+__speed_in_hz", "SPEED_IN_HZ", "STATUS", PassingDataMotorStatus);

      return message;
    }

    ///// <summary>
    ///// PassingDataEnvByRegex
    ///// </summary>
    ///// <param name="message"></param>
    ///// <param name="dicEnvKey2Env"></param>
    ///// <param name="patternSource"></param>
    ///// <param name="patternDeskSearch">ex: @"[^a-zA-Z0-9_]"</param>
    ///// <param name="patternDeskReplace">"_"</param>
    ///// <param name="passingDataMotorStatus"></param>
    ///// <returns></returns>
    //private DevicePayloadMessage PassingDataEnvByRegex(DevicePayloadMessage message,
    //  Dictionary<string, EnvData> dicEnvKey2Env,
    //  string patternSource,
    //  string patternDeskSearch,
    //  string patternDeskReplace,
    //  Func<double, double> passingDataMotorStatus)
    //{
    //  EnvData env = null;
    //  EnvData env_source = null;
    //  EnvData env_desk = null;
    //  double deskValue = 0.0;
    //  double? sourceValue = 0.0;
    //  string deskEnv = "";
    //  string fixedDeskEnv = "";

    //  for (int i = 0; i < message.Data.Count; i++)
    //  {
    //    env_source = null;
    //    env_desk = null;
    //    deskEnv = "";
    //    env = message.Data[i];
    //    string fixedEnvKey = StringUtil.ConvertToESField(env.Key);
    //    if (fixedEnvKey != null)
    //    {
    //      // gvl_env_factories_1__workshops_1__lines_1__machines_1__conveyors_1__motors_1__current_in_a
    //      if (StringUtil.IsMatch(fixedEnvKey, patternSource))
    //      {
    //        env_source = message.Data[i];
    //        deskEnv = Regex.Replace(env.Key, patternDeskSearch, patternDeskReplace);
    //        fixedDeskEnv = StringUtil.ConvertToESField(deskEnv);
    //        if (dicEnvKey2Env.ContainsKey(fixedDeskEnv))
    //        {
    //          env_desk = dicEnvKey2Env[fixedDeskEnv];
    //        }
    //      }
    //    }

    //    if (env_source != null)
    //    {
    //      sourceValue = ConvertUtil.ConvertToNullableDouble(env_source.Value);
    //      if (sourceValue != null)
    //      {
    //        deskValue = passingDataMotorStatus(sourceValue.Value);

    //        if (env_desk != null)
    //        {
    //          env_desk.Value = deskValue.ToString();
    //        }
    //        else
    //        {
    //          message.Data.Append(new EnvData(deskEnv, deskValue.ToString()));
    //        }
    //      }
    //    }
    //  }

    //  return message;
    //}

    private double PassingDataMotorStatus(double speed)
    {
      if (speed >= 1)
      {
        return 1;
      }
      return 0;
    }

    private Dictionary<string, TraceDataTableObj> getListTraceDataTableObj(DevicePayloadMessage message, string esSubfix = "")
    {
      Dictionary<string, TraceDataTableObj> dicName2TableObj = new Dictionary<string, TraceDataTableObj>();

      EnvConfigModel envConfig = null;
      foreach (var keyVal in this.dicEnvKey2Config)
      {
        envConfig = keyVal.Value;
        if (!dicName2TableObj.ContainsKey(envConfig.TableSerial))
        {
          var subDicEnvKey2Config = dicEnvKey2Config.Where(pair => pair.Value.TableSerial == envConfig.TableSerial)
                                                      .ToDictionary(pair => pair.Key, pair => pair.Value);


          Dictionary<string, object> subDicEnvKey2Value = new Dictionary<string, object>();
          foreach (var env in message.Data)
          {
            string fixedEnvKey = StringUtil.ConvertToESField(env.Key, message.ConnectionId);
            if (subDicEnvKey2Config.ContainsKey(fixedEnvKey))
            {
              if (!subDicEnvKey2Value.ContainsKey(fixedEnvKey))
              {
                subDicEnvKey2Value.Add(fixedEnvKey, env.Value);
              }
              else
              {
                this._logger.Warning($"[ImportDataFactory] [GetListTraceDataTableObj] EnviromentVarName is duplicate: {env.Key}");
              }
            }
          }

          //var subDicEnvKey2Value = DicEnvKey2Value.Where(pair => subDicEnvKey2Config.ContainsKey(pair.Key))
          //                                          .ToDictionary(pair => pair.Key, pair => pair.Value);

          if (subDicEnvKey2Value.Count >= 1)
          {
            TraceDataTableObj tableObj = new TraceDataTableObj(
              envConfig.TableSerial,
              envConfig.GetESIndexName(esSubfix),
              envConfig.GetESPatternSearch(esSubfix),
              envConfig.GetESPatternSearch(),
              subDicEnvKey2Config,
              subDicEnvKey2Value,
              message);
            dicName2TableObj.Add(envConfig.TableSerial, tableObj);
          }
        }
      }

      return dicName2TableObj;
    }

    public Dictionary<string, TraceDataTableObj> GetListTraceDataTableObj()
    {
      Dictionary<string, TraceDataTableObj> dicName2TableObj = new Dictionary<string, TraceDataTableObj>();

      EnvConfigModel envConfig = null;
      foreach (var keyVal in this.dicEnvKey2Config)
      {
        envConfig = keyVal.Value;
        if (!dicName2TableObj.ContainsKey(envConfig.TableSerial))
        {
          var subDicEnvKey2Config = dicEnvKey2Config.Where(pair => pair.Value.TableSerial == envConfig.TableSerial)
                                                      .ToDictionary(pair => pair.Key, pair => pair.Value);

          TraceDataTableObj tableObj = new TraceDataTableObj(
            envConfig.TableSerial,
            envConfig.GetESIndexName(),
            envConfig.GetESPatternSearch(),
            envConfig.GetESPatternSearch(),
            subDicEnvKey2Config);
          dicName2TableObj.Add(envConfig.TableSerial, tableObj);
        }
      }

      return dicName2TableObj;
    }

    public async Task<Dictionary<string, DevicePayloadMessage>> GetTraceData(ESRestfullAPIService esAPIService,
      Dictionary<string, TraceDataTableObj> listTable,
      DateTime dateTimeFrom,
      DateTime dateTimeTo,
      EnumDBProvider databaseType)
    {
      Dictionary<string, DevicePayloadMessage> dicMessageId2Message = new Dictionary<string, DevicePayloadMessage>();
      foreach (var key in listTable.Keys)
      {
        var val = listTable[key];
        try
        {
          await val.GetTraceData(this.conn, this.connCus, esAPIService, dateTimeFrom, dateTimeTo, dicMessageId2Message, databaseType);
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
      return dicMessageId2Message;
    }
  }
}
