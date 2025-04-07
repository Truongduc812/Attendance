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

namespace SourceBaseBE.Database.Repository
{
	public class ParameterRepository : BaseCRUDRepository<ParameterEntity>
	{
		public ParameterRepository(CommonDBContext dbContext)
			: base(dbContext)
		{
		}
		public override string GetName()
		{
			return nameof(ParameterRepository);
		}
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override ParameterEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				ParameterEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<ParameterEntity>(cacheKey, null);
				//}

				//if (result == null)
    //    {
          var dataSet = _context.Set<ParameterEntity>();
          IQueryable<ParameterEntity> queryable;
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
					//result.Parameter2s = result.Parameter2s.Select(x => new Parameter2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		public override async Task<ParameterEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				ParameterEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<ParameterEntity>(cacheKey, null);
				//}

				//if (result == null)
				//{
					result = await (isTracking ?
					_context.Set<ParameterEntity>()
							  //.AsNoTracking()
							  .AsQueryable()
							  /*[GEN-7]*/
							  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefaultAsync() :
							  _context.Set<ParameterEntity>()
							  .AsNoTracking()
							  .AsQueryable()
							  /*[GEN-7]*/
							  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefaultAsync());
					//result.Parameter2s = result.Parameter2s.Select(x => new Parameter2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		public override List<ParameterEntity> GetList(PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<ParameterEntity>? result = CachedFunc.GetRedisData<List<ParameterEntity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<ParameterEntity>();
					if (pagingReq != null)
					{
						result = _context.Set<ParameterEntity>()
								.AsNoTracking()
								.AsQueryable()
								/*[GEN-11]*/
								.Where(entity => entity.DeletedFlag != true)
								.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
								.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
								.AsParallel()
								.ToList();

					}
					else
					{
						result = _context.Set<ParameterEntity>()
								.AsNoTracking()
								.AsQueryable()
								/*[GEN-13]*/
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
		public List<ParameterEntity> GetListByDeviceId(int DeviceId, PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<ParameterEntity>? result = CachedFunc.GetRedisData<List<ParameterEntity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<ParameterEntity>();
					if (pagingReq != null)
					{
						result = _context.Set<ParameterEntity>()
								.AsNoTracking()
								.AsQueryable()
								.Include(x => x.Device)
								/*[GEN-11]*/
								.Where(entity => entity.DeletedFlag != true && entity.DeviceId == DeviceId)
								.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
								.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
								.AsParallel()
								.ToList();

					}
					else
					{
						result = _context.Set<ParameterEntity>()
								.AsNoTracking()
								.AsQueryable()
								.Include(x => x.Device)
								/*[GEN-13]*/
								.Where(entity => entity.DeletedFlag != true && entity.DeviceId == DeviceId)
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
		public ParameterEntity Upsert(ParameterEntity entity/*[GEN-8]*/, long? userId = null)
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
		public ParameterEntity Insert(ParameterEntity entity/*[GEN-8]*/, long? userId = null)
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
					_context.Set<ParameterEntity>().Add(entity);
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
		public ParameterEntity Update(ParameterEntity entity/*[GEN-8]*/, long? userId = null)
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
					_context.Set<ParameterEntity>().Update(entity);
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
		public override string GetDisplayField(ParameterEntity entity)
		{
			return entity.EnviromentVarName.ToString();
		}
		public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
		{
			List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
			return rs;
		}
	}
}
