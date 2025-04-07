using Confluent.Kafka;
using Elasticsearch.Net;
using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Common.Exceptions;
using iSoft.Common.Lock;
using iSoft.Common.Utils;
using iSoft.DBLibrary;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.DBLibrary.SQLBuilder;
using iSoft.DBLibrary.SQLBuilder.Interfaces;
using System.Data;
using System.Text;
using SourceBaseBE.CommonFunc.EnvConfigData;
using SourceBaseBE.Database.Enums;
using iSoft.ElasticSearch.Models;
using iSoft.ElasticSearch.Services;
using iSoft.DBLibrary.SQLBuilder.Enum;
using ClickHouse.Client.Utility;
using SourceBaseBE.Database.SQLScripts.Interface;
using iSoft.Common.Enums.DBProvider;
//using iSoft.ConnectionCommon.Model;
using iSoft.Common.ExtensionMethods;
using Serilog;
using Npgsql;
using System.Data.SqlClient;
using NPOI.SS.Formula.Eval;
using iSoft.Common.Cached;
using iSoft.RabbitMQ.Payload;
using iSoft.Redis.Services;

namespace SourceBaseBE.CommonFunc.DataService
{
  public class TraceDataTableObj
  {
    private MemCached cachedLock = new MemCached(1);
    private readonly ILogger _logger = Log.Logger;
    public string TableName;
    public string IndexName;
    public string ESPatternSearch;
    public string ESPatternSearchBase;
    public Dictionary<string, EnvConfigModel> dicEnvKey2Config = new Dictionary<string, EnvConfigModel>();
    public Dictionary<string, object> DicEnvKey2Value = new Dictionary<string, object>();
    private DevicePayloadMessage message = null;
    private int minTimeIntervalInSeconds;

    public TraceDataTableObj(
      string tableName,
      string indexName,
      string esPatternSearch,
      string esPatternSearchBase,
      Dictionary<string, EnvConfigModel> dicEnvKey2Config,
      Dictionary<string, object> dicEnvKey2Value,
      DevicePayloadMessage message)
    {
      this.TableName = tableName;
      this.IndexName = indexName;
      this.ESPatternSearch = esPatternSearch;
      this.ESPatternSearchBase = esPatternSearchBase;
      this.dicEnvKey2Config = dicEnvKey2Config;
      this.DicEnvKey2Value = dicEnvKey2Value;
      this.message = message;
      this.minTimeIntervalInSeconds = dicEnvKey2Config.Min(keyVal => keyVal.Value.MinTimeIntervalInSeconds);
    }
    public TraceDataTableObj(
      string tableName,
      string indexName,
      string esPatternSearch,
      string esPatternSearchBase,
      Dictionary<string, EnvConfigModel> dicEnvKey2Config)
    {
      this.TableName = tableName;
      this.IndexName = indexName;
      this.ESPatternSearch = esPatternSearch;
      this.ESPatternSearchBase = esPatternSearchBase;
      this.dicEnvKey2Config = dicEnvKey2Config;
      this.minTimeIntervalInSeconds = dicEnvKey2Config.Min(keyVal => keyVal.Value.MinTimeIntervalInSeconds);
    }

    public EnumFuncResult SaveTraceData(IDbConnection conn,
      //IDbTransaction transaction,
      IDBConnectionCustom connCus,
      ref Dictionary<string, LastData> dicTableName2LastData,
      EnumDBProvider databaseType)
    {
      EnumFuncResult ret;

      //lock (Lock.GetLockObject(ConstantLock.lockWriteInput, 60))
      //{
      //  this._logger.LogMsg(Messages.IBegin_0_1, "[SaveTraceData]", message.MessageId + ", " + message.ToJson());
      //}
      IDbTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

      ret = createTableIfNotExists(conn, transaction, ref dicTableName2LastData, databaseType);
      //if (ret == EnumFuncResult.Ok)
      //{
      //  this._logger.LogInformation($"[SaveTraceData] Created table: {this.TableSerial}");
      //}
      if (ret == EnumFuncResult.Error)
      {
        try
        {
          transaction.Rollback();
        }
        catch { }
        return ret;
      }

      try
      {
        ret = insertTraceData(conn, transaction, connCus, ref dicTableName2LastData, databaseType);

        transaction.Commit();
      }
      catch (DBException ex)
      {
        try
        {
          transaction.Rollback();
        }
        catch { }

        // Check column not exists:
        transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        try
        {
          var sqlDebugData = new SQLDebugData();
          bool alterColumnFlag = false;

          var sqlScriptObj = SQLScripts.GetSQLScriptInstance(databaseType);
          foreach (var keyVal in this.dicEnvKey2Config)
          {
            var envConfig = keyVal.Value;

            // Column not exists
            if (this.isExistsColumn(conn, transaction, envConfig.GetEnvKeyDB(), databaseType) == EnumFuncResult.Error)
            {
              using (IDbCommand command = conn.CreateCommand())
              {
                command.Transaction = transaction;
                StringBuilder sb = new StringBuilder();
                command.CommandText = sqlScriptObj.GetSQL_AlterColumnTraceData()
                  .Replace("@tableName", this.TableName)
                  .Replace("@columnName", envConfig.GetEnvKeyDB())
                  .Replace("@dataType", envConfig.GetSQLDataType(databaseType));

                sqlDebugData.SQLString = command.CommandText;

                var count = command.ExecuteNonQuery();

                _logger.LogInformation("[{0}], AlterColumnTraceData: {1}", "SaveTraceData", sqlDebugData.ToString());
                alterColumnFlag = true;
              }
            }
          }
          if (alterColumnFlag)
          {
            transaction.Commit();
            return EnumFuncResult.Ok;
          }
        }
        catch (Exception ex2)
        {
          try
          {
            transaction.Rollback();
          }
          catch { }
        }
        throw ex;
      }
      return ret;
    }

    private EnumFuncResult createTableIfNotExists(IDbConnection conn,
      IDbTransaction transaction,
      ref Dictionary<string, LastData> dicTableName2LastData,
      EnumDBProvider databaseType)
    {
      const string funcName = nameof(createTableIfNotExists);
      var sqlDebugData = new SQLDebugData();
      try
      {
        if (!dicTableName2LastData.ContainsKey(this.TableName))
        {
          using (IDbCommand command = conn.CreateCommand())
          {
            command.Transaction = transaction;
            StringBuilder sb = new StringBuilder();
            foreach (var keyVal in this.dicEnvKey2Config)
            {
              var envConfig = keyVal.Value;
              sb.Append(string.Format(@"""{0}"" {1} NULL,", envConfig.GetEnvKeyDB(), envConfig.GetSQLDataType(databaseType)));
            }
            var sqlScriptObj = SQLScripts.GetSQLScriptInstance(databaseType);
            command.CommandText = sqlScriptObj.GetSQL_CreateTableTraceData()
              .Replace("@tableName", this.TableName)
              .Replace("@fields", StringUtil.RemoveLastChar(sb.ToString()));

            sqlDebugData.SQLString = command.CommandText;

            var count = command.ExecuteNonQuery();

            _logger.LogInformation("[{0}], CreateTableTraceData: {1}", funcName, sqlDebugData.ToString());
          }
          dicTableName2LastData.Add(this.TableName, new LastData());
          return EnumFuncResult.Ok;
        }
        else
        {
        }

        return EnumFuncResult.Ignore;
        //}
      }
      catch (Exception ex)
      {
        _logger.LogError(sqlDebugData.ToString());
        throw new DBException(ex);
      }
    }

    public EnumFuncResult isExistsTable(IDbConnection conn, EnumDBProvider databaseType)
    {
      var sqlDebugData = new SQLDebugData();
      try
      {
        using (IDbCommand command = conn.CreateCommand())
        {
          var sqlScriptObj = SQLScripts.GetSQLScriptInstance(databaseType);
          command.CommandText = sqlScriptObj.GetSQL_IsExistsTable()
            .Replace("@tableName", this.TableName);

          sqlDebugData.SQLString = command.CommandText;

          var rsData = command.ExecuteScalar();

          _logger.LogInformation(sqlDebugData.ToString());

          if (rsData == null || string.IsNullOrEmpty(rsData.ToString()))
          {
            return EnumFuncResult.Error;
          }
        }
        return EnumFuncResult.Ok;
      }
      catch (Exception ex)
      {
        _logger.LogError(sqlDebugData.ToString());
        throw new DBException(ex);
      }
    }

    public EnumFuncResult isExistsColumn(IDbConnection conn, IDbTransaction transaction, string columnName, EnumDBProvider databaseType)
    {
      var sqlDebugData = new SQLDebugData();
      try
      {
        using (IDbCommand command = conn.CreateCommand())
        {
          if (transaction != null)
          {
            command.Transaction = transaction;
          }
          var sqlScriptObj = SQLScripts.GetSQLScriptInstance(databaseType);
          command.CommandText = sqlScriptObj.GetSQL_IsExistsColumn()
            .Replace("@tableName", this.TableName)
            .Replace("@columnName", columnName);

          sqlDebugData.SQLString = command.CommandText;

          int? rsData = (int?)command.ExecuteScalar();

          //_logger.LogInformation(sqlDebugData.ToString());

          if (rsData == null)
          {
            return EnumFuncResult.Error;
          }
        }
        return EnumFuncResult.Ok;
      }
      catch (Exception ex)
      {
        _logger.LogError(sqlDebugData.ToString());
        throw new DBException(ex);
      }
    }

    private EnumFuncResult insertTraceData(IDbConnection conn,
      IDbTransaction transaction,
      IDBConnectionCustom connCus,
      ref Dictionary<string, LastData> dicTableName2LastData,
      EnumDBProvider databaseType)
    {
      const string funcName = nameof(insertTraceData);
      var sqlDebugData = new SQLDebugData();
      try
      {
        //if (!dicTableName2LastData.ContainsKey(this.TableSerial))
        //{
        //  throw new DBException($"[insertTraceData] Table not exists: ${this.TableSerial}");
        //}
        var listValue = getListValues();
        var listColumnName = getListColumnName();
        DateTime curDatetime = DateTimeUtil.GetLocalDateTime(DateTime.Now);
        ISQLBuilder query = BaseSQLBuilder.GetSQLBuilderInstance(databaseType)
            .New()
            .Insert(this.TableName, new string[] {TraceDataISendVarCounts.MessageId.ToString(),
                                                  TraceDataISendVarCounts.ConnectionId.ToString(),
                                                  TraceDataISendVarCounts.ExecuteAt.ToString(),
                                                  TraceDataISendVarCounts.CreatedAt.ToString()}
                                                .Concat(listColumnName.ToArray()).ToArray());
        query.Values(new object[] { this.message.MessageId,
                                    this.message.ConnectionId,
                                    this.message.ExecuteAt,
                                    curDatetime}
                                  .Concat(listValue.ToArray()).ToArray());

        //lock (Lock.GetLockObject(ConstantLock.lockExecuteSQL, 60 * 15))
        //{
        using (IDbCommand command = conn.CreateCommand())
        {
          command.Transaction = transaction;
          command.CommandText = query.GetSQL();
          foreach (var item in query.GetParameters())
          {
            connCus.AddParam(command, item.Key, item.Value == null ? DBNull.Value : item.Value);
          }
          sqlDebugData.Params = query.GetParameters();
          sqlDebugData.SQLString = command.CommandText;

          if (this.minTimeIntervalInSeconds == 0)
          {
            var count = command.ExecuteNonQuery();

            this._logger.LogMsg(Messages.ISuccess_0_1, $"[{funcName}]", message.MessageId + ", TableSerial: " + this.TableName + ", ExecuteAt: " + this.message.ExecuteAt);
          }
          else
          {
            lock (Lock.lockObj_SaveTraceLastData)
            {
              var lastData = dicTableName2LastData[this.TableName];
              if (lastData.lastTime != null
                && Math.Abs((long)DateTimeUtil.CompareDateTime(lastData.lastTime, this.message.ExecuteAt.Ticks, 0)) < this.minTimeIntervalInSeconds
                && CompareUtil.MatchData(lastData.listObj, listValue, this.TableName))
              {
                this._logger.LogInformation($"[{funcName}] Not collect table, " + message.MessageId + ", TableSerial: " + this.TableName + ", ExecuteAt: " + this.message.ExecuteAt + ", lastTime: " + (lastData.lastTime != null ? (new DateTime((long)lastData.lastTime)) : "null"));
                return EnumFuncResult.Ignore;
              }

              dicTableName2LastData[this.TableName].lastTime = this.message.ExecuteAt.Ticks;
              dicTableName2LastData[this.TableName].listObj = listValue;
            }

            var count = command.ExecuteNonQuery();

            this._logger.LogMsg(Messages.ISuccess_0_1, $"[{funcName}]", message.MessageId + ", TableSerial: " + this.TableName + ", ExecuteAt: " + this.message.ExecuteAt);

          }
        }

        return EnumFuncResult.Ok;
        //}
      }
      catch (NpgsqlException ex)
      {
        // Check if the exception is due to a unique constraint violation
        if (ex.SqlState == "23505") // Unique violation SQL state code in PostgreSQL
        {
          this._logger.Information($"[{funcName}] Duplicate message, TableSerial: {TableName} MessageId: {message.MessageId}");
          return EnumFuncResult.Ignore;
        }
        else
        {
          _logger.LogError(sqlDebugData.ToString());
          throw new DBException(ex);
        }
      }
      catch (SqlException ex)
      {
        // Check if the exception is due to a unique constraint violation
        if (ex.Number == 2627) // Unique violation SQL state code in PostgreSQL
        {
          this._logger.Information($"[{funcName}] Duplicate message, TableSerial: {TableName} MessageId: {message.MessageId}");
          return EnumFuncResult.Ignore;
        }
        else
        {
          _logger.LogError(sqlDebugData.ToString());
          throw new DBException(ex);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(sqlDebugData.ToString());
        throw new DBException(ex);
      }
    }

    public async Task GetTraceData(IDbConnection conn,
      IDBConnectionCustom connCus,
      ESRestfullAPIService esAPIService,
      DateTime dateTimeFrom,
      DateTime dateTimeTo,
      Dictionary<string, DevicePayloadMessage> dicMessageId2Message,
      EnumDBProvider databaseType)
    {
      var sqlDebugData = new SQLDebugData();
      try
      {
        Dictionary<string, object> dicData = new Dictionary<string, object>();
        long ignoreESCount = 0;
        long pushCount = 0;

        var listColumnName = getListColumnName();
        DateTime curDatetime = DateTimeUtil.GetLocalDateTime(DateTime.Now);
        ISQLBuilder query = BaseSQLBuilder.GetSQLBuilderInstance(databaseType)
            .New()
            .From(this.TableName)
            .Selects("*")
            .WhereGreaterThen<DateTime>(new FieldName(TraceDataISendVarCounts.ExecuteAt.ToString()), dateTimeFrom, false)
            .WhereLessThen<DateTime>(new FieldName(TraceDataISendVarCounts.ExecuteAt.ToString()), dateTimeTo, true)
            .OrderBy(TraceDataISendVarCounts.ExecuteAt.ToString(), SQLSortOrder.ASC)
            ;

        using (IDbCommand command = conn.CreateCommand())
        {
          command.CommandText = query.GetSQL();
          foreach (var item in query.GetParameters())
          {
            connCus.AddParam(command, item.Key, item.Value);
          }
          sqlDebugData.SQLString = command.CommandText;
          sqlDebugData.Params = query.GetParameters();
          string[] arrColumnName = null;
          using (IDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              try
              {
                if (arrColumnName == null)
                {
                  arrColumnName = reader.GetColumnNames();
                }
                dicData.Clear();

                string? messageId = reader.IsDBNull(reader.GetOrdinal(TraceDataISendVarCounts.MessageId.ToString())) ? null : reader.GetString(reader.GetOrdinal(TraceDataISendVarCounts.MessageId.ToString()));
                DateTime? executeAt = reader.IsDBNull(reader.GetOrdinal(TraceDataISendVarCounts.ExecuteAt.ToString())) ? null : reader.GetDateTime(reader.GetOrdinal(TraceDataISendVarCounts.ExecuteAt.ToString()));

                if (messageId == null || executeAt == null)
                {
                  continue;
                }

                try
                {
                  bool existsFlag = await esAPIService.IsExistsMessageIdAsync(this.ESPatternSearchBase, messageId);
                  if (existsFlag)
                  {
                    ignoreESCount++;
                    continue;
                  }
                }
                catch (Exception ex)
                {
                  ignoreESCount++;
                  _logger.LogMsg(Messages.ErrBaseException.SetParameters(ex));
                  throw ex;
                }

                long? connectionId = reader.IsDBNull(reader.GetOrdinal(TraceDataISendVarCounts.ConnectionId.ToString())) ? null : reader.GetInt64(reader.GetOrdinal(TraceDataISendVarCounts.ConnectionId.ToString()));
                //DateTime? createdAt = reader.IsDBNull(reader.GetOrdinal(TraceDataISendVarCounts.CreatedAt.ToString())) ? null : reader.GetDateTime(reader.GetOrdinal(TraceDataISendVarCounts.CreatedAt.ToString()));

                for (int i = 0; i < arrColumnName.Length; i++)
                {
                  string columnName = arrColumnName[i];
                  string configKey = StringUtil.ConvertToESField(columnName, message.ConnectionId);
                  if (this.dicEnvKey2Config.ContainsKey(configKey))
                  {
                    var envConfig = this.dicEnvKey2Config[configKey];
                    object? value = envConfig.GetValueFromReader(reader);
                    if (value != null)
                    {
                      dicData.Add(configKey, value.ToString());
                    }
                  }
                }

                if (dicData.Count >= 1)
                {
                  pushCount++;
                  if (!dicMessageId2Message.ContainsKey(messageId))
                  {
                    DevicePayloadMessage message = new DevicePayloadMessage()
                    {
                      MessageId = messageId,
                      ConnectionId = connectionId.Value,
                      Data = dicData,
                      ExecuteAt = executeAt.Value,
                    };

                    dicMessageId2Message.Add(messageId, message);
                  }
                  else
                  {
                    foreach (var keyVal in dicMessageId2Message[messageId].Data)
                    {
                      if (!dicData.ContainsKey(keyVal.Key))
                      {
                        dicData.Add(keyVal.Key, keyVal.Value);
                      }
                    }
                    dicMessageId2Message[messageId].Data = dicData;
                  }
                }
              }
              catch (Exception ex)
              {
                _logger.LogMsg(Messages.ErrBaseException.SetParameters(ex));
                throw ex;
              }
            }
          }
        }

        _logger.LogInformation($"[GetTraceData] TableName: {this.TableName}, pushCount: {pushCount}, ignoreESCount: {ignoreESCount}");

        return;
      }
      catch (Exception ex)
      {
        _logger.LogError(sqlDebugData.ToString());
        throw new DBException(ex);
      }
    }

    private object GetEntityType()
    {
      throw new NotImplementedException();
    }

    public List<string> getListColumnName()
    {
      return this.dicEnvKey2Config.Values.Select(x => x.GetEnvKeyDB()).ToList();
    }
    public List<object> getListValues()
    {
      List<object> listInsertValue = new List<object>();
      foreach (var keyVal in this.dicEnvKey2Config)
      {
        if (this.DicEnvKey2Value.ContainsKey(keyVal.Value.GetKey()))
        {
          listInsertValue.Add(keyVal.Value.GetValueFromObject(this.DicEnvKey2Value[keyVal.Value.GetKey()]));
        }
        else
        {
          listInsertValue.Add(null);
        }
      }
      return listInsertValue;
    }

    public async Task<EnumFuncResult> SaveESData(ESRestfullAPIService esAPIService,
                                      Dictionary<string, LastData> dicIndexName2LastData)
    {
      EnumFuncResult ret;

      //lock (Lock.GetLockObject($"{ConstantLock.lockWriteInput}_{this.ESPatternSearchBase}_{message.MessageId}", 60))
      //{
      //  this._logger.LogMsg(Messages.IBegin_0_1, "[SaveESData]", message.MessageId + ", " + message.ToJson());
      //}

      string keyLock = $"SaveESData_{this.ESPatternSearchBase}_{message.MessageId}";
      if (CachedFunc.RequireLockAndRetry(keyLock, 1000, 12, 8))
      {
        try
        {
          if (!await esAPIService.IsExistsMessageIdAsync(this.ESPatternSearchBase, message.MessageId))
          {
            ret = await createIndexIfNotExists(esAPIService, dicIndexName2LastData);
            if (ret == EnumFuncResult.Ok)
            {
              this._logger.LogInformation($"[SaveESData] Created index: {this.IndexName}");
            }
            else if (ret == EnumFuncResult.Error)
            {
              _logger.LogError($"[SaveESData] createIndexIfNotExists error, IndexName: {this.IndexName}");
            }
            ret = await insertESData(esAPIService, dicIndexName2LastData);
            return ret;
          }
          else
          {
            this._logger.LogInformation($"[SaveESData] IsExistsMessageIdAsync (Duplicate), ESPatternSearchBase: {this.ESPatternSearchBase}, MessageId: {message.MessageId}");
            return EnumFuncResult.Ignore;
          }
        }
        finally
        {
          CachedFunc.UnLock(keyLock);
        }
      }
      else
      {
        this._logger.LogInformation($"[SaveESData] Check locked item (Duplicate), ESPatternSearchBase: {this.ESPatternSearchBase}, MessageId: {message.MessageId}");
        throw new RetryException($"[SaveESData] Check locked item (Duplicate), ESPatternSearchBase: {this.ESPatternSearchBase}, MessageId: {message.MessageId}");
      }
    }
    private async Task<EnumFuncResult> createIndexIfNotExists(ESRestfullAPIService esAPIService,
                                                              Dictionary<string, LastData> dicIndexName2LastData)
    {
      try
      {
        lock (Lock.GetLockObject(ConstantLock.lockSaveESLastData, 60 * 15))
        {
          if (!dicIndexName2LastData.ContainsKey(this.IndexName))
          {
            List<IESFieldName> parentList = this.dicEnvKey2Config.Values.ToList().ConvertAll(x => (IESFieldName)x);
            esAPIService.CreateStreamIndexAsync(parentList, this.IndexName, this.ESPatternSearch).Wait();
            dicIndexName2LastData.Add(this.IndexName, new LastData());
            return EnumFuncResult.Ok;
          }
        }

        return EnumFuncResult.Ignore;
      }
      catch (Exception ex)
      {
        throw new DBException(ex);
      }
    }

    private async Task<EnumFuncResult> insertESData(ESRestfullAPIService esAPIService,
                                        Dictionary<string, LastData> dicIndexName2LastData)
    {
      try
      {
        //if (!dicIndexName2LastData.ContainsKey(this.IndexName))
        //{
        //  throw new DBException($"[insertESData] Index not exists: ${this.IndexName}");
        //}

        var listValue = getListValues();
        var listColumnName = getListColumnName();
        DateTime curDatetime = DateTime.Now;

        List<IESFieldName> parentList = this.dicEnvKey2Config.Values.ToList().ConvertAll(x => (IESFieldName)x);
        string json = parentList.ToJson();
        Dictionary<string, IESFieldName> dicEnvKey2ESField = new Dictionary<string, IESFieldName>();
        foreach (var keyVal in dicEnvKey2Config)
        {
          dicEnvKey2ESField.Add(keyVal.Key, keyVal.Value);
        }

        if (this.minTimeIntervalInSeconds == 0)
        {
          esAPIService.PushStreamDataAsync(message.ConnectionId, message.ExecuteAt, message.MessageId, GetShiftId(message.ExecuteAt), dicEnvKey2ESField, DicEnvKey2Value, this.IndexName).Wait();
          this._logger.LogMsg(Messages.ISuccess_0_1, "[insertESData]", message.MessageId + ", IndexName: " + this.IndexName + ", ExecuteAt: " + this.message.ExecuteAt);
        }
        else
        {
          lock (Lock.lockObj_SaveESLastData)
          {
            var lastData = dicIndexName2LastData[this.IndexName];
            if (lastData.lastTime != null
              && Math.Abs((long)DateTimeUtil.CompareDateTime(lastData.lastTime, this.message.ExecuteAt.Ticks, 0)) < this.minTimeIntervalInSeconds
              && CompareUtil.MatchData(lastData.listObj, listValue, this.TableName))
            {
              this._logger.LogInformation("[insertESData] Not collect index, " + message.MessageId + ", IndexName: " + this.IndexName + ", ExecuteAt: " + this.message.ExecuteAt + ", lastTime: " + (lastData.lastTime != null ? (new DateTime((long)lastData.lastTime)) : "null"));
              return EnumFuncResult.Ignore;
            }

            dicIndexName2LastData[this.IndexName].lastTime = this.message.ExecuteAt.Ticks;
            dicIndexName2LastData[this.IndexName].listObj = listValue;
          }

          esAPIService.PushStreamDataAsync(message.ConnectionId, message.ExecuteAt, message.MessageId, GetShiftId(message.ExecuteAt), dicEnvKey2ESField, DicEnvKey2Value, this.IndexName).Wait();

          this._logger.LogMsg(Messages.ISuccess_0_1, "[insertESData]", message.MessageId + ", IndexName: " + this.IndexName + ", ExecuteAt: " + this.message.ExecuteAt);

        }

        return EnumFuncResult.Ok;
      }
      catch (Exception ex)
      {
        throw new DBException(ex);
      }
    }

    private int GetShiftId(DateTime executeAt)
    {
      //DateTime executeAt = DateTimeUtil.GetLocalDateTime(executeAt0);
      long ticksCheck06 = new DateTime(executeAt.Year, executeAt.Month, executeAt.Day, 6, 0, 0).Ticks;
      long ticksCheck14 = new DateTime(executeAt.Year, executeAt.Month, executeAt.Day, 14, 0, 0).Ticks;
      long ticksCheck22 = new DateTime(executeAt.Year, executeAt.Month, executeAt.Day, 22, 0, 0).Ticks;

      if (DateTimeUtil.CompareDateTime(ticksCheck06, executeAt.Ticks, -1) >= 0
        && DateTimeUtil.CompareDateTime(executeAt.Ticks, ticksCheck14, -1) >= 0)
      {
        return 1;
      }

      if (DateTimeUtil.CompareDateTime(ticksCheck14, executeAt.Ticks, -1) >= 0
        && DateTimeUtil.CompareDateTime(executeAt.Ticks, ticksCheck22, -1) >= 0)
      {
        return 2;
      }

      return 3;

      //int hour = executeAt.Hour;
      //if (hour >= 6 && hour < 14)
      //{
      //  return 1;
      //}
      //else if (hour >= 14 && hour < 22)
      //{
      //  return 2;
      //}
      //else
      //{
      //  return 3;
      //}
    }
  }
}
