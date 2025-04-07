using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Common.Exceptions;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Models.RequestModels;
using iSoft.Common.Utils;
using iSoft.Database.Models;
using iSoft.Redis.Services;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.EntityFrameworkCore;
using SourceBaseBE.Database.DBContexts;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;
using UserEntity = SourceBaseBE.Database.Entities.UserEntity;

namespace SourceBaseBE.Database.Repository
{
    public class DepartmentRepository : BaseCRUDRepository<DepartmentEntity>
    {
        public override string TableName
        {
            get { return "Departments"; }
        }
        public DepartmentRepository(CommonDBContext dbContext)
          : base(dbContext)
        {
        }
        public List<DepartmentEntity> GetDepartmentsByListIds(List<long?> listDepartmentId, bool isDirect = false)
        {
            try
            {
                List<DepartmentEntity>? result = null;
                string cacheKey = $"{cacheKeyListByListIds}_listDepartmentId:{(listDepartmentId == null ? "" : string.Join(",", listDepartmentId))}";
                cacheKey = EncodeUtil.MD5(cacheKey);
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<List<DepartmentEntity>>(cacheKey, null);
                }

                if (result == null)
                {
                    if (listDepartmentId == null || listDepartmentId.Count <= 0)
                    {
                        result = _context.Set<DepartmentEntity>().
                        Include(x => x.DepartmentAdmins)
                        .ThenInclude(x => x.User)
                        .ThenInclude(x => x.ItemEmployee)
                        .Where(x => x.DeletedFlag != true)
                        .ToList();
                    }
                    else
                    {
                        result = _context.Set<DepartmentEntity>().
                          Include(x => x.DepartmentAdmins)
                          .ThenInclude(x => x.User)
                          .ThenInclude(x => x.ItemEmployee)
                          .Where(x => x.DeletedFlag != true)
                          .Where(x => listDepartmentId.Contains(x.Id))
                          .ToList();
                    }

                    CachedFunc.AddEntityCacheKey(GetName(), cacheKey, true);
                    CachedFunc.SetRedisData(cacheKey, result, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
                }
                return result;
            }
            catch (Exception dbEx)
            {
                throw new DBException(dbEx);
            }
        }
        public List<DepartmentEntity> GetListDepartment(bool isDirect = false)
        {
            try
            {
                List<DepartmentEntity>? result = null;

                result = _context.Set<DepartmentEntity>()
                  .Where(x => x.DeletedFlag == false)
                  .Include(x => x.DepartmentAdmins)
                    .ThenInclude(x => x.User)
                    .ThenInclude(x => x.ItemEmployee)
                  .ToList();
                return result;
            }
            catch (Exception dbEx)
            {
                throw new DBException(dbEx);
            }
        }
        public override string GetName()
        {
            return nameof(DepartmentRepository);
        }
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override DepartmentEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                DepartmentEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //  result = CachedFunc.GetRedisData<DepartmentEntity>(cacheKey, null);
                //}

                //if (result == null)
                //{
                var dataSet = _context.Set<DepartmentEntity>();
                IQueryable<DepartmentEntity> queryable;
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
                //result.Department2s = result.Department2s.Select(x => new Department2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override async Task<DepartmentEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                DepartmentEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //  result = CachedFunc.GetRedisData<DepartmentEntity>(cacheKey, null);
                //}

                //if (result == null)
                //{
                result = await (isTracking ?
                _context.Set<DepartmentEntity>()
                  //.AsNoTracking()
                  .AsQueryable()
                  /*[GEN-7]*/
                  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                  .FirstOrDefaultAsync() :
                  _context.Set<DepartmentEntity>()
                  .AsNoTracking()
                  .AsQueryable()
                  /*[GEN-7]*/
                  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                  .FirstOrDefaultAsync());
                //result.Department2s = result.Department2s.Select(x => new Department2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        /// <summary>
        /// GetList (@GenCRUD)
        /// </summary>
        /// <param name="pagingReq"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override List<DepartmentEntity> GetList(PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetList";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<DepartmentEntity>? result = CachedFunc.GetRedisData<List<DepartmentEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<DepartmentEntity>();
                    if (pagingReq != null && pagingReq.PageSize > 0)
                    {
                        result = _context.Set<DepartmentEntity>()
                          .AsNoTracking()
                          .AsQueryable()
                          /*[GEN-11]*/
                          .Where(entity => entity.DeletedFlag != true)
                          .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                          .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                          .AsParallel()
                          .ToList();

                        //.Select(entity => new DepartmentEntity
                        // {
                        //   Id = entity.Id,
                        //   Department2s = entity.Department2s.Select(x => new Department2Entity { Id = x.Id, Name = x.Name }).ToList()
                        // })

                        for (var i = 0; i < result.Count; i++)
                        {


                            /*[GEN-12]*/
                        }
                    }
                    else
                    {
                        result = _context.Set<DepartmentEntity>()
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

        public List<DepartmentEntity> GetDepartmentByListIds(List<long?> ips, PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetDepartmentByListIds_{ips}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<DepartmentEntity>? result = CachedFunc.GetRedisData<List<DepartmentEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<DepartmentEntity>();
                    if (pagingReq != null && pagingReq.PageSize > 0)
                    {
                        result = _context.Set<DepartmentEntity>()
                          .AsNoTracking()
                          .AsQueryable()
                          /*[GEN-11]*/
                          .Where(entity => entity.DeletedFlag != true)
                          .Where(entity => ips.Contains(entity.Id))
                          .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                          .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                          .AsParallel()
                          .ToList();

                        //.Select(entity => new DepartmentEntity
                        // {
                        //   Id = entity.Id,
                        //   Department2s = entity.Department2s.Select(x => new Department2Entity { Id = x.Id, Name = x.Name }).ToList()
                        // })

                        for (var i = 0; i < result.Count; i++)
                        {


                            /*[GEN-12]*/
                        }
                    }
                    else
                    {
                        result = _context.Set<DepartmentEntity>()
                          .AsNoTracking()
                          .AsQueryable()
                          /*[GEN-13]*/
                          .Where(entity => entity.DeletedFlag != true)
                          .Where(entity => ips.Contains(entity.Id))
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


        public DepartmentEntity Upsert(DepartmentEntity entity/*[GEN-8]*/, long? userId = null)
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
        public DepartmentEntity Insert(DepartmentEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<DepartmentEntity>().Add(entity);
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
        public DepartmentEntity Update(DepartmentEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<DepartmentEntity>().Update(entity);
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
        public override string GetDisplayField(DepartmentEntity entity)
        {
            return entity.Name.ToString();
        }
        public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
        {
            List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
            return rs;
        }

        public override IEnumerable<DepartmentEntity> InsertMulti(IEnumerable<DepartmentEntity> entities, long? userId = null)
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
                _context.Set<DepartmentEntity>().AddRange(entities);
                var result = _context.SaveChanges();
                ClearCachedData();
                return entities;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        //public IEnumerable<DepartmentEntity> UpSertMulti(IEnumerable<DepartmentEntity> entities, long? userId = null)
        //{
        //	try
        //	{
        //		if (entities != null && entities.Count() > 0)
        //		{
        //			foreach (var entity in entities)
        //			{
        //				if (entity != null && entity.Id <= 0)
        //				{
        //					Insert(entity/*[GEN-4]*/, userId);
        //				}
        //				else
        //				{
        //					if (userId != null)
        //					{
        //						entity.UpdatedBy = userId;
        //					}
        //					entity.UpdatedAt = DateTime.Now;
        //					_context.Set<DepartmentEntity>().Update(entity);
        //				}
        //			}
        //			var result = _context.SaveChanges();
        //			ClearCachedData();
        //		}

        //		return entities;
        //	}
        //	catch (Exception ex)
        //	{
        //		throw new DBException(ex);
        //	}
        //}



        public DepartmentEntity? GetByName(string name, bool isDirect = false)
        {
            try
            {
                if (name == null || name == "") return null;
                DepartmentEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}_GetByName:{name}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<DepartmentEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<DepartmentEntity>()
                      .AsQueryable()
                      .AsNoTracking()
                      .FirstOrDefault(entity => entity.DeletedFlag != true && entity.Name != null && entity.Name.ToLower() == name.ToLower());

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

        public AdminDepartmentPagingResponseModel GetListAdminDepartment(
          EnumDepartmentAdmin? AdminDepartment,
        PagingParamRequestModel pagingReq,
        Dictionary<string, object> paramFilter,
        SearchModel paramSearch,
        Dictionary<string, long> paramSort)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListAdminDepartment{AdminDepartment}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}_{AdminDepartment}_{pagingReq.DepartmentId}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);
                AdminDepartmentPagingResponseModel rs = CachedFunc.GetRedisData<AdminDepartmentPagingResponseModel>(cacheKey, null);
                if (rs == null)
                {
                    rs = new AdminDepartmentPagingResponseModel();
                    if (pagingReq == null)
                    {
                        pagingReq = new PagingParamRequestModel();
                        pagingReq.Page = 0;
                        pagingReq.PageSize = ConstCommon.ConstSelectListMaxRecord;
                    }

                    var query = _context.Set<DepartmentAdminEntity>()
                      .AsNoTracking()
                      .AsQueryable();

                    //* where
                    if (pagingReq.DepartmentId != null)
                    {
                        query = query.Where(p => p.DeletedFlag != true)
                           .Where(p => p.DepartmentId == pagingReq.DepartmentId)
                           .Where(p => p.Role == AdminDepartment)
                           .Include(p => p.Department)
                           .Include(p => p.User).ThenInclude(p => p.ItemEmployee)
                           .Where(p => p.User != null && p.User.DeletedFlag != true);
                        //.AsQueryable();
                    }
                    else
                    {
                        query = query.Where(p => p.DeletedFlag != true)
                           .Where(p => p.Role == AdminDepartment)
                           .Include(p => p.Department)
                           .Include(p => p.User).ThenInclude(p => p.ItemEmployee)
                           .Where(p => p.User != null && p.User.DeletedFlag != true);
                        //.AsQueryable();
                    }
                    query = query.OrderBy(x => x.Id);
                    query = DepartmentAdminListResponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = DepartmentAdminListResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = DepartmentAdminListResponseModel.PrepareQuerySort(query, paramSort);
                    if (AdminDepartment != null)
                        query = query.Where(x => x.Role == AdminDepartment);
                    if (query == null) throw new Exception("Empty Employee Return");
                    rs.ListData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                      .Select(x => new DepartmentAdminListResponseModel().SetData(x))
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

        public UserPagingResponseModel GetListUserAdmin(
        EnumDepartmentAdmin? AdminDepartment,
        PagingParamRequestModel pagingReq,
        Dictionary<string, object> paramFilter,
        SearchModel paramSearch,
        Dictionary<string, long> paramSort,
        List<DepartmentAdminEntity> listAdmin,
        List<DepartmentEntity> listDepartment )
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetListUserAdmin{AdminDepartment}_{listAdmin}_{listDepartment}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}_{AdminDepartment}_{pagingReq.DepartmentId}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);
                UserPagingResponseModel rs = CachedFunc.GetRedisData<UserPagingResponseModel>(cacheKey, null);
                if (rs == null)
                {
                    rs = new UserPagingResponseModel();
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

                    query = query

                        .Include(p => p.DepartmentAdmins.Where(da => da.DeletedFlag != true))
                        .Where(p => p.DeletedFlag != true && p.DepartmentAdmins.Any(da => da.DeletedFlag != true))
                        .Include(p => p.ItemEmployee);

                    query = UserAdminListResponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = UserAdminListResponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = UserAdminListResponseModel.PrepareQuerySort(query, paramSort);

                    if (query == null)
                    {
                        throw new Exception("Empty Employee Return");
                    }

                    rs.ListData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                      .Select(x => new UserAdminListResponseModel().SetData(x, listAdmin,listDepartment))
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

        public async Task<List<DepartmentEntity>> InsertIfNotExist(List<DepartmentEntity> entities, long? userId = null)
        {

            var rs = new List<DepartmentEntity>();
            try
            {
                if (entities != null && entities.Count() > 0)
                {
                    foreach (var entity in entities)
                    {
                        var existed = _context.Set<DepartmentEntity>().FirstOrDefault(x => x.Name.Trim() == entity.Name.Trim() && x.DeletedFlag != true);
                        if (existed != null)
                        {
                            rs.Add(existed);
                            continue;
                        }
                        entity.CreatedBy = userId;
                        entity.UpdatedBy = userId;
                        entity.CreatedAt = DateTime.Now;
                        entity.UpdatedAt = DateTime.Now;
                        rs.Add((await _context.Set<DepartmentEntity>().AddAsync(entity)).Entity);
                    }
                    await _context.SaveChangesAsync();

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
