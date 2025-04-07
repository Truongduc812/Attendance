using iSoft.Common.ConfigsNS;
using iSoft.Common.Exceptions;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Utils;
using iSoft.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using iSoft.RabbitMQ.Payload;
using System.Threading;
using SourceBaseBE.Database.Entities.TraceData;
using SourceBaseBE.Database.DBContexts;
using iSoft.DBLibrary.DBConnections.Factory;
using SourceBaseBE.CommonFunc.DataService;
using SourceBaseBE.CommonFunc.EnvConfigData;
using iSoft.Common.Enums;
using Npgsql;
using System.Data.SqlClient;
using iSoft.Redis.Services;
using iSoft.Common.Services;
using iSoft.InfluxDB.Services;

namespace iSoft.RabbitMQ.Services
{
  public class TraceDataConsumerHostedService : ConsumerHostedService
  {
    private static object lockObj_initDB = new object();
    private IDbConnection conn = null;
    private ImportDataFactory importDataFactory = null;
    public TraceDBContext _dbContext;
    Dictionary<long, DeviceConnectionEntity> dicConnEntity0 = new Dictionary<long, DeviceConnectionEntity>();
    //private TraceConnectionRepository _connectionRepository;

    public EnumConnectionStatus DBConnectionStatus = EnumConnectionStatus.None;

    public TraceDataConsumerHostedService(
      ILogger<ConsumerHostedService> logger,
      RabbitMQService rabbitMQService,
      string queueName)
      : base(logger, rabbitMQService, queueName)
    {
      lock (lockObj_initDB)
      {
        DBConnectionStatus = initDB();
      }
    }

    private EnumConnectionStatus initDB()
    {
      try
      {
        try
        {
          _logger.LogInformation($"*** TRY CONNECT DATABASE *** {CommonConfig.GetConfig().TraceDatabaseConfig.GetHostName()}");

          CachedFunc.SetRedisConfig(CommonConfig.GetConfig().RedisConfig);

          var dbConfig = CommonConfig.GetConfig().TraceDatabaseConfig;
          var connCus = DBConnectionFactory.CreateDBConnection(dbConfig);
          this.conn = connCus.GetConnection();
          this.conn.Open();

          var envConfigData = EnvConfigData.Ins;
          importDataFactory = new ImportDataFactory(conn, connCus, envConfigData.GetListEnvConfigModel(), null);
          _logger.LogInformation("*** CONNECT DATABASE SUCCESS ***");
          return EnumConnectionStatus.Connected;
        }
        catch (NpgsqlException ex)
        {
          if (ex.SqlState == "3D000")
          {
            var dbConfig = CommonConfig.GetConfig().TraceDatabaseConfig;
            var connCus = DBConnectionFactory.CreateDBConnection(dbConfig);
            bool result = TraceDBContext.CreateDatabase(connCus).Result;
            if (result)
            {
              _logger.LogInformation("*** CREATE DATABASE SUCCESS ***");
            }
            return EnumConnectionStatus.Error;
          }
          else
          {
            _logger.LogMsg(Messages.ErrException, ex);
            return EnumConnectionStatus.Error;
          }
        }
        catch (SqlException ex)
        {
          if (ex.Number == 4060)
          {
            var dbConfig = CommonConfig.GetConfig().TraceDatabaseConfig;
            var connCus = DBConnectionFactory.CreateDBConnection(dbConfig);
            bool result = TraceDBContext.CreateDatabase(connCus).Result;
            if (result)
            {
              _logger.LogInformation("*** CREATE DATABASE SUCCESS ***");
            }
            return EnumConnectionStatus.Error;
          }
          else
          {
            _logger.LogMsg(Messages.ErrException, ex);
            return EnumConnectionStatus.Error;
          }
        }
        catch (Exception ex)
        {
          _logger.LogMsg(Messages.ErrException, ex);
          return EnumConnectionStatus.Error;
        }
      }
      catch (Exception ex0)
      {
        _logger.LogMsg(Messages.ErrException, ex0);
        return EnumConnectionStatus.Error;
      }
    }

    public override async Task Init(CancellationToken stoppingToken)
    {
      while (true)
      {
        try
        {
          if (this.RabbitMQConnectionStatus == EnumConnectionStatus.Connected
              && this.DBConnectionStatus == EnumConnectionStatus.Connected)
          {
            await ReadMessages(handleMessage);
            break;
          }

          if (this.RabbitMQConnectionStatus != EnumConnectionStatus.Connected)
          {
            RabbitMQConnectionStatus = initRabbitMQ(_rabbitMQService, _queueProperties.QueueName);
          }

          if (this.DBConnectionStatus != EnumConnectionStatus.Connected)
          {
            lock (lockObj_initDB)
            {
              DBConnectionStatus = initDB();
            }
          }
        }
        catch (Exception ex)
        {
          _logger.LogMsg(Messages.ErrException, ex);
        }
        Thread.Sleep(10000);
      }
    }

    public override async Task handleMessage(DeliveryObj deliveryMessage)
    {
      string errMessage = "";
      string dataJson = "";
      string funcName = "handleMessage";
      DevicePayloadMessage? message = null;
      try
      {
        DateTime startTime = DateTime.Now;
        message = deliveryMessage.GetData<DevicePayloadMessage>(ref dataJson, ref errMessage);

        if (message == null)
        {
          this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, $"err: message = null, {errMessage}, json: {dataJson}");
          this.RemoveErrorMessage(deliveryMessage);
          return;
        }
        if (message.Data == null || message.Data.Count <= 0 || message.ConnectionId == null || message.MessageId == null)
        {
          this._logger.LogMsg(Messages.ErrInputInvalid_0_1, funcName, message.MessageId + ", " + $"json: {dataJson}");
          this.RemoveErrorMessage(deliveryMessage);
          return;
        }

        if (!cached.ContainsKey($"log_trace_message_seconds_10m_all"))
        {
          cached.AddToCache($"log_trace_message_seconds_10m_all", true, 60 * 10);
          this._logger.LogMsg(Messages.IBegin_0_1, $"[tracedata10m] All", message.MessageId + ", message: " + message.ToJson());
        }

        importDataFactory.ImportTraceData(message, CommonConfig.GetConfig().TraceDatabaseConfig.DatabaseType);
        this.RemoveSuccessMessage(deliveryMessage);
        this._logger.LogMsg(Messages.ISuccess_0_1, funcName, message.MessageId + ", " + DateTimeUtil.GetHumanStr(DateTime.Now - startTime));
        return;
      }
      catch (JsonReaderException ex)
      {
        this._logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
        this.RemoveErrorMessage(deliveryMessage);
      }
      catch (RetryException ex)
      {
        this._logger.LogWarning($"RetryException, {message.MessageId}, " + ex.Message);
        this.RetryMessage(deliveryMessage);
      }
      catch (DBException ex)
      {
        this._logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
        this.RetryMessage(deliveryMessage);

        if (ex.Message.Contains("Connection is not open"))
        {
          try
          {
            if (this.conn != null && this.conn.State == ConnectionState.Open)
            {
              this._logger.LogWarning("Try close connection");
              this.conn.Close();
              this.conn.Dispose();
            }
          }
          catch (Exception ex2) { }

          lock (lockObj_initDB)
          {
            DBConnectionStatus = initDB();
          }
        }

        //throw ex;
      }
      catch (CriticalException ex)
      {
        this._logger.LogMsg(Messages.ErrDBException.SetParameters($"json: {dataJson}", ex));
        this.RetryMessage(deliveryMessage);
        //throw ex;
      }
      catch (BaseException ex)
      {
        this._logger.LogMsg(Messages.ErrBaseException.SetParameters($"json: {dataJson}", ex));
        this.RetryMessage(deliveryMessage);
        //throw ex;
      }
      catch (Exception ex)
      {
        this._logger.LogMsg(Messages.ErrException.SetParameters($"json: {dataJson}", ex));
        this.RetryMessage(deliveryMessage);
        //throw ex;
      }
    }

  }
}
