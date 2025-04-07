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
using Microsoft.Extensions.Logging;
using iSoft.Common.Models.ResponseModels;
using iSoft.Common.Utils;
using iSoft.Database.Repository;
using OfficeOpenXml;
using SourceBaseBE.Database.Helpers;
using SourceBaseBE.Database.Models.RequestModels.Generate;
using iSoft.Common.Enums;
using System.IO;
using SourceBaseBE.Database.Models.RequestModels;
using iSoft.ExportLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using SourceBaseBE.MainService.Models.RequestModels.Report;
using System.IO.Compression;
using System.Threading.Tasks;
using PRPO.Database.Helpers;
using SourceBaseBE.Database.Models.ResponseModels;
using NPOI.SS.Util;

namespace SourceBaseBE.MainService.Services
{
	public class WorkingTypeService : BaseCRUDService<WorkingTypeEntity>
	{
		private Database.Repository.UserRepository _authUserRepository;
		private WorkingTypeRepository _workingTypeRepository;
		private LanguageRepository _languageRepository;
		private WorkingTypeRepository _repositoryImp;
		/*[GEN-1]*/

		public WorkingTypeService(CommonDBContext dbContext, ILogger<WorkingTypeService> logger)
		  : base(dbContext, logger)
		{
			_repository = new WorkingTypeRepository(_dbContext);
			_repositoryImp = (WorkingTypeRepository)_repository;
			_authUserRepository = new Database.Repository.UserRepository(_dbContext);
			_languageRepository = new LanguageRepository(_dbContext);
			/*[GEN-2]*/
		}
		public override WorkingTypeEntity GetById(long id, bool isDirect = false, bool isTracking = true)
		{
			var entity = _repository.GetById(id, isDirect, isTracking);
			var entityRS = (WorkingTypeEntity)_authUserRepository.FillTrackingUser(entity);
			return entityRS;
		}
		public override List<WorkingTypeEntity> GetList(PagingRequestModel pagingReq = null)
		{
			var list = _repository.GetList(pagingReq);
			return list;
		}
		public override List<Dictionary<string, object>> GetFormDataObjElement(WorkingTypeEntity entity)
		{
			string entityName = nameof(WorkingTypeEntity);
			var listRS = new List<Dictionary<string, object>>();
			List<object> objectList = null;
			var properties = typeof(WorkingTypeEntity).GetProperties().Where(p => p.Name == "Name" || p.Name == "Code" || p.Name == "Normal_Meal" ||
											  p.Name == "OT_150" || p.Name == "Normal_Night_30" || p.Name == "OT_200" ||
											  p.Name == "Weekend_Meal" || p.Name == "Weekend_OT_200" || p.Name == "Holiday_Meal" ||
											  p.Name == "Holiday_OT_300" || p.Name == "Holiday_OT_Night_390" || p.Name == "Description");
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
							//objectList = new List<object>(formDataTypeAttr.Options);
							listRS.Add(new Dictionary<string, object> {
	  {"display_name", GetDisplayName(property.Name, entityName)},
	  {"key", property.Name},
	  {"value", property.GetValue(entity)},
	  {"type", formDataTypeAttr.TypeName.ToStringValue()},
	  {"select_data", GetListOptionData(objectList)},
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
		public override WorkingTypeEntity Upsert(WorkingTypeEntity entity, long? userId = null)
		{
			List<GenTemplateEntity> genTemplateChildren = null;

			/*[GEN-3]*/
			var upsertedEntity = ((WorkingTypeRepository)_repository).Upsert(entity, genTemplateChildren/*[GEN-4]*/, userId);
			var entityRS = (WorkingTypeEntity)_authUserRepository.FillTrackingUser(upsertedEntity);
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
		private List<long> GetListIdChildren(WorkingTypeEntity entity, string childEntity)
		{
			switch (childEntity)
			{

				case nameof(GenTemplateEntity):
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
				case nameof(GenTemplateEntity):
					break;
				/*[GEN-6]*/
				default:
					break;
			}
			return listRS;
		}

		public WorkingTypePagingResponseModel GetListSymbol(
		   PagingFilterRequestModel pagingReq = null)
		{
			List<WorkingTypeListReponseModel> listResponseModel = null;

			Dictionary<string, object> filterParams = StringToDictionaryHelper.ToStringAndObj(pagingReq.FilterStr);
			SearchModel searchParams = StringToDictionaryHelper.ToDicOrString(pagingReq.SearchStr, true);
			Dictionary<string, long> sortParams = StringToDictionaryHelper.ToStringLongTest(pagingReq.SortStr);
			var ret = _repositoryImp.GetListSymbol(pagingReq, filterParams, searchParams, sortParams);
			var disPlayProps = JsonPropertyHelper<WorkingTypeListReponseModel>.GetFilterProperties();
			var lang = string.IsNullOrEmpty(pagingReq.Language) ? EnumLanguage.EN.ToString() : pagingReq.Language.ToUpper();
			var columns = this._languageRepository.ReadColumnLanguage(disPlayProps, lang);
			columns = WorkingTypeListReponseModel.AddKeySearchFilterable(columns);
			ret.Columns = columns;
			return ret;

		}

		public async Task<bool> ImportFileSymbel(WorkingTypeRequestModel request)
		{
			var dicFormFile = request.GetFiles();
			if (dicFormFile == null || dicFormFile.Count < 1) return false;
			string filePath = UploadUtil.UploadFileExcel(dicFormFile);
			FileInfo existingFile = new FileInfo(filePath);
			if (!existingFile.Exists == true) return false;
			using (var package = ExcelPro.LoadTempFile(filePath))
			{
				var workBook = package.Workbook;
				if (workBook == null || workBook.Worksheets.Count < 1) return false;
				/* Check if barcode network*/

				List<WorkingTypeEntity> listWorkTypeEntity = new List<WorkingTypeEntity>();
				WorkingTypeEntity workTypeEntity = new WorkingTypeEntity();
				List<object> listData = new List<object>();
				bool IsExit = false;
				for (int i = 0; (i < workBook.Worksheets.Count) && (IsExit == false); i++)
				{
					string sheetName = workBook.Worksheets[i].Name.ToString().Trim().ToLower();
					if (sheetName != Environment.GetEnvironmentVariable("WorkingTypeSheetName")) continue;
					ExcelWorksheet Worksheet = workBook.Worksheets[i];
					IsExit = true;

					int StartRow = 0;
					int COL_Description = 1;

					int COL_Symbol = 2;
					int COL_Meal = 3;
					int COL_OT_Hrs_150 = 4;
					int COL_OT_Night_30 = 5;
					int COL_OT_Hrs_200 = 6;

					int COL_Meal_Week = 10;
					int COL_OT_Hrs_200_Week = 11;
					int COL_OT_Night_270 = 12;

					int COL_Meal_Holiday = 16;
					int COL_OT_Hrs_300_Holiday = 17;
					int COL_OT_Night_390_Holiday = 18;
					string tempDescription = "";

					//Find start-row
					bool is_exit_start_row = false;

					for (int row = 0; (row <= Worksheet.Dimension.End.Row) && (is_exit_start_row == false); row++)
					{
						string description = ExcelHelper.GetCellValue(Worksheet, row, COL_Description);
						if (description.Trim().ToLower() == "diễn giải")
						{
							is_exit_start_row = true;
							StartRow = row + 1;
							break;
						}
					}
					int nCountToExit = 0;
					for (int row = StartRow; (row <= Worksheet.Dimension.End.Row) && (nCountToExit < 10); row++)
					{
						string description = ExcelHelper.GetCellValue(Worksheet, row, COL_Description);
						string symbol = ExcelHelper.GetCellValue(Worksheet, row, COL_Symbol);
						int meal = ExcelHelper.GetIntCellValue(Worksheet, row, COL_Meal);
						int ot_Hrs_150 = ExcelHelper.GetIntCellValue(Worksheet, row, COL_OT_Hrs_150);
						int ot_Night_30 = ExcelHelper.GetIntCellValue(Worksheet, row, COL_OT_Night_30);
						int ot_Hrs_200 = ExcelHelper.GetIntCellValue(Worksheet, row, COL_OT_Hrs_200);
						int meal_Week = ExcelHelper.GetIntCellValue(Worksheet, row, COL_Meal_Week);
						int ot_Hrs_200_Week = ExcelHelper.GetIntCellValue(Worksheet, row, COL_OT_Hrs_200_Week);
						int ot_Night_270 = ExcelHelper.GetIntCellValue(Worksheet, row, COL_OT_Night_270);
						int meal_Holiday = ExcelHelper.GetIntCellValue(Worksheet, row, COL_Meal_Holiday);
						int ot_Hrs_300_Holiday = ExcelHelper.GetIntCellValue(Worksheet, row, COL_OT_Hrs_300_Holiday);
						int ot_Night_390_Holiday = ExcelHelper.GetIntCellValue(Worksheet, row, COL_OT_Night_390_Holiday);
						string descriptionNext = ExcelHelper.GetCellValue(Worksheet, row + 1, COL_Description);
						if (symbol == "0" || symbol == "")
						{
							nCountToExit++;
							continue;
						}
						if (description != "" && descriptionNext == "")
						{
							tempDescription = description;
						}
						var workingType = _repositoryImp.GetBySymbol(symbol);
						if (workingType != null)
						{
							workTypeEntity = workingType;
						}
						else
						{
							workTypeEntity = new WorkingTypeEntity();
						}
						workTypeEntity.Code = symbol;
						workTypeEntity.Name = symbol;
						workTypeEntity.Normal_Meal = meal;
						workTypeEntity.OT_150 = ot_Hrs_150;
						workTypeEntity.Normal_Night_30 = ot_Night_30;
						workTypeEntity.OT_200 = ot_Hrs_200;

						workTypeEntity.Weekend_Meal = meal_Week;
						workTypeEntity.Weekend_OT_200 = ot_Hrs_200_Week;
						workTypeEntity.Weekend_Night_OT_270 = ot_Night_270;

						workTypeEntity.Holiday_Meal = meal_Holiday;
						workTypeEntity.Holiday_OT_300 = ot_Hrs_300_Holiday;
						workTypeEntity.Holiday_OT_Night_390 = ot_Night_390_Holiday;

						if (description == "")
						{
							workTypeEntity.Description = tempDescription;
						}
						else
						{
							workTypeEntity.Description = description;
						}
						listWorkTypeEntity.Add(workTypeEntity);
						listData.AddRange(listWorkTypeEntity);
					}
				}
				if (listWorkTypeEntity != null && listWorkTypeEntity.Count > 0)
				{
					listWorkTypeEntity.OrderBy(p => p.Name);
					_repositoryImp.UpSertMulti(listWorkTypeEntity);
				}
				else
				{
					return false;
				}
				//check remove data not include file excel import 
				if (request.IsReplace.GetValueOrDefault())
				{
					var listEmployees = _repositoryImp.RemoveIfNotExistInList(listWorkTypeEntity);
				}
			}
			return true;
		}

		public async Task<IActionResult> ExportSymbol(PagingRequestModel request)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));
			//var templatePath = "./ReportTemplate/TemplateReportSymbol.xlsx";
			var templatePath = ConstMain.ConstTemplateFilePath;
			string nameSheet = "Quy ước ký hiệu";
			int startRow = 5;
			int startRowMerger = 5;
			int numberRowSemilar = 0;
			string tempDescription = "";
			using (var outStream = new MemoryStream())
			{
				using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
				{
					bool isTemplateExist = File.Exists(templatePath);
					_logger.LogInformation($"Template Path: {templatePath}");
					_logger.LogInformation($"Template Path Exist: {isTemplateExist}");
					var templatePackage = ExcelPro.LoadTempFile(templatePath);
					var symbols = _repositoryImp.GetList();
					//templatePackage = ExcelPro.CloneWorkbook(ref templatePackage, "new.xlsx");
					//ExcelPro.AddNewWorksheet(ref templatePackage, nameSheet, "TemplateSheetSymbol");

					foreach (var symbol in symbols)
					{
						if (tempDescription != symbol.Description)
						{

							if (((startRowMerger + numberRowSemilar) - startRowMerger) > 0)
							{
								//ExcelPro.MergeCells(ref templatePackage, nameSheet, $"A{startRow - numberRowSemilar - 1}", $"A{startRow - 1}", tempDescription);
                ExcelPro.SetValue(ref templatePackage, nameSheet, $"A{startRow}", symbol.Description);
              }
							else
							{
								// set description 
								ExcelPro.SetValue(ref templatePackage, nameSheet, $"A{startRow}", symbol.Description);
							}
							tempDescription = symbol.Description;
							startRowMerger = startRow;
							numberRowSemilar = 0;
						}
						else
						{
							numberRowSemilar++;
						}

						// set data fomular
						ExcelPro.SetValue(ref templatePackage, nameSheet, $"B{startRow}", symbol.Code);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"C{startRow}", symbol.Normal_Meal);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"D{startRow}", symbol.OT_150);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"E{startRow}", symbol.Normal_Night_30);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"F{startRow}", symbol.OT_200);

						// set data fomular
						ExcelPro.SetValue(ref templatePackage, nameSheet, $"I{startRow}", symbol.Code);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"J{startRow}", symbol.Weekend_Meal);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"K{startRow}", symbol.Weekend_OT_200);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"L{startRow}", symbol.Weekend_Night_OT_270);

						// set data fomular
						ExcelPro.SetValue(ref templatePackage, nameSheet, $"O{startRow}", symbol.Code);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"P{startRow}", symbol.Holiday_Meal);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"Q{startRow}", symbol.Holiday_OT_300);
						ExcelPro.SetValueInt(ref templatePackage, nameSheet, $"R{startRow}", symbol.Holiday_OT_Night_390);

						startRow++;
					}
					 

					var path = $"./{nameSheet}.xlsx";

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
	}
}