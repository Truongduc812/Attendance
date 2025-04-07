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
using iSoft.Common.Enums;
using NPOI.SS.Util;

namespace SourceBaseBE.Database.Repository
{
    public class DepartmentAdminRepository : BaseCRUDRepository<DepartmentAdminEntity>
    {
        public override string TableName
        {
            get { return "DepartmentAdmins"; }
        }
        public DepartmentAdminRepository(CommonDBContext dbContext)
              : base(dbContext)
        {
        }
        public override string GetName()
        {
            return nameof(DepartmentAdminRepository);
        }
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override DepartmentAdminEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                DepartmentAdminEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{userId}";
                //if (!isDirect)
                //{
                //  result = CachedFunc.GetRedisData<DepartmentAdminEntity>(cacheKey, null);
                //}

                //if (result == null)
                //{
                var dataSet = _context.Set<DepartmentAdminEntity>();
                IQueryable<DepartmentAdminEntity> queryable;
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
                    .Include(x => x.User)
                    .Include(x => x.Department)
                    .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                    .Where(x => x.User.DeletedFlag != true)
                    .FirstOrDefault();
                //result.DepartmentAdmin2s = result.DepartmentAdmin2s.Select(x => new DepartmentAdmin2Entity() { Id = x.Id, Name = x.Name }).ToList();

                //  CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
                //  CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
                //}
                return result;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        public List<DepartmentAdminEntity>? GetByUserAndRoleAndDepartment(long userId, List<EnumDepartmentAdmin> enumDepartmentAdmin, long? departmentId = 0, bool isDirect = false)
        {
            try
            {
                List<DepartmentAdminEntity>? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetByUserAndRole_{userId}_{enumDepartmentAdmin}_{departmentId}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<DepartmentAdminEntity>>(cacheKey, null);
                }

                if (result == null)
                {
                    if (departmentId > 0)
                    {
                        result = _context.Set<DepartmentAdminEntity>()
                                       //.AsNoTracking()
                                       .AsQueryable()
                                       /*[GEN-7]*/
                                       .Where(entity => entity.DeletedFlag != true && entity.UserId == userId && entity.DepartmentId == departmentId && entity.Role != null && enumDepartmentAdmin.Contains(entity.Role.Value)).ToList();
                    }
                    else
                    {
                        result = _context.Set<DepartmentAdminEntity>()
                                       //.AsNoTracking()
                                       .AsQueryable()
                                       /*[GEN-7]*/
                                       .Where(entity => entity.DeletedFlag != true && entity.UserId == userId && entity.Role != null && enumDepartmentAdmin.Contains(entity.Role.Value)).ToList();

                    }

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


        public DepartmentAdminEntity GetByUserRoleDepartment(long? userId, long? departmentId, EnumDepartmentAdmin? departmentAdmin, bool isDirect = false)
        {
            try
            {
                DepartmentAdminEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetByUserRoleDepartment_{userId}_{departmentId}_{departmentAdmin}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<DepartmentAdminEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<DepartmentAdminEntity>()
                        //.AsNoTracking()
                        .AsQueryable()
                        /*[GEN-7]*/
                        .FirstOrDefault(entity => entity.DeletedFlag != true
                        && entity.UserId == userId
                        && entity.DepartmentId == departmentId
                        && entity.Role != null
                        && entity.Role == departmentAdmin);

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

        public long GetIdByUserRoleDepartment(long? userId, long? departmentId, EnumDepartmentAdmin? departmentAdmin, bool isDirect = false)
        {
            try
            {
                long result = 0;
                string cacheKey = $"{cacheKeyDetail}:GetByUserRoleDepartment_{userId}_{departmentId}_{departmentAdmin}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<long>(cacheKey, 0);
                }

                if (result == 0)
                {
                    var data = _context.Set<DepartmentAdminEntity>()
                        //.AsNoTracking()
                        .AsQueryable()
                        /*[GEN-7]*/
                        .FirstOrDefault(entity => entity.DeletedFlag != true && entity.UserId == userId && entity.DepartmentId == departmentId && entity.Role != null && entity.Role == departmentAdmin);

                    if (data != null)
                    {
                        result = data.Id;
                    }
                    else
                    {
                        result = 0;
                    }
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

        public List<DepartmentAdminEntity>? GetListNotExited(List<long> listIds, bool isDirect = false)
        {
            try
            {
                List<DepartmentAdminEntity>? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetListNotExited{listIds}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<DepartmentAdminEntity>>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<DepartmentAdminEntity>()
                        //.AsNoTracking()
                        .AsQueryable()
                        /*[GEN-7]*/
                        .Where(entity => entity.DeletedFlag != true)
                        .Where(entity => !listIds.Contains(entity.Id))
                        .ToList();
                    //result.DepartmentAdmin2s = result.DepartmentAdmin2s.Select(x => new DepartmentAdmin2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public List<DepartmentEntity>? GetListDepartmentByAdmin(long userId, List<EnumDepartmentAdmin> enumDepartmentAdmin, bool isDirect = false)
        {
            try
            {
                List<DepartmentEntity>? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetListDepartmentByAdmin{userId}_{enumDepartmentAdmin}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<DepartmentEntity>>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<DepartmentAdminEntity>()
                        //.AsNoTracking()
                        .AsQueryable()
                        .Include(x => x.Department)
                        /*[GEN-7]*/
                        .Where(entity => entity.DeletedFlag != true
                        && entity.UserId == userId
                        && entity.Role != null && enumDepartmentAdmin.Contains(entity.Role.Value))
                        .ToList().Select(x => x.Department).ToList()
                        ;

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

        public DepartmentAdminEntity? GetById(long departmentId, long userId, bool isDirect = false)
        {
            try
            {
                DepartmentAdminEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetById_{departmentId}:{userId}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<DepartmentAdminEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<DepartmentAdminEntity>()
                        //.AsNoTracking()
                        .AsQueryable()
                        /*[GEN-7]*/
                        .Where(entity => entity.DeletedFlag != true && entity.DepartmentId == departmentId && entity.UserId == userId)
                        .FirstOrDefault();
                    //result.DepartmentAdmin2s = result.DepartmentAdmin2s.Select(x => new DepartmentAdmin2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public DepartmentAdminEntity? GetByIdAndRole(long departmentId, long userId, EnumDepartmentAdmin roleAdmin, bool isDirect = false)
        {
            try
            {
                DepartmentAdminEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}:GetByIdAndRole_{departmentId}:{userId}:{roleAdmin}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<DepartmentAdminEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<DepartmentAdminEntity>()
                      //.AsNoTracking()
                      .AsQueryable()
                      /*[GEN-7]*/
                      .Where(entity => entity.DeletedFlag != true && entity.DepartmentId != null && entity.DepartmentId == departmentId && entity.UserId != null && entity.UserId == userId
                                                && entity.Role != null && entity.Role == roleAdmin)
                      .FirstOrDefault();
                    //result.DepartmentAdmin2s = result.DepartmentAdmin2s.Select(x => new DepartmentAdmin2Entity() { Id = x.Id, Name = x.Name }).ToList();

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

        public long? GetDepartmentUser(long? userId, bool isDirect = false)
        {
            try
            {
                long? result = null;
                string cacheKey = $"{cacheKeyDetail}_GetDepartmentUser:_{userId}";

                if (userId < 1) return 0;
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<long?>(cacheKey, null);
                }
                if (result == null)
                {
                    var data = _context.Set<DepartmentAdminEntity>()
                        //.AsNoTracking()
                        .AsQueryable()
                        /*[GEN-7]*/
                        .Where(entity => entity.DeletedFlag != true)
                        .Where(entity => entity.UserId == userId)
                        .FirstOrDefault();
                    if (data != null)
                    {
                        result = data.DepartmentId;
                    }
                    else
                    {
                        result = null;
                    }
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

        public List<long?>? GetListDepartmentByUserId(long? userId, bool isDirect = false)
        {
            try
            {
                if (userId == null || userId < 1) return null;

                string cacheKey = $"{cacheKeyListByListIds}GetListDepartmentByUserId:_{userId}";
                List<long?>? result = null;

                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<long?>>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<DepartmentAdminEntity>()
                        .AsQueryable()
                        .Where(entity => entity.DeletedFlag != true && entity.UserId == userId)
                        .Select(x => (long?)x.DepartmentId)
                        .ToList();
                    result = result.Distinct().ToList();

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
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override async Task<DepartmentAdminEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                DepartmentAdminEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{userId}";
                //if (!isDirect)
                //{
                //  result = CachedFunc.GetRedisData<DepartmentAdminEntity>(cacheKey, null);
                //}

                //if (result == null)
                //{
                var dataSet = _context.Set<DepartmentAdminEntity>();
                IQueryable<DepartmentAdminEntity> queryable;
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

                //result.DepartmentAdmin2s = result.DepartmentAdmin2s.Select(x => new DepartmentAdmin2Entity() { Id = x.Id, Name = x.Name }).ToList();

                //  CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
                //  CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetDetailCacheExpiredTimeInSeconds);
                //}
                return result;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        public Task<List<DepartmentAdminEntity>> GetByDepartment(DepartmentEntity departmentEntity)
        {
            return _context
              .Set<DepartmentAdminEntity>()
              .Include(x => x.Department)
              .Include(x => x.User)
                .ThenInclude(x => x.ItemEmployee)
              .AsNoTracking()
              .Where(x => x.DepartmentId == departmentEntity.Id).ToListAsync();

        }
        public async Task<bool> CheckAdminDepartmentIsExisted(long UserId, long departmnetId, EnumDepartmentAdmin? role)
        {
            try
            {
                var data = _context
               .Set<DepartmentAdminEntity>()
               .Include(x => x.Department)
               .Include(x => x.User)
                 .ThenInclude(x => x.ItemEmployee)
               .AsNoTracking()
               .Where(x => x.DepartmentId == departmnetId && x.UserId == UserId && x.Role == role).FirstOrDefault();
                if (data == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        public List<EnumDepartmentAdmin> GetRoleOfUser(long userId, long departmentId, bool isDirect = false)
        {
            try
            {
                if (userId <= 0) return null;
                List<EnumDepartmentAdmin> result = null;
                string cacheKey = $"{cacheKeyDetail}_RoleOfUser:{userId}_{departmentId}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<EnumDepartmentAdmin>>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<DepartmentAdminEntity>()
                        .AsNoTracking()
                        .AsQueryable()
                        /*[GEN-7]*/
                        .Where(entity => entity.DeletedFlag != true && entity.UserId == userId && entity.DepartmentId == departmentId)
                        .Select(entity => entity.Role.GetValueOrDefault()).ToList();
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
        public override List<DepartmentAdminEntity> GetList(PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetList";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<DepartmentAdminEntity>? result = CachedFunc.GetRedisData<List<DepartmentAdminEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<DepartmentAdminEntity>();
                    if (pagingReq != null)
                    {
                        result = _context.Set<DepartmentAdminEntity>()
                          .AsNoTracking()
                          .AsQueryable()
                          /*[GEN-11]*/
                          .Where(entity => entity.DeletedFlag != true)
                          .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                          .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                          .AsParallel()
                          .ToList();

                        //.Select(entity => new DepartmentAdminEntity
                        // {
                        //   Id = entity.Id,
                        //   DepartmentAdmin2s = entity.DepartmentAdmin2s.Select(x => new DepartmentAdmin2Entity { Id = x.Id, Name = x.Name }).ToList()
                        // })

                        for (var i = 0; i < result.Count; i++)
                        {


                            /*[GEN-12]*/
                        }
                    }
                    else
                    {
                        result = _context.Set<DepartmentAdminEntity>()
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

        public List<DepartmentAdminEntity> GetList()
        {
            try
            {
                List<DepartmentAdminEntity> result = _context.Set<DepartmentAdminEntity>()
                  .AsNoTracking()
                  .AsQueryable()
                  /*[GEN-13]*/
                  .Where(entity => entity.DeletedFlag != true)
                  .Include(entity => entity.Department)
                  .Include(entity => entity.User)
                  .ThenInclude(emp => emp.ItemEmployee)
                  .OrderBy(entity => entity.Department)
                  .ThenBy(entity => entity.Id)
                  .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
                  .AsParallel()
                  .ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        public Task<List<DepartmentAdminEntity>> GetCurrentAdmin()
        {
            try
            {
                var listEmp = _context.Set<DepartmentAdminEntity>()
                  .Where(p => p.DeletedFlag != true)
                  .Include(p => p.User)
                  .ThenInclude(el => el.ItemEmployee)
                  .Include(p => p.Department)
                  .Where(p => p.User.DeletedFlag != true)
                  .ToListAsync();
                return listEmp;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public DepartmentAdminEntity Upsert(DepartmentAdminEntity entity/*[GEN-8]*/, long? userId = null)
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
        public DepartmentAdminEntity Insert(DepartmentAdminEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<DepartmentAdminEntity>().Add(entity);
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
        public DepartmentAdminEntity Update(DepartmentAdminEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<DepartmentAdminEntity>().Update(entity);
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

        public override IEnumerable<DepartmentAdminEntity> InsertMulti(IEnumerable<DepartmentAdminEntity> entities, long? userId = null)
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

                    }
                }
                _context.Set<DepartmentAdminEntity>().AddRange(entities);
                var result = _context.SaveChanges();
                ClearCachedData();
                return entities;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        public IEnumerable<DepartmentAdminEntity> UpSertMulti(IEnumerable<DepartmentAdminEntity> entities, long? userId = null)
        {
            try
            {
                if (entities != null && entities.Count() > 0)
                {
                    foreach (var entity in entities)
                    {
                        if (entity != null && entity.Id <= 0)
                        {
                            Insert(entity/*[GEN-4]*/, userId);
                        }
                        else
                        {
                            if (userId != null)
                            {
                                entity.UpdatedBy = userId;
                            }
                            entity.UpdatedAt = DateTime.Now;
                            _context.Set<DepartmentAdminEntity>().Update(entity);
                        }
                    }
                    var result = _context.SaveChanges();
                    ClearCachedData();
                }

                return entities;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }


        public override string GetDisplayField(DepartmentAdminEntity entity)
        {
            return entity.ToString();
        }
        public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
        {
            List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
            return rs;
        }
        public Task<List<DepartmentAdminEntity>> GetDepartmentAdmin(EnumDepartmentAdmin enumDepartmentAdmin)
        {
            return _context.Set<DepartmentAdminEntity>()
                  .Include(x => x.User)
                  .ThenInclude(x => x.ItemEmployee)
                  .Where(x => x.Role == enumDepartmentAdmin)
                  .ToListAsync();
        }
        public Task<List<DepartmentAdminEntity>> GetDepartmentAdmin(long? departmnetId, EnumDepartmentAdmin enumDepartmentAdmin)
        {
            if (departmnetId == null) return null;
            return _context.Set<DepartmentAdminEntity>()
                  .Include(x => x.User)
                  .ThenInclude(x => x.ItemEmployee)
                  .Where(x => x.Role == enumDepartmentAdmin && x.DepartmentId == departmnetId)
                  .ToListAsync();
        }
        public async Task<bool> RemoveIfNotExistInList(List<DepartmentAdminEntity> employeeEntities)
        {
            try
            {
                var listEmp = _context.Set<DepartmentAdminEntity>()
                  .Where(p => /*p.DeletedFlag != true &&*/ !employeeEntities.Contains(p))
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

    }
}
