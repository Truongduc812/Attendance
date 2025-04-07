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
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Models.RequestModels;
using Azure;
using Nest;
using iSoft.Common.Enums;

namespace SourceBaseBE.MainService.Services
{
    public class MasterDataService : BaseCRUDService<MasterDataEmployeeEntity>
    {
        private UserRepository _authUserRepository;
        public MasterDataRepository _repositoryImp;
        public DepartmentRepository _departmentRepository;
        public JobTitleRepository _jobTitleRepository;
        public WorkingDayRepository _workingDayRepository;
        public WorkingTypeRepository workingTypeRepository;
        public DepartmentAdminRepository departmentAdminRepository;
        public EmployeeRepository employeeRepository;

        /*[GEN-1]*/

        public MasterDataService(CommonDBContext dbContext, ILogger<MasterDataService> logger)
          : base(dbContext, logger)
        {
            _repository = new MasterDataRepository(_dbContext);
            _repositoryImp = (MasterDataRepository)_repository;
            _authUserRepository = new UserRepository(_dbContext);
            _departmentRepository = new DepartmentRepository(_dbContext);
            _jobTitleRepository = new JobTitleRepository(_dbContext);
            _workingDayRepository = new WorkingDayRepository(_dbContext);
            workingTypeRepository = new WorkingTypeRepository(_dbContext);
            departmentAdminRepository = new DepartmentAdminRepository(_dbContext);
            employeeRepository = new EmployeeRepository(_dbContext);
            /*[GEN-2]*/
        }
        public override MasterDataEmployeeEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repository.GetById(id, isDirect, isTracking);
            var entityRS = (MasterDataEmployeeEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override async Task<MasterDataEmployeeEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = await _repository.GetByIdAsync(id, isDirect);
            var entityRS = (MasterDataEmployeeEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override List<MasterDataEmployeeEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<MasterDataEmployeeEntity>().ToList();
            return listRS;
        }
        public override List<Dictionary<string, object>> GetFormDataObjElement(MasterDataEmployeeEntity entity)
        {
            string entityName = nameof(MasterDataEmployeeEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;
            var properties = typeof(MasterDataEmployeeEntity).GetProperties();
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
        public override MasterDataEmployeeEntity Upsert(MasterDataEmployeeEntity entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((MasterDataRepository)_repository).Upsert(entity/*[GEN-4]*/, userId);
            var entityRS = (MasterDataEmployeeEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
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
        private List<long> GetListIdChildren(MasterDataEmployeeEntity entity, string childEntity)
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
            switch (targetEntity)
            {

                /*[GEN-6]*/
                default:
                    break;
            }
            return listRS;
        }

        public MasterDataHomeReponseModel GetMasterHomeDatas(MasterDataHomeRequestModel reqParams)
        {
            try
            {
                var departments = reqParams?.Department == true ? this._departmentRepository.GetAll() : null;
                if (departments != null && departments.Any())
                {
                    var unknonwDepartmentEntity = _departmentRepository.GetByName("Unknown"); // Replace with the ID or condition of the specific item
                    if (unknonwDepartmentEntity != null)
                    {
                        var specificItem = departments.FirstOrDefault(x => x.Id == unknonwDepartmentEntity.Id);
                        if (specificItem != null)
                        {
                            departments.Remove(specificItem);
                            departments.Add(specificItem);
                        }
                    }
                }
                var jobTitles = reqParams?.JobTitle == true ? this._jobTitleRepository.GetAll() : null;



                var response = new MasterDataHomeReponseModel();
                List<MasterStatusReponseModel> statuss = new List<MasterStatusReponseModel>();

                //* read master status enum
                if (reqParams?.Status == true)
                {
                    foreach (var item in Enum.GetValues(typeof(EnumInOutTypeStatus)))
                    {
                        EnumInOutTypeStatus status = (EnumInOutTypeStatus)item;
                        long value = (long)status;
                        statuss.Add(new MasterStatusReponseModel()
                        {
                            Id = value.ToString(),
                            Name = item.ToString(),
                        });
                    }
                }
                response.Departments = departments?.Select(d => new MasterDataResponseModel()
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                }).ToList();

                response.JobTitles = jobTitles?.Select(d => new MasterDataResponseModel()
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                }).ToList();

                response.Statuss = statuss;


                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public MasterDataSettingReponseModel GetMasterDatasFunctionPage(MasterDataRequestModel reqParams)
        {
            try
            {
                var jobTitles = reqParams?.JobTitle == true ? this._jobTitleRepository.GetAll() : null;
                var response = new MasterDataFunctionSettingReponseModel();
                var contractType = new List<MasterStatusReponseModel>();
                if (reqParams?.Contract == true)
                {
                    foreach (var item in Enum.GetValues(typeof(EnumTypeContract)))
                    {
                        EnumTypeContract status = (EnumTypeContract)item;
                        long value = (long)status;
                        contractType.Add(new MasterStatusReponseModel()
                        {
                            Id = value.ToString(),
                            Name = item.ToString(),
                        });
                    }
                }
                response.JobTitles = jobTitles?.Select(d => new MasterDataResponseModel()
                {
                    Id = d.Id.ToString(),
                    Name = d.Name
                }).ToList();

                response.Contracts = contractType;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public MasterDataFunctionSettingReponseModel GetMasterDatasAdmin(MasterDataAdminRequestModel reqParams)
        {
            try
            {
                var role = new List<MasterDataResponseModel>();
                var jobTitles = reqParams?.JobTitle == true ? this._jobTitleRepository.GetAll() : null;

                //* read master status enum
                //if (reqParams?.Role == true)
                //{
                //	foreach (var item in Enum.GetValues(typeof(EnumDepartmentAdmin)))
                //	{
                //		if(item.ToString() == EnumDepartmentAdmin.None.ToString())
                //		{
                //			continue;
                //		}
                //        EnumFaceId status = (EnumFaceId)item;
                //        long value = (long)status;
                //        role.Add(new MasterDataResponseModel()
                //        {
                //          Id = value.ToString(),
                //          Name = item.ToString(),
                //        });
                //      }
                //}
                var response = new MasterDataFunctionSettingReponseModel();
                response.JobTitles = jobTitles?.Select(d => new MasterDataResponseModel()
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                }).ToList();
                response.Roles = role.Select(d => new MasterDataResponseModel()
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                }).ToList();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public MasterDataEmployeeReportReponseModel GetMasterEmployeeReportDatas(MasterDataRequestModel reqParams)
        {
            try
            {
                var departments = reqParams?.Department == true ? this._departmentRepository.GetAll() : null;
                var jobTitles = reqParams?.JobTitle == true ? this._jobTitleRepository.GetAll() : null;

                //* read master status enum

                var response = new MasterDataEmployeeReportReponseModel();
                response.Departments = departments?.Select(d => new MasterDataResponseModel()
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                }).ToList();
                response.JobTitles = jobTitles?.Select(d => new MasterDataResponseModel()
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                }).ToList();
                if (reqParams.Type == true)
                {
                    response.Type = new List<MasterDataResponseModel>();
                    response.Type.Add(new MasterDataResponseModel()
                    {
                        Id = "0",
                        Name = "Current",
                    });
                    response.Type.Add(new MasterDataResponseModel()
                    {
                        Id = "1",
                        Name = "Recommend",
                    });
                }

                var contractType = new List<MasterStatusReponseModel>();
                if (reqParams?.Contract == true)
                {
                    foreach (var item in Enum.GetValues(typeof(EnumTypeContract)))
                    {
                        EnumTypeContract status = (EnumTypeContract)item;
                        long value = (long)status;
                        contractType.Add(new MasterStatusReponseModel()
                        {
                            Id = value.ToString(),
                            Name = item.ToString(),
                        });
                    }
                }
                response.Contracts = contractType;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public MasterDataReportReponseModel GetMasterDetailReportDatas(MasterDataReportRequestModel reqParams)
        {
            try
            {
                //var departmentAdmin1s = reqParams?.Requester == true ? this._workingDayRepository.GetAll() : null;
                var Response = new MasterDataReportReponseModel();
                var statusWorkingdate = new List<MasterStatusReponseModel>();

                //* read master status enum
                if (reqParams?.Status == true)
                {
                    foreach (var item in Enum.GetValues(typeof(EnumWorkingDayStatus)))
                    {
                        EnumWorkingDayStatus status = (EnumWorkingDayStatus)item;
                        long value = (long)status;
                        statusWorkingdate.Add(new MasterStatusReponseModel()
                        {
                            Id = value.ToString(),
                            Name = item.ToString(),
                        });
                    }
                }

                var statusTypeWorkingdate = new List<MasterStatusReponseModel>();
                //* read master status enum

                if (reqParams?.Status == true)
                {
                    foreach (var item in Enum.GetValues(typeof(EnumWorkingTypeCode)))
                    {
                        EnumWorkingTypeCode status = (EnumWorkingTypeCode)item;
                        long value = (long)status;
                        statusTypeWorkingdate.Add(new MasterStatusReponseModel()
                        {
                            Id = value.ToString(),
                            Name = item.ToString(),
                        });
                    }
                }
                Response.Statuss = statusWorkingdate;
                Response.Types = statusTypeWorkingdate;
                return Response;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<MasterDataPendingRequestReponseModel> GetMasterPendingRequestDatas(MasterDataTotalPendingRequestModel reqParams)
        {
            try
            {
                //var departmentAdmin1s = reqParams?.Requester == true ? this._workingDayRepository.GetAll() : null;
                var Response = new MasterDataPendingRequestReponseModel();
                var employeeNames = new List<MasterDataResponseModel>();

                var departments = new List<MasterDataResponseModel>();
                //* read master editer 

                if (reqParams?.Department == true)
                {
                    var departs = _departmentRepository.GetAll();
                    foreach (var item in departs)
                    {
                        departments.Add(new MasterDataResponseModel()
                        {
                            Id = item.Id.ToString(),
                            Name = item.Name.ToString(),
                        });
                    }
                }
                //* read master job title 
                var titlesList = new List<MasterDataResponseModel>();
                if (reqParams?.JobTitle == true)
                {
                    var titles = _jobTitleRepository.GetAll();
                    foreach (var item in titles)
                    {
                        titlesList.Add(new MasterDataResponseModel()
                        {
                            Id = item.Id.ToString(),
                            Name = item.Name.ToString(),
                        });
                    }
                }
                //
                Response.Departments = departments;
                Response.Titles = titlesList;
                return Response;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MasterDataDetailPendingRequestReponseModel> GetMasterDetailPendingRequestDatas(MasterDataDetailPendingRequestModel reqParams)
        {
            try
            {
                //var departmentAdmin1s = reqParams?.Requester == true ? this._workingDayRepository.GetAll() : null;
                var Response = new MasterDataDetailPendingRequestReponseModel();

                var editer = new List<MasterDataResponseModel>();
                //* read master editer 

                if (reqParams?.Editer == true)
                {
                    var employeeDepartment = employeeRepository.GetById(reqParams.EmployeeId.GetValueOrDefault());
                    if (employeeDepartment == null)
                        throw new Exception($"Employee with id {reqParams.EmployeeId} NOT FOUND");
                    var departmentAdmin1s = await departmentAdminRepository.GetDepartmentAdmin(employeeDepartment.DepartmentId, EnumDepartmentAdmin.Admin1);
                    foreach (var item in departmentAdmin1s)
                    {
                        editer.Add(new MasterDataResponseModel()
                        {
                            Id = item.UserId.ToString(),
                            Name = item.User.ItemEmployee.Name.ToString(),
                        });
                    }
                }
                //* read master job title 
                var status = new List<MasterDataResponseModel>();
                if (reqParams?.ApproveStatus == true)
                {
                    var workingStatus = Enum.GetNames(typeof(EnumApproveStatus));
                    foreach (var item in workingStatus)
                    {
                        status.Add(new MasterDataResponseModel()
                        {
                            Id = ((int)(Enum.Parse<EnumApproveStatus>(item))).ToString(),
                            Name = item.ToString(),
                        });
                    }
                }
                //
                Response.Requester = editer;
                Response.Satuss = status;
                return Response;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public MasterDataReportReponseModel GetMasterTimesSheetDatas(bool Status)
        {
            try
            {
                //* read master status enum

                var response = new MasterDataReportReponseModel();
                var statusMaster = new List<MasterStatusReponseModel>();
                if (Status)
                {
                    foreach (var item in Enum.GetValues(typeof(EnumFaceId)))
                    {
                        EnumFaceId status = (EnumFaceId)item;
                        long value = (long)status;
                        statusMaster.Add(new MasterStatusReponseModel()
                        {
                            Id = value.ToString(),
                            Name = item.ToString(),
                        });
                    }
                }
                response.Statuss = statusMaster;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public MasterDataReportReponseModel GetMasterFilterStatus(bool Status)
        {
            try
            {
                //* read master status enum

                var response = new MasterDataReportReponseModel();
                var statusMaster = new List<MasterStatusReponseModel>();
                if (Status)
                {
                    foreach (var item in Enum.GetValues(typeof(EnumWorkingDayStatus)))
                    {
                        EnumWorkingDayStatus status = (EnumWorkingDayStatus)item;
                        long value = (long)status;
                        statusMaster.Add(new MasterStatusReponseModel()
                        {
                            Id = value.ToString(),
                            Name = item.ToString(),
                        });
                    }
                }
                response.Statuss = statusMaster;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public MasterDataReportReponseModel GetMasterFilterRequest(bool Status)
        {
            try
            {
                //* read master status enum

                var response = new MasterDataReportReponseModel();
                var statusMaster = new List<MasterStatusReponseModel>();
                if (Status)
                {
                    foreach (var item in Enum.GetValues(typeof(EnumActionRequest)))
                    {
                        EnumActionRequest status = (EnumActionRequest)item;
                        long value = (long)status;
                        statusMaster.Add(new MasterStatusReponseModel()
                        {
                            Id = value.ToString(),
                            Name = item.ToString(),
                        });
                    }
                }
                response.Statuss = statusMaster;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}