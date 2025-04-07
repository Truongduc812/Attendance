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
    public class TimeSheetApprovalRepository : BaseCRUDRepository<TimeSheetApprovalEntity>
    {
        public TimeSheetApprovalRepository(CommonDBContext dbContext)
            : base(dbContext)
        {
        }
        public override string GetName()
        {
            return nameof(TimeSheetApprovalRepository);
        }
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override TimeSheetApprovalEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                TimeSheetApprovalEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<TimeSheetApprovalEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<TimeSheetApprovalEntity>();
                IQueryable<TimeSheetApprovalEntity> queryable;
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
                //result.TimeSheetApproval2s = result.TimeSheetApproval2s.Select(x => new TimeSheetApproval2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public override async Task<TimeSheetApprovalEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                TimeSheetApprovalEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<TimeSheetApprovalEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<TimeSheetApprovalEntity>();
                IQueryable<TimeSheetApprovalEntity> queryable;
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

                //result.TimeSheetApproval2s = result.TimeSheetApproval2s.Select(x => new TimeSheetApproval2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public override List<TimeSheetApprovalEntity> GetList(PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<TimeSheetApprovalEntity>? result = CachedFunc.GetRedisData<List<TimeSheetApprovalEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<TimeSheetApprovalEntity>();
                    if (pagingReq != null)
                    {
                        result = _context.Set<TimeSheetApprovalEntity>()
                                .AsNoTracking()
                                .AsQueryable()
                                /*[GEN-11]*/
                                .Where(entity => entity.DeletedFlag != true)
                                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                                .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                                .AsParallel()
                                .ToList();

                        //.Select(entity => new TimeSheetApprovalEntity
                        // {
                        //   Id = entity.Id,
                        //   TimeSheetApproval2s = entity.TimeSheetApproval2s.Select(x => new TimeSheetApproval2Entity { Id = x.Id, Name = x.Name }).ToList()
                        // })

                        for (var i = 0; i < result.Count; i++)
                        {


                            /*[GEN-12]*/
                        }
                    }
                    else
                    {
                        result = _context.Set<TimeSheetApprovalEntity>()
                                .AsNoTracking()
                                .AsQueryable()
                                /*[GEN-13]*/
                                .Where(entity => entity.DeletedFlag != true)
                                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                                .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
                                .AsParallel()
                                .ToList();

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        public List<TimeSheetApprovalEntity> GetList(List<long>? ids = null)
        {
            try
            {
                var query = _context.Set<TimeSheetApprovalEntity>()
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

        public TimeSheetApprovalEntity Upsert(TimeSheetApprovalEntity entity/*[GEN-8]*/, long? userId = null)
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
        public TimeSheetApprovalEntity Insert(TimeSheetApprovalEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<TimeSheetApprovalEntity>().Add(entity);
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
        public TimeSheetApprovalEntity Update(TimeSheetApprovalEntity entity/*[GEN-8]*/, long? userId = null)
        {
            try
            {
                return base.Update(entity, userId);
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public override string GetDisplayField(TimeSheetApprovalEntity entity)
        {
            return entity.ToString();
        }
        public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
        {
            List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
            return rs;
        }
    }
}
