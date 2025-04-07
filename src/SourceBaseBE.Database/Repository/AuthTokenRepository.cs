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
using iSoft.Common.Models.ConfigModel.Subs;

namespace SourceBaseBE.Database.Repository
{
	public class AuthTokenRepository : BaseCRUDRepository<AuthTokenEntity>
	{
		public AuthTokenRepository(CommonDBContext dbContext)
			: base(dbContext)
		{
		}
		public override string GetName()
		{
			return nameof(AuthTokenRepository);
		}
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override AuthTokenEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				AuthTokenEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<AuthTokenEntity>(cacheKey, null);
				//}

				//if (result == null)
    //    {
          var dataSet = _context.Set<AuthTokenEntity>();
          IQueryable<AuthTokenEntity> queryable;
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
					//result.AuthToken2s = result.AuthToken2s.Select(x => new AuthToken2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		public override List<AuthTokenEntity> GetList(PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<AuthTokenEntity>? result = CachedFunc.GetRedisData<List<AuthTokenEntity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<AuthTokenEntity>();
					if (pagingReq != null)
					{
						result = _context.Set<AuthTokenEntity>()
								.AsNoTracking()
								.AsQueryable()
								/*[GEN-11]*/
								.Where(entity => entity.DeletedFlag != true)
								.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
								.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
								.AsParallel()
								.ToList();

						//.Select(entity => new AuthTokenEntity
						// {
						//   Id = entity.Id,
						//   AuthToken2s = entity.AuthToken2s.Select(x => new AuthToken2Entity { Id = x.Id, Name = x.Name }).ToList()
						// })

						for (var i = 0; i < result.Count; i++)
						{


							/*[GEN-12]*/
						}
					}
					else
					{
						result = _context.Set<AuthTokenEntity>()
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
		public AuthTokenEntity Upsert(AuthTokenEntity entity/*[GEN-8]*/, long? userId = null)
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
		public AuthTokenEntity Insert(AuthTokenEntity entity/*[GEN-8]*/, long? userId = null)
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
					_context.Set<AuthTokenEntity>().Add(entity);
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
		public AuthTokenEntity Update(AuthTokenEntity entity/*[GEN-8]*/, long? userId = null)
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
					_context.Set<AuthTokenEntity>().Update(entity);
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
		public override string GetDisplayField(AuthTokenEntity entity)
		{
			return entity.AccessToken.ToString();
		}
		public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
		{
			List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
			return rs;
		}
	}
}
