using iSoft.Common;
using iSoft.Common.Cached;
using iSoft.Common.Enums;
using iSoft.Common.Exceptions;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Models.ResponseModels;
using iSoft.Common.Utils;
using iSoft.Redis.Services;
using Microsoft.EntityFrameworkCore;
using NPOI.Util;
using SourceBaseBE.Database.DBContexts;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.RequestModels.Report;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Models.SpecialModels;
using static StackExchange.Redis.Role;

namespace SourceBaseBE.Database.Repository
{
    public class WorkingDayRepository : BaseCRUDRepository<WorkingDayEntity>
    {
        public WorkingDayRepository(CommonDBContext dbContext)
          : base(dbContext)
        {
        }
        public override string GetName()
        {
            return nameof(WorkingDayRepository);
        }
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override WorkingDayEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                WorkingDayEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<WorkingDayEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<WorkingDayEntity>();
                IQueryable<WorkingDayEntity> queryable;
                if (!isTracking)
                {
                    queryable = dataSet.AsNoTracking().AsQueryable();
                }
                else
                {
                    queryable = dataSet.AsQueryable();
                }
                result = queryable
                  .Include(x => x.Employee)
                            .AsQueryable()
                            /*[GEN-7]*/
                            .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                            .FirstOrDefault();
                //result.WorkingDay2s = result.WorkingDay2s.Select(x => new WorkingDay2Entity() { Id = x.Id, Name = x.Name }).ToList();

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

        public WorkingDayEntity? GetById(long? id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                WorkingDayEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<WorkingDayEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<WorkingDayEntity>();
                IQueryable<WorkingDayEntity> queryable;
                if (!isTracking)
                {
                    queryable = dataSet.AsNoTracking().AsQueryable();
                }
                else
                {
                    queryable = dataSet.AsQueryable();
                }
                result = queryable
                  .Include(x => x.Employee)
                            /*[GEN-7]*/
                            .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                            .FirstOrDefault();
                //result.WorkingDay2s = result.WorkingDay2s.Select(x => new WorkingDay2Entity() { Id = x.Id, Name = x.Name }).ToList();

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

        public WorkingDayEntity? GetById(long? id)
        {
            try
            {
                WorkingDayEntity? result = null;

                var dataSet = _context.Set<WorkingDayEntity>();
                IQueryable<WorkingDayEntity> queryable;

                queryable = dataSet.AsNoTracking().AsQueryable();

                result = queryable
                            /*[GEN-7]*/
                            .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                            .FirstOrDefault();

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
        public override async Task<WorkingDayEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                WorkingDayEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<WorkingDayEntity>(cacheKey, null);
                //}

                //if (result == null)
                //{
                result = await (isTracking ?
                _context.Set<WorkingDayEntity>()
                  //.AsNoTracking()
                  .AsQueryable()
                  /*[GEN-7]*/
                  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                  .FirstOrDefaultAsync() :
                  _context.Set<WorkingDayEntity>()
                  .AsNoTracking()
                  .AsQueryable()
                  /*[GEN-7]*/
                  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                  .FirstOrDefaultAsync());
                //result.WorkingDay2s = result.WorkingDay2s.Select(x => new WorkingDay2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public override List<WorkingDayEntity> GetList(PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<WorkingDayEntity>? result = CachedFunc.GetRedisData<List<WorkingDayEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<WorkingDayEntity>();
                    if (pagingReq != null)
                    {
                        result = _context.Set<WorkingDayEntity>()
                          .AsNoTracking()
                          .AsQueryable()
                          /*[GEN-11]*/
                          .Where(entity => entity.DeletedFlag != true)
                          .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                          .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                          .AsParallel()
                          .ToList();

                        //.Select(entity => new WorkingDayEntity
                        // {
                        //   Id = entity.Id,
                        //   WorkingDay2s = entity.WorkingDay2s.Select(x => new WorkingDay2Entity { Id = x.Id, Name = x.Name }).ToList()
                        // })

                        for (var i = 0; i < result.Count; i++)
                        {


                            /*[GEN-12]*/
                        }
                    }
                    else
                    {
                        result = _context.Set<WorkingDayEntity>()
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

        public List<WorkingDayEntity> GetListWorkingDayUpdateState(long intervalUpdate)
        {
            try
            {
                var currentTime = DateTime.Now;
                List<WorkingDayEntity> result = new List<WorkingDayEntity>();

                result = _context.Set<WorkingDayEntity>()
                    .AsNoTracking()
                    .AsQueryable()
                    .Include(entity => entity.Employee)
                    /*[GEN-11]*/
                    .Where(entity => entity.DeletedFlag != true && entity.Employee.DeletedFlag != true)
                    .Where(entity => entity.InOutState != EnumInOutTypeStatus.Outside)
                    .Where(entity => (entity.Time_In.HasValue && entity.Time_In <= currentTime.AddSeconds(-intervalUpdate)) || (entity.Time_In == null && entity.CreatedAt <= currentTime.AddDays(-1)))
                    .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                    .AsParallel()
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public WorkingDayEntity Upsert(WorkingDayEntity entity/*[GEN-8]*/, long? userId = null)
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
        public WorkingDayEntity Insert(WorkingDayEntity entity/*[GEN-8]*/, long? userId = null)
        {
            try
            {
                if (entity.Id > 0)
                {
                    throw new DBException($"Insert, Unexpected Id in entity, Id={entity.Id}");
                }
                else
                {
                    var existed = this.CheckIfExist(entity).Result;
                    if (userId != null)
                    {
                        entity.CreatedBy = userId;
                    }
                    if (existed == null)
                    {
                        entity.CreatedAt = DateTime.Now;
                        entity.UpdatedBy = entity.CreatedBy;
                        entity.UpdatedAt = entity.CreatedAt;
                        entity.DeletedFlag = false;
                    }
                    else
                    {
                        entity.Id = existed.Id;
                        return this.Update(entity, userId);
                    }
                    /*[GEN-10]*/
                    _context.Set<WorkingDayEntity>().Add(entity);
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
        public WorkingDayEntity Update(WorkingDayEntity entity/*[GEN-8]*/, long? userId = null)
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
                    //_context.Entry<WorkingDayEntity>(entity).State = EntityState.Detached;
                    /*[GEN-9]*/
                    base.Update(entity);
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
        public override string GetDisplayField(WorkingDayEntity entity)
        {
            return entity.Notes.ToString();
        }

        public override IEnumerable<WorkingDayEntity> InsertMulti(IEnumerable<WorkingDayEntity> entities, long? userId = null)
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
                        _context.Set<WorkingDayEntity>().AddRange(entity);
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

        // solution2
        public WorkingDayPagingResponseModel GetListWorkingDayv2(
         PagingFilterRequestModel pagingReq,
         Dictionary<string, object> paramFilter,
        SearchModel paramSearch,
         Dictionary<string, long> paramSort)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_Attendance_GetListWorkingDayv2";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                WorkingDayPagingResponseModel rs = CachedFunc.GetRedisData<WorkingDayPagingResponseModel>(cacheKey, null);
                if (rs == null)
                {
                    rs = new WorkingDayPagingResponseModel();
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
                    DateTime? startDate = DateTimeHelper.GetStartOfDate(pagingReq.DateFrom.GetValueOrDefault()).AddDays(-1);
                    DateTime? endDate = DateTimeHelper.GetEndOfDate(pagingReq.DateTo.GetValueOrDefault());

                    var query = _context.Set<WorkingDayEntity>()
                      .AsNoTracking()
                      .AsQueryable();
                    //* where
                    query = query.AsNoTracking()
                         .Include(p => p.Employee)
                         .ThenInclude(jt => jt.Department)
                         .Include(p => p.Employee)
                         .ThenInclude(d => d.JobTitle)
                         .OrderBy(x => x.UpdatedAt)
                         .AsQueryable();
                    query = query.Where(entity => entity.DeletedFlag != true && entity.Employee.DeletedFlag != true);
                    query = query.Where(s => s.WorkingDate.Value >= startDate && s.WorkingDate.Value <= endDate);

                    //check miss using faceId
                    // Lấy danh sách vào ngày trước ngày endDate
                    var previousDay = endDate.Value.AddDays(-2);
                    var currentDay = endDate.Value.AddDays(-1);

                    Dictionary<string, WorkingDayEntity> previousWd = new Dictionary<string, WorkingDayEntity>();
                    Dictionary<string, WorkingDayEntity> currentWd = new Dictionary<string, WorkingDayEntity>();

                    var previousDayEntries = query.Where(s => s.WorkingDate.Value.Date == previousDay.Date).ToList();
                    var CurrentDayEntries = query.Where(s => s.WorkingDate.Value.Date == currentDay.Date).ToList();

                    foreach (var keyValue in previousDayEntries)
                    {
                        previousWd[keyValue.EmployeeName] = keyValue;
                    }
                    foreach (var keyValue in CurrentDayEntries)
                    {
                        currentWd[keyValue.EmployeeName] = keyValue;
                    }

                    // Kiểm tra các điều kiện
                    var resultMissCheckOut = new List<WorkingDayEntity>();
                    foreach (var kvp in currentWd)
                    {
                        var employeeName = kvp.Key;
                        var currentEntry = kvp.Value;

                        if (currentEntry.InOutState == EnumInOutTypeStatus.Inside &&
                            previousWd.ContainsKey(employeeName) &&
                            previousWd[employeeName].InOutState == EnumInOutTypeStatus.Inside)
                        {
                            resultMissCheckOut.Add(previousWd[employeeName]);
                        }
                    }

                    if (resultMissCheckOut != null && resultMissCheckOut.Count > 0)
                    {
                        query = query.Where(q => !resultMissCheckOut.Contains(q));
                    }

                    var totalByDate = query;
                    query = DashboardResponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = DashboardResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = DashboardResponseModel.PrepareQuerySort(query, paramSort);

                    var skip = pagingReq.GetSkip();
                    rs.ListData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit()).ToList().Select(x => new DashboardResponseModel().SetData(x)).ToList();
                    rs.TotalRecord = query.LongCount();
                    var countModels = new List<CountResponseModel>
      {
           new CountResponseModel { Key = "inside", Number  = totalByDate.LongCount(x=>x.InOutState== EnumInOutTypeStatus.Inside) },
           new CountResponseModel { Key = "outside", Number =totalByDate.LongCount(x=>x.InOutState== EnumInOutTypeStatus.Outside) },
           new CountResponseModel { Key = "unknown", Number = totalByDate .LongCount(x=>x.InOutState== EnumInOutTypeStatus.Unknown) },

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

        public List<EmployeeListGroupByDepartmentResponseModel> GetListAttendanceGroupByDepartment(
         AttendanceGroupByDepartmentRequest requestModel,
         List<EmployeeEntity> listEmployee,
         Dictionary<string, object> paramFilter,
         SearchModel paramSearch,
         Dictionary<string, long> paramSort)
        {
            try
            {
                List<EmployeeListGroupByDepartmentResponseModel>? result = new List<EmployeeListGroupByDepartmentResponseModel>(); ;
                long totalRecord = 0;
                string cacheKey = $"{cacheKeyList}_Attendance_GetListAttendanceGroupByDepartment";
                if (requestModel != null)
                {
                    cacheKey = $"{cacheKey}:{requestModel.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                result = CachedFunc.GetRedisData<List<EmployeeListGroupByDepartmentResponseModel>>(cacheKey, null);
                if (result == null)
                {
                    if (requestModel.Page <= 1)
                    {
                        requestModel.Page = 1;
                    }
                    if (requestModel.PageSize <= 0)
                    {
                        requestModel.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }

                    DateTime? startDate = DateTimeHelper.GetStartOfDate(requestModel.DateFrom.GetValueOrDefault());
                    DateTime? endDate = DateTimeHelper.GetEndOfDate(requestModel.DateTo.GetValueOrDefault());

                    var query = _context.Set<WorkingDayEntity>()
                      .AsNoTracking()
                      .AsQueryable();
                    //* where
                    query = query.AsNoTracking()
                         .Include(p => p.Employee)
                         .ThenInclude(jt => jt.Department)
                         .OrderBy(x => x.UpdatedAt)
                         .AsQueryable();
                    query = query.Where(entity => entity.DeletedFlag != true && entity.Employee.DeletedFlag != true);
                    query = query.Where(s => s.WorkingDate.Value >= startDate && s.WorkingDate.Value <= endDate);

                    //get list by departmentId
                    if (requestModel.DepartmentId != null && requestModel.DepartmentId.Any())
                    {
                        query = query.Where(x => x.Employee.DepartmentId != null)
                                .Where(x => requestModel.DepartmentId.Contains(x.Employee.DepartmentId.Value));
                    }

                    //check miss using faceId
                    // Lấy danh sách vào ngày trước ngày endDate
                    var previousDay = endDate.Value.AddDays(-2);
                    var currentDay = endDate.Value.AddDays(-1);

                    Dictionary<string, WorkingDayEntity> previousWd = new Dictionary<string, WorkingDayEntity>();
                    Dictionary<string, WorkingDayEntity> currentWd = new Dictionary<string, WorkingDayEntity>();

                    var previousDayEntries = query.Where(s => s.WorkingDate.Value.Date == previousDay.Date).ToList();
                    var CurrentDayEntries = query.Where(s => s.WorkingDate.Value.Date == currentDay.Date).ToList();

                    foreach (var keyValue in previousDayEntries)
                    {
                        previousWd[keyValue.EmployeeName] = keyValue;
                    }
                    foreach (var keyValue in CurrentDayEntries)
                    {
                        currentWd[keyValue.EmployeeName] = keyValue;
                    }

                    // Kiểm tra các điều kiện
                    var resultMissCheckOut = new List<WorkingDayEntity>();
                    foreach (var kvp in currentWd)
                    {
                        var employeeName = kvp.Key;
                        var currentEntry = kvp.Value;

                        if (currentEntry.InOutState == EnumInOutTypeStatus.Inside &&
                            previousWd.ContainsKey(employeeName) &&
                            previousWd[employeeName].InOutState == EnumInOutTypeStatus.Inside)
                        {
                            resultMissCheckOut.Add(previousWd[employeeName]);
                        }
                    }

                    if (resultMissCheckOut != null && resultMissCheckOut.Count > 0)
                    {
                        query = query.Where(q => !resultMissCheckOut.Contains(q));
                    }

                    var totalByDate = query;
                    query = EmployeeDepartmentResponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = EmployeeDepartmentResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = EmployeeDepartmentResponseModel.PrepareQuerySort(query, paramSort);

                    var dicGroupedDataSpecific = new Dictionary<string, List<EmployeeDepartmentResponseModel>>();
                    var dicGroupedDataUnknown = new Dictionary<string, List<EmployeeDepartmentResponseModel>>();

                    foreach (var entity in query)
                    {
                        if (entity.Employee?.Department?.DeletedFlag == true)
                        {
                            continue;
                        }

                        var departmentName = entity.Employee?.Department?.Name ?? "Unknown";

                        var responseModel = new EmployeeDepartmentResponseModel
                        {
                            Name = entity.Employee?.Name,
                            Department = departmentName,
                            DepartmentId = entity.Employee?.Department?.Id,
                            Status = entity.InOutState.ToString(),
                            Avatar = entity.Employee?.Avatar != null
                                     ? entity.Employee.Avatar.Replace(ConstDatabase.ConstImageSaveAvatarPath, ConstDatabase.ConstImageGetAvatarPath)
                                     : string.Empty
                        };

                        if (departmentName == "Unknown")
                        {
                            AddToDictionary(dicGroupedDataUnknown, departmentName, responseModel);
                        }
                        else
                        {
                            AddToDictionary(dicGroupedDataSpecific, departmentName, responseModel);
                        }
                    }

                    // Merge groupedDataUnknown into groupedDataSpecific
                    foreach (var keyVal in dicGroupedDataUnknown)
                    {
                        AddRangeToDictionary(dicGroupedDataSpecific, keyVal.Key, keyVal.Value);
                    }


                    // Create the result list
                    var departmentGroupList = dicGroupedDataSpecific.Select(g => new EmployeeListGroupByDepartmentResponseModel
                    {
                        Department = g.Key,
                        DepartmentId = g.Value.Where(x => x.DepartmentId != null)?.FirstOrDefault()?.DepartmentId,
                    });
                    departmentGroupList = departmentGroupList.ToList();

                    if (listEmployee == null || listEmployee.Count <= 0)
                    {
                        throw new Exception("GetListAttendanceGroupByDepartment, ListEmployee is null");
                    }
                    foreach (var department in departmentGroupList)
                    {
                        var employeeDataQuery = query.Where(x => x.Employee.Department == null || x.Employee.Department.Name == "Unknown"
                                                    ? x.Employee.Department == null || string.IsNullOrEmpty(x.Employee.Department.Name) || department.Department == "Unknow"
                                                    : x.Employee.Department.Name == department.Department);

                        var totalEmployeeOfDepartment = listEmployee.Where(x => x.Department != null)
                                                                    .Where(x => x.Department.Name == department.Department).LongCount();

                        var totalEmployeeDepartment = employeeDataQuery;
                        employeeDataQuery = EmployeeDepartmentResponseModel.PrepareWhereQueryFilterStateStatus(employeeDataQuery, paramFilter);
                        department.TotalRecord = employeeDataQuery.LongCount();

                        var pagedEmployeeData = FetchPagedEmployeeData(employeeDataQuery, requestModel);

                        // Check if the current page is empty;
                        // fetch nearest page with data
                        if (!pagedEmployeeData.Any())
                        {
                            pagedEmployeeData = FetchNearestPageWithData(employeeDataQuery, requestModel);
                        }

                        department.EmployeeList = pagedEmployeeData;
                        var inside = totalEmployeeDepartment.LongCount(x => x.InOutState == EnumInOutTypeStatus.Inside);
                        var countModels = new List<CountResponseModel>
                        {
                            new CountResponseModel { Key = "inside", Number  = inside },
                            new CountResponseModel { Key = "outside",Number = totalEmployeeOfDepartment == 0 ? 0: totalEmployeeOfDepartment - inside },
                            new CountResponseModel { Key = "unknown", Number = totalEmployeeDepartment.LongCount(x=>x.InOutState== EnumInOutTypeStatus.Unknown) },

                        };
                        department.Counts = countModels;
                    }

                    result = departmentGroupList.ToList();
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

        public void AddToDictionary(Dictionary<string, List<EmployeeDepartmentResponseModel>> dictionary, string key, EmployeeDepartmentResponseModel value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = new List<EmployeeDepartmentResponseModel>();
            }
            dictionary[key].Add(value);
        }

        public void AddRangeToDictionary(Dictionary<string, List<EmployeeDepartmentResponseModel>> dictionary, string key, List<EmployeeDepartmentResponseModel> values)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = new List<EmployeeDepartmentResponseModel>();
            }
            dictionary[key].AddRange(values);
        }
        private List<EmployeeDepartmentResponseModel> FetchPagedEmployeeData(
                                                                    IQueryable<WorkingDayEntity> query,
                                                                    AttendanceGroupByDepartmentRequest requestModel)
        {
            return query
                .Skip(requestModel.GetSkip())
                .Take(requestModel.GetLimit())
                .Select(x => new EmployeeDepartmentResponseModel
                {
                    Name = x.Employee.Name,
                    Department = x.Employee.Department.Name,
                    Status = x.InOutState.ToString(),
                    Avatar = x.Employee.Avatar != null ? x.Employee.Avatar.Replace(ConstDatabase.ConstImageSaveAvatarPath, ConstDatabase.ConstImageGetAvatarPath)
                                                       : string.Empty
                }).ToList();
        }

        private List<EmployeeDepartmentResponseModel> FetchNearestPageWithData(
                                                                IQueryable<WorkingDayEntity> query,
                                                                AttendanceGroupByDepartmentRequest requestModel)
        {
            var currentPage = requestModel.Page;
            while (currentPage > 1)
            {
                currentPage--;
                var pagedData = query
                    .Skip((currentPage - 1) * requestModel.PageSize)
                    .Take(requestModel.PageSize)
                    .Select(x => new EmployeeDepartmentResponseModel
                    {
                        Name = x.Employee.Name,
                        Department = x.Employee.Department.Name,
                        Status = x.InOutState.ToString(),
                        Avatar = x.Employee.Avatar != null ? x.Employee.Avatar.Replace(ConstDatabase.ConstImageSaveAvatarPath, ConstDatabase.ConstImageGetAvatarPath)
                                                       : string.Empty
                    }).ToList();

                if (pagedData.Any())
                {
                    return pagedData;
                }
            }

            return new List<EmployeeDepartmentResponseModel>();
        }

        public DetailAttendancePagingResponseModel GetListWorkingDayByEmployeeId2(
                                                      EmployeeAttendanceDetailRequest pagingReq,
                                                      Dictionary<string, object> paramFilter,
                                                      SearchModel paramSearch,
                                                      Dictionary<string, long> paramSort)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListWorkingDayByEmployeeId2";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}" +
                                $"_{EncodeUtil.MD5(paramFilter.ToJson())}" +
                                $"_{EncodeUtil.MD5(paramSearch.ToJson())}" +
                                $"_{EncodeUtil.MD5(paramSort.ToJson())}";
                    cacheKey = EncodeUtil.MD5(cacheKey);
                }
                DetailAttendancePagingResponseModel rs = CachedFunc.GetRedisData<DetailAttendancePagingResponseModel>(cacheKey, null);
                if (rs == null || rs.rawDatas == null)
                {
                    rs = new DetailAttendancePagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new EmployeeAttendanceDetailRequest();
                        pagingReq.Page = 0;
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }
                    if (pagingReq.PageSize <= 0)
                    {
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }
                    DateTime? startDate = DateTimeHelper.GetStartOfDate(pagingReq.DateFrom.GetValueOrDefault());
                    DateTime? endDate = DateTimeHelper.GetEndOfDate(pagingReq.DateTo.GetValueOrDefault());

                    var queryCurrentWd = _context.Set<WorkingDayEntity>()
                      .AsNoTracking()
                      .AsQueryable();
                    //* where
                    queryCurrentWd = queryCurrentWd.Where(p => p.DeletedFlag != true && p.EmployeeEntityId == pagingReq.EmployeeId);
                    queryCurrentWd = queryCurrentWd.AsNoTracking()
                         .Include(p => p.Employee)
                         .ThenInclude(jt => jt.Department)
                         .Include(p => p.Employee)
                         .ThenInclude(d => d.JobTitle)
                         .Include(x => x.WorkingType)
                          .Include(x => x.WorkingDayUpdates)
                         .ThenInclude(x => x.WorkingDayApprovals)
                        .Include(x => x.TimeSheets)
                         .OrderBy(x => x.WorkingDate)
                         .AsQueryable();

                    queryCurrentWd = DetailAttendanceResponse.PrepareDetailReportWhereQueryFilter(queryCurrentWd, paramFilter);
                    queryCurrentWd = DetailAttendanceResponse.PrepareDetailReportWhereQuerySearch(queryCurrentWd, paramSearch);
                    queryCurrentWd = DetailAttendanceResponse.PrepareDetailReportQuerySort(queryCurrentWd, paramSort);
                    queryCurrentWd = queryCurrentWd.Where(s => (s.WorkingDate.Value >= startDate && s.WorkingDate.Value <= endDate));
                    rs.TotalRecord = queryCurrentWd.LongCount();
                    rs.rawDatas = queryCurrentWd.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit()).ToList();
                    //var calculate = CalculateMonthWorkingType(rs.rawDatas, listWorkingType);
                    //rs.ListData = rs.rawDatas
                    //  //.OrderBy(x => x.WorkingDate)
                    //  .Select(x => new DetailAttendanceResponse().SetData(x))
                    //  .ToList();

                    var list = rs.rawDatas.ToList();
                    foreach (var item in list)
                    {
                        if (item.TimeSheets != null && item.TimeSheets.Count > 0)
                        {
                            item.TimeSheets = item.TimeSheets.Where(x => x.DeletedFlag != true).ToList();
                        }
                    }

                    rs.ListData = list.Select(x => new DetailAttendanceResponse().SetData(x)).ToList();

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
        public List<WorkingDayEntity> GetListWorkingDayByEmployeeId3(
                                                      long employeeId,
                                                      DateTime getDate,
                                                      bool isTracking = false)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListWorkingDayByEmployeeId3";
                cacheKey = $"{cacheKey}:{getDate}_{employeeId}";
                cacheKey = EncodeUtil.MD5(cacheKey);
                List<WorkingDayEntity> rs = CachedFunc.GetRedisData<List<WorkingDayEntity>>(cacheKey, null);
                if (rs == null || isTracking)
                {
                    rs = new List<WorkingDayEntity>();

                    DateTime? startDate = DateTimeHelper.GetStartOfDate(getDate);
                    DateTime? endDate = DateTimeHelper.GetEndOfDate(getDate);

                    IQueryable<WorkingDayEntity> queryCurrentWd;
                    if (isTracking)
                    {
                        queryCurrentWd = _context.Set<WorkingDayEntity>()
                          .AsQueryable();
                    }
                    else
                    {
                        queryCurrentWd = _context.Set<WorkingDayEntity>()
                          .AsNoTracking()
                          .AsQueryable();
                    }

                    //* where
                    queryCurrentWd = queryCurrentWd.Where(p => p.DeletedFlag != true && p.EmployeeEntityId == employeeId);
                    queryCurrentWd = queryCurrentWd.AsNoTracking()
                         .Include(p => p.Employee)
                         .ThenInclude(jt => jt.Department)
                         .Include(p => p.Employee)
                         .ThenInclude(d => d.JobTitle)
                         .Include(x => x.WorkingType)
                          .Include(x => x.WorkingDayUpdates)
                         .ThenInclude(x => x.WorkingDayApprovals)
                        .Include(x => x.TimeSheets)
                         .OrderBy(x => x.WorkingDate)
                         .AsQueryable();

                    queryCurrentWd = queryCurrentWd.Where(s => (s.WorkingDate.Value >= startDate && s.WorkingDate.Value <= endDate));
                    rs = queryCurrentWd.ToList();

                    foreach (var item in rs)
                    {
                        if (item.TimeSheets != null && item.TimeSheets.Count > 0)
                        {
                            item.TimeSheets = item.TimeSheets.Where(x => x.DeletedFlag != true).ToList();
                        }
                    }

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

        public async Task<List<WorkingTypeProcess>> CalculateMonthWorkingType_bk(List<WorkingDayEntity> workingDayEntity)
        {
            List<WorkingTypeProcess> ret = new List<WorkingTypeProcess>();
            for (int i = 0; i < workingDayEntity.Count; i++)
            {
                var rcmWT = new WorkingTypeProcess()
                {
                    Date = workingDayEntity[i].WorkingDate.GetValueOrDefault()
                };
                var timeSheets = workingDayEntity[i].TimeSheets?.OrderBy(x => x.RecordedTime).ToList();
                if (timeSheets == null || timeSheets.Count <= 0) continue;
                double counterBeforeShift3 = 0;
                double counterExtendShift3 = 0;
                bool isCheckInDay = false;
                var timeCheckIns = timeSheets.Where(x => x.Status == EnumFaceId.Check_In).Select(x =>
                {
                    var timeCheckInNew = DateTimeUtil.RoundToNearest(x.RecordedTime.GetValueOrDefault(), TimeSpan.FromMinutes(30));
                    timeCheckInNew = new DateTime(1901, 1, 1, timeCheckInNew.Hour, timeCheckInNew.Minute, timeCheckInNew.Second);
                    return timeCheckInNew;
                })?.OrderByDescending(x => x)?.FirstOrDefault();
                var timeCheckOuts = timeSheets.Where(x => x.Status == EnumFaceId.Check_Out).Select(x =>
                {
                    var timeCheckInNew = DateTimeUtil.RoundToNearest(x.RecordedTime.GetValueOrDefault(), TimeSpan.FromMinutes(30));
                    timeCheckInNew = new DateTime(1901, 1, 1, timeCheckInNew.Hour, timeCheckInNew.Minute, timeCheckInNew.Second);
                    return timeCheckInNew;
                })?.OrderByDescending(x => x)?.FirstOrDefault();
                if (timeCheckIns.Value.Year == 0001) timeCheckIns = null;           //  if time is default value => set to null 
                if (timeCheckOuts.Value.Year == 0001) timeCheckOuts = null;     // 
                if (timeCheckIns == null && timeCheckOuts != null)
                {
                    timeCheckIns = timeCheckOuts.Value.AddHours(-8);
                }
                if (timeCheckOuts == null && timeCheckIns != null)
                {
                    if (i >= workingDayEntity.Count - 1)
                    {
                        timeCheckOuts = timeCheckIns.Value.AddHours(8);
                    }
                    else
                    {
                        timeCheckOuts = workingDayEntity[i + 1]
                          .TimeSheets
                          .FirstOrDefault(x => x.RecordedTime.Value.Year == rcmWT.Date.AddDays(1).Year
                          && x.RecordedTime.Value.Month == rcmWT.Date.AddDays(1).Month
                          && x.RecordedTime.Value.Day == rcmWT.Date.AddDays(1).Day
                          && x.Status == EnumFaceId.Check_Out
                          )?.RecordedTime;
                        timeCheckOuts = new DateTime(1901, 1, 2, timeCheckOuts.Value.Hour, timeCheckOuts.Value.Minute, timeCheckOuts.Value.Second);
                    }
                }
                if ((timeCheckIns >= ConstDatabase.StartTimeShift3 && timeCheckIns <= ConstDatabase.NextdayStartTimeShift1)) // if main shift is 3
                {
                    // check if any OT time from  0 -> 22 h
                    if (timeCheckOuts.Value.Day != timeCheckIns.Value.Day)
                    {
                        timeCheckOuts = timeCheckIns.GetValueOrDefault().AddHours(8);
                        counterBeforeShift3 = timeCheckOuts < ConstDatabase.StartTimeShift2.AddDays(1)
                        ? (timeCheckOuts - ConstDatabase.StartTimeShift1.AddDays(1)).Value.TotalHours
                        : (timeCheckOuts - ConstDatabase.StartTimeShift2.AddDays(1)).Value.TotalHours;
                    }
                    else
                    {
                        counterBeforeShift3 = timeCheckOuts < ConstDatabase.StartTimeShift2
                      ? (timeCheckOuts - ConstDatabase.StartTimeShift1).Value.TotalHours
                      : (timeCheckOuts - ConstDatabase.StartTimeShift2).Value.TotalHours;
                    }

                    rcmWT.Code = (counterBeforeShift3 > 0 ? $"{counterBeforeShift3}+" : "") + "C3";
                }
                else // main shift is 1,2
                {
                    if ((timeCheckIns < ConstDatabase.StartTimeShift1) || timeCheckOuts > ConstDatabase.StartTimeShift3) // đi sớm, hoặc về muộn lấn giờ ca 3 => tính OT <x>C3
                    {
                        rcmWT.Code = $"" +
                          $"{((ConstDatabase.StartTimeShift1 - timeCheckIns).Value.TotalHours > 0 ? (ConstDatabase.StartTimeShift1 - timeCheckIns).Value.TotalHours : 0) +
                          ((timeCheckOuts - ConstDatabase.StartTimeShift3).Value.TotalHours > 0 ? (timeCheckOuts - ConstDatabase.StartTimeShift3).Value.TotalHours : 0)}C3";
                    }
                    else
                    {
                        rcmWT.Code = $"{(timeCheckOuts - timeCheckIns)?.TotalHours - 8}"; // normal OT, if working hour is more than 8
                    }
                }
                /// 
                ret.Add(rcmWT);
            }

            return ret;
        }
        public List<WorkingDayEntity> GetEmployeeWorkingDayByDate(long employeeId, DateTime startTime, DateTime endTime, bool isTracking = false)
        {
            if (isTracking)
            {
                var listRS = _context.Set<WorkingDayEntity>()
                    //.AsNoTracking()
                    .Where(x => x.DeletedFlag != true)
                  .Where(x => x.EmployeeEntityId == employeeId && x.WorkingDate >= startTime && x.WorkingDate < endTime)
                  .Include(x => x.WorkingType)
                  .Include(x => x.TimeSheets)
                  .OrderBy(x => x.WorkingDate)
                  .ToList();
                foreach (var item in listRS)
                {
                    item.TimeSheets = item.TimeSheets.Where(x => x.DeletedFlag != true).ToList();
                }
                return listRS;
            }
            else
            {
                var listRS = _context.Set<WorkingDayEntity>()
                    .AsNoTracking()
                    .Where(x => x.DeletedFlag != true)
                  .Where(x => x.EmployeeEntityId == employeeId && x.WorkingDate >= startTime && x.WorkingDate < endTime)
                  .Include(x => x.WorkingType)
                  .Include(x => x.TimeSheets)
                  .OrderBy(x => x.WorkingDate)
                  .ToList();
                foreach (var item in listRS)
                {
                    item.TimeSheets = item.TimeSheets.Where(x => x.DeletedFlag != true).ToList();
                }
                return listRS;
            }
        }

        public Dictionary<long, List<WorkingDayEntity>> GetEmployeeWorkingDayByDate(
          List<long> listEmployeeId,
          DateTime startTime,
          DateTime endTime,
          bool isDirect = false)
        {
            try
            {
                Dictionary<long, List<WorkingDayEntity>>? result = null;
                //string cacheKey = $"{cacheKeyListByListIds}_GetEmployeeWorkingDayByDate_{startTime}_{endTime}:{(listEmployeeId == null ? "" : string.Join(",", listEmployeeId))}";
                //cacheKey = EncodeUtil.MD5(cacheKey);
                //if (!isDirect)
                //{
                //    result = CachedFunc.GetRedisData<Dictionary<long, List<WorkingDayEntity>>>(cacheKey, null);
                //}

                //if (result == null || result.Count <= 0)
                {
                    result = new Dictionary<long, List<WorkingDayEntity>>();
                    var list = _context.Set<WorkingDayEntity>()
                    .AsSplitQuery()
                    .Include(x => x.WorkingType)
                    .Include(x => x.TimeSheets)
                    .Include(x => x.Employee)
                    .ThenInclude(x=> x.Department)
                    .Include(x => x.Employee)
                    .ThenInclude(x => x.JobTitle)
                    .Include(x => x.WorkingDayUpdates)
                    .Where(x => x.DeletedFlag != true)
                    .Where(x => (x.EmployeeEntityId != null && listEmployeeId.Contains(x.EmployeeEntityId.GetValueOrDefault()))
                          && x.WorkingDate >= startTime
                          && x.WorkingDate <= endTime)
                    .OrderBy(x => x.WorkingDate)
                    .ToList();

                    foreach (var item in list)
                    {
                        if (item.TimeSheets != null && item.TimeSheets.Count > 0)
                        {
                            item.TimeSheets = item.TimeSheets.Where(x => x.DeletedFlag != true).ToList();
                        }
                    }

                    foreach (var item in list)
                    {
                        if (!result.ContainsKey(item.EmployeeEntityId.Value))
                        {
                            result.Add(item.EmployeeEntityId.Value, new List<WorkingDayEntity>());
                        }
                        result[item.EmployeeEntityId.Value].Add(item);
                    }

                    //CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
                    // TODO: error at convert to json so long when call SetRedisData()
                    //CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
                }
                return result;
            }
            catch (Exception dbEx)
            {
                throw new DBException(dbEx);
            }
        }

        /// <summary>
        ///  Get Employee current OT Hours, by default it cannot upper 40
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="workingTypeEntities"> list of working type OT </param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<int> GetOTHour(long employeeId, List<WorkingTypeEntity> workingTypeEntities, List<HolidayScheduleEntity> holidayScheduleEntities, DateTime startTime, DateTime endTime, WorkingDayEntity incomming = null)
        {
            var listWTIds = workingTypeEntities
              .Select(x => x.Code).ToList();
            var ots = _context.Set<WorkingDayEntity>()
              .Include(x => x.WorkingType)
              .AsNoTracking()
              .Where(x => x.DeletedFlag != true)
              .Where(x => x.WorkingDate >= startTime && x.WorkingDate <= endTime)
              .Where(x => x.EmployeeEntityId == employeeId)
              .Where(x => (x.WorkingType != null && listWTIds.Contains(x.WorkingType.Code)) || (x.RecommendType != null && listWTIds.Contains(x.RecommendType))).ToList();
            if (incomming != null)
            {
                ots.Add(incomming);
            }
            int sumOT = 0;
            foreach (var ot in ots)
            {
                if (ot.WorkingType == null && ot.WorkingTypeEntityId.GetValueOrDefault() > 0)
                {
                    ot.WorkingType = workingTypeEntities.FirstOrDefault(x => x.Id == ot.WorkingTypeEntityId);
                }
                sumOT += await GetOTHour(workingTypeEntities, holidayScheduleEntities, ot);
            }
            return sumOT;
        }
        public async Task<int> GetOTHour(List<WorkingTypeEntity> workingTypeEntities, List<HolidayScheduleEntity> holidayScheduleEntities, List<WorkingDayEntity> ots)
        {
            var listWTIds = workingTypeEntities
              .Select(x => x.Code).ToList();
            int sumOT = 0;
            foreach (var ot in ots)
            {
                if (ot.WorkingType == null && ot.WorkingTypeEntityId.GetValueOrDefault() > 0)
                {
                    ot.WorkingType = workingTypeEntities.FirstOrDefault(x => x.Id == ot.WorkingTypeEntityId);
                }
                if (ot.WorkingTypeEntityId == null && ot.RecommendType != null)
                {
                    ot.WorkingType = workingTypeEntities.FirstOrDefault(x => x.Code == ot.RecommendType);
                }
                sumOT += await GetOTHour(workingTypeEntities, holidayScheduleEntities, ot);
            }
            return sumOT;
        }
        public async Task<OTModel> GetOTHours(List<WorkingTypeEntity> workingTypeEntities, List<HolidayScheduleEntity> holidayScheduleEntities, List<WorkingDayEntity> ots)
        {
            var listWTIds = workingTypeEntities
              .Select(x => x.Code).ToList();
            var lastOT = new OTModel();
            var retOTs = new List<WorkingDayEntity>();
            var added = new WorkingDayEntity();
            var lastOTHours = lastOT.Total;
            foreach (var ot in ots)
            {
                if (ot.WorkingType == null && ot.WorkingTypeEntityId.GetValueOrDefault() > 0)
                {
                    ot.WorkingType = workingTypeEntities.FirstOrDefault(x => x.Id == ot.WorkingTypeEntityId);
                }
                if (ot.WorkingTypeEntityId == null && ot.RecommendType != null)
                {
                    ot.WorkingType = workingTypeEntities.FirstOrDefault(x => x.Code == ot.RecommendType);
                }
                added = ot.Copy();
                lastOT = await GetOTHour(workingTypeEntities, holidayScheduleEntities, ot, lastOT);
                if (lastOTHours < lastOT.Total)
                {
                    if (lastOT.Total > GlobalConsts.MAXIMUM_OT_Hour)
                    {
                        var commonWktype = workingTypeEntities.FirstOrDefault(x => x.Code == "0");

                        added.WorkingTypeEntityId = commonWktype?.Id;
                        added.WorkingType = commonWktype;
                    }
                    lastOTHours = lastOT.Total;
                }
                retOTs.Add(added);
            }
            lastOT.AfterProcessWds = retOTs;
            //this.UpSertMulti(retOTs);
            return lastOT;
        }
        public async Task<int> GetOTHour(
            List<WorkingTypeEntity> workingTypeEntities,
            List<HolidayScheduleEntity> holidayScheduleEntities,
            WorkingDayEntity ot)
        {
            var listWTIds = workingTypeEntities
              .Select(x => x.Id).ToList();
            int sumOT = 0;
            int ot150 = 0, ot200 = 0, weeken_200 = 0, ot_night_270 = 0, ot_300 = 0, ot_390 = 0;
            var holiday = holidayScheduleEntities.Where(x => x.StartDate <= ot.WorkingDate && x.EndDate >= ot.WorkingDate).FirstOrDefault();
            if (holiday != null)
            {
                switch (holiday.HolidayType.GetValueOrDefault())
                {
                    case EnumHolidayCode.HLD:

                        ot_300 += (ot.WorkingType?.Holiday_OT_300).GetValueOrDefault();
                        //hld_meal += (ot.WorkingType?.Holiday_Meal).GetValueOrDefault();
                        ot_390 += (ot.WorkingType?.Holiday_OT_Night_390).GetValueOrDefault();
                        break;
                    case EnumHolidayCode.OFF:
                        weeken_200 += (ot.WorkingType?.Weekend_OT_200).GetValueOrDefault();
                        //weeken_meal += (ot.WorkingType?.Weekend_Meal).GetValueOrDefault();
                        ot_night_270 += (ot.WorkingType?.Weekend_Night_OT_270).GetValueOrDefault();
                        break;
                    case EnumHolidayCode.XMS:
                        var weeken = ot.WorkingDate.GetValueOrDefault().DayOfWeek;
                        if (weeken == DayOfWeek.Saturday || weeken == DayOfWeek.Sunday)
                        {
                            weeken_200 += (ot?.WorkingType?.Weekend_OT_200).GetValueOrDefault();
                            //weeken_meal += (ot.WorkingType?.Weekend_Meal).GetValueOrDefault();
                            ot_night_270 += (ot.WorkingType?.Weekend_Night_OT_270).GetValueOrDefault();
                        }
                        else
                        {
                            //meal += (ot.WorkingType?.Normal_Meal).GetValueOrDefault();
                            ot150 += (ot.WorkingType?.OT_150).GetValueOrDefault();
                            ot200 += (ot.WorkingType?.OT_200).GetValueOrDefault();
                        }
                        break;
                }
            }
            else
            {
                var weeken = ot.WorkingDate.GetValueOrDefault().DayOfWeek;
                if (weeken == DayOfWeek.Saturday || weeken == DayOfWeek.Sunday)
                {
                    weeken_200 += (ot.WorkingType?.Weekend_OT_200).GetValueOrDefault();
                    //weeken_meal += (ot.WorkingType?.Weekend_Meal).GetValueOrDefault();
                    ot_night_270 += (ot.WorkingType?.Weekend_Night_OT_270).GetValueOrDefault();
                }
                else
                {
                    //meal += (ot.WorkingType?.Normal_Meal).GetValueOrDefault();
                    ot150 += (ot.WorkingType?.OT_150).GetValueOrDefault();
                    ot200 += (ot.WorkingType?.OT_200).GetValueOrDefault();
                }
            }
            return /*meal +*/ ot150 + ot200 +/* weeken_meal +*/ weeken_200 + ot_night_270 + /*hld_meal +*/ ot_300 + ot_390;
        }
        public async Task<OTModel> GetOTHour(
           List<WorkingTypeEntity> workingTypeEntities,
           List<HolidayScheduleEntity> holidayScheduleEntities,
           WorkingDayEntity ot,
           OTModel last)
        {
            var ret = new OTModel()
            {
                EmpCode = ot.Employee?.EmployeeCode,
                Name = ot.EmployeeEntityId?.ToString(),
            };
            var listWTIds = workingTypeEntities
              .Select(x => x.Id).ToList();
            int sumOT = 0;
            int meal = 0, ot150 = 0, ot200 = 0, weeken_meal = 0, weeken_200 = 0, ot_night_270 = 0, hld_meal = 0, ot_300 = 0, ot_390 = 0;
            var holiday = holidayScheduleEntities.Where(x => x.StartDate <= ot.WorkingDate && x.EndDate >= ot.WorkingDate).FirstOrDefault();
            if (holiday != null)
            {
                switch (holiday.HolidayType.GetValueOrDefault())
                {
                    case EnumHolidayCode.HLD:

                        ot_300 += (ot.WorkingType?.Holiday_OT_300).GetValueOrDefault();
                        hld_meal += (ot.WorkingType?.Holiday_Meal).GetValueOrDefault();
                        ot_390 += (ot.WorkingType?.Holiday_OT_Night_390).GetValueOrDefault();
                        break;
                    case EnumHolidayCode.OFF:
                        weeken_200 += (ot.WorkingType?.Weekend_OT_200).GetValueOrDefault();
                        weeken_meal += (ot.WorkingType?.Weekend_Meal).GetValueOrDefault();
                        ot_night_270 += (ot.WorkingType?.Weekend_Night_OT_270).GetValueOrDefault();
                        break;
                    case EnumHolidayCode.XMS:
                        var weeken = ot.WorkingDate.GetValueOrDefault().DayOfWeek;
                        if (weeken == DayOfWeek.Saturday || weeken == DayOfWeek.Sunday)
                        {
                            weeken_200 += (ot?.WorkingType?.Weekend_OT_200).GetValueOrDefault();
                            weeken_meal += (ot.WorkingType?.Weekend_Meal).GetValueOrDefault();
                            ot_night_270 += (ot.WorkingType?.Weekend_Night_OT_270).GetValueOrDefault();
                        }
                        else
                        {
                            meal += (ot.WorkingType?.Normal_Meal).GetValueOrDefault();
                            ot150 += (ot.WorkingType?.OT_150).GetValueOrDefault();
                            ot200 += (ot.WorkingType?.OT_200).GetValueOrDefault();
                        }
                        break;
                }
            }
            else
            {
                var weeken = ot.WorkingDate.GetValueOrDefault().DayOfWeek;
                if (weeken == DayOfWeek.Saturday || weeken == DayOfWeek.Sunday)
                {
                    weeken_200 += (ot.WorkingType?.Weekend_OT_200).GetValueOrDefault();
                    weeken_meal += (ot.WorkingType?.Weekend_Meal).GetValueOrDefault();
                    ot_night_270 += (ot.WorkingType?.Weekend_Night_OT_270).GetValueOrDefault();
                }
                else
                {
                    meal += (ot.WorkingType?.Normal_Meal).GetValueOrDefault();
                    ot150 += (ot.WorkingType?.OT_150).GetValueOrDefault();
                    ot200 += (ot.WorkingType?.OT_200).GetValueOrDefault();
                }
            }
            last.OT150Hours = last.Total + ot150 > GlobalConsts.MAXIMUM_OT_Hour && ot150 > 0 ? last.OT150Hours + ot150 : last.OT150Hours;
            last.Total += ot150;
            last.OT200Hours = last.Total + ot200 + weeken_200 > GlobalConsts.MAXIMUM_OT_Hour && ot200 + weeken_200 > 0 ? last.OT200Hours + ot200 + weeken_200 : last.OT200Hours;
            last.Total += ot200 + weeken_200;
            last.OT270NightHours = last.Total + ot_night_270 > GlobalConsts.MAXIMUM_OT_Hour && ot_night_270 > 0 ? last.OT270NightHours + ot_night_270 : last.OT270NightHours;
            last.Total += ot_night_270;
            last.OT300Hours = last.Total + ot_300 > GlobalConsts.MAXIMUM_OT_Hour && ot_300 > 0 ? last.OT300Hours + ot_300 : last.OT300Hours;
            last.Total += ot_300;
            last.OT390Hours = last.Total + ot_390 > GlobalConsts.MAXIMUM_OT_Hour && ot_390 > 0 ? last.OT390Hours + ot_390 : last.OT390Hours;
            last.Total += ot_390;
            return last;
        }
        public async Task<WorkingDayEntity> GetEmpDate(DateTime date, long empId)
        {

            var data = await _context.Set<WorkingDayEntity>()
              .Where(x => x.DeletedFlag != true)
              .Include(x => x.TimeSheets)
              .Include(x => x.WorkingDayUpdates)
              .FirstOrDefaultAsync(x => x.WorkingDate.Value.Year == date.Year
              && x.WorkingDate.Value.Month == date.Month
              && x.WorkingDate.Value.Day == date.Day
              && x.DeletedFlag != true
              && x.EmployeeEntityId == empId
              );
            return data;

        }

        public async Task<WorkingDayEntity> GetByTimeSheet(TimeSheetEntity timeSheetEntity)
        {
            return _context.Set<WorkingDayEntity>()
              .AsNoTracking()
              .Include(x => x.Employee)
              .Where(x => x.DeletedFlag != true)
              .FirstOrDefault(x => x.EmployeeEntityId == timeSheetEntity.EmployeeId
              && x.WorkingDate.Value.Day == timeSheetEntity.RecordedTime.Value.Day
            && x.WorkingDate.Value.Month == timeSheetEntity.RecordedTime.Value.Month
              && x.WorkingDate.Value.Year == timeSheetEntity.RecordedTime.Value.Year);
            ;
        }
        private async Task<WorkingDayEntity> CheckIfExist(WorkingDayEntity entity)
        {
            return await _context.Set<WorkingDayEntity>()
                .Where(x => x.DeletedFlag != true)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);
        }

        public Task<List<WorkingDayEntity>> GetWorkingDayByDate(DateTime startTime, DateTime endTime)
        {
            return _context.Set<WorkingDayEntity>()
              .Where(x => x.DeletedFlag != true)
              .Where(x => x.WorkingDate >= startTime && x.WorkingDate <= endTime)
              .Include(x => x.Employee)
              .ThenInclude(x => x.Department)
              .Include(x => x.Employee)
              .ThenInclude(x => x.JobTitle)
              .OrderBy(x => x.WorkingDate)
              .ToListAsync();
        }
        public Task<List<WorkingDayEntity>> GetWorkingDayByDate(DateTime time)
        {
            return _context.Set<WorkingDayEntity>()
              .Where(x => x.WorkingDate.Value.Day == time.Day
              && x.WorkingDate.Value.Month == time.Month
              && x.WorkingDate.Value.Year == time.Year
              && x.DeletedFlag != true
              && x.WorkingTypeEntityId != null
              )
              .ToListAsync();
        }
        public OTListPagingResponseModel GetListOTWorkingDay(
                                                     EmployeeAttendanceListOTRequest pagingReq,
                                                     Dictionary<string, object> paramFilter,
                                                     SearchModel paramSearch,
                                                     Dictionary<string, long> paramSort,
                                                     List<WorkingTypeEntity> wktypeOTs
            )
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListOTWorkingDay";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}" +
                                $"_{paramFilter.ToJson()}" +
                                $"_{paramSearch.ToJson()}" +
                                $"_{paramSort.ToJson()}";
                }
                OTListPagingResponseModel rs = CachedFunc.GetRedisData<OTListPagingResponseModel>(cacheKey, null);
                if (rs == null || rs.rawDatas == null)
                {
                    rs = new OTListPagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new EmployeeAttendanceListOTRequest();
                        pagingReq.Page = 0;
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }
                    if (pagingReq.PageSize <= 0)
                    {
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }
                    DateTime? startDate = DateTimeHelper.GetStartOfDate(pagingReq.DateFrom.GetValueOrDefault());
                    DateTime? endDate = DateTimeHelper.GetEndOfDate(pagingReq.DateTo.GetValueOrDefault());
                    var wkOTIds = wktypeOTs.Select(x => x.Id);
                    var wkOTCodes = wktypeOTs.Select(x => x.Code);
                    var queryCurrentWd = _context.Set<WorkingDayEntity>()
                      .AsNoTracking()
                      .AsQueryable();
                    //* where
                    queryCurrentWd = queryCurrentWd.Where(p => p.DeletedFlag != true);
                    queryCurrentWd = queryCurrentWd
                        .AsNoTracking()
                         .Include(p => p.Employee)
                         .ThenInclude(jt => jt.Department)
                         .Include(p => p.Employee)
                         .ThenInclude(d => d.JobTitle)
                         .Include(x => x.WorkingType)
                         .OrderBy(x => x.WorkingDate)
                         .Include(x => x.WorkingDayUpdates)
                         .ThenInclude(x => x.WorkingDayApprovals)
                         .Where(x => x.Employee != null && x.Employee.DeletedFlag != true /*&& pagingReq.DepartmentId.Contains(x.Employee.DepartmentId.Value)*/)
                         .Where(x => (x.WorkingType == null && wkOTCodes.Contains(x.RecommendType)) || (x.WorkingType != null && wkOTCodes.Contains(x.WorkingType.Code)))
                         .AsQueryable();

                    queryCurrentWd = OTListResponseModel.PrepareDetailReportWhereQueryFilter(queryCurrentWd, paramFilter, wktypeOTs);
                    queryCurrentWd = OTListResponseModel.PrepareDetailReportWhereQuerySearch(queryCurrentWd, paramSearch);
                    queryCurrentWd = OTListResponseModel.PrepareDetailReportQuerySort(queryCurrentWd, paramSort);
                    queryCurrentWd = queryCurrentWd.Where(s => (s.WorkingDate.Value >= startDate && s.WorkingDate.Value <= endDate));
                    rs.TotalRecord = queryCurrentWd.LongCount();
                    rs.rawDatas = queryCurrentWd.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit()).ToList()
                         .GroupBy(x => x.EmployeeEntityId)
                         .SelectMany(x => x).ToList();

                    rs.ListData = rs.rawDatas
                      .Select(x => new OTListResponseModel().SetData(x, wktypeOTs))
                      .ToList();
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

        public OTListPagingResponseModel GetListOTWorkingDayV2(
                                                     EmployeeAttendanceListOTRequest request,
                                                     Dictionary<string, object> paramFilter,
                                                     SearchModel paramSearch,
                                                     Dictionary<string, long> paramSort,
                                                     List<WorkingDayEntity> listWorkingDay,
                                                     List<WorkingTypeEntity> wktypeOTs
            )
        {
            try
            {
                OTListPagingResponseModel rs = new OTListPagingResponseModel();
                var wkOTIds = wktypeOTs.Select(x => x.Id);
                var wkOTCodes = wktypeOTs.Select(x => x.Code);

                //listWorkingDay = listWorkingDay.Where(x => x.Employee != null && x.Employee.DeletedFlag != true /*&& pagingReq.DepartmentId.Contains(x.Employee.DepartmentId.Value)*/)
                //         .Where(x => (x.WorkingType == null && wkOTCodes.Contains(x.RecommendType)) || (x.WorkingType != null && wkOTCodes.Contains(x.WorkingType.Code))).ToList();

                //if (listWorkingDay == null && !listWorkingDay.Any())
                //{
                //    return null;
                //}

                listWorkingDay = OTListResponseModel.PrepareDetailReportWhereQueryFilterV2(listWorkingDay, paramFilter, wktypeOTs);
                listWorkingDay = OTListResponseModel.PrepareDetailReportWhereQuerySearchV2(listWorkingDay, paramSearch);
                listWorkingDay = OTListResponseModel.PrepareDetailReportQuerySortV2(listWorkingDay, paramSort);

                rs.TotalRecord = listWorkingDay.LongCount();
                rs.rawDatas = listWorkingDay.Skip(request.GetSkip()).Take(request.GetLimit()).ToList()
                     .GroupBy(x => x.EmployeeEntityId)
                     .SelectMany(x => x).ToList();

                rs.ListData = rs.rawDatas
                  .Select(x => new OTListResponseModel().SetData(x, wktypeOTs))
                  .ToList();
                return rs;
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
        public WorkingDayEntity? GetWorkingDayEmployeeByDate(long employeeId, DateTime workingDate, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                WorkingDayEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<WorkingDayEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<WorkingDayEntity>();
                IQueryable<WorkingDayEntity> queryable;
                if (!isTracking)
                {
                    queryable = dataSet.AsNoTracking().AsQueryable();
                }
                else
                {
                    queryable = dataSet.AsQueryable();
                }
                result = queryable
                  .Include(x => x.Employee)
                            .AsQueryable()
                            /*[GEN-7]*/
                            .Where(entity => entity.DeletedFlag != true && entity.EmployeeEntityId == employeeId)
                            .Where(entity => entity.DeletedFlag != true && entity.WorkingDate == workingDate)
                            .FirstOrDefault();
                //result.WorkingDay2s = result.WorkingDay2s.Select(x => new WorkingDay2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public List<WorkingDayEntity> GetListByEmployeeIdAndRangeTime(long employeeId, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}";
                cacheKey = $"{cacheKeyList}:GetListByEmployeeIdAndRangeTime_{employeeId}_{dateFrom}_{dateTo}";
                List<WorkingDayEntity>? result = CachedFunc.GetRedisData<List<WorkingDayEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<WorkingDayEntity>();
                    result = _context.Set<WorkingDayEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .Include(entity => entity.TimeSheets)
                      .Include(entity => entity.Employee)
                      .ThenInclude(entity => entity.Department)
                      .Include(entity => entity.Employee)
                      .ThenInclude(entity => entity.JobTitle)
                      /*[GEN-13]*/
                      .Where(entity => entity.DeletedFlag != true)
                      .Where(entity => entity.EmployeeEntityId == employeeId).ToList();

                    result = result.Where(entity => entity.WorkingDate >= dateFrom && entity.WorkingDate <= dateTo)
                                    .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                                    .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord).ToList();
                    for (var i = 0; i < result.Count; i++)
                    {


                        /*[GEN-14]*/
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
    }
}
