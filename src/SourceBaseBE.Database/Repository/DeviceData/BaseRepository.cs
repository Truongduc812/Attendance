using SourceBaseBE.Database.DBContexts;
using iSoft.DBLibrary.SQLBuilder.Interfaces;
using iSoft.Extensions.EntityFrameworkCore.Repository;
using iSoft.Extensions.EntityFrameworkCore.UnitOfWork;
using Microsoft.Extensions.Logging;
using System.Data;
using iSoft.DBLibrary.DBConnections.Interfaces;
using iSoft.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using iSoft.DBLibrary.SQLBuilder;
using iSoft.DBLibrary.Entities;
using iSoft.DBLibrary.SQLBuilder.Enum;

namespace SourceBaseBE.Database.Repository.DeviceData
{
	public class BaseRepository<T> : GenericRepository<CommonDBContext, T> where T : BaseEntity
	{
		internal readonly ILogger<BaseRepository<T>> logger;
		internal ISQLBuilder sqlBuilder;
		internal IDBConnectionCustom dbConnCus;

		protected string tableName = "";
		public BaseRepository(CommonDBContext dbContext)
			: base(dbContext)
		{
		}

		public BaseRepository(IUnitOfWork<CommonDBContext> unitOfWork)
			: base(unitOfWork)
		{
		}


		public BaseRepository(ILoggerFactory loggerFactory, ISQLBuilder sqlBuilder, CommonDBContext dbContext)
			: base(dbContext)
		{
			this.logger = loggerFactory?.CreateLogger<BaseRepository<T>>();
			this.sqlBuilder = sqlBuilder;
			this.dbConnCus = dbContext.dbConnectionCustom;
			this._unitOfWork = new UnitOfWork<CommonDBContext>(this.Context);

    }

		public int RunQuery(IDbConnection conn, IDbTransaction tran, ISQLBuilder query, ref SQLDebugData sqlDebugData)
		{
			int count = 0;
			using (IDbCommand command = conn.CreateCommand())
			{
				if (tran != null)
				{
					command.Transaction = tran;
				}
				command.CommandText = query.GetSQL();
				foreach (var item in query.GetParameters())
				{
					dbConnCus.AddParam(command, item.Key, item.Value == null ? 0 : item.Value);
				}
				sqlDebugData.Params = query.GetParameters();
				sqlDebugData.SQLString = command.CommandText;
				count = command.ExecuteNonQuery();
			}
			return count;
		}
		public List<TEntity> GetByListId<TEntity>(string table, List<long> deviceIds, string oderByField = "Id", SQLSortOrder orderBy = SQLSortOrder.ASC) where TEntity : BaseEntity
		{
			var query = new SQLServerSQLBuilder()
									.Selects("*")
									.From(table)
									.WhereIn(new FieldName("Id"), deviceIds.Cast<object>().ToArray())
									.Where(new FieldName("DeletedFlag"), false)
									.OrderBy(oderByField, orderBy)
									;
			object[] parameters = null;
			var sqlRaw = query.GetSQLRaw(ref parameters);

			var rs = Context.Set<TEntity>().FromSqlRaw(sqlRaw, parameters).ToList();
			return rs;
		}
	}
}