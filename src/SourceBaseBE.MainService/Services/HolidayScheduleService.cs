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
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;
using iSoft.Common.Models.ResponseModels;
using iSoft.Common.Utils;
using OfficeOpenXml;
using SourceBaseBE.Database.Helpers;
using System.IO;
using SourceBaseBE.Database.Models.RequestModels.Generate;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using iSoft.Common.Enums;
using NPOI.SS.UserModel;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using iSoft.ExportLibrary.Services;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using OfficeOpenXml.Sorting;
using NPOI.SS.Util;
using iSoft.Database.Repository;
using Microsoft.OpenApi.Extensions;
using Microsoft.AspNetCore.Routing.Template;
using iSoft.ExportLibrary.Models;
using Aspose.Cells;
using SourceBaseBE.MainService.CommonFuncNS;
using NPOI.POIFS.FileSystem;
using Nest;
using elFinder.NetCore;

namespace SourceBaseBE.MainService.Services
{
    public class HolidayScheduleService : BaseCRUDService<HolidayScheduleEntity>
    {
        private Database.Repository.UserRepository _authUserRepository;
        public HolidayScheduleRepository _repositoryImp;
        public LanguageRepository _languageRepository;

        /*[GEN-1]*/

        public HolidayScheduleService(CommonDBContext dbContext, ILogger<HolidayScheduleService> logger)
          : base(dbContext, logger)
        {
            _repository = new HolidayScheduleRepository(_dbContext);
            _repositoryImp = (HolidayScheduleRepository)_repository;
            _authUserRepository = new Database.Repository.UserRepository(_dbContext);
            _languageRepository = new LanguageRepository(_dbContext);

            /*[GEN-2]*/
        }
        public override HolidayScheduleEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repository.GetById(id, isDirect, isTracking);
            var entityRS = (HolidayScheduleEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override async Task<HolidayScheduleEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = await _repository.GetByIdAsync(id, isDirect);
            var entityRS = (HolidayScheduleEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override List<HolidayScheduleEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<HolidayScheduleEntity>().ToList();
            return listRS;
        }
        public override List<Dictionary<string, object>> GetFormDataObjElement(HolidayScheduleEntity entity)
        {
            string entityName = nameof(HolidayScheduleEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;
            var properties = typeof(HolidayScheduleEntity).GetProperties()
                                    .Where(p => p.Name == "Name" || p.Name == "StartDate" || p.Name == "EndDate" || p.Name == "Description" ||
                                        p.Name == "HolidayType");
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
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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
                            //objectList = new List<object>(formDataTypeAttr.Options);
                            listRS.Add(new Dictionary<string, object> {
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
      {"key", property.Name},
      {"value", property.GetValue(entity)},
      {"type", formDataTypeAttr.TypeName.ToStringValue()},
      {"select_data", GetListOptionData(property.Name,entityName,"")},
      {"default_value", formDataTypeAttr.DefaultVal},
      {"isRequire",formDataTypeAttr.IsRequired }
    });
                            addedFlag = true;
                            break;
                        case EnumFormDataType.timespan:
                            listRS.Add(new Dictionary<string, object> {
      {"display_name", JsonPropertyHelper<HolidayScheduleEntity>.GetDisplayName(property.Name)},
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

        /// <summary>
        /// UpsertIfNotExist (@GenCRUD)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override HolidayScheduleEntity Upsert(HolidayScheduleEntity entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((HolidayScheduleRepository)_repository).Upsert(entity/*[GEN-4]*/, userId);
            var entityRS = (HolidayScheduleEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
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
        private List<long> GetListIdChildren(HolidayScheduleEntity entity, string childEntity)
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
                case nameof(HolidayScheduleEntity.HolidayType):
                    var listStatus = Enum.GetValues<EnumHolidayCode>();
                    listRS = listStatus.Select(x => new FormSelectOptionModel((int)x, x.ToString())).ToList();
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

        public IEnumerable<HolidayScheduleEntity> InsertMulti(IEnumerable<HolidayScheduleEntity> entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((HolidayScheduleRepository)_repository).InsertMulti(entity/*[GEN-4]*/, userId);
            return upsertedEntity;
        }

        //public IEnumerable<HolidayScheduleEntity> UpSertMulti(IEnumerable<HolidayScheduleEntity> entity, long? userId = null)
        //{

        //  /*[GEN-3]*/
        //  var upsertedEntity = ((HolidayScheduleRepository)_repository).UpSertMulti(entity/*[GEN-4]*/, userId);
        //  return upsertedEntity;
        //}

        public HolidaySchedulePagingResponseModel GetListHolidaySchedule(
           PagingHolidayRequestModel pagingReq = null)
        {
            List<HolidaySchedulePagingResponseModel> listResponseModel = null;

            SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var ret = _repositoryImp.GetListHolidaySchedule(pagingReq, searchParams, sortParams);
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<HolidayScheduleListResponseModel>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = HolidayScheduleListResponseModel.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;

        }

        public async Task<List<HolidayScheduleEntity>> ImportFileHolidaySchedule(HolidayScheduleRequestModel request, long? currentUserId)
        {
            var dicFormFile = request.GetFiles();
            //DateTime? dateMix = null;
            //DateTime? dateMax = null;

            if (dicFormFile == null || dicFormFile.Count < 1)
            {
                throw new BaseException($"GetFiles = null");
            }
            string filePath = UploadUtil.UploadFileExcel(dicFormFile);
            FileInfo existingFile = new FileInfo(filePath);
            if (!existingFile.Exists == true)
            {
                throw new BaseException($"File not exists: {filePath}");
            }

            List<HolidayScheduleEntity> listHolidaySchedule = new List<HolidayScheduleEntity>();
            List<HolidayScheduleEntity> listHolidayScheduleCheckedExist = new List<HolidayScheduleEntity>();
            string reportTemplateSheet = Environment.GetEnvironmentVariable("HolidaySheetName");

            using (var package = ExcelPro.LoadTempFile(filePath))
            {
                ExcelWorkbook workBook = package.Workbook;
                if (workBook == null)
                {
                    throw new BaseException($"Workbook is null: {filePath}");
                };
                var ws = workBook.Worksheets[reportTemplateSheet];
                if (ws == null)
                {
                    throw new BaseException($"Sheet not exists: {reportTemplateSheet}");
                }

                listHolidaySchedule = ReadListHolidayFromExcel(ws);

            }

            if (listHolidaySchedule == null || listHolidaySchedule.Count <= 0)
            {
                throw new BaseException($"Data is empty: {filePath}");
            }

            using (var transaction = this._dbContext.Database.BeginTransaction())
            {
                try
                {
                    //_repositoryImp.DeleteAll(currentUserId, true);
                    foreach (var item in listHolidaySchedule)
                    {
                        var holidayEntity = _repositoryImp.GetByDateAndSymbelAndName(item.StartDate, item.EndDate, item.HolidayType, item.Name);
                        if (holidayEntity != null)
                        {

                            listHolidayScheduleCheckedExist.Add(holidayEntity);
                        }
                        else
                        {
                            //HolidayScheduleEntity holidayScheduleEntity = new HolidayScheduleEntity();
                            //holidayScheduleEntity.StartDate = item.StartDate;
                            //holidayScheduleEntity.EndDate = item.EndDate; 
                            //holidayScheduleEntity.HolidayType = item.HolidayType;
                            //holidayScheduleEntity.Name = item.Name;
                            listHolidayScheduleCheckedExist.Add(item);
                        }
                        //_repositoryImp.Upsert(holidayEntity);
                    }

                    _repositoryImp.UpSertMulti(listHolidayScheduleCheckedExist);

                    await SetCurrentYearHolidayToExcel(listHolidayScheduleCheckedExist);

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

            return listHolidaySchedule;
        }

        public async Task<IActionResult> ExportFileHolidaySchedule(PagingHolidayReportModel request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            //var templatePath = "./ReportTemplate/TemplateHolidaySchedule.xlsx";
            var templatePath = ConstMain.ConstTemplateFilePath;
            string sheetName = "Lịch nghỉ lễ, tết";

            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    //var currentDate = new DateTime(request.Year.Value.Year, 0, 0) ;

                    bool isTemplateExist = File.Exists(templatePath);
                    _logger.LogInformation($"Template Path: {templatePath}");
                    _logger.LogInformation($"Template Path Exist: {isTemplateExist}");
                    var templatePackage = ExcelPro.LoadTempFile(templatePath);
                    var holidaySchedules = _repositoryImp.GetByDate(request);
                    //templatePackage = ExcelPro.CloneWorkbook(ref templatePackage, "new.xlsm");
                    //ExcelPro.AddNewWorksheet(ref templatePackage, sheetName, "Lịch nghỉ lễ, tết");


                    var startRow = 4;
                    var Stt = 1;
                    int col_Country = 1;
                    int col_Date = 2;
                    int col_Name = 3;
                    int col_Symbel = 4;
                    int rowCurrent = 4;

                    DateTime? startDate = DateTime.Now;
                    DateTime? endDate = DateTime.Now;
                    DateTime? dateCompare = DateTime.Now;

                    if (holidaySchedules != null)
                    {
                        var data = holidaySchedules.OrderBy(x => x.StartDate).ToList();
                        foreach (var item in data)
                        {
                            startDate = item.StartDate;
                            dateCompare = startDate;
                            endDate = item.EndDate;
                            if (startDate == endDate)
                            {
                                ExcelPro.SetValue(ref templatePackage, sheetName, $"A{startRow}", "VNM");
                                ExcelPro.SetValue(ref templatePackage, sheetName, $"B{startRow}", startDate.Value.ToString("dd/MM/yyyy"));
                                ExcelPro.SetValue(ref templatePackage, sheetName, $"C{startRow}", item.Name);
                                ExcelPro.SetValue(ref templatePackage, sheetName, $"D{startRow}", item.HolidayType.Value.ToString());

                                startRow++;
                            }

                            else
                            {
                                int i = 1;
                                while (dateCompare <= endDate)
                                {
                                    ExcelPro.SetValue(ref templatePackage, sheetName, $"A{startRow}", "VNM");
                                    ExcelPro.SetValue(ref templatePackage, sheetName, $"B{startRow}", dateCompare.Value.ToString("dd/MM/yyyy"));
                                    ExcelPro.SetValue(ref templatePackage, sheetName, $"C{startRow}", item.Name);
                                    ExcelPro.SetValue(ref templatePackage, sheetName, $"D{startRow}", item.HolidayType.Value.ToString());
                                    dateCompare = startDate.Value.AddDays(i);
                                    i++;
                                    startRow++;
                                }
                            }
                        }
                    }

                    var path = $"./Report_Holiday_Schedule_{request.DateFrom.Value.ToString("yyyyy_MM_dd")}_To_{request.DateTo.Value.ToString("yyyyy_MM_dd")}.xlsx";
                    // delete when response done
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    templatePackage.SaveAs(new FileInfo(path));
                    //templatePackage.SaveAs(new FileInfo("\"C:\\Users\\truon\\Desktop\""));
                    var fileInArchive = archive.CreateEntry(path, System.IO.Compression.CompressionLevel.Optimal);
                    using (var entryStream = fileInArchive.Open())
                    {
                        using (var fileInCompression = new MemoryStream(File.ReadAllBytes(ConstMain.ConstTemplateFilePath)))
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
        private async Task SetCurrentYearHolidayToExcel(List<HolidayScheduleEntity> listHolidayScheduleNew)
        {
            bool changedFlag = false;
            string fileBKPath = string.Format(ConstMain.ConstTemplateBKFilePath, DateTimeUtil.GetDateTimeStr(DateTime.Now, "yyyyMMdd_HHmmss_fff"));
            try
            {
                var oldFile = ExcelPro.LoadTempFile(ConstMain.ConstTemplateFilePath);
                ExcelPro.CloneWorkbook(ref oldFile, fileBKPath);
                //FileUtil.CopyFile(ConstMain.ConstTemplateFilePath, fileBKPath);

                var templatePath = ConstMain.ConstTemplateFilePath;
                string reportTemplateSheet = Environment.GetEnvironmentVariable("HolidaySheetName");

                ExcelPro._templatePath = templatePath;

                using (var report = ExcelPro.LoadReportFormat())
                {
                    var sheet1 = report.Workbook.Worksheets[reportTemplateSheet];
                    if (sheet1 == null)
                    {
                        throw new Exception($"Invalid template: {reportTemplateSheet}");
                    }

                    var listHolidayOld = new List<HolidayScheduleEntity>();

                    listHolidayOld = ReadListHolidayFromExcel(sheet1);
                    var listHolidayMerged = MergeListHoliday(listHolidayOld, listHolidayScheduleNew);

                    var countOldRow = CommonFuncMainService.GetCountRow(sheet1, "A4");
                    if (countOldRow >= 1)
                    {
                        var endCell = ConvertUtil.GetCellNext(ConvertUtil.GetCellNext("A4", Direct.Down, countOldRow - 1), Direct.Right, 3);
                        ExcelRange targetRange0 = sheet1.Cells[$"A4:{endCell}"];
                        object[,] defaultValues = new object[countOldRow, 3 + 1];
                        for (int i = 0; i < countOldRow; i++)
                        {
                            for (int j = 0; j < 3 + 1; j++)
                            {
                                defaultValues[i, j] = "";
                            }
                        }
                        targetRange0.Value = defaultValues;
                    }

                    var listNew = listHolidayMerged;
                    int rowCount = listNew.Count;
                    int beginRowIndex = 4;
                    ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel();
                    exportSheetDataModel.SheetIndex = sheet1.Index;
                    int columnIndex = 0;
                    exportSheetDataModel.BeginRowIndex = beginRowIndex;
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(HolidayListModel.Country));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(HolidayListModel.Date));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(HolidayListModel.Name));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(HolidayListModel.Code));

                    //*******************     Get data **************************************************************** //
                    var listData = new List<Dictionary<object, object>>();
                    foreach (var holiday in listNew)
                    {
                        DateTime dt = holiday.StartDate;
                        while (dt <= holiday.EndDate)
                        {
                            var dicRow = new Dictionary<object, object>();
                            dicRow.Add(nameof(HolidayListModel.Country), "VNM");
                            dicRow.Add(nameof(HolidayListModel.Date), dt);
                            dicRow.Add(nameof(HolidayListModel.Name), holiday.Name);
                            dicRow.Add(nameof(HolidayListModel.Code), holiday.HolidayType?.ToString());
                            listData.Add(dicRow);
                            dt = dt.AddDays(1);
                        }
                    }

                    int insertEmptyRow = listData.Count - countOldRow;
                    if (insertEmptyRow > 0)
                    {
                        sheet1.InsertRow(beginRowIndex + 1, insertEmptyRow, beginRowIndex);
                    }
                    else if (insertEmptyRow < 0)
                    {
                        sheet1.DeleteRow(beginRowIndex + 1, insertEmptyRow * -1);
                    }


                    exportSheetDataModel.DicCellName2Value.Add("B1", DateTime.Now.Month);
                    exportSheetDataModel.DicCellName2Value.Add("C1", DateTime.Now.Year);
                    exportSheetDataModel.ListChartDataModel = listData;
                    long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                                                    report.Workbook.Worksheets,
                                                    exportSheetDataModel);


                    var file = new FileInfo(templatePath);
                    report.Save();
                    changedFlag = true;
                    report.Dispose();

                }
            }
            catch (Exception ex)
            {
                _logger.LogMsg(Messages.ErrException, ex);
                if (changedFlag)
                {
                    try
                    {
                        FileUtil.CopyFile(fileBKPath, ConstMain.ConstTemplateFilePath);
                    }
                    catch (Exception ex2)
                    {
                        _logger.LogInformation($"Recover template file error: {fileBKPath}");
                        _logger.LogMsg(Messages.ErrException, ex2);
                    }
                }
                throw ex;
            }
        }

        public async Task<string> SetCurrentYearHolidayToExcel(PagingHolidayReportModel request)
        {
            bool changedFlag = false;
            string fileExport = string.Format(ConstMain.ConstTemplateBKFilePath, DateTimeUtil.GetDateTimeStr(DateTime.Now, "yyyyMMdd"));
            try
            {
                // Load and clone the template file
                var oldFile = ExcelPro.LoadTempFile(ConstMain.ConstTemplateFilePath);
                ExcelPro.CloneWorkbook(ref oldFile, fileExport);

                // Load the cloned file
                using (var report = new ExcelPackage(new FileInfo(fileExport)))
                {
                    var reportTemplateSheet = Environment.GetEnvironmentVariable("HolidaySheetName");
                    string AdminSheetName = Environment.GetEnvironmentVariable("AdminSheetName");
                    var sheet1 = report.Workbook.Worksheets[reportTemplateSheet];
                    if (sheet1 == null)
                    {
                        throw new Exception($"Invalid template: {reportTemplateSheet}");
                    }

                    var listHoliday = _repositoryImp.GetByDate(request).OrderBy(x => x.StartDate);

                    var countOldRow = CommonFuncMainService.GetCountRow(sheet1, "A4");
                    if (countOldRow >= 1)
                    {
                        var endCell = ConvertUtil.GetCellNext(ConvertUtil.GetCellNext("A4", Direct.Down, countOldRow - 1), Direct.Right, 3);
                        ExcelRange targetRange0 = sheet1.Cells[$"A4:{endCell}"];
                        object[,] defaultValues = new object[countOldRow, 3 + 1];
                        for (int i = 0; i < countOldRow; i++)
                        {
                            for (int j = 0; j < 3 + 1; j++)
                            {
                                defaultValues[i, j] = "";
                            }
                        }
                        targetRange0.Value = defaultValues;
                    }

                    var listNew = listHoliday;
                    int rowCount = listNew.Count();
                    int beginRowIndex = 4;
                    ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel
                    {
                        SheetIndex = sheet1.Index,
                        BeginRowIndex = beginRowIndex
                    };
                    int columnIndex = 0;
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(HolidayListModel.Country));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(HolidayListModel.Date));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(HolidayListModel.Name));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(HolidayListModel.Code));

                    //*******************     Get data **************************************************************** //
                    var listData = new List<Dictionary<object, object>>();
                    foreach (var holiday in listNew)
                    {
                        DateTime dt = holiday.StartDate;
                        while (dt <= holiday.EndDate)
                        {
                            var dicRow = new Dictionary<object, object>
                    {
                        { nameof(HolidayListModel.Country), "VNM" },
                        { nameof(HolidayListModel.Date), dt },
                        { nameof(HolidayListModel.Name), holiday.Name },
                        { nameof(HolidayListModel.Code), holiday.HolidayType?.ToString() }
                    };
                            listData.Add(dicRow);
                            dt = dt.AddDays(1);
                        }
                    }

                    int insertEmptyRow = listData.Count - countOldRow;
                    if (insertEmptyRow > 0)
                    {
                        sheet1.InsertRow(beginRowIndex + 1, insertEmptyRow, beginRowIndex);
                    }
                    else if (insertEmptyRow < 0)
                    {
                        sheet1.DeleteRow(beginRowIndex + 1, insertEmptyRow * -1);
                    }

                    exportSheetDataModel.DicCellName2Value.Add("B1", DateTime.Now.Month);
                    exportSheetDataModel.DicCellName2Value.Add("C1", DateTime.Now.Year);
                    exportSheetDataModel.ListChartDataModel = listData;
                    long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                                                  report.Workbook.Worksheets,
                                                  exportSheetDataModel);

                    // Save the changes to the cloned file
                    report.Save();
                    oldFile.Workbook.Worksheets.Delete(AdminSheetName);
                    oldFile.Save();
                    changedFlag = true;
                }
                return fileExport;
            }
            catch (Exception ex)
            {
                _logger.LogMsg(Messages.ErrException, ex);
                if (changedFlag)
                {
                    try
                    {
                        FileUtil.CopyFile(fileExport, ConstMain.ConstTemplateFilePath);
                    }
                    catch (Exception ex2)
                    {
                        _logger.LogInformation($"Recover template file error: {fileExport}");
                        _logger.LogMsg(Messages.ErrException, ex2);
                    }
                }
                throw ex;
            }
        }
        public List<HolidayScheduleEntity> ReadListHolidayFromExcel(ExcelWorksheet ws)
        {
            string checkCell = "A4";
            string beginCell = checkCell;
            int countItem = CommonFuncMainService.GetCountRow(ws, checkCell);
            string endCellLeft = ConvertUtil.GetCellNext(checkCell, Direct.Down, countItem - 1);
            string endCellRight = ConvertUtil.GetCellNext(endCellLeft, Direct.Right, 3);
            string rangeAddress = $"{beginCell}:{endCellRight}";
            ExcelRange targetRange = ws.Cells[rangeAddress];
            List<HolidayScheduleEntity> listHolidaySchedule = new List<HolidayScheduleEntity>();

            var listHolidayCode = Enum.GetNames(typeof(EnumHolidayCode));

            int columnIndex = 0;
            for (int row = 0; row < countItem; row++)
            {
                columnIndex = 0;
                var cellValCountry = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString() ?? "";
                var cellValDate = ConvertUtil.ConvertToNullableDateTime(targetRange.GetCellValue<object>(row, columnIndex++));
                var cellValName = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString() ?? "";
                var cellValCode = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString() ?? "";

                if (cellValCountry.IsNullOrEmpty()
                  || cellValDate == null
                  || cellValName.IsNullOrEmpty()
                  || cellValCode.IsNullOrEmpty())
                {
                    continue;
                }

                var holidayType = listHolidayCode.Where(x => x.ToLower() == cellValCode.ToLower()).FirstOrDefault();

                listHolidaySchedule.Add(new HolidayScheduleEntity()
                {
                    HolidayType = (EnumHolidayCode?)Enum.Parse(typeof(EnumHolidayCode), holidayType),
                    StartDate = cellValDate.Value,
                    EndDate = cellValDate.Value,
                    Name = cellValName,
                });
            }
            return listHolidaySchedule;
        }



        private List<HolidayScheduleEntity> MergeListHoliday(
          List<HolidayScheduleEntity> listHolidayOld,
          List<HolidayScheduleEntity> listHolidayNew)
        {
            List<HolidayScheduleEntity> listRS = new List<HolidayScheduleEntity>();
            var dicStartDate2Holiday = new Dictionary<string, HolidayScheduleEntity>();

            foreach (HolidayScheduleEntity holiday in listHolidayNew)
            {
                if (holiday.StartDate != null)
                {
                    if (!dicStartDate2Holiday.ContainsKey(holiday.StartDate.ToString("dd-MM-yyyy")))
                    {
                        dicStartDate2Holiday.Add(holiday.StartDate.ToString("dd-MM-yyyy"), holiday);
                    }
                    else
                    {
                        dicStartDate2Holiday[holiday.StartDate.ToString("dd-MM-yyyy")] = holiday;
                    }
                }
            }

            foreach (HolidayScheduleEntity employee in listHolidayOld)
            {
                if (employee.StartDate != null)
                {
                    if (!dicStartDate2Holiday.ContainsKey(employee.StartDate.ToString("dd-MM-yyyy")))
                    {
                        listRS.Add(employee);
                    }
                }
            }
            listRS.AddRange(listHolidayNew);
            return listRS;
        }
    }
}
