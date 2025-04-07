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
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;
using iSoft.Common.Enums;
using System.Linq.Dynamic.Core;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Utils;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Repository
{
    public class HolidayScheduleRepository : BaseCRUDRepository<HolidayScheduleEntity>
    {
        public override string TableName
        {
            get { return "HolidaySchedule"; }
        }
        public HolidayScheduleRepository(CommonDBContext dbContext)
                : base(dbContext)
        {
        }
        public override string GetName()
        {
            return nameof(HolidayScheduleRepository);
        }
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override HolidayScheduleEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                HolidayScheduleEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<HolidayScheduleEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<HolidayScheduleEntity>();
                IQueryable<HolidayScheduleEntity> queryable;
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
                //result.HolidaySchedule2s = result.HolidaySchedule2s.Select(x => new HolidaySchedule2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public override async Task<HolidayScheduleEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                HolidayScheduleEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<HolidayScheduleEntity>(cacheKey, null);
                //}

                //if (result == null)
                //{
                result = await (isTracking ?
                _context.Set<HolidayScheduleEntity>()
                          //.AsNoTracking()
                          .AsQueryable()
                          /*[GEN-7]*/
                          .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                          .FirstOrDefaultAsync() :
                          _context.Set<HolidayScheduleEntity>()
                          .AsNoTracking()
                          .AsQueryable()
                          /*[GEN-7]*/
                          .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                          .FirstOrDefaultAsync());
                //result.HolidaySchedule2s = result.HolidaySchedule2s.Select(x => new HolidaySchedule2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public override List<HolidayScheduleEntity> GetList(PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetList";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<HolidayScheduleEntity>? result = CachedFunc.GetRedisData<List<HolidayScheduleEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<HolidayScheduleEntity>();
                    if (pagingReq != null)
                    {
                        result = _context.Set<HolidayScheduleEntity>()
                                .AsNoTracking()
                                .AsQueryable()
                                /*[GEN-11]*/
                                .Where(entity => entity.DeletedFlag != true)
                                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                                .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                                .AsParallel()
                                .ToList();

                        //.Select(entity => new HolidayScheduleEntity
                        // {
                        //   Id = entity.Id,
                        //   HolidaySchedule2s = entity.HolidaySchedule2s.Select(x => new HolidaySchedule2Entity { Id = x.Id, Name = x.Name }).ToList()
                        // })

                        for (var i = 0; i < result.Count; i++)
                        {


                            /*[GEN-12]*/
                        }
                    }
                    else
                    {
                        result = _context.Set<HolidayScheduleEntity>()
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

        //public List<HolidayScheduleEntity> GetList()
        //{
        //	try
        //	{
        //		List<HolidayScheduleEntity> result = _context.Set<HolidayScheduleEntity>()
        //		  .AsNoTracking()
        //		  .AsQueryable()
        //		  /*[GEN-13]*/
        //		  .Where(entity => entity.DeletedFlag != true)
        //		  .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
        //		  .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
        //		  .AsParallel()
        //		  .ToList();
        //		return result;
        //	}
        //	catch (Exception ex)
        //	{
        //		throw new DBException(ex);
        //	}
        //}
        public HolidayScheduleEntity Upsert(HolidayScheduleEntity entity/*[GEN-8]*/, long? userId = null)
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
        public HolidayScheduleEntity Insert(HolidayScheduleEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<HolidayScheduleEntity>().Add(entity);
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
        public HolidayScheduleEntity Update(HolidayScheduleEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<HolidayScheduleEntity>().Update(entity);
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

        public List<HolidayScheduleEntity> GetByDate(PagingHolidayReportModel request)
        {
            return _context
                .Set<HolidayScheduleEntity>()
                .AsNoTracking()
        .Where(p => p.DeletedFlag != true)
        .Where(p => (p.StartDate >= request.DateFrom && p.StartDate <= request.DateTo) || (p.EndDate <= request.DateTo && p.EndDate >= request.DateFrom))
        .AsQueryable()
        .ToList();

        }
        public List<HolidayScheduleEntity> GetByDate(DateTime date)
        {
            return _context
                .Set<HolidayScheduleEntity>()
                .AsNoTracking()
        .Where(p => p.DeletedFlag != true)
        .Where(p => (p.StartDate >= date && p.StartDate <= date) || (p.EndDate <= date && p.EndDate >= date))
        .AsQueryable()
        .ToList();

        }
        public List<HolidayScheduleEntity> GetListByDate(DateTime dateMin, DateTime dateMax)
        {
            return _context
                .Set<HolidayScheduleEntity>()
                .AsNoTracking()
        .Where(p => p.DeletedFlag != true)
        .Where(p => (p.StartDate >= dateMin && p.StartDate <= dateMax) || (p.EndDate <= dateMax && p.EndDate >= dateMin))
        .AsQueryable()
        .ToList();

        }
        public HolidayScheduleEntity GetByDateAndSymbel(DateTime startDay, DateTime endTime, EnumHolidayCode enumHolidayCode)
        {
            return _context.HolidaySchedules
                //.AsNoTracking()
                .Where(p => p.DeletedFlag != true)
                .FirstOrDefault(p => p.StartDate.Year == startDay.Year && p.EndDate.Year == endTime.Year && p.HolidayType == enumHolidayCode);
        }

        public HolidayScheduleEntity GetByDateAndSymbelAndName(DateTime startDay, DateTime endTime, EnumHolidayCode? enumHolidayCode, string name)
        {
            return _context.HolidaySchedules
                //.AsNoTracking()
                .Where(p => p.DeletedFlag != true)
                .FirstOrDefault(p => p.StartDate == startDay && p.EndDate == endTime && p.HolidayType == enumHolidayCode&& p.Name == name);
        }
        public override string GetDisplayField(HolidayScheduleEntity entity)
        {
            return entity.Name.ToString();
        }
        public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
        {
            List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
            return rs;
        }

        //public IEnumerable<HolidayScheduleEntity> UpSertMulti(IEnumerable<HolidayScheduleEntity> entities, long? userId = null)
        //{
        //  try
        //  {
        //    if (entities != null && entities.Count() > 0)
        //    {
        //      foreach (var entity in entities)
        //      {
        //        if (entity != null && entity.Id <= 0)
        //        {
        //          Insert(entity/*[GEN-4]*/, userId);
        //        }
        //        else
        //        {
        //          if (userId != null)
        //          {
        //            entity.UpdatedBy = userId;
        //          }
        //          entity.UpdatedAt = DateTime.Now;
        //          _context.Set<HolidayScheduleEntity>().Update(entity);
        //        }
        //      }
        //      var result = _context.SaveChanges();
        //      ClearCachedData();
        //    }

        //    return entities;
        //  }
        //  catch (Exception ex)
        //  {
        //    throw new DBException(ex);
        //  }
        //}

        public HolidaySchedulePagingResponseModel GetListHolidaySchedule(
         PagingHolidayRequestModel pagingReq,
         SearchModel paramSearch,
         Dictionary<string, long> paramSort)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_Holiday_Schedule_Setting_{pagingReq.DateFrom}_{pagingReq.DateTo}_{pagingReq.SearchStr}_{pagingReq.SortStr}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                HolidaySchedulePagingResponseModel rs = CachedFunc.GetRedisData<HolidaySchedulePagingResponseModel>(cacheKey, null);
                if (rs == null)
                {
                    rs = new HolidaySchedulePagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new PagingHolidayRequestModel();
                        pagingReq.Page = 0;
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }

                    var query = _context.Set<HolidayScheduleEntity>()
                        .AsNoTracking()
                        .AsQueryable();

                    //* where
                    query = query.Where(p => p.DeletedFlag != true);
                    query = query.Where(p => (p.StartDate >= pagingReq.DateFrom) && (p.EndDate <= pagingReq.DateTo))
                                                       .AsQueryable();



                    query = HolidayScheduleListResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = HolidayScheduleListResponseModel.PrepareQuerySort(query, paramSort);

                    if (query == null) throw new Exception("Empty Holiday Return");
                    rs.ListData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                                                         .Select(x => new HolidayScheduleListResponseModel().SetData(x))
                                                         .ToList();
                    rs.TotalRecord = query.AsParallel().LongCount();
                    CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
                    CachedFunc.SetRedisData(cacheKey, rs, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
                }
                return rs;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        public async Task<bool> RemoveIfNotExistInList(List<HolidayScheduleEntity> employeeEntities)
        {
            try
            {
                var listEmp = _context.Set<HolidayScheduleEntity>()
                  .Where(p => p.DeletedFlag != true && !employeeEntities.Contains(p))
                  .ToList();
                _context.RemoveRange(listEmp);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public List<HolidayScheduleEntity> GetCurrentYearHolidays()
        {
            try
            {
                var listEmp = _context.Set<HolidayScheduleEntity>()
                  .Where(p => p.DeletedFlag != true && (p.StartDate.Year == DateTime.Now.Year || p.EndDate.Year == DateTime.Now.Year))
                  .OrderByDescending(p => p.StartDate.Year)
                    .ToList();
                return listEmp;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
    }
}
