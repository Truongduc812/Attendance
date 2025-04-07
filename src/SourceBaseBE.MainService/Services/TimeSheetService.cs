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
using iSoft.Common.Utils;
using iSoft.ExportLibrary.Models;
using iSoft.ExportLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SourceBaseBE.Database.Models.RequestModels.Report;
using System.IO.Compression;
using System.IO;
using SourceBaseBE.Database.Models;
using SourceBaseBE.Database.Models.TrackDevice;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.MainService.Models.ResponseModels.Report;
using SourceBaseBE.MainService.Models.ResponseModels;
using iSoft.Common.CommonFunctionNS;

namespace SourceBaseBE.MainService.Services
{
    public class TimeSheetService : BaseCRUDService<TimeSheetEntity>
    {
        private UserRepository _authUserRepository;
        public TimeSheetRepository _repositoryImp;
        public EmployeeRepository _employeeRepository;
        private WorkingDayRepository WorkingDayRepository;
        private WorkingTypeRepository WorkingTypeRepository;
        private LanguageRepository _languageRepository;
        private DepartmentRepository _departmentRepository;
        private TimeSheetUpdateRepository _timeSheetUpdateRepository;
        /*[GEN-1]*/

        public TimeSheetService(CommonDBContext dbContext, ILogger<TimeSheetService> logger)
          : base(dbContext, logger)
        {
            _repository = new TimeSheetRepository(_dbContext);
            _repositoryImp = (TimeSheetRepository)_repository;
            _authUserRepository = new UserRepository(_dbContext);
            this.WorkingDayRepository = new WorkingDayRepository(_dbContext);
            this.WorkingTypeRepository = new WorkingTypeRepository(_dbContext);
            this._languageRepository = new LanguageRepository(_dbContext);
            _departmentRepository = new DepartmentRepository(_dbContext);
            _employeeRepository = new EmployeeRepository(_dbContext);
            this._timeSheetUpdateRepository = new TimeSheetUpdateRepository(_dbContext);
            /*[GEN-2]*/
        }
        public override TimeSheetEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repositoryImp.GetById(id, isDirect, isTracking);
            var entityRS = (TimeSheetEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public TimeSheetEntity GetById(long? id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repositoryImp.GetById(id, isDirect, isTracking);
            var entityRS = (TimeSheetEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override async Task<TimeSheetEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = await _repositoryImp.GetByIdAsync(id, isDirect);
            var entityRS = (TimeSheetEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override List<TimeSheetEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<TimeSheetEntity>().ToList();
            return listRS;
        }
        public List<Dictionary<string, object>> GetFormDataObjElement(TimeSheetEntity entity, long? employeeId, EmployeeEntity? employeeEntity)
        {
            string entityName = nameof(TimeSheetEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;
            var properties = typeof(TimeSheetEntity).GetProperties();
            //var propertiesDepartment = typeof(EmployeeEntity).GetProperties()
            //            .Where(p => p.Name == "Name")
            //            .ToList();
            //var combinedProperties = propertiesDepartment.Concat(properties).ToList();
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

                            var dictionary = new Dictionary<string, object> {
                            {"display_name", GetDisplayName(property.Name, entityName)},
                            {"key", property.Name},
                            {"value", property.GetValue(entity)},
                            {"type", formDataTypeAttr.TypeName.ToStringValue()},
                            {"min", formDataTypeAttr.Min},
                            {"max", formDataTypeAttr.Max},
                            {"default_value", formDataTypeAttr.DefaultVal},
                            {"isRequire",formDataTypeAttr.IsRequired },
                            };
                            if (property.Name == nameof(TimeSheetEntity.RecordedTime))
                            {
                                dictionary.Add("order", 3);
                            }

                            listRS.Add(dictionary);
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
                                    { "isRequire", formDataTypeAttr.IsRequired }
                        });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.select:
                            //objectList = new List<object>(formDataTypeAttr.Options);
                            string parentEntity = property.Name;

                            var dictionarySelect = new Dictionary<string, object>{ 
                            {"display_name", property.Name.Replace("Id","").Replace("Entity","")},
                            {"key", property.Name.Replace("Entity","")},
                            {"value", property.GetValue(entity)},
                            {"type", EnumFormDataType.select.ToStringValue()},
                            {"select_data", GetListOptionData(parentEntity, entityName, "")},
                            { "isRequire", formDataTypeAttr.IsRequired }
                            };
                            if (property.Name == nameof(TimeSheetEntity.Status))
                            {
                                dictionarySelect.Add("order", 2);
                            }

                            listRS.Add(dictionarySelect);
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
                        case EnumFormDataType.label:
                            continue;
                        case EnumFormDataType.hidden:
                            if (property.Name == "EmployeeId")
                            {
                                if (entity?.Id != null && entity?.Id > 0)
                                {
                                    string parentEntity1 = property.Name;
                                    listRS.Add(new Dictionary<string, object> {
                                    {"display_name", property.Name.Replace("Id","").Replace("Entity","")},
                                    {"key", property.Name.Replace("Entity","")},
                                    {"value", employeeEntity.Id},
                                    {"type", EnumFormDataType.select.ToStringValue()},
                                    {"select_data", GetListOptionData(parentEntity1, entityName, "")},
                                    { "isRequire", formDataTypeAttr.IsRequired },
                                    {"searchable", true},
                                    {"order", 1},
                                    {"disabled",entity.Id>0?true:false}
                                    });
                                }
                                else
                                {
                                    string parentEntity1 = property.Name;
                                    listRS.Add(new Dictionary<string, object> {
                                    {"display_name", property.Name.Replace("Id","").Replace("Entity","")},
                                    {"key", property.Name.Replace("Entity","")},
                                    {"value", employeeEntity?.Id},
                                    {"type", EnumFormDataType.select.ToStringValue()},
                                    {"select_data", GetListOptionData(parentEntity1, entityName, "")},
                                    { "isRequire", formDataTypeAttr.IsRequired },
                                    {"searchable", true},
                                    {"order", 1},
                                    });
                                }
                                
                                addedFlag = true;
                            }
                            else
                            {
                                listRS.Add(new Dictionary<string, object> {
                                  {"display_name", GetDisplayName(property.Name, entityName)},
                                  {"key", property.Name.Replace("Entity", "")},
                                  {"value", property.GetValue(entity)},
                                  {"type", formDataTypeAttr.TypeName.ToStringValue()},
                                {"isRequire",property.Name!="Id"?  formDataTypeAttr.IsRequired:false }
                                });
                            }

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
        public override TimeSheetEntity Upsert(TimeSheetEntity entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((TimeSheetRepository)_repositoryImp).Upsert(entity/*[GEN-4]*/, userId);
            var entityRS = (TimeSheetEntity)_authUserRepository.FillTrackingUser(upsertedEntity);

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
        private List<long> GetListIdChildren(TimeSheetEntity entity, string childEntity)
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
                case nameof(TimeSheetEntity.Status):
                    var status = Enum.GetNames<EnumFaceId>();
                    var valueStatus = Enum.GetValues<EnumFaceId>();
                    int count = 0;
                    listRS.AddRange(status.Select(x => new FormSelectOptionModel((int)valueStatus[count++], x)));
                    break;
                /*[GEN-6]*/
                case "EmployeeId":

                    listRS = this._employeeRepository.GetSelectData(entityName, category);
                    break;
                default:
                    break;
            }
            return listRS;
        }
        public async Task<List<TimeSheetEntity>> GetListByWdId(long wdId, bool isTracking)
        {
            if (wdId <= 0) return null;
            var list = _repositoryImp.GetTimeSheetsByWdId(wdId, isTracking);

            return list;
        }
        
        public async Task<List<TimeSheetEntity>> GetListByWd(DateTime date, long empId)
        {
            if (date == null) return null;
            var list = await _repositoryImp.GetEmpDayTimeSheet(date, empId);

            return list;
        }
        public async Task<TimeSheetPagingResponse> GetListByWdIdWithFilter(TimeSheetListRequest pagingReq)
        {
            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            var searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);

            var ret = _repositoryImp.GetListTimeSheetsByWdId(
                                                              pagingReq.WorkingdayId,
                                                              pagingReq.EmployeeId,
                                                              pagingReq.ViewDepartmentIds,
                                                              pagingReq,
                                                              filterParams,
                                                              searchParams,
                                                              sortParams);

            var disPlayProps = JsonPropertyHelper<TimesheetListResponseModel>.GetFilterProperties();
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = TimesheetListResponseModel.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;
        }
        private string getNewSheetName(string name, List<string> listSheetName, int subFix = 1)
        {
            bool existsFlag = false;
            foreach (var sheetName in listSheetName)
            {
                if (name.RemoveSpecialChar().ToUpper().Trim() == sheetName.ToUpper().Trim())
                {
                    existsFlag = true;
                    break;
                }
            }
            if (!existsFlag)
            {
                return name.RemoveSpecialChar().ToUpper().Trim();
            }
            return getNewSheetName(name + "_" + subFix.ToString(), listSheetName, subFix + 1);
        }
        public async Task<IActionResult> ExportAttendanceRecord(ExportDepartmentAttendanceRequest request, List<DepartmentEntity> departments)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var templatePath = "./wwwroot/ReportTemplate/templateTimeSheet.xlsx";
            string reportTemplateSheet = "is_template";
            string excelFileNameTemplate = "TimeSheet {0} {1}.xlsx";
            string zipFileName = "TimeSheetTotal_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".zip";
            var currentDate = new DateTime(request.DateFrom.Year, request.DateFrom.Month, 1, 0, 0, 0);

            //var zipFilePath = $"{zipFileName}.zip";
            //var departments = _departmentRepository.GetDepartmentsByListIds(request.ListDepartmentId);
            //departments.Add(null);
            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    while (currentDate <= request.DateTo)
                    {
                        var excelFileName = string.Format(excelFileNameTemplate, currentDate.Year, currentDate.Month);
                        var excelFilePath = $"./{excelFileName}";
                        bool isTemplateExist = File.Exists(templatePath);
                        _logger.LogInformation($"Template Path: {templatePath}");
                        _logger.LogInformation($"Template Path Exist: {isTemplateExist}");
                        //var templatePackage = ExcelPro.LoadTempFile(templatePath);
                        var daysOfMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);


                        ExcelPro._templatePath = templatePath;
                        using (var report = ExcelPro.LoadReportFormat())
                        {
                            foreach (var department in departments)
                            {
                                var listEmployee = _employeeRepository.GetByDepartment(department);
                                List<long> listEmployeeId = listEmployee.Select(x => x.Id).ToList();

                                int rowCount = listEmployee.Count;
                                int beginRowIndex = 2;
                                int insertEmptyRow = rowCount - 1;
                                int headerCopyColumnCount = 17;
                                double rowHeight = 27.8;

                                string sheetName = getNewSheetName(department != null ? department.Name : "UnKnown", report.Workbook.Worksheets.Select(x => x.Name).ToList());
                                ExcelWorksheet sheet1 = report.Workbook.Worksheets.Copy(reportTemplateSheet, sheetName);
                                report.Workbook.Worksheets.MoveBefore(sheetName, reportTemplateSheet);
                                sheet1.View.SetTabSelected(false);

                                /************************************************************************/
                                /*                          INSERT EMPTY ROWS START                     */
                                /************************************************************************/

                                // set formula sumarize 
                                var startColWd = 8;
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
                                var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
                                if (request.DateFrom.Month == currentDate.Month && request.DateFrom.Year == currentDate.Year)
                                {
                                    startOfMonth = new DateTime(currentDate.Year, currentDate.Month, request.DateFrom.Day, 0, 0, 0);
                                }
                                var endOfMonth = new DateTime(currentDate.Year, currentDate.Month, daysOfMonth, 0, 0, 0);
                                if (request.DateTo.Month == currentDate.Month && request.DateTo.Year == currentDate.Year)
                                {
                                    endOfMonth = DateTimeUtil.GetEndDate(request.DateTo);
                                }

                                var dicEmployeeId2Workingdays = _repositoryImp.GetListTimeSheetsForExport(
                                                            listEmployeeId,
                                                            startOfMonth,
                                                            endOfMonth);

                                List<Dictionary<object, object>> listData = new List<Dictionary<object, object>>();
                                foreach (var data in dicEmployeeId2Workingdays)
                                {
                                    var dic = new Dictionary<object, object>();
                                    dic.Add(nameof(TimeSheetExcel.MNV), data.MNV);
                                    dic.Add(nameof(TimeSheetExcel.Name), data.Name);
                                    dic.Add(nameof(TimeSheetExcel.Date), data.Date);
                                    dic.Add(nameof(TimeSheetExcel.DOW), data.DOW);
                                    dic.Add(nameof(TimeSheetExcel.RecordedTime), data.RecordedTime);
                                    dic.Add(nameof(TimeSheetExcel.Type), data.Type);
                                    dic.Add(nameof(TimeSheetExcel.Department), data.Department);
                                    listData.Add(dic);
                                }

                                /*GET DATA TO listData END*/

                                /************************************************************************/
                                /*                          SET METADATA START                  */
                                /************************************************************************/
                                ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel();
                                exportSheetDataModel.SheetIndex = sheet1.Index;
                                /*SET METADATA END*/

                                /************************************************************************/
                                /*                          SET DATA TO EXCEL START                  */
                                /************************************************************************/
                                exportSheetDataModel.BeginRowIndex = beginRowIndex;
                                exportSheetDataModel.BeginNoNumber = 1;
                                exportSheetDataModel.ListChartDataModel = listData;
                                int columnIndex = 0;
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(TimeSheetExcel.MNV));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(TimeSheetExcel.Name));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(TimeSheetExcel.Date));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(TimeSheetExcel.DOW));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(TimeSheetExcel.RecordedTime));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(TimeSheetExcel.Type));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(TimeSheetExcel.Department));
                                columnIndex++;
                                long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                                                        report.Workbook.Worksheets,
                                                        exportSheetDataModel);
                                /*SET DATA TO EXCEL END*/

                            }

                            if (File.Exists(excelFilePath))
                            {
                                File.Delete(excelFilePath);
                            }

                            report.Workbook.Worksheets.Delete(reportTemplateSheet);

                            var file = new FileInfo(excelFilePath);
                            report.SaveAs(file);
                            report.Dispose();
                        }
                        currentDate = currentDate.AddMonths(1);

                        var fileInArchive = archive.CreateEntry(excelFileName, System.IO.Compression.CompressionLevel.Optimal);
                        using (var entryStream = fileInArchive.Open())
                        {
                            using (var fileInCompression = new MemoryStream(File.ReadAllBytes(excelFilePath)))
                            {
                                await fileInCompression.CopyToAsync(entryStream);
                            }
                        }
                        if (File.Exists(excelFilePath))
                        {
                            File.Delete(excelFilePath);
                        }
                    }
                }


                return new FileContentResult(outStream.ToArray(), "application/zip")
                {
                    FileDownloadName = zipFileName
                };
            }
        }
        public Task<TimeSheetEntity> GetEmpTimeSheet(DateTime dateTime, EmployeeEntity employee, EnumFaceId inOutTypeStatus)
        {
            return _repositoryImp.GetEmpTimeSheet(dateTime, employee, inOutTypeStatus);

        }


        public async Task<TimeSheetDetailResponse> GetListIncommingDetailTimeSheet(
           EmployeeAttendanceDetailRequest request = null
          )
        {
            if (request == null || request.EmployeeId.GetValueOrDefault() == 0 || request.EmployeeId.GetValueOrDefault() < -1)
            {
                throw new ArgumentNullException("Invalid argument");
            }
            TimeSheetDetailResponse ret = new TimeSheetDetailResponse();

            // get employeeModel information
            if (request.EmployeeId <= 0 && request.EmployeeId != -1)
            {
                throw new Exception("Employee Not found");
            }

            // get summarize 
            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(request.FilterStr);
            SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(request.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(request.SortStr);


            var pendingTimeSheets = await _timeSheetUpdateRepository
              .GetIncommingTimeSheet(request.EmployeeId, request.AcceptDepartmentId, EnumApproveStatus.PENDING, request, filterParams, searchParams, sortParams);
             
            var lang = string.IsNullOrEmpty(request.Language) ? EnumLanguage.EN.ToString() : request.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<TimesheetEditListResponseModel>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = TimesheetEditListResponseModel.AddKeySearchFilterable(columns);

            pendingTimeSheets.Columns = columns;
            ret.AttendanceRecord = pendingTimeSheets;
            //
            return ret;
        }
    }
}