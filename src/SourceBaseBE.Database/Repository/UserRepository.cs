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
using iSoft.Database.Entities;
using UserEntity = SourceBaseBE.Database.Entities.UserEntity;
using ISoftProjectEntity = SourceBaseBE.Database.Entities.ISoftProjectEntity;
using iSoft.Common.Models.ConfigModel.Subs;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;
using iSoft.Common.Enums;
using static iSoft.Common.ConstCommon;
using NPOI.POIFS.FileSystem;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Utils;
using System.Linq;

namespace SourceBaseBE.Database.Repository
{
    public class UserRepository : BaseCRUDRepository<UserEntity>
    {
        public UserRepository(CommonDBContext dbContext)
          : base(dbContext)
        {
        }
        public override string GetName()
        {
            return nameof(UserRepository);
        }
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override UserEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                UserEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}_GetById:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<UserEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<UserEntity>();
                IQueryable<UserEntity> queryable;
                if (!isTracking)
                {
                    queryable = dataSet.AsNoTracking().AsQueryable();
                }
                else
                {
                    queryable = dataSet.AsQueryable();
                }
                result = queryable
                  .Include(entity => entity.ListISoftProject)
                              .Include(entity => entity.ListAuthGroup)
                              .Include(entity => entity.ListAuthPermission)/*[GEN-7]*/
                              .Include(entity => entity.ItemEmployee)
                              .Include(entity => entity.DepartmentAdmins)
                              .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                              .FirstOrDefault();
                //result.User2s = result.User2s.Select(x => new User2Entity() { Id = x.Id, Name = x.Name }).ToList();

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

        public UserEntity? GetByIdEmployee(string employeeCode, bool isDirect = false)
        {
            try
            {
                UserEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}_GetByIdEmployee:{employeeCode}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<UserEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<UserEntity>()
                        //.AsNoTracking()
                        .AsQueryable()
                        .Include(entity => entity.ListISoftProject)
                        .Include(entity => entity.ListAuthGroup)
                        .Include(entity => entity.ListAuthPermission)/*[GEN-7]*/
                        .Include(entity => entity.ItemEmployee)
                        .Where(entity => entity.DeletedFlag != true && entity.ItemEmployee.EmployeeCode == employeeCode)
                        .FirstOrDefault();
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


        public UserModel? GetProfileById(long id, bool isDirect = false)
        {
            if (id <= 0) // user root
            {
                return new UserModel()
                {
                    Displayname = "Root",
                    EmployeeId = 0,
                    Roles = new List<EnumDepartmentAdmin?>()
                    {
                            EnumDepartmentAdmin.Admin1, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3
                    }
                };
            }
            try
            {
                UserModel? result = null;
                string cacheKey = $"{cacheKeyDetail}_GetProfileById:{id}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<UserModel>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<UserEntity>()
                      //.AsNoTracking()
                      .AsQueryable()
                      .Include(entity => entity.ItemEmployee)
                      .Include(entity => entity.DepartmentAdmins)
                      .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                      .Select(u => new UserModel
                      {
                          Id = u.Id,
                          EmployeeId = u.EmployeeId,
                          Username = u.Username,
                          Role = u.Role,
                          Address = u.Address,
                          CompanyName = u.CompanyName,
                          Birthday = u.Birthday.Value.ToString(ConstDateTimeFormat.DDMMYYYY),
                          Email = u.Email,
                          FirstName = u.FirstName,
                          MiddleName = u.MiddleName,
                          LastName = u.LastName,
                          Gender = u.Gender,
                          PhoneNumber = u.PhoneNumber,
                          Displayname = u.Displayname,
                          Avatar = u.Avatar,
                          Roles = u.DepartmentAdmins.Select(role => role.Role).Distinct().ToList()
                      })
                      .FirstOrDefault();
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


        public long? GetEmployeeByUserId(long? userId, bool isDirect = false)
        {
            try
            {
                if (userId == null)
                {
                    return null;
                }
                UserEntity? result = null;
                string cacheKey = $"GetEmployeeByUserId_{cacheKeyDetail}:{userId}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<UserEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<UserEntity>()
                        //.AsNoTracking()
                        .AsQueryable()
                        /*[GEN-7]*/
                        .FirstOrDefault(entity => entity.DeletedFlag != true && entity.Id == userId);
                    //result.DepartmentAdmin2s = result.DepartmentAdmin2s.Select(x => new DepartmentAdmin2Entity() { Id = x.Id, Name = x.Name }).ToList();

                    CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
                    CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
                }
                return result?.EmployeeId;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        public long? GetUserByName(string name, bool isDirect = false)
        {
            if (string.IsNullOrEmpty(name)) return null;
            try
            {
                long? result = null;
                string cacheKey = $"{cacheKeyDetail}_GetUserByName:{name}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<long>(cacheKey, 0);
                }

                if (result <= 0)
                {
                    var data = _context.Set<UserEntity>()
                     .AsQueryable()
                     .AsNoTracking()
                     .FirstOrDefault(entity => entity.DeletedFlag != true
                     && entity.Username == name);

                    result = data?.Id;
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


        /// <summary>
        /// GetList (@GenCRUD)
        /// </summary>
        /// <param name="pagingReq"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override List<UserEntity> GetList(PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetList";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}_GetList:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<UserEntity>? result = CachedFunc.GetRedisData<List<UserEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<UserEntity>();
                    if (pagingReq != null)
                    {
                        result = _context.Set<UserEntity>()
                          .AsNoTracking()
                          .AsQueryable()
                          .Where(entity => entity.DeletedFlag != true)
                          .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                          .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                          .AsParallel()
                          .ToList();

                    }
                    else
                    {
                        result = _context.Set<UserEntity>()
                          .AsNoTracking()
                          .AsQueryable()
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

        public List<UserEntity> GetListByAdminRole(EnumDepartmentAdmin adminRole)
        {
            try
            {
                string cacheKey = $"";
                cacheKey = $"{cacheKeyList}_GetListByAdminRole:{adminRole}";
                List<UserEntity>? result = CachedFunc.GetRedisData<List<UserEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = _context.Set<UserEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .Include(entity => entity.ItemEmployee)
                      .Include(entity => entity.DepartmentAdmins)
                      .Where(entity => entity.DepartmentAdmins.Any(item => item.Role == adminRole))
                      .Where(entity => entity.DeletedFlag != true)
                      .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
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

        public List<UserEntity> GetListByAdminRoleAndDepartment(EnumDepartmentAdmin adminRole, List<long?> departmentIds)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListByAdminRole:{adminRole}_Departments:{string.Join(",", departmentIds)}";
                List<UserEntity>? result = CachedFunc.GetRedisData<List<UserEntity>>(cacheKey, null);

                if (result == null)
                {
                    result = _context.Set<UserEntity>()
                        .AsNoTracking()
                        .AsQueryable()
                        .Include(entity => entity.ItemEmployee)
                        .Include(entity => entity.DepartmentAdmins)
                        .Where(entity => entity.DepartmentAdmins.Any(item => item.Role == adminRole && item.DeletedFlag != true))
                        .Where(entity => entity.DepartmentAdmins.Any(item => departmentIds.Contains(item.DepartmentId) && item.DeletedFlag != true)) 
                        .Where(entity => entity.DeletedFlag != true)
                        .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
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

        public List<UserEntity> GetListByListAdminRole(List<EnumDepartmentAdmin?> adminRoles)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListByListAdminRole:{string.Join(",", adminRoles)}";
                List<UserEntity>? result = CachedFunc.GetRedisData<List<UserEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = _context.Set<UserEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .Include(entity => entity.ItemEmployee)
                      .Include(entity => entity.DepartmentAdmins)
                      .Where(entity => entity.DepartmentAdmins.Any(item => adminRoles.Contains(item.Role)))
                      .Where(entity => entity.DeletedFlag != true)
                      .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
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
        public UserEntity Upsert(UserEntity entity, List<ISoftProjectEntity> iSoftProjectChildren, List<AuthGroupEntity> authGroupChildren, List<AuthPermissionEntity> authPermissionChildren/*[GEN-8]*/, long? userId = null)
        {
            try
            {
                if (entity.Id <= 0)
                {
                    // Insert
                    entity = Insert(entity, iSoftProjectChildren, authGroupChildren, authPermissionChildren/*[GEN-4]*/, userId);
                }
                else
                {
                    // Update
                    entity = Update(entity, iSoftProjectChildren, authGroupChildren, authPermissionChildren/*[GEN-4]*/, userId);
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public UserEntity Insert(UserEntity entity, List<ISoftProjectEntity> iSoftProjectChildren, List<AuthGroupEntity> authGroupChildren, List<AuthPermissionEntity> authPermissionChildren/*[GEN-8]*/, long? userId = null)
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
                    entity.ListISoftProject = MergeChildrenEntity(entity.ListISoftProject, iSoftProjectChildren);

                    entity.ListAuthGroup = MergeChildrenEntity(entity.ListAuthGroup, authGroupChildren);

                    entity.ListAuthPermission = MergeChildrenEntity(entity.ListAuthPermission, authPermissionChildren);
                    /*[GEN-10]*/
                    _context.Set<UserEntity>().Add(entity);
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
        public UserEntity Update(UserEntity entity, List<ISoftProjectEntity> iSoftProjectChildren, List<AuthGroupEntity> authGroupChildren, List<AuthPermissionEntity> authPermissionChildren/*[GEN-8]*/, long? userId = null)
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
                    entity.ListISoftProject = MergeChildrenEntity(entity.ListISoftProject, iSoftProjectChildren);

                    entity.ListAuthGroup = MergeChildrenEntity(entity.ListAuthGroup, authGroupChildren);

                    entity.ListAuthPermission = MergeChildrenEntity(entity.ListAuthPermission, authPermissionChildren);
                    /*[GEN-9]*/
                    _context.Set<UserEntity>().Update(entity);
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
        public override string GetDisplayField(UserEntity entity)
        {
            return entity.Username.ToString();
        }
        public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
        {
            List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
            return rs;
        }
        public BaseCRUDEntity FillTrackingUser(BaseCRUDEntity entity)
        {
            if (entity == null)
            {
                return entity;
            }
            string createdUserName = "";
            string updatedUserName = "";
            List<UserEntity> listUser = this.GetListWithNoInclude();
            Dictionary<long, UserEntity> dicEntity = listUser.ToDictionary(x => x.Id);
            if (entity.CreatedBy != null && dicEntity.ContainsKey(entity.CreatedBy.Value))
            {
                createdUserName = dicEntity[entity.CreatedBy.Value].Username;
            }
            if (entity.UpdatedBy != null && dicEntity.ContainsKey(entity.UpdatedBy.Value))
            {
                updatedUserName = dicEntity[entity.UpdatedBy.Value].Username;
            }
            entity.CreatedUsername = createdUserName;
            entity.UpdatedUsername = updatedUserName;
            return entity;
        }
        public List<BaseCRUDEntity> FillTrackingUser(List<BaseCRUDEntity> listEntity)
        {
            if (listEntity == null || listEntity.Count <= 0)
            {
                return listEntity;
            }
            string createdUserName = "";
            string updatedUserName = "";
            List<UserEntity> listUser = this.GetListWithNoInclude();
            Dictionary<long, UserEntity> dicEntity = listUser.ToDictionary(x => x.Id);
            for (int i = 0; i < listEntity.Count; i++)
            {
                var entity = listEntity[i];
                createdUserName = "";
                updatedUserName = "";
                if (entity.CreatedBy != null && dicEntity.ContainsKey(entity.CreatedBy.Value))
                {
                    createdUserName = dicEntity[entity.CreatedBy.Value].Username;
                }
                if (entity.UpdatedBy != null && dicEntity.ContainsKey(entity.UpdatedBy.Value))
                {
                    updatedUserName = dicEntity[entity.UpdatedBy.Value].Username;
                }
                listEntity[i].CreatedUsername = createdUserName;
                listEntity[i].UpdatedUsername = updatedUserName;
            }
            return listEntity;
        }

        public Task<UserEntity> GetUserByUserName(string username)
        {
            return _context.Set<UserEntity>().FirstOrDefaultAsync(x => x.Username == username && x.DeletedFlag != true);
        }

        public UserDepartmentPagingResponseModel GetListUserDepartment( 
        PagingParamRequestModel pagingReq,
        Dictionary<string, object> paramFilter,
        SearchModel paramSearch,
        Dictionary<string, long> paramSort)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListAdminDepartment ";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}_{pagingReq.DepartmentId}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);
                UserDepartmentPagingResponseModel rs = CachedFunc.GetRedisData<UserDepartmentPagingResponseModel>(cacheKey, null);
                if (rs == null)
                {
                    rs = new UserDepartmentPagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new PagingParamRequestModel();
                        pagingReq.Page = 0;
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }

                    var query = _context.Set<UserEntity>()
                      .AsNoTracking()
                      .AsQueryable();

                    //* where
                    if (pagingReq.DepartmentId != null)
                    {
                        query = query.Where(p => p.DeletedFlag != true)
                           .Include(p => p.ItemEmployee)
                           .ThenInclude(p => p.Department)
                           .Include(p => p.ItemEmployee)
                           .ThenInclude(p => p.JobTitle)
                           .Include(p => p.DepartmentAdmins)
                           .ThenInclude(p => p.Department)
                           .Where(p => p.ItemEmployee.DepartmentId == pagingReq.DepartmentId);
                        //.AsQueryable();
                    }
                    else
                    {
                        query = query.Where(p => p.DeletedFlag != true)
                           .Include(p => p.ItemEmployee)
                           .ThenInclude(p => p.Department)
                           .Include(p => p.ItemEmployee)
                           .ThenInclude(p => p.JobTitle)
                           .Include(p => p.DepartmentAdmins)
                           .ThenInclude(p => p.Department);  
                        //.AsQueryable();
                    }
                    query = query.OrderBy(x => x.Id);
                    query = UserDepartmentResponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = UserDepartmentResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = UserDepartmentResponseModel.PrepareQuerySort(query, paramSort);

                    rs.ListData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                      .Select(x => new UserDepartmentResponseModel().SetData(x))
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
    }
}
