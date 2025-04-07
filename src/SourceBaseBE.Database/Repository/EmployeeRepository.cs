using iSoft.Common;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.RequestModels;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using iSoft.Redis.Services;
using SourceBaseBE.Database.DBContexts;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Models.RequestModels;
using System.Linq.Dynamic.Core;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Utils;
using NPOI.SS.Formula.Functions;
using SourceBaseBE.Database.Models.RequestModels.Report;
using iSoft.Database.Models;
using iSoft.Common.Enums;
using System.Linq;

namespace SourceBaseBE.Database.Repository
{
    public class EmployeeRepository : BaseCRUDRepository<EmployeeEntity>
    {
        public override string TableName
        {
            get { return "Employees"; }
        }
        public EmployeeRepository(CommonDBContext dbContext)
          : base(dbContext)
        {
        }
        public override string GetName()
        {
            return nameof(EmployeeRepository);
        }
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override EmployeeEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                EmployeeEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<EmployeeEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<EmployeeEntity>();
                IQueryable<EmployeeEntity> queryable;
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
                //result.Employee2s = result.Employee2s.Select(x => new Employee2Entity() { Id = x.Id, Name = x.Name }).ToList();

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

        public EmployeeEntity? GetById(long? id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                EmployeeEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<EmployeeEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<EmployeeEntity>();
                IQueryable<EmployeeEntity> queryable;
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
                //result.Employee2s = result.Employee2s.Select(x => new Employee2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public override async Task<EmployeeEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                EmployeeEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}_GetByIdAsync:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<EmployeeEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<EmployeeEntity>();
                IQueryable<EmployeeEntity> queryable;
                if (!isTracking)
                {
                    queryable = dataSet.AsNoTracking().AsQueryable();
                }
                else
                {
                    queryable = dataSet.AsQueryable();
                }
                var data = queryable
                        .Where(entity => entity.DeletedFlag != true && entity.Id == id);

                result = await data.Include(x => x.JobTitle).Include(x => x.Department).FirstOrDefaultAsync();
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
        public override List<EmployeeEntity> GetList(PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetList";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<EmployeeEntity>? result = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<EmployeeEntity>();
                    if (pagingReq != null)
                    {
                        result = _context.Set<EmployeeEntity>()
                          .AsNoTracking()
                          .AsQueryable()
                          /*[GEN-11]*/
                          .Where(entity => entity.DeletedFlag != true)
                        .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                          .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                          .AsParallel()
                          .ToList();

                        //.Select(entity => new EmployeeEntity
                        // {
                        //   Id = entity.Id,
                        //   Employee2s = entity.Employee2s.Select(x => new Employee2Entity { Id = x.Id, Name = x.Name }).ToList()
                        // })

                        for (var i = 0; i < result.Count; i++)
                        {


                            /*[GEN-12]*/
                        }
                    }
                    else
                    {
                        result = _context.Set<EmployeeEntity>()
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

        /// <summary>
        /// GetList (@GenCRUD)
        /// </summary>
        /// <param name="pagingReq"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public List<EmployeeEntity> GetListEmployeeIncludeDepartment()
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListEmployeeIncludeDepartment";

                cacheKey = $"{cacheKeyList}_GetListEmployeeIncludeDepartment";

                List<EmployeeEntity>? result = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<EmployeeEntity>();
                    result = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      /*[GEN-13]*/
                      .Where(entity => entity.DeletedFlag != true)
                      .Include(entity => entity.Department)
                      .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                      .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
                      .AsParallel()
                      .ToList();

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


        public List<EmployeeEntity> GetList()
        {
            try
            {
                List<EmployeeEntity> result = _context.Set<EmployeeEntity>()
                  .AsNoTracking()
                  .AsQueryable()
                  /*[GEN-13]*/
                  .Where(entity => entity.DeletedFlag != true)
                  .Include(entity => entity.JobTitle)
                  .Include(entity => entity.Department)
                  .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
                  .OrderBy(entity => entity.Department)
                  .ThenBy(entity => entity.Id)
                  .AsParallel()
                  .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public override List<EmployeeEntity> GetAll()
        {
            try
            {
                List<EmployeeEntity>? result = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKeyAll, null);
                if (result == null)
                {
                    result = new List<EmployeeEntity>();
                    result = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .Where(entity => entity.DeletedFlag != true)
                      .OrderBy(entity => EF.Functions.Unaccent(entity.Name.ToLower())).ThenBy(entity => entity.Order).ThenBy(entity => entity.Id)
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
        public List<EmployeeEntity> GetAll(List<long> acceptDepartmentIds)
        {
            try
            {
                List<EmployeeEntity>? result = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKeyAll, null);
                if (result == null)
                {
                    result = new List<EmployeeEntity>();
                    result = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .Where(entity => entity.DeletedFlag != true && entity.DepartmentId != null && acceptDepartmentIds.Contains(entity.DepartmentId.Value))
                      .OrderBy(entity => EF.Functions.Unaccent(entity.Name.ToLower())).ThenBy(entity => entity.Order).ThenBy(entity => entity.Id)
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
        public Task<List<EmployeeEntity>> GetCurrentEmployee()
        {
            try
            {
                var listEmp = _context.Set<EmployeeEntity>()
                  .Where(p => p.DeletedFlag != true)
                      .Include(p => p.JobTitle)
                      .Include(p => p.Department)
                      .OrderBy(p => p.Department)
                      .ThenBy(p => p.Id)
                  .ToListAsync();
                return listEmp;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public EmployeeEntity Upsert(EmployeeEntity entity/*[GEN-8]*/, long? userId = null, bool isTracked = false)
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
                    entity = Update(entity/*[GEN-4]*/, userId, isTracked);
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public EmployeeEntity Insert(EmployeeEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<EmployeeEntity>().Add(entity);
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
        public new EmployeeEntity Update(EmployeeEntity entity/*[GEN-8]*/, long? userId = null, bool isTracked = false)
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
                    entity = base.Update(entity);
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDirect"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public EmployeeEntity? GetByEmpCode(string name, bool isDirect = false)
        {
            if (string.IsNullOrEmpty(name)) return null;
            try
            {
                EmployeeEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}_GetByEmpCode:{name}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<EmployeeEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<EmployeeEntity>()
                      .AsQueryable()
                      .AsNoTracking()
                      .FirstOrDefault(entity => entity.DeletedFlag != true
                      && entity.EmployeeCode != null
                      && entity.EmployeeCode == name);

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
        public EmployeeEntity? GetByCodes(string empCode, string empMachineCode, bool isDirect = false)
        {
            try
            {
                EmployeeEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetByCodes_{empCode}:{empMachineCode}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<EmployeeEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .FirstOrDefault(entity => entity.DeletedFlag != true
                      && entity.EmployeeCode.ToLower() == empCode.ToLower()
                      && entity.EmployeeMachineCode.ToLower() == empMachineCode.ToLower()
                      );

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
        public EmployeeEntity? GetByEmpMachineCode(string empMachineCode, bool isDirect = false)
        {
            try
            {
                EmployeeEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetByEmpMachineCode_{empMachineCode}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<EmployeeEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .FirstOrDefault(entity => entity.DeletedFlag != true && entity.EmployeeMachineCode.ToLower() == empMachineCode.ToLower());

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
        public override string GetDisplayField(EmployeeEntity entity)
        {
            return entity.Name.ToString();
        }

        public override IEnumerable<EmployeeEntity> InsertMulti(IEnumerable<EmployeeEntity> entities, long? userId = null)
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
                        _context.Set<EmployeeEntity>().AddRange(entity);
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

        //



        public async Task<EmployeePagingResponseModel> GetListAttendanceReport(
         TotalReportListRequest pagingReq,
         Dictionary<string, object> paramFilter,
         SearchModel paramSearch,
         Dictionary<string, long> paramSort,
         long? currentUserId = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListAttendanceReport";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);
                EmployeePagingResponseModel rs = CachedFunc.GetRedisData<EmployeePagingResponseModel>(cacheKey, null);
                if (rs == null /*|| rs.ListData.Count == 0*/)
                {
                    rs = new EmployeePagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new TotalReportListRequest();
                    }
                    if (pagingReq.Page <= 1)
                    {
                        pagingReq.Page = 1;
                    }
                    if (pagingReq.PageSize <= 0)
                    {
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }
                    var query = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .AsQueryable();

                    //* where
                    query = query.Where(p => p.DeletedFlag != true)
                       .Include(p => p.Department)
                       .Include(p => p.JobTitle)
                       .Where(x => x.DepartmentId != null && pagingReq.DepartmentId.Contains(x.DepartmentId))
                       .OrderBy(x => x.Id);

                    query = DashboardResponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = DashboardResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = DashboardResponseModel.PrepareQuerySort(query, paramSort);

                    if (query == null) throw new Exception("Empty Requester Return");
                    rs.rawDatas = await (query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit()).ToListAsync());
                    rs.ListData = rs.rawDatas
                      .Select(x => new EmployeeListAttendanceResponseModel().SetData(x))
                      .ToList();
                    rs.TotalRecord = query.LongCount();
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
        public List<EmployeeEntity> GetByDepartment(DepartmentEntity departmentEntity, bool isDirect = false)
        {
            try
            {
                List<EmployeeEntity>? result = null;
                string cacheKey = $"{cacheKeyList}_GetByDepartment:{departmentEntity?.Id}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKey, null);
                }

                if (result == null)
                {
                    if (departmentEntity != null)
                    {
                        result = _context
                          .Set<EmployeeEntity>()
                          .Include(x => x.Department)
                          .Include(x => x.JobTitle)
                          .AsNoTracking()
                          .Where(x => x.DepartmentId == departmentEntity.Id)
                          .Select(x => new EmployeeEntity()
                          {
                              Id = x.Id,
                              EmployeeCode = x.EmployeeCode,
                              Name = x.Name,
                              JoiningDate = x.JoiningDate,
                              Address = x.Address,
                              FirstName = x.FirstName,
                              LastName = x.LastName,
                              MiddleName = x.MiddleName,
                              FullName = x.FullName,
                              Email = x.Email,
                              Birthday = x.Birthday,
                              DisplayName = x.DisplayName,
                              PhoneNumber = x.PhoneNumber,

                              JobTitle = new JobTitleEntity()
                              {
                                  Name = x.JobTitle == null ? "" : x.JobTitle.Name
                              },

                              Department = new DepartmentEntity()
                              {
                                  Name = x.Department == null ? "" : x.Department.Name
                              }
                          })
                          .ToList();
                    }
                    else
                    {
                        result = _context
                          .Set<EmployeeEntity>()
                          .Include(x => x.Department)
                          .Include(x => x.JobTitle)
                          .AsNoTracking()
                          .Where(x => x.DepartmentId == null)
                          .Select(x => new EmployeeEntity()
                          {
                              Id = x.Id,
                              EmployeeCode = x.EmployeeCode,
                              Name = x.Name,
                              JoiningDate = x.JoiningDate,
                              Address = x.Address,
                              FirstName = x.FirstName,
                              LastName = x.LastName,
                              MiddleName = x.MiddleName,
                              FullName = x.FullName,
                              Email = x.Email,
                              Birthday = x.Birthday,
                              DisplayName = x.DisplayName,
                              PhoneNumber = x.PhoneNumber,

                              JobTitle = new JobTitleEntity()
                              {
                                  Name = x.JobTitle == null ? "" : x.JobTitle.Name
                              },

                              Department = new DepartmentEntity()
                              {
                                  Name = x.Department == null ? "" : x.Department.Name
                              }
                          })
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

        public List<EmployeeEntity> GetByDepartmentWithContract(DepartmentEntity departmentEntity, List<long>? contracts, bool isDirect = false)
        {
            try
            {
                List<EmployeeEntity>? result = null;
                string cacheKey = $"{cacheKeyList}_GetByDepartment:{departmentEntity?.Id}_{contracts}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKey, null);
                }

                if (result == null)
                {
                    IQueryable<EmployeeEntity> query;

                    if (departmentEntity != null)
                    {
                        query = _context
                          .Set<EmployeeEntity>()
                          .Include(x => x.Department)
                          .Include(x => x.JobTitle)
                          .AsNoTracking()
                          .Where(x => x.DeletedFlag != true)
                          .Where(x => x.DepartmentId == departmentEntity.Id);
                    }
                    else
                    {
                        query = _context
                          .Set<EmployeeEntity>()
                          .Include(x => x.Department)
                          .Include(x => x.JobTitle)
                          .AsNoTracking()
                          .Where(x => x.DeletedFlag != true)
                          .Where(x => x.DepartmentId == null);
                    }
                    query = query.OrderBy(x => EF.Functions.Unaccent(x.Name.ToLower()));
                    // Apply contract filter if contracts list is provided
                    if (contracts != null && contracts.Count > 0)
                    {
                        var contractEnums = contracts.Select(c => (EnumTypeContract)Enum.ToObject(typeof(EnumTypeContract), c)).ToList();
                        query = query.Where(x => x.Contract.HasValue && contractEnums.Contains(x.Contract.Value));
                    }



                    result = query.Select(x => new EmployeeEntity()
                    {
                        Id = x.Id,
                        EmployeeCode = x.EmployeeCode,
                        Name = x.Name,
                        JoiningDate = x.JoiningDate,
                        Address = x.Address,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        MiddleName = x.MiddleName,
                        FullName = x.FullName,
                        Email = x.Email,
                        Birthday = x.Birthday,
                        DisplayName = x.DisplayName,
                        PhoneNumber = x.PhoneNumber,
                        Contract = x.Contract,

                        JobTitle = new JobTitleEntity()
                        {
                            Name = x.JobTitle == null ? "" : x.JobTitle.Name
                        },

                        Department = new DepartmentEntity()
                        {
                            Name = x.Department == null ? "" : x.Department.Name
                        }
                    }).ToList();

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

        public EmployeeDepartmentPagingResponseModel GetListEmployeeDepartment(
          PagingParamRequestModel pagingReq,
          Dictionary<string, object> paramFilter,
          SearchModel paramSearch,
          Dictionary<string, long> paramSort)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_Employee_Department_Setting";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                EmployeeDepartmentPagingResponseModel rs = CachedFunc.GetRedisData<EmployeeDepartmentPagingResponseModel>(cacheKey, null);
                if (rs == null)
                {
                    rs = new EmployeeDepartmentPagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new PagingParamRequestModel();
                        pagingReq.Page = 0;
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }

                    var query = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .AsQueryable();

                    //* where
                    if (pagingReq.DepartmentId != null)
                    {
                        query = query.Where(p => p.DeletedFlag != true)
                          .Where(p => p.DepartmentId == pagingReq.DepartmentId)
                          .Include(p => p.Department)
                          .Include(p => p.JobTitle);
                    }
                    else
                    {
                        query = query.Where(p => p.DeletedFlag != true)
                          .Include(p => p.Department)
                          .Include(p => p.JobTitle);
                    }


                    query = query.OrderBy(x => x.Id);
                    query = DepartmentListEmployeeResponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = DepartmentListEmployeeResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = DepartmentListEmployeeResponseModel.PrepareQuerySort(query, paramSort);

                    if (query == null) throw new Exception("Empty Requester Return");
                    rs.ListData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                      .Select(x => new DepartmentListEmployeeResponseModel().SetData(x))
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
        public async Task<bool> RemoveIfNotExistInList(List<EmployeeEntity> employeeEntities)
        {
            try
            {
                var listEmp = _context.Set<EmployeeEntity>()
                  .Where(p => !employeeEntities.Contains(p))
                  .ToList();
                _context.RemoveRange(listEmp);
                await _context.SaveChangesAsync();
                ClearCachedData();
                return true;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public List<EmployeeEntity> GetByListEmployeeCode(List<string> listCode, bool isDirect = false)
        {
            try
            {
                List<EmployeeEntity>? result = null;
                string cacheKey = $"{cacheKeyDetail}_GetByListEmployeeCode:{EncodeUtil.MD5(string.Join(",", listCode))}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context
                              .Set<EmployeeEntity>()
                              .AsNoTracking()
                              .Include(x => x.Department)
                              .Include(x => x.JobTitle)
                              .Where(x => x.DeletedFlag != true)
                              .Where(x => x.EmployeeCode != null && listCode.Contains(x.EmployeeCode)).ToList();

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
        public EmployeeEntity? GetByIdAndRelation(long id, bool isDirect = false)
        {
            try
            {
                EmployeeEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetByIdAndRelation_{id}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<EmployeeEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .Include(x => x.Department)
                      .Include(x => x.JobTitle)
                      .AsQueryable()
                      /*[GEN-7]*/
                      .FirstOrDefault(entity => entity.DeletedFlag != true && entity.Id == id);
                    //result.Employee2s = result.Employee2s.Select(x => new Employee2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public List<EmployeeEntity>? GetWithdRelations(bool isDirect = false)
        {
            try
            {
                List<EmployeeEntity>? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetWithdRelations";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .Include(x => x.Department)
                      .Include(x => x.JobTitle)
                      .AsQueryable()
                      /*[GEN-7]*/
                      .Where(entity => entity.DeletedFlag != true).ToList();
                    //result.Employee2s = result.Employee2s.Select(x => new Employee2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public Task<List<EmployeeEntity>> GetCurrentEmployees()
        {
            try
            {
                var listEmp = _context.Set<EmployeeEntity>()
                  .ToListAsync();
                return listEmp;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public async Task<List<EmployeeEntity>> InsertIfNotExist(List<EmployeeEntity> entities, long? userId = null)
        {

            var rs = new List<EmployeeEntity>();
            try
            {
                if (entities != null && entities.Count() > 0)
                {
                    foreach (var entity in entities)
                    {
                        var existed = _context.Set<EmployeeEntity>().FirstOrDefault(x => x.EmployeeCode.Trim() == entity.EmployeeCode.Trim() && x.DeletedFlag != true);
                        if (existed != null)
                        {
                            rs.Add(existed);
                            continue;
                        }
                        entity.CreatedBy = userId;
                        entity.UpdatedBy = userId;
                        entity.CreatedAt = DateTime.Now;
                        entity.UpdatedAt = DateTime.Now;
                        rs.Add((await _context.Set<EmployeeEntity>().AddAsync(entity)).Entity);
                        await _context.SaveChangesAsync();
                    }
                }
                return rs;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }


        public List<FormSelectOptionModel> GetSelectData(List<long> departmentIds)
        {
            List<FormSelectOptionModel> rs = this.GetAll(departmentIds).Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
            return rs;
        }

        public List<EmployeeEntity> GetListEmployeeToExportOTTime(
                                                 DepartmentEntity departmentEntity,
                                                 ExportOTTimeRequest pagingReq,
                                                 Dictionary<string, object> paramFilter,
                                                 SearchModel paramSearch,
                                                 Dictionary<string, long> paramSort,
                                                 List<WorkingTypeEntity> listWorkingTypeOT)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListEmployeeToExportOTTime_{departmentEntity.Id}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                List<EmployeeEntity> rs = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKey, null);
                if (rs == null)
                {
                    rs = new List<EmployeeEntity>();
                    if (pagingReq == null)
                    {
                        pagingReq = new ExportOTTimeRequest();
                        pagingReq.Page = 0;
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }

                    var query = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .Include(x => x.WorkingDayEntitys)
                      .Include(p => p.Department)
                      .Include(p => p.JobTitle)
                      .Where(x => x.DeletedFlag != true)
                      .Where(x => x.DepartmentId == departmentEntity.Id)
                      .Where(x => x.WorkingDayEntitys.Any(wk => wk.WorkingDate.HasValue
                                             && wk.WorkingDate.Value >= pagingReq.DateFrom
                                             && wk.WorkingDate.Value <= pagingReq.DateTo));

                    query = query.OrderBy(x => x.Name); 
                    query = EmployeeToExportOTTimeResponseModel.PrepareWhereQueryFilter(query, paramFilter, listWorkingTypeOT);
                    query = EmployeeToExportOTTimeResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = EmployeeToExportOTTimeResponseModel.PrepareQuerySort(query, paramSort);


                    if (query == null) throw new Exception("Empty Requester Return");
                    rs = query.Take(ConstCommon.ConstExportInDayMaxRecord)
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

        public List<EmployeeEntity> GetListEmployeeDepartment(
          EmployeeAttendanceListOTRequest pagingReq,
          Dictionary<string, object> paramFilter,
          SearchModel paramSearch,
          Dictionary<string, long> paramSort)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListEmployeeDepartment";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                List<EmployeeEntity> rs = CachedFunc.GetRedisData<List<EmployeeEntity>>(cacheKey, null);
                if (rs == null)
                {
                    rs = new List<EmployeeEntity>();
                    if (pagingReq == null)
                    {
                        pagingReq = new EmployeeAttendanceListOTRequest();
                        pagingReq.Page = 0;
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }

                    var query = _context.Set<EmployeeEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .Include(entity => entity.Department)
                      .Include(entity => entity.JobTitle)
                      .Where(entity => entity.DeletedFlag != true);


                    query = query.OrderBy(x => x.Name).ThenBy(x => x.Order);
                    query = DepartmentListEmployeeResponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = DepartmentListEmployeeResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = DepartmentListEmployeeResponseModel.PrepareQuerySort(query, paramSort);

                    if (query == null) throw new Exception("Empty Requester Return");
                    rs = query.Take(ConstCommon.ConstExportInDayMaxRecord)
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
    }


}

