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
using iSoft.Common.Models.ConfigModel.Subs;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;
using iSoft.Common.ExtensionMethods;
using iSoft.Common.Utils;

namespace SourceBaseBE.Database.Repository
{
    public class WorkingTypeRepository : BaseCRUDRepository<WorkingTypeEntity>
    {
        public WorkingTypeRepository(CommonDBContext dbContext)
          : base(dbContext)
        {
        }
        public override string GetName()
        {
            return nameof(Example001Repository);
        }
        /// <summary>
        /// GetById (@GenCRUD)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public override WorkingTypeEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                WorkingTypeEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //	result = CachedFunc.GetRedisData<WorkingTypeEntity>(cacheKey, null);
                //}

                //if (result == null)
                //    {
                var dataSet = _context.Set<WorkingTypeEntity>();
                IQueryable<WorkingTypeEntity> queryable;
                if (!isTracking)
                {
                    queryable = dataSet.AsNoTracking().AsQueryable();
                }
                else
                {
                    queryable = dataSet.AsQueryable();
                }
                result = queryable
                    .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                                .FirstOrDefault();
                //result.Example0012s = result.Example0012s.Select(x => new Example0012Entity() { Id = x.Id, Name = x.Name }).ToList();

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

        public WorkingTypeEntity? GetBySymbol(string symbol, bool isDirect = false)
        {
            try
            {
                WorkingTypeEntity? result = null;
                result = _context.Set<WorkingTypeEntity>()
                    .AsQueryable()
                    .Where(entity => entity.DeletedFlag != true && entity.Code.ToLower() == symbol.ToLower())
                    .FirstOrDefault();
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
        public override List<WorkingTypeEntity> GetList(PagingRequestModel pagingReq = null)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_GetList";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}_GetList:{pagingReq.Page}|{pagingReq.PageSize}";
                }
                List<WorkingTypeEntity>? result = CachedFunc.GetRedisData<List<WorkingTypeEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<WorkingTypeEntity>();
                    if (pagingReq != null)
                    {
                        result = _context.Set<WorkingTypeEntity>()
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
                        result = _context.Set<WorkingTypeEntity>()
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

        public List<WorkingTypeEntity> GetList()
        {
            try
            {
                List<WorkingTypeEntity> result = new List<WorkingTypeEntity>();

                result = _context.Set<WorkingTypeEntity>()
                    .AsNoTracking()
                    .AsQueryable()
                    .Where(entity => entity.DeletedFlag != true)
                    .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
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
        public WorkingTypeEntity Upsert(WorkingTypeEntity entity, List<GenTemplateEntity> genTemplateChildren/*[GEN-8]*/, long? userId = null)
        {
            try
            {
                if (entity.Id <= 0)
                {
                    // Insert
                    entity = Insert(entity, genTemplateChildren/*[GEN-4]*/, userId);
                }
                else
                {
                    // Update
                    entity = Update(entity, genTemplateChildren/*[GEN-4]*/, userId);
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public WorkingTypeEntity Insert(WorkingTypeEntity entity, List<GenTemplateEntity> genTemplateChildren/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<WorkingTypeEntity>().Add(entity);
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
        public WorkingTypeEntity Update(WorkingTypeEntity entity, List<GenTemplateEntity> genTemplateChildren/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<WorkingTypeEntity>().Update(entity);
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

        public override IEnumerable<WorkingTypeEntity> InsertMulti(IEnumerable<WorkingTypeEntity> entities, long? userId = null)
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
                _context.Set<WorkingTypeEntity>().AddRange(entities);
                var result = _context.SaveChanges();
                ClearCachedData();
                return entities;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        public IEnumerable<WorkingTypeEntity> UpSertMulti(IEnumerable<WorkingTypeEntity> entities, long? userId = null)
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
                            _context.Set<WorkingTypeEntity>().Update(entity);
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

        public override string GetDisplayField(WorkingTypeEntity entity)
        {
            return entity.Name.ToString();
        }
        public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
        {
            List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
            return rs;
        }

        public WorkingTypePagingResponseModel GetListSymbol(
         PagingFilterRequestModel pagingReq,
         Dictionary<string, object> paramFilter,
         SearchModel paramSearch,
         Dictionary<string, long> paramSort)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_Attendance_Setting";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKey}:{pagingReq.GetKeyCache()}";
                }
                cacheKey += $"{paramFilter.ToJson()}";
                cacheKey += $"{paramSearch.ToJson()}";
                cacheKey += $"{paramSort.ToJson()}";
                cacheKey = EncodeUtil.MD5(cacheKey);

                WorkingTypePagingResponseModel rs = CachedFunc.GetRedisData<WorkingTypePagingResponseModel>(cacheKey, null);
                if (rs == null)
                {
                    rs = new WorkingTypePagingResponseModel();
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
                    var query = _context.Set<WorkingTypeEntity>()
              .AsNoTracking()
              .AsQueryable();

                    //* where
                    query = query.Where(p => p.DeletedFlag != true).OrderBy(x => x.Id);
                    query = WorkingTypeListReponseModel.PrepareWhereQueryFilter(query, paramFilter);
                    query = WorkingTypeListReponseModel.PrepareWhereQuerySearch(query, paramSearch);
                    query = WorkingTypeListReponseModel.PrepareQuerySort(query, paramSort);

                    if (query == null) throw new Exception("Empty symbol Return");

                    //rs.ListData = query.AsParallel().Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                    //  .Select(x => new WorkingTypeListReponseModel().SetData(x))
                    //  .ToList();

                    rs.ListData = query.Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                      .Select(x => new WorkingTypeListReponseModel().SetData(x))
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

        public async Task<bool> RemoveIfNotExistInList(List<WorkingTypeEntity> employeeEntities)
        {
            try
            {
                var listEmp = _context.Set<WorkingTypeEntity>()
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
        public async Task<List<WorkingTypeEntity>> GetOTTypes()
        {
            try
            {
                var listRet = await _context.Set<WorkingTypeEntity>()
                  .Where(p => p.DeletedFlag != true)
                  .AsNoTracking()
                  .Where(x => x.OT_150 > 0
                  || x.OT_200 > 0
                  || x.Holiday_OT_300 > 0
                  || x.Holiday_OT_Night_390 > 0
                  || x.Weekend_Night_OT_270 > 0
                  || x.Weekend_OT_200 > 0
                  )
                  .Where(x => x.Code != "C3")
                  .ToListAsync();
                return listRet;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
    }
}
