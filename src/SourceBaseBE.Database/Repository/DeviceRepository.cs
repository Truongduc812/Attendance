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
	public class DeviceRepository : BaseCRUDRepository<DeviceEntity>
	{
		public DeviceRepository(CommonDBContext dbContext)
			: base(dbContext)
		{
		}
		public override string GetName()
		{
			return nameof(DeviceRepository);
		}
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override DeviceEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				DeviceEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<DeviceEntity>(cacheKey, null);
				//}

				//if (result == null)
				//    {
				var dataSet = _context.Set<DeviceEntity>();
				IQueryable<DeviceEntity> queryable;
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
				//result.Device2s = result.Device2s.Select(x => new Device2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		public override async Task<DeviceEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				DeviceEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<DeviceEntity>(cacheKey, null);
				//}

				//if (result == null)
				//    {
				var dataSet = _context.Set<DeviceEntity>();
				IQueryable<DeviceEntity> queryable;
				if (!isTracking)
				{
					queryable = dataSet.AsNoTracking().AsQueryable();
				}
				else
				{
					queryable = dataSet.AsQueryable();
				}
				result = await queryable
							.Where(entity => entity.DeletedFlag != true && entity.Id == id)
							.FirstOrDefaultAsync();

				//result.GenTemplate2s = result.GenTemplate2s.Select(x => new GenTemplate2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		public new List<DeviceEntity> GetList(PagingRequestModel pagingReq = null, bool isHardGet = false)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<DeviceEntity>? result = !isHardGet ? CachedFunc.GetRedisData<List<DeviceEntity>>(cacheKey, null) : null;
				if (result == null)
				{
					result = new List<DeviceEntity>();
					if (pagingReq != null)
					{
						result = _context.Set<DeviceEntity>()
								/*[GEN-11]*/
								.Where(entity => entity.DeletedFlag != true)
								.Include(x => x.Parameters)
								//.ThenInclude(x => x.Limitations)
								.AsNoTracking()
								.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
								.AsParallel()
								.ToList();


					}
					else
					{
						result = _context.Set<DeviceEntity>()
								.AsNoTracking()
								.AsQueryable()
								.Include(x => x.Parameters)
								//.ThenInclude(x => x.Limitations)
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
		public DeviceEntity Upsert(DeviceEntity entity/*[GEN-8]*/, long? userId = null)
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
		public DeviceEntity Insert(DeviceEntity entity/*[GEN-8]*/, long? userId = null)
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
					_context.Set<DeviceEntity>().Add(entity);
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
		public DeviceEntity Update(DeviceEntity entity/*[GEN-8]*/, long? userId = null)
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
					_context.Set<DeviceEntity>().Update(entity);
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
		public override string GetDisplayField(DeviceEntity entity)
		{
			return entity.Name.ToString();
		}
		public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
		{
			List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
			return rs;
		}
	}
}
