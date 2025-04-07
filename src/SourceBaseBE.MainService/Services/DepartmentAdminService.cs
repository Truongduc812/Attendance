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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using iSoft.Common.Models.ResponseModels;
using iSoft.Common.Utils;
using OfficeOpenXml;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;
using System.IO;
using SourceBaseBE.Database.Models.RequestModels.Generate;
using iSoft.Common.Enums;
using Confluent.Kafka;
using iSoft.ExportLibrary.Services;
using NPOI.SS.Formula.Functions;
using Nest;
using NPOI.Util;
using iSoft.ExportLibrary.Models;
using static SourceBaseBE.MainService.Models.EmployeeListmodel;
using NPOI.SS.UserModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using StackExchange.Redis;

namespace SourceBaseBE.MainService.Services
{
    public class DepartmentAdminService : BaseCRUDService<DepartmentAdminEntity>
    {
        private UserRepository _authUserRepository;
        public DepartmentAdminRepository _repositoryImp;
        public DepartmentRepository _departmentRepository;
        public EmployeeRepository _employeeRepository;
        public DepartmentAdminRepository _departmentDepartmentRepository;
        public UserRepository _userRepository;
        public LanguageRepository _languageRepository;


        /*[GEN-1]*/

        public DepartmentAdminService(CommonDBContext dbContext, ILogger<DepartmentAdminService> logger)
          : base(dbContext, logger)
        {
            _repository = new DepartmentAdminRepository(_dbContext);
            _repositoryImp = (DepartmentAdminRepository)_repository;
            _authUserRepository = new UserRepository(_dbContext);
            _departmentRepository = new DepartmentRepository(_dbContext);
            _employeeRepository = new EmployeeRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);
            _languageRepository = new LanguageRepository(_dbContext);
            _departmentDepartmentRepository = new DepartmentAdminRepository(_dbContext);
            /*[GEN-2]*/
        }
        public override DepartmentAdminEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repositoryImp.GetById(id, isDirect, isTracking);
            var entityRS = (DepartmentAdminEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }

        public List<DepartmentAdminEntity> GetByUserAndRoleAndDepartment(long userid, List<EnumDepartmentAdmin> enumDepartmentAdmin, long? departmentId = 0, bool isDirect = false)
        {
            var entity = _repositoryImp.GetByUserAndRoleAndDepartment(userid, enumDepartmentAdmin, departmentId, isDirect);
            return entity;
        }
        public List<DepartmentEntity> GetUserAndRole(long userid, List<EnumDepartmentAdmin> enumDepartmentAdmin, bool isDirect = false)
        {
            var entity = _repositoryImp.GetListDepartmentByAdmin(userid, enumDepartmentAdmin, isDirect);
            return entity;
        }
        public override async Task<DepartmentAdminEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = await _repository.GetByIdAsync(id, isDirect);
            var entityRS = (DepartmentAdminEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override List<DepartmentAdminEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<DepartmentAdminEntity>().ToList();
            return listRS;
        }
        public async Task<bool> CheckAdminDepartmentExisted(long departmentId, long userId, EnumDepartmentAdmin role)
        {
            var result = await _repositoryImp.CheckAdminDepartmentIsExisted(departmentId, userId, role);
            return result;
        }
        public List<long?> GetListDepartmentByUserId(long? userId, bool isDirect)
        {
            var list = _repositoryImp.GetListDepartmentByUserId(userId, isDirect).ToList();
            return list;
        }

        public override List<Dictionary<string, object>> GetFormDataObjElement(DepartmentAdminEntity entity)
        {
            string entityName = nameof(DepartmentAdminEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;

            var properties = typeof(DepartmentAdminEntity).GetProperties()
                        .Where(p => p.Name == "DepartmentId" || p.Name == "Role" || p.Name == "Id")
                        .ToList();
            bool addedFlag = false;
            foreach (var property in properties)
            {
                addedFlag = false;
                //// foreignKeyAttribute
                //var foreignKeyAttribute = (ForeignKeyAttribute)Attribute.GetCustomAttribute(property, typeof(ForeignKeyAttribute));
                //if (foreignKeyAttribute != null && !addedFlag)
                //{
                //  string parentEntity = foreignKeyAttribute.Name;
                //  listRS.Add(new Dictionary<string, object> {
                //  {"display_name", GetDisplayName(property.Name.Replace("Id","").Replace("Entity",""), entityName)},
                //  {"key", property.Name},
                //  {"value", property.GetValue(entity)},
                //  {"type", EnumFormDataType.select.ToStringValue()},
                //  {"select_data", GetListOptionData(parentEntity, entityName, "")},
                //});
                //  addedFlag = true;
                //}

                //// ListEntityAttribute
                //var listEntityAttribute = (ListEntityAttribute)Attribute.GetCustomAttribute(property, typeof(ListEntityAttribute));
                //if (listEntityAttribute != null && !addedFlag)
                //{
                //  string childEntity = listEntityAttribute.EntityTargetName;

                //  listRS.Add(new Dictionary<string, object> {
                //  {"display_name", GetDisplayName(property.Name, entityName)},
                //  {"key", property.Name},
                //  {"value", GetListIdChildren(entity, childEntity)},
                //  {"type", EnumFormDataType.selectMulti.ToStringValue()},
                //  {"select_multi_data", GetListOptionData(childEntity, entityName, listEntityAttribute.Category)},
                //});
                //  addedFlag = true;
                //}

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
                            listRS.Add(new Dictionary<string, object> {
    {"display_name", GetDisplayName(property.Name, entityName)},
    {"key", property.Name},
    {"value", property.GetValue(entity)},
    {"type", formDataTypeAttr.TypeName.ToStringValue()},
    {"placeholder", formDataTypeAttr.Placeholder},
    {"unit", formDataTypeAttr.Unit},
    {"isRequire",formDataTypeAttr.IsRequired }
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
    {"radio_data", GetListOptionData(objectList)},
    {"default_value", formDataTypeAttr.DefaultVal},
    {"isRequire",formDataTypeAttr.IsRequired }
  });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.select:
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
                            listRS.Add(new Dictionary<string, object> {
                  {"display_name", GetDisplayName(property.Name, entityName)},
                  {"key", property.Name},
                  {"value", GetListRoleByUserId(entity.UserId)},
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
    {"isRequire",formDataTypeAttr.IsRequired }

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
    {"isRequire",formDataTypeAttr.IsRequired }

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

        private object GetListRoleByUserId(long? userId)
        {
            if (userId == null)
            {
                return null;
            }
            var userEntity = _userRepository.GetById(userId.Value, false, false);
            List<int> list = userEntity.DepartmentAdmins.Where(x => x.DeletedFlag != true)
                                                        .Select(x => (int)x.Role).ToList();
            list.Sort((int x, int y) =>
            {
                if (x == (int)EnumDepartmentAdmin.Admin3)
                {
                    return 1;
                }
                if (y == (int)EnumDepartmentAdmin.Admin3)
                {
                    return -1;
                }
                if (x == (int)EnumDepartmentAdmin.Admin2)
                {
                    return 1;
                }
                if (y == (int)EnumDepartmentAdmin.Admin2)
                {
                    return -1;
                }
                if (x == (int)EnumDepartmentAdmin.Admin1)
                {
                    return 1;
                }
                if (y == (int)EnumDepartmentAdmin.Admin1)
                {
                    return -1;
                }
                return 0;
            });
            return list;
        }

        ///// <summary>
        ///// UpsertIfNotExist (@GenCRUD)
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public DepartmentAdminEntity Upsert(DepartmentAdminRequestModel request, long? userId = null)
        //{

        //  foreach (var item in request.DepartmentAdmins)
        //  {
        //    departmentAdminEntity = new DepartmentAdminEntity();
        //    departmentAdminEntity.Role = (EnumDepartmentAdmin)item;
        //    departmentAdminEntity.DepartmentId = request.DepartmentId;
        //    listDepartmentAdmin.Add(departmentAdminEntity);
        //  }



        //  /*[GEN-3]*/
        //  var upsertedEntity = ((DepartmentAdminRepository)_repository).Upsert(entity/*[GEN-4]*/, userId);
        //  var entityRS = (DepartmentAdminEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
        //  return entityRS;
        //}
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
        private List<long> GetListIdChildren(DepartmentAdminEntity entity, string childEntity)
        {
            switch (childEntity)
            {


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
                case nameof(DepartmentAdminEntity.Department):
                    var listEmps = _departmentRepository.GetAll();
                    listRS = listEmps.Select(x => new FormSelectOptionModel(x.Id, x.Name)).ToList();
                    break;
                case nameof(DepartmentAdminEntity.User):
                    var listTypes = _userRepository.GetAll();
                    listRS = listTypes.Select(x => new FormSelectOptionModel(x.Id, x.Username)).ToList();
                    break;
                case nameof(DepartmentAdminEntity.Role):
                    //var listStatus = Enum.GetValues<EnumDepartmentAdmin>();
                    var listStatus = new List<EnumDepartmentAdmin>() {
            EnumDepartmentAdmin.User,
            EnumDepartmentAdmin.Admin1,
            EnumDepartmentAdmin.Admin2,
            EnumDepartmentAdmin.Admin3,
          };
                    listRS = listStatus.Select(x => new FormSelectOptionModel((int)x, x.ToString())).ToList();
                    break;
                case "Employee":
                    var listEmployee = _employeeRepository.GetAll();
                    listRS = listEmployee.Select(x => new FormSelectOptionModel(x.Id, x.Name)).ToList();
                    break;
                /*[GEN-6]*/
                default:
                    break;
            }
            return listRS;
        }

        public override object GetDisplayName(string name, string entityName)
        {
            return $"{name}";
        }

        public long? GetValueEmployeeId(DepartmentAdminEntity entity)
        {
            if (entity == null) return null;
            var value = _userRepository.GetEmployeeByUserId(entity.UserId, true);
            if (value == null) return null;
            return value;
        }

        public async Task<Messages.Message> ImportFileAdminDepartment(DepartmentAdminRequestModel request)
        {
            using (var transaction = this._dbContext.Database.BeginTransaction())
            {
                var dicFormFile = request.GetFiles();

                if (dicFormFile == null && dicFormFile.Count <= 0)
                {
                    return Messages.ErrNotFound_0_1.SetParameters("Not Found File Template Import", "");
                }

                string filePath = UploadUtil.UploadFileExcel(dicFormFile);

                FileInfo existingFile = new FileInfo(filePath);
                if (!existingFile.Exists == true)
                {
                    return Messages.ErrNotFound_0_1.SetParameters("Not Found File Template Import", "");
                }

                List<DepartmentAdminEntity> listAdminDepartment = new List<DepartmentAdminEntity>();
                List<DepartmentAdminEntity> listAdminDepartmentChecked = new List<DepartmentAdminEntity>();
                List<long> listIds = new List<long>();

                using (var package = ExcelPro.LoadTempFile(filePath))
                {
                    ExcelWorkbook workBook = package.Workbook;
                    if (workBook == null || workBook.Worksheets.Count <= 0)
                    {
                        return Messages.ErrNotFound_0_1.SetParameters("Not Found Sheet Data", "");
                    }

                    /* Check if barcode network*/
                    bool IsExit = false;
                    bool isCheckFoundSheet = false;
                    int count = 0;
                    for (int i = 0; (i < workBook.Worksheets.Count) && (IsExit == false); i++)
                    {
                        string sheetName = workBook.Worksheets[i].Name.ToString().Trim().ToLower();
                        if (sheetName == "Department Admin".ToLower())
                        {
                            isCheckFoundSheet = true;
                        }
                        else
                        {
                            continue;
                        }
                        ExcelWorksheet Worksheet = workBook.Worksheets[i];
                        IsExit = true;
                        int StartRow = 0;
                        int COL_STT = 2;
                        int COL_Name = 3;
                        int COL_Department = 4;
                        int COL_Admin1 = 6;
                        int COL_Admin2 = 7;
                        int COL_Admin3 = 8;

                        //Find start-row
                        bool is_exit_start_row = false;
                        for (int row = 0; (row <= Worksheet.Dimension.End.Row) && (is_exit_start_row == false); row++)
                        {
                            string STT = ExcelHelper.GetCellValue(Worksheet, row, COL_STT);
                            if (STT.Trim().ToLower() == "stt")
                            {
                                is_exit_start_row = true;
                                StartRow = row + 1;
                                break;
                            }
                        }

                        int nCountToExit = 0;
                        DepartmentAdminEntity departmentAdminEntity = null;
                        DepartmentEntity departmentEntity = null;
                        EmployeeEntity employeeEntityAdmin1 = null;
                        EmployeeEntity employeeEntityAdmin2 = null;
                        EmployeeEntity employeeEntityAdmin3 = null;
                        UserEntity userEntity1 = null;
                        UserEntity userEntity2 = null;
                        UserEntity userEntity3 = null;
                        List<string> departmentStr = new List<string>();

                        //
                        for (int row = 9; (row <= Worksheet.Dimension.End.Row) && (nCountToExit < 10); row++)
                        {
                            string STT = ExcelHelper.GetCellValue(Worksheet, row, COL_STT);
                            if (STT == "")
                            {
                                nCountToExit++;
                                continue;
                            }

                            string department = ExcelPro.GetValue(package, sheetName, row, "C")?.ToString();
                            if (string.IsNullOrEmpty(department))
                            {
                                continue;
                            }
                            else
                            {
                                departmentEntity = _departmentRepository.GetByName(department);
                            }

                            if (departmentEntity == null)
                            {
                                department = "";
                                continue;
                            }

                            string employeeAdmin1 = ProcessEmpCode(ExcelPro.GetValue(package, sheetName, row, "H")?.ToString());
                            string employeeAdmin2 = ProcessEmpCode(ExcelPro.GetValue(package, sheetName, row, "I")?.ToString());
                            string employeeAdmin3 = ProcessEmpCode(ExcelPro.GetValue(package, sheetName, row, "J")?.ToString());
                            if (!string.IsNullOrEmpty(employeeAdmin1))
                            {
                                employeeEntityAdmin1 = _employeeRepository.GetByEmpCode(employeeAdmin1);
                            }
                            if (employeeEntityAdmin1 == null)
                            {
                                string admin1AccountName = ProcessAdminAccount(ExcelPro.GetValue(package, sheetName, row, "H")?.ToString());
                                if (!string.IsNullOrEmpty(admin1AccountName))
                                {
                                    userEntity1 = await _userRepository.GetUserByUserName(admin1AccountName);
                                    if (userEntity1 == null)
                                    {
                                        userEntity1 = new UserEntity()
                                        {
                                            Username = admin1AccountName,
                                            Password = EncodeUtil.MD5("123123"),
                                            Displayname = admin1AccountName,
                                            Role = "Admin"
                                        };
                                        userEntity1 = _userRepository.Upsert(userEntity1);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(employeeAdmin2))
                            {
                                employeeEntityAdmin2 = _employeeRepository.GetByEmpCode(employeeAdmin2);
                            }
                            if (employeeEntityAdmin2 == null)
                            {
                                string admin2AccountName = ProcessAdminAccount(ExcelPro.GetValue(package, sheetName, row, "I")?.ToString());
                                if (!string.IsNullOrEmpty(admin2AccountName))
                                {
                                    userEntity2 = await _userRepository.GetUserByUserName(admin2AccountName);
                                    if (userEntity2 == null)
                                    {
                                        userEntity2 = new UserEntity()
                                        {
                                            Username = admin2AccountName,
                                            Password = EncodeUtil.MD5("123123"),
                                            Displayname = admin2AccountName,
                                            Role = "Admin"
                                        };
                                        userEntity2 = _userRepository.Upsert(userEntity2);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(employeeAdmin3))
                            {
                                employeeEntityAdmin3 = _employeeRepository.GetByEmpCode(employeeAdmin3);
                            }
                            if (employeeEntityAdmin3 == null)
                            {
                                string admin3AccountName = ProcessAdminAccount(ExcelPro.GetValue(package, sheetName, row, "J")?.ToString());
                                if (!string.IsNullOrEmpty(admin3AccountName))
                                {
                                    userEntity3 = await _userRepository.GetUserByUserName(admin3AccountName);
                                    if (userEntity3 == null)
                                    {
                                        userEntity3 = new UserEntity()
                                        {
                                            Username = admin3AccountName,
                                            Password = EncodeUtil.MD5("123123"),
                                            Displayname = admin3AccountName,
                                            Role = "Admin"
                                        };
                                        userEntity3 = _userRepository.Upsert(userEntity3);
                                    }
                                }
                            }

                            if ((userEntity1 != null && employeeEntityAdmin1 == null) || employeeEntityAdmin1 != null)
                            {
                                DepartmentAdminEntity departmentAdmin = new DepartmentAdminEntity();
                                if (employeeEntityAdmin1 != null)
                                {
                                    userEntity1 = _userRepository.GetByIdEmployee(employeeEntityAdmin1.EmployeeCode, true);
                                    if (userEntity1 != null)
                                    {
                                        departmentAdmin = _repositoryImp.GetByIdAndRole(departmentEntity.Id, userEntity1.Id, EnumDepartmentAdmin.Admin1, true);
                                    }
                                    else
                                    {
                                        if (userEntity1 == null)
                                        {
                                            userEntity1 = new UserEntity()
                                            {
                                                Username = employeeEntityAdmin1.Name.ToLower(),
                                                Password = EncodeUtil.MD5("123123"),
                                                Displayname = employeeEntityAdmin1.Name,
                                                Role = "Admin"
                                            };
                                            userEntity1 = _userRepository.Upsert(userEntity1);
                                        }
                                    }
                                }
                                if (userEntity1 != null && employeeEntityAdmin1 == null)
                                {
                                    departmentAdmin = _repositoryImp.GetByIdAndRole(departmentEntity.Id, userEntity1.Id, EnumDepartmentAdmin.Admin1, true);
                                }

                                if (departmentAdmin != null)
                                {
                                    departmentAdminEntity = departmentAdmin;
                                }
                                else
                                {
                                    departmentAdminEntity = new DepartmentAdminEntity();
                                    departmentAdminEntity.CreatedAt = DateTime.Now;
                                }
                                departmentAdminEntity.UpdatedAt = DateTime.Now;
                                departmentAdminEntity.DeletedFlag = false;
                                departmentAdminEntity.DepartmentId = departmentEntity?.Id;
                                departmentAdminEntity.UserId = userEntity1?.Id;
                                departmentAdminEntity.Role = EnumDepartmentAdmin.Admin1;
                                listAdminDepartment.Add(departmentAdminEntity);
                            }


                            //Admin 2
                            if ((userEntity2 != null && employeeEntityAdmin2 == null) || employeeEntityAdmin2 != null)
                            {
                                DepartmentAdminEntity departmentAdmin = new DepartmentAdminEntity();
                                if (employeeEntityAdmin2 != null)
                                {
                                    userEntity2 = _userRepository.GetByIdEmployee(employeeEntityAdmin2.EmployeeCode, true);
                                    if (userEntity2 != null)
                                    {
                                        departmentAdmin = _repositoryImp.GetByIdAndRole(departmentEntity.Id, userEntity2.Id, EnumDepartmentAdmin.Admin2, true);
                                    }
                                    else
                                    {
                                        if (userEntity2 == null)
                                        {
                                            userEntity2 = new UserEntity()
                                            {
                                                Username = employeeEntityAdmin2.Name.ToLower(),
                                                Password = EncodeUtil.MD5("123123"),
                                                Displayname = employeeEntityAdmin2.Name,
                                                Role = "Admin"
                                            };
                                            userEntity2 = _userRepository.Upsert(userEntity2);
                                        }
                                    }
                                }
                                if (userEntity2 != null && employeeEntityAdmin2 == null)
                                {
                                    departmentAdmin = _repositoryImp.GetByIdAndRole(departmentEntity.Id, userEntity2.Id, EnumDepartmentAdmin.Admin2, true);
                                }

                                if (departmentAdmin != null)
                                {
                                    departmentAdminEntity = departmentAdmin;
                                }
                                else
                                {
                                    departmentAdminEntity = new DepartmentAdminEntity();
                                    departmentAdminEntity.CreatedAt = DateTime.Now;
                                }
                                departmentAdminEntity.UpdatedAt = DateTime.Now;
                                departmentAdminEntity.DeletedFlag = false;
                                departmentAdminEntity.DepartmentId = departmentEntity?.Id;
                                departmentAdminEntity.UserId = userEntity2?.Id;
                                departmentAdminEntity.Role = EnumDepartmentAdmin.Admin2;
                                listAdminDepartment.Add(departmentAdminEntity);
                            }

                            //Admin 3 
                            if ((userEntity3 != null && employeeEntityAdmin3 == null) || employeeEntityAdmin3 != null)
                            {
                                DepartmentAdminEntity departmentAdmin = new DepartmentAdminEntity();
                                if (employeeEntityAdmin3 != null)
                                {
                                    userEntity3 = _userRepository.GetByIdEmployee(employeeEntityAdmin3.EmployeeCode, true);
                                    if (userEntity3 != null)
                                    {
                                        departmentAdmin = _repositoryImp.GetByIdAndRole(departmentEntity.Id, userEntity3.Id, EnumDepartmentAdmin.Admin3, true);
                                    }
                                    else
                                    {
                                        if (userEntity3 == null)
                                        {
                                            userEntity3 = new UserEntity()
                                            {
                                                Username = employeeEntityAdmin3.Name.ToLower(),
                                                Password = EncodeUtil.MD5("123123"),
                                                Displayname = employeeEntityAdmin3.Name,
                                                Role = "Admin"
                                            };
                                            userEntity3 = _userRepository.Upsert(userEntity3);
                                        }
                                    }
                                }
                                if (userEntity3 != null && employeeEntityAdmin3 == null)
                                {
                                    departmentAdmin = _repositoryImp.GetByIdAndRole(departmentEntity.Id, userEntity3.Id, EnumDepartmentAdmin.Admin3, true);
                                }

                                if (departmentAdmin != null)
                                {
                                    departmentAdminEntity = departmentAdmin;
                                }
                                else
                                {
                                    departmentAdminEntity = new DepartmentAdminEntity();
                                    departmentAdminEntity.CreatedAt = DateTime.Now;
                                }
                                departmentAdminEntity.UpdatedAt = DateTime.Now;
                                departmentAdminEntity.DeletedFlag = false;
                                departmentAdminEntity.DepartmentId = departmentEntity?.Id;
                                departmentAdminEntity.UserId = userEntity3?.Id;
                                departmentAdminEntity.Role = EnumDepartmentAdmin.Admin3;
                                listAdminDepartment.Add(departmentAdminEntity);
                            }
                            employeeAdmin1 = "";
                            employeeAdmin2 = "";
                            employeeAdmin3 = "";
                            employeeEntityAdmin1 = null;
                            employeeEntityAdmin2 = null;
                            employeeEntityAdmin3 = null;
                            departmentAdminEntity = null;
                            userEntity1 = null;
                            userEntity2 = null;
                            userEntity3 = null;

                        }
                    }

                    if (!isCheckFoundSheet)
                    {
                        return Messages.ErrNotFound_0_1.SetParameters("Not Found Sheet Data", "");
                    }
                }


                try
                {
                    foreach (var item in listAdminDepartment)
                    {
                        var departmentAdmin = _repositoryImp.GetByUserRoleDepartment(item.UserId, item.DepartmentId, item.Role, true);
                        if (departmentAdmin != null)
                        {
                            listAdminDepartmentChecked.Add(departmentAdmin);
                        }
                        else
                        {
                            listAdminDepartmentChecked.Add(item);
                        }
                    }

                    if (listAdminDepartmentChecked != null && listAdminDepartmentChecked.Count > 0)
                    {
                        _repositoryImp.UpSertMulti(listAdminDepartmentChecked);
                    }

                    if (request.IsReplace.GetValueOrDefault())
                    {
                        foreach (var item in listAdminDepartment)
                        {
                            var id = _repositoryImp.GetIdByUserRoleDepartment(item.UserId, item.DepartmentId, item.Role, true);
                            listIds.Add(id);
                        }

                        var listAdminOld = _repositoryImp.GetListNotExited(listIds);
                        _repositoryImp.DeleteMulti(listAdminOld);
                    }

                    await SetCurrentDepartmentToExcel();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch { }
                    throw new DBException(ex);
                }
            }

            return null;
        }

        public async Task<string> SetCurrentDepartmentToExcel(string path = null)
        {
            var templatePath = ConstMain.ConstTemplateFilePath;
            string reportTemplateSheet = Environment.GetEnvironmentVariable("AdminSheetName");
            using (var template = ExcelPro.LoadTempFile(templatePath))
            {
                var ws = template.Workbook.Worksheets[reportTemplateSheet];
                if (ws == null)
                {
                    throw new Exception("Invalid template");
                }
                ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel();
                exportSheetDataModel.SheetIndex = ws.Index;
                int columnIndex = 1;
                exportSheetDataModel.BeginRowIndex = 9;
                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(DepartmentAdminListmodel.STT));
                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(DepartmentAdminListmodel.Department));
                columnIndex = columnIndex + 4;
                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(DepartmentAdminListmodel.Admin1));
                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(DepartmentAdminListmodel.Admin2));
                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(DepartmentAdminListmodel.Admin3));

                // ******************* Get data **************************************************************** //
                var currentAdmins = await _departmentDepartmentRepository.GetCurrentAdmin();

                int count = 1;
                var datas = DepartmentAdminListmodel.SetData(currentAdmins)
                    .Select(x => new Dictionary<object, object>()
                {
                      { nameof(DepartmentAdminListmodel.STT), x.STT },
                      {nameof(DepartmentAdminListmodel.Department),x.Department },
                      {nameof(DepartmentAdminListmodel.Admin1),x.Admin1 },
                      {nameof(DepartmentAdminListmodel.Admin2),x.Admin2 },
                      {nameof(DepartmentAdminListmodel.Admin3),x.Admin3 },
                });
                var startRowData = 9;
                var insertEmptyRow = datas.Count() - 1;
                double rowHeight = 27.8;

                // clear datas before write 
                ws.Cells["B10:J1000"].Clear();
                // hide formular cells
                ws.Columns[ExcelPro.GetColumnNumber("K")].Hidden = true;
                ws.Columns[ExcelPro.GetColumnNumber("L")].Hidden = true;
                ws.Columns[ExcelPro.GetColumnNumber("M")].Hidden = true;
                ws.Columns[ExcelPro.GetColumnNumber("N")].Hidden = true;
                ws.Columns[ExcelPro.GetColumnNumber("O")].Hidden = true;
                // insert rows 
                if (insertEmptyRow > 0)
                {
                    ws.InsertRow(startRowData + 1, insertEmptyRow, startRowData);
                    for (int i = 0; i < insertEmptyRow; i++)
                    {
                        ws.Rows[startRowData + 1 + i].Height = rowHeight;
                    }
                }
                exportSheetDataModel.ListChartDataModel = datas.ToList();
                // write data to excel
                long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                                                template.Workbook.Worksheets,
                                                exportSheetDataModel);
                ws.View.SetTabSelected();
                if (path.IsNullOrEmpty())
                {
                    template.Save();
                }
                else
                {
                    template.SaveAs(path);
                }

                template.Dispose();
            }
            return path;
        }

        private string ProcessEmpCode(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                input = input.Split("     ")[0];
                var indexTitle = input.IndexOf("Chức vụ");
                if (indexTitle > -1)
                {
                    input = input.Substring(0, indexTitle);
                }
                input = input.Replace("Mã NV:", "").Trim();
            }
            return input;
        }
        private string ProcessAdminAccount(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                input = input.Split("     ")[0];
                var indexTitle = input.IndexOf("Chức vụ");
                if (indexTitle > -1)
                {
                    input = input.Substring(0, indexTitle);
                }
                input = input.Replace("Account:", "").Trim();
            }
            return input;
        }

    }
}