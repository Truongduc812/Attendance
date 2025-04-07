using SourceBaseBE.Database.DBContexts;
using iSoft.DBLibrary.SQLBuilder.Interfaces;
using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.Extensions.EntityFrameworkCore.UnitOfWork;
using Microsoft.Extensions.Logging;
using SourceBaseBE.Database.DTOs;
using SourceBaseBE.Database.Enums;
using System.Data;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.Common.Exceptions;
using iSoft.Common.Lock;
using System.Diagnostics;
using iSoft.DBLibrary;
using iSoft.Common;
using SourceBaseBE.Database.Entities.TraceData;
using SourceBaseBE.Database.DTOs.Interfaces;
using SourceBaseBE.Database.Entities;
using Serilog;

namespace SourceBaseBE.Database.Repository.TraceDataNS
{
  public class TraceConnectionRepository : GenericRepository<TraceDBContext, DeviceConnectionEntity>
  {
    internal readonly Serilog.ILogger _logger = Log.Logger;
    internal ISQLBuilder sqlBuilder;
    //private IDBConnectionCustom dbConnCus;


    public TraceConnectionRepository(TraceDBContext dbContext)
        : base(dbContext)
    {
    }

    public TraceConnectionRepository(IUnitOfWork<TraceDBContext> unitOfWork)
        : base(unitOfWork)
    {
    }


    public TraceConnectionRepository(ISQLBuilder sqlBuilder, TraceDBContext dbContext)
        : base(dbContext)
    {
      this.sqlBuilder = sqlBuilder;
      //this.dbConnCus = dbConnectionCus;
    }

    public List<DeviceConnectionEntity> InsertIfNotExists(List<DeviceConnectionEntity> listEntity, IDbConnection conn)
    {
      try
      {
        const string funcName = "ConnectionRepository InsertIfNotExists";
        List<DeviceConnectionEntity> insertedResult = new List<DeviceConnectionEntity>();
        List<DeviceConnectionEntity> rs = new List<DeviceConnectionEntity>();
        if (listEntity == null || listEntity.Count == 0)
        {
          return insertedResult;
        }
        DateTime curDatetime = DateTime.Now;
        ISQLBuilder query = this.sqlBuilder
            .New()
            .Insert(TableName.DeviceConnections, new object[] {DeviceConnections.ConnectionKey,
                                                          DeviceConnections.CreatedAt })
            ;
        lock (Lock.lockObj_RunSQLServer)
        {
          // Select from db
          foreach (var entity in listEntity)
          {
            var entity2 = this.SelectOne(entity.ConnectionKey, conn);
            if (entity2 == null)
            {
              query.Values(new object[] { entity.ConnectionKey, curDatetime });
              insertedResult.Add(entity);
            }
            else
            {
              rs.Add(entity2);
            }
          }

          // Insert to db
          if (insertedResult.Count >= 1)
          {
            using (IDbCommand command = conn.CreateCommand())
            {
              command.CommandText = query.GetSQL();
              foreach (var item in query.GetParameters())
              {
                this.Context.dbConnectionCustom.AddParam(command, item.Key, item.Value);
              }
              var count = command.ExecuteNonQuery();

              _logger.LogMsg(Messages.ISuccess_0_1, funcName, $"Inserted {count} records");
            }
          }

          // Reselect from db
          foreach (var entity in insertedResult)
          {
            var entity2 = this.SelectOne(entity.ConnectionKey, conn);
            if (entity2 == null)
            {
              throw new DBException("Reselect from db not found");
            }
            else
            {
              rs.Add(entity2);
            }
          }
        }
        return rs;
      }
      catch (Exception ex)
      {
        throw new DBException(ex);
      }
    }
    public DeviceConnectionEntity SelectOne(long connId, IDbConnection conn)
    {
      try
      {
        var listData = new List<DeviceConnectionEntity>();
        const string funcName = "SelectOne";
        DateTime curDatetime = DateTime.Now;
        ISQLBuilder query = this.sqlBuilder
            .New()
            .From(TableName.DeviceConnections)
            .Selects("*")
            .Where(new FieldName(DeviceConnections.ConnectionKey.ToString()), connId)
            ;
        lock (Lock.lockObj_RunSQLServer)
        {
          using (IDbCommand command = conn.CreateCommand())
          {
            command.CommandText = query.GetSQL();
            foreach (var item in query.GetParameters())
            {
              this.Context.dbConnectionCustom.AddParam(command, item.Key, item.Value);
            }

            using (IDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                DeviceConnectionEntity entity = new DeviceConnectionEntity();
                entity.FillData(reader);
                listData.Add(entity);
              }
            }

            //_logger.LogMsg(Messages.ISuccess_0_1, funcName, $"Inserted {listEntity.Count} records");
            if (listData.Count == 1)
            {
              return listData[0];
            }
            else if (listData.Count >= 2)
            {
              throw new DBException($"listData.Count >= 2, connId = {connId}");
            }
            return null;
          }
        }
      }
      catch (Exception ex)
      {
        throw new DBException(ex);
      }
    }
  }
}