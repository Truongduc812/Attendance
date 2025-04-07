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
using SourceBaseBE.Database.DTOs.Interfaces;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Entities.TraceData;

namespace SourceBaseBE.Database.Repository.DeviceData
{
	public class EnvironmentRepository : GenericRepository<CommonDBContext, EnvironmentEntity>
	{
		internal readonly ILogger<EnvironmentRepository> logger;
		internal ISQLBuilder sqlBuilder;
		//private IDBConnectionCustom dbConnCus;


		public EnvironmentRepository(CommonDBContext dbContext)
			: base(dbContext)
		{
		}

		public EnvironmentRepository(IUnitOfWork<CommonDBContext> unitOfWork)
			: base(unitOfWork)
		{
		}


		public EnvironmentRepository(ILoggerFactory loggerFactory, ISQLBuilder sqlBuilder, CommonDBContext dbContext)
			: base(dbContext)
		{
			this.logger = loggerFactory?.CreateLogger<EnvironmentRepository>();
			this.sqlBuilder = sqlBuilder;
			//this.dbConnCus = dbConnectionCus;
		}

		public List<EnvironmentEntity> InsertIfNotExists(List<EnvironmentEntity> listEntity, IDbConnection conn)
		{
			try
			{
				const string funcName = "EnvironmentRepository InsertIfNotExists";
				List<EnvironmentEntity> insertedResult = new List<EnvironmentEntity>();
				List<EnvironmentEntity> rs = new List<EnvironmentEntity>();
				if (listEntity == null || listEntity.Count == 0)
				{
					return insertedResult;
				}
				DateTime curDatetime = DateTime.Now;
				ISQLBuilder query = this.sqlBuilder
					.New()
					.Insert(TableName.Environments, new object[] { Enums.Environments.ConnectionId,
														  Enums.Environments.EnvKey,
														  Enums.Environments.TraceColumnId,
														  Enums.Environments.TraceColumnDataType,
														  Enums.Environments.CreatedAt })
					;
				lock (Lock.GetLockObject(ConstantLock.lockExecuteSQL, 60 * 15))
				{
					var maxTraceColumnId = -1;

					// Select from db
					int i = 0;
					foreach (var entity in listEntity)
					{
						var entity2 = this.SelectOne(entity.EnvKey, conn);
						if (entity2 == null)
						{
							query.Values(new object[] { entity.ConnectionId,
										  entity.EnvKey,
										  entity.TraceColumnName,
										  entity.TraceColumnDataType,
										  curDatetime });
							i++;
							insertedResult.Add(entity);
						}
						else
						{
							rs.Add(entity2);
						}
					}

					// Insert to db if not exists
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

							logger.LogMsg(Messages.ISuccess_0_1, funcName, $"Inserted {count} records");
						}
					}

					// Select inserted item from db to response
					foreach (var entity in insertedResult)
					{
						var entity2 = this.SelectOne(entity.EnvKey, conn);
						if (entity2 == null)
						{
							throw new DBException("Inserted item not found");
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
		public bool IsExists(string envKey, IDbConnection conn)
		{
			try
			{
				const string funcName = "IsExists";
				List<EnvironmentEntity> result = new List<EnvironmentEntity>();
				DateTime curDatetime = DateTime.Now;
				ISQLBuilder query = this.sqlBuilder
					.New()
					.From(TableName.Environments)
					.SelectCount(new FieldName(Enums.Environments.EnvKey.ToString()), "count")
					.Where(new FieldName(Enums.Environments.EnvKey.ToString()), envKey)
					;
				lock (Lock.GetLockObject(ConstantLock.lockExecuteSQL, 60 * 15))
				{
					using (IDbCommand command = conn.CreateCommand())
					{
						command.CommandText = query.GetSQL();
						foreach (var item in query.GetParameters())
						{
							this.Context.dbConnectionCustom.AddParam(command, item.Key, item.Value);
						}
						int count = (int)command.ExecuteScalar();

						//_logger.LogMsg(Messages.ISuccess_0_1, funcName, $"Inserted {listEntity.Count} records");
						if (count > 0)
						{
							return true;
						}
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public int SelectMaxTraceColumnId(IDbConnection conn)
		{
			try
			{
				const string funcName = "SelectMaxTraceColumnId";
				List<EnvironmentEntity> result = new List<EnvironmentEntity>();
				DateTime curDatetime = DateTime.Now;
				ISQLBuilder query = this.sqlBuilder
					.New()
					.From(TableName.Environments)
					.SelectMax(new FieldName(Enums.Environments.TraceColumnId.ToString()), "max")
					;
				lock (Lock.GetLockObject(ConstantLock.lockExecuteSQL, 60 * 15))
				{
					using (IDbCommand command = conn.CreateCommand())
					{
						command.CommandText = query.GetSQL();
						foreach (var item in query.GetParameters())
						{
							this.Context.dbConnectionCustom.AddParam(command, item.Key, item.Value);
						}
						var resultObj = command.ExecuteScalar();
						if (resultObj != null && resultObj != DBNull.Value)
						{
							int max = (int)resultObj;
							if (max >= 0)
							{
								return max;
							}
						}

						return 0;
					}
				}
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public EnvironmentEntity SelectOne(string envKey, IDbConnection conn)
		{
			try
			{
				var listData = new List<EnvironmentEntity>();
				const string funcName = "SelectOne";
				DateTime curDatetime = DateTime.Now;
				ISQLBuilder query = this.sqlBuilder
					.New()
					.From(TableName.Environments)
					.Selects("*")
					.Where(new FieldName(Enums.Environments.EnvKey.ToString()), envKey)
					;
				lock (Lock.GetLockObject(ConstantLock.lockExecuteSQL, 60 * 15))
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
								EnvironmentEntity entity = new EnvironmentEntity();
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
							throw new DBException($"listData.Count >= 2, EnvKey = {envKey}");
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
		public Dictionary<string, EnvironmentEntity> SelectAll(IDbConnection conn)
		{
			try
			{
				var dicData = new Dictionary<string, EnvironmentEntity>();
				const string funcName = "SelectAll";
				DateTime curDatetime = DateTime.Now;
				ISQLBuilder query = this.sqlBuilder
					.New()
					.From(TableName.Environments)
					.Selects("*")
					;
				lock (Lock.GetLockObject(ConstantLock.lockExecuteSQL, 60 * 15))
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
								EnvironmentEntity entity = new EnvironmentEntity();
								entity.FillData(reader);
								dicData.Add(entity.EnvKey, entity);
							}
						}
						return dicData;
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