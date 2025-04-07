using Serilog;
using SourceBaseBE.Database.Repository;
using SourceBaseBE.Database.DBContexts;
using System;
using iSoft.Common.Exceptions;
using System.Data;
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
using SourceBaseBE.MainService.Models.RequestModels.Report;
using SourceBaseBE.MainService.Models.ResponseModels.Report;
using SourceBaseBE.Database.Models.RequestModels;
using iSoft.ExportLibrary.Services;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using iSoft.Common.Utils;
using OfficeOpenXml;
using System.IO.Compression;
using SourceBaseBE.Database.Models.RequestModels.Report;
using iSoft.ExportLibrary.Models;
using Sprache;
using SourceBaseBE.MainService.CommonFuncNS;
using SourceBaseBE.MainService.Models;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.EntityFrameworkCore;
using Aspose.Cells;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml.Drawing;
using System.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SourceBaseBE.Database;
using MathNet.Numerics.Statistics.Mcmc;
using NPOI.POIFS.FileSystem;
using Nest;
using Elasticsearch.Net;
using iSoft.Redis.Services;
using iSoft.Common.ExtensionMethods;
using System.Diagnostics.Contracts;

namespace SourceBaseBE.MainService.Services
{
    public class EmployeeService : BaseCRUDService<EmployeeEntity>
    {
        private UserRepository _authUserRepository;
        private EmployeeRepository _employeeRepository;
        private TimeSheetRepository _timeSheetRepository;
        private LanguageRepository _languageRepository;
        private WorkingDayRepository _workingdayRepository;
        private WorkingDayUpdateRepository workingDayUpdateRepository;
        private WorkingTypeRepository _workingTypeRepository;
        private DepartmentRepository _departmentRepository;
        private JobTitleRepository _jobTitleRepository;
        private DepartmentAdminRepository _departmentAdminRepository;
        private HolidayScheduleRepository _holidayScheduleRepository;

        private WorkingDayService _workingDayService;
        private const int constTotalCol = 12;

        /*[GEN-1]*/

        public EmployeeService(CommonDBContext dbContext, ILogger<EmployeeService> logger, WorkingDayService workingDayService)
          : base(dbContext, logger)
        {
            _repository = new EmployeeRepository(_dbContext);
            _employeeRepository = (EmployeeRepository)_repository;
            _authUserRepository = new UserRepository(_dbContext);
            _timeSheetRepository = new TimeSheetRepository(_dbContext);
            _languageRepository = new LanguageRepository(_dbContext);
            _workingdayRepository = new WorkingDayRepository(_dbContext);
            _workingTypeRepository = new WorkingTypeRepository(_dbContext);
            _departmentRepository = new DepartmentRepository(_dbContext);
            _departmentAdminRepository = new DepartmentAdminRepository(_dbContext);
            _jobTitleRepository = new JobTitleRepository(_dbContext);
            workingDayUpdateRepository = new WorkingDayUpdateRepository(_dbContext);
            _holidayScheduleRepository = new HolidayScheduleRepository(_dbContext);

            _workingDayService = workingDayService;
            /*[GEN-2]*/
        }
        public override EmployeeEntity GetById(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _repository.GetById(id, isDirect, isTracking);
            var entityRS = (EmployeeEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public EmployeeEntity GetById(long? id, bool isDirect = false, bool isTracking = true)
        {
            var entity = _employeeRepository.GetById(id, isDirect, isTracking);
            var entityRS = (EmployeeEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }

        public override async Task<EmployeeEntity> GetByIdAsync(long id, bool isDirect = false, bool isTracking = true)
        {
            var entity = await _repository.GetByIdAsync(id, isDirect);
            var entityRS = (EmployeeEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }
        public override List<EmployeeEntity> GetList(PagingRequestModel pagingReq = null)
        {
            var list = _repository.GetList(pagingReq);
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<EmployeeEntity>().ToList();
            return listRS;
        }
        /// <summary>
        /// UpsertIfNotExist (@GenCRUD)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EmployeeEntity Upsert(EmployeeEntity entity, long? userId = null)
        {
            var existEmployeeMachine = _employeeRepository.GetByEmpMachineCode(entity.EmployeeMachineCode);
            var existEmployeeCode = _employeeRepository.GetByEmpCode(entity.EmployeeCode);
            if (existEmployeeCode != null)
            {
                if (existEmployeeCode.Id != entity.Id)
                {
                    throw new Exception($"EmployeeCode {entity.EmployeeCode} has been used!");
                }
            }
            if (existEmployeeMachine != null)
            {
                if (existEmployeeMachine.Id != entity.Id)
                {
                    throw new Exception($"Employee Machine Code {entity.EmployeeMachineCode} has been used!");
                }
            }

            var upsertedEntity = ((EmployeeRepository)_repository).Upsert(entity/*[GEN-4]*/, userId);
            var entityRS = (EmployeeEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
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
        private List<long> GetListIdChildren(EmployeeEntity entity, string childEntity)
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
                case nameof(EmployeeEntity.Department):
                    var departments = _departmentRepository.GetAll();
                    listRS.AddRange(departments.Select(x => new FormSelectOptionModel(x.Id, x.Name)));
                    break;
                case nameof(EmployeeEntity.JobTitle):
                    var titles = _jobTitleRepository.GetAll();
                    listRS.AddRange(titles.Select(x => new FormSelectOptionModel(x.Id, x.Name)));
                    break;
                case nameof(EmployeeEntity.Gender):
                    var genderVals = Enum.GetNames<EnumGender>();
                    var genderKeyss = Enum.GetValues<EnumGender>();
                    for (int i = 0; i < genderKeyss.Length; i++)
                    {
                        listRS.Add(new FormSelectOptionModel((int)genderKeyss[i], genderVals[i]));
                    }
                    break;
                case nameof(EmployeeEntity.Contract):
                    var listContract = Enum.GetValues<EnumTypeContract>();
                    listRS = listContract.Select(x => new FormSelectOptionModel((int)x, x.ToString())).ToList();
                    break;

                /*[GEN-6]*/
                default:
                    break;
            }
            return listRS;
        }

        public IEnumerable<EmployeeEntity> InsertMulti(IEnumerable<EmployeeEntity> entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((EmployeeRepository)_repository).InsertMulti(entity/*[GEN-4]*/, userId);
            return upsertedEntity;
        }

        public IEnumerable<EmployeeEntity> UpdateMulti(IEnumerable<EmployeeEntity> entity, long? userId = null)
        {

            /*[GEN-3]*/
            var upsertedEntity = ((EmployeeRepository)_repository).UpdateMulti(entity/*[GEN-4]*/, userId);
            return upsertedEntity;
        }

        public EmployeeEntity GetByName(string name, bool isDirect = false)
        {
            var entity = _employeeRepository.GetByEmpCode(name, isDirect);
            var entityRS = (EmployeeEntity)_authUserRepository.FillTrackingUser(entity);
            return entityRS;
        }

        public List<EmployeeEntity> GetAll()
        {
            var list = _employeeRepository.GetAll();
            var listRS = _authUserRepository.FillTrackingUser(list.Cast<BaseCRUDEntity>().ToList()).Cast<EmployeeEntity>().ToList();
            return listRS;
        }
        public List<FormSelectOptionModel> GetAll(List<long> acceptDepartmentIds)
        {
            var list = _employeeRepository.GetSelectData(acceptDepartmentIds);

            return list;
        }

        public async Task<EmployeePagingResponseModel> GetListAttendanceReport(
           TotalReportListRequest pagingReq = null,
           long? currentUserId = null)
        {
            List<DashboardResponseModel> listResponseModel = null;
            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var ret = await _employeeRepository.GetListAttendanceReport(pagingReq, filterParams, searchParams, sortParams, currentUserId);
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<EmployeeListAttendanceResponseModel>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = EmployeeListAttendanceResponseModel.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;
        }
        public async Task<AttendanceDetailResponse> GetListDetailAttendanceReport(
           EmployeeAttendanceDetailRequest request = null
          )
        {
            if (request == null || request.EmployeeId.GetValueOrDefault() <= 0) throw new ArgumentNullException("Invalid argument");
            AttendanceDetailResponse ret = new AttendanceDetailResponse();
            // get employeeModel information
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId.Value);
            if (employee == null) throw new Exception("Employee Not found");
            ret.EmployeeInformation = GetEmployeeDetailData(employee);

            // get summarize 
            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(request.FilterStr);
            SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(request.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(request.SortStr);

            var listWorkingType = _workingTypeRepository.GetAll();

            var workingDays0 = _workingdayRepository.GetListWorkingDayByEmployeeId2(request, filterParams, searchParams, sortParams);
            var workingDays = _workingDayService.GetListWorkingDayByEmployeeId(request.EmployeeId.Value, workingDays0, listWorkingType, request.DateFrom, request.DateTo);

            var lang = string.IsNullOrEmpty(request.Language) ? EnumLanguage.EN.ToString() : request.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<DetailAttendanceResponse>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = DetailAttendanceResponse.AddKeySearchFilterable(columns);
            workingDays.Columns = columns;
            ret.SummarizeData = GetSummarizeAttendanceFormData(workingDays.rawDatas);
            ret.AttendanceRecord = workingDays;
            //
            return ret;
        }


        public async Task<AttendanceEditDetailResponse> GetListIncommingDetailAttendanceReport(
           EmployeeAttendanceDetailRequest request = null
          )
        {
            if (request == null || request.EmployeeId.GetValueOrDefault() <= 0) throw new ArgumentNullException("Invalid argument");
            AttendanceEditDetailResponse ret = new AttendanceEditDetailResponse();
            // get employeeModel information
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId.Value);
            if (employee == null) throw new Exception("Employee Not found");
            // get summarize 
            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(request.FilterStr);
            SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(request.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(request.SortStr);
            var pendingWorkingDays = await workingDayUpdateRepository
              .GetIncommingRecord(employee.Id, EnumApproveStatus.PENDING, request, filterParams, searchParams, sortParams);
            var lang = string.IsNullOrEmpty(request.Language) ? EnumLanguage.EN.ToString() : request.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<DetailEditAttendanceResponse>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = DetailEditAttendanceResponse.AddKeySearchFilterable(columns);
            pendingWorkingDays.Columns = columns;
            ret.AttendanceRecord = pendingWorkingDays;
            //
            return ret;
        }
        public async Task<IActionResult> ExportAttendanceRecord(ExportDepartmentAttendanceRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var templatePath = ConstMain.ConstTemplateFilePath;
            string reportTemplateSheet = "is_template";
            string excelFileNameTemplate = "BANG CHAM CONG {0}__{1}_{2}.xlsx";
            string zipFileName = "BANG_CHAM_CONG_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".zip";
            var requestDate = new DateTime(request.DateFrom.Year, request.DateFrom.Month, 1, 0, 0, 0);
            var listOTData = new List<Dictionary<object, object>>();
            var listHoliday = _holidayScheduleRepository.GetList();
            var listWorkingType = _workingTypeRepository.GetAll();
            var listWkTypeOT = await _workingTypeRepository.GetOTTypes();
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
            var departments = _departmentRepository.GetDepartmentsByListIds(request.ListDepartmentId);
            // add sheet Total
            var totalDepart = new DepartmentEntity()
            {
                Name = "Total",
                Employees = new List<EmployeeEntity>()
            };
            departments.Add(totalDepart);

            var listEmpTotal = new List<EmployeeEntity>();
            //
            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    while (requestDate <= request.DateTo)
                    {
                        var daysOfMonth = DateTime.DaysInMonth(requestDate.Year, requestDate.Month);
                        var startDate = request.DateFrom;
                        var endDate = request.DateTo;
                        var stringContracts = request.List_Contract?.Split(",").Select(x => x + "_");
                        var excelFileName = string.Format(excelFileNameTemplate, requestDate.ToString("MM_yyyy"), $"{DateTime.Now.ToString("HH_mm")}", $"{stringContracts}");
                        var excelFilePath = $"./{excelFileName}";
                        bool isTemplateExist = File.Exists(templatePath);
                        _logger.LogInformation($"Template Path: {templatePath}");
                        _logger.LogInformation($"Template Path Exist: {isTemplateExist}");
                        //var templatePackage = ExcelPro.LoadTempFile(templatePath);


                        ExcelPro._templatePath = templatePath;
                        using (var report = ExcelPro.LoadReportFormat())
                        {
                            //remove sheet employee
                            report.Workbook.Worksheets.Delete(Environment.GetEnvironmentVariable("EmployeeSheetName"));
                            foreach (var department in departments)
                            {
                                List<EmployeeEntity> listEmployee = null;
                                if (department.Name != "Total")
                                {
                                    listEmployee = _employeeRepository.GetByDepartmentWithContract(department, request.ListContractId);
                                }
                                if (listEmployee != null)
                                {
                                    listEmpTotal.AddRange(listEmployee);
                                }
                                else
                                {
                                    listEmployee = listEmpTotal;
                                }
                                List<long> listEmployeeId = listEmployee.Select(x => x.Id).ToList();

                                int rowCount = listEmployee.Count;
                                int beginRowIndex = 7;
                                int insertEmptyRow = rowCount - 1;
                                int headerCopyColumnCount = 18;
                                double rowHeight = 27.8;
                                int columnIndexDeviation = 60;
                                int columnIndexEditOT = 100;

                                string sheetName = getNewSheetName(department.Name, report.Workbook.Worksheets.Select(x => x.Name).ToList());
                                ExcelWorksheet sheet1 = report.Workbook.Worksheets.Copy(reportTemplateSheet, sheetName);
                                report.Workbook.Worksheets.MoveBefore(sheetName, reportTemplateSheet);
                                sheet1.View.SetTabSelected(false);

                                /************************************************************************/
                                /*                          SET METADATA START                  */
                                /************************************************************************/
                                #region SET METADATA START
                                ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel();
                                exportSheetDataModel.SheetIndex = sheet1.Index;
                                exportSheetDataModel.DicCellName2Value.Add("V3", requestDate.Month);
                                exportSheetDataModel.DicCellName2Value.Add("W3", requestDate.Year);

                                exportSheetDataModel.DicCellName2Value.Add(ConvertUtil.GetCellNext("AN13", Direct.Down, insertEmptyRow), department.DepartmentAdmins?.FirstOrDefault(x => x.Role == EnumDepartmentAdmin.Admin1)?.User?.ItemEmployee?.Name); // admin 1 name
                                exportSheetDataModel.DicCellName2Value.Add(ConvertUtil.GetCellNext("AY13", Direct.Down, insertEmptyRow), department.DepartmentAdmins?.FirstOrDefault(x => x.Role == EnumDepartmentAdmin.Admin2)?.User?.ItemEmployee?.Name); // admin2 name
                                exportSheetDataModel.DicCellName2Value.Add(ConvertUtil.GetCellNext("BD13", Direct.Down, insertEmptyRow), department.DepartmentAdmins?.FirstOrDefault(x => x.Role == EnumDepartmentAdmin.Admin3)?.User?.ItemEmployee?.Name);// admin3 name

                                #endregion
                                /*SET METADATA END*/

                                /************************************************************************/
                                /*                          DELETE COLUMN                  */
                                /************************************************************************/
                                #region DELETE COLUMN 
                                if (daysOfMonth != 31)
                                {
                                    //sheet1.DeleteColumn(39 - (31 - daysOfMonth), 31 - daysOfMonth);
                                    ExcelPro.HideColumn(sheet1, Enumerable.Range(39 - (31 - daysOfMonth), 31 - daysOfMonth).ToArray());
                                }
                                // hide columndeviation
                                var start = 39 - ((31 - daysOfMonth) + daysOfMonth - request.DateTo.Day);
                                //sheet1.DeleteColumn(start, daysOfMonth - request.DateTo.Day - 1);
                                ExcelPro.HideColumn(sheet1, Enumerable.Range(columnIndexDeviation, daysOfMonth + 3).ToArray());
                                #endregion

                                /************************************************************************/
                                /*                          INSERT EMPTY ROWS START                     */
                                /************************************************************************/
                                #region INSERT EMPTY ROWS START
                                //var colInd = ExcelPro.GetColumnNumber("AM") - (31 - daysOfMonth);
                                var colInd = ExcelPro.GetColumnNumber("AM");
                                string cellCopy = $"{ExcelPro.GetExcelColumnName(colInd)}{beginRowIndex}";
                                var startcolSummarize = ConvertUtil.GetColumnIndex(cellCopy);
                                var startColWd = 8; // default startColWd

                                var titleRange = ExcelPro.GetExcelRangeFromIndex(startColWd, beginRowIndex - 1, startcolSummarize - 1 - (31 - daysOfMonth), beginRowIndex - 1, true);
                                var dataRange = ExcelPro.GetExcelRangeFromIndex(startColWd, beginRowIndex, startcolSummarize - 1 - (31 - daysOfMonth), beginRowIndex);
                                var sumRange = ExcelPro.GetExcelRangeFromIndex(startcolSummarize + 1, beginRowIndex, startcolSummarize + 5 /*- (31 - daysOfMonth)*/, beginRowIndex);

                                ////
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize, $"=SUM(COUNTIFS({titleRange}" + ",{\"HLD\",\"WD\",\"XMS\"}))" + $"-SUM({sumRange})");
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 1, $"=COUNTIF({dataRange},\"UP\")");
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 2, $"=COUNTIF({dataRange},\"S\")");
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 3, $"=COUNTIF({dataRange},\"S75\")");
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 4, $"=COUNTIF({dataRange},\"MR\")");
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 5, $"=COUNTIF({dataRange},\"SW\")");
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 6, GetOT150Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 7, GetNightShift30Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 8, GetOT200Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 10, GetOTNight270Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 11, GetOT300Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 12, GetOT390Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 13, GetMealFormula(daysOfMonth, 6, beginRowIndex, startColWd));
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 14, GetPFormula(dataRange));
                                ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 16, GetTotalOTHour(startcolSummarize + 16, beginRowIndex));
                                //
                                if (insertEmptyRow >= 1)
                                {
                                    sheet1.InsertRow(beginRowIndex + 1, insertEmptyRow, beginRowIndex);
                                }
                                // set formula sumarize 

                                if (insertEmptyRow >= 1)
                                {
                                    for (int i = 0; i < insertEmptyRow - 1; i++)
                                    {
                                        sheet1.Rows[beginRowIndex + 1 + i].Height = rowHeight;
                                    }
                                    for (var j = 0; j < headerCopyColumnCount; j++)
                                    {
                                        string cellCopySource = ConvertUtil.GetCellNext(cellCopy, Direct.Right, j);
                                        string cellDesk1 = ConvertUtil.GetCellNext(cellCopySource, Direct.Down, 1);
                                        string cellDesk2 = ConvertUtil.GetCellNext(cellDesk1, Direct.Down, insertEmptyRow - 1);
                                        ExcelRange sourceRange = sheet1.Cells[cellCopySource];
                                        ExcelRange targetRange = sheet1.Cells[$"{cellDesk1}:{cellDesk2}"];
                                        targetRange.FormulaR1C1 = sourceRange.FormulaR1C1;
                                    }
                                }

                                #endregion
                                /*INSERT EMPTY ROWS END*/

                                /************************************************************************/
                                /*                          GET DATA TO listData START                  */
                                /************************************************************************/
                                #region GET DATA TO listData START
                                var startOfMonth = new DateTime(requestDate.Year, requestDate.Month, 1, 0, 0, 0);
                                if (requestDate.Month == request.DateFrom.Month && requestDate.Year == request.DateFrom.Year)
                                {
                                    startOfMonth = new DateTime(requestDate.Year, requestDate.Month, request.DateFrom.Day, 0, 0, 0);
                                }
                                var endOfMonth = new DateTime(requestDate.Year, requestDate.Month, daysOfMonth, 0, 0, 0);
                                if (requestDate.Month == request.DateTo.Month && requestDate.Year == request.DateTo.Year)
                                {
                                    endOfMonth = new DateTime(requestDate.Year, requestDate.Month, request.DateTo.Day, 0, 0, 0);
                                }
                                var dicEmployeeId2Workingdays = _workingdayRepository.GetEmployeeWorkingDayByDate(
                                    listEmployeeId,
                                    startOfMonth,
                                    endOfMonth);

                                foreach (var keyVal in dicEmployeeId2Workingdays)
                                {
                                    var employeeId = keyVal.Key;
                                    var listWorkingDay = keyVal.Value;
                                    // Set recommentType
                                    var dicWorkingDayId2Type = _workingDayService.CalculateMonthWorkingType(employeeId, listWorkingDay, listWorkingType, request.DateFrom, request.DateTo, dicHolidayScheduleStartDate);
                                    for (int i = 0; i < listWorkingDay.Count; i++)
                                    {
                                        var item = listWorkingDay[i];
                                        if (dicWorkingDayId2Type.ContainsKey(item.Id))
                                        {
                                            item.RecommendType = dicWorkingDayId2Type[item.Id].WorkingType.Code;
                                            if (item.TimeDeviation == null)
                                            {
                                                item.TimeDeviation = dicWorkingDayId2Type[item.Id].TimeDeviatioinInSeconds;
                                            }
                                            item.WorkingDayHighlight = dicWorkingDayId2Type[item.Id].workingDayHighlight;

                                            if (item.WorkingType == null)
                                            {
                                                // nếu tăng ca ngày t7 chủ nhật thì trực tiếp set OT mà k cần đợi approve => request by Mr.Đức ngày 20/06/2024
                                                //if ((listHoliday.Where(x => x.StartDate <= item.WorkingDate && x.EndDate >= item.WorkingDate).FirstOrDefault() != null) || item.WorkingDate.GetValueOrDefault().DayOfWeek == DayOfWeek.Sunday || item.WorkingDate.GetValueOrDefault().DayOfWeek == DayOfWeek.Saturday)
                                                if (IsWeekenOrHoliday(item.WorkingDate.Value, dicHolidayScheduleStartDate))
                                                {
                                                    item.WorkingType = listWorkingType.FirstOrDefault(x => x.Code == item.RecommendType);
                                                    item.WorkingTypeEntityId = item.WorkingType?.Id;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogMsg(Messages.ErrException, $"Employee: {employeeId}, WorkingDayObj: {item.Id} have not recommendType");
                                        }
                                    }
                                }

                                List<Dictionary<object, object>> listData = new List<Dictionary<object, object>>();
                                List<Dictionary<object, object>> listDataTimeDeviation = new List<Dictionary<object, object>>();
                                List<Dictionary<object, object>> listDataEditOT = new List<Dictionary<object, object>>();
                                EmployeeEntity employee = null;
                                for (int eIndex = 0; eIndex < listEmployee.Count; eIndex++)
                                {
                                    employee = listEmployee[eIndex];
                                    var dic = new Dictionary<object, object>();
                                    var dic_deviation = new Dictionary<object, object>();
                                    var dic_OT_Edit = new Dictionary<object, object>();
                                    dic.Add(nameof(EmployeeEntity.EmployeeCode), employee.EmployeeCode);
                                    dic.Add(nameof(EmployeeEntity.Name), employee.Name);
                                    dic.Add(nameof(EmployeeEntity.JoiningDate), employee.JoiningDate == null ? "" : employee.JoiningDate.Value.ToString(ConstDateTimeFormat.DDMMYYYY));
                                    dic.Add(nameof(EmployeeEntity.JobTitle), employee.JobTitle == null ? "" : employee.JobTitle.Name);
                                    dic.Add(nameof(EmployeeEntity.Department), employee.Department == null ? "" : employee.Department.Name);

                                    if (dicEmployeeId2Workingdays.ContainsKey(employee.Id))
                                    {
                                        var listWorkingday = dicEmployeeId2Workingdays[employee.Id];
                                        foreach (var wo in listWorkingday)
                                        {
                                            // Default workingType
                                            object workingType = EnumWorkingDayType.Type_P;
                                            int code = 0;
                                            bool isNumFormat = false;
                                            if (wo.WorkingDate == null)
                                            {
                                                _logger.LogMsg(Messages.ErrBaseException, $"Employee {employee.EmployeeCode}, WorkingDate == null");
                                                continue;
                                            }
                                            int day = wo.WorkingDate.Value.Day;
                                            if (wo.WorkingType != null)
                                            {

                                                if (int.TryParse(wo.WorkingType.Code, out code))
                                                {
                                                    workingType = code;
                                                }
                                                else
                                                {
                                                    workingType = wo.WorkingType.Code;
                                                }
                                                // highlight Edit OTs
                                                var otType = WorkingShiftModel.IsOTType(workingType.ToString());
                                                if (otType > 0)
                                                {
                                                    string keyOTEdit = "otEdit_day_" + day;
                                                    if (!dic_OT_Edit.ContainsKey(keyOTEdit))
                                                    {
                                                        dic_OT_Edit.Add(keyOTEdit, otType);
                                                    }
                                                    else
                                                    {
                                                        dic_OT_Edit[keyOTEdit] = otType;
                                                    }
                                                }
                                            }

                                            else if (wo.RecommendType != null)
                                            {
                                                double? newTimeDeviation = wo.TimeDeviation;
                                                wo.RecommendType = WorkingShiftModel.ConvertOTType2DefaultType(wo.RecommendType, wo.WorkingDate, dicHolidayScheduleStartDate, ref newTimeDeviation);
                                                if (newTimeDeviation != wo.TimeDeviation)
                                                {
                                                    _logger.LogInformation($"[ExportAttendanceRecord] TimeDeviation Updated, oldDeviation: {wo.TimeDeviation}, newTimeDeviation: {newTimeDeviation}, WorkingDayId: {wo.WorkingDayId}");
                                                    wo.TimeDeviation = newTimeDeviation;
                                                }

                                                if (int.TryParse(wo.RecommendType, out code))
                                                {
                                                    workingType = code;
                                                }
                                                else
                                                {
                                                    workingType = wo.RecommendType;
                                                }
                                                // 


                                            }


                                            string key = "day_" + day;
                                            string keyDeviation = "deviation_day_" + day;
                                            if (!dic_deviation.ContainsKey(keyDeviation))
                                            {
                                                if (wo.TimeDeviation != null)
                                                {
                                                    dic_deviation.Add(keyDeviation, Math.Floor((double)(wo.TimeDeviation / 3600)));
                                                }
                                                else
                                                {
                                                    dic_deviation.Add(keyDeviation, 0D);
                                                }
                                            }

                                            if (dic.ContainsKey(key))
                                            {
                                                workingType = "ERROR";
                                                dic[key] = workingType;
                                                _logger.LogMsg(Messages.ErrBaseException, $"Employee {employee.EmployeeCode}, WorkingType {wo.WorkingDate.Value.ToString(ConstDateTimeFormat.YYYYMMDD)}");
                                            }
                                            else
                                            {
                                                dic.Add(key, workingType);
                                            }
                                        }

                                    }

                                    // Set default values: isWeekend or isHoliday: Type_0, else: Type_P
                                    for (int day = 1; day <= daysOfMonth; day++)
                                    {
                                        string key = "day_" + day;
                                        string keyDeviation = "deviation_day_" + day;
                                        string keyOTEdit = "otEdit_day_" + day;
                                        if (!dic.ContainsKey(key) || dic[key].ToString() == EnumWorkingDayType.Type_P.ToString())
                                        {
                                            DateTime checkDay = new DateTime(requestDate.Year, requestDate.Month, day);
                                            string holidayKey = DateTimeUtil.GetDateTimeStr(checkDay, ConstDateTimeFormat.YYYYMMDD);

                                            var isWeekend = checkDay.DayOfWeek == DayOfWeek.Sunday || checkDay.DayOfWeek == DayOfWeek.Saturday;
                                            var isHoliday = dicHolidayScheduleStartDate.ContainsKey(holidayKey);

                                            if (isWeekend || isHoliday)
                                            {
                                                int code2 = 0;
                                                if (int.TryParse(EnumWorkingDayType.Type_0, out code2))
                                                {
                                                    dic[key] = code2;
                                                }
                                            }
                                        }
                                        if (!dic_deviation.ContainsKey(keyDeviation))
                                        {
                                            dic_deviation.Add(keyDeviation, 0D);
                                        }
                                        if (!dic_OT_Edit.ContainsKey(keyOTEdit))
                                        {
                                            dic_OT_Edit.Add(keyOTEdit, 0D);
                                        }
                                    }

                                    listData.Add(dic);
                                    if (department.Name != "Total")
                                    {
                                        listDataTimeDeviation.Add(dic_deviation);
                                    }
                                    listDataEditOT.Add(dic_OT_Edit);
                                }

                                #endregion
                                /*GET DATA TO listData END*/

                                /************************************************************************/
                                /*                          SET DATA TO EXCEL START                  */
                                /************************************************************************/
                                #region SET DATA TO EXCEL START 
                                // Set default value to excel file: Type_P
                                if (rowCount >= 1)
                                {
                                    var endCell = ConvertUtil.GetCellNext(ConvertUtil.GetCellNext("H7", Direct.Down, rowCount - 1), Direct.Right, daysOfMonth - 1);
                                    ExcelRange targetRange0 = sheet1.Cells[$"H7:{endCell}"];
                                    object[,] defaultValues = new object[listData.Count, daysOfMonth];
                                    for (int i = 0; i < listData.Count; i++)
                                    {
                                        for (int j = 0; j < daysOfMonth; j++)
                                        {
                                            defaultValues[i, j] = EnumWorkingDayType.Type_P;
                                        }
                                    }
                                    targetRange0.Value = defaultValues;
                                }

                                exportSheetDataModel.BeginRowIndex = beginRowIndex;
                                exportSheetDataModel.BeginNoNumber = 1;
                                exportSheetDataModel.ListChartDataModel = listData;
                                int columnIndex = 0;
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, "[No.]");
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.EmployeeCode));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.Name));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.JoiningDate));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.JobTitle));
                                exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.Department));
                                columnIndex++;
                                for (int i = 1; i <= daysOfMonth; i++)
                                {
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, "day_" + i.ToString());
                                }

                                long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                                    report.Workbook.Worksheets,
                                    exportSheetDataModel,
                                    EnumWorkingDayType.Type_P); // Set default value: Type_P

                                // Write time-deviation
                                exportSheetDataModel.DicCellName2Value.Clear();
                                exportSheetDataModel.ListChartDataModel.Clear();
                                exportSheetDataModel.ListExcelRangeStyle.Clear();
                                exportSheetDataModel.DicColumnIndex2EnvKey.Clear();
                                exportSheetDataModel.ListChartDataModel = listDataTimeDeviation;
                                for (int i = 1; i <= daysOfMonth; i++)
                                {
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndexDeviation + i, "deviation_day_" + i.ToString());
                                }

                                long insertedRowCount2 = ExportExcelService.SetFormatExcelHistory(
                                    report.Workbook.Worksheets,
                                    exportSheetDataModel,
                                    "");

                                int beginRowIndex2 = beginRowIndex;
                                int beginColumnIndex = 8;
                                int rowIndex2 = beginRowIndex2;
                                int columnIndex2 = beginColumnIndex;
                                foreach (var dataTimeDeviation in listDataTimeDeviation)
                                {
                                    columnIndex2 = beginColumnIndex;
                                    for (int i = 1; i <= 31; i++)
                                    {
                                        string keyDeviation = "deviation_day_" + i;
                                        if (dataTimeDeviation.ContainsKey(keyDeviation) && dataTimeDeviation[keyDeviation] != null)
                                        {
                                            double? timeDeviationVal = (double?)dataTimeDeviation[keyDeviation];
                                            if (timeDeviationVal != 0)
                                            {
                                                if (timeDeviationVal < 0)
                                                {
                                                    long? seconds = (long?)Math.Abs((decimal)timeDeviationVal);
                                                    var commentStr = DateTimeUtil.GetHumanStrByHours_TextHours(seconds, false);
                                                    ExcelRange commentRange = sheet1.Cells[rowIndex2, columnIndex2];
                                                    commentRange.AddComment($"Time Deviation: - {commentStr}");
                                                }
                                                else
                                                {
                                                    long? seconds = (long?)Math.Abs((decimal)timeDeviationVal);
                                                    var commentStr = DateTimeUtil.GetHumanStrByHours_TextHours(seconds, false);
                                                    ExcelRange commentRange = sheet1.Cells[rowIndex2, columnIndex2];
                                                    commentRange.AddComment($"Time Deviation: + {commentStr}");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Do nothing
                                        }

                                        columnIndex2++;
                                    }
                                    rowIndex2++;
                                }


                                // Write HightLightOT Edit
                                exportSheetDataModel.DicCellName2Value.Clear();
                                exportSheetDataModel.ListChartDataModel.Clear();
                                exportSheetDataModel.ListExcelRangeStyle.Clear();
                                exportSheetDataModel.DicColumnIndex2EnvKey.Clear();
                                exportSheetDataModel.ListChartDataModel = listDataEditOT;
                                for (int i = 1; i <= daysOfMonth; i++)
                                {
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndexEditOT + i, $"otEdit_day_{i}");
                                }

                                insertedRowCount2 = ExportExcelService.SetFormatExcelHistory(
                                   report.Workbook.Worksheets,
                                   exportSheetDataModel,
                                   "");
                                #endregion
                                /*SET DATA TO EXCEL END*/
                            }
                            // remove unneccessary sheet
                            report.Workbook.Worksheets.Delete(reportTemplateSheet);
                            report.Workbook.Worksheets.Delete(Environment.GetEnvironmentVariable("AdminSheetName"));
                            var file = new FileInfo(excelFilePath);
                            report.SaveAs(file);
                            report.Dispose();
                        }
                        requestDate = requestDate.AddMonths(1);

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

        public async Task<IActionResult> ExportSumarizeOTTimeEmployee(ExportOTTimeRequest request)
        {
            try
            {
                //Get search, filter from User
                Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(request.FilterStr);
                SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(request.SearchStr, true);
                Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(request.SortStr);

                if (request == null) throw new ArgumentNullException(nameof(request));
                var templatePath = ConstMain.ConstTemplateOTTimeFilePath;
                string reportTemplateSheet = "is_template";
                string excelFileNameTemplate = "Sumarize_OT_Time_{0}_{1}_{2}.xlsx";
                string zipFileName = "Sumarize_OT_Time_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".zip";
                var requestDate = new DateTime(request.DateFrom.Year, request.DateFrom.Month, 1, 0, 0, 0);
                var listOTData = new List<Dictionary<object, object>>();
                var listHoliday = _holidayScheduleRepository.GetList();
                var listWorkingType = _workingTypeRepository.GetAll();
                var listWkTypeOT = await _workingTypeRepository.GetOTTypes();
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
                var departments = _departmentRepository.GetDepartmentsByListIds(request.ListDepartmentId);
                // add sheet Total
                var totalDepart = new DepartmentEntity()
                {
                    Name = "Total",
                    Employees = new List<EmployeeEntity>()
                };
                departments.Add(totalDepart);

                var listEmpTotal = new List<EmployeeEntity>();
                //
                using (var outStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                    {
                        while (requestDate <= request.DateTo)
                        {
                            var daysOfMonth = DateTime.DaysInMonth(requestDate.Year, requestDate.Month);
                            var startDate = request.DateFrom;
                            var endDate = request.DateTo;
                            var stringContracts = request.List_Contract?.Split(",").Select(x => x + "_");
                            var excelFileName = string.Format(excelFileNameTemplate, requestDate.ToString("MM_yyyy"), $"{DateTime.Now.ToString("HH_mm")}", $"{stringContracts}");
                            var excelFilePath = $"./{excelFileName}";
                            bool isTemplateExist = File.Exists(templatePath);
                            _logger.LogInformation($"Template Path: {templatePath}");
                            _logger.LogInformation($"Template Path Exist: {isTemplateExist}");
                            //var templatePackage = ExcelPro.LoadTempFile(templatePath);

                            ExcelPro._templatePath = templatePath;
                            using (var report = ExcelPro.LoadReportFormat())
                            {
                                /*INSERT EMPTY ROWS END*/
                                /************************************************************************/
                                /*                          GET DATA TO listData START                  */
                                /************************************************************************/
                                foreach (var department in departments)
                                {
                                    List<EmployeeEntity> listEmployee = null;
                                    if (department.Name != "Total")
                                    {
                                        listEmployee = _employeeRepository.GetListEmployeeToExportOTTime(department, request, filterParams, searchParams, sortParams, listWkTypeOT);

                                    }
                                    else
                                    {
                                        if (listEmpTotal == null || !listEmpTotal.Any())
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            listEmployee = listEmpTotal.OrderBy(x => x.Name).ThenBy(x => x.Id).ToList();
                                        }
                                    }
                                    if (listEmployee == null || !listEmployee.Any())
                                    {
                                        continue;
                                    }
                                    List<long> listEmployeeId = listEmployee.Select(x => x.Id).ToList();

                                    #region GET DATA TO listData START
                                    var startOfMonth = new DateTime(requestDate.Year, requestDate.Month, 1, 0, 0, 0);
                                    if (requestDate.Month == request.DateFrom.Month && requestDate.Year == request.DateFrom.Year)
                                    {
                                        startOfMonth = new DateTime(requestDate.Year, requestDate.Month, request.DateFrom.Day, 0, 0, 0);
                                    }
                                    var endOfMonth = new DateTime(requestDate.Year, requestDate.Month, daysOfMonth, 0, 0, 0);
                                    if (requestDate.Month == request.DateTo.Month && requestDate.Year == request.DateTo.Year)
                                    {
                                        endOfMonth = new DateTime(requestDate.Year, requestDate.Month, request.DateTo.Day, 0, 0, 0);
                                    }
                                    var dicEmployeeId2Workingdays = _workingdayRepository.GetEmployeeWorkingDayByDate(
                                        listEmployeeId,
                                        startOfMonth,
                                        endOfMonth);

                                    foreach (var keyVal in dicEmployeeId2Workingdays)
                                    {
                                        var employeeId = keyVal.Key;
                                        var listWorkingDay = keyVal.Value;

                                        // Set recommentType
                                        var dicWorkingDayId2Type = _workingDayService.CalculateMonthWorkingType(employeeId, listWorkingDay, listWorkingType, request.DateFrom, request.DateTo, dicHolidayScheduleStartDate);

                                        for (int i = 0; i < listWorkingDay.Count; i++)
                                        {
                                            var item = listWorkingDay[i];
                                            if (dicWorkingDayId2Type.ContainsKey(item.Id))
                                            {
                                                item.RecommendType = dicWorkingDayId2Type[item.Id].WorkingType.Code;
                                                if (item.TimeDeviation == null)
                                                {
                                                    item.TimeDeviation = dicWorkingDayId2Type[item.Id].TimeDeviatioinInSeconds;
                                                }
                                                item.WorkingDayHighlight = dicWorkingDayId2Type[item.Id].workingDayHighlight;

                                                if (item.WorkingType == null)
                                                {
                                                    // nếu tăng ca ngày t7 chủ nhật thì trực tiếp set OT mà k cần đợi approve => request by Mr.Đức ngày 20/06/2024
                                                    //if ((listHoliday.Where(x => x.StartDate <= item.WorkingDate && x.EndDate >= item.WorkingDate).FirstOrDefault() != null) || item.WorkingDate.GetValueOrDefault().DayOfWeek == DayOfWeek.Sunday || item.WorkingDate.GetValueOrDefault().DayOfWeek == DayOfWeek.Saturday)
                                                    if (IsWeekenOrHoliday(item.WorkingDate.Value, dicHolidayScheduleStartDate))
                                                    {
                                                        item.WorkingType = listWorkingType.FirstOrDefault(x => x.Code == item.RecommendType);
                                                        item.WorkingTypeEntityId = item.WorkingType?.Id;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                _logger.LogMsg(Messages.ErrException, $"Employee: {employeeId}, WorkingDayObj: {item.Id} have not recommendType");
                                            }
                                        }

                                        //Apply filter workingType
                                        listWorkingDay = OTListResponseModel.PrepareDetailReportWhereQueryFilterV2(listWorkingDay, filterParams, listWkTypeOT);
                                        listWorkingDay = OTListResponseModel.PrepareDetailReportWhereQuerySearchV2(listWorkingDay, searchParams);
                                        listWorkingDay = OTListResponseModel.PrepareDetailReportQuerySortV2(listWorkingDay, sortParams);

                                    }
                                    //Remove employee not have OT
                                    EmployeeEntity employee = null;
                                    int totalNormalWorkingHour = 0;
                                    listEmployee.RemoveAll(employee => !dicEmployeeId2Workingdays.ContainsKey(employee.Id) ||
                                                                       !HasWorkingDayOTType(dicEmployeeId2Workingdays[employee.Id],
                                                                       dicHolidayScheduleStartDate));

                                    if (listEmployee != null && department.Name != "Total")
                                    {
                                        listEmpTotal.AddRange(listEmployee);
                                    }
                                    //else
                                    //{
                                    //    listEmployee = listEmpTotal;
                                    //}

                                    if (!listEmployee.Any())
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        listEmployee = listEmployee.OrderBy(x => x.Name).ThenBy(x => x.Id).ToList();
                                    }

                                    //Insert data excel
                                    int rowCount = listEmployee.Count;
                                    int beginRowIndex = 7;
                                    int insertEmptyRow = rowCount - 1;
                                    int headerCopyColumnCount = 18;
                                    double rowHeight = 27.8;
                                    int columnIndexDeviation = 60;
                                    int columnIndexEditOT = 100;

                                    string sheetName = getNewSheetName(department.Name, report.Workbook.Worksheets.Select(x => x.Name).ToList());
                                    ExcelWorksheet sheet1 = report.Workbook.Worksheets.Copy(reportTemplateSheet, sheetName);
                                    report.Workbook.Worksheets.MoveBefore(sheetName, reportTemplateSheet);
                                    sheet1.View.SetTabSelected(false);

                                    /************************************************************************/
                                    /*                          SET METADATA START                  */
                                    /************************************************************************/
                                    #region SET METADATA START
                                    ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel();
                                    exportSheetDataModel.SheetIndex = sheet1.Index;
                                    exportSheetDataModel.DicCellName2Value.Add("V3", requestDate.Month);
                                    exportSheetDataModel.DicCellName2Value.Add("W3", requestDate.Year);

                                    exportSheetDataModel.DicCellName2Value.Add(ConvertUtil.GetCellNext("AN13", Direct.Down, insertEmptyRow), department.DepartmentAdmins?.FirstOrDefault(x => x.Role == EnumDepartmentAdmin.Admin1)?.User?.ItemEmployee?.Name); // admin 1 name
                                    exportSheetDataModel.DicCellName2Value.Add(ConvertUtil.GetCellNext("AY13", Direct.Down, insertEmptyRow), department.DepartmentAdmins?.FirstOrDefault(x => x.Role == EnumDepartmentAdmin.Admin2)?.User?.ItemEmployee?.Name); // admin2 name
                                    exportSheetDataModel.DicCellName2Value.Add(ConvertUtil.GetCellNext("BD13", Direct.Down, insertEmptyRow), department.DepartmentAdmins?.FirstOrDefault(x => x.Role == EnumDepartmentAdmin.Admin3)?.User?.ItemEmployee?.Name);// admin3 name

                                    #endregion
                                    /*SET METADATA END*/

                                    /************************************************************************/
                                    /*                          DELETE COLUMN                  */
                                    /************************************************************************/
                                    #region DELETE COLUMN 
                                    if (daysOfMonth != 31)
                                    {
                                        //sheet1.DeleteColumn(39 - (31 - daysOfMonth), 31 - daysOfMonth);
                                        ExcelPro.HideColumn(sheet1, Enumerable.Range(39 - (31 - daysOfMonth), 31 - daysOfMonth).ToArray());
                                    }
                                    // hide columndeviation
                                    var start = 39 - ((31 - daysOfMonth) + daysOfMonth - request.DateTo.Day);
                                    //sheet1.DeleteColumn(start, daysOfMonth - request.DateTo.Day - 1);
                                    ExcelPro.HideColumn(sheet1, Enumerable.Range(columnIndexDeviation, daysOfMonth + 3).ToArray());
                                    #endregion

                                    /************************************************************************/
                                    /*                          INSERT EMPTY ROWS START                     */
                                    /************************************************************************/
                                    #region INSERT EMPTY ROWS START
                                    //var colInd = ExcelPro.GetColumnNumber("AM") - (31 - daysOfMonth);
                                    var colInd = ExcelPro.GetColumnNumber("AN");
                                    string cellCopy = $"{ExcelPro.GetExcelColumnName(colInd)}{beginRowIndex}";
                                    var startcolSummarize = ConvertUtil.GetColumnIndex(cellCopy);
                                    var startColWd = 8; // default startColWd

                                    var titleRange = ExcelPro.GetExcelRangeFromIndex(startColWd, beginRowIndex - 1, startcolSummarize - 1 - (31 - daysOfMonth), beginRowIndex - 1, true);
                                    var dataRange = ExcelPro.GetExcelRangeFromIndex(startColWd, beginRowIndex, startcolSummarize - 1 - (31 - daysOfMonth), beginRowIndex);

                                    //// Set formulator
                                    ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize, GetOT150Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                    ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 1, GetNightShift30Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                    ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 2, GetOT200Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                    ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 3, GetOTNight270Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                    ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 4, GetOT300Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                    ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 5, GetOT390Formula(daysOfMonth, 6, beginRowIndex, startColWd));
                                    ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 6, GetMealFormula(daysOfMonth, 6, beginRowIndex, startColWd));
                                    ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 7, GetFormulaTotalOTPerMonth(startcolSummarize + 7, beginRowIndex));
                                    ExcelPro.SetFormula(report, sheetName, beginRowIndex, startcolSummarize + 8, GetFormulaTotalPercentOTPerMonth(startcolSummarize + 8, beginRowIndex));
                                    //
                                    if (insertEmptyRow >= 1)
                                    {
                                        sheet1.InsertRow(beginRowIndex + 1, insertEmptyRow, beginRowIndex);
                                    }
                                    // set formula sumarize 

                                    if (insertEmptyRow >= 1)
                                    {
                                        for (int i = 0; i < insertEmptyRow - 1; i++)
                                        {
                                            sheet1.Rows[beginRowIndex + 1 + i].Height = rowHeight;
                                        }
                                        for (var j = 0; j < headerCopyColumnCount; j++)
                                        {
                                            string cellCopySource = ConvertUtil.GetCellNext(cellCopy, Direct.Right, j);
                                            string cellDesk1 = ConvertUtil.GetCellNext(cellCopySource, Direct.Down, 1);
                                            string cellDesk2 = ConvertUtil.GetCellNext(cellDesk1, Direct.Down, insertEmptyRow - 1);
                                            ExcelRange sourceRange = sheet1.Cells[cellCopySource];
                                            ExcelRange targetRange = sheet1.Cells[$"{cellDesk1}:{cellDesk2}"];
                                            targetRange.FormulaR1C1 = sourceRange.FormulaR1C1;
                                        }
                                    }

                                    #endregion



                                    List<Dictionary<object, object>> listData = new List<Dictionary<object, object>>();
                                    List<Dictionary<object, object>> listDataTimeDeviation = new List<Dictionary<object, object>>();
                                    List<Dictionary<object, object>> listDataEditOT = new List<Dictionary<object, object>>();

                                    for (int eIndex = 0; eIndex < listEmployee.Count; eIndex++)
                                    {
                                        totalNormalWorkingHour = 0;
                                        employee = listEmployee[eIndex];
                                        var dic = new Dictionary<object, object>();
                                        var dic_deviation = new Dictionary<object, object>();
                                        var dic_OT_Edit = new Dictionary<object, object>();

                                        if (dicEmployeeId2Workingdays.ContainsKey(employee.Id))
                                        {
                                            var listWorkingday = dicEmployeeId2Workingdays[employee.Id];

                                            ////Check the working day with a type that includes OT
                                            //var resultCheckHaveOTType = HasWorkingDayOTType(listWorkingday, dicHolidayScheduleStartDate);
                                            //if (!resultCheckHaveOTType)
                                            //{
                                            //    continue;
                                            //}
                                            dic.Add(nameof(EmployeeEntity.EmployeeCode), employee.EmployeeCode);
                                            dic.Add(nameof(EmployeeEntity.Name), employee.Name);
                                            dic.Add(nameof(EmployeeEntity.JoiningDate), employee.JoiningDate == null ? "" : employee.JoiningDate.Value.ToString(ConstDateTimeFormat.DDMMYYYY));
                                            dic.Add(nameof(EmployeeEntity.JobTitle), employee.JobTitle == null ? "" : employee.JobTitle.Name);
                                            dic.Add(nameof(EmployeeEntity.Department), employee.Department == null ? "" : employee.Department.Name);

                                            foreach (var wo in listWorkingday)
                                            {
                                                // Default workingType
                                                object workingType = EnumWorkingDayType.Type_P;
                                                //object workingType = null;
                                                int code = 0;
                                                bool isNumFormat = false;
                                                if (wo.WorkingDate == null)
                                                {
                                                    _logger.LogMsg(Messages.ErrBaseException, $"Employee {employee.EmployeeCode}, WorkingDate == null");
                                                    continue;
                                                }
                                                int day = wo.WorkingDate.Value.Day;
                                                if (wo.WorkingType != null)
                                                {
                                                    if (int.TryParse(wo.WorkingType.Code, out code))
                                                    {
                                                        workingType = code;
                                                    }
                                                    else
                                                    {
                                                        workingType = wo.WorkingType.Code;
                                                    }

                                                    //Calculate normal working hour 
                                                    if (!IsWeekenOrHoliday(wo.WorkingDate.Value, dicHolidayScheduleStartDate))
                                                    {
                                                        totalNormalWorkingHour += GetTotalNormalWorkingHour(workingType.ToString());
                                                    }

                                                    // highlight Edit OTs
                                                    var otType = WorkingShiftModel.IsOTType(workingType.ToString());
                                                    if (otType > 0)
                                                    {
                                                        //string keyOTEdit = "otEdit_day_" + day;
                                                        //if (!dic_OT_Edit.ContainsKey(keyOTEdit))
                                                        //{
                                                        //    dic_OT_Edit.Add(keyOTEdit, otType);
                                                        //}
                                                        //else
                                                        //{
                                                        //    dic_OT_Edit[keyOTEdit] = otType;
                                                        //}
                                                    }
                                                    else if (otType == 0)
                                                    {
                                                        workingType = 0;
                                                    }

                                                }

                                                else if (wo.RecommendType != null)
                                                {
                                                    double? newTimeDeviation = wo.TimeDeviation;
                                                    wo.RecommendType = WorkingShiftModel.ConvertOTType2DefaultType(wo.RecommendType, wo.WorkingDate, dicHolidayScheduleStartDate, ref newTimeDeviation);
                                                    if (newTimeDeviation != wo.TimeDeviation)
                                                    {
                                                        _logger.LogInformation($"[ExportAttendanceRecord] TimeDeviation Updated, oldDeviation: {wo.TimeDeviation}, newTimeDeviation: {newTimeDeviation}, WorkingDayId: {wo.WorkingDayId}");
                                                        wo.TimeDeviation = newTimeDeviation;
                                                    }

                                                    if (int.TryParse(wo.RecommendType, out code))
                                                    {
                                                        workingType = code;
                                                    }
                                                    else
                                                    {
                                                        workingType = wo.RecommendType;
                                                    }

                                                    //Calculate normal working hour 
                                                    if (!IsWeekenOrHoliday(wo.WorkingDate.Value, dicHolidayScheduleStartDate))
                                                    {
                                                        totalNormalWorkingHour += GetTotalNormalWorkingHour(workingType.ToString());
                                                    }

                                                    //Chekc OT Type
                                                    var otType = WorkingShiftModel.IsOTType(workingType.ToString());
                                                    if (otType == 0)
                                                    {
                                                        workingType = 0;
                                                    }
                                                }

                                                string key = "day_" + day;
                                                //string keyDeviation = "deviation_day_" + day;
                                                //if (!dic_deviation.ContainsKey(keyDeviation))
                                                //{
                                                //    if (wo.TimeDeviation != null)
                                                //    {
                                                //        dic_deviation.Add(keyDeviation, Math.Floor((double)(wo.TimeDeviation / 3600)));
                                                //    }
                                                //    else
                                                //    {
                                                //        dic_deviation.Add(keyDeviation, 0D);
                                                //    }
                                                //}

                                                if (dic.ContainsKey(key))
                                                {
                                                    workingType = "ERROR";
                                                    dic[key] = workingType;
                                                    _logger.LogMsg(Messages.ErrBaseException, $"Employee {employee.EmployeeCode}, WorkingType {wo.WorkingDate.Value.ToString(ConstDateTimeFormat.YYYYMMDD)}");
                                                }
                                                else
                                                {
                                                    dic.Add(key, workingType);
                                                }
                                            }

                                            //Add total working to dic 
                                            dic.Add("TotalNormalWorkingHour", totalNormalWorkingHour);

                                            // Set default values: isWeekend or isHoliday: Type_0, else: Type_P
                                            for (int day = 1; day <= daysOfMonth; day++)
                                            {
                                                string key = "day_" + day;
                                                //string keyDeviation = "deviation_day_" + day;
                                                string keyOTEdit = "otEdit_day_" + day;
                                                if (!dic.ContainsKey(key) || dic[key].ToString() == EnumWorkingDayType.Type_P.ToString())
                                                {
                                                    DateTime checkDay = new DateTime(requestDate.Year, requestDate.Month, day);
                                                    string holidayKey = DateTimeUtil.GetDateTimeStr(checkDay, ConstDateTimeFormat.YYYYMMDD);

                                                    var isWeekend = checkDay.DayOfWeek == DayOfWeek.Sunday || checkDay.DayOfWeek == DayOfWeek.Saturday;
                                                    var isHoliday = dicHolidayScheduleStartDate.ContainsKey(holidayKey);

                                                    if (isWeekend || isHoliday)
                                                    {
                                                        int code2 = 0;
                                                        if (int.TryParse(EnumWorkingDayType.Type_0, out code2))
                                                        {
                                                            dic[key] = code2;
                                                        }
                                                    }
                                                }
                                                //if (!dic_deviation.ContainsKey(keyDeviation))
                                                //{
                                                //    dic_deviation.Add(keyDeviation, 0D);
                                                //}
                                                //if (!dic_OT_Edit.ContainsKey(keyOTEdit))
                                                //{
                                                //    dic_OT_Edit.Add(keyOTEdit, 0D);
                                                //}
                                            }

                                            listData.Add(dic);
                                            //if (department.Name != "Total")
                                            //{
                                            //    listDataTimeDeviation.Add(dic_deviation);
                                            //}
                                            //listDataEditOT.Add(dic_OT_Edit);
                                        }
                                    }

                                    #endregion
                                    /*GET DATA TO listData END*/

                                    /************************************************************************/
                                    /*                          SET DATA TO EXCEL START                  */
                                    /************************************************************************/
                                    #region SET DATA TO EXCEL START  

                                    exportSheetDataModel.BeginRowIndex = beginRowIndex;
                                    exportSheetDataModel.BeginNoNumber = 1;
                                    exportSheetDataModel.ListChartDataModel = listData;
                                    int columnIndex = 0;
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, "[No.]");
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.EmployeeCode));
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.Name));
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.JoiningDate));
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.JobTitle));
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeEntity.Department));
                                    columnIndex++;
                                    for (int i = 1; i <= daysOfMonth; i++)
                                    {
                                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, "day_" + i.ToString());
                                    }
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex + (31 - daysOfMonth), "TotalNormalWorkingHour");

                                    long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                                        report.Workbook.Worksheets,
                                        exportSheetDataModel,
                                        0); // Set default value: Type_P

                                    // Write HightLightOT Edit
                                    exportSheetDataModel.DicCellName2Value.Clear();
                                    exportSheetDataModel.ListChartDataModel.Clear();
                                    exportSheetDataModel.ListExcelRangeStyle.Clear();
                                    exportSheetDataModel.DicColumnIndex2EnvKey.Clear();
                                    exportSheetDataModel.ListChartDataModel = listDataEditOT;
                                    for (int i = 1; i <= daysOfMonth; i++)
                                    {
                                        exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndexEditOT + i, $"otEdit_day_{i}");
                                    }

                                    long insertedRowCount2 = ExportExcelService.SetFormatExcelHistory(
                                       report.Workbook.Worksheets,
                                       exportSheetDataModel,
                                       "");
                                    #endregion
                                    /*SET DATA TO EXCEL END*/
                                }
                                // remove unneccessary sheet
                                report.Workbook.Worksheets.Delete(reportTemplateSheet);
                                report.Workbook.Worksheets.Delete(Environment.GetEnvironmentVariable("AdminSheetName"));
                                var file = new FileInfo(excelFilePath);
                                report.SaveAs(file);
                                report.Dispose();
                            }
                            requestDate = requestDate.AddMonths(1);

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
            catch (Exception ex)
            {

                throw;
            }
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

        public async Task<IActionResult> ExportEmployeeAttendanceRecord(ExportEmployeeAttendanceRequest request)
        {
            //if (request == null) throw new ArgumentNullException(nameof(request));
            //var templatePath = "./wwwroot/ReportTemplate/template.xlsx";

            //using (var outStream = new MemoryStream())
            //{
            //	using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
            //	{
            //		var requestDate = new DateTime(request.DateFrom.Year, request.DateFrom.Month, 1, 0, 0, 0);

            //		while (requestDate <= request.DateTo)
            //		{
            //			bool isTemplateExist = File.Exists(templatePath);
            //			_logger.LogInformation($"Template Path: {templatePath}");
            //			_logger.LogInformation($"Template Path Exist: {isTemplateExist}");
            //			var templatePackage = ExcelPro.LoadTempFile(templatePath);
            //			var daysOfMonth = DateTime.DaysInMonth(requestDate.Year, requestDate.Month);
            //			var employess = await _employeeRepository.GetByListId(request.ListEmployeeId);
            //			string sheetName = "Employees";
            //			templatePackage = ExcelPro.CloneWorkbook(ref templatePackage, "new.xlsx");
            //			ExcelPro.AddNewWorksheet(ref templatePackage, sheetName, "template");
            //			// set month
            //			ExcelPro.SetValue(ref templatePackage, sheetName, "T3", requestDate.Month.ToString());
            //			// set year
            //			ExcelPro.SetValue(ref templatePackage, sheetName, "U3", requestDate.Year.ToString());
            //			// set Requester Infos
            //			var startRow = 7;

            //			// set days of month 
            //			var startday = 1;
            //			var startcol = 6;
            //			var startcolSummarize = 6;
            //			while (startday <= daysOfMonth)
            //			{
            //				if (startcol >= 34)
            //				{
            //					ExcelPro.CopyStyles(ref templatePackage, sheetName, 4, startcol - 1, 4, startcol);
            //					ExcelPro.CopyStyles(ref templatePackage, sheetName, 5, startcol - 1, 5, startcol);
            //					ExcelPro.CopyStyles(ref templatePackage, sheetName, 6, startcol - 1, 6, startcol);
            //				}
            //				var date = $"DATE({requestDate.Year},{requestDate.Month},{startday})";
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, 4, startcol, $"={date}");
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, 5, startcol, $"=IF(WEEKDAY({date},1)=1,\"CN\",WEEKDAY({date}))");
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, 6, startcol, $"=IF(ISNA(VLOOKUP({date}," +
            //				  $"Schedule,3,0)),IF(WEEKDAY({date},2)>5,\"OFF\",\"WD\"),VLOOKUP({date},Schedule,3,0))");
            //				startday++;
            //				startcol++;
            //			}
            //			// set titles summarize
            //			for (int i = 0; i < constTotalCol; i++)
            //			{
            //				ExcelPro.SetValue(ref templatePackage, sheetName, 4, startcol + i, "Total");
            //			}
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 4, startcol + constTotalCol, "Meal");
            //			// 
            //			startcolSummarize = startcol;
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol, "working days");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 1, "Unpaid day");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 2, "Paid 100%");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 3, "Paid 75%");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 4, "Maternity Leave");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 5, "Stop Working");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 6, "OT hrs (150%)");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 7, "Nigh shift (30%)");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 8, "OT hrs (200%)");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 9, "OT Nigh shift (270%)");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 10, "OT hrs (300%)");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 11, "OT Nigh shift (390%)");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 12, "Allowance");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 6, startcol + 13, "P");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 4, startcol + 14, "Notice");
            //			ExcelPro.SetValue(ref templatePackage, sheetName, 4, startcol + 15, "TỔNG GIỜ \r\nTĂNG CA TRONG THÁNG (KHÔNG VƯỢT 40H)");
            //			// set working day type
            //			foreach (var employeeModel in employess)
            //			{
            //				// set code
            //				ExcelPro.SetValue(ref templatePackage, sheetName, $"B{startRow}", employeeModel.EmployeeCode);
            //				// set name
            //				ExcelPro.SetValue(ref templatePackage, sheetName, $"C{startRow}", employeeModel.Name);
            //				// set joining date
            //				ExcelPro.SetValue(ref templatePackage, sheetName, $"D{startRow}", employeeModel.JoiningDate.GetValueOrDefault().ToString("dd-mm-yyyy"));

            //				// set job title
            //				ExcelPro.SetValue(ref templatePackage, sheetName, $"E{startRow}", employeeModel.JobTitle?.Name);

            //				// working day type
            //				var startOfMonth = new DateTime(requestDate.Year, requestDate.Month, 1, 0, 0, 0);
            //				var endOfMonth = new DateTime(requestDate.Year, requestDate.Month, daysOfMonth, 0, 0, 0);
            //				var workingdays = await _workingdayRepository.GetEmployeeWorkingDayByDate(employeeModel.Id, startOfMonth, endOfMonth);
            //				foreach (var wod in workingdays)
            //				{
            //					// set working type in each day
            //					var startDayOfWeekCol = 6;
            //					ExcelPro.SetValue(ref templatePackage, sheetName, startRow - 1, startDayOfWeekCol++, (wod.WorkingType != null && wod.WorkingType.Code != "0") ? wod.WorkingType.Code : "");

            //				}

            //				// set formula sumarize 
            //				var titleRange = ExcelPro.GetExcelRangeFromIndex(6, 6, startcolSummarize - 1, 6);
            //				var dataRange = ExcelPro.GetExcelRangeFromIndex(6, startRow, startcolSummarize - 1, startRow);
            //				var sumRange = ExcelPro.GetExcelRangeFromIndex(startcolSummarize + 1, startRow, startcolSummarize + 5, startRow);
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize, $"=SUM(COUNTIFS({titleRange}" + ",{\"HLD\",\"WD\",\"XMS\"}))" + $"-SUM({sumRange})");
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 1, $"=COUNTIF({dataRange},\"UP\")");
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 2, $"=COUNTIF({dataRange},\"S\")");
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 3, $"=COUNTIF({dataRange},\"S75\")");
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 4, $"=COUNTIF({dataRange},\"MR\")");
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 5, $"=COUNTIF({dataRange},\"SW\")");
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 6, GetOT150Formula(daysOfMonth, 6, startRow));
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 7, GetNightShift30Formula(daysOfMonth, 6, startRow));
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 8, GetOT200Formula(daysOfMonth, 6, startRow));
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 9, GetOTNight270Formula(daysOfMonth, 6, startRow));
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 10, GetOT300Formula(daysOfMonth, 6, startRow));
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 11, GetOT390Formula(daysOfMonth, 6, startRow));
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 12, GetMealFormula(daysOfMonth, 6, startRow));
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 13, GetPFormula(dataRange));
            //				ExcelPro.SetFormula(ref templatePackage, sheetName, startRow, startcolSummarize + 15, GetTotalOTHour(startcolSummarize + 15, startRow));
            //				startRow++;
            //			}
            //			var path = $"./ReportAttendance_{requestDate.ToString("dd-MM-yyyy")}_Employees_{request.List_Employees.Select(x => x + "_")}_.xlsx";
            //			if (File.Exists(path))
            //			{
            //				File.Delete(path);
            //			}
            //			templatePackage.SaveAs(new FileInfo(path));
            //			requestDate = requestDate.AddMonths(1);

            //			var fileInArchive = archive.CreateEntry(path, System.IO.Compression.CompressionLevel.Optimal);
            //			using (var entryStream = fileInArchive.Open())
            //			{
            //				using (var fileInCompression = new MemoryStream(File.ReadAllBytes(path)))
            //				{
            //					await fileInCompression.CopyToAsync(entryStream);
            //				}
            //			}
            //			// delete when response done
            //			if (File.Exists(path))
            //			{
            //				File.Delete(path);
            //			}

            //		}
            //	}
            //	return new FileContentResult(outStream.ToArray(), "application/zip");
            //}
            return null;
        }
        private string GetOT150Formula(int numberDayOfMonth, int rowTitleDayOfWeek, int currentEmpRow, int startCol = 8)
        {
            var formula = "";
            for (int i = 0; i < numberDayOfMonth; i++)
            {
                var titleDayOfWeek = ExcelPro.GetRefExcelColumnName(startCol + i, rowTitleDayOfWeek);
                var currentRowColumnName = ExcelPro.GetExcelColumnName(startCol + i);
                var currentColumn = $"{currentRowColumnName}{currentEmpRow}";
                if (i <= 0)
                    formula = $"=IF(OR({titleDayOfWeek}=\"WD\",{titleDayOfWeek}=\"XMS\"),VLOOKUP({currentColumn},Formula,3,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,3,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,3,0),0)))";
                else
                {
                    formula += $"+IF(OR({titleDayOfWeek}=\"WD\",{titleDayOfWeek}=\"XMS\"),VLOOKUP({currentColumn},Formula,3,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,3,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,3,0),0)))";
                }
            }

            return formula;
        }
        private string GetNightShift30Formula(int numberDayOfMonth, int rowTitleDayOfWeek, int currentEmpRow, int startCol = 8)
        {
            var formula = "";
            for (int i = 0; i < numberDayOfMonth; i++)
            {
                var titleDayOfWeek = ExcelPro.GetRefExcelColumnName(startCol + i, rowTitleDayOfWeek);
                var currentRowColumnName = ExcelPro.GetExcelColumnName(startCol + i);
                var currentColumn = $"{currentRowColumnName}{currentEmpRow}";
                if (i <= 0)
                    formula = $"=IF(OR({titleDayOfWeek}=\"WD\",{titleDayOfWeek}=\"XMS\"),VLOOKUP({currentColumn},Formula,4,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,4,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,4,0),0)))";
                else
                {
                    formula += $"+IF(OR({titleDayOfWeek}=\"WD\",{titleDayOfWeek}=\"XMS\"),VLOOKUP({currentColumn},Formula,4,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,4,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,4,0),0)))";
                }
            }

            return formula;
        }
        private string GetOT200Formula(int numberDayOfMonth, int rowTitleDayOfWeek, int currentEmpRow, int startCol = 8)
        {
            var formula = "";
            for (int i = 0; i < numberDayOfMonth; i++)
            {
                var titleDayOfWeek = ExcelPro.GetRefExcelColumnName(startCol + i, rowTitleDayOfWeek);
                var currentRowColumnName = ExcelPro.GetExcelColumnName(startCol + i);
                var currentColumn = $"{currentRowColumnName}{currentEmpRow}";
                if (i <= 0)
                    formula = $"IF(OR({titleDayOfWeek}=\"WD\",{titleDayOfWeek}=\"XMS\"),VLOOKUP({currentColumn},Formula,5,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,5,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,5,0),0)))";
                else
                {
                    formula += $"+IF(OR({titleDayOfWeek}=\"WD\",{titleDayOfWeek}=\"XMS\"),VLOOKUP({currentColumn},Formula,5,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,5,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,5,0),0)))";
                }
            }

            return formula;
        }
        private string GetOTNight270Formula(int numberDayOfMonth, int rowTitleDayOfWeek, int currentEmpRow, int startCol = 8)
        {
            var formula = "";
            for (int i = 0; i < numberDayOfMonth; i++)
            {
                var titleDayOfWeek = ExcelPro.GetRefExcelColumnName(startCol + i, rowTitleDayOfWeek);
                var currentRowColumnName = ExcelPro.GetExcelColumnName(startCol + i);
                var currentColumn = $"{currentRowColumnName}{currentEmpRow}";
                if (i <= 0)
                    formula = $"IF({titleDayOfWeek}=\"WD\",VLOOKUP({currentColumn},Formula,6,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,6,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,6,0),0)))";
                else
                {
                    formula += $"+IF({titleDayOfWeek}=\"WD\",VLOOKUP({currentColumn},Formula,6,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,6,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,6,0),0)))";
                }
            }

            return formula;
        }
        private string GetOT300Formula(int numberDayOfMonth, int rowTitleDayOfWeek, int currentEmpRow, int startCol = 8)
        {
            var formula = "";
            for (int i = 0; i < numberDayOfMonth; i++)
            {
                var titleDayOfWeek = ExcelPro.GetRefExcelColumnName(startCol + i, rowTitleDayOfWeek);
                var currentRowColumnName = ExcelPro.GetExcelColumnName(startCol + i);
                var currentColumn = $"{currentRowColumnName}{currentEmpRow}";
                if (i <= 0)
                    formula = $"IF({titleDayOfWeek}=\"WD\",VLOOKUP({currentColumn},Formula,7,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,7,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,7,0),0)))";
                else
                {
                    formula += $"+IF({titleDayOfWeek}=\"WD\",VLOOKUP({currentColumn},Formula,7,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,7,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,7,0),0)))";
                }
            }

            return formula;
        }
        private string GetOT390Formula(int numberDayOfMonth, int rowTitleDayOfWeek, int currentEmpRow, int startCol = 8)
        {
            var formula = "";
            for (int i = 0; i < numberDayOfMonth; i++)
            {
                var titleDayOfWeek = ExcelPro.GetRefExcelColumnName(startCol + i, rowTitleDayOfWeek);
                var currentRowColumnName = ExcelPro.GetExcelColumnName(startCol + i);
                var currentColumn = $"{currentRowColumnName}{currentEmpRow}";
                if (i <= 0)
                    formula = $"IF({titleDayOfWeek}=\"WD\",VLOOKUP({currentColumn},Formula,8,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,8,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,8,0),0)))";
                else
                {
                    formula += $"+IF({titleDayOfWeek}=\"WD\",VLOOKUP({currentColumn},Formula,8,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,8,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,8,0),0)))";
                }
            }

            return formula;
        }
        private string GetMealFormula(int numberDayOfMonth, int rowTitleDayOfWeek, int currentEmpRow, int startCol = 8)
        {
            var formula = "";
            for (int i = 0; i < numberDayOfMonth; i++)
            {
                var titleDayOfWeek = ExcelPro.GetRefExcelColumnName(startCol + i, rowTitleDayOfWeek);
                var currentRowColumnName = ExcelPro.GetExcelColumnName(startCol + i);
                var currentColumn = $"{currentRowColumnName}{currentEmpRow}";
                if (i <= 0)
                    formula = $"IF(OR({titleDayOfWeek}=\"WD\",{titleDayOfWeek}=\"XMS\"),VLOOKUP({currentColumn},Formula,2,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,2,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,2,0),0)))";
                else
                {
                    formula += $"+IF(OR({titleDayOfWeek}=\"WD\",{titleDayOfWeek}=\"XMS\"),VLOOKUP({currentColumn},Formula,2,0),IF({titleDayOfWeek}=\"OFF\",VLOOKUP({currentColumn},Formula1,2,0),IF({titleDayOfWeek}=\"HLD\",VLOOKUP({currentColumn},Formula2,2,0),0)))";
                }
            }
            return formula;
        }
        private string GetPFormula(string dataRange)
        {
            var formula = "";

            formula = $"=COUNTIF({dataRange},\"P\")+(COUNTIF({dataRange},\"P/2\")/2)";
            return formula;
        }
        private string GetTotalOTHour(int columnSum, int row)
        {
            var OT150ColumnName = ExcelPro.GetExcelColumnName(columnSum - 10) + row.ToString();
            //var OT30ColumnName = ExcelPro.GetExcelColumnName(columnSum - 9) + row.ToString();
            var OT200ColumnName = ExcelPro.GetExcelColumnName(columnSum - 8) + row.ToString();
            var OT210ColumnName = ExcelPro.GetExcelColumnName(columnSum - 7) + row.ToString();
            var OT270ColumnName = ExcelPro.GetExcelColumnName(columnSum - 6) + row.ToString();
            var OT300ColumnName = ExcelPro.GetExcelColumnName(columnSum - 5) + row.ToString();
            var OT390ColumnName = ExcelPro.GetExcelColumnName(columnSum - 4) + row.ToString();
            return $"={OT150ColumnName}+{OT200ColumnName}+{OT210ColumnName}+{OT270ColumnName}+{OT300ColumnName}+{OT390ColumnName}";
        }
        private string GetFormulaTotalOTPerMonth(int columnSum, int row)
        {
            var OT150ColumnName = ExcelPro.GetExcelColumnName(columnSum - 7) + row.ToString();
            var OT200ColumnName = ExcelPro.GetExcelColumnName(columnSum - 5) + row.ToString();
            var OT300ColumnName = ExcelPro.GetExcelColumnName(columnSum - 2) + row.ToString();
            return $"={OT150ColumnName}+{OT200ColumnName}+{OT300ColumnName}";
        }
        private string GetFormulaTotalPercentOTPerMonth(int columnSum, int row)
        {
            var totalNormalWorkingColumnName = ExcelPro.GetExcelColumnName(columnSum - 9) + row.ToString();
            var totalOTColumnName = ExcelPro.GetExcelColumnName(columnSum - 1) + row.ToString();
            return $"={totalOTColumnName}/{totalNormalWorkingColumnName}";
        }

        const int MAX_HOUR_IN_SHIFT = 8;
        public int GetTotalNormalWorkingHour(string workingType)
        {
            var otType = WorkingShiftModel.IsOTType(workingType.ToString());
            if (otType == 0)
            {
                return WorkingShiftModel.GetHourNormalWorking(workingType.ToString());
            }
            else
            {
                return MAX_HOUR_IN_SHIFT;
            }
        }
        public async Task<IActionResult> ExportEmployeeDepartment(ExportEmployeeDepartmentRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            //var templatePath = "./ReportTemplate/TemplateEmployeeDepartment.xlsx";
            var templatePath = ConstMain.ConstTemplateFilePath;

            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    var currentDate = new DateTime(request.DateFrom.Year, request.DateFrom.Month, 1, 0, 0, 0);


                    bool isTemplateExist = File.Exists(templatePath);
                    _logger.LogInformation($"Template Path: {templatePath}");
                    _logger.LogInformation($"Template Path Exist: {isTemplateExist}");
                    var templatePackage = ExcelPro.LoadTempFile(templatePath);
                    //var daysOfMonth = DateTime.DaysInMonth(requestDate.Year, requestDate.Month);
                    //var departments = _departmentRepository.GetDepartmentsByListIds(request.ListDepartmentId);
                    //var departments = _departmentRepository.GetDepartmentsByListIds(request.ListDepartmentId);
                    //int count = 0;

                    string nameSheet = "employeeModel";

                    var employess = _employeeRepository.GetList();
                    if (employess == null)
                    {
                        return null;
                    }

                    //templatePackage = ExcelPro.CloneWorkbook(ref templatePackage, "new.xlsx");
                    //var checkNameSheetExist = ExcelPro.WorksheetExists(templatePackage, department.Name);

                    //ExcelPro.AddNewWorksheet(ref templatePackage, nameSheet, "is_template_employee");

                    var startRow = 10;
                    var Stt = 1;
                    foreach (var employee in employess)
                    {
                        string[] pairs = null;
                        // set STT
                        ExcelPro.SetValue(ref templatePackage, nameSheet, $"C{startRow}", Stt.ToString());

                        // set EmployeeeCode
                        ExcelPro.SetValue(ref templatePackage, nameSheet, $"D{startRow}", employee?.EmployeeCode);

                        // set name
                        ExcelPro.SetValue(ref templatePackage, nameSheet, $"E{startRow}", employee?.Name);

                        // set Name Eployee
                        ExcelPro.SetValue(ref templatePackage, nameSheet, $"F{startRow}", employee?.Department?.Name);

                        // set jobtitle
                        ExcelPro.SetValue(ref templatePackage, nameSheet, $"G{startRow}", employee?.JobTitle?.Name);

                        // set birdday
                        if (employee?.Birthday != null)
                        {
                            string a = employee.Birthday.Value.ToString("dd-MMM-yyyy");
                            ExcelPro.SetValue(ref templatePackage, nameSheet, $"I{startRow}", employee.Birthday.Value.ToString("dd-MMM-yyyy"));
                        }

                        // set joindate
                        if (employee?.JoiningDate != null)
                        {
                            ExcelPro.SetValue(ref templatePackage, nameSheet, $"L{startRow}", employee?.JoiningDate.Value.ToString("dd-MMM-yyyy"));

                        }

                        // set phone
                        ExcelPro.SetValue(ref templatePackage, nameSheet, $"J{startRow}", employee?.PhoneNumber);

                        if (employee.Address != null)
                        {
                            pairs = employee.Address.Split(',');
                        }

                        if (pairs != null && pairs.Count() >= 3)
                        {
                            // set ward
                            ExcelPro.SetValue(ref templatePackage, nameSheet, $"P{startRow}", pairs[pairs.Length - 3]);

                            // set city
                            ExcelPro.SetValue(ref templatePackage, nameSheet, $"Q{startRow}", pairs[pairs.Length - 2]);

                            // set province
                            ExcelPro.SetValue(ref templatePackage, nameSheet, $"R{startRow}", pairs[pairs.Length - 1]);
                        }


                        // set adresss
                        ExcelPro.SetValue(ref templatePackage, nameSheet, $"S{startRow}", employee?.Address);

                        startRow++;
                        Stt++;
                    }
                    var path = $"./Report_Employee_Department{request.List_Department}.xlsx";
                    // delete when response done
                    if (File.Exists(path))
                    {
                        File.Delete(path);
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

        public async Task<IActionResult> ExportEmployeeAdminDepartment(ExportEmployeeDepartmentRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var templatePath = ConstMain.ConstTemplateFilePath;

            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    var currentDate = new DateTime(request.DateFrom.Year, request.DateFrom.Month, 1, 0, 0, 0);

                    int count = 0;
                    string nameSheet = "Department Admin";
                    var startRow = 9;
                    var Stt = 1;

                    bool isTemplateExist = File.Exists(templatePath);
                    _logger.LogInformation($"Template Path: {templatePath}");
                    _logger.LogInformation($"Template Path Exist: {isTemplateExist}");
                    var templatePackage = ExcelPro.LoadTempFile(templatePath);
                    var departmentAdmins = _departmentAdminRepository.GetList();
                    var departmentGroup = departmentAdmins.GroupBy(p => p.User?.ItemEmployee?.EmployeeCode).ToList();

                    foreach (var adminEntity in departmentGroup)
                    {
                        foreach (var adminEntity2 in adminEntity)
                        {
                            // set STT
                            ExcelPro.SetValue(ref templatePackage, nameSheet, $"B{startRow}", Stt.ToString());
                            //// set department name 
                            ExcelPro.SetValue(ref templatePackage, nameSheet, $"C{startRow}", adminEntity2?.Department?.Name);
                            if (adminEntity2?.User?.ItemEmployee == null)
                            {
                                continue;
                            }
                            switch (adminEntity2.Role)
                            {
                                case EnumDepartmentAdmin.Admin1:
                                    ExcelPro.SetValue(ref templatePackage, nameSheet, $"H{startRow}", adminEntity2?.User?.ItemEmployee?.EmployeeCode);
                                    break;
                                case EnumDepartmentAdmin.Admin2:
                                    ExcelPro.SetValue(ref templatePackage, nameSheet, $"I{startRow}", adminEntity2?.User?.ItemEmployee?.EmployeeCode);
                                    break;
                                case EnumDepartmentAdmin.Admin3:
                                    ExcelPro.SetValue(ref templatePackage, nameSheet, $"J{startRow}", adminEntity2?.User?.ItemEmployee?.EmployeeCode);
                                    break;
                                default:
                                    break;

                            }
                        }
                        startRow++;
                        Stt++;
                    }
                    var path = $"./Report_Admin_Department_{request.List_Department}.xlsx";
                    // delete when response done
                    if (File.Exists(path))
                    {
                        File.Delete(path);
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
        public EmployeeDepartmentPagingResponseModel GetListEmployeeDepartment(
           PagingParamRequestModel pagingReq = null)
        {
            List<DepartmentListEmployeeResponseModel> listResponseModel = null;

            Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
            SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
            Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
            var ret = _employeeRepository.GetListEmployeeDepartment(pagingReq, filterParams, searchParams, sortParams);
            var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
            var disPlayProps = JsonPropertyHelper<DepartmentListEmployeeResponseModel>.GetFilterProperties();
            var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
            columns = DepartmentListEmployeeResponseModel.AddKeySearchFilterable(columns);
            ret.Columns = columns;
            return ret;

        }


        public async Task<Messages.Message> ImportFileEmployeeDepartment(DepartmentRequestModel request, long? currentUserId)
        {
            var dicFormFile = request.GetFiles();
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

            var listEmployeeNew = new List<EmployeeEntity>();
            var listEmployeeModel = new List<EmployeeImportModel>();
            string reportTemplateSheet = Environment.GetEnvironmentVariable("EmployeeSheetName");

            List<DepartmentEntity> listDepartment = null;
            List<JobTitleEntity> listTitle = null;

            using (var package = ExcelPro.LoadTempFile(filePath))
            {
                ExcelWorkbook workBook = package.Workbook;
                if (workBook == null)
                {
                    throw new BaseException($"Workbook is null: {filePath}");
                }
                ;
                var ws = workBook.Worksheets[reportTemplateSheet];
                if (ws == null)
                {
                    throw new BaseException($"Sheet not exists: {reportTemplateSheet}");
                }

                listEmployeeModel = ReadListEmployeeFromExcel(ws, ref listDepartment, ref listTitle);
            }

            if (listEmployeeModel == null || listEmployeeModel.Count <= 0)
            {
                throw new BaseException($"Data is empty: {filePath}");
            }

            using (var transaction = this._dbContext.Database.BeginTransaction())
            {
                try
                {
                    //this._departmentRepository.DeleteAll(currentUserId, true);
                    //this._jobTitleRepository.DeleteAll(currentUserId, true);
                    listDepartment = await this._departmentRepository.InsertIfNotExist(listDepartment, currentUserId);
                    listTitle = await this._jobTitleRepository.InsertIfNotExist(listTitle, currentUserId);

                    var dicDepartment = listDepartment.ToDictionary(x => x.Name, x => x);
                    var dicTitle = listTitle.ToDictionary(x => x.Name.Trim(), x => x);

                    var listCode = listEmployeeModel.Where(x => x != null).Select(x => x.EmployeeCode).ToList();
                    listCode = ConvertUtil.RemoveDuplicate(listCode);
                    var listEmps = _employeeRepository.GetByListEmployeeCode(listCode);
                    var dicEmployeeOld = new Dictionary<string, EmployeeEntity>();
                    foreach (var emp in listEmps)
                    {
                        if (!dicEmployeeOld.ContainsKey(emp.EmployeeCode))
                        {
                            dicEmployeeOld.Add(emp.EmployeeCode, emp);
                        }
                        else
                        {
                            dicEmployeeOld[emp.EmployeeCode] = emp;
                        }
                    }

                    foreach (var employeeModel in listEmployeeModel)
                    {
                        if (employeeModel.EmployeeCode == null)
                        {
                            continue;
                        }
                        var employeeEntity = employeeModel.GetEntity();
                        employeeEntity.DepartmentId = employeeEntity.DepartmentId == null ? dicDepartment[employeeModel.DepartmentStr].Id : employeeEntity.DepartmentId;
                        employeeEntity.JobTitleId = employeeEntity.JobTitleId == null ? dicTitle[employeeModel.JobTitleStr].Id : employeeEntity.JobTitleId;
                        if (employeeModel.EmployeeCode != null)
                        {
                            if (dicEmployeeOld.ContainsKey(employeeModel.EmployeeCode))
                            {
                                employeeEntity.Id = dicEmployeeOld[employeeModel.EmployeeCode].Id;

                                var existed_machinecode = _employeeRepository.GetByEmpMachineCode(employeeModel.EmployeeMachineCode);

                                if (existed_machinecode == null)
                                {
                                    employeeEntity.EmployeeMachineCode = string.IsNullOrEmpty(employeeModel.EmployeeMachineCode)
                                        ? dicEmployeeOld[employeeModel.EmployeeCode].EmployeeMachineCode : employeeModel.EmployeeMachineCode;
                                }
                                else
                                {
                                    employeeEntity.EmployeeMachineCode = dicEmployeeOld[employeeModel.EmployeeCode].EmployeeMachineCode;
                                    existed_machinecode.EmployeeMachineCode = "";
                                    this._employeeRepository.Upsert(existed_machinecode, currentUserId);
                                }

                                employeeEntity.EmployeeCode = dicEmployeeOld[employeeModel.EmployeeCode].EmployeeCode;
                            }
                            else
                            {
                                var existed_machinecode = _employeeRepository.GetByEmpMachineCode(employeeModel.EmployeeMachineCode);

                                if (existed_machinecode == null)
                                {
                                    employeeEntity.EmployeeMachineCode = employeeModel.EmployeeMachineCode;
                                    employeeEntity.EmployeeCode = employeeModel.EmployeeCode;
                                }
                                else
                                {
                                    employeeEntity.Id = existed_machinecode.Id;
                                    employeeEntity.EmployeeMachineCode = existed_machinecode.EmployeeMachineCode;
                                    employeeEntity.EmployeeCode = employeeModel.EmployeeCode;
                                    existed_machinecode.EmployeeMachineCode = "";
                                    this._employeeRepository.Upsert(existed_machinecode, currentUserId);
                                }
                            }
                        }

                        if (employeeEntity.EmployeeMachineCode == null)
                        {
                            employeeEntity.EmployeeMachineCode = "";
                        }

                        employeeEntity.Address = employeeModel.Address != null ? employeeModel.Address : employeeEntity.Address;
                        employeeEntity.Birthday = employeeModel.BirthDay != null ? employeeModel.BirthDay : employeeEntity.Birthday;
                        employeeEntity.JoiningDate = employeeModel.JoiningDate != null ? employeeModel.JoiningDate : employeeEntity.JoiningDate;
                        employeeEntity.Name = employeeModel.Name;
                        employeeEntity.PhoneNumber = employeeModel.PhoneNumber != null ? employeeModel.PhoneNumber : employeeEntity.PhoneNumber;
                        employeeEntity.EmployeeStatus = employeeModel.EmployeeStatus;
                        employeeEntity.DeletedFlag = employeeEntity.EmployeeStatus != EnumEmployeeStatus.Actived;
                        employeeEntity.Contract = employeeModel.Contract != null ? employeeModel.Contract : employeeEntity.Contract;
                        employeeEntity.Avatar = employeeModel.Avatar != null ? employeeModel.Avatar : employeeEntity.Avatar;
                        listEmployeeNew.Add(employeeEntity);
                    }

                    //this._employeeRepository.DeleteAll(currentUserId, true);
                    this._employeeRepository.UpSertMulti(listEmployeeNew, currentUserId);

                    await SetCurrentEmployeeToExcel(listEmployeeModel);

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

        private List<EmployeeImportModel> ReadListEmployeeFromExcel(ExcelWorksheet ws, ref List<DepartmentEntity> listDepartment, ref List<JobTitleEntity> listTitle)
        {
            var listEmployeeModel = new List<EmployeeImportModel>();

            string checkCell = "E8";
            int startRowData = 8;
            int startColumnData = 3;
            string beginCell = ConvertUtil.GetCellNext(checkCell, Direct.Left, 2);
            int countItem = CommonFuncMainService.GetCountRow(ws, checkCell);
            string endCellLeft = ConvertUtil.GetCellNext(checkCell, Direct.Down, countItem - 1);
            string endCellRight = ConvertUtil.GetCellNext(endCellLeft, Direct.Right, 15);
            string rangeAddress = $"{beginCell}:{endCellRight}";
            ExcelRange targetRange = ws.Cells[rangeAddress];
            //var listHolidayCode = Enum.GetNames(typeof(EnumHolidayCode));

            int columnIndex = 0;
            for (int row = 0; row < countItem; row++)
            {
                columnIndex = 0;
                var cellVal_No = ConvertUtil.ConvertToNullableLong(targetRange.GetCellValue<object>(row, columnIndex++));
                var cellVal_EmployeeCode = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_EmployeeMachineCode = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Name = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Department = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_JobTitle = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Contract = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_BirthDay = ConvertUtil.ConvertToNullableDouble(targetRange.GetCellValue<object>(row, columnIndex++));
                var cellVal_PhoneNumber = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_StatusStr = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_StartDay = ConvertUtil.ConvertToNullableDouble(targetRange.GetCellValue<object>(row, columnIndex++));
                var cellVal_LeavingDay = ConvertUtil.ConvertToNullableDouble(targetRange.GetCellValue<object>(row, columnIndex++));
                var cellVal_KindOfTermination = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Reason = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Ward = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_City = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Province = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Address = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Note = targetRange.GetCellValue<object>(row, columnIndex++)?.ToString();
                var cellVal_Avatar = SaveImageFromCell(ws, row, columnIndex++, startRowData, startColumnData, ConstDatabase.ConstImageSaveAvatarPath, cellVal_Name.ToString(), cellVal_EmployeeCode.ToString());
                Console.WriteLine(row);
                if (cellVal_Department.IsNullOrEmpty())
                {
                    cellVal_Department = "Unknown";
                }
                if (cellVal_JobTitle.IsNullOrEmpty())
                {
                    cellVal_JobTitle = "Unknown";
                }
                if (cellVal_EmployeeMachineCode.IsNullOrEmpty())
                {
                    cellVal_EmployeeMachineCode = cellVal_EmployeeCode;
                }
                if (cellVal_Name.IsNullOrEmpty()
                  //|| cellVal_Department.IsNullOrEmpty()
                  || cellVal_EmployeeMachineCode.IsNullOrEmpty()
                  || (cellVal_EmployeeCode.IsNullOrEmpty() && cellVal_StatusStr == EnumEmployeeStatusStr.Active))
                {
                    continue;
                }

                var employeeStatus = cellVal_StatusStr == EnumEmployeeStatusStr.Active ? EnumEmployeeStatus.Actived : EnumEmployeeStatus.InActive;
                if (listEmployeeModel.Count > 0 && listEmployeeModel.Any(x => x.EmployeeCode == cellVal_EmployeeCode))
                {
                    throw new BaseException($"Duplicate EmployeeCode {cellVal_EmployeeCode}, row: {ConvertUtil.GetCellNext(beginCell, Direct.Down, row)}");
                }
                if (listEmployeeModel.Count > 0 && listEmployeeModel.Any(x => x.EmployeeMachineCode == cellVal_EmployeeMachineCode))
                {
                    throw new BaseException($"Duplicate EmployeeMachineCode {cellVal_EmployeeMachineCode}, row: {ConvertUtil.GetCellNext(beginCell, Direct.Down, row)}");
                }

                listEmployeeModel.Add(new EmployeeImportModel()
                {
                    EmployeeCode = cellVal_EmployeeCode?.Trim(),
                    EmployeeMachineCode = cellVal_EmployeeMachineCode?.Trim(),
                    Name = cellVal_Name?.Trim(),
                    DepartmentStr = cellVal_Department?.Trim(),
                    JobTitleStr = cellVal_JobTitle?.Trim(),
                    Contract = !string.IsNullOrEmpty(cellVal_Contract)
                    ? (EnumTypeContract)Enum.Parse(typeof(EnumTypeContract), cellVal_Contract?.ToString().ToUpper())
                    : null,
                    //Contract = EnumTypeContract.FLE,
                    BirthDay = cellVal_BirthDay == null ? null : DateTime.FromOADate(cellVal_BirthDay.Value),
                    PhoneNumber = cellVal_PhoneNumber?.Trim(),
                    EmployeeStatus = employeeStatus,
                    JoiningDate = cellVal_StartDay == null ? null : DateTime.FromOADate(cellVal_StartDay.Value),
                    LeavingDay = cellVal_LeavingDay == null ? null : DateTime.FromOADate(cellVal_LeavingDay.Value),
                    KindOfTermination = cellVal_KindOfTermination,
                    Reason = cellVal_Reason,
                    Ward = cellVal_Ward,
                    City = cellVal_City,
                    Province = cellVal_Province?.Trim(),
                    Address = cellVal_Address?.Trim(),
                    Note = cellVal_Note?.Trim(),
                    Avatar = !string.IsNullOrEmpty(cellVal_Avatar) ? cellVal_Avatar.Trim() : null,
                }); ;
            }

            List<string> listDeptmentStr = listEmployeeModel.Select(x => x.DepartmentStr).ToList();
            listDeptmentStr = ConvertUtil.RemoveDuplicate(listDeptmentStr);
            listDepartment = listDeptmentStr.Select(x => new DepartmentEntity()
            {
                Name = x
            }).ToList();

            List<string> listTitleStr = listEmployeeModel.Select(x => x.JobTitleStr).ToList();
            listTitleStr = ConvertUtil.RemoveDuplicate(listTitleStr);
            listTitle = listTitleStr.Select(x => new JobTitleEntity()
            {
                Name = x
            }).ToList();

            return listEmployeeModel;
        }

        private string? SaveImageFromCell(
                                        ExcelWorksheet worksheet,
                                        int row,
                                        int column,
                                        int rowStartData,
                                        int columnStartData,
                                        string saveDirectory,
                                        string nameEmployee,
                                        string employeeCode)
        {
            foreach (var drawing in worksheet.Drawings)
            {
                if (drawing is ExcelPicture picture)
                {
                    var startRow = picture.From.Row + 1 - rowStartData; // Data start from row numberRowStart
                    var startColumn = picture.From.Column + 1 - columnStartData; // Data start from row columnStartData

                    if (startRow == row && startColumn == column)
                    {
                        if (!string.IsNullOrEmpty(nameEmployee))
                        {
                            nameEmployee = StringUtil.RemoveSpecialChar(nameEmployee).Replace(" ", "");
                        }
                        string fileName = $"{nameEmployee}_{employeeCode}_{Guid.NewGuid()}.png".ToString().Replace("-", "_");
                        string filePath = Path.Combine(saveDirectory, fileName);

                        if (!Directory.Exists(saveDirectory))
                        {
                            Directory.CreateDirectory(saveDirectory);
                        }

                        using (var imageStream = new MemoryStream(picture.Image.ImageBytes))
                        {
                            var image = SixLabors.ImageSharp.Image.Load(imageStream);
                            image.Save(filePath);
                        }

                        return filePath;
                    }
                }
            }
            return null;
        }


        public async Task SetCurrentEmployeeToExcel(List<EmployeeImportModel> listEmployeeNew)
        {
            bool changedFlag = false;
            string fileBKPath = string.Format(ConstMain.ConstTemplateBKFilePath, DateTimeUtil.GetDateTimeStr(DateTime.Now, "yyyyMMdd_HHmmss_fff"));
            // Lấy đường dẫn thư mục từ fileBKPath
            string directoryPath = Path.GetDirectoryName(fileBKPath);

            // Kiểm tra và tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            try
            {
                var oldFile = ExcelPro.LoadTempFile(ConstMain.ConstTemplateFilePath);
                ExcelPro.CloneWorkbook(ref oldFile, fileBKPath);
                //FileUtil.CopyFile(ConstMain.ConstTemplateFilePath, fileBKPath);

                var templatePath = ConstMain.ConstTemplateFilePath;
                string reportTemplateSheet = Environment.GetEnvironmentVariable("EmployeeSheetName");

                ExcelPro._templatePath = templatePath;

                using (var report = ExcelPro.LoadReportFormat())
                {
                    var sheet1 = report.Workbook.Worksheets[reportTemplateSheet];
                    if (sheet1 == null)
                    {
                        throw new Exception($"Invalid template: {reportTemplateSheet}");
                    }

                    List<DepartmentEntity> listDepartment = null;
                    List<JobTitleEntity> listTitle = null;
                    var listEmployeeOld = ReadListEmployeeFromExcel(sheet1, ref listDepartment, ref listTitle);

                    var listEmployeeMerged = MergeListEmployeeModel(listEmployeeOld, listEmployeeNew);
                    listEmployeeMerged = listEmployeeMerged.Where(x => x.EmployeeStatus == EnumEmployeeStatus.Actived).ToList();

                    var countOldRow = CommonFuncMainService.GetCountRow(sheet1, "E8");

                    // Set default value
                    if (countOldRow >= 1)
                    {
                        var endCell = ConvertUtil.GetCellNext(ConvertUtil.GetCellNext("C8", Direct.Down, countOldRow - 1), Direct.Right, 17);
                        ExcelRange targetRange0 = sheet1.Cells[$"C8:{endCell}"];
                        object[,] defaultValues = new object[countOldRow, 17 + 1];
                        for (int i = 0; i < countOldRow; i++)
                        {
                            for (int j = 0; j < 17 + 1; j++)
                            {
                                defaultValues[i, j] = "";
                            }
                        }
                        targetRange0.Value = defaultValues;
                    }

                    int beginRowIndex = 8;

                    ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel();
                    exportSheetDataModel.SheetIndex = sheet1.Index;
                    //
                    // special cell
                    exportSheetDataModel.DicCellName2Value.Add("F5", DateTime.Now.ToString("MM/dd/yyyy"));
                    //
                    int columnIndex = 2;
                    exportSheetDataModel.BeginNoNumber = 1;
                    exportSheetDataModel.BeginRowIndex = beginRowIndex;
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, "[No.]");
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.EmployeeCode));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.EmployeeMachineCode));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Name));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.DepartmentStr));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.JobTitleStr));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Contract));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.BirthDay));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.PhoneNumber));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.EmployeeStatus));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.JoiningDate));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.LeavingDay));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.KindOfTermination));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Reason));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Ward));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.City));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Province));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Address));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Note));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Avatar));
                    //*******************     Get data **************************************************************** //
                    var listData = new List<Dictionary<object, object>>();
                    foreach (var employeeModel in listEmployeeMerged)
                    {
                        var dicRow = new Dictionary<object, object>();
                        dicRow.Add(nameof(EmployeeImportModel.EmployeeCode), employeeModel.EmployeeCode);
                        dicRow.Add(nameof(EmployeeImportModel.Name), employeeModel.Name);
                        dicRow.Add(nameof(EmployeeImportModel.DepartmentStr), employeeModel.DepartmentStr);
                        dicRow.Add(nameof(EmployeeImportModel.JobTitleStr), employeeModel.JobTitleStr);
                        dicRow.Add(nameof(EmployeeImportModel.Contract), employeeModel.Contract);
                        dicRow.Add(nameof(EmployeeImportModel.BirthDay), employeeModel.BirthDay);
                        dicRow.Add(nameof(EmployeeImportModel.PhoneNumber), employeeModel.PhoneNumber);
                        dicRow.Add(nameof(EmployeeImportModel.EmployeeStatus), EnumEmployeeStatusStr.GetFromCode(employeeModel.EmployeeStatus));
                        dicRow.Add(nameof(EmployeeImportModel.JoiningDate), employeeModel.JoiningDate);
                        dicRow.Add(nameof(EmployeeImportModel.LeavingDay), employeeModel.LeavingDay);
                        dicRow.Add(nameof(EmployeeImportModel.KindOfTermination), employeeModel.KindOfTermination);
                        dicRow.Add(nameof(EmployeeImportModel.Reason), employeeModel.Reason);
                        dicRow.Add(nameof(EmployeeImportModel.Ward), employeeModel.Ward);
                        dicRow.Add(nameof(EmployeeImportModel.City), employeeModel.City);
                        dicRow.Add(nameof(EmployeeImportModel.Province), employeeModel.Province);
                        dicRow.Add(nameof(EmployeeImportModel.Address), employeeModel.Address);
                        dicRow.Add(nameof(EmployeeImportModel.Note), employeeModel.Note);
                        dicRow.Add(nameof(EmployeeImportModel.Avatar), employeeModel.Avatar);
                        listData.Add(dicRow);
                    }

                    int insertEmptyRow = listData.Count - countOldRow;
                    if (insertEmptyRow > 0)
                    {
                        sheet1.InsertRow(beginRowIndex + 1, insertEmptyRow, beginRowIndex);
                    }
                    else if (insertEmptyRow < 0)
                    {
                        sheet1.DeleteRow(beginRowIndex + 1, insertEmptyRow * -1);
                        sheet1.DeleteRow(8, beginRowIndex);
                    }

                    exportSheetDataModel.ListChartDataModel = listData;
                    long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                            report.Workbook.Worksheets,
                            exportSheetDataModel);

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
        public async Task<string> SetCurrentEmployeeToExcel()
        {
            bool changedFlag = false;
            string fileBKPath = string.Format("Employees_{0}.xlsx", DateTimeUtil.GetDateTimeStr(DateTime.Now, "yyyyMMdd_HHmmss_fff"));
            try
            {

                //FileUtil.CopyFile(ConstMain.ConstTemplateFilePath, fileBKPath);

                var templatePath = ConstMain.ConstTemplateFilePath;
                string reportTemplateSheet = Environment.GetEnvironmentVariable("EmployeeSheetName");
                string AdminSheetName = Environment.GetEnvironmentVariable("AdminSheetName");
                ExcelPro._templatePath = templatePath;

                using (var report = ExcelPro.LoadReportFormat())
                {
                    var sheet1 = report.Workbook.Worksheets[reportTemplateSheet];
                    if (sheet1 == null)
                    {
                        throw new Exception($"Invalid template: {reportTemplateSheet}");
                    }

                    List<DepartmentEntity> listDepartment = null;
                    List<JobTitleEntity> listTitle = null;
                    var listEmployeeMerged = _employeeRepository.GetWithdRelations().Select(x => new EmployeeImportModel().SetModel(x)).OrderByDescending(x => x.DepartmentStr);

                    var countOldRow = CommonFuncMainService.GetCountRow(sheet1, "E8");
                    // Set default value
                    if (countOldRow >= 1)
                    {
                        var endCell = ConvertUtil.GetCellNext(ConvertUtil.GetCellNext("C8", Direct.Down, countOldRow - 1), Direct.Right, 17);
                        ExcelRange targetRange0 = sheet1.Cells[$"C8:{endCell}"];
                        object[,] defaultValues = new object[countOldRow, 17 + 1];
                        for (int i = 0; i < countOldRow; i++)
                        {
                            for (int j = 0; j < 17 + 1; j++)
                            {
                                defaultValues[i, j] = "";
                            }
                        }
                        targetRange0.Value = defaultValues;
                    }

                    int beginRowIndex = 8;

                    ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel();
                    exportSheetDataModel.SheetIndex = sheet1.Index;
                    // special cell
                    exportSheetDataModel.DicCellName2Value.Add("F5", DateTime.Now.ToString("MM/dd/yyyy"));
                    //
                    int columnIndex = 2;
                    exportSheetDataModel.BeginNoNumber = 1;
                    exportSheetDataModel.BeginRowIndex = beginRowIndex;
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, "[No.]");
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.EmployeeCode));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.EmployeeMachineCode));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Name));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.DepartmentStr));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.JobTitleStr));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Contract));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.BirthDay));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.PhoneNumber));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.EmployeeStatus));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.JoiningDate));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.LeavingDay));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.KindOfTermination));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Reason));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Ward));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.City));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Province));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Address));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Note));
                    exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeImportModel.Avatar));
                    //*******************     Get data **************************************************************** //
                    var listData = new List<Dictionary<object, object>>();
                    foreach (var employeeModel in listEmployeeMerged)
                    {
                        if (listData.FirstOrDefault(x => x.Values.Contains(nameof(EmployeeImportModel.EmployeeCode))) != null) continue;
                        var dicRow = new Dictionary<object, object>();
                        dicRow.Add(nameof(EmployeeImportModel.EmployeeCode), employeeModel.EmployeeCode);
                        dicRow.Add(nameof(EmployeeImportModel.EmployeeMachineCode), employeeModel.EmployeeMachineCode);
                        dicRow.Add(nameof(EmployeeImportModel.Name), employeeModel.Name);
                        dicRow.Add(nameof(EmployeeImportModel.DepartmentStr), employeeModel.DepartmentStr);
                        dicRow.Add(nameof(EmployeeImportModel.JobTitleStr), employeeModel.JobTitleStr);
                        dicRow.Add(nameof(EmployeeImportModel.Contract), employeeModel.Contract);
                        dicRow.Add(nameof(EmployeeImportModel.BirthDay), employeeModel.BirthDay);
                        dicRow.Add(nameof(EmployeeImportModel.PhoneNumber), employeeModel.PhoneNumber);
                        dicRow.Add(nameof(EmployeeImportModel.EmployeeStatus), EnumEmployeeStatusStr.GetFromCode(employeeModel.EmployeeStatus));
                        dicRow.Add(nameof(EmployeeImportModel.JoiningDate), employeeModel.JoiningDate);
                        dicRow.Add(nameof(EmployeeImportModel.LeavingDay), employeeModel.LeavingDay);
                        dicRow.Add(nameof(EmployeeImportModel.KindOfTermination), employeeModel.KindOfTermination);
                        dicRow.Add(nameof(EmployeeImportModel.Reason), employeeModel.Reason);
                        dicRow.Add(nameof(EmployeeImportModel.Ward), employeeModel.Ward);
                        dicRow.Add(nameof(EmployeeImportModel.City), employeeModel.City);
                        dicRow.Add(nameof(EmployeeImportModel.Province), employeeModel.Province);
                        dicRow.Add(nameof(EmployeeImportModel.Address), employeeModel.Address);
                        dicRow.Add(nameof(EmployeeImportModel.Note), employeeModel.Note);
                        dicRow.Add(nameof(EmployeeImportModel.Avatar), employeeModel.Avatar);
                        listData.Add(dicRow);
                    }

                    int insertEmptyRow = listData.Count - countOldRow;
                    sheet1.Cells["C8:V10000"].Clear();
                    if (insertEmptyRow > 0)
                    {
                        sheet1.InsertRow(beginRowIndex + 1, listData.Count, beginRowIndex);
                    }
                    exportSheetDataModel.ListChartDataModel = listData;
                    long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                            report.Workbook.Worksheets,
                            exportSheetDataModel);


                    report.Save();
                    changedFlag = true;
                    report.Dispose();
                    var oldFile = ExcelPro.LoadTempFile(ConstMain.ConstTemplateFilePath);
                    ExcelPro.CloneWorkbook(ref oldFile, fileBKPath);
                    //delete sheet Admin
                    oldFile.Workbook.Worksheets.Delete(AdminSheetName);
                    oldFile.Save();

                }
                return fileBKPath;
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

        private List<EmployeeImportModel> MergeListEmployeeModel(
          List<EmployeeImportModel> listEmployeeOld,
          List<EmployeeImportModel> listEmployeeNew)
        {
            List<EmployeeImportModel> listRS = new List<EmployeeImportModel>();
            var dicEmployeeCode2Employee = new Dictionary<string, EmployeeImportModel>();

            foreach (EmployeeImportModel employee in listEmployeeNew)
            {
                if (employee.EmployeeCode != null)
                {
                    if (string.IsNullOrEmpty(employee.DepartmentStr))
                    {
                        employee.DepartmentStr = "Unknown";
                    }
                    if (string.IsNullOrEmpty(employee.JobTitleStr))
                    {
                        employee.JobTitleStr = "Unknown";
                    }
                    if (string.IsNullOrEmpty(employee.EmployeeMachineCode))
                    {
                        employee.EmployeeMachineCode = employee.EmployeeCode;
                    }
                    if (!dicEmployeeCode2Employee.ContainsKey(employee.EmployeeCode))
                    {
                        dicEmployeeCode2Employee.Add(employee.EmployeeCode, employee);
                    }
                    else
                    {
                        dicEmployeeCode2Employee[employee.EmployeeCode] = employee;
                    }
                }
            }

            foreach (EmployeeImportModel employee in listEmployeeOld)
            {
                if (employee.EmployeeCode != null)
                {
                    if (string.IsNullOrEmpty(employee.DepartmentStr))
                    {
                        employee.DepartmentStr = "Unknown";
                    }
                    if (string.IsNullOrEmpty(employee.JobTitleStr))
                    {
                        employee.JobTitleStr = "Unknown";
                    }
                    if (string.IsNullOrEmpty(employee.EmployeeMachineCode))
                    {
                        employee.EmployeeMachineCode = employee.EmployeeCode;
                    }
                    if (!dicEmployeeCode2Employee.ContainsKey(employee.EmployeeCode))
                    {
                        listRS.Add(employee);
                    }
                }
            }
            listRS.AddRange(listEmployeeNew);
            return listRS;
        }

        private bool isOTType(WorkingTypeEntity wt)
        {
            return wt != null && (wt.OT_150 > 0 || wt.OT_200 > 0 || wt.Holiday_OT_300 > 0 || wt.Holiday_OT_Night_390 > 0 || wt.Weekend_Night_OT_270 > 0 || wt.Weekend_OT_200 > 0);
        }

        private List<Dictionary<string, object>> GetSummarizeAttendanceFormData(List<WorkingDayEntity> workingDayEntities)
        {
            if (workingDayEntities == null) throw new ArgumentNullException("Invalid argument");
            var ret = new List<Dictionary<string, object>>();
            var workingTypes = _workingTypeRepository.GetAll();
            var groupCodes = workingTypes.GroupBy(x => x.Code).ToList();
            var totalOt = 0;
            for (int i = 0; i < groupCodes.Count; i++)
            {
                var groupCode = groupCodes[i].FirstOrDefault().Code;
                ret.Add(new Dictionary<string, object> {
  {"display_name",groupCode.ToUpper()},
  {"value", workingDayEntities.Count(x => (x as WorkingDayEntity)?.WorkingType?.Name == groupCode)},
  {"type", EnumFormDataType.label.ToString()},
  });
            }
            // for normal working day
            ret.Add(new Dictionary<string, object> {
  {"display_name","Working day"},
  {"value", workingDayEntities.Count(x =>x.WorkingType==null || x.WorkingType.Name==null)},
  {"type", EnumFormDataType.label.ToString()},
  });

            return ret;
        }
        public List<Dictionary<string, object>> GetEmployeeDetailData(EmployeeEntity employee)
        {
            if (employee == null) throw new ArgumentNullException("Invalid argument");
            var ret = new List<Dictionary<string, object>>();

            // Name
            ret.Add(new Dictionary<string, object> {
  {"display_name","Name"},
  {"key", "employee_name"},
  {"value", employee.Name},
  {"type", EnumFormDataType.label.ToString()},
  });
            // Mã nhân viên
            ret.Add(new Dictionary<string, object> {
  {"display_name","Mã nhân viên"},
  {"key", "employee_code"},
  {"value", employee.EmployeeCode},
  {"type", EnumFormDataType.label.ToString()},
  });
            // JobTitle
            ret.Add(new Dictionary<string, object> {
  {"display_name","JobTitle"},
  {"key", "employee_title"},
  {"value", employee.JobTitle?.Name},
  {"type", EnumFormDataType.label.ToString()},
  });
            // Department
            ret.Add(new Dictionary<string, object> {
  {"display_name","Department"},
  {"key", "employee_department"},
  {"value", employee.Department?.Name},
  {"type", EnumFormDataType.label.ToString()},
  });
            // Phone
            ret.Add(new Dictionary<string, object> {
  {"display_name","Phone"},
  {"key", "employee_phone"},
  {"value", employee.PhoneNumber},
  {"type", EnumFormDataType.label.ToString()},
  });
            // image
            ret.Add(new Dictionary<string, object> {
  {"display_name","ImageUrl"},
                {"key", "employee_image"},
                {"value", employee.Avatar != null ? employee.Avatar.Replace(ConstDatabase.ConstImageSaveAvatarPath, ConstDatabase.ConstImageGetAvatarPath)
                    : string.Empty},
                {"type", EnumFormDataType.image.ToString()},
            });

            return ret;
        }



        public override List<Dictionary<string, object>> GetFormDataObjElement(EmployeeEntity entity)
        {
            string entityName = nameof(EmployeeEntity);
            var listRS = new List<Dictionary<string, object>>();
            List<object> objectList = null;
            var properties = typeof(EmployeeEntity).GetProperties();
            bool addedFlag = false;
            foreach (var property in properties)
            {
                addedFlag = false;
                // foreignKeyAttribute
                var foreignKeyAttribute = (ForeignKeyAttribute)Attribute.GetCustomAttribute(property, typeof(ForeignKeyAttribute));
                if (foreignKeyAttribute != null && !addedFlag)
                {
                    string parentEntity = foreignKeyAttribute.Name;
                    var getformURL = "";
                    var upsertURL = "";
                    var isAddedOnCRUDForm = false;
                    //if (property.Name.Contains("Department"))
                    //{
                    //	getformURL = "Department/get-form-data";
                    //	upsertURL = "Department/upsert";
                    //}
                    /*else*/
                    if (property.Name.Contains("JobTitle"))
                    {
                        getformURL = "JobTitle/get-form-data";
                        upsertURL = "JobTitle/upsert";
                        isAddedOnCRUDForm = true;
                    }
                    listRS.Add(new Dictionary<string, object> {
  {"display_name", property.Name.Replace("Id","").Replace("Entity","")},
  {"key", property.Name},
  {"value", property.GetValue(entity)},
  {"type", EnumFormDataType.select.ToStringValue()},
  {"select_data", GetListOptionData(parentEntity, entityName, "")},
  {"searchable", true},
  {"createEnable", isAddedOnCRUDForm},
  {"upsertURL", upsertURL},
  {"getformURL", getformURL},
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
                if (formDataTypeAttr == null) continue;
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
                    switch (formDataTypeAttr.TypeName)
                    {
                        case EnumFormDataType.integerNumber:
                            break;
                        case EnumFormDataType.floatNumber:
                            switch (property.Name)
                            {
                                default:
                                    formDataTypeAttr.Min = "0";
                                    formDataTypeAttr.Max = "86400";
                                    formDataTypeAttr.DefaultVal = "0";
                                    formDataTypeAttr.Unit = "s";
                                    break;
                            }
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
                            break;
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
                            if (property.Name == "Avatar")
                            {
                                var avatarPath = entity.Avatar?.ToString()?.Replace(
                                    ConstDatabase.ConstImageSaveAvatarPath,
                                    ConstDatabase.ConstImageGetAvatarPath) ?? string.Empty;

                                listRS.Add(new Dictionary<string, object> {
                                    {"display_name", property.Name},
                                    {"key", property.Name},
                                    {"value", avatarPath},
                                    {"type", formDataTypeAttr.TypeName.ToStringValue()},
                                    {"width", formDataTypeAttr.Width},
                                    {"height", formDataTypeAttr.Height},
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
                                {"width", formDataTypeAttr.Width},
                                {"height", formDataTypeAttr.Height},
                                {"isRequire",formDataTypeAttr.IsRequired }
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
            var disName = JsonPropertyHelper<EmployeeEntity>.GetFormDisplayName(name);
            return disName;
        }
        public EmployeeEntity? GetByIdAndRelation(long id, bool isDirect = false)
        {
            return _employeeRepository.GetByIdAndRelation(id, isDirect);
        }
        /// <summary>
        /// Khi số giờ OT của nhân viên vượt quá 40H, note qua 1 sheet khác để admin có thể chèn giờ OT vào ngày nào đó tháng sau
        /// Recommend by: Mr.Huấn (Syngenta) & Mr.Trung Dũng (i-Soft)
        /// </summary>
        private async Task NoteOver40HOTHours(ExcelPackage report, string sheet, List<EmployeeEntity> employees, DateTime month)
        {
            //*******************     Get data **************************************************************** //
            var listData = new List<Dictionary<object, object>>();
            foreach (var employee in employees)
            {
                var otHours = await _workingDayService.GetOTsHours(employee.Id, month);
                if (otHours <= 40) continue;
                var dicRow = new Dictionary<object, object>();
                dicRow.Add(nameof(EmployeeOverHourModel.EmployeeCode), employee.EmployeeCode);
                dicRow.Add(nameof(EmployeeOverHourModel.Name), employee.Name);
                dicRow.Add(nameof(EmployeeOverHourModel.DepartmentStr), employee.Department?.Name);
                dicRow.Add(nameof(EmployeeOverHourModel.JobTitleStr), employee.JobTitle?.Name);
                dicRow.Add(nameof(EmployeeOverHourModel.OTOVer40), otHours - 40);
                dicRow.Add(nameof(EmployeeOverHourModel.Note), $"Calculated at {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                listData.Add(dicRow);
            }
            //*******************    Config columns **************************************************************** //
            int beginRowIndex = 13;
            ExportSheetDataModel exportSheetDataModel = new ExportSheetDataModel();
            var sheet1 = report.Workbook.Worksheets[sheet];
            exportSheetDataModel.SheetIndex = sheet1.Index;
            // special cell
            exportSheetDataModel.DicCellName2Value.Add("D6", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            //
            int columnIndex = 1;
            exportSheetDataModel.BeginNoNumber = 1;
            exportSheetDataModel.BeginRowIndex = beginRowIndex;
            exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, "[No.]");
            exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeOverHourModel.EmployeeCode));
            exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeOverHourModel.Name));
            exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeOverHourModel.JobTitleStr));
            exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeOverHourModel.DepartmentStr));
            exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeOverHourModel.OTOVer40));
            exportSheetDataModel.DicColumnIndex2EnvKey.Add(columnIndex++, nameof(EmployeeOverHourModel.Note));

            int insertEmptyRow = listData.Count - 20;
            if (insertEmptyRow > 0)
            {
                sheet1.InsertRow(beginRowIndex + 1, listData.Count, beginRowIndex);
            }
            exportSheetDataModel.ListChartDataModel = listData;
            long insertedRowCount = ExportExcelService.SetFormatExcelHistory(
                    report.Workbook.Worksheets,
                    exportSheetDataModel);
        }
        public async Task<AttendanceOTsResponse> GetListOTs(
          EmployeeAttendanceListOTRequest request = null
         )
        {

            if (request == null) throw new ArgumentNullException("Invalid argument");

            string cacheKey = $"GetListEmployeeDepartment";
            if (request != null)
            {
                cacheKey = $"{cacheKey}:{request.GetKeyCache()}_{request.DepartmentId}";
            }
            cacheKey += $"{request.FilterStr.ToJson()}";
            cacheKey += $"{request.SearchStr.ToJson()}";
            cacheKey += $"{request.SortStr.ToJson()}";
            cacheKey += $"{request.DateFrom.ToJson()}";
            cacheKey += $"{request.DateTo.ToJson()}";
            cacheKey = EncodeUtil.MD5(cacheKey);

            AttendanceOTsResponse ret = CachedFunc.GetRedisData<AttendanceOTsResponse>(cacheKey, null);
            //if (ret == null)
            {
                ret = new AttendanceOTsResponse();
                OTListPagingResponseModel oTListPagingResponseModel = new OTListPagingResponseModel();
                List<WorkingDayEntity> listOT = new List<WorkingDayEntity>();

                // get summarize 
                Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(request.FilterStr);
                SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(request.SearchStr, true);
                Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(request.SortStr);
                var listHoliday = _holidayScheduleRepository.GetList();
                var listWorkingType = _workingTypeRepository.GetAll();
                var listWkTypeOT = await _workingTypeRepository.GetOTTypes();
                var wkOTIds = listWkTypeOT.Select(x => x.Id);
                var wkOTCodes = listWkTypeOT.Select(x => x.Code);
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

                var listEmployee = _employeeRepository.GetListEmployeeDepartment(request, filterParams, searchParams, sortParams);
                if (listEmployee != null && listEmployee.Any())
                {
                    var listEmployeeId = listEmployee?.Select(x => x.Id).ToList();
                    var requestDate = new DateTime(request.DateFrom.Value.Year, request.DateFrom.Value.Month, 1, 0, 0, 0);
                    var startOfMonth = new DateTime(requestDate.Year, requestDate.Month, 1, 0, 0, 0);
                    var daysOfMonth = DateTime.DaysInMonth(requestDate.Year, requestDate.Month);
                    if (requestDate.Month == request.DateFrom.Value.Month && requestDate.Year == request.DateFrom.Value.Year)
                    {
                        startOfMonth = new DateTime(requestDate.Year, requestDate.Month, request.DateFrom.Value.Day, 0, 0, 0);
                    }
                    var endOfMonth = new DateTime(requestDate.Year, requestDate.Month, daysOfMonth, 0, 0, 0);
                    if (requestDate.Month == request.DateTo.Value.Month && requestDate.Year == request.DateTo.Value.Year)
                    {
                        endOfMonth = new DateTime(requestDate.Year, requestDate.Month, request.DateTo.Value.Day, 0, 0, 0);
                    }
                    var dicEmployeeId2Workingdays = _workingdayRepository.GetEmployeeWorkingDayByDate(
                        listEmployeeId,
                        startOfMonth,
                        endOfMonth);

                    foreach (var keyVal in dicEmployeeId2Workingdays)
                    {
                        var employeeId = keyVal.Key;
                        var listWorkingDay = keyVal.Value;
                        double? newTimeDeviation = null;
                        // Set recommentType
                        var dicWorkingDayId2Type = _workingDayService.CalculateMonthWorkingType(employeeId, listWorkingDay, listWorkingType, request.DateFrom, request.DateTo, dicHolidayScheduleStartDate);
                        for (int i = 0; i < listWorkingDay.Count; i++)
                        {
                            var item = listWorkingDay[i];
                            if (dicWorkingDayId2Type.ContainsKey(item.Id))
                            {
                                item.RecommendType = dicWorkingDayId2Type[item.Id].WorkingType.Code;
                                if (item.TimeDeviation == null)
                                {
                                    item.TimeDeviation = dicWorkingDayId2Type[item.Id].TimeDeviatioinInSeconds;
                                }
                                item.WorkingDayHighlight = dicWorkingDayId2Type[item.Id].workingDayHighlight;

                                if (item.WorkingType == null)
                                {
                                    // nếu tăng ca ngày t7 chủ nhật thì trực tiếp set OT mà k cần đợi approve => request by Mr.Đức ngày 20/06/2024
                                    //if ((listHoliday.Where(x => x.StartDate <= item.WorkingDate && x.EndDate >= item.WorkingDate).FirstOrDefault() != null) || item.WorkingDate.GetValueOrDefault().DayOfWeek == DayOfWeek.Sunday || item.WorkingDate.GetValueOrDefault().DayOfWeek == DayOfWeek.Saturday)
                                    //if (IsWeekenOrHoliday(item.WorkingDate.Value, dicHolidayScheduleStartDate))
                                    //{
                                    //    item.WorkingType = listWorkingType.FirstOrDefault(x => x.Code == item.RecommendType);
                                    //    item.WorkingTypeEntityId = item.WorkingType?.Id;
                                    //}
                                }
                            }
                            else
                            {
                                _logger.LogMsg(Messages.ErrException, $"Employee: {employeeId}, WorkingDayObj: {item.Id} have not recommendType");
                            }

                            // Default workingType
                            object workingType = EnumWorkingDayType.Type_P;
                            int code = 0;
                            bool isNumFormat = false;
                            if (item.WorkingDate == null)
                            {
                                _logger.LogMsg(Messages.ErrBaseException, $"EmployeeId {employeeId}, WorkingDate == null");
                                continue;
                            }
                            int day = item.WorkingDate.Value.Day;
                            if (item.WorkingType != null)
                            {

                                if (int.TryParse(item.WorkingType.Code, out code))
                                {
                                    workingType = code;
                                }
                                else
                                {
                                    workingType = item.WorkingType.Code;
                                }
                            }

                            else if (item.RecommendType != null)
                            {
                                item.RecommendType = WorkingShiftModel.ConvertOTType2DefaultType(item.RecommendType, item.WorkingDate, dicHolidayScheduleStartDate, ref newTimeDeviation);
                                if (int.TryParse(item.RecommendType, out code))
                                {
                                    workingType = code;
                                }
                                else
                                {
                                    workingType = item.RecommendType;
                                }
                            }
                        }

                        var listWorkingdayOT = listWorkingDay.Where(x => x.Employee != null && x.Employee.DeletedFlag != true /*&& pagingReq.DepartmentId.Contains(x.Employee.DepartmentId.Value)*/)
                                 .Where(x => (x.WorkingType == null && wkOTCodes.Contains(x.RecommendType)) || (x.WorkingType != null && wkOTCodes.Contains(x.WorkingType.Code)));

                        if (listWorkingdayOT.Any())
                        {
                            listOT.AddRange(listWorkingdayOT);
                        }
                    }
                    if (listOT.Any())
                    {
                        oTListPagingResponseModel = _workingdayRepository.GetListOTWorkingDayV2(request, filterParams, searchParams, sortParams, listOT, listWkTypeOT);
                    }
                }
                var lang = string.IsNullOrEmpty(request.Language) ? EnumLanguage.EN.ToString() : request.Language.ToUpper();
                var disPlayProps = JsonPropertyHelper<OTListResponseModel>.GetFilterProperties();
                var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
                columns = OTListResponseModel.AddKeySearchFilterable(columns);
                oTListPagingResponseModel.Columns = columns;
                ret.AttendanceRecord = oTListPagingResponseModel;

                //set cache
                CachedFunc.AddEntityCacheKey(_workingdayRepository.GetName(), cacheKey, true);
                CachedFunc.SetRedisData(cacheKey, ret, ConstCommon.ConstGetListCacheExpiredTimeInSeconds);
            }
            return ret;
        }
        public void CalculateEmpWkType(long empId,
            DateTime time, List<WorkingTypeEntity> listWorkingType)
        {
            var datefrom = new DateTime(time.Year, time.Month, 1);
            var dateto = new DateTime(time.Year, time.Month, DateTime.DaysInMonth(time.Year, time.Month));

            var listWorkingDay = _workingdayRepository.GetEmployeeWorkingDayByDate(
                                    empId,
                                    datefrom,
                                    dateto);

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
            var employeeId = empId;
            //var listWorkingDay = listWorkingday;

            Dictionary<string, WorkingTypeEntity> dicType2Entity = listWorkingType.ToDictionary(x => x.Code, x => x);

            // Set recommentType
            var dicWorkingDayId2Type = _workingDayService.CalculateMonthWorkingType(employeeId, listWorkingDay, listWorkingType, datefrom, dateto, dicHolidayScheduleStartDate);
            for (int i = 0; i < listWorkingDay.Count; i++)
            {
                var workingDay = listWorkingDay[i];
                if (dicWorkingDayId2Type.ContainsKey(workingDay.Id))
                {
                    workingDay.RecommendType = dicWorkingDayId2Type[workingDay.Id].WorkingType.Code;
                    //if (workingDay.TimeDeviation == null)
                    //{
                    workingDay.TimeDeviation = dicWorkingDayId2Type[workingDay.Id].TimeDeviatioinInSeconds;
                    //}
                    workingDay.WorkingDayHighlight = dicWorkingDayId2Type[workingDay.Id].workingDayHighlight;

                    //// nếu tăng ca ngày t7 chủ nhật thì trực tiếp set OT mà k cần đợi approve => request by Mr.Đức ngày 20/06/2024
                    //if (IsWeekenOrHoliday(workingDay.WorkingDate.Value, dicHolidayScheduleStartDate))
                    //{
                    //    workingDay.WorkingTypeEntityId = dicWorkingDayId2Type[workingDay.Id].WorkingType.Id;
                    //}
                    //else
                    //{
                    //    // Ngày thường không tính OT
                    //    double? newTimeDeviation = workingDay.TimeDeviation;
                    //    string newRcmTypeStr = WorkingShiftModel.ConvertOTType2DefaultType(dicWorkingDayId2Type[workingDay.Id].WorkingType.Code, workingDay.WorkingDate, dicHolidayScheduleStartDate, ref newTimeDeviation);
                    //    if (dicType2Entity.ContainsKey(newRcmTypeStr))
                    //    {
                    //        workingDay.WorkingTypeEntityId = dicType2Entity[newRcmTypeStr].Id;
                    //    }
                    //    else
                    //    {
                    //        workingDay.WorkingTypeEntityId = null;
                    //    }
                    //}

                    // Ngày nghỉ: set lại type theo ConvertOTType2DefaultType (1-11=>8, 12-16=>12), set lại timeDeviation
                    // Ngày thường: set lại type theo ConvertOTType2DefaultType (1-16=>8)
                    double? newTimeDeviation = workingDay.TimeDeviation;
                    string newRcmTypeStr = WorkingShiftModel.ConvertOTType2DefaultType(workingDay.RecommendType, workingDay.WorkingDate, dicHolidayScheduleStartDate, ref newTimeDeviation);
                    if (newTimeDeviation != workingDay.TimeDeviation)
                    {
                        _logger.LogInformation($"[CalculateEmpWkType] TimeDeviation Updated, oldDeviation: {workingDay.TimeDeviation}, newTimeDeviation: {newTimeDeviation}, WorkingDayId: {workingDay.WorkingDayId}");
                        workingDay.TimeDeviation = newTimeDeviation;
                    }

                    if (dicType2Entity.ContainsKey(newRcmTypeStr))
                    {
                        workingDay.WorkingTypeEntityId = dicType2Entity[newRcmTypeStr].Id;
                    }
                    else
                    {
                        workingDay.WorkingTypeEntityId = null;
                    }

                    _workingdayRepository.Upsert(workingDay);
                }
                else
                {
                    _logger.LogMsg(Messages.ErrException, $"---CalculateEmpWkType: Employee: {employeeId}, WorkingDayObj: {workingDay.Id} have not recommendType");
                }
            }
        }

        public bool IsWeekenOrHoliday(DateTime checkDay, Dictionary<string, HolidayScheduleEntity> dicHolidayScheduleStartDate)
        {
            string holidayKey = DateTimeUtil.GetDateTimeStr(checkDay, ConstDateTimeFormat.YYYYMMDD);

            var isWeekend = checkDay.DayOfWeek == DayOfWeek.Sunday || checkDay.DayOfWeek == DayOfWeek.Saturday;
            var isHoliday = dicHolidayScheduleStartDate.ContainsKey(holidayKey);

            return isWeekend || isHoliday;
        }

        //public bool HasWorkingDayOTType(List<WorkingDayEntity> listWorkingDay)
        //{
        //    return listWorkingDay.Any(workingDay =>
        //        (workingDay.WorkingType != null && WorkingShiftModel.IsOTType(workingDay.WorkingType.ToString()) == 1) ||
        //        (workingDay.RecommendType != null && WorkingShiftModel.IsOTType(workingDay.RecommendType.ToString()) == 1));
        //}

        public bool HasWorkingDayOTType(List<WorkingDayEntity> listWorkingDay, Dictionary<string, HolidayScheduleEntity> dicHolidayScheduleStartDate)
        {
            double? newTimeDeviation = 0;
            int code = 0;
            object workingType = EnumWorkingDayType.Type_P;
            //object workingType = null;
            foreach (var workingday in listWorkingDay)
            {
                if (workingday.WorkingType != null)
                {
                    if (int.TryParse(workingday.WorkingType.Code, out code))
                    {
                        workingType = code;
                    }
                    else
                    {
                        workingType = workingday.WorkingType.Code;
                    }

                    if (IsWeekenOrHoliday(workingday.WorkingDate.Value, dicHolidayScheduleStartDate) && workingType.ToString() != EnumWorkingDayType.Type_0)
                    {
                        return true;
                    }

                    var otType = WorkingShiftModel.IsOTType(workingType.ToString());
                    if (otType == 1)
                    {
                        return true;
                    }
                }
                else if (workingday.RecommendType != null)
                {
                    workingday.RecommendType = WorkingShiftModel.ConvertOTType2DefaultType(workingday.RecommendType, workingday.WorkingDate, dicHolidayScheduleStartDate, ref newTimeDeviation);
                    if (int.TryParse(workingday.RecommendType, out code))
                    {
                        workingType = code;
                    }
                    else
                    {
                        workingType = workingday.RecommendType;
                    }
                    if (IsWeekenOrHoliday(workingday.WorkingDate.Value, dicHolidayScheduleStartDate) && workingType.ToString() != EnumWorkingDayType.Type_0)
                    {
                        return true;
                    }
                    var otType = WorkingShiftModel.IsOTType(workingType.ToString());
                    if (otType == 1)
                    {
                        return true;
                    }
                }
                else if (workingday.RecommendType == null && workingday.WorkingType == null)
                {
                    continue;
                }
            }
            return false;
        }

    }
}