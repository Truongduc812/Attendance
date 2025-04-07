using iSoft.Common;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.RequestModels;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using iSoft.Redis.Services;
using SourceBaseBE.Database.DBContexts;
using SourceBaseBE.Database.Entities;
using iSoft.Database.Models;
using UserEntity = SourceBaseBE.Database.Entities.UserEntity;
using ISoftProjectEntity = SourceBaseBE.Database.Entities.ISoftProjectEntity;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Common.Models.ResponseModels;

namespace SourceBaseBE.Database.Repository
{
	public class LanguageRepository : BaseCRUDRepository<LanguageEntity>
	{
		public LanguageRepository(CommonDBContext dbContext)
			: base(dbContext)
		{
		}
		public override string GetName()
		{
			return nameof(LanguageRepository);
		}
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override LanguageEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				LanguageEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<LanguageEntity>(cacheKey, null);
				//}

				//if (result == null)
    //    {
          var dataSet = _context.Set<LanguageEntity>();
          IQueryable<LanguageEntity> queryable;
          if (!isTracking)
          {
            queryable = dataSet.AsNoTracking().AsQueryable();
          }
          else
          {
            queryable = dataSet.AsQueryable();
          }
          result = queryable
                /*[GEN-7]*/
                .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefault();
					//result.Language2s = result.Language2s.Select(x => new Language2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override async Task<LanguageEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				LanguageEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<LanguageEntity>(cacheKey, null);
				//}

				//if (result == null)
				//{
					result = await (isTracking ?
					_context.Set<LanguageEntity>()
							  //.AsNoTracking()
							  .AsQueryable()
							  /*[GEN-7]*/
							  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefaultAsync() :
							  _context.Set<LanguageEntity>()
							  .AsNoTracking()
							  .AsQueryable()
							  /*[GEN-7]*/
							  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefaultAsync());
					//result.Language2s = result.Language2s.Select(x => new Language2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		/// <summary>
		/// GetList (@GenCRUD)
		/// </summary>
		/// <param name="pagingReq"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override List<LanguageEntity> GetList(PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<LanguageEntity>? result = CachedFunc.GetRedisData<List<LanguageEntity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<LanguageEntity>();
					if (pagingReq != null)
					{
						result = _context.Set<LanguageEntity>()
								.AsNoTracking()
								.AsQueryable()
								/*[GEN-11]*/
								.Where(entity => entity.DeletedFlag != true)
								.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
								.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
								.AsParallel()
								.ToList();

						//.Select(entity => new LanguageEntity
						// {
						//   Id = entity.Id,
						//   Language2s = entity.Language2s.Select(x => new Language2Entity { Id = x.Id, Name = x.Name }).ToList()
						// })

						for (var i = 0; i < result.Count; i++)
						{


							/*[GEN-12]*/
						}
					}
					else
					{
						result = _context.Set<LanguageEntity>()
								.AsNoTracking()
								.AsQueryable()
								/*[GEN-13]*/
								.Where(entity => entity.DeletedFlag != true)
								.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
								.Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
								.AsParallel()
								.ToList();

						for (var i = 0; i < result.Count; i++)
						{


							/*[GEN-14]*/
						}
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
		public LanguageEntity Upsert(LanguageEntity entity/*[GEN-8]*/, long? userId = null)
		{
			try
			{
				if (entity.Id <= 0)
				{
					// Insert
					entity = Insert(entity/*[GEN-4]*/, userId);
				}
				else
				{
					// Update
					entity = Update(entity/*[GEN-4]*/, userId);
				}
				return entity;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public LanguageEntity Insert(LanguageEntity entity/*[GEN-8]*/, long? userId = null)
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

					/*[GEN-10]*/
					_context.Set<LanguageEntity>().Add(entity);
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
		public LanguageEntity Update(LanguageEntity entity/*[GEN-8]*/, long? userId = null)
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
					entity.UpdatedAt = DateTime.Now;

					/*[GEN-9]*/
					_context.Set<LanguageEntity>().Update(entity);
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
		public override string GetDisplayField(LanguageEntity entity)
		{
			return entity.Id.ToString();
		}
		public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
		{
			List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
			return rs;
		}

		public List<ColumnResponseModel> ReadColumnLanguage(List<string> colDisplays, List<string> colKeys, string lang)
		{
			try
			{

				//* query language in SourceBaseBE
				Dictionary<string, string> dataCommon = this._context.Set<LanguageEntity>()
					.Where(e => e.DeletedFlag != true)
					.Where(e => e.Language == lang)
					.Where(e => colDisplays.Contains(e.Key))
					.ToDictionary(e => e.Key, e => e.DisplayName);
				//* override data SourceBaseBE into common
				List<ColumnResponseModel> results = new List<ColumnResponseModel>();
				int i = 0;
				foreach (string key in colKeys)
				{
					var col = new ColumnResponseModel();
					col.Key = key;

					//* check colum has category = SourceBaseBE
					if (dataCommon.ContainsKey(key) && dataCommon[key] != null)
					{
						col.DisplayName = dataCommon[key];
						col.Key = colKeys[i++];
					}
					else
					{
						col.DisplayName = colDisplays[i];
						col.Key = colKeys[i++];

					}

					results.Add(col);
				}

				return results;
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}
		public List<ColumnResponseModel> ReadColumnLanguage(Dictionary<string, ColumnResponseModel> cols, string lang)
		{
			try
			{
				//* remove null col
				var colKeys = cols.Keys.ToList();
				var colDisplays = cols.Values.ToList();
				// remove non display field

				//* query language in SourceBaseBE
				Dictionary<string, string> dataCommon = this._context.Set<LanguageEntity>()
					.Where(e => e.DeletedFlag != true)
					.Where(e => e.Language == lang)
					.Where(e => colKeys.Contains(e.Key))
					.ToDictionary(e => e.Key, e => e.DisplayName);
				//* override data SourceBaseBE into common
				List<ColumnResponseModel> results = new List<ColumnResponseModel>();
				int i = 0;
				foreach (string key in colKeys)
				{
					var col = new ColumnResponseModel();
					col.Key = key;
					//* check colum has category = SourceBaseBE
					if (dataCommon.ContainsKey(key) && dataCommon[key] != null)
					{
						col.DisplayName = dataCommon[key];
						col.Key = colKeys[i];
					}
					else
					{
						col.DisplayName = colDisplays[i].DisplayName;
						col.Key = colDisplays[i].Key;
					}
					col.Displayable = colDisplays[i].Displayable;
					results.Add(col);
					i++;
				}

				return results;
			}
			catch (Exception dbEx)
			{
				throw new DBException(dbEx);
			}
		}
	}
}
