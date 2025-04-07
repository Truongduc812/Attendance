//using System.Data;
//using iSoft.DBLibrary.DBConnections.Factory;
//using iSoft.Common;
//using iSoft.Common.Exceptions;
//using Newtonsoft.Json;
//using iSoft.ConnectionCommon.MessageBroker;
//using iSoft.Common.Cached;
//using iSoft.ConnectionCommon.MessageQueueNS;
//using SourceBaseBE.CommonFunc.MessageQueue;
//using SourceBaseBE.Database.DBContexts;
//using iSoft.DBLibrary.SQLBuilder;
//using iSoft.DBLibrary.DBConnections.Interfaces;
//using iSoft.Common.ConfigsNS;
//using System.Collections.Generic;
//using System;
//using System.Threading.Tasks;
//using System.Threading;
//using Microsoft.Data.SqlClient;
//using SourceBaseBE.Database.Repository.TraceDataNS;
//using SourceBaseBE.Database.Entities.TraceData;
//using System.Linq;
//using iSoft.Common.Utils;
//using SourceBaseBE.CommonFunc.DataService;
//using SourceBaseBE.CommonFunc.EnvConfigData;
////using iSoft.ConnectionCommon.Model;
//using iSoft.Common.ExtensionMethods;
//using Serilog;
//using iSoft.Redis.Services;
//using iSoft.Common.Models.ConfigModel.Subs;
//using iSoft.RabbitMQ.Payload;

//namespace SourceBaseBE.Main.TraceDataService.Services
//{
//    public partial class TraceDataService
//	{
//    private readonly CommonDBContext _context;

//    private const int CONST_INTERVAL_DELETE_OLD_DATA_IN_SECONDS = 60 * 10;
//		private const int CONST_STORE_TIME_OLD_DATA_IN_SECONDS = 3600 * 24;

//		private readonly ILogger _logger = Serilog.Log.Logger;
//		private TraceEnvironmentRepository environmentRepository;
//		private TraceConnectionRepository connectionRepository;

//		private static readonly object lockObjectHandleMessage = new object();
//		private static readonly List<Task> processingTasks = new List<Task>();
//		private MemCached cached = new MemCached(60);
//		private IDBConnectionCustom connCus = null;
//		private IDbConnection conn = null;
//		private DateTime lastDeleteOldData = DateTime.Now;
//		//private RedisService redisService;

//		private ImportDataFactory importDataFactory = null;

//		private object lockObj1 = new object();
//		private object lockObj2 = new object();
//		private object lockObj3 = new object();
//		private object lockObj4 = new object();
//		private object lockObj5 = new object();

//		Dictionary<long, DeviceConnectionEntity> dicConnEntity0 = new Dictionary<long, DeviceConnectionEntity>();

//    internal ServerConfigModel _redisConfig;

//    public TraceDataService()
//    {

//      _redisConfig = CommonConfig.GetConfig().RedisConfig;

//      CachedFunc.SetRedisConfig(_redisConfig);
//    }
//		public async void Run()
//		{
//			string funcName = "Run";
//			this._logger.LogMsg(Messages.IFuncStart_0, funcName);

//			while (true)
//			{
//				try
//        {
//          Thread.Sleep(1000);
//          GC.Collect();

//          if (conn == null || conn.State == System.Data.ConnectionState.Closed)
//					{
//						this._logger.LogInformation("Try connect db ...");
//						var dbConfig = CommonConfig.GetConfig().TraceDatabaseConfig;

//						try
//            {
//              connCus = DBConnectionFactory.CreateDBConnection(dbConfig);
//              conn = connCus.GetConnection();
//              environmentRepository = new TraceEnvironmentRepository(BaseSQLBuilder.GetSQLBuilderInstance(dbConfig.DatabaseType), new TraceDBContext(connCus));
//              connectionRepository = new TraceConnectionRepository(BaseSQLBuilder.GetSQLBuilderInstance(dbConfig.DatabaseType), new TraceDBContext(connCus));
//              conn.Open();
//            }
//            catch (Exception ex)
//						{
//							throw new DBException(ex);
//            }
//            this._logger.LogInformation("Connection opened successfully.");

//            var envConfigData = EnvConfigData.Ins;
//            this._logger.Information("EnvConfig: {0}", envConfigData.GetListEnvConfigModel());

//            importDataFactory = new ImportDataFactory(conn, connCus, envConfigData.GetListEnvConfigModel(), null);
//					}

//					//if (this.redisService == null)
//					//{
//					//  this.redisService = new RedisService();
//					//  this.redisService.ConnectRedis(CommonConfig.GetConfig().RedisConfig);
//					//  this._logger.LogInformation("ConnectRedis successfully.");
//					//}

//					try
//					{
//						await registerEventAsync();

//						await Task.Run(async () =>
//						{
//							while (true)
//							{
//								await ProcessPendingTasks();
//								await Task.Delay(500);
//							}
//						});
//					}
//					catch (SqlException ex)
//					{
//						throw ex;
//					}
//					catch (DBException ex)
//					{
//						throw ex;
//					}
//					catch (BaseException ex)
//					{
//						throw ex;
//					}
//					catch (Exception ex)
//					{
//						throw new BaseException(ex);
//					}

//				}
//				catch (SqlException ex)
//				{
//					this._logger.LogMsg(Messages.ErrBaseException.SetParameters(ex));
//					try
//					{
//						var result = await TraceDBContext.CreateDatabase(connCus);
//						if (!result)
//						{
//							Thread.Sleep(10000);
//						}
//					}
//					catch (Exception ex2)
//					{
//						this._logger.LogMsg(Messages.ErrBaseException.SetParameters(ex2));
//            Thread.Sleep(10000);
//					}
//				}
//				catch (DBException ex)
//				{
//					this._logger.LogMsg(Messages.ErrBaseException.SetParameters(ex));
//					try
//					{
//						var result = await TraceDBContext.CreateDatabase(connCus);
//						if (!result)
//						{
//							Thread.Sleep(10000);
//						}
//					}
//					catch (Exception ex2)
//          {
//            this._logger.LogMsg(Messages.ErrBaseException.SetParameters(ex2));
//            Thread.Sleep(10000);
//					}
//				}
//				catch (BaseException ex)
//				{
//					this._logger.LogMsg(Messages.ErrException.SetParameters(ex));
//					Thread.Sleep(10000);
//				}
//				catch (Exception ex)
//				{
//					this._logger.LogMsg(Messages.ErrException.SetParameters(ex));
//					Thread.Sleep(10000);
//				}
//				finally
//				{
//					try
//					{
//						conn?.Close();
//						conn = null;
//					}
//					catch { }

//					//this.redisService?.CloseRedis();
//					//this.redisService = null;
//				}
//			}
//		}

//		private async Task registerEventAsync()
//		{
//			string funcName = "registerEventAsync";
//			try
//			{
//				this._logger.LogMsg(Messages.IFuncStart_0, funcName);
//				await MessageQueue.Init(MessageQueueConfig.GetIMagQueueConfig(), CommonConfig.GetConfig().RabbitMQConfig);
//				await MessageQueue.Subscribe(TopicName.TraceDataTopic, this.handleMessage0, false);

//				this._logger.LogMsg(Messages.IFuncEnd_0, funcName);
//			}
//			catch (Exception ex)
//			{
//				this._logger.LogMsg(Messages.ErrException.SetParameters(funcName, ex));
//				throw new BaseException(ex);
//			}
//		}

//		private async Task handleMessage0(PayloadMessage payload)
//		{
//			var task = Task.Run(() => handleMessage(payload));

//			lock (lockObjectHandleMessage)
//			{
//				processingTasks.Add(task);
//			}
//		}

//		private async Task ProcessPendingTasks()
//		{
//			Task[] tasksCopy;

//			lock (lockObjectHandleMessage)
//			{
//				tasksCopy = processingTasks.ToArray();
//				processingTasks.Clear();
//			}

//			await Task.WhenAll(tasksCopy);
//		}
//		private async Task handleMessage(PayloadMessage payload)
//		{
//			string errMessage = "";
//			string dataJson = "";
//			string funcName = "handleMessage";
//			DevicePayloadMessage? message = null;
//			try
//      {
//        DateTime startTime = DateTime.Now;
//				message = payload.GetData<DevicePayloadMessage>(ref dataJson, ref errMessage);

//				if (message == null)
//				{
//					this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, $"err: message = null, {errMessage}, json: {dataJson}");
//					MessageQueue.DeleteMessage(payload);
//					return;
//				}
//				if (message.Data == null || message.Data.Count <= 0 || message.ConnectionId == null || message.MessageId == null)
//				{
//					this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, message.MessageId + ", " + $"json: {dataJson}");
//					MessageQueue.DeleteMessage(payload);
//					return;
//        }

//        if (!cached.ContainsKey($"log_trace_message_seconds_10m_all"))
//        {
//          cached.AddToCache($"log_trace_message_seconds_10m_all", true, 60 * 10);
//          this._logger.LogMsg(Messages.IBegin_0_1, $"[tracedata10m] All", message.MessageId + ", message: " + message.ToJson());
//        }

//        //if (message.Data.Length > ConstDatabase.TraceDataMaxEnvironment)
//        //{
//        //  this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, message.MessageId + ", " + $"err: message.Data.Length error, length: {message.Data.Length}, json: {dataJson}");
//        //  MessageQueue.DeleteMessage(payload);
//        //  return;
//        //}

//        //lock (lockObj1)
//        //{
//        //  if (!cached.ContainsKey("TraceDataService_handleMessage_seconds_30"))
//        //  {
//        //    cached.AddToCache("TraceDataService_handleMessage_seconds_30", true, 30);
//        //    this._logger.LogMsg(Messages.IBegin_0_1, funcName, message.MessageId + ", " + message.ToJson());
//        //  }
//        //}

//        /*****************************************************************/
//        /* Insert to DB DeviceConnections - START
//        /*****************************************************************/
//        Dictionary<long, DeviceConnectionEntity> dicConnEntity = new Dictionary<long, DeviceConnectionEntity>();
//				foreach (var env in message.Data)
//				{
//					if (!dicConnEntity0.ContainsKey(message.ConnectionId))
//					{
//						if (!dicConnEntity.ContainsKey(message.ConnectionId))
//						{
//							dicConnEntity.Add(message.ConnectionId, new DeviceConnectionEntity(message.ConnectionId));
//						}
//					}
//				}
//				if (dicConnEntity.Count >= 1)
//				{
//					var listConnEntity = connectionRepository.InsertIfNotExists(dicConnEntity.Values.ToList(), conn);
//					foreach (var connEntity in listConnEntity)
//					{
//						if (!dicConnEntity0.ContainsKey(connEntity.ConnectionKey))
//						{
//							dicConnEntity0.Add(connEntity.ConnectionKey, connEntity);
//						}
//					}
//				}

//				if (!dicConnEntity0.ContainsKey(message.ConnectionId))
//				{
//					this._logger.LogMsg(Messages.ErrNotFound_0_1, funcName, message.MessageId + ", " + $"message.ConnectionId: {message.ConnectionId}");
//          await MessageQueue.RetryMessage(message.MessageId, payload, message);
//          return;
//				}
//				// Insert to DB DeviceConnections - END

//				//QueueProperties queueProperties = MessageQueueConfig.GetQueueProperties(payload.QueueName);
//				//lock (lockObj2)
//				//{
//				//  // Check duplicate
//				//  if (cached.ContainsKey(ConstantLock.lockDataSwitchingKeyPrefix + message.MessageId.ToString()))
//				//  {
//				//    this._logger.LogMsg(Messages.ErrDuplicateItem_0_1, funcName, message);
//				//    MessageQueue.DeleteMessage(payload);
//				//    return;
//				//  }
//				//  cached.AddToCache(ConstantLock.lockDataSwitchingKeyPrefix + message.MessageId.ToString(), true, queueProperties.TimeRetryInSeconds - 1);
//				//}

//				importDataFactory.ImportTraceData(message, CommonConfig.GetConfig().TraceDatabaseConfig.DatabaseType);

//				MessageQueue.AckMessage(payload);
//				this._logger.LogMsg(Messages.ISuccess_0_1, funcName, message.MessageId + ", " + DateTimeUtil.GetHumanStr(DateTime.Now - startTime));
//				return;
//			}
//			catch (JsonReaderException ex)
//			{
//				this._logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
//				MessageQueue.DeleteMessage(payload);
//      }
//      catch (RetryException ex)
//      {
//        this._logger.Warning($"RetryException, {message.MessageId}, " + ex.Message);
//        await MessageQueue.RetryMessage(message.MessageId, payload, message);
//      }
//      catch (DBException ex)
//			{
//				this._logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
//				await MessageQueue.RetryMessage(message.MessageId, payload, message);
//				throw ex;
//			}
//			catch (CriticalException ex)
//			{
//				this._logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
//        await MessageQueue.RetryMessage(message.MessageId, payload, message);
//				throw ex;
//			}
//			catch (BaseException ex)
//			{
//				this._logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
//        await MessageQueue.RetryMessage(message.MessageId, payload, message);
//				throw ex;
//			}
//			catch (Exception ex)
//			{
//				this._logger.LogMsg(Messages.ErrException.SetParameters($"json: {dataJson}", ex));
//        await MessageQueue.RetryMessage(message.MessageId, payload, message);
//				throw ex;
//			}

//			if (conn == null || conn.State != ConnectionState.Open)
//			{
//				throw new DBException("Connection is closed");
//			}

//		}
//	}
//}