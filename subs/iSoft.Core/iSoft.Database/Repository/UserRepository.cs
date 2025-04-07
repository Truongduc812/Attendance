using iSoft.Database.DBContexts;
using iSoft.Database.Entities;
using Microsoft.Extensions.Logging;
using System.Data;
using iSoft.Common.Exceptions;
using iSoft.Common.Models.RequestModels;
using iSoft.Common;
using iSoft.Database.Models;
using Microsoft.EntityFrameworkCore;
using iSoft.Common.Models.ConfigModel.Subs;
using iSoft.Redis.Services;

namespace iSoft.Database.Repository
{
    public class UserRepository : BaseCRUDRepository<UserEntity>
    {
        public UserRepository(CommonDBContext dbContext, ServerConfigModel redisConfig, ILoggerFactory loggerFactory)
            : base(dbContext, redisConfig, loggerFactory)
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
                //string cacheKey = $"{cacheKeyDetail}:{id}";
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
                      .Include(entity => entity.ListISoftProject)/*[GEN-7]*/
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
        public UserEntity? GetByUsername(string userName, bool isDirect = false)
        {
            try
            {
                UserEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}_userName:{userName}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<UserEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<UserEntity>()
                              //.AsNoTracking()
                              .AsQueryable()
                              .Include(entity => entity.ListISoftProject)/*[GEN-7]*/
                              .Where(entity => entity.DeletedFlag != true && entity.Username == userName)
                              .FirstOrDefault();
                    //result.User2s = result.User2s.Select(x => new User2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
        public UserEntity? GetByUsernameAndPassword(string username, string password, bool isDirect = false)
        {
            try
            {
                UserEntity? result = null;
                string cacheKey = $"{cacheKeyDetail}_UAP2:{username}_{password}";
                if (!isDirect)
                {
                    result = CachedFunc.GetRedisData<UserEntity>(cacheKey, null);
                }

                if (result == null)
                {
                    result = _context.Set<UserEntity>()
                              //.AsNoTracking()
                              .AsQueryable()
                              .Include(entity => entity.ListISoftProject)/*[GEN-7]*/
                              .Where(entity => entity.DeletedFlag != true && entity.Username == username && entity.Password == password)
                              .FirstOrDefault();
                    //result.User2s = result.User2s.Select(x => new User2Entity() { Id = x.Id, Name = x.Name }).ToList();

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
                string cacheKey = $"{cacheKeyList}";
                if (pagingReq != null)
                {
                    cacheKey = $"{cacheKeyList}:{pagingReq.Page}|{pagingReq.PageSize}";
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
                                .Include(entity => entity.ListISoftProject)
                                .Where(entity => entity.DeletedFlag != true)
                                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                                .Skip(pagingReq.GetSkip()).Take(pagingReq.GetLimit())
                                .AsParallel()
                                .ToList();

                        for (var i = 0; i < result.Count; i++)
                        {

                            result[i].ListISoftProject = result[i].ListISoftProject?.Select(x => new ISoftProjectEntity() { Id = x.Id }).ToList();
                            /*[GEN-12]*/
                        }
                    }
                    else
                    {
                        result = _context.Set<UserEntity>()
                                .AsNoTracking()
                                .AsQueryable()
                                .Include(entity => entity.ListISoftProject)
                                .Where(entity => entity.DeletedFlag != true)
                                .OrderBy(entity => entity.Order).ThenBy(entity => entity.Id)
                                .Skip(0).Take(ConstCommon.ConstSelectListMaxRecord)
                                .AsParallel()
                                .ToList();

                        for (var i = 0; i < result.Count; i++)
                        {

                            result[i].ListISoftProject = result[i].ListISoftProject?.Select(x => new ISoftProjectEntity() { Id = x.Id }).ToList();
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
        public UserEntity Upsert(UserEntity entity, List<ISoftProjectEntity> iSoftProjectChildren/*[GEN-8]*/, long? userId = null)
        {
            try
            {
                if (entity.Id <= 0)
                {
                    // Insert
                    entity = Insert(entity, iSoftProjectChildren/*[GEN-4]*/, userId);
                }
                else
                {
                    // Update
                    entity = Update(entity, iSoftProjectChildren/*[GEN-4]*/, userId);
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public UserEntity Insert(UserEntity entity, List<ISoftProjectEntity> iSoftProjectChildren/*[GEN-8]*/, long? userId = null)
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
        public UserEntity Update(UserEntity entity, List<ISoftProjectEntity> iSoftProjectChildren/*[GEN-8]*/, long? userId = null)
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
    }
}
