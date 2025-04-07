using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.Common.Enums.DBProvider;
using Serilog;
using iSoft.Common.ConfigsNS;
using SourceBaseBE.Database.Repository;
using SourceBaseBE.Database.DBContexts;
using MathNet.Numerics.Statistics.Mcmc;

using System;
using iSoft.Common.Exceptions;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using iSoft.Common.Models.RequestModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using System.Linq;
using SourceBaseBE.MainService.Models;
using SourceBaseBE.Database.Enums;

using iSoft.Database.Extensions;
using iSoft.Common.Models;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Database.Models;
using UserEntity = SourceBaseBE.Database.Entities.UserEntity;
using ISoftProjectEntity = SourceBaseBE.Database.Entities.ISoftProjectEntity;
using Microsoft.Extensions.Logging;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;
using iSoft.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using iSoft.Common.Utils;
using ICSharpCode.SharpZipLib.Zip;
using SourceBaseBE.Database;
using InfluxDB.Client.Api.Domain;
using Microsoft.AspNetCore.JsonPatch.Internal;

namespace SourceBaseBE.MainService.Services
{
    public class UserService : BaseCRUDService<UserEntity>
    {
        private UserRepository _authUserRepository;
        public UserRepository _repositoryImp;
        private ISoftProjectRepository _iSoftProjectRepository;
        private AuthGroupRepository _authGroupRepository;
        private AuthPermissionRepository _authPermissionRepository;
        private LanguageRepository _languageRepository;
        private EmployeeRepository _employeeRepository;
        private DepartmentAdminRepository _departmentAdminRepository;
        private DepartmentRepository _departmentRepositoty;
        /*[GEN-1]*/

        public UserService(CommonDBContext dbContext, ILogger<UserService> logger)
          : base(dbContext, logger)
        {
            _repository = new UserRepository(_dbContext);
            _repositoryImp = (UserRepository)_repository;
            _authUserRepository = new UserRepository(_dbContext);
            this._iSoftProjectRepository = new(this._dbContext);
            this._authGroupRepository = new(this._dbContext);
            this._authPermissionRepository = new(this._dbContext);
            this._languageRepository = new LanguageRepository(_dbContext);
            this._employeeRepository = new EmployeeRepository(_dbContext);
            this._departmentAdminRepository = new DepartmentAdminRepository(_dbContext);
            this._departmentRepositoty = new DepartmentRepository(_dbContext);
            /*[GEN-2]*/
        }
        public override UserEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repositoryImp.GetById(id, isDirect, isTracking);
            var entityRS = (UserEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public UserModel GetProfileById(long id, bool isDirect = false)
        {
            var entity = _repositoryImp.GetProfileById(id, isDirect);
            //var entityRS = (UserEntity)_authUserRepository.FillTrackingUser(entity);
            return entity;
        }
        public override List<UserEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<UserEntity>().ToList();
            return listRS;
        }
        public List<UserEntity> GetListByAdminRole(EnumDepartmentAdmin adminRole)
        {
            var list = _repositoryImp.GetListByAdminRole(adminRole);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<UserEntity>().ToList();
            return listRS;
        }
        public List<UserEntity> GetListByAdminRoleAndDepartment(EnumDepartmentAdmin adminRole,List<long?> departmetnIds)
        {
            var list = _repositoryImp.GetListByAdminRoleAndDepartment(adminRole, departmetnIds);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<UserEntity>().ToList();
            return listRS;
        }
        public List<UserEntity> GetAll()
        {
            var list = _repositoryImp.GetAll();
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<UserEntity>().ToList();
            return listRS;
        }
        public long? GetUserByName(string name, bool isDirect = false)
        {
            long? UserId = _repositoryImp.GetUserByName(name, isDirect);
            //var entityRS = (long)_authUserRepository.FillTrackingUser(UserId.Value);
            return UserId;
        }
        public List<Dictionary<string, object>> GetFormDataObjElement(UserEntity entity, long? DepartmentId, bool required = false)
        {
            string entityName = nameof(UserEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;
            var properties = typeof(UserEntity).GetProperties()
                                               .Where(p => p.Name == "Avatar" || p.Name == "Address" || p.Name == "CompanyName" || p.Name == "EmployeeId" ||
                                                           p.Name == "Password" || p.Name == "Username" || p.Name == "Birthday" || p.Name == "Email" ||
                                                           p.Name == "FirstName" || p.Name == "MiddleName" || p.Name == "LastName" || p.Name == "Gender" ||
                                                           p.Name == "PhoneNumber" || p.Name == "Displayname" || p.Name == "DepartmentAdmins" || p.Name == "Id");
            var propertiesDepartment = typeof(DepartmentEntity).GetProperties()
                                    .Where(p => p.Name == "Name")
                                    .ToList();
            var combinedProperties = propertiesDepartment.Concat(properties).ToList();
            bool addedFlag = false;
            foreach (var property in combinedProperties)
            {
                addedFlag = false;
                // foreignKeyAttribute
                var foreignKeyAttribute = (ForeignKeyAttribute)Attribute.GetCustomAttribute(property, typeof(ForeignKeyAttribute));
                if (foreignKeyAttribute != null && !addedFlag)
                {
                    string parentEntity = foreignKeyAttribute.Name;
                    listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name.Replace("Id","").Replace("Entity",""), entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", EnumFormDataType.select.ToStringValue()},
                  {"select_data", GetListOptionData(parentEntity, entityName, "")},
                  {"searchable", true},
                });
                    addedFlag = true;
                }

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
                  {"isRequire",formDataTypeAttr.IsRequired }
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
                  {"isRequire",formDataTypeAttr.IsRequired }
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.textbox:
                            string name = nameof(DepartmentEntity).Replace("Entity", "");
                            if (property.Name == "Name")
                            {
                                listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(name, entityName)},
                  {"key", name+"Id"},
                  {"value", _departmentAdminRepository.GetDepartmentUser(entity.Id)},
                  {"type", EnumFormDataType.select.ToStringValue()},
                  {"select_data", GetListOptionData(name,entityName,"")},
                  {"default_value", formDataTypeAttr.DefaultVal},
                  {"isRequire",formDataTypeAttr.IsRequired }
                });
                            }
                            else
                            {
                                listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"placeholder", formDataTypeAttr.Placeholder},
                  {"unit", formDataTypeAttr.Unit},
                  {"isRequire",formDataTypeAttr.IsRequired }
                });
                            }
                            addedFlag = true;
                            break;
                        case EnumFormDataType.password:
                            if(entity.Id > 0)
                            {
                                break;
                            }
                            if(required == true)
                            {
                                listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", null},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"placeholder", formDataTypeAttr.Placeholder},
                  {"unit", formDataTypeAttr.Unit},
                  {"isRequire",formDataTypeAttr.IsRequired }
                });
                            }
                            else
                            {
                                listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", null},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"placeholder", formDataTypeAttr.Placeholder},
                  {"unit", formDataTypeAttr.Unit} 
                });
                            }

                            
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
                  {"isRequire",formDataTypeAttr.IsRequired }
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
                  {"isRequire",formDataTypeAttr.IsRequired }
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
                  {"isRequire",formDataTypeAttr.IsRequired }
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
                  {"radio_data", GetListOptionData(property.Name,entityName,"")},
                  {"default_value", formDataTypeAttr.DefaultVal},
                  {"isRequire",formDataTypeAttr.IsRequired }
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
                  {"select_data", GetListOptionData(property.Name,entityName,"")},
                  {"default_value", formDataTypeAttr.DefaultVal},
                  {"isRequire",formDataTypeAttr.IsRequired }
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.selectMulti:
                            //objectList = new List<object>(formDataTypeAttr.Options);
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", JsonPropertyHelper<UserEntity>.GetDisplayName(property.Name)},
                  {"key", property.Name},
                  //{"value", property.GetValue(entity)},
                  {"value", GetListRoleUser(entity.Id, DepartmentId)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"select_data", GetListOptionData(property.Name,entityName,"")},
                  {"default_value", formDataTypeAttr.DefaultVal},
                  {"isRequire",formDataTypeAttr.IsRequired }
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
                  { "isRequire", formDataTypeAttr.IsRequired }

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
                  { "isRequire", formDataTypeAttr.IsRequired }
                });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.hidden:
                            listRS.Add(new Dictionary<string, object> {
      {"display_name", GetDisplayName(property.Name, entityName)},
      {"key", property.Name},
      {"value",property.GetValue(entity)},
      {"type", formDataTypeAttr.TypeName.ToStringValue()},
    });


                            addedFlag = true;
                            break;
                        default:
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", property.GetValue(entity)},
                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                  {"isRequire",formDataTypeAttr.IsRequired }
                });
                            addedFlag = true;
                            break;
                    }
                }
            }
            return listRS;
        }

        public override object GetDisplayName(string name, string entityName)
        {
            return $"{name}";
        }

        public List<EnumDepartmentAdmin> GetListRoleUser(long userId, long? departmentId)
        {
            return _departmentAdminRepository.GetRoleOfUser(userId, departmentId.GetValueOrDefault(), true);
        }
        public long? GetDepartmentUser(long userId)
        {
            return _departmentAdminRepository.GetDepartmentUser(userId);
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

            List<AuthGroupEntity> authGroupChildren = null;
            if (entity.AuthGroupIds != null && entity.AuthGroupIds.Count >= 1)
            {
                authGroupChildren = _authGroupRepository.GetListByListIds(entity.AuthGroupIds, true);
            }

            List<AuthPermissionEntity> authPermissionChildren = null;
            if (entity.AuthPermissionIds != null && entity.AuthPermissionIds.Count >= 1)
            {
                authPermissionChildren = _authPermissionRepository.GetListByListIds(entity.AuthPermissionIds, true);
            }
            /*[GEN-3]*/
            var upsertedEntity = ((UserRepository)_repository).Upsert(entity, iSoftProjectChildren, authGroupChildren, authPermissionChildren/*[GEN-4]*/, userId);
            var entityRS = (UserEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
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

                case nameof(AuthGroupEntity):
                    if (entity.ListAuthGroup == null)
                    {
                        return new List<long>();
                    }
                    return entity.ListAuthGroup.Select(x => x.Id).ToList();

                case nameof(AuthPermissionEntity):
                    if (entity.ListAuthPermission == null)
                    {
                        return new List<long>();
                    }
                    return entity.ListAuthPermission.Select(x => x.Id).ToList();
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
            targetEntity = targetEntity.Replace("Entity", "").Replace("Id", "");
            switch (targetEntity)
            {
                case nameof(ISoftProjectEntity):
                    listRS = this._iSoftProjectRepository.GetSelectData(entityName, category);
                    break;

                case nameof(AuthGroupEntity):
                    listRS = this._authGroupRepository.GetSelectData(entityName, category);
                    break;

                case nameof(AuthPermissionEntity):
                    listRS = this._authPermissionRepository.GetSelectData(entityName, category);
                    break;
                case "Employee":
                    listRS = this._employeeRepository.GetSelectData(entityName, category);
                    break;
                case nameof(UserEntity.DepartmentAdmins):
                    //var listStatus = Enum.GetValues<EnumDepartmentAdmin>();
                    var listStatus = new List<EnumDepartmentAdmin>() {
            EnumDepartmentAdmin.User,
            EnumDepartmentAdmin.Admin1,
            EnumDepartmentAdmin.Admin2,
            EnumDepartmentAdmin.Admin3,
          };
                    listRS = listStatus.Select(x => new FormSelectOptionModel((int)x, x.ToString())).ToList();
                    break;
                case nameof(UserEntity.Gender):
                    var listGender = Enum.GetValues<iSoft.Common.Enums.EnumGender>();
                    listRS = listGender.Select(x => new FormSelectOptionModel((int)x, x.ToString())).ToList();
                    break;
                case "Department":
                    var listTmp = this._departmentRepositoty.GetAll();
                    listRS = listTmp.Select(x => new FormSelectOptionModel(x.Id, x.Name)).ToList();
                    break;
                /*[GEN-6]*/
                default:
                    break;
            }
            return listRS;
        }

        public EnumDepartmentAdmin GetRoleDepartmentData(UserEntity userEntity, EnumDepartmentAdmin enumDepartmentAdmin)
        {
            EnumDepartmentAdmin enumDepartmentAdmin1 = EnumDepartmentAdmin.User;


            return enumDepartmentAdmin1;
        }


        public bool UpsertTransaction(UserEntity entity, List<DepartmentAdminEntity> listDepartmentAdmin, long? departmentId, long? userIdUpdateAction = 0)
        {
            using (var transaction = this._dbContext.Database.BeginTransaction())
            {
                try
                {
                    UserEntity userEntity = null;
                    userEntity = _repositoryImp.GetById(entity.Id, true);

                    if (userEntity == null)
                    {
                        userEntity = _repository.Upsert(entity, userIdUpdateAction);
                    }

                    if (userEntity == null)
                    {
                        throw new DBException("Upsert user return null value.");
                    }

                    if (userEntity.DepartmentAdmins != null && userEntity.DepartmentAdmins.Count > 0)
                    {
                        var listDepartmentAdminOld = userEntity.DepartmentAdmins.ToList();
                        var listAdmin3Item = listDepartmentAdminOld.Where(x => x.DeletedFlag != true && x.DepartmentId == departmentId && x.Role == EnumDepartmentAdmin.Admin3).ToList();
                        if (listAdmin3Item != null && listAdmin3Item.Count >= 1)
                        {
                            var listAdmin3ItemNew = listDepartmentAdmin?.Where(x => x.DeletedFlag != true && x.DepartmentId == departmentId && x.Role == EnumDepartmentAdmin.Admin3).ToList();
                            if (listAdmin3ItemNew == null || listAdmin3ItemNew.Count <= 0)
                            {
                                throw new DBException("Can not disable role Admin3.");
                            }
                        }
                        if (listDepartmentAdminOld != null && listDepartmentAdminOld.Count > 0)
                        {
                            _departmentAdminRepository.DeleteMulti(listDepartmentAdminOld, userIdUpdateAction);
                        }
                    }

                    foreach (var departmentAdmin in listDepartmentAdmin)
                    {
                        departmentAdmin.UserId = userEntity.Id;
                        departmentAdmin.DepartmentId = departmentId;
                    }
                    _departmentAdminRepository.UpSertMulti(listDepartmentAdmin, userIdUpdateAction);
                    transaction.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        //return false;
                    }
                    catch { }
                    throw new DBException(ex);
                }
            }
        }

        public async Task<bool> ChangePassword(UserEntity userEntity, string newPasswork)
        {
            try
            {
                if (userEntity != null)
                {
                    string pw = EncodeUtil.MD5(newPasswork);
                    userEntity.Password = pw;
                    var result = _repositoryImp.Upsert(userEntity, null, true);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw new DBException(ex);
            }
        }
        public bool IsStrongPassword(string password)
        {
            if (password.Length < GlobalConsts.MINIMUM_LENGTH_PASSWORD || password.Length > GlobalConsts.MAXIMUM_LENGTH_PASSWORD)
            {
                return false;
            }

            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }

        public UserDepartmentPagingResponseModel GetListAdminSetting(
          PagingParamRequestModel pagingReq = null)
        {
            List<UserDepartmentResponseModel> listResponseModel = null;

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var ret = _repositoryImp.GetListUserDepartment(pagingReq, filterParams, searchParams, sortParams);
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<UserDepartmentResponseModel>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = UserDepartmentResponseModel.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;

        }
    }
}