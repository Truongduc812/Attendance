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
using SourceBaseBE.Database.Helpers;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels.Report;
using SourceBaseBE.Database.Models.ResponseModels;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Models.ResponseModels.Report;
using iSoft.Redis.Services;
using iSoft.Common.Utils;

namespace SourceBaseBE.MainService.Services
{
    public class TimeSheetUpdateService : BaseCRUDService<TimeSheetUpdateEntity>
    {
        private UserRepository _userRepository;
        public TimeSheetUpdateRepository _repositoryImp;
        public EmployeeRepository employeeRepository;
        public EmployeeService employeeService;
        public DepartmentAdminService departmentAdminService;
        public LanguageRepository _languageRepository;
        public TimeSheetService timeSheetService;
        public TimeSheetApprovalService approvalService;
        public WorkingDayService workingDayService;
        TimeSheetRepository timeSheetRepository;
        WorkingTypeRepository workingTypeRepository;
        WorkingDayRepository _workingDayRepository;
        /*[GEN-1]*/

        public TimeSheetUpdateService(CommonDBContext dbContext,
            ILogger<TimeSheetUpdateService> logger,
            DepartmentAdminService departmentAdminService,
            TimeSheetService timeSheetService,
            TimeSheetApprovalService approvalService,
            WorkingDayService workingDayService,
            EmployeeService employeeService
            )
          : base(dbContext, logger)
        {
            _repository = new TimeSheetUpdateRepository(_dbContext);
            _repositoryImp = (TimeSheetUpdateRepository)_repository;
            timeSheetRepository = new TimeSheetRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);
            employeeRepository = new EmployeeRepository(_dbContext);
            _languageRepository = new LanguageRepository(_dbContext);
            workingTypeRepository = new WorkingTypeRepository(_dbContext);
            _workingDayRepository = new WorkingDayRepository(_dbContext);
            this.departmentAdminService = departmentAdminService;
            this.timeSheetService = timeSheetService;
            this.approvalService = approvalService;
            this.workingDayService = workingDayService;
            this.employeeService = employeeService;
            /*[GEN-2]*/
        }
        public override TimeSheetUpdateEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repository.GetById(id, isDirect, isTracking);
            var entityRS = (TimeSheetUpdateEntity)_userRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override async Task<TimeSheetUpdateEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = await _repository.GetByIdAsync(id, isDirect);
            var entityRS = (TimeSheetUpdateEntity)_userRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override List<TimeSheetUpdateEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _userRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<TimeSheetUpdateEntity>().ToList();
            return listRS;
        }
        public List<TimeSheetUpdateEntity> GetList(List<long>? ids)
        {
            var list = _repositoryImp.GetList(ids);
            return list;
        }
        public override List<Dictionary<string, object>> GetFormDataObjElement(TimeSheetUpdateEntity entity)
        {
            string entityName = nameof(TimeSheetUpdateEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;
            var properties = typeof(TimeSheetUpdateEntity).GetProperties();
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
        public override TimeSheetUpdateEntity Upsert(TimeSheetUpdateEntity entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((TimeSheetUpdateRepository)_repository).Upsert(entity/*[GEN-4]*/, userId);
            var entityRS = (TimeSheetUpdateEntity)_userRepository.FillTrackingUser(upsertedEntity);
            return entityRS;
        }
        public override int Delete(long id, long? userId = null, bool isSoftDelete = true)
        {
            int deletedCount = _repository.Delete(id, userId, isSoftDelete);
            return deletedCount;
        }

        public List<TimeSheetUpdateEntity> DeleteMulti(List<TimeSheetUpdateEntity> timeSheetApprovals, long? userId = null, bool isSoftDelete = true)
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
        private List<long> GetListIdChildren(TimeSheetUpdateEntity entity, string childEntity)
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

        //public TimeSheetUpdateEntity UpsertTestTransaction(TimeSheetUpdateEntity entity, long? userId = null)
        //{
        //  using (var transaction = this._dbContext.Database.BeginTransaction())
        //  {
        //    try
        //    {
        //      _repository...
        //      _userRepository...
        //      transaction.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //      try
        //      {
        //        transaction.Rollback();
        //      }
        //      catch { }
        //      throw new DBException(ex);
        //    }
        //  }
        //}
        public Task<TimeSheetUpdateEntity> GetByIdWithRelation(long id)
        {
            return _repositoryImp.GetByIdWithRelationAsync(id);
        }
        public Task<List<TimeSheetUpdateEntity>> GetListUpdate(List<long> ids)
        {
            return _repositoryImp.GetByListIds(ids, true, true);
        }
        public async Task<List<TimeSheetUpdateEntity>> ChangeState(
            List<TimeSheetUpdateEntity> listUpdates,
            long currentUserId,
            EditTimeSheetPendingRequest request,
            List<long> listAllowDepartmentId)
        {
            using (var transaction = this._dbContext.Database.BeginTransaction())
            {
                try
                {
                    EmployeeEntity empployee = null;
                    TimeSheetEntity timesheet = null;
                    bool updateWorkingDayFlag = false;
                    bool isReject = false;
                    List<long> listWorkingDayId = new List<long>();

                    foreach (var timeSheetUpdate in listUpdates)
                    {
                        empployee = timeSheetUpdate.Employee;
                        if (empployee == null)
                        {
                            throw new Exception("EMPLOYEE NOT FOUND!");
                        }
                        if (empployee.DepartmentId != null && !listAllowDepartmentId.Contains(empployee.DepartmentId.Value))
                        {
                            throw new Exception("NO PERMISSION!");
                        }
                        timesheet = timeSheetUpdate.TimeSheetEntity;
                        switch (request.Status.Value)
                        {
                            case EnumApproveStatus.ACCEPT:
                                if (timeSheetUpdate.ActionRequest == EnumActionRequest.Delete && timesheet != null)
                                {
                                    timeSheetRepository.Delete(timesheet, currentUserId);
                                }
                                if (timeSheetUpdate.ActionRequest == EnumActionRequest.Create || (timeSheetUpdate.ActionRequest == EnumActionRequest.Edit))
                                {
                                    if (timesheet == null)
                                    {
                                        timesheet = new TimeSheetEntity();
                                    }
                                    timesheet.RecordedTime = timeSheetUpdate.RecordedTime;
                                    timesheet.EmployeeId = timeSheetUpdate.EmployeeId.GetValueOrDefault();
                                    timesheet.Status = timeSheetUpdate.Status.GetValueOrDefault();
                                    timesheet = timeSheetRepository.Upsert(timesheet);
                                    updateWorkingDayFlag = true;
                                }
                                break;
                            case EnumApproveStatus.REJECT:
                                isReject = true;
                                break;
                            default:
                                break;
                        }
                        var approve = timeSheetUpdate.TimeSheetApprovalEntities?.FirstOrDefault();
                        if (approve != null)
                        {
                            approve.Notes = request.ApproveReason;
                            approve.UpdatedAt = DateTime.Now;
                            approve.UserEntityId = currentUserId;
                            approve.Status = request.Status.GetValueOrDefault();
                            approvalService.Upsert(approve);
                        }

                        if (isReject)
                        {
                            continue;
                        }
                        WorkingDayEntity wd = _workingDayRepository.GetListWorkingDayByEmployeeId3(empployee.Id, timesheet.RecordedTime.Value, true).FirstOrDefault();

                        if (wd == null)
                        {
                            wd = new WorkingDayEntity()
                            {
                                EmployeeEntityId = timesheet.EmployeeId,
                                WorkingDate = timesheet.RecordedTime,
                                CreatedAt = DateTime.Now,
                                CreatedBy = currentUserId,
                            };
                        }
                        else
                        {
                            wd.UpdatedAt = DateTime.Now;
                            wd.UpdatedBy = currentUserId;

                            //if (!DateTimeHelper.IsSameDay(wd.WorkingDate.Value, timesheet.RecordedTime.Value))
                            //{
                            //    throw new Exception($"Timesheet must have same day with Working day,Differ between {wd.WorkingDate} and {timeSheetUpdate.RecordedTime}");
                            //}
                        }

                        wd = workingDayService.Upsert(wd);
                        listWorkingDayId.Add(wd.Id);

                        timesheet.WorkingDayId = wd.Id;
                        timesheet = timeSheetRepository.Upsert(timesheet);

                        //}
                    }

                    // after upsert => recalculate working type of working day
                    var wkTypes = workingTypeRepository.GetList();

                    if (listWorkingDayId.Count >= 1)
                    {
                        listWorkingDayId = ConvertUtil.RemoveDuplicate(listWorkingDayId);

                        foreach (var workingDayId in listWorkingDayId)
                        {
                            List<TimeSheetEntity> listTimeSheet = await this.timeSheetService.GetListByWdId(workingDayId, true);
                            workingDayService.ReCalculate(listTimeSheet, workingDayId, wkTypes, true);
                        }
                    }

                    Dictionary<string, bool> dicExistsFlag = new Dictionary<string, bool>();

                    foreach (var update in listUpdates)
                    {
                        var time = update.RecordedTime.Value;
                        var datefrom = new DateTime(time.Year, time.Month, 1);
                        var dateto = new DateTime(time.Year, time.Month, DateTime.DaysInMonth(time.Year, time.Month));
                        string key = $"{time.Year}_{time.Month}";

                        if (!dicExistsFlag.ContainsKey(key))
                        {
                            dicExistsFlag.Add(key, true);
                            employeeService.CalculateEmpWkType(update.EmployeeId.GetValueOrDefault(), time, wkTypes);
                        }
                    }

                    transaction.Commit();
                    return listUpdates;
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
        }
        public async Task<TimeSheetPendingRequestPagingResponseModel> GetListPendingRequest(
        EnumApproveStatus enumActionRequest,
        TotalReportListRequest pagingReq = null)
        {

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            var searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var employees = await employeeRepository.GetListAttendanceReport(pagingReq, filterParams, searchParams, sortParams);
            var ret = _repositoryImp.GetListPendingRequest(enumActionRequest, employees.rawDatas, pagingReq);
            ret.TotalRecord = employees.TotalRecord;
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<WorkingdayUpdateDTO>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = WorkingdayUpdateDTO.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;

        }
        public async Task<PersonalTimeSheetPendingRequestPagingResponseModel> GetListPersonalPendingRequest(
         PersonalPendingRequest pagingReq = null)
        {

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            var searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var ret = await _repositoryImp.GetPersonalPendingRequest(pagingReq.EmployeeId.GetValueOrDefault(), pagingReq, filterParams, searchParams, sortParams);
            var disPlayProps = JsonPropertyHelper<DetailTimeSheetUpdateDTO>.GetFilterProperties();
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = DetailTimeSheetUpdateDTO.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;
        }

        public async Task<HistoricalTimeSheetPendingRequestPagingResponseModel> GetListHistoricalPendingRequest(
         PersonalPendingRequest pagingReq = null)
        {

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            var searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var ret = await _repositoryImp.GetHistoricalPendingRequest(pagingReq.EmployeeId.GetValueOrDefault(), pagingReq, filterParams, searchParams, sortParams);
            var disPlayProps = JsonPropertyHelper<HistoricalTimeSheetUpdate>.GetFilterProperties();
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = HistoricalTimeSheetUpdate.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;
        }

    }
}