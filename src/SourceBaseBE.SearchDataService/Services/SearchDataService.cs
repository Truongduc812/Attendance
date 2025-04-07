//using iSoft.Common;
//using iSoft.Common.Exceptions;
//using Newtonsoft.Json;
////using iSoft.ConnectionCommon.MessageBroker;
//using iSoft.Common.Cached;
////using iSoft.ConnectionCommon.MessageQueueNS;
//using SourceBaseBE.CommonFunc.MessageQueue;
//using iSoft.Common.ConfigsNS;
//using System.Collections.Generic;
//using System;
//using System.Threading.Tasks;
//using Microsoft.Data.SqlClient;
//using SourceBaseBE.Database.Repository.DeviceData;
//using iSoft.Common.Utils;
//using SourceBaseBE.Database.Entities.TraceData;
//using SourceBaseBE.CommonFunc.DataService;
//using SourceBaseBE.CommonFunc.EnvConfigData;
////using iSoft.ConnectionCommon.Model;
//using iSoft.Common.ExtensionMethods;
//using System.Threading;
//using iSoft.Redis.Services;
//using iSoft.Common.Models.ConfigModel.Subs;
//using Serilog;

//namespace SourceBaseBE.Main.SearchDataServiceNS.Services
//{
//    public class SearchDataService
//  {
//    private const int CONST_INTERVAL_DELETE_OLD_DATA_IN_SECONDS = 60 * 10;
//    private const int CONST_STORE_TIME_OLD_DATA_IN_SECONDS = 3600 * 24;

//    private readonly ILogger _logger = Serilog.Log.Logger;
//    //private DeviceDataSecondRepository deviceDataRepo_Second;
//    //private DeviceDataMinuteRepository deviceDataRepo_Minute;
//    private EnvironmentRepository environmentRepository;
//    private ConnectionRepository connectionRepository;

//    private static readonly object lockObjectHandleMessage = new object();
//    private static readonly List<Task> processingTasks = new List<Task>();
//    private MemCached cached = new MemCached(60);
//    //private IDBConnectionCustom connCus = null;
//    //private IDbConnection conn = null;
//    private DateTime lastDeleteOldData = DateTime.Now;
//    //private RedisService redisService;

//    private ImportDataFactory importDataFactory = null;

//    private object lockObj1 = new object();
//    private object lockObj2 = new object();
//    private object lockObj3 = new object();
//    private object lockObj4 = new object();
//    private object lockObj5 = new object();
//    Dictionary<string, DeviceConnectionEntity> dicConnEntity0 = new Dictionary<string, DeviceConnectionEntity>();

//    internal ServerConfigModel _redisConfig;

//    public SearchDataService()
//    {
//      _redisConfig = CommonConfig.GetConfig().RedisConfig;

//      CachedFunc.SetRedisConfig(_redisConfig);
//    }
//    public async Task Run()
//    {
//      string funcName = "Run";
//      this._logger.LogMsg(Messages.IFuncStart_0, funcName);

//      while (true)
//      {
//        try
//        {
//          Thread.Sleep(1000);
//          GC.Collect();

//          var envConfigData = EnvConfigData.Ins;
//          //this._logger.LogInformation("EnvConfig: {0}", envConfigData.GetListEnvConfigModel());

//          importDataFactory = new ImportDataFactory(null, null, envConfigData.GetListEnvConfigModel(), CommonConfig.GetConfig().ElasticSearchConfig);
          

//          try
//          {
//            await registerEventAsync();

//            await Task.Run(async () =>
//            {
//              while (true)
//              {
//                await ProcessPendingTasks();
//                await Task.Delay(500);
//              }
//            });
//          }
//          catch (SqlException ex)
//          {
//            throw ex;
//          }
//          catch (DBException ex)
//          {
//            throw ex;
//          }
//          catch (BaseException ex)
//          {
//            throw ex;
//          }
//          catch (Exception ex)
//          {
//            throw new BaseException(ex);
//          }

//        }
//        catch (SqlException ex)
//        {
//          this._logger.LogMsg(Messages.ErrBaseException.SetParameters(ex));
//          //Thread.Sleep(10000);
//        }
//        catch (DBException ex)
//        {
//          this._logger.LogMsg(Messages.ErrBaseException.SetParameters(ex));
//          //Thread.Sleep(10000);
//        }
//        catch (BaseException ex)
//        {
//          this._logger.LogMsg(Messages.ErrException.SetParameters(ex));
//          //Thread.Sleep(10000);
//        }
//        catch (Exception ex)
//        {
//          this._logger.LogMsg(Messages.ErrException.SetParameters(ex));
//          //Thread.Sleep(10000);
//        }
//        //finally
//        //{
//        //  //conn?.Close();
//        //  //conn = null;

//        //  //this.redisService?.CloseRedis();
//        //  //this.redisService = null;
//        //}
//      }
//    }

//    private async Task registerEventAsync()
//    {
//      string funcName = "registerEventAsync";
//      try
//      {
//        this._logger.LogMsg(Messages.IFuncStart_0, funcName);
//        await MessageQueue.Init(MessageQueueConfig.GetIMagQueueConfig(), CommonConfig.GetConfig().RabbitMQConfig);
//        await MessageQueue.Subscribe(TopicName.SearchDataTopic, this.handleMessage0, false);

//        this._logger.LogMsg(Messages.IFuncEnd_0, funcName);
//      }
//      catch (Exception ex)
//      {
//        this._logger.LogMsg(Messages.ErrException.SetParameters(funcName, ex));
//        throw new BaseException(ex);
//      }
//    }

//    private async Task handleMessage0(PayloadMessage payload)
//    {
//      var task = Task.Run(() => handleMessage(payload));

//      lock (lockObjectHandleMessage)
//      {
//        processingTasks.Add(task);
//      }
//    }

//    private async Task ProcessPendingTasks()
//    {
//      Task[] tasksCopy;

//      lock (lockObjectHandleMessage)
//      {
//        tasksCopy = processingTasks.ToArray();
//        processingTasks.Clear();
//      }

//      await Task.WhenAll(tasksCopy);
//    }
//    private async Task handleMessage(PayloadMessage payload)
//    {
//      string errMessage = "";
//      string dataJson = "";
//      string funcName = "handleMessage";
//      DevicePayloadMessage? message = null;
//      try
//      {
//        DateTime startTime = DateTime.Now;
//        message = payload.GetData<DevicePayloadMessage>(ref dataJson, ref errMessage);

//        if (message == null)
//        {
//          this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, $"err: message = null, {errMessage}, json: {dataJson}");
//          MessageQueue.DeleteMessage(payload);
//          return;
//        }
//        if (message.Data == null || message.Data.Count <= 0 || message.ConnectionId == null || message.MessageId == null)
//        {
//          this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, message.MessageId + ", " + $"json: {dataJson}");
//          MessageQueue.DeleteMessage(payload);
//          return;
//        }

//        //await MessageQueue.RetryMessage(message.MessageId, payload, message);
//        //return;

//        if (!cached.ContainsKey($"log_es_message_seconds_10m_all"))
//        {
//          cached.AddToCache($"log_es_message_seconds_10m_all", true, 60 * 10);
//          this._logger.LogMsg(Messages.IBegin_0_1, $"[searchdata10m] All", message.MessageId + ", message: " + message.ToJson());
//        }

//        //if (message.Data.Length > ConstDatabase.TraceDataMaxEnvironment)
//        //{
//        //  this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, message.MessageId + ", " + $"err: message.Data.Length error, length: {message.Data.Length}, json: {dataJson}");
//        //  MessageQueue.DeleteMessage(payload);
//        //  return;
//        //}

//        //QueueProperties queueProperties = MessageQueueConfig.GetQueueProperties(payload.QueueName);
//        //lock (lockObj2)
//        //{
//        //  // Check duplicate
//        //  if (cached.ContainsKey(ConstantLock.lockSearchDataService2KeyPrefix + message.MessageId.ToString()))
//        //  {
//        //    this._logger.LogMsg(Messages.ErrDuplicateItem_0_1, funcName, message);
//        //    MessageQueue.DeleteMessage(payload);
//        //    return;
//        //  }
//        //  cached.AddToCache(ConstantLock.lockSearchDataService2KeyPrefix + message.MessageId.ToString(), true, queueProperties.TimeRetryInSeconds - 1);
//        //}

//        await importDataFactory.ImportESData(message, "_main");

//        MessageQueue.AckMessage(payload);
//        this._logger.LogMsg(Messages.ISuccess_0_1, funcName, message.MessageId + ", " + DateTimeUtil.GetHumanStr(DateTime.Now - startTime));
//        return;
//      }
//      catch (JsonReaderException ex)
//      {
//        this._logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
//        MessageQueue.DeleteMessage(payload);
//      }
//      catch (RetryException ex)
//      {
//        this._logger.Warning($"RetryException, {message.MessageId}, " + ex.Message);
//        await MessageQueue.RetryMessage(message.MessageId, payload, message);
//      }
//      catch (DBException ex)
//      {
//        this._logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
//        await MessageQueue.RetryMessage(message.MessageId, payload, message);
//        throw ex;
//      }
//      catch (CriticalException ex)
//      {
//        this._logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
//        await MessageQueue.RetryMessage(message.MessageId, payload, message);
//        throw ex;
//      }
//      catch (BaseException ex)
//      {
//        this._logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
//        await MessageQueue.RetryMessage(message.MessageId, payload, message);
//        throw ex;
//      }
//      catch (Exception ex)
//      {
//        this._logger.LogMsg(Messages.ErrException.SetParameters($"json: {dataJson}", ex));
//        await MessageQueue.RetryMessage(message.MessageId, payload, message);
//        throw ex;
//      }

//      //if (conn == null || conn.State != ConnectionState.Open)
//      //{
//      //  throw new DBException("Connection is closed");
//      //}

//    }
//  }
//}