using iSoft.DBLibrary.Entities;
using iSoft.Common.Models.RequestModels;
using SourceBaseBE.Database.DBContexts;
using iSoft.Common.Exceptions;
using SourceBaseBE.Database.Entities;
using System.Reflection;
using iSoft.Database.Entities;
using iSoft.Common;
using Microsoft.Extensions.Logging;
using iSoft.Redis.Services;
using iSoft.Database.Models;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Common.ConfigsNS;
using iSoft.DBLibrary.SQLBuilder;
using iSoft.DBLibrary.SQLBuilder.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SourceBaseBE.Database.Repository
{
	public class BaseCRUDRepository<TEntity> : IDisposable where TEntity : BaseCRUDEntity, new()
	{
		internal readonly CommonDBContext _context;
		private bool _disposedValue;

		internal ServerConfigModel _redisConfig;
		public static string cacheKeyList;
		public static string cacheKeyDetail;
		public static string cacheKeyTotalRecord;
		public static string cacheKeyFormData;
		public static string cacheKeyListWithNoInclude;
		public static string cacheKeyListByListIds;
		public static string cacheKeyAll;

		private string _tableName = nameof(TEntity);
		public virtual string TableName
		{
			get { return _tableName; }
		}

		internal void ClearCachedData()
		{
			CachedFunc.ClearRedisByEntity(GetName());
		}
		public virtual string GetName()
		{
			return nameof(BaseCRUDRepository<TEntity>);
		}
		public BaseCRUDRepository(CommonDBContext context)
		{
			_context = context;
			_redisConfig = CommonConfig.GetConfig().RedisConfig;
			CachedFunc.SetRedisConfig(_redisConfig);
			cacheKeyList = $"{ConstCommon.ConstSourceBaseBECacheMainService}_{GetName()}_list";
			cacheKeyDetail = $"{ConstCommon.ConstSourceBaseBECacheMainService}_{GetName()}_detail";
			cacheKeyTotalRecord = $"{ConstCommon.ConstSourceBaseBECacheMainService}_{GetName()}_total";
			cacheKeyFormData = $"{ConstCommon.ConstSourceBaseBECacheMainService}_{GetName()}_formData";
			cacheKeyListWithNoInclude = $"{ConstCommon.ConstSourceBaseBECacheMainService}_{GetName()}_no_include";
			cacheKeyListByListIds = $"{ConstCommon.ConstSourceBaseBECacheMainService}_{GetName()}_list_ids";
			cacheKeyAll = $"{ConstCommon.ConstSourceBaseBECacheMainService}_{GetName()}_all";

		}
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					//// TODO: dispose managed state (managed objects)
					//_context.Dispose();

					var fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
					foreach (var field in fields)
					{
						var type = (field.GetValue(this) as IDisposable);
						type?.Dispose();
					}
				}

				_disposedValue = true;
			}
		}
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		public virtual string GetDisplayField(TEntity entity)
		{
			return entity.Id.ToString();
		}
		public virtual bool IsExists(long Id)
		{
			if (GetById(Id) != null)
			{
				return true;
			}
			return false;
		}
		public virtual TEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				TEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<TEntity>(cacheKey, null);
				//}

				//if (result == null)
				//    {
				var dataSet = _context.Set<TEntity>();
				IQueryable<TEntity> queryable;
				if (!isTracking)
				{
					queryable = dataSet.AsNoTracking().AsQueryable();
				}
				else
				{
					queryable = dataSet.AsQueryable();
				}
				result = queryable
					.Where(entity => entity.DeletedFlag != true && entity.Id == id)
					.FirstOrDefault();

				//	CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
				//	CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
				//}
				return result;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual async Task<TEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				TEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<TEntity>(cacheKey, null);
				//}

				//if (result == null)
				//    {
				var dataSet = _context.Set<TEntity>();
				IQueryable<TEntity> queryable;
				if (!isTracking)
				{
					queryable = dataSet.AsNoTracking().AsQueryable();
				}
				else
				{
					queryable = dataSet.AsQueryable();
				}
				result = queryable
					.Where(entity => entity.DeletedFlag != true && entity.Id == id)
					.FirstOrDefault();
				//	CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
				//	CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
				//}
				return result;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}

		public virtual long GetTotalRecord()
		{
			try
			{
				long? result = CachedFunc.GetRedisData<long>(cacheKeyTotalRecord, -1);
				if (result == -1)
				{
					result = 0;
					result = (from entity in _context.Set<TEntity>()
							  where entity.DeletedFlag != true
							  select entity)
					  .LongCount();

					CachedFunc.AddEntityCacheKey(GetName(), cacheKeyTotalRecord, true);
					CachedFunc.SetRedisData(cacheKeyTotalRecord, result, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
				}
				return result.Value;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual List<TEntity> GetListByListIds(List<long> Ids, bool isDirect = false)
		{
			try
			{
				List<TEntity>? result = null;
				string cacheKey = $"{cacheKeyListByListIds}:{string.Join(",", Ids)}";
				if (!isDirect)
				{
					result = CachedFunc.GetRedisData<List<TEntity>>(cacheKey, null);
				}

				if (result == null)
				{
					result = new List<TEntity>();

					result = _context.Set<TEntity>()
					  .AsQueryable()
					  .Where(entity => entity.DeletedFlag != true && Ids.Contains(entity.Id))
					  .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
					  .AsParallel()
					  .ToList();

					CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
					CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
				}
				return result;
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}
		public virtual List<TEntity> GetListWithNoInclude(PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyListWithNoInclude}";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyListWithNoInclude}:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<TEntity>? result = CachedFunc.GetRedisData<List<TEntity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<TEntity>();
					if (pagingReq != null)
					{
						result = _context.Set<TEntity>()
						  .AsNoTracking()
						  .AsQueryable()
						  .Where(entity => entity.DeletedFlag != true)
						  .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
						  .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
						  .AsParallel()
						  .ToList();
					}
					else
					{
						result = _context.Set<TEntity>()
						  .AsNoTracking()
						  .AsQueryable()
						  .Where(entity => entity.DeletedFlag != true)
						  .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
						  .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
						  .AsParallel()
						  .ToList();
					}

					CachedFunc.AddEntityCacheKey(GetName(), cacheKey, false);
					CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetListNotIncludeCacheExpiredTimeInSeconds);
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual List<FormSelectOptionModel> GetSelectData(string entityName, string category)
		{
			List<FormSelectOptionModel> rs = this.GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
			return rs;
		}


		public virtual List<TEntity> GetList(PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<TEntity> result = CachedFunc.GetRedisData<List<TEntity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<TEntity>();
					if (pagingReq != null || pagingReq.PageSize <= 0)
					{
						result = _context.Set<TEntity>()
						  .AsNoTracking()
						  .AsQueryable()
						  .Where(entity => entity.DeletedFlag != true)
						  .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
						  .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
						  .AsParallel()
						  .ToList();
					}
					else
					{
						result = _context.Set<TEntity>()
						  .AsNoTracking()
						  .AsQueryable()
						  .Where(entity => entity.DeletedFlag != true)
						  .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
						  .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
						  .AsParallel()
						  .ToList();
					}

					CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
					CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}

		public virtual List<TEntity> GetAll()
		{
			try
			{
				List<TEntity>? result = CachedFunc.GetRedisData<List<TEntity>>(cacheKeyAll, null);
				if (result == null)
				{
					result = new List<TEntity>();
					result = _context.Set<TEntity>()
					  .AsNoTracking()
					  .AsQueryable()
					  .Where(entity => entity.DeletedFlag != true)
					  .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
					  .AsParallel()
					  .ToList();

					CachedFunc.AddEntityCacheKey(GetName(), cacheKeyAll, true);
					CachedFunc.SetRedisData(cacheKeyAll, result, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual TEntity Upsert(TEntity entity, long? userId = null, bool isTracked = false)
		{
			try
			{
				if (entity.Id <= 0)
				{
					// Insert
					entity = Insert(entity, userId);
				}
				else
				{
					// Update
					entity = Update(entity, userId, isTracked);
				}
				return entity;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual TEntity Insert(TEntity entity, long? userId = null)
		{
			try
			{
				if (entity.Id > 0)
				{
					throw new DBException($"Insert, Unexpected Id in entity, Id={entity.Id}");
				}
				else
				{
					if (userId != null)
					{
						entity.CreatedBy = userId;
					}
					entity.CreatedAt = DateTime.Now;
					entity.UpdatedBy = entity.CreatedBy;
					entity.UpdatedAt = entity.CreatedAt;
					entity.DeletedFlag = false;
					entity = _context.Set<TEntity>().Add(entity).Entity;
				}
				var result = _context.SaveChanges();
				ClearCachedData();
				return entity;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual TEntity Update(TEntity entity, long? userId = null, bool isTracked = false)
		{
			try
			{
				if (entity.Id <= 0)
				{
					throw new DBException("Update, Id not found in entity");
				}
				else
				{
					if (userId != null)
					{
						entity.UpdatedBy = userId;
					}
					_context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
					entity.UpdatedAt = DateTime.Now;
					this._context.ChangeTracker.Clear();
					//if (isTracked)
					entity = _context.Set<TEntity>().Update(entity).Entity;
				}
				var result = _context.SaveChanges();
				ClearCachedData();
				return entity;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual IEnumerable<TEntity> InsertMulti(IEnumerable<TEntity> entities, long? userId = null)
		{
			try
			{
				foreach (var entity in entities)
				{
					if (entity.Id > 0)
					{
						throw new DBException("Insert, Unexpected Id in entity");
					}
					else
					{
						if (userId != null)
						{
							entity.CreatedBy = userId;
						}
						entity.CreatedAt = DateTime.Now;
						entity.UpdatedBy = entity.CreatedBy;
						entity.UpdatedAt = entity.CreatedAt;
						entity.DeletedFlag = false;
						_context.Set<TEntity>().Add(entity);
					}
				}
				var result = _context.SaveChanges();
				ClearCachedData();
				return entities;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual IEnumerable<TEntity> UpdateMulti(IEnumerable<TEntity> entities, long? userId = null)
		{
			try
			{
				foreach (var entity in entities)
				{
					if (entity.Id <= 0)
					{
						throw new DBException("Update, Id not found in entity");
					}
					else
					{
						if (userId != null)
						{
							entity.UpdatedBy = userId;
						}
						entity.UpdatedAt = DateTime.Now;
						_context.Set<TEntity>().Update(entity);
					}
				}
				var result = _context.SaveChanges();
				ClearCachedData();
				return entities;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}

		public virtual IEnumerable<TEntity> DeleteMulti(IEnumerable<TEntity> entities, long? userId = null, bool isSoftDelete = true)
		{
			try
			{
				foreach (var entity in entities)
				{
					if (entity.Id <= 0)
					{
						throw new DBException("Delete, Id not found in entity");
					}
					else
					{
						if (isSoftDelete)
						{
							if (userId != null)
							{
								entity.UpdatedBy = userId;
							}
							entity.UpdatedAt = DateTime.Now;
							entity.DeletedFlag = true;
							_context.Set<TEntity>().Update(entity);
						}
						else
						{
							_context.Set<TEntity>().Remove(entity);
						}
					}
				}
				var result = _context.SaveChanges();
				ClearCachedData();
				return entities;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}

		public virtual void DeleteAll(long? userId = null, bool isSoftDelete = true)
		{
			try
			{
				if (isSoftDelete)
				{
					ISQLBuilder query = BaseSQLBuilder.GetSQLBuilderInstance(this._context.dbConnectionCustom.GetDBConfig().DatabaseType)
					  .New()
					  .Update(this.TableName)
					  .Set(new FieldName(nameof(BaseCRUDEntity.DeletedFlag)), true)
					  .Set(new FieldName(nameof(BaseCRUDEntity.UpdatedAt)), DateTime.UtcNow)
					  .Set(new FieldName(nameof(BaseCRUDEntity.UpdatedBy)), userId)
					  ;

					object[] parameters = null;
					var sql = query.GetSQLRaw(ref parameters);
					var count = this._context.Database.ExecuteSqlRaw(sql, parameters);
				}
				else
				{
					ISQLBuilder query = BaseSQLBuilder.GetSQLBuilderInstance(this._context.dbConnectionCustom.GetDBConfig().DatabaseType)
					  .New()
					  .Delete(this.TableName)
					  ;

					object[] parameters = null;
					var sql = query.GetSQLRaw(ref parameters);
					var count = this._context.Database.ExecuteSqlRaw(sql, parameters);
				}
				ClearCachedData();
				return;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual int Delete(TEntity entity, long? userId = null, bool isSoftDelete = true)
		{
			try
			{
				if (entity.Id > 0)
				{
					if (isSoftDelete)
					{
						if (userId != null)
						{
							entity.UpdatedBy = userId;
						}
						entity.UpdatedAt = DateTime.Now;
						entity.DeletedFlag = true;
						_context.Set<TEntity>().Update(entity);
					}
					else
					{
						_context.Set<TEntity>().Remove(entity);
					}
					var result = _context.SaveChanges();
					ClearCachedData();
					return result;
				}
				return 0;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual int Delete(long id, long? userId = null, bool isSoftDelete = true)
		{
			try
			{
				if (id > 0)
				{
					TEntity? entity = GetById(id, true);
					if (entity == null)
					{
						return 0;
					}
					return Delete(entity, userId, isSoftDelete);
				}
				return 0;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public virtual List<TEntity2> MergeChildrenEntity<TEntity2>(List<TEntity2> listCurrent, List<TEntity2> listNew)
		  where TEntity2 : BaseCRUDEntity
		{
			Dictionary<long, TEntity2> dictGenTemplate = new Dictionary<long, TEntity2>();
			Dictionary<long, TEntity2> dictNew = new Dictionary<long, TEntity2>();
			if (listCurrent == null)
			{
				listCurrent = new List<TEntity2>();
			}
			else
			{
				dictGenTemplate = listCurrent.ToDictionary(x => x.Id);
			}

			if (listNew == null)
			{
				listCurrent.Clear();
			}
			else
			{
				foreach (var newItem in listNew)
				{
					// Not exists in current list
					if (!dictGenTemplate.ContainsKey(newItem.Id))
					{
						listCurrent.Add(newItem);
					}
				}

				dictNew = listNew.ToDictionary(x => x.Id);
				for (var i = listCurrent.Count - 1; i >= 0; i--)
				{
					// Not exists in new list
					if (!dictNew.ContainsKey(listCurrent[i].Id))
					{
						listCurrent.RemoveAt(i);
					}
				}
			}
			return listCurrent;
		}
		public virtual IEnumerable<TEntity> UpSertMulti(IEnumerable<TEntity> entities, long? userId = null)
		{
			try
			{
				if (entities != null && entities.Count() > 0)
				{
					foreach (var entity in entities)
					{
						Upsert(entity);
					}
				}

				return entities;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		
	}
}
