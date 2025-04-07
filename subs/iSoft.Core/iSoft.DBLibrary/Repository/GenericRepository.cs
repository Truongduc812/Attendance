using System;
using System.Collections.Generic;
using System.Linq;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;
using System.Data.Entity.Validation;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using iSoft.Extensions.EntityFrameworkCore.UnitOfWork;
using System.Data.Common;
using iSoft.DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using iSoft.Common.Exceptions;
using iSoft.DBLibrary.SQLBuilder.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using iSoft.DBLibrary.SQLBuilder;
using Newtonsoft.Json.Linq;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;
using iSoft.DBLibrary.SQLBuilder.Enum;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Entity;

namespace iSoft.Extensions.EntityFrameworkCore.Repository
{


	public class GenericRepository<TContext, TEntity> : IDisposable, IGenericRepository<TEntity> where TEntity : BaseEntity where TContext : DbContext, new()
	{

		internal ISQLBuilder sqlBuilder;
		private Microsoft.EntityFrameworkCore.DbSet<TEntity> _entities;
		private string _errorMessage = string.Empty;
		public TContext Context { get; set; }
		private bool disposedValue;
		protected IUnitOfWork<TContext> _unitOfWork;
		public string tableName = "";
		public GenericRepository(IUnitOfWork<TContext> unitOfWork)
				: this(unitOfWork.Context)
		{
			this._unitOfWork = unitOfWork;
		}
		public GenericRepository(TContext context)
		{

			Context = context;
			this._unitOfWork = new UnitOfWork<TContext>(Context);
		}

		public virtual IQueryable<TEntity> Table
		{
			get { return Entities; }
		}
		protected virtual Microsoft.EntityFrameworkCore.DbSet<TEntity> Entities
		{
			get { return _entities ?? (_entities = Context.Set<TEntity>()); }
		}

		public IUnitOfWork<TContext> GetUnitOfWork()
		{
			return this._unitOfWork;
		}

    public virtual TEntity GetById(object entityID)
		{
			var ret = Context.Find<TEntity>(entityID);
			return ret;
		}

		public async virtual Task UpsertAsync(long entityId, TEntity entity, long? userId = null)
		{
			_unitOfWork.CreateTransaction();
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");
				if (Context == null || disposedValue)
					throw new NullReferenceException("Context is null");
				Context.ChangeTracker.Clear();
				if (entityId <= 0)
				{
					try_to_set_value_of_prop(entity, prop_name: "CreatedAt", DateTime.Now);
					try_to_set_value_of_prop(entity, prop_name: "CreatedBy", userId);
					Entities.Add(entity);
				}
				else
				{
					try_to_set_value_of_prop(entity, prop_name: "UpdatedAt", DateTime.Now);
					try_to_set_value_of_prop(entity, prop_name: "UpdatedBy", userId);
					Entities.Update(entity);
				}
				var result = Context.SaveChanges();
				_unitOfWork.Commit();
				//commented out call to SaveChanges as Context save changes will be 
				//called with Unit of work
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				_unitOfWork.Rollback();
				throw new DBException(dbEx);
			}
		}
		public virtual IEnumerable<TEntity> GetAll()
		{
			return Context.Set<TEntity>().AsEnumerable();
		}


		public virtual void Insert(TEntity entity, long? userId = null)
		{
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");

				try_to_set_value_of_prop(entity, prop_name: "Created", DateTime.Now, false);
				try_to_set_value_of_prop(entity, prop_name: "Inserted", DateTime.Now, false);
				try_to_set_value_of_prop(entity, prop_name: "CreatedAt", DateTime.Now);
				try_to_set_value_of_prop(entity, prop_name: "CreatedBy", userId);


				Entities.Add(entity);
				if (Context == null || disposedValue)
					throw new NullReferenceException("Context is null");
				Context.SaveChanges();
				//commented out call to SaveChanges as Context save changes will be 
				//called with Unit of work
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}

		public void BulkInsert(IEnumerable<TEntity> entities, long? userId = null)
		{
			try
			{
				if (entities == null)
				{
					throw new ArgumentNullException("entities");
				}
				//Context.Configuration.AutoDetectChangesEnabled = false;

				// auto update inserted and created
				foreach (var entity in entities)
				{
					try_to_set_value_of_prop(entity, prop_name: "Created", DateTime.Now, false);
					try_to_set_value_of_prop(entity, prop_name: "Inserted", DateTime.Now, false);
					try_to_set_value_of_prop(entity, prop_name: "CreatedAt", DateTime.Now);
					try_to_set_value_of_prop(entity, prop_name: "CreatedBy", userId);

				}

				Context.Set<TEntity>().AddRange(entities);
				Context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						_errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName,
											 validationError.ErrorMessage) + Environment.NewLine;
					}
				}
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}

		public virtual void Update(TEntity entity, long? userId = null)
		{
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");
				if (Context == null || disposedValue)
					throw new ArgumentNullException("Context");

				SetEntryModified(entity, userId);
				Context.SaveChanges(); //commented out call to SaveChanges as Context save changes will be called with Unit of work
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}

		private void try_to_set_value_of_prop(TEntity entity, string prop_name, object value, bool overWrite = true)
		{
			// try update field updated value
			try
			{
				if (overWrite)
				{
					var updated_prop = entity?.GetType().GetProperty(prop_name);
					updated_prop?.SetValue(entity, value);
				}
				else if (try_to_get_value_of_prop(entity, prop_name) is null)
				{
					var updated_prop = entity?.GetType().GetProperty(prop_name);
					updated_prop?.SetValue(entity, value);
				}
			}
			catch { }
		}
		private object try_to_get_value_of_prop(TEntity entity, string prop_name)
		{
			// try update field updated value
			try
			{
				var updated_prop = entity?.GetType().GetProperty(prop_name);
				var val = updated_prop?.GetValue(entity);
				return val;
			}
			catch { return null; }
		}

		public virtual void Update(IEnumerable<TEntity> entities, long? userId = null)
		{
			try
			{
				if (entities == null) throw new ArgumentNullException("entities");
				if (Context == null || disposedValue) throw new ArgumentNullException("Context");

				foreach (var entity in entities)
				{
					if (entity == null)
						throw new ArgumentNullException("entity");
					try_to_set_value_of_prop(entity, prop_name: "Updated", DateTime.Now);
					try_to_set_value_of_prop(entity, prop_name: "UpdatedAt", DateTime.Now);
					SetEntryModified(entity, userId);
				}
				Context.SaveChanges(); //commented out call to SaveChanges as Context save changes will be called with Unit of work
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}
		public virtual void SetEntryModified(TEntity entity, long? userId = null)
		{
			try_to_set_value_of_prop(entity, prop_name: "Updated", DateTime.Now);
			try_to_set_value_of_prop(entity, prop_name: "UpdatedAt", DateTime.Now);
			try_to_set_value_of_prop(entity, prop_name: "UpdatedBy", userId);

			Context.Entry(entity).State = EntityState.Modified;
		}

		public virtual void Delete(TEntity entity)
		{
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");
				if (Context == null || disposedValue)
					throw new ArgumentNullException("Context");
				Entities.Remove(entity);
				Context.SaveChanges();// commented out call to SaveChanges as Context save changes will be called with Unit of work
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}

		public bool IsExisted(TEntity obj)
		{
			return Entities.Any(x => x == obj);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					//Context.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return Context.Set<TEntity>().AsQueryable();
		}

		public async virtual Task<TEntity> GetByIdAsync(object id)
		{
			return await Context.FindAsync<TEntity>(id);
		}
		public async Task<int> InsertAsync(TEntity entity, long? userId = null)
		{
			//this._unitOfWork = new UnitOfWork<TContext>(Context);
			//await _unitOfWork.CreateTransactionAsync();
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");

				try_to_set_value_of_prop(entity, prop_name: "Created", DateTime.Now, false);
				try_to_set_value_of_prop(entity, prop_name: "CreatedAt", DateTime.Now, false);
				try_to_set_value_of_prop(entity, prop_name: "CreatedBy", userId, true);
				try_to_set_value_of_prop(entity, prop_name: "Inserted", DateTime.Now, false);

				await Entities.AddAsync(entity);
				if (Context == null || disposedValue)
					throw new NullReferenceException("Context is null");
				var result = await Context.SaveChangesAsync();
				//commented out call to SaveChanges as Context save changes will be 
				//called with Unit of work
				//await _unitOfWork.CommitAsync();
				return result;
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				//await _unitOfWork.RollbackAsync();
				throw new DBException(dbEx);
			}
		}

		public async Task<int> BulkInsertAsync(IEnumerable<TEntity> entities, long? userId = null)
		{
			try
			{
				if (entities == null)
				{
					throw new ArgumentNullException("entities");
				}
				//Context.Configuration.AutoDetectChangesEnabled = false;

				// auto update inserted and created
				foreach (var entity in entities)
				{
					try_to_set_value_of_prop(entity, prop_name: "Created", DateTime.Now, false);
					try_to_set_value_of_prop(entity, prop_name: "CreatedAt", DateTime.Now, false);
					try_to_set_value_of_prop(entity, prop_name: "CreatedBy", userId, true);
					try_to_set_value_of_prop(entity, prop_name: "Inserted", DateTime.Now, false);
				}

				await Context.Set<TEntity>().AddRangeAsync(entities);
				return await Context.SaveChangesAsync();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						_errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName,
											 validationError.ErrorMessage) + Environment.NewLine;
					}
				}
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}

		public async Task<int> UpdateAsync(TEntity entity, long? userId = null)
		{
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");
				if (Context == null || disposedValue)
					throw new ArgumentNullException("Context");

				SetEntryModified(entity, userId);
				return await Context.SaveChangesAsync(); //commented out call to SaveChanges as Context save changes will be called with Unit of work
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}

		public async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, long? userId = null)
		{
			await _unitOfWork.CreateTransactionAsync();
			try
			{
				if (entities == null) throw new ArgumentNullException("entities");
				if (Context == null || disposedValue) throw new ArgumentNullException("Context");

				foreach (var entity in entities)
				{
					if (entity == null)
						throw new ArgumentNullException("entity");
					try_to_set_value_of_prop(entity, prop_name: "Updated", DateTime.Now);
					try_to_set_value_of_prop(entity, prop_name: "UpdatedAt", DateTime.Now);
					try_to_set_value_of_prop(entity, prop_name: "UpdatedBy", userId);
				}
				Context.Set<TEntity>().UpdateRange(entities);
				await Context.SaveChangesAsync(); //commented out call to SaveChanges as Context save changes will be called with Unit of work
				await _unitOfWork.CommitAsync();
				return entities.Count();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				_unitOfWork.Rollback();
				throw new DBException(dbEx);
			}
		}

		public async Task<int> DeleteAsync(TEntity entity)
		{
			try
			{
				if (entity == null)
					throw new ArgumentNullException("entity");
				if (Context == null || disposedValue)
					throw new ArgumentNullException("Context");
				Entities.Remove(entity);
				return await Context.SaveChangesAsync();// commented out call to SaveChanges as Context save changes will be called with Unit of work
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}

		public IEnumerable<TEntity> GetByProperty(string table, string column, object value)
		{
			try
			{
				//this.sqlBuilder = new SQLServerSQLBuilder(); // TODO: input outside
				throw new NotImplementedException("new SQLServerSQLBuilder()");
				object[] parameters = null;
				var type = value.GetType();
				var sql = this.sqlBuilder.New().Selects("*").From(table).Where(new FieldName(column), value).GetSQLRaw(ref parameters);
				return Context.Set<TEntity>().FromSqlRaw<TEntity>(sql, parameters);
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}

		public async Task<IEnumerable<TEntity>> GetByPropertyAsync(string table, string column, object value)
		{
			try
      {
        //this.sqlBuilder = new SQLServerSQLBuilder(); // TODO: input outside
        throw new NotImplementedException("new SQLServerSQLBuilder()");
        object[] parameters = null;
				//var type = value.GetType();
				var sql = this.sqlBuilder.New().Selects("*").From(table).Where(new FieldName(column), value).GetSQLRaw(ref parameters);
				return Context.Set<TEntity>().FromSqlRaw<TEntity>(sql, parameters);
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}
		public async Task<IEnumerable<TEntity>> GetByLikePropertyAsync(string table, string column, object value)
		{
			try
      {
        //this.sqlBuilder = new SQLServerSQLBuilder(); // TODO: input outside
        throw new NotImplementedException("new SQLServerSQLBuilder()");
        object[] parameters = null;
				var type = value.GetType();
				var sql = this.sqlBuilder.New().Selects("*").From(table).WhereLike(new FieldName(table, column), value.ToString()).GetSQLRaw(ref parameters);
				return await Context.Set<TEntity>().FromSqlRaw<TEntity>(sql, parameters).ToListAsync();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}
		public async Task<IQueryable<TEntity>> GetByMultiPropertiesAsync(string tableName, Dictionary<string, object> columns)
		{
			try
      {
        //this.sqlBuilder = new SQLServerSQLBuilder(); // TODO: input outside
        throw new NotImplementedException("new SQLServerSQLBuilder()");
        object[] parameters = null;
				var query = this.sqlBuilder.New().Selects("*").From(tableName);
				foreach (var column in columns)
				{
					query.Where(new FieldName(column.Key), column.Value);
				}

				var sql = query.GetSQLRaw(ref parameters);
				return Context.Set<TEntity>().FromSqlRaw(sql, parameters);
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}
		public async Task<IQueryable<TEntity>> GetBetWeen(string tableName, string fieldName, object from, object to)
		{
			try
      {
        //this.sqlBuilder = new SQLServerSQLBuilder(); // TODO: input outside
        throw new NotImplementedException("new SQLServerSQLBuilder()");
        object[] parameters = null;
				var query = this.sqlBuilder.New().Selects("*").From(tableName);
				query = query.WhereBetween<DateTime>(new FieldName(fieldName), from, to);
				var sql = query.GetSQLRaw(ref parameters);
				return Context.Set<TEntity>().FromSqlRaw(sql, parameters);
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}
		public async Task<IQueryable<TEntity>> GetSkipAndTake(string tableName, int skip, int take)
		{
			try
      {
        //this.sqlBuilder = new SQLServerSQLBuilder(); // TODO: input outside
        throw new NotImplementedException("new SQLServerSQLBuilder()");
        var query = this.sqlBuilder.New().Selects("*")
					.From(tableName)
					.OrderBy(new FieldName("CreatedAt"), SQLSortOrder.DESC)
					.Offset(skip)
					.Limit(take);

				object[] parameters = null;
				var sql = query.GetSQLRaw(ref parameters);
				return Context.Set<TEntity>().FromSqlRaw(sql, parameters);
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
					foreach (var validationError in validationErrors.ValidationErrors)
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
				throw new DBException(_errorMessage, dbEx);
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}
	}
}
