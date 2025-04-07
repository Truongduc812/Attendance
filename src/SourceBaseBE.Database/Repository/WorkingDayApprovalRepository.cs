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
	public class WorkingDayApprovalRepository : BaseCRUDRepository<WorkingDayApprovalEntity>
	{
		public WorkingDayApprovalRepository(CommonDBContext dbContext)
			: base(dbContext)
		{
		}
		public override string GetName()
		{
			return nameof(WorkingDayApprovalRepository);
		}
		/// <summary>
		/// GetById (@GenCRUD)
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		/// <exception cref="DBException"></exception>
		public override WorkingDayApprovalEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				WorkingDayApprovalEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<WorkingDayApprovalEntity>(cacheKey, null);
				//}

				//if (result == null)
    //    {
          var dataSet = _context.Set<WorkingDayApprovalEntity>();
          IQueryable<WorkingDayApprovalEntity> queryable;
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
					//result.WorkingDayApproval2s = result.WorkingDayApproval2s.Select(x => new WorkingDayApproval2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		public override async Task<WorkingDayApprovalEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
		{
			try
			{
				WorkingDayApprovalEntity? result = null;
				//string cacheKey = $"{cacheKeyDetail}:{id}";
				//if (!isDirect)
				//{
				//	result = CachedFunc.GetRedisData<WorkingDayApprovalEntity>(cacheKey, null);
				//}

				//if (result == null)
				//{
					result = await (isTracking ?
					_context.Set<WorkingDayApprovalEntity>()
							  //.AsNoTracking()
							  .AsQueryable()
							  /*[GEN-7]*/
							  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefaultAsync() :
							  _context.Set<WorkingDayApprovalEntity>()
							  .AsNoTracking()
							  .AsQueryable()
							  /*[GEN-7]*/
							  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
							  .FirstOrDefaultAsync());
					//result.WorkingDayApproval2s = result.WorkingDayApproval2s.Select(x => new WorkingDayApproval2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
		public override List<WorkingDayApprovalEntity> GetList(PagingRequestModel pagingReq = null)
		{
			try
			{
				string cacheKey = $"{cacheKeyList}_GetList";
				if (pagingReq != null)
				{
					cacheKey = $"{cacheKeyList}_GetList:{pagingReq.Page}|{pagingReq.PageSize}";
				}
				List<WorkingDayApprovalEntity>? result = CachedFunc.GetRedisData<List<WorkingDayApprovalEntity>>(cacheKey, null);
				if (result == null)
				{
					result = new List<WorkingDayApprovalEntity>();
					if (pagingReq != null)
					{
						result = _context.Set<WorkingDayApprovalEntity>()
								.AsNoTracking()
								.AsQueryable()
								/*[GEN-11]*/
								.Where(entity => entity.DeletedFlag != true)
								.OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
								.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
								.AsParallel()
								.ToList();

						//.Select(entity => new WorkingDayApprovalEntity
						// {
						//   Id = entity.Id,
						//   WorkingDayApproval2s = entity.WorkingDayApproval2s.Select(x => new WorkingDayApproval2Entity { Id = x.Id, Name = x.Name }).ToList()
						// })

						for (var i = 0; i < result.Count; i++)
						{


							/*[GEN-12]*/
						}
					}
					else
					{
						result = _context.Set<WorkingDayApprovalEntity>()
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
        public List<WorkingDayApprovalEntity> GetList(List<long>? ids = null)
        {
            try
            {
                var query = _context.Set<WorkingDayApprovalEntity>()
                    .AsNoTracking()
                    .AsQueryable()
                    .Where(entity => entity.DeletedFlag != true);

                if (ids != null && ids.Count > 0)
                {
                    query = query.Where(entity => ids.Contains(entity.Id));
                }

                var result = query
                    .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                    .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
                    //.AsParallel()
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public WorkingDayApprovalEntity Upsert(WorkingDayApprovalEntity entity/*[GEN-8]*/, long? userId = null)
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
		public WorkingDayApprovalEntity Insert(WorkingDayApprovalEntity entity/*[GEN-8]*/, long? userId = null)
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
					_context.Set<WorkingDayApprovalEntity>().Add(entity);
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
		public WorkingDayApprovalEntity Update(WorkingDayApprovalEntity entity/*[GEN-8]*/, long? userId = null)
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
					base.Update(entity);
				}
				var result = _context.SaveChanges();
				return entity;
			}
			catch (Exception ex)
			{
				throw new DBException(ex);
			}
		}
		public override string GetDisplayField(WorkingDayApprovalEntity entity)
		{
			return entity.ToString();
		}
		public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
		{
			List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
			return rs;
		}
		public Task<WorkingDayApprovalEntity> GetUserRequestChange(long userId, long workingdayUpdateId)
		{
			return _context.Set<WorkingDayApprovalEntity>()
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.ApproverId == userId && x.WorkingDayUpdateId == workingdayUpdateId);
		}
	}
}
