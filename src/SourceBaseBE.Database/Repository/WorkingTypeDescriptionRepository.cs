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
    public class WorkingTypeDescriptionRepository : BaseCRUDRepository<WorkingTypeDescriptionEntity>
    {
        public override string TableName
        {
            get { return "WorkingTypeDescriptions"; }
        }
        public WorkingTypeDescriptionRepository(CommonDBContext dbContext)
          : base(dbContext)
        {
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
        public override WorkingTypeDescriptionEntity? GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                WorkingTypeDescriptionEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //  result = CachedFunc.GetRedisData<DepartmentEntity>(cacheKey, null);
                //}

                //if (result == null)
                //{
                var dataSet = _context.Set<WorkingTypeDescriptionEntity>();
                IQueryable<WorkingTypeDescriptionEntity> queryable;
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
        public override async Task<WorkingTypeDescriptionEntity>? GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            try
            {
                WorkingTypeDescriptionEntity? result = null;
                //string cacheKey = $"{cacheKeyDetail}:{id}";
                //if (!isDirect)
                //{
                //  result = CachedFunc.GetRedisData<DepartmentEntity>(cacheKey, null);
                //}

                //if (result == null)
                //{
                result = await (isTracking ?
                _context.Set<WorkingTypeDescriptionEntity>()
                  //.AsNoTracking()
                  .AsQueryable()
                  /*[GEN-7]*/
                  .Where(entity => entity.DeletedFlag != true && entity.Id == id)
                  .FirstOrDefaultAsync() :
                  _context.Set<WorkingTypeDescriptionEntity>()
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

        public WorkingTypeDescriptionEntity Upsert(WorkingTypeDescriptionEntity entity/*[GEN-8]*/, long? userId = null)
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
        public WorkingTypeDescriptionEntity Insert(WorkingTypeDescriptionEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<WorkingTypeDescriptionEntity>().Add(entity);
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
        public WorkingTypeDescriptionEntity Update(WorkingTypeDescriptionEntity entity/*[GEN-8]*/, long? userId = null)
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
                    _context.Set<WorkingTypeDescriptionEntity>().Update(entity);
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
        public override string GetDisplayField(WorkingTypeDescriptionEntity entity)
        {
            return entity.Name.ToString();
        }
        public override List<FormSelectOptionModel> GetSelectData(string entityName, string category)
        {
            List<FormSelectOptionModel> rs = GetListWithNoInclude().Select(x => new FormSelectOptionModel(x.Id, GetDisplayField(x))).ToList();
            return rs;
        }

        public override IEnumerable<WorkingTypeDescriptionEntity> InsertMulti(IEnumerable<WorkingTypeDescriptionEntity> entities, long? userId = null)
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
                _context.Set<WorkingTypeDescriptionEntity>().AddRange(entities);
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



        public WorkingTypeDescriptionEntity? GetByName(string name, bool isDirect = false)
        {
            try
            {
                if (name == null || name == "") return null;
                WorkingTypeDescriptionEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}_GetByName:{name}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<WorkingTypeDescriptionEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<WorkingTypeDescriptionEntity>()
                      .AsQueryable()
                      .AsNoTracking()
                      .FirstOrDefault(entity => entity.DeletedFlag != true && entity.Name != null && entity.Name.ToUpper() == name.ToUpper());

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

        public WorkingTypeDescriptionEntity? GetByWorkingTypeId(long workingTypeId, bool isDirect = false)
        {
            try
            {
                WorkingTypeDescriptionEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}_GetByWorkingTypeId:{workingTypeId}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<WorkingTypeDescriptionEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<WorkingTypeDescriptionEntity>()
                      .AsQueryable()
                      .AsNoTracking()
                      .FirstOrDefault(entity => entity.DeletedFlag != true && entity.WorkingTypeId == workingTypeId);

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


        public async Task<List<WorkingTypeDescriptionEntity>> InsertIfNotExist(List<WorkingTypeDescriptionEntity> entities, long? userId = null)
        {

            var rs = new List<WorkingTypeDescriptionEntity>();
            try
            {
                if (entities != null && entities.Count() > 0)
                {
                    foreach (var entity in entities)
                    {
                        var existed = _context.Set<WorkingTypeDescriptionEntity>().FirstOrDefault(x => x.Name.Trim() == entity.Name.Trim() && x.DeletedFlag != true);
                        if (existed != null)
                        {
                            rs.Add(existed);
                            continue;
                        }
                        entity.CreatedBy = userId;
                        entity.UpdatedBy = userId;
                        entity.CreatedAt = DateTime.Now;
                        entity.UpdatedAt = DateTime.Now;
                        rs.Add((await _context.Set<WorkingTypeDescriptionEntity>().AddAsync(entity)).Entity);
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

        public List<WorkingTypeDescriptionEntity> GetListByName(string name)
        {
            try
            {
                string cacheKey = $"{cacheKeyList}_{name}";

                List<WorkingTypeDescriptionEntity> result = CachedFunc.GetRedisData<List<WorkingTypeDescriptionEntity>>(cacheKey, null);
                if (result == null)
                {
                    result = new List<WorkingTypeDescriptionEntity>();

                    result = _context.Set<WorkingTypeDescriptionEntity>()
                      .AsNoTracking()
                      .AsQueryable()
                      .Include(entity => entity.WorkingTypeItem)
                      .Where(entity => entity.DeletedFlag != true)
                      .Where(entity => entity.Name != null)
                      .Where(entity => entity.Name.ToUpper() == name.ToUpper())
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
    }
}
