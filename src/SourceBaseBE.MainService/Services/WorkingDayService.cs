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
using SourceBaseBE.Database.Enums;
using iSoft.Database.Extensions;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Database.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.ResponseModels;
using iSoft.Common.Utils;
using SourceBaseBE.Database.Models.SpecialModels;
using iSoft.ExportLibrary.Models;
using iSoft.ExportLibrary.Services;
using System.IO.Compression;
using System.IO;
using SourceBaseBE.MainService.Models;
using SourceBaseBE.Database.EnumProject;
using iSoft.Redis.Services;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Enums;
using NPOI.SS.Formula.Eval;
using Nest;
using SourceBaseBE.Database.Models.RequestModels.Report;
using OfficeOpenXml;
using SourceBaseBE.Database;
using SourceBaseBE.MainService.CommonFuncNS;
using elFinder.NetCore.Models;

namespace SourceBaseBE.MainService.Services
{
    public class WorkingDayService : BaseCRUDService<WorkingDayEntity>
    {
        private UserRepository _authUserRepository;
        public WorkingDayRepository _repositoryImp;
        public LanguageRepository _languageRepository;
        public EmployeeRepository _employeeRepository;
        public WorkingTypeRepository workingTypeRepository;
        public HolidayScheduleRepository holidayScheduleRepository;
        private WorkingTypeRepository _workingTypeRepository;
        private HolidayScheduleRepository _holidayScheduleRepository;
        private TimeSheetRepository _timeSheetRepository;
        private WorkingTypeDescriptionRepository _workingTypeDescriptionRepository;


        /*[GEN-1]*/

        public WorkingDayService(CommonDBContext dbContext, ILogger<WorkingDayService> logger)
          : base(dbContext, logger)
        {
            _repository = new WorkingDayRepository(_dbContext);
            _repositoryImp = (WorkingDayRepository)_repository;
            _authUserRepository = new UserRepository(_dbContext);
            _languageRepository = new LanguageRepository(_dbContext);
            _employeeRepository = new EmployeeRepository(_dbContext);
            workingTypeRepository = new WorkingTypeRepository(_dbContext);
            holidayScheduleRepository = new HolidayScheduleRepository(_dbContext);
            _workingTypeRepository = new WorkingTypeRepository(_dbContext);
            _holidayScheduleRepository = new HolidayScheduleRepository(_dbContext);
            _timeSheetRepository = new TimeSheetRepository(_dbContext);
            _workingTypeDescriptionRepository = new WorkingTypeDescriptionRepository(_dbContext);
            /*[GEN-2]*/
        }
        public override WorkingDayEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repositoryImp.GetById(id, isDirect, isTracking);
            var entityRS = (WorkingDayEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }

        public override async Task<WorkingDayEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = await _repository.GetByIdAsync(id, isDirect);
            var entityRS = (WorkingDayEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override List<WorkingDayEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<WorkingDayEntity>().ToList();
            return listRS;
        }
        public Task<WorkingDayEntity> GetByTimeSheet(TimeSheetEntity e)
        {
            return _repositoryImp.GetByTimeSheet(e);
        }
        public override List<Dictionary<string, object>> GetFormDataObjElement(WorkingDayEntity entity)
        {
            string entityName = nameof(WorkingDayEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;
            var properties = typeof(WorkingDayEntity).GetProperties();
            bool addedFlag = false;
            foreach (var property in properties)
            {
                addedFlag = false;
                // foreignKeyAttribute
                var formDataTypeAttr = (FormDataTypeAttribute)Attribute.GetCustomAttribute(property, typeof(FormDataTypeAttribute));
                var foreignKeyAttribute = (ForeignKeyAttribute)Attribute.GetCustomAttribute(property, typeof(ForeignKeyAttribute));
                if (foreignKeyAttribute != null && !addedFlag && formDataTypeAttr == null)
                {
                    string parentEntity = foreignKeyAttribute.Name;
                    listRS.Add(new Dictionary<string, object> {
    {"display_name", property.Name.Replace("Id","").Replace("Entity","")},
    {"key", property.Name.Replace("Entity","")},
    {"value", property.GetValue(entity)},
    {"type", EnumFormDataType.select.ToStringValue()},
    {"select_data", GetListOptionData(parentEntity, entityName, "")},
    {"searchable", true},
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

                if (formDataTypeAttr == null && !addedFlag)
                {
                    if (property.PropertyType == typeof(string) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(string))
                    {
                        formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.textbox, false, property.Name);
                    }
                    else if (property.PropertyType == typeof(DateTime) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime))
                    {
                        formDataTypeAttr = new FormDataTypeAttribute(EnumFormDataType.datetime, false, defaultVal: DateTime.Now, "");
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
                            break;
                        case EnumFormDataType.floatNumber:
                            formDataTypeAttr.Min = "0";
                            formDataTypeAttr.Max = "86400";
                            formDataTypeAttr.DefaultVal = "0";
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
                            if (property.Name != nameof(WorkingDayEntity.Notes)) break;
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
                        case EnumFormDataType.readonlyType:
                            listRS.Add(new Dictionary<string, object> {
    {"display_name", GetDisplayName(property.Name, entityName)},
    {"key", property.Name.Replace("Entity", "")},
    {"value", property.GetValue(entity)},
    {"type", formDataTypeAttr.TypeName.ToStringValue()},
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
    {"isRequire",formDataTypeAttr.IsRequired },

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
                  {"isRequire",formDataTypeAttr.IsRequired },
                  {"disabled",entity.Id>0?true:false}
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
        { "isRequire", formDataTypeAttr.IsRequired }
  });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.select:
                            //objectList = new List<object>(formDataTypeAttr.Options);
                            string parentEntity = property.Name;
                            listRS.Add(new Dictionary<string, object> {
    {"display_name", property.Name.Replace("Id","").Replace("Entity","")},
    {"key", property.Name.Replace("Entity","")},
    {"value", property.GetValue(entity)},
    {"type", EnumFormDataType.select.ToStringValue()},
    {"select_data", GetListOptionData(parentEntity, entityName, "")},
    {"searchable", true},
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
                        default:
                            listRS.Add(new Dictionary<string, object> {
    {"display_name", GetDisplayName(property.Name, entityName)},
    {"key", property.Name.Replace("Entity", "")},
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

        /// <summary>
        /// UpsertIfNotExist (@GenCRUD)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override WorkingDayEntity Upsert(WorkingDayEntity entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((WorkingDayRepository)_repository).Upsert(entity/*[GEN-4]*/, userId);
            var entityRS = (WorkingDayEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
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
        private List<long> GetListIdChildren(WorkingDayEntity entity, string childEntity)
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
                //case nameof(WorkingDayEntity.Employee):
                //	var listEmps = _employeeRepository.GetAll();
                //	listRS = listEmps.Select(x => new FormSelectOptionModel(x.Id, x.Name)).ToList();
                //	break;
                case nameof(WorkingDayEntity.WorkingTypeEntityId):
                    var listTypes = workingTypeRepository.GetAll();
                    listRS = listTypes.Select(x => new FormSelectOptionModel(x.Id, x.Name, x.Description)).ToList();
                    break;
                case nameof(WorkingDayEntity.WorkingDayStatus):
                    var listStatus = Enum.GetValues<EnumWorkingDayStatus>();
                    listRS = listStatus.Select(x => new FormSelectOptionModel((int)x, x.ToString())).ToList();
                    break;
                /*[GEN-6]*/
                default:
                    break;
            }
            return listRS;
        }

        public IEnumerable<WorkingDayEntity> InsertMulti(IEnumerable<WorkingDayEntity> entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((WorkingDayRepository)_repository).InsertMulti(entity/*[GEN-4]*/, userId);
            return upsertedEntity;
        }


        public WorkingDayPagingResponseModel GetListAttendancev2(
          PagingFilterRequestModel pagingReq = null)
        {
            List<DashboardResponseModel> listResponseModel = null;
            List<DashboardResponseModel> listEmployeeStatusFilter = null;

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            var searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var ret = _repositoryImp.GetListWorkingDayv2(pagingReq, filterParams, searchParams, sortParams);
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<DashboardResponseModel>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = DashboardResponseModel.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;

        }

        public WorkingDayGroupByDepartmentPagingResponseModel GetListAttendanceGroupByDepartment(
          AttendanceGroupByDepartmentRequest requestModel = null)
        {
            List<DashboardResponseModel> listResponseModel = null;
            List<DashboardResponseModel> listEmployeeStatusFilter = null;
            WorkingDayGroupByDepartmentPagingResponseModel result = new WorkingDayGroupByDepartmentPagingResponseModel();
            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(requestModel.FilterStr);
            var searchParams = StringToDictionaryHelper.ToDicOrString(requestModel.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(requestModel.SortStr);
            var listEmployee = _employeeRepository.GetListEmployeeIncludeDepartment();
            result.ListData = _repositoryImp.GetListAttendanceGroupByDepartment(requestModel, listEmployee, filterParams, searchParams, sortParams);
            return result;
        }
        public override object GetDisplayName(string name, string entityName)
        {
            var disName = JsonPropertyHelper<WorkingDayEntity>.GetFormDisplayName(name);
            return disName;
        }

        public async Task<int> GetOTsHours(long employeeId, DateTime WorkingDate, WorkingDayEntity incomming = null)
        {
            var startOfMonth = new DateTime(WorkingDate.Year, WorkingDate.Month, 1, 0, 0, 0);
            var endOfMonth = startOfMonth.AddMonths(1);
            var workingTypesOT = await workingTypeRepository.GetOTTypes();
            var holidays = holidayScheduleRepository.GetList();
            var wods = _repositoryImp.GetEmployeeWorkingDayByDate(employeeId, startOfMonth, endOfMonth);
            var OTHours = await _repositoryImp.GetOTHour(workingTypesOT, holidays, wods);

            return OTHours;
        }
        public async Task<OTModel> GetOTsHours(List<WorkingDayEntity> wds, List<HolidayScheduleEntity> holidays, List<WorkingTypeEntity> workingTypes)
        {
            return await _repositoryImp.GetOTHours(workingTypes, holidays, wds);
        }
        public async Task CreateWdIfNotExist(DateTime date, bool isVirtual = false)
        {
            var employees = _employeeRepository.GetAll();
            var workingTypeUPDefault = workingTypeRepository.GetBySymbol(EnumWorkingDayType.Type_P);
            var workingTypeDefault = workingTypeRepository.GetBySymbol("0");
            var holidaySchedule = holidayScheduleRepository.GetList();
            if (holidaySchedule == null || holidaySchedule.Count < 0 || workingTypeDefault == null || workingTypeUPDefault == null) { return; }
            foreach (var employee in employees)
            {
                var wd = await _repositoryImp.GetEmpDate(date, employee.Id);
                if (wd == null)
                {
                    var isHoliday = holidaySchedule.Count(x => x.StartDate <= date && x.EndDate >= date) > 0;
                    var isWeekend = date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
                    DateTime? timein = isVirtual ? date.AddHours(new Random().Next(0, 23)) : null;
                    DateTime? timeout = isVirtual ? timein?.AddHours(new Random().Next(6, 12)) : null;
                    timeout = timeout?.AddMinutes(new Random().Next(0, 59));
                    wd = new WorkingDayEntity()
                    {
                        CreatedAt = DateTime.Now,
                        EmployeeEntityId = employee.Id,
                        Time_In = timein,
                        Time_Out = timeout,
                        //InOutState = EnumInOutTypeStatus.Unknown,
                        WorkingDayStatus = !isVirtual ? EnumWorkingDayStatus.Absent : EnumWorkingDayStatus.Attended,
                        UpdatedAt = DateTime.Now,
                        WorkingDate = DateTimeUtil.GetStartDate(date),
                        WorkingTypeEntityId = (!isHoliday && !isWeekend) ? (workingTypeUPDefault != null ? workingTypeUPDefault.Id : null) : workingTypeDefault != null ? workingTypeDefault.Id : null
                    };
                    _repositoryImp.Insert(wd);
                }
            }
        }
        public async Task CreateRandomWdIfNotExist(DateTime date, bool isVirtual = false)
        {
            var employees = _employeeRepository.GetAll();
            var wkType = workingTypeRepository.GetAll();
            var holidaySchedule = holidayScheduleRepository.GetList();
            if (holidaySchedule == null || holidaySchedule.Count < 0) { return; }
            foreach (var employee in employees)
            {
                var wd = await _repositoryImp.GetEmpDate(date, employee.Id);
                if (wd == null)
                {
                    var isHoliday = holidaySchedule.Count(x => x.StartDate <= date && x.EndDate >= date) > 0;
                    var isWeekend = date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
                    DateTime? timein = isVirtual ? date.AddHours(new Random().Next(0, 23)) : null;
                    DateTime? timeout = isVirtual ? timein?.AddHours(new Random().Next(6, 12)) : null;
                    timeout = timeout?.AddMinutes(new Random().Next(0, 59));
                    wd = new WorkingDayEntity()
                    {
                        CreatedAt = DateTime.Now,
                        EmployeeEntityId = employee.Id,
                        Time_In = timein,
                        Time_Out = timeout,
                        //InOutState = EnumInOutTypeStatus.Unknown,
                        WorkingDayStatus = !isVirtual ? EnumWorkingDayStatus.Absent : EnumWorkingDayStatus.Attended,
                        UpdatedAt = DateTime.Now,
                        WorkingDate = DateTimeUtil.GetStartDate(date),
                        WorkingTypeEntityId = wkType[new Random().Next(0, wkType.Count)]?.Id
                    };
                    _repositoryImp.Insert(wd);
                }
            }
        }
        public WorkingDayEntity ReCalculate(List<TimeSheetEntity>? timesheets, long? workingDayId, List<WorkingTypeEntity>? listWorkingType, bool isJustEdit = false)
        {
            try
            {
                Dictionary<DateTime, WorkingDayEntity> dicWorkingDay = new Dictionary<DateTime, WorkingDayEntity>();

                WorkingDayEntity existedWorkingday = null;
                bool isReinit = false;

                if (timesheets == null || timesheets.Count() <= 0)
                {
                    existedWorkingday = this._repositoryImp.GetById(workingDayId);

                    if (existedWorkingday == null)
                    {
                        return null;
                    }
                    else
                    {
                        var workingType = listWorkingType?.Where(x => x.Code == EnumWorkingDayType.Type_P).FirstOrDefault();

                        existedWorkingday.Time_In = null;
                        existedWorkingday.Time_Out = null;
                        existedWorkingday.TimeDeviation = null;
                        existedWorkingday.InOutState = EnumInOutTypeStatus.Unknown;
                        existedWorkingday.WorkingTypeEntityId = workingType?.Id;
                        existedWorkingday.WorkingType = workingType;
                        existedWorkingday.WorkingDayStatus = EnumWorkingDayStatus.Absent;
                        existedWorkingday = this.Upsert(existedWorkingday);
                        return existedWorkingday;
                    }
                }
                else
                {
                    timesheets = timesheets.OrderBy(x => x.RecordedTime).ToList();
                    foreach (var timesheet in timesheets)
                    {
                        if (!dicWorkingDay.ContainsKey(DateTimeUtil.GetStartDate(timesheet.RecordedTime.Value)))
                        {
                            if (timesheet.WorkingDay == null)
                            {
                                // if timesheet have not map to working day => found to map
                                existedWorkingday = _repositoryImp.GetEmpDate(timesheet.RecordedTime.Value, timesheet.EmployeeId.GetValueOrDefault()).Result;
                                // if today working date is null => search for yesterday working day
                                // create new working day
                                if (existedWorkingday == null)
                                {
                                    existedWorkingday = new Database.Entities.WorkingDayEntity()
                                    {
                                        WorkingDate = timesheet.RecordedTime,
                                        CreatedAt = DateTime.Now,
                                        UpdatedAt = DateTime.Now,
                                        EmployeeEntityId = timesheet.EmployeeId,
                                        TimeDeviation = 0,
                                        TimeSheets = new List<Database.Entities.TimeSheetEntity>()
                                    };
                                }
                                existedWorkingday?.TimeSheets?.Add(timesheet);
                            }
                            else
                            {
                                existedWorkingday = timesheet.WorkingDay;
                            }

                            dicWorkingDay.Add(DateTimeUtil.GetStartDate(timesheet.RecordedTime.Value), existedWorkingday);
                        }
                        else
                        {
                            existedWorkingday = dicWorkingDay[DateTimeUtil.GetStartDate(timesheet.RecordedTime.Value)];
                        }

                        var state = timesheet.Status;


                        if (isJustEdit == true)
                        {
                            if (!isReinit)
                            {
                                existedWorkingday.Time_In = null;
                                existedWorkingday.Time_Out = null;
                                isReinit = true;
                            }
                        }
                        if (state == EnumFaceId.Check_In)
                        {
                            existedWorkingday.InOutState = EnumInOutTypeStatus.Inside;
                            // update if last time check in lower than timesheet check in
                            if (existedWorkingday.Time_In == null || existedWorkingday.Time_In < timesheet.RecordedTime)
                            {
                                existedWorkingday.Time_In = timesheet.RecordedTime;
                                existedWorkingday.Time_Out = null;
                            }
                        }
                        else if (state == EnumFaceId.Check_Out)
                        {
                            existedWorkingday.InOutState = EnumInOutTypeStatus.Outside;
                            if (existedWorkingday.Time_Out == null || existedWorkingday.Time_Out <= timesheet.RecordedTime || (existedWorkingday.WorkingDayUpdates == null || existedWorkingday.WorkingDayUpdates.Count <= 0))
                            {
                                existedWorkingday.Time_Out = timesheet.RecordedTime;
                                if (existedWorkingday.Time_In == null) // shift 3 of lastday checkout record => update yester workingday timeout
                                {
                                    var yesterdayWd = _repositoryImp.GetEmpDate(timesheet.RecordedTime.Value.AddDays(-1), timesheet.EmployeeId.GetValueOrDefault()).Result;
                                    if (yesterdayWd != null && yesterdayWd.Time_Out == null) // if not assign timeout for yesterday working day
                                    {
                                        yesterdayWd.Time_Out = timesheet.RecordedTime;
                                        yesterdayWd.UpdatedAt = DateTime.Now;
                                        var yesterdayDeviation = (yesterdayWd.Time_Out - yesterdayWd.Time_In - TimeSpan.FromHours(8)).GetValueOrDefault().TotalSeconds;
                                        if (yesterdayWd.TimeDeviation == null)
                                        {
                                            // => if time deviation is lower than total second in a day
                                            if (yesterdayDeviation <= 24 * 60 * 60)
                                            {

                                                yesterdayWd.TimeDeviation = yesterdayDeviation;
                                            }
                                            else
                                            {
                                                yesterdayWd.TimeDeviation = 0;
                                            }
                                        }
                                        yesterdayWd.InOutState = EnumInOutTypeStatus.Outside;
                                        // TODO: yesterdayWd cũng cần check lại bug giống existedWorkingday (bị truy vấn lên nhiều lần cùng ID)
                                        _repositoryImp.Upsert(yesterdayWd);
                                    }
                                    else
                                    {
                                        var deviation = (existedWorkingday.Time_Out - existedWorkingday.Time_In - TimeSpan.FromHours(8)).GetValueOrDefault().TotalSeconds;
                                        if (existedWorkingday.TimeDeviation == null)
                                        {
                                            if (deviation <= 24 * 60 * 60)
                                                existedWorkingday.TimeDeviation = deviation;
                                            else
                                                existedWorkingday.TimeDeviation = 0;
                                        }
                                    }
                                }
                            }
                        }
                        existedWorkingday.WorkingDayStatus = EnumWorkingDayStatus.Attended;
                        existedWorkingday.UpdatedAt = DateTime.Now;
                        if (existedWorkingday.Id == 0)
                        {
                            existedWorkingday.CreatedAt = DateTime.Now;
                        }
                    }
                    existedWorkingday = this.Upsert(existedWorkingday);
                    return existedWorkingday;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void ReCalculateDelete(long workingDayId, List<WorkingTypeEntity>? listWorkingType)
        //{
        //    try
        //    {
        //        var workingDay = this._repositoryImp.GetById(workingDayId);
        //        var workingType = listWorkingType?.Where(x => x.Code.ToLower().ToString() == "up").FirstOrDefault();
        //        if (workingDay != null)
        //        {
        //            workingDay.Time_In = null;
        //            workingDay.Time_Out = null;
        //            workingDay.TimeDeviation = null;
        //            workingDay.InOutState = EnumInOutTypeStatus.Unknown;
        //            workingDay.WorkingTypeEntityId = workingType?.Id;
        //            workingDay.WorkingDayStatus = EnumWorkingDayStatus.Absent;
        //            workingDay.UpdatedAt = DateTime.Now;
        //            workingDay.CreatedAt = DateTime.Now;
        //            this.Upsert(workingDay);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DetailAttendancePagingResponseModel GetListWorkingDayByEmployeeId(
                    long employeeId,
                    DetailAttendancePagingResponseModel workingDays0,
                    List<WorkingTypeEntity> listWorkingType,
                    DateTime? dateFrom,
                    DateTime? dateTo)
        {
            var listHoliday = _holidayScheduleRepository.GetList();
            var dicHolidayScheduleStartDate = new Dictionary<string, HolidayScheduleEntity>();
            foreach (var item in listHoliday)
            {
                DateTime dt = DateTimeUtil.GetStartDate(item.StartDate);
                for (dt = DateTimeUtil.GetStartDate(item.StartDate); dt <= DateTimeUtil.GetStartDate(item.EndDate); dt = dt.AddDays(1))
                {
                    string keyHoliday = DateTimeUtil.GetDateTimeStr(dt, ConstDateTimeFormat.YYYYMMDD);
                    if (!dicHolidayScheduleStartDate.ContainsKey(keyHoliday))
                    {
                        dicHolidayScheduleStartDate.Add(keyHoliday, item);
                    }
                }
            }

            var dicWorkingDayId2Type = CalculateMonthWorkingType(employeeId, workingDays0.rawDatas, listWorkingType, dateFrom, dateTo, dicHolidayScheduleStartDate);
            for (int i = 0; i < workingDays0.ListData.Count; i++)
            {
                var item = workingDays0.ListData[i];
                if (dicWorkingDayId2Type.ContainsKey(item.Id.Value))
                {
                    item.RecommendType = dicWorkingDayId2Type[item.Id.Value].WorkingType.Code;
                    item.recommendTypeId = dicWorkingDayId2Type[item.Id.Value].WorkingType.Id;
                    if (item.TimeDeviation == null)
                    {
                        item.TimeDeviation = dicWorkingDayId2Type[item.Id.Value].TimeDeviatioinInSeconds;
                    }
                    item.WorkingDayHighlight = dicWorkingDayId2Type[item.Id.Value].workingDayHighlight;
                }
                else
                {
                    _logger.LogMsg(Messages.ErrException, $"Employee: {employeeId}, WorkingDayObj: {item.Id.Value} have not recommendType");
                }
            }
            // UpdateRcmTypeAndDeviationToDB(dicWorkingDayId2Type);
            return workingDays0;
        }
        private async Task<bool> UpdateRcmTypeAndDeviationToDB(Dictionary<long, RecommendTypeResultModel> datas)
        {
            foreach (var data in datas)
            {
                var wd = _repositoryImp.GetById(data.Key, false, true);
                if (wd == null || (wd.RecommendType == data.Value.WorkingType?.Code && wd.TimeDeviation == data.Value.TimeDeviatioinInSeconds)) continue;
                if (wd.RecommendType != data.Value.WorkingType?.Code)
                {
                    wd.RecommendType = data.Value.WorkingType?.Code;
                    //workingDayEntity.RecommendTypeId = data.Value.WorkingType?.Id;
                }
                if (wd.TimeDeviation == null)
                {
                    wd.TimeDeviation = data.Value.TimeDeviatioinInSeconds;
                }
                wd.UpdatedAt = DateTime.Now;
                this._repositoryImp.Update(wd);
            }
            return true;
        }
        public Dictionary<long, RecommendTypeResultModel> CalculateMonthWorkingType(
                      long employeeId,
                      List<WorkingDayEntity> listWorkingDay0,
                      List<WorkingTypeEntity> listMasterWorkingType,
                      DateTime? dateFrom,
                      DateTime? dateTo,
                      Dictionary<string, HolidayScheduleEntity> dicHolidayScheduleStartDate)
        {
            var dicWorkingDayId2TypeRS = new Dictionary<long, RecommendTypeResultModel>();
            var dicWorkingDayId2TypeStrAndDeviation = new Dictionary<long, RecommendTypeResultModel>();
            Dictionary<long, WorkingShiftModel> dicWorkingDayId2Hours = new Dictionary<long, WorkingShiftModel>();

            if (listWorkingDay0 == null || listWorkingDay0.Count <= 0)
            {
                return dicWorkingDayId2TypeRS;
            }

            var dateOfPreMonth = listWorkingDay0[0].WorkingDate.Value.AddDays(-1);
            var dateOfNextMonth = listWorkingDay0[listWorkingDay0.Count - 1].WorkingDate.Value.AddDays(1);

            var workingDayOfPreMonth = _repositoryImp.GetListWorkingDayByEmployeeId3(employeeId, dateOfPreMonth).FirstOrDefault();
            var workingDayOfNextMonth = _repositoryImp.GetListWorkingDayByEmployeeId3(employeeId, dateOfNextMonth).FirstOrDefault();

            List<WorkingDayEntity> listWorkingDayExtend = new List<WorkingDayEntity>();
            if (workingDayOfPreMonth != null)
            {
                listWorkingDayExtend.Add(workingDayOfPreMonth);
            }
            listWorkingDayExtend.AddRange(listWorkingDay0);
            if (workingDayOfNextMonth != null)
            {
                listWorkingDayExtend.Add(workingDayOfNextMonth);
            }

            var dicMissCheckOutEndOfDay = new Dictionary<long, bool>();
            for (int wdIndex = 0; wdIndex < listWorkingDayExtend.Count; wdIndex++)
            {
                var workingDay = listWorkingDayExtend[wdIndex];

                if (workingDay == null)
                {
                    if (!dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                    {
                        dicWorkingDayId2TypeStrAndDeviation.Add(workingDay.Id, new RecommendTypeResultModel(EnumWorkingDayType.Type_P, null));
                    }
                    continue;
                }

                if (workingDay.WorkingDate == null)
                {
                    if (!dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                    {
                        dicWorkingDayId2TypeStrAndDeviation.Add(workingDay.Id, new RecommendTypeResultModel(EnumWorkingDayType.Type_P, null));
                    }
                    continue;
                }

                if (workingDay.TimeSheets == null || workingDay.TimeSheets.Count <= 0)
                {
                    if (!dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                    {
                        dicWorkingDayId2TypeStrAndDeviation.Add(workingDay.Id, new RecommendTypeResultModel(EnumWorkingDayType.Type_P, null));
                    }
                    continue;
                }

                var listTimeSheet = workingDay.TimeSheets.ToList();

                if (listTimeSheet == null || listTimeSheet.Count <= 0)
                {
                    if (!dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                    {
                        dicWorkingDayId2TypeStrAndDeviation.Add(workingDay.Id, new RecommendTypeResultModel(EnumWorkingDayType.Type_P, null));
                    }
                    continue;
                }

                listTimeSheet.Sort((x, y) =>
                      {
                          int result = DateTimeUtil.CompareDateTime2(x.RecordedTime, y.RecordedTime);
                          if (result == 0)
                          {
                              result = x.Id.CompareTo(y.Id);
                          }
                          return result;
                      });

                List<TimeRangeModel> listTimeRange = getListTimeRange(workingDay, listTimeSheet, ref dicMissCheckOutEndOfDay);

                if (listTimeRange.Count <= 0)
                {
                    if (!dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                    {
                        dicWorkingDayId2TypeStrAndDeviation.Add(workingDay.Id, new RecommendTypeResultModel(EnumWorkingDayType.Type_P, null));
                    }
                    continue;
                }

                if (workingDay.WorkingDate.Value.Day == DateTime.Now.Day && workingDay.WorkingDate.Value.Year == DateTime.Now.Year && workingDay.WorkingDate.Value.Month == DateTime.Now.Month && dicMissCheckOutEndOfDay.ContainsKey(workingDay.Id))
                {
                    if (!dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                    {
                        dicWorkingDayId2TypeStrAndDeviation.Add(workingDay.Id, new RecommendTypeResultModel(EnumWorkingDayType.Type_0, null));
                    }
                    continue;
                }

                WorkingShiftModel workingShiftModel = new WorkingShiftModel(workingDay, listTimeRange);

                if (!dicWorkingDayId2Hours.ContainsKey(workingDay.Id))
                {
                    dicWorkingDayId2Hours.Add(workingDay.Id, workingShiftModel);
                }
                else
                {
                    dicWorkingDayId2Hours[workingDay.Id] = workingShiftModel;
                }
            }

            dicWorkingDayId2Hours = moveTimeRange2PreWorkingDay(listWorkingDayExtend, dicWorkingDayId2Hours);

            for (int wdIndex = 0; wdIndex < listWorkingDayExtend.Count; wdIndex++)
            {
                var workingDay = listWorkingDayExtend[wdIndex];

                if (dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                {
                    continue;
                }
                var hoursModel = dicWorkingDayId2Hours[workingDay.Id];

                hoursModel.CalculatorHours();
                hoursModel.CalculatorRecommendType(dicHolidayScheduleStartDate);
            }

            //dicWorkingDayId2Hours = moveOverOTHours2OthersWorkingDay(listWorkingDayExtend, dicWorkingDayId2Hours, dicHolidayScheduleStartDate, dateFrom, dateTo);

            for (int wdIndex = 0; wdIndex < listWorkingDayExtend.Count; wdIndex++)
            {
                var workingDay = listWorkingDayExtend[wdIndex];

                if (dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                {
                    continue;
                }
                var hoursModel = dicWorkingDayId2Hours[workingDay.Id];

                string recommendType = hoursModel.ReconnemdType;
                long timeDeviation = hoursModel.GetTimeDeviation();
                EnumWorkingDayHighlight workingDayHighlight = hoursModel.WorkingDayHighlight;

                if (!dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                {
                    var recommendTypeResultModel = new RecommendTypeResultModel(recommendType, timeDeviation, workingDayHighlight);
                    recommendTypeResultModel.OTHours = hoursModel.OTHours;
                    recommendTypeResultModel.Over4HHours = hoursModel.Over4HHours;
                    recommendTypeResultModel.OTHourType = hoursModel.OTHourType;
                    if (!dicWorkingDayId2TypeStrAndDeviation.ContainsKey(workingDay.Id))
                    {
                        dicWorkingDayId2TypeStrAndDeviation.Add(workingDay.Id, recommendTypeResultModel);
                    }
                    else
                    {
                        dicWorkingDayId2TypeStrAndDeviation[workingDay.Id] = recommendTypeResultModel;
                    }
                    continue;
                }
            }

            if (workingDayOfPreMonth != null)
            {
                dicWorkingDayId2TypeStrAndDeviation.Remove(workingDayOfPreMonth.Id);
            }
            if (workingDayOfNextMonth != null)
            {
                dicWorkingDayId2TypeStrAndDeviation.Remove(workingDayOfNextMonth.Id);
            }

            Dictionary<string, WorkingTypeEntity> dicType2Entity = listMasterWorkingType.ToDictionary(x => x.Code, x => x);
            foreach (var keyVal in dicWorkingDayId2TypeStrAndDeviation)
            {
                if (keyVal.Value.RecommendTypeStr == EnumWorkingDayType.Type_NULL)
                {
                    var recommendTypeResultModel = new RecommendTypeResultModel(keyVal.Value.RecommendTypeStr, keyVal.Value.TimeDeviatioinInSeconds, keyVal.Value.workingDayHighlight);
                    recommendTypeResultModel.WorkingType = null;
                    recommendTypeResultModel.OTHours = keyVal.Value.OTHours;
                    recommendTypeResultModel.Over4HHours = keyVal.Value.Over4HHours;
                    recommendTypeResultModel.OTHourType = keyVal.Value.OTHourType;
                    if (!dicWorkingDayId2TypeRS.ContainsKey(keyVal.Key))
                    {
                        dicWorkingDayId2TypeRS.Add(keyVal.Key, recommendTypeResultModel);
                    }
                    else
                    {
                        dicWorkingDayId2TypeRS[keyVal.Key] = recommendTypeResultModel;
                    }
                }
                else if (dicType2Entity.ContainsKey(keyVal.Value.RecommendTypeStr))
                {
                    var recommendTypeResultModel = new RecommendTypeResultModel(keyVal.Value.RecommendTypeStr, keyVal.Value.TimeDeviatioinInSeconds, keyVal.Value.workingDayHighlight);
                    recommendTypeResultModel.WorkingType = dicType2Entity[keyVal.Value.RecommendTypeStr];
                    recommendTypeResultModel.OTHours = keyVal.Value.OTHours;
                    recommendTypeResultModel.Over4HHours = keyVal.Value.Over4HHours;
                    recommendTypeResultModel.OTHourType = keyVal.Value.OTHourType;

                    if (!dicWorkingDayId2TypeRS.ContainsKey(keyVal.Key))
                    {
                        dicWorkingDayId2TypeRS.Add(keyVal.Key, recommendTypeResultModel);
                    }
                    else
                    {
                        dicWorkingDayId2TypeRS[keyVal.Key] = recommendTypeResultModel;
                    }
                }
                else
                {
                    throw new NotImplementedException($"WorkingDayType = {keyVal.Value} is not exists in DB");
                }
            }

            return dicWorkingDayId2TypeRS;
        }

        //private Dictionary<long, WorkingShiftModel> moveOverOTHours2OthersWorkingDay(
        //    List<WorkingDayEntity> listWorkingDay,
        //    Dictionary<long, WorkingShiftModel> dicWorkingDayId2Hours,
        //    Dictionary<string, HolidayScheduleEntity> dicHolidayScheduleStartDate,
        //    DateTime? dateFrom,
        //    DateTime? dateTo)
        //{
        //    if (dicWorkingDayId2Hours.Count <= 0)
        //    {
        //        return dicWorkingDayId2Hours;
        //    }

        //    if (dateFrom == null || dateTo == null)
        //    {
        //        throw new NotImplementedException("date = null");
        //    }

        //    List<int> listOverHoursIndex = new List<int>();
        //    for (int i = 0; i < listWorkingDay.Count; i++)
        //    {
        //        var workingDay1 = listWorkingDay[i];
        //        if (workingDay1 != null
        //            && dicWorkingDayId2Hours.ContainsKey(workingDay1.Id))
        //        {
        //            var hourModel = dicWorkingDayId2Hours[workingDay1.Id];
        //            if (hourModel.Over4HHours > 0 && hourModel.WorkingDayHighlight == EnumWorkingDayHighlight.OTHoursOver4H)
        //            {
        //                if (workingDay1.WorkingDate.Value.Date >= dateFrom.Value.Date 
        //                    && workingDay1.WorkingDate.Value.Date <= dateTo.Value.Date)
        //                {
        //                    listOverHoursIndex.Add(i);
        //                }
        //            }
        //        }
        //    }
        //    if (listOverHoursIndex.Count <= 0)
        //    {
        //        return dicWorkingDayId2Hours;
        //    }

        //    var dicWorkingDay = listWorkingDay.Where(x => x.WorkingDate.HasValue).ToDictionary(x => x.WorkingDate.Value, x => x);

        //    //List<WorkingDayEntity> listWorkingDayFull = new List<WorkingDayEntity>();
        //    //var date = dateFrom.Value;
        //    //for (; date <= dateTo.Value; date = date.AddDays(1))
        //    //{
        //    //    if (dicWorkingDay.ContainsKey(date))
        //    //    {
        //    //        listWorkingDayFull.Add(dicWorkingDay[date]);
        //    //    }
        //    //    else
        //    //    {
        //    //        //// TODO: Insert working day
        //    //        //var workingDayAdded = new WorkingDayEntity();
        //    //        //workingDayAdded.WorkingDate
        //    //        //listWorkingDayFull.Add(workingDayAdded);
        //    //    }
        //    //}

        //    foreach (var wIndex in listOverHoursIndex)
        //    {
        //        var workingDay1 = listWorkingDay[wIndex];
        //        var hourModel1 = dicWorkingDayId2Hours[workingDay1.Id];
        //        int overHours = hourModel1.Over4HHours;

        //        for (int i = wIndex + 1; ;)
        //        {
        //            if (i < listWorkingDay.Count)
        //            {
        //                var workingDay2 = listWorkingDay[i];
        //                if (dicWorkingDayId2Hours.ContainsKey(workingDay2.Id))
        //                {
        //                    var hourModel2 = dicWorkingDayId2Hours[workingDay2.Id];
        //                    if (hourModel2.OTHours == 0
        //                        && (hourModel1.ShiftId == EnumShiftId.Shift3 && hourModel2.ShiftId == EnumShiftId.Shift3
        //                            || hourModel1.ShiftId != EnumShiftId.Shift3 && hourModel2.ShiftId != EnumShiftId.Shift3))
        //                    {
        //                        if (hourModel1.IsWeekenOrHoliday(dicHolidayScheduleStartDate)
        //                            && hourModel2.IsWeekenOrHoliday(dicHolidayScheduleStartDate))
        //                        {
        //                            hourModel2.OTHours = overHours;
        //                            hourModel2.OTHourType = hourModel1.OTHourType;
        //                            hourModel2.CalculatorOTRecommentType(hourModel2.OTHours);
        //                            hourModel1.Over4HHours = 0;
        //                            hourModel1.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H_Moved;
        //                            break;
        //                        }
        //                        else if (!hourModel1.IsWeekenOrHoliday(dicHolidayScheduleStartDate)
        //                            && !hourModel2.IsWeekenOrHoliday(dicHolidayScheduleStartDate))
        //                        {
        //                            hourModel2.OTHours = overHours;
        //                            hourModel2.OTHourType = hourModel1.OTHourType;
        //                            hourModel2.CalculatorOTRecommentType(hourModel2.OTHours);
        //                            hourModel1.Over4HHours = 0;
        //                            hourModel1.WorkingDayHighlight = EnumWorkingDayHighlight.OTHoursOver4H_Moved;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }

        //            i++;

        //            if (i >= listWorkingDay.Count)
        //            {
        //                i = 0;
        //            }

        //            if (i == wIndex)
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    return dicWorkingDayId2Hours;
        //}

        //private List<WorkingDayEntity> getWorkingDay0(List<WorkingDayEntity> listWorkingDayExtend)
        //{
        //  if (listWorkingDayExtend.Count >= 1)
        //  {
        //    DateTime dateOfPreMonth = listWorkingDayExtend[0].WorkingDate.Value.AddDays(-1);
        //    DateTime dateOfNextMonth = listWorkingDayExtend[listWorkingDayExtend.Count - 1].WorkingDate.Value.AddDays(1);


        //  }
        //}

        ///// <summary>
        ///// moveHoursShift3AMToPreWorkingDay
        ///// </summary>
        ///// <param name="dicWorkingDayId2Hours"></param>
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //private Dictionary<long, WorkingShiftModel> moveHoursShift3AMToPreWorkingDay(
        //            List<WorkingDayEntity> listWorkingDay,
        //            Dictionary<long, WorkingShiftModel> dicWorkingDayId2Hours)
        //{
        //    int hoursPM = 0;
        //    int hoursAM3 = 0;
        //    int hoursNormalAM = 0;

        //    if (dicWorkingDayId2Hours.Count <= 0)
        //    {
        //        return dicWorkingDayId2Hours;
        //    }

        //    for (int i = 0; i < listWorkingDay.Count - 1; i++)
        //    {
        //        var workingDay1 = listWorkingDay[i];
        //        var workingDay2 = listWorkingDay[i + 1];
        //        var deltaSeconds = DateTimeUtil.CompareDateTime(
        //                                            DateTimeUtil.GetStartDate(workingDay1.WorkingDate.Value),
        //                                            DateTimeUtil.GetStartDate(workingDay2.WorkingDate.Value),
        //                                            long.MaxValue,
        //                                            false);

        //        if (deltaSeconds != TimeSpan.FromDays(1).TotalSeconds)
        //        {
        //            continue;
        //        }

        //        if (dicWorkingDayId2Hours.ContainsKey(workingDay1.Id)
        //          && dicWorkingDayId2Hours.ContainsKey(workingDay2.Id))
        //        {
        //            var hoursModel1 = dicWorkingDayId2Hours[workingDay1.Id];
        //            var hoursModel2 = dicWorkingDayId2Hours[workingDay2.Id];

        //            hoursNormalAM = DateTimeUtil.GetHours(hoursModel2.ShiftNormalOTSubAM);
        //            if (hoursNormalAM >= 1)
        //            {
        //                hoursModel1.ShiftNormalOTSubAM += hoursModel2.ShiftNormalOTSubAM;
        //                hoursModel1.ShiftNormalOTShift3Total += hoursModel2.ShiftNormalOTSubAM;
        //                hoursModel1.CalculatorShiftId();

        //                hoursModel2.ShiftNormalOTShift3Total -= hoursModel2.ShiftNormalOTSubAM;
        //                hoursModel2.ShiftNormalOTSubAM = 0;
        //                hoursModel2.CalculatorShiftId();
        //            }

        //            hoursAM3 = DateTimeUtil.GetHours(hoursModel2.Shift3SubAM);
        //            if (hoursAM3 >= 1)
        //            {
        //                hoursModel1.Shift3SubAM += hoursModel2.Shift3SubAM;
        //                hoursModel1.Shift3Total += hoursModel2.Shift3SubAM;
        //                hoursModel1.CalculatorShiftId();

        //                hoursModel2.Shift3Total -= hoursModel2.Shift3SubAM;
        //                hoursModel2.Shift3SubAM = 0;
        //                hoursModel2.CalculatorShiftId();
        //            }
        //        }
        //    }

        //    return dicWorkingDayId2Hours;
        //}

        private Dictionary<long, WorkingShiftModel> moveTimeRange2PreWorkingDay(
            List<WorkingDayEntity> listWorkingDay,
            Dictionary<long, WorkingShiftModel> dicWorkingDayId2Hours)
        {
            if (dicWorkingDayId2Hours.Count <= 0)
            {
                return dicWorkingDayId2Hours;
            }

            for (int i = 0; i < listWorkingDay.Count - 1; i++)
            {
                var workingDay1 = listWorkingDay[i];
                var workingDay2 = listWorkingDay[i + 1];
                var deltaSeconds = DateTimeUtil.CompareDateTime(
                                                    DateTimeUtil.GetStartDate(workingDay1.WorkingDate.Value),
                                                    DateTimeUtil.GetStartDate(workingDay2.WorkingDate.Value),
                                                    long.MaxValue,
                                                    false);

                // 2 ngày không liên tiếp
                if (deltaSeconds != TimeSpan.FromDays(1).TotalSeconds)
                {
                    continue;
                }

                if (dicWorkingDayId2Hours.ContainsKey(workingDay1.Id)
                  && dicWorkingDayId2Hours.ContainsKey(workingDay2.Id))
                {
                    var hoursModel1 = dicWorkingDayId2Hours[workingDay1.Id];
                    var hoursModel2 = dicWorkingDayId2Hours[workingDay2.Id];

                    for (int j = hoursModel2.ListTimeRange.Count - 1; j >= 0; j--)
                    {
                        var timeRange = hoursModel2.ListTimeRange[j];
                        if (timeRange.TimeRangeType == EnumTimeRangeType.IsNotCheckIn)
                        {
                            hoursModel1.ListTimeRange.Add(timeRange);
                            hoursModel2.ListTimeRange.RemoveAt(j);
                        }
                    }
                }
            }

            return dicWorkingDayId2Hours;
        }


        private List<TimeRangeModel> getListTimeRange(WorkingDayEntity workingDay, List<TimeSheetEntity> listTimeSheet, ref Dictionary<long, bool> dicMissCheckOutEndOfDay)
        {
            List<TimeRangeModel> listTimeRange = new List<TimeRangeModel>();

            bool isGetStartOfDayFlag = true;

            DateTime startOfDay = DateTimeUtil.GetStartDate(workingDay.WorkingDate.Value);
            DateTime endOfDay = startOfDay.AddDays(1);

            TimeSheetEntity lastCheckIn = null;
            for (int tsIndex = 0; tsIndex < listTimeSheet.Count; tsIndex++)
            {
                var timeSheet = listTimeSheet[tsIndex];
                if (timeSheet.RecordedTime == null)
                {
                    continue;
                }

                if (timeSheet.Status == EnumFaceId.Check_In)
                {
                    if (lastCheckIn == null)
                    {
                        lastCheckIn = timeSheet;
                    }
                    else
                    {
                        // Checkin 2 lần liên tiếp --> lấy lần thứ 1
                        continue;
                    }
                }
                else if (timeSheet.Status == EnumFaceId.Check_Out)
                {
                    if (lastCheckIn == null)// Không có checkin ở đầu ngày
                    {
                        if (isGetStartOfDayFlag)
                        {
                            var timeRange = new TimeRangeModel(startOfDay, timeSheet.RecordedTime.Value);
                            timeRange.TimeRangeType = EnumTimeRangeType.IsNotCheckIn;
                            listTimeRange.Add(timeRange);
                            isGetStartOfDayFlag = false;
                        }
                        else
                        {
                            // Checkout 2 lần liên tiếp --> lấy lần thứ 2
                            listTimeRange[listTimeRange.Count - 1].EndTime = timeSheet.RecordedTime.Value;
                        }
                    }
                    else
                    {
                        var timeRange = new TimeRangeModel(lastCheckIn.RecordedTime.Value, timeSheet.RecordedTime.Value);
                        listTimeRange.Add(timeRange);
                        lastCheckIn = null;
                        isGetStartOfDayFlag = false;
                    }
                }
                else
                {
                    continue;
                }
            }

            if (lastCheckIn != null)// Không có checkout ở cuối ngày
            {
                var timeRange = new TimeRangeModel(lastCheckIn.RecordedTime.Value, endOfDay);
                listTimeRange.Add(timeRange);
                dicMissCheckOutEndOfDay[workingDay.Id] = true;
                isGetStartOfDayFlag = false;
                lastCheckIn = null;
            }

            return listTimeRange;
        }
        public async Task<string> ExportCurrentInoutState(PagingFilterRequestModel request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var templatePath = "./wwwroot/ReportTemplate/emplee_current_template.xlsx";
            string reportTemplateSheet = "Sheet1";
            string excelFileNameTemplate = "Report_Currently_ {0} {1}.xlsx";
            string zipFileName = "Report_Currently_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".zip";
            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    var excelFileName = string.Format(excelFileNameTemplate, request.DateFrom?.ToString("yyyyMMdd"), request.DateTo?.ToString("yyyyMMdd"));
                    var excelFilePath = $"./{excelFileName}";
                    bool isTemplateExist = File.Exists(templatePath);
                    _logger.LogInformation($"Template Path: {templatePath}");
                    _logger.LogInformation($"Template Path Exist: {isTemplateExist}");
                    ExcelPro._templatePath = templatePath;
                    using (var report = ExcelPro.LoadReportFormat())
                    {
                        var listWds = await _repositoryImp.GetWorkingDayByDate(request.DateFrom.GetValueOrDefault().AddDays(-1), request.DateTo.GetValueOrDefault());
                        List<long> listWdIds = listWds.Select(x => x.Id).ToList();

                        int rowCount = listWdIds.Count;
                        int beginRowIndex = 8;
                        int insertEmptyRow = rowCount - 1;
                        int headerCopyColumnCount = 17;
                        double rowHeight = 27.8;
                        var sheet1 = report.Workbook.Worksheets[reportTemplateSheet];

                        /************************************************************************/
                        /*                          INSERT EMPTY ROWS START                     */
                        /************************************************************************/

                        // set formula sumarize 
                        var startColWd = 3;
                        if (insertEmptyRow >= 1)
                        {
                            sheet1.InsertRow(beginRowIndex + 1, insertEmptyRow, beginRowIndex);
                            for (int i = 0; i < insertEmptyRow; i++)
                            {
                                sheet1.Rows[beginRowIndex + 1 + i].Height = rowHeight;
                            }
                        }
                        /*INSERT EMPTY ROWS END*/

                        /************************************************************************/
                        /*                          GET DATA TO listData START                  */
                        /************************************************************************/
                        var dicEmployeeId2Workingdays = listWds;
                        int count = 0;
                        List<Dictionary<object, object>> listData = listWds.Select(x =>
                        {
                            var ret = new Dictionary<object, object>();
                            ret.Add("STT", ++count);
                            ret.Add(nameof(WorkingDayEntity.WorkingDate), x.WorkingDate?.ToString("dd/MM/yyyy"));
                            ret.Add(nameof(WorkingDayEntity.EmployeeName), x.Employee?.Name);
                            ret.Add(nameof(WorkingDayEntity.Employee.EmployeeCode), x.Employee?.EmployeeCode);
                            ret.Add(nameof(WorkingDayEntity.Employee.PhoneNumber), x.Employee?.PhoneNumber);
                            ret.Add(nameof(WorkingDayEntity.Employee.Department), x.Employee?.Department?.Name);
                            ret.Add(nameof(WorkingDayEntity.Employee.JobTitle), x.Employee?.JobTitle?.Name);
                            ret.Add(nameof(WorkingDayEntity.InOutState), x.InOutState.ToString());
                            ret.Add(nameof(WorkingDayEntity.Time_In), x.Time_In?.ToString("HH:mm:ss"));
                            ret.Add(nameof(WorkingDayEntity.Time_Out), x.Time_Out?.ToString("HH:mm:ss"));
                            return ret;
                        }).ToList();
                        EmployeeEntity employee = null;

                        /*GET DATA TO listData END*/

                        /************************************************************************/
                        /*                          SET METADATA START                  */
                        /************************************************************************/
                        ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel();
                        exportSheetDataModel.SheetIndex = sheet1.Index;
                        exportSheetDataModel.DicCellName2Value.Add("D4", $"{request.DateFrom?.ToString("dd/MM/yyy")}-{request.DateTo?.ToString("dd/MM/yyy")}");
                        exportSheetDataModel.DicCellName2Value.Add("D5", DateTime.Now.ToString("dd/MM/yyy HH:mm:ss"));
                        /*SET METADATA END*/

                        /************************************************************************/
                        /*                          SET DATA TO EXCEL START                  */
                        /************************************************************************/
                        exportSheetDataModel.BeginRowIndex = beginRowIndex;
                        exportSheetDataModel.BeginNoNumber = 1;
                        exportSheetDataModel.ListChartDataModel = listData;
                        int columnIndex = 2;
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, "STT");
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(WorkingDayEntity.WorkingDate));
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(WorkingDayEntity.EmployeeName));
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(WorkingDayEntity.Employee.EmployeeCode));
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(WorkingDayEntity.Employee.PhoneNumber));
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(WorkingDayEntity.Employee.Department));
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(WorkingDayEntity.Employee.JobTitle));
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(WorkingDayEntity.InOutState));
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(WorkingDayEntity.Time_In));
                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(WorkingDayEntity.Time_Out));
                        columnIndex++;
                        long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                              report.Workbook.Worksheets,
                              exportSheetDataModel);
                        /*SET DATA TO EXCEL END*/
                        var file = new FileInfo(excelFilePath);
                        report.SaveAs(file);
                        report.Dispose();
                        var fileInArchive = archive.CreateEntry(excelFileName, System.IO.Compression.CompressionLevel.Optimal);
                        using (var entryStream = fileInArchive.Open())
                        {
                            using (var fileInCompression = new MemoryStream(File.ReadAllBytes(excelFilePath)))
                            {
                                await fileInCompression.CopyToAsync(entryStream);
                            }
                        }
                    }
                    return excelFilePath;
                }
            }
        }
        public async Task<WorkingDayEntity> GetByIdOrDate(long id, long employeeId, DateTime date, bool isTracking = false)
        {
            var entity = await _repository.GetByIdAsync(id, true);
            date = DateTimeUtil.GetStartDate(date);
            if (entity == null && employeeId > 0)
            {
                entity = (_repositoryImp.GetEmployeeWorkingDayByDate(employeeId, date, date.AddDays(1), isTracking)).FirstOrDefault();
            }
            return entity;
        }

        public string ImportFileWorkingDayDayOffEmployee(WorkingDayOffRequestModel request, long? currentUserId)
        {
            var listErrorMesssage = new List<string>();
            var dicFormFile = request.GetFiles();
            if (dicFormFile == null || dicFormFile.Count < 1)
            {
                return $"GetFiles = null";
            }
            string filePath = UploadUtil.UploadFileExcel(dicFormFile);
            FileInfo existingFile = new FileInfo(filePath);
            if (!existingFile.Exists == true)
            {
                return $"File not exists: {filePath}";
            }

            var listEmployeeNew = new List<EmployeeEntity>();
            var listWorkingdayDayOffModel = new List<WorkingDayDayOffImportModel>();
            string reportTemplateSheet = Environment.GetEnvironmentVariable("WorkingDayDayOff");
            var listWorkingDayEntity = new List<WorkingDayEntity>();
            var listErrorMessage = new List<string>();
            using (var package = ExcelPro.LoadTempFile(filePath))
            {
                ExcelWorkbook workBook = package.Workbook;
                if (workBook == null)
                {
                    return $"Workbook is null: {filePath}";
                };
                var ws = workBook.Worksheets[0];
                if (ws == null)
                {
                    return $"Sheet not exists: {reportTemplateSheet}";
                }

                listWorkingdayDayOffModel = ReadListWorkingDayDayOffFromExcel(ws, ref listErrorMessage);
            }

            if (listErrorMessage.Count > 0)
            {
                string errorMessage = string.Join(Environment.NewLine, listErrorMessage);
                return errorMessage;
            }
            if (listWorkingdayDayOffModel == null || listWorkingdayDayOffModel.Count <= 0)
            {
                return $"Data is empty: {filePath}";
            }

            using (var transaction = this._dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var workingDayOffModel in listWorkingdayDayOffModel)
                    {
                        if (workingDayOffModel.DateDayOff == null)
                        {
                            return $"DateDayOff is required for employee {workingDayOffModel.EmployeeCode}.";
                        }
                        var employee = _employeeRepository.GetByEmpCode(workingDayOffModel.EmployeeCode);
                        if (employee == null)
                        {
                            return $"Not found employee have employeeCode {workingDayOffModel.EmployeeCode}";
                        }
                        var workingDayEntity = _repositoryImp.GetWorkingDayEmployeeByDate(employee.Id, workingDayOffModel.DateDayOff.Value);

                        //Get workingType by leave type 
                        var listWorkingTypeDesc = _workingTypeDescriptionRepository.GetListByName(workingDayOffModel.LeaveType);
                        if (listWorkingTypeDesc == null || !listWorkingTypeDesc.Any())
                        {
                            return $"Leave type '{workingDayOffModel.LeaveType}' is not mapped to a WorkingType. Please add a mapping.";
                        }

                        //Check mapping Description with WorkingType
                        WorkingTypeDescriptionEntity workingTypeDescriptionEntity = null;
                        if (listWorkingTypeDesc.Count > 1)
                        {
                            if (workingDayOffModel.Unit == 0.5)
                            {
                                workingTypeDescriptionEntity = listWorkingTypeDesc.Where(x => x.WorkingTypeItem.Name.ToUpper() == "P/2").FirstOrDefault();
                                if (workingTypeDescriptionEntity == null)
                                {
                                    return $"Leave type '{workingDayOffModel.LeaveType}' is not mapped to a WorkingType. Please add a mapping.";
                                }
                            }
                            else if (workingDayOffModel.Unit == 1)
                            {
                                workingTypeDescriptionEntity = listWorkingTypeDesc.Where(x => x.WorkingTypeItem.Name.ToUpper() == "P").FirstOrDefault();
                                if (workingTypeDescriptionEntity == null)
                                {
                                    return $"Leave type '{workingDayOffModel.LeaveType}' is not mapped to a WorkingType. Please add a mapping.";
                                }
                            }
                            else
                            {
                                return $"Leave type '{workingDayOffModel.LeaveType}' is not define with {workingDayOffModel.Unit} day.";
                            }
                        }
                        else
                        {
                            workingTypeDescriptionEntity = listWorkingTypeDesc.FirstOrDefault();
                            if (workingTypeDescriptionEntity.WorkingTypeItem.Name == "P" || workingTypeDescriptionEntity.WorkingTypeItem.Name == "P/2")
                            {
                                return "Please add Leave type for annual leave P && P/2.";
                            }
                        }

                        //Upsert WorkingDayEntity
                        if (workingDayEntity == null)
                        {
                            workingDayEntity = new WorkingDayEntity()
                            {
                                WorkingDate = workingDayOffModel.DateDayOff,
                                WorkingDayStatus = EnumWorkingDayStatus.Absent,
                                InOutState = EnumInOutTypeStatus.Outside,
                                EmployeeEntityId = employee.Id,
                                CreatedAt = DateTime.Now,
                            };
                        }
                        workingDayEntity.UpdatedAt = DateTime.Now;
                        workingDayEntity.WorkingTypeEntityId = workingTypeDescriptionEntity.WorkingTypeId;

                        if (!listWorkingDayEntity.Any(x => x.EmployeeEntityId == workingDayEntity.EmployeeEntityId
                                 && x.WorkingDate == workingDayEntity.WorkingDate))
                        {
                            listWorkingDayEntity.Add(workingDayEntity);
                        }
                        else
                        {
                            listWorkingDayEntity.RemoveAll(x => x.EmployeeEntityId == workingDayEntity.EmployeeEntityId
                                                              && x.WorkingDate == workingDayEntity.WorkingDate);

                            listWorkingDayEntity.Add(workingDayEntity);
                        }
                    }
                    if (listWorkingDayEntity.Count > 0)
                    {
                        _repositoryImp.UpSertMulti(listWorkingDayEntity, currentUserId);
                    }
                    transaction.Commit();
                    return "";
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

        private List<WorkingDayDayOffImportModel> ReadListWorkingDayDayOffFromExcel(ExcelWorksheet ws, ref List<string> listErrorMessage)
        {
            var listWorkingdayDayOffModel = new List<WorkingDayDayOffImportModel>();
            string checkCell = "D2";
            int beginRow = 2;
            string beginCell = ConvertUtil.GetCellNext(checkCell, Direct.Left, 3);
            int countItem = CommonFuncMainService.GetCountRow(ws, checkCell);
            string endCellLeft = ConvertUtil.GetCellNext(checkCell, Direct.Down, countItem - 1);
            string endCellRight = ConvertUtil.GetCellNext(endCellLeft, Direct.Right, 15);
            string rangeAddress = $"{beginCell}:{endCellRight}";
            ExcelRange targetRange = ws.Cells[rangeAddress];

            int columnIndex = 0;
            for (int row = 0; row < countItem; row++)
            {
                columnIndex = 0;
                var cellVal_DateDayOff = ConvertUtil.ConvertToNullableDateTime(targetRange.GetCellValue<object>(row, columnIndex++));
                var cellVal_DayOfWeek = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_EmployeeName = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_EmployeCode = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_TimeOffTable = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_LeaveType = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Unit = ConvertUtil.ConvertToNullableDouble(targetRange.GetCellValue<object>(row, columnIndex++));
                var cellVal_TimeUnit = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();

                var WorkingdayDayOffModel = new WorkingDayDayOffImportModel()
                {
                    DateDayOff = cellVal_DateDayOff,
                    DayOfWeek = cellVal_DayOfWeek?.Trim(),
                    EmployeeName = cellVal_EmployeeName?.Trim(),
                    EmployeeCode = cellVal_EmployeCode?.Trim(),
                    TimeOffTable = cellVal_TimeOffTable?.Trim(),
                    LeaveType = cellVal_LeaveType?.Trim(),
                    Unit = cellVal_Unit,
                    TimeUnit = cellVal_TimeUnit?.Trim()
                };
                var missingColumns = ValidateData(WorkingdayDayOffModel, row, columnIndex, beginRow);
                if (listWorkingdayDayOffModel.Any(x => x.DateDayOff == WorkingdayDayOffModel?.DateDayOff && x.EmployeeCode == WorkingdayDayOffModel?.EmployeeCode))
                {
                    var errorDupplicateMessage = ValidateDataDuplicateData(WorkingdayDayOffModel, row, beginRow);
                    listErrorMessage.Add(errorDupplicateMessage);
                }
                if (missingColumns != null && missingColumns.Any())
                {
                    listErrorMessage.AddRange(missingColumns);
                    continue;
                }
                listWorkingdayDayOffModel.Add(WorkingdayDayOffModel);
            }

            return listWorkingdayDayOffModel;
        }
        public List<string> ValidateData(WorkingDayDayOffImportModel model, int row, int column, int beginRow = 0)
        {
            int columnIndexDateDayOff = 1;
            int columnIndexEmployeeName = 3;
            int columnIndexEmployeeCode = 4;
            int columnIndexLeaveType = 6;
            int columnIndexUnit = 7;
            var missingColumns = new List<string>();
            bool hashValue = false;

            if (model.DateDayOff == null && model.EmployeeCode == null && model.EmployeeName == null && model.Unit == null && model.LeaveType == null)
            {
                return null;
            }
            if (model.DateDayOff == null)
            {
                var columnName = ConvertUtil.GetCellNameByIndex(row + beginRow, columnIndexDateDayOff);
                string errorMessage = $"Data at {columnName} is null";
                missingColumns.Add(errorMessage);
            }
            if (string.IsNullOrEmpty(model.EmployeeCode))
            {
                var columnName = ConvertUtil.GetCellNameByIndex(row + beginRow, columnIndexEmployeeCode);
                string errorMessage = $"Data at {columnName} is null";
                missingColumns.Add(errorMessage);
            }
            if (string.IsNullOrEmpty(model.EmployeeName))
            {
                var columnName = ConvertUtil.GetCellNameByIndex(row + beginRow, columnIndexEmployeeName);
                string errorMessage = $"Data at {columnName} is null";
                missingColumns.Add(errorMessage);
            }
            if (model.Unit == null || model.Unit == 0)
            {
                var columnName = ConvertUtil.GetCellNameByIndex(row + beginRow, columnIndexUnit);
                string errorMessage = $"Data at {columnName} is null";
                missingColumns.Add(errorMessage);
            }
            if (string.IsNullOrEmpty(model.LeaveType))
            {
                var columnName = ConvertUtil.GetCellNameByIndex(row + beginRow, columnIndexLeaveType);
                string errorMessage = $"Data at {columnName} is null";
                missingColumns.Add(errorMessage);
            }
            return missingColumns;
        }

        public string ValidateDataDuplicateData(WorkingDayDayOffImportModel model, int row, int beginRow = 0)
        {
            return $"{model.EmployeeName} have duplicated data day {model.DateDayOff} at row {row + beginRow}";
        }
    }
}