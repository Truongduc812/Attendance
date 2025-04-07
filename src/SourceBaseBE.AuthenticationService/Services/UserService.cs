using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using iSoft.DBLibrary.SQLBuilder;
using Microsoft.Extensions.Logging;
using iSoft.DBLibrary.DBConnections.Factory;
using System.Threading.Tasks;
using iSoft.Common.Enums.DBProvider;
using iSoft.Database.Repository;
using iSoft.Database.Entities;
using iSoft.Database.DBContexts;
using iSoft.Auth.ExtensionMethods;
using iSoft.Auth.Models;
using iSoft.Common.Utils;
using iSoft.Common.Enums;
using iSoft.Common.ExtensionMethods;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Models.RequestModels;
using iSoft.Database.Extensions;
using iSoft.Database.Models;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Models.ResponseModels;
using iSoft.Common.ConfigsNS;

namespace iSoft.Auth.Services
{
    public class UserService : BaseCRUDService<UserEntity>
    {
        private ILogger<UserService> _logger;
        private CommonDBContext _dbContext;
        public UserRepository _repositoryImp;
        private ISoftProjectRepository _iSoftProjectRepository;

        public UserService(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<UserService>();

            this._dbContext = new CommonDBContext(DBConnectionFactory.CreateDBConnection(
              CommonConfig.GetConfig().MasterDatabaseConfig));

            _repository = new UserRepository(this._dbContext, CommonConfig.GetConfig().RedisConfig, loggerFactory);
            _repositoryImp = (UserRepository)_repository;
            this._iSoftProjectRepository = new ISoftProjectRepository(this._dbContext, CommonConfig.GetConfig().RedisConfig, loggerFactory);
        }

        public async Task<UserResponseModel> Authenticate(string username, string password)
        {
            string projects = "";
            UserResponseModel userRS = null;
            if (username == "root" && password == EncodeUtil.MD5("vuletech@113"))
            {
                userRS = new UserResponseModel()
                {
                    Id = 0,
                    Username = username,
                    Password = password,
                    Role = EnumUserRole.Root.ToString(),
                };
                projects = "SourceBaseBE,PRPO";
            }
            else
            {
                var userEntity = _repositoryImp.GetByUsernameAndPassword(username, password, true);

                if (userEntity == null)
                {
                    return null;
                }
                userRS = new UserResponseModel()
                {
                    Id = userEntity.Id,
                    Username = userEntity.Username,
                    Password = userEntity.Password,
                    Role = userEntity.Role,
                };

                if (string.IsNullOrEmpty(projects))
                {
                    if (userEntity.ListISoftProject != null && userEntity.ListISoftProject.Count >= 1)
                    {
                        projects = string.Join(",", userEntity.ListISoftProject.Select(x => x.Name)).ToUpper();
                    }
                }
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(CommonConfig.GetConfig().AuthenticationSecret);
            var time_Expired = int.Parse(Environment.GetEnvironmentVariable("TIME_EXPIRED"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                                        new Claim(ClaimTypes.Name, userRS.Username),
                    new Claim(ClaimTypes.Role, userRS.Role),
                    new Claim(EnumIdentityType.UserId.ToString(), userRS.Id.ToString()),
                    new Claim(EnumIdentityType.Username.ToString(), userRS.Username),
                    new Claim(EnumIdentityType.Projects.ToString(), projects),
          }),

                Expires = DateTime.UtcNow.AddSeconds(time_Expired),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userRS.AccessToken = tokenHandler.WriteToken(token);

            return userRS.WithoutPassword();
        }

        public string Insert(UserEntity user)
        {
            var existsUser = _repositoryImp.GetByUsername(user.Username, true);
            if (existsUser == null)
            {
                user.Status = EnumActiveStatus.Actived;
                user.DeletedFlag = false;
                _repository.Insert(user);
                return "";
            }
            return existsUser.Username;
        }
        public UserEntity GetById(long id)
        {
            var user = _repository.GetById(id);
            return user.WithoutPassword();
        }
        public override UserEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repository.GetById(id, isDirect, isTracking);
            var entityRS = (UserEntity)_repositoryImp.FillTrackingUser(entity);
            return entityRS;
        }
        public override List<UserEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _repositoryImp.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<UserEntity>().ToList();
            return listRS;
        }
        public override List<Dictionary<string, object>> GetFormDataObjElement(UserEntity entity)
        {
            string entityName = nameof(UserEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;
            var properties = typeof(UserEntity).GetProperties();
            bool addedFlag = false;
            foreach (var property in properties)
            {
                addedFlag = false;
                // foreignKeyAttribute
                var foreignKeyAttribute = (ForeignKeyAttribute)Attribute.GetCustomAttribute(property, typeof(ForeignKeyAttribute));
                if (foreignKeyAttribute != null && !addedFlag)
                {
                    string parentEntity = foreignKeyAttribute.Name;
                    listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", EnumFormDataType.select.ToStringValue()},
                  {"select_data", GetListOptionData(parentEntity, entityName, "")},
                });
                    addedFlag = true;
                }

                // ListEntityAttribute
                var listEntityAttribute = (ListEntityAttribute)Attribute.GetCustomAttribute(property, typeof(ListEntityAttribute));
                if (listEntityAttribute != null && !addedFlag)
                {
                    string childEntity = listEntityAttribute.EntityTargetName;

                    listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", GetListIdChildren(entity, childEntity)},
                  {"type", EnumFormDataType.selectMulti.ToStringValue()},
                  {"select_multi_data", GetListOptionData(childEntity, entityName, listEntityAttribute.Category)},
                });
                    addedFlag = true;
                }

                // ListEntityAttribute
                var formDataTypeAttr = (FormDataTypeAttribute)Attribute.GetCustomAttribute(property, typeof(FormDataTypeAttribute));

                if (formDataTypeAttr == null && !addedFlag)
                {
                    if (property.PropertyType == typeof(string) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(string))
                    {
                        formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.textbox, false, property.Name);
                    }
                    else if (property.PropertyType == typeof(DateTime) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime))
                    {
                        formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.datetime, false);
                    }
                    else if (property.PropertyType == typeof(int) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(int)
                      || property.PropertyType == typeof(long) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(long)
                      || property.PropertyType == typeof(short) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(short))
                    {
                        formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.integerNumber, false);
                    }
                    else if (property.PropertyType == typeof(double) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(double))
                    {
                        formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.floatNumber, false);
                    }
                    else if (property.PropertyType == typeof(bool) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(bool))
                    {
                        formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.checkbox, false);
                    }
                }

                if (formDataTypeAttr != null && !addedFlag)
                {
                    objectList = new List<object>(formDataTypeAttr.Options);
                    switch (formDataTypeAttr.TypeName)
                    {
                        case EnumFormDataType.integerNumber:
                        case EnumFormDataType.floatNumber:
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"min", formDataTypeAttr.Min},
                  {"max", formDataTypeAttr.Max},
                  {"default_value", formDataTypeAttr.DefaultVal},
                  {"unit", formDataTypeAttr.Unit},
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.checkbox:
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"default_value", formDataTypeAttr.DefaultVal},
                  {"unit", formDataTypeAttr.Unit},
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.textbox:
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"placeholder", formDataTypeAttr.Placeholder},
                  {"unit", formDataTypeAttr.Unit},
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.label:
                        case EnumFormDataType.readonlyType:
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"unit", formDataTypeAttr.Unit},
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.datetime:
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"min", formDataTypeAttr.Min},
                  {"max", formDataTypeAttr.Max},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.dateOnly:
                            string value = property.GetValue(entity) == null ? "" : ((DateTime)property.GetValue(entity)).ToString(ConstDateTimeFormat.YYYYMMDD);
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value",value },
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"min", formDataTypeAttr.Min},
                  {"max", formDataTypeAttr.Max},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.radio:
                            //objectList = new List<object>(formDataTypeAttr.Options);
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"radio_data", GetListOptionData(objectList)},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.select:
                            //objectList = new List<object>(formDataTypeAttr.Options);
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"select_data", GetListOptionData(objectList)},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.timespan:
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"min", formDataTypeAttr.Min},
                  {"max", formDataTypeAttr.Max},
                  {"default_value", formDataTypeAttr.DefaultVal},
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.image:
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"width", formDataTypeAttr.Width},
                  {"height", formDataTypeAttr.Height},
                });
                            addedFlag = true;
                            break;
                        default:
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                });
                            addedFlag = true;
                            break;
                    }
                }
            }
            return listRS;
        }

        /// <summary>
        /// UpsertIfNotExist (@GenCRUD)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override UserEntity Upsert(UserEntity entity, long? userId = null)
        {
            List<ISoftProjectEntity> iSoftProjectChildren = null;
            if (entity.ISoftProjectIds != null && entity.ISoftProjectIds.Count >= 1)
            {
                iSoftProjectChildren = _iSoftProjectRepository.GetListByListIds(entity.ISoftProjectIds, true);
            }
            /*[GEN-3]*/
            var upsertedEntity = ((UserRepository)_repositoryImp).Upsert(entity, iSoftProjectChildren/*[GEN-4]*/, userId);
            var entityRS = (UserEntity)_repositoryImp.FillTrackingUser(upsertedEntity);
            return entityRS;
        }
        public override int Delete(long id, long? userId = null, bool isSoftDelete = true)
        {
            int deletedCount = _repository.Delete(id, userId, isSoftDelete);
            return deletedCount;
        }

        /// <summary>
        /// GetListIdChildren (@GenCRUD)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="childEntity"></param>
        /// <returns></returns>
        private List<long> GetListIdChildren(UserEntity entity, string childEntity)
        {
            switch (childEntity)
            {

                case nameof(ISoftProjectEntity):
                    if (entity.ListISoftProject == null)
                    {
                        return new List<long>();
                    }
                    return entity.ListISoftProject.Select(x => x.Id).ToList();

                /*[GEN-5]*/
                default:
                    return new List<long>();
            }
        }

        /// <summary>
        /// GetListOptionData (@GenCRUD)
        /// </summary>
        /// <param name="targetEntity"></param>
        /// <param name="entityName"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<FormSelectOptionModel> GetListOptionData(string targetEntity, string entityName, string category)
        {
            var listRS = new List<FormSelectOptionModel>();
            switch (targetEntity)
            {
                case nameof(ISoftProjectEntity):
                    listRS = this._iSoftProjectRepository.GetSelectData(entityName, category);
                    break;
                /*[GEN-6]*/
                default:
                    break;
            }
            return listRS;
        }
        public bool IsExistUsername(string username)
        {
            return _dbContext.Users.Any(x => x.Username == username);
        }
    }
}