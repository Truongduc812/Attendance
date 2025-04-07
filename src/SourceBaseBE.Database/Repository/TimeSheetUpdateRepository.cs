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
using SourceBaseBE.Database.Helpers;
using SourceBaseBE.Database.Models.ResponseModels;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models.ResponseModels;
using iSoft.Common.Utils;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Repository
{
    public class TimeSheetUpdateRepository : BaseCRUDRepository<TimeSheetUpdateEntity>
    {
        public TimeSheetUpdateRepository(CommonDBContext dbContext)
            : base(dbContext)
        {
        }
        public override string GetName()
        {
            return nameof(TimeSheetUpdateRepository);
        }
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override TimeSheetUpdateEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                TimeSheetUpdateEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<TimeSheetUpdateEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<TimeSheetUpdateEntity>();
                IQueryable<TimeSheetUpdateEntity> queryable;
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
                //result.TimeSheetUpdate2s = result.TimeSheetUpdate2s.Select(x => new TimeSheetUpdate2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public override async Task<TimeSheetUpdateEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                TimeSheetUpdateEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<TimeSheetUpdateEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<TimeSheetUpdateEntity>();
                IQueryable<TimeSheetUpdateEntity> queryable;
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

                //result.TimeSheetUpdate2s = result.TimeSheetUpdate2s.Select(x => new TimeSheetUpdate2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public override List<TimeSheetUpdateEntity> GetList(PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<TimeSheetUpdateEntity>? result = CachedFunc.GetRedisData<List<TimeSheetUpdateEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<TimeSheetUpdateEntity>();
                    if (pagingReq != null)
                    {
                        result = _context.Set<TimeSheetUpdateEntity>()
                                .AsNoTracking()
                                .AsQueryable()
                                /*[GEN-11]*/
                                .Where(entity => entity.DeletedFlag != true)
                                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                                .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                                .AsParallel()
                                .ToList();

                        //.Select(entity => new TimeSheetUpdateEntity
                        // {
                        //   Id = entity.Id,
                        //   TimeSheetUpdate2s = entity.TimeSheetUpdate2s.Select(x => new TimeSheetUpdate2Entity { Id = x.Id, Name = x.Name }).ToList()
                        // })

                        for (var i = 0; i < result.Count; i++)
                        {


                            /*[GEN-12]*/
                        }
                    }
                    else
                    {
                        result = _context.Set<TimeSheetUpdateEntity>()
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

        public List<TimeSheetUpdateEntity> GetList(List<long>? ids = null)
        {
            try
            {
                var query = _context.Set<TimeSheetUpdateEntity>()
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

        public TimeSheetUpdateEntity Upsert(TimeSheetUpdateEntity entity/*[GEN-8]*/, long? userId = null)
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
        public TimeSheetUpdateEntity Insert(TimeSheetUpdateEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<TimeSheetUpdateEntity>().Add(entity);
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
        public TimeSheetUpdateEntity Update(TimeSheetUpdateEntity entity/*[GEN-8]*/, long? userId = null)
        {
            try
            {
                base.Update(entity, userId);
                ClearCachedData();
                return entity;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public override string GetDisplayField(TimeSheetUpdateEntity entity)
        {
            return entity.ToString();
        }
        public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
        {
            List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
            return rs;
        }
        public async Task<TimeSheetUpdateEntity>? GetByIdWithRelationAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                TimeSheetUpdateEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}:{id}";
                if (!isDirect && !isTracking)
                {
                    result = CachedFunc.GetRedisData<TimeSheetUpdateEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    var dataSet = _context.Set<TimeSheetUpdateEntity>();
                    IQueryable<TimeSheetUpdateEntity> queryable;
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
                                .Include(x => x.TimeSheetEntity)
                                .Include(x => x.Employee)
                                .FirstOrDefaultAsync();

                    //result.TimeSheetUpdate2s = result.TimeSheetUpdate2s.Select(x => new TimeSheetUpdate2Entity() { Id = x.Id, Name = x.Name }).ToList();

                    CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
                    CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public async Task<List<TimeSheetUpdateEntity>>? GetByListIds(List<long> ids, bool isDirect = false, bool isTracking = false)
        {
            try
            {
                List<TimeSheetUpdateEntity>? result = null;
                string cacheKey = $"{cacheKeyDetail}:{ids}";
                if (!isDirect && !isTracking)
                {
                    result = CachedFunc.GetRedisData<List<TimeSheetUpdateEntity>>(cacheKey, null);
                }

                if (result == null)
                {
                    var dataSet = _context.Set<TimeSheetUpdateEntity>();
                    IQueryable<TimeSheetUpdateEntity> queryable;
                    if (!isTracking)
                    {
                        queryable = dataSet.AsNoTracking().AsQueryable();
                    }
                    else
                    {
                        queryable = dataSet.AsQueryable();
                    }
                    result = await queryable
                                .Where(entity => entity.DeletedFlag != true && ids.Contains(entity.Id))
                                .Include(x => x.TimeSheetEntity)
                                .Include(x => x.TimeSheetApprovalEntities)
                                .Include(x => x.Employee).ToListAsync();

                    //result.TimeSheetUpdate2s = result.TimeSheetUpdate2s.Select(x => new TimeSheetUpdate2Entity() { Id = x.Id, Name = x.Name }).ToList();

                    CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
                    CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public TimeSheetPendingRequestPagingResponseModel GetListPendingRequest(
         EnumApproveStatus enumApproveStatus,
         List<EmployeeEntity> employees,
         PagingFilterRequestModel pagingReq)
        {
            try
            {
                if (employees == null)
                {
                    throw new Exception("Invalid parameter: employee");
                }
                string cacheKey = $"{cacheKeyList}_GetListPendingRequest_{string.Join(",",employees)}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                var employeeIds = employees.Select(x => x.Id).ToList();
                TimeSheetPendingRequestPagingResponseModel rs = CachedFunc.GetRedisData<TimeSheetPendingRequestPagingResponseModel>(cacheKey, null);
                if (rs == null)
                {
                    rs = new TimeSheetPendingRequestPagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new PagingFilterRequestModel();
                    }
                    if (pagingReq.Page <= 1)
                    {
                        pagingReq.Page = 1;
                    }
                    if (pagingReq.PageSize <= 0)
                    {
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }
                    DateTime? startDate = DateTimeHelper.GetStartOfDate(pagingReq.DateFrom.GetValueOrDefault());
                    DateTime? endDate = DateTimeHelper.GetEndOfDate(pagingReq.DateTo.GetValueOrDefault());

                    var query = _context.Set<TimeSheetUpdateEntity>()
                            .AsNoTracking()
                             .Include(x => x.TimeSheetApprovalEntities)
                            .AsQueryable()
                           ;
                    //* where
                    query = query
                        .Where(p => p.DeletedFlag != true)
                        .Where(x => employeeIds.Contains(x.EmployeeId.GetValueOrDefault()));
                    query = query.Where(x => x.CreatedAt >= startDate && (x.CreatedAt <= endDate || x.UpdatedAt <= endDate));

                    if (enumApproveStatus == EnumApproveStatus.PENDING)
                    {
                        query = query.Where(x => x.TimeSheetApprovalEntities.Any(a => a.Status == EnumApproveStatus.PENDING));
                    }
                    //var list = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit()).ToList();
                    rs.ListData = TimeSheetUpdateDTO.SetData(query.ToList(), employees);
                    rs.TotalRecord = employees.Count;
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
        public async Task<PersonalTimeSheetPendingRequestPagingResponseModel> GetPersonalPendingRequest(
           long? employeeId,
           PagingFilterRequestModel pagingReq,
            Dictionary<string, object> paramFilter,
            SearchModel paramSearch,
            Dictionary<string, long> paramSort
            )
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetPersonalPendingRequest";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey += $"{employeeId.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                PersonalTimeSheetPendingRequestPagingResponseModel rs = CachedFunc.GetRedisData<PersonalTimeSheetPendingRequestPagingResponseModel>(cacheKey, null);
                if (rs == null || rs.ListData == null || rs.ListData.Count <= 0)
                {
                    rs = new PersonalTimeSheetPendingRequestPagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new PagingFilterRequestModel();
                    }
                    if (pagingReq.Page <= 1)
                    {
                        pagingReq.Page = 1;
                    }
                    if (pagingReq.PageSize <= 0)
                    {
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }
                    DateTime? startDate = DateTimeHelper.GetStartOfDate(pagingReq.DateFrom.GetValueOrDefault());
                    DateTime? endDate = DateTimeHelper.GetEndOfDate(pagingReq.DateTo.GetValueOrDefault());

                    var query = _context.Set<TimeSheetUpdateEntity>()
                            .AsNoTracking()
                            .AsQueryable();

                    //* where
                    query = query.Where(p => p.DeletedFlag != true)
                        .Include(x => x.UserEntity)
                        .ThenInclude(x => x.ItemEmployee)
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.Department)
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.JobTitle)
                        .Include(p => p.TimeSheetEntity)
                        .Include(x => x.TimeSheetApprovalEntities)
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                        .OrderByDescending(x => x.UpdatedAt)
                        .AsQueryable();

                    if (employeeId > 0)
                    {
                        query = query.Where(x => x.EmployeeId == employeeId);
                    }

                    query = DetailTimeSheetUpdateDTO.PrepareWhereQueryFilter(query, paramFilter);
                    query = DetailTimeSheetUpdateDTO.PrepareWhereQuerySearch(query, paramSearch);
                    query = DetailTimeSheetUpdateDTO.PrepareQuerySort(query, paramSort);
                    //var raw = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit());
                    rs.ListData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit()).Select(x => DetailTimeSheetUpdateDTO.SetData(x)).ToList();
                    rs.TotalRecord = query.AsParallel().LongCount();
                    var lst_working_aprrovals = query.SelectMany(x => x.TimeSheetApprovalEntities).Where(x => x.DeletedFlag != true);
                    var countModels = new List<CountResponseModel>
                      {
                         new CountResponseModel { Key = "approval", Number =  lst_working_aprrovals.LongCount(x=>x.Status== EnumApproveStatus.ACCEPT) },
                         new CountResponseModel { Key = "denied", Number  = lst_working_aprrovals.LongCount(x=>x.Status== Enums.EnumApproveStatus.REJECT)},
                         new CountResponseModel { Key = "pending", Number = lst_working_aprrovals.LongCount(x=>x.Status== EnumApproveStatus.PENDING) }
                      };
                    rs.Counts = countModels;
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

        public async Task<HistoricalTimeSheetPendingRequestPagingResponseModel> GetHistoricalPendingRequest(
           long? employeeId,
           PagingFilterRequestModel pagingReq,
            Dictionary<string, object> paramFilter,
            SearchModel paramSearch,
            Dictionary<string, long> paramSort
            )
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetHistoricalPendingRequest";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey += $"{employeeId.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                HistoricalTimeSheetPendingRequestPagingResponseModel rs = CachedFunc.GetRedisData<HistoricalTimeSheetPendingRequestPagingResponseModel>(cacheKey, null);
                if (rs == null || rs.ListData == null || rs.ListData.Count <= 0)
                {
                    rs = new HistoricalTimeSheetPendingRequestPagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new PagingFilterRequestModel();
                    }
                    if (pagingReq.Page <= 1)
                    {
                        pagingReq.Page = 1;
                    }
                    if (pagingReq.PageSize <= 0)
                    {
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }
                    DateTime? startDate = DateTimeHelper.GetStartOfDate(pagingReq.DateFrom.GetValueOrDefault());
                    DateTime? endDate = DateTimeHelper.GetEndOfDate(pagingReq.DateTo.GetValueOrDefault());

                    var query = _context.Set<TimeSheetUpdateEntity>()
                            .AsNoTracking()
                            .AsQueryable();

                    //* where
                    query = query.Where(p => p.DeletedFlag != true)
                        .Include(x => x.UserEntity)
                        .ThenInclude(x => x.ItemEmployee)
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.Department)
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.JobTitle)
                        .Include(p => p.TimeSheetEntity)
                        .Include(x => x.TimeSheetApprovalEntities)
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                        .OrderByDescending(x => x.UpdatedAt)
                        .AsQueryable();

                    if (employeeId > 0)
                    {
                        query = query.Where(x => x.EmployeeId == employeeId);
                    }

                    query = HistoricalTimeSheetUpdate.PrepareWhereQueryFilter(query, paramFilter);
                    query = HistoricalTimeSheetUpdate.PrepareWhereQuerySearch(query, paramSearch);
                    query = HistoricalTimeSheetUpdate.PrepareQuerySort(query, paramSort);
                    //var raw = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit());
                    rs.ListData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit()).Select(x => HistoricalTimeSheetUpdate.SetData(x)).ToList();
                    rs.TotalRecord = query.AsParallel().LongCount();
                    var lst_working_aprrovals = query.SelectMany(x => x.TimeSheetApprovalEntities).Where(x => x.DeletedFlag != true);
                    var countModels = new List<CountResponseModel>
                      {
                         new CountResponseModel { Key = "approval", Number =  lst_working_aprrovals.LongCount(x=>x.Status== EnumApproveStatus.ACCEPT) },
                         new CountResponseModel { Key = "denied", Number  = lst_working_aprrovals.LongCount(x=>x.Status== Enums.EnumApproveStatus.REJECT)},
                         new CountResponseModel { Key = "pending", Number = lst_working_aprrovals.LongCount(x=>x.Status== EnumApproveStatus.PENDING) }
                      };
                    rs.Counts = countModels;
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

        public async Task<DetailTimeSheetPagingResponseModel> GetIncommingTimeSheet(
           long? employeeId,
           List<long> acceptDepartmentIds,
            EnumApproveStatus status,
           PagingFilterRequestModel pagingReq,
            Dictionary<string, object> paramFilter,
            SearchModel paramSearch,
            Dictionary<string, long> paramSort
            )
        {
            if (employeeId <= 0 && employeeId != -1) throw new Exception($"Invalid Employee");
            try
            {
                string cacheKey = $"{cacheKeyList}_{employeeId}_{status}_GetIncommingTimeSheet";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                DetailTimeSheetPagingResponseModel rs = CachedFunc.GetRedisData<DetailTimeSheetPagingResponseModel>(cacheKey, null);
                if (rs == null || rs.ListData == null || rs.ListData.Count <= 0)
                {
                    rs = new DetailTimeSheetPagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new PagingFilterRequestModel();
                    }
                    if (pagingReq.Page <= 1)
                    {
                        pagingReq.Page = 1;
                    }
                    if (pagingReq.PageSize <= 0)
                    {
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }
                    DateTime? startDate = pagingReq.DateFrom == null ? DateTimeHelper.GetStartOfDate(DateTime.Now).AddHours(6) : DateTimeHelper.GetStartOfDate(pagingReq.DateFrom.GetValueOrDefault());
                    DateTime? endDate = pagingReq.DateTo == null ? DateTimeHelper.GetEndOfDate(DateTime.Now).AddDays(1).AddHours(6) : DateTimeHelper.GetEndOfDate(pagingReq.DateTo.GetValueOrDefault());




                    var query = _context.Set<TimeSheetUpdateEntity>()
                        .AsNoTracking()
                        .AsQueryable()
                   .Include(x => x.TimeSheetEntity)
                   .Include(x => x.Employee)
                   .Include(x => x.UserEntity)
                   .Include(x => x.TimeSheetApprovalEntities)
                   .Where(x => x.DeletedFlag != true)
                   .Where(x => x.Employee != null && acceptDepartmentIds.Contains(x.Employee.DepartmentId.GetValueOrDefault()))
                   .Where(x => x.CreatedAt >= pagingReq.DateFrom && x.CreatedAt <= pagingReq.DateTo && x.TimeSheetEntityId == null)
                   .Where(x => x.TimeSheetApprovalEntities.FirstOrDefault().Status == status);

                    if (employeeId != -1)
                    {
                        query = query.Where(x => x.EmployeeId == employeeId);
                    }

                    query = TimesheetEditListResponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = TimesheetEditListResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = TimesheetEditListResponseModel.PrepareQuerySort(query, paramSort);

                    rs.ListIncomingData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit()).Select(x => new TimesheetEditListResponseModel().SetData(x)).ToList();
                    rs.TotalRecord = await query.LongCountAsync();

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

        public void Detached(TimeSheetUpdateEntity entity)
        {
            if (entity != null)
            {
                this._context.Entry(entity).State = EntityState.Detached;
                foreach (var approve in entity.TimeSheetApprovalEntities)
                {
                    this._context.Entry(approve).State = EntityState.Detached;
                }
                if (entity.TimeSheetEntity != null)
                {
                    this._context.Entry(entity.TimeSheetEntity).State = EntityState.Detached;
                }
            }
        }
    }
}
