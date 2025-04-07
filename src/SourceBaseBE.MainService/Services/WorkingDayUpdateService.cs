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
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using iSoft.ExportLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.IO;
using iSoft.Common.CommonFunctionNS;
using static SourceBaseBE.MainService.ConstMain;
using NPOI.Util;
using SourceBaseBE.Database.Models.RequestModels.Report;
using iSoft.Redis.Services;

namespace SourceBaseBE.MainService.Services
{
    public class WorkingDayUpdateService : BaseCRUDService<WorkingDayUpdateEntity>
    {
        private UserRepository _authUserRepository;
        public WorkingDayUpdateRepository _repositoryImp;
        public LanguageRepository _languageRepository;
        public WorkingDayApprovalRepository _workingDayApprovalRepository;
        public WorkingDayRepository _workingDayRepository;
        public EmployeeRepository employeeRepository;
        public WorkingTypeRepository workingTypeRepository;
        public WorkingDayService workingDayService;
        public HolidayScheduleRepository holidayScheduleRepository;
        public UserService userService;
        /*[GEN-1]*/

        public WorkingDayUpdateService(CommonDBContext dbContext, ILogger<WorkingDayUpdateService> logger,
            WorkingDayService workingDayService
            , UserService userService)
          : base(dbContext, logger)
        {
            _repository = new WorkingDayUpdateRepository(_dbContext);
            _repositoryImp = (WorkingDayUpdateRepository)_repository;
            _authUserRepository = new UserRepository(_dbContext);
            _languageRepository = new LanguageRepository(_dbContext);
            _workingDayApprovalRepository = new WorkingDayApprovalRepository(_dbContext);
            _workingDayRepository = new WorkingDayRepository(_dbContext);
            employeeRepository = new EmployeeRepository(_dbContext);
            holidayScheduleRepository = new HolidayScheduleRepository(_dbContext);
            workingTypeRepository = new WorkingTypeRepository(_dbContext);
            this.workingDayService = workingDayService;
            this.userService = userService;
            /*[GEN-2]*/
        }
        public override WorkingDayUpdateEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repository.GetById(id, isDirect, isTracking);
            var entityRS = (WorkingDayUpdateEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override async Task<WorkingDayUpdateEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = await _repository.GetByIdAsync(id, isDirect);
            var entityRS = (WorkingDayUpdateEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override List<WorkingDayUpdateEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<WorkingDayUpdateEntity>().ToList();
            return listRS;
        }
        public List<WorkingDayUpdateEntity> GetList(List<long>? ids)
        {
            var list = _repositoryImp.GetList(ids);
            return list;
        }
        public List<Dictionary<string, object>> GetFormDataObjElement(WorkingDayUpdateEntity entity, long CurrentEditerId)
        {
            if (entity == null)
                entity = new WorkingDayUpdateEntity();
            string entityName = nameof(WorkingDayUpdateEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;
            var properties = typeof(WorkingDayUpdateEntity).GetProperties();
            bool addedFlag = false;
            foreach (var property in properties)
            {
                addedFlag = false;
                // foreignKeyAttribute
                var foreignKeyAttribute = (ForeignKeyAttribute)Attribute.GetCustomAttribute(property, typeof(ForeignKeyAttribute));
                // ListEntityAttribute
                var formDataTypeAttr = (FormDataTypeAttribute)Attribute.GetCustomAttribute(property, typeof(FormDataTypeAttribute));

                if (foreignKeyAttribute != null && !addedFlag && formDataTypeAttr == null)
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
          {"select_data", GetListOptionData(property.Name, entityName, "")},
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
                        case EnumFormDataType.hidden:
                            if (property.Name == "EditerId")
                            {
                                listRS.Add(new Dictionary<string, object> {
          {"display_name", GetDisplayName(property.Name, entityName)},
          {"key", property.Name},
          {"value",CurrentEditerId},
          {"type", formDataTypeAttr.TypeName.ToStringValue()},
        });

                            }
                            else
                            {
                                listRS.Add(new Dictionary<string, object> {
          {"display_name", GetDisplayName(property.Name, entityName)},
          {"key", property.Name},
          {"value",property.GetValue(entity)},
          {"type", formDataTypeAttr.TypeName.ToStringValue()},
        });
                            }

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
            return listRS.Where(x => x["display_name"] != null || x["key"].ToString().Contains("Id")).ToList();
        }

        /// <summary>
        /// UpsertIfNotExist (@GenCRUD)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override WorkingDayUpdateEntity Upsert(WorkingDayUpdateEntity entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((WorkingDayUpdateRepository)_repository).Upsert(entity/*[GEN-4]*/, userId);
            var entityRS = (WorkingDayUpdateEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
            return entityRS;
        }
        public override int Delete(long id, long? userId = null, bool isSoftDelete = true)
        {
            int deletedCount = _repository.Delete(id, userId, isSoftDelete);
            return deletedCount;
        }

        public List<WorkingDayUpdateEntity> DeleteMulti(List<WorkingDayUpdateEntity> timeSheetApprovals, long? userId = null, bool isSoftDelete = true)
        {
            var deletedEntities = _repositoryImp.DeleteMulti(timeSheetApprovals, userId, isSoftDelete).ToList();
            return deletedEntities;
        }
        /// <summary>
        /// GetListIdChildren (@GenCRUD)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="childEntity"></param>
        /// <returns></returns>
        private List<long> GetListIdChildren(WorkingDayUpdateEntity entity, string childEntity)
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
            targetEntity = targetEntity.Replace("Entity", "");
            switch (targetEntity)
            {
                case nameof(WorkingDayUpdateEntity.EmployeeId):
                    var listEmps = employeeRepository.GetAll();
                    listRS = listEmps.Select(x => new FormSelectOptionModel(x.Id, x.Name)).ToList();
                    break;
                case nameof(WorkingDayUpdateEntity.WorkingTypeId):
                    var listTypes = workingTypeRepository.GetAll();
                    listRS = listTypes.Select(x => new FormSelectOptionModel(x.Id, x.Name)).ToList();
                    break;
                case nameof(WorkingDayUpdateEntity.WorkingDayStatus):
                    var listStatus = Enum.GetValues<EnumWorkingDayStatus>();
                    listRS = listStatus.Select(x => new FormSelectOptionModel((int)x, x.ToString())).ToList();
                    break;
                /*[GEN-6]*/
                default:
                    break;
            }
            return listRS;
        }
        public async Task<PendingRequestPagingResponseModel> GetListPendingRequest(
          EnumApproveStatus enumApproveStatus,
          TotalReportListRequest pagingReq = null)
        {

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            var searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var employees = await employeeRepository.GetListAttendanceReport(pagingReq, filterParams, searchParams, sortParams);
            var ret = _repositoryImp.GetListPendingRequest(enumApproveStatus, employees.rawDatas, pagingReq);
            ret.TotalRecord = employees.TotalRecord;
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<WorkingdayUpdateDTO>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = WorkingdayUpdateDTO.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;

        }
        public async Task<PersonalPendingRequestPagingResponseModel> GetListPersonalPendingRequest(
          PersonalPendingRequest pagingReq = null)
        {

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            var searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var ret = await _repositoryImp.GetPersonalPendingRequest(pagingReq.EmployeeId.GetValueOrDefault(), pagingReq, filterParams, searchParams, sortParams);
            var disPlayProps = JsonPropertyHelper<DetailWorkingdayUpdateDTO>.GetFilterProperties();
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = DetailWorkingdayUpdateDTO.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;

        }

        public async Task<HistoricalPendingRequestPagingResponseModel> GetListHistoricalRequest(
          HistoricalPendingRequest pagingReq = null)
        {

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            var searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var ret = await _repositoryImp.GetHistoricalPendingRequest(pagingReq.EmployeeId.GetValueOrDefault(), pagingReq, filterParams, searchParams, sortParams);
            var disPlayProps = JsonPropertyHelper<HistoricalWorkingdayUpdateDTO>.GetFilterProperties();
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = HistoricalWorkingdayUpdateDTO.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;

        }
        public async Task<List<WorkingDayUpdateEntity>> ChangeStateWorkingdayUpdate(
          EditPersonPendingRequest request = null)
        {
            List<WorkingDayUpdateEntity> listRet = new List<WorkingDayUpdateEntity>();

            foreach (var wdid in request.ListWorkingDayUpdateId)
            {
                var wdU = await _repositoryImp.GetByIdWithWdAAsync(wdid);
                var wd = wdU?.WorkingDay;
                var approver = await _authUserRepository.GetByIdAsync(request.UserId.GetValueOrDefault());
                if (wdU == null) throw new Exception($"WORKINGDAY UPDATE WITH ID {wdid} NOT FOUND ");
                if (approver == null) throw new Exception($"USER APPROVE WITH ID {request.UserId} NOT FOUND ");
                //var lastemployeeOTHours = 0;
                var workingTypesOT = await workingTypeRepository.GetOTTypes();
                var holidays = holidayScheduleRepository.GetList();
                var incommingOTHours = 0;
                if (wd == null)
                {
                    wd = await workingDayService.GetByIdOrDate(wdU.WorkingDayId.GetValueOrDefault(), wdU.EmployeeId.GetValueOrDefault(), wdU.WorkingDate.GetValueOrDefault());
                    if (wd == null)
                    {
                        wd = new WorkingDayEntity();
                        wd.WorkingDate = wdU.WorkingDate;
                        wd.EmployeeEntityId = wdU.EmployeeId;
                    }
                }
                var incomming = wd.Copy();
                incomming.WorkingTypeEntityId = wdU.WorkingTypeId;
                switch (request.Status.GetValueOrDefault())
                {
                    case EnumApproveStatus.ACCEPT:
                        // check if admin assign meal type
                        if (wdU.ActionRequest == EnumActionRequest.Delete)
                        {
                            if (wd != null)
                            {
                                workingDayService.Delete(wd.Id, approver.Id);
                                wd.DeletedFlag = true;
                            }
                        }
                        else
                        {
                            if (wdU.WorkingType != null && wdU.WorkingType.Code.Contains("M"))
                            {
                                var listEmpWkingInDay = await _workingDayRepository.GetWorkingDayByDate(wdU.WorkingDate.GetValueOrDefault());
                                foreach (var emp in listEmpWkingInDay)
                                {
                                    emp.WorkingTypeEntityId = workingTypesOT.FirstOrDefault(x => x.Code == emp.WorkingType.Code + "M")?.Id;
                                    emp.UpdatedAt = DateTime.Now;
                                    emp.Notes = $"Update To Meal because of {request.UserId}";
                                    _workingDayRepository.Upsert(emp);
                                }
                            }
                            //
                            wd.WorkingDate = wdU.WorkingDate;
                            wd.Notes = wdU.Notes;
                            wd.TimeDeviation = wdU.Time_Deviation != null ? (double)wdU.Time_Deviation : wd.TimeDeviation;
                            wd.Time_In = wdU.Time_In;
                            wd.Time_Out = wdU.Time_Out;
                            wd.UpdatedBy = wdU.UpdatedBy;
                            wd.UpdatedAt = DateTime.Now;
                            wd.WorkingTypeEntityId = wdU.WorkingTypeId;
                            if (wdU.WorkingDayStatus != null)
                            {
                                wd.WorkingDayStatus = wdU.WorkingDayStatus;
                            }
                            else
                            {
                                if (wdU.Time_In != null || wdU.Time_Out != null)
                                {
                                    wd.WorkingDayStatus = EnumWorkingDayStatus.Attended;
                                }
                            }
                            wd.DeletedFlag = false;
                            // update working day if accepted
                            wd = _workingDayRepository.Upsert(wd);
                        }

                        break;
                    case EnumApproveStatus.REJECT:
                        wd.WorkingDate = wdU.OriginalWorkDate;
                        wd.Notes = wdU.Notes;
                        wd.TimeDeviation = wdU.OriginTimeDeviation;
                        wd.Time_In = wdU.OriginTimeIn;
                        wd.Time_Out = wdU.OriginTimeOut;
                        wd.UpdatedBy = request.UserId;
                        wd.UpdatedAt = DateTime.Now;
                        wd.WorkingTypeEntityId = wdU.OriginWorkingTypeId;
                        wd.WorkingDayStatus = wdU.OriginWorkingDayStatus;
                        // update working day if accepted
                        wd = _workingDayRepository.Upsert(wd);
                        break;
                    default:
                        break;
                }
                // add working day approval logs
                var wdA = wdU.WorkingDayApprovals.FirstOrDefault();
                if (wdA == null) wdA = new WorkingDayApprovalEntity();
                wdA.ApproverId = approver.Id;
                //wdA.WorkingDayId = wdU.WorkingDayId;
                wdA.ApproveStatus = request.Status.GetValueOrDefault();
                wdA.Notes = string.IsNullOrWhiteSpace(request.Note) ? request.Note : wdA.Notes;
                wdA.Approve_Reason = string.IsNullOrWhiteSpace(request.ApproveReason) ? request.ApproveReason : wdA.Approve_Reason;
                wdA.UpdatedAt = DateTime.Now;
                wdA.CreatedBy = wdA.CreatedBy == null ? approver.Id : wdA.CreatedBy;
                wdA.CreatedAt = wdA.CreatedAt == null ? DateTime.Now : wdA.CreatedAt;
                wdA.UpdatedBy = approver.Id;
                wdA.WorkingDayId = wd?.Id;
                // 
                wdU.WorkingDayId = wd?.Id;
                _repositoryImp.Update(wdU);
                _workingDayApprovalRepository.Upsert(wdA);
                listRet.Add(wdU);
            }
            CachedFunc.ClearRedisByEntity(nameof(WorkingDayEntity));
            CachedFunc.ClearRedisByEntity(nameof(WorkingDayUpdateEntity));
            CachedFunc.ClearRedisByEntity(nameof(WorkingDayApprovalEntity));
            return listRet;

        }
        public async Task<IActionResult> ExportEmployeePendingRequest(ExportEmployeePendingRequest request)
        {
            if (request == null || request.EmployeeId == null) throw new ArgumentNullException(nameof(request));
            var templatePath = "./wwwroot/ReportTemplate/templatePendingRequest.xlsx";
            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {

                    // query datas
                    var datas = await _repositoryImp.GetEmployeePendingRequest(request.EmployeeId.GetValueOrDefault(), request.DateFrom.GetValueOrDefault(), request.DateTo.GetValueOrDefault());
                    if (datas == null || datas.Count <= 0) return null;
                    var templatePackage = ExcelPro.LoadTempFile(templatePath);
                    var startRow = 9;

                    var startStt = 1;
                    foreach (var data in datas)
                    {
                        var startCol = 1;
                        // stt
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, startStt++.ToString());
                        // submiter
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.Editer?.ItemEmployee?.Name);
                        // employee
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.WorkingDay?.Employee.Name);
                        // origin time in
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.OriginTimeIn.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm:ss"));
                        // origin time out
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.OriginTimeOut.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm:ss"));
                        // origin time deviation
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.OriginTimeDeviation.ToString());
                        // origin type
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.OriginWorkingType?.Name);
                        // origin status
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.OriginWorkingDayStatus?.ToString());
                        //  time in
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.Time_In.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm:ss"));
                        //  time out
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.Time_Out.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm:ss"));
                        //  time deviation
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.Time_Deviation.ToString());
                        //  type
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.WorkingType?.Name);
                        //  status
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.WorkingDayStatus?.ToString());

                        //  reason
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.Update_Reason);
                        //  note
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.Notes);
                        //  create at
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.CreatedAt.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm"));
                        //   update at
                        ExcelPro.SetValue(ref templatePackage, "Pending Request", startRow, startCol++, data.UpdatedAt.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm"));
                        startRow++;
                    }
                    //
                    // zip file
                    var path = $"./ReportPendingRequest_{request.DateFrom.GetValueOrDefault().ToString("dd-MM-yyyy")}To{request.DateTo.GetValueOrDefault().ToString("dd-MM-yyyy")}_Employees_{datas[0].WorkingDay.Employee.Name}_.xlsx";
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    templatePackage.SaveAs(new FileInfo(path));

                    var fileInArchive = archive.CreateEntry(path, System.IO.Compression.CompressionLevel.Optimal);
                    using (var entryStream = fileInArchive.Open())
                    {
                        using (var fileInCompression = new MemoryStream(File.ReadAllBytes(path)))
                        {
                            await fileInCompression.CopyToAsync(entryStream);
                        }
                    }
                    // delete when response done
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                return new FileContentResult(outStream.ToArray(), "application/zip");
            }
        }
    }
}