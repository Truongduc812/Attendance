using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;
using System.Globalization;
using NPOI.OpenXmlFormats.Dml;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Common.Utils;
using iSoft.Common.Enums;
using ConnectionCommon.Connection;
using iSoft.TrackDeviceService.Models;

namespace iSoft.TrackDeviceService.Helpers
{
	public static class ModelBuilderExtensions
	{
		private static readonly string _excelFile = "./SeedData/UDF-POS2-MasterData.xlsx";
		public static void Seed(this ModelBuilder modelBuilder)
		{
			// load entities from excel

			var _parameters = loadExcel_Parameter();
			var _limitations = loadExcel_Limitation();
			//AuthGroup
			//AuthGroupPermission
			//AuthPermission
			//AuthUserGroup
			//AuthUserPermission


			modelBuilder.Entity<Param>().HasData(_parameters);
			modelBuilder.Entity<Limitation>().HasData(_limitations);
		}

		private static List<Param> loadExcel_Parameter()
		{
			var _excel = new ExcelMapper(_excelFile);

			_excel.AddMapping<Param>(columnName: "Id", p => p.Id);
			_excel.AddMapping<Param>(columnName: "Name", p => p.Name);
			_excel.AddMapping<Param>(columnName: "SerialCode", p => p.SerialCode);
			_excel.AddMapping<Param>(columnName: "Category", p => p.Category);
			_excel.AddMapping<Param>(columnName: "Description", p => p.Description);
			_excel.AddMapping<Param>(columnName: "IsEnable", p => p.isEnable)
							.SetCellUsing((c, o) =>
							{
								if (o is true) c.SetCellValue("☑️"); else c.SetCellValue("");
							})
							.SetPropertyUsing(v =>
							{
								if ((v as string) == "☑️") return true; return false;
							})
							; //"☑️" = true
			_excel.AddMapping<Param>(columnName: "IsDeleted", p => p.isDelete)
							.SetCellUsing((c, o) =>
							{
								if (o is true) c.SetCellValue("☑️"); else c.SetCellValue("");
							})
							.SetPropertyUsing(v =>
							{
								if ((v as string) == "☑️") return true; return false;
							})
							; //"☑️" = true
			var _objs = _excel.Fetch<Param>(sheetName: nameof(Param)).ToList();
			return _objs;
		}
		private static List<Limitation> loadExcel_Limitation()
		{
			var _excel = new ExcelMapper(_excelFile);
			_excel.Ignore<Limitation>(l => l.Param);

			_excel.AddMapping<Limitation>(columnName: "Id", p => p.Id);
			_excel.AddMapping<Limitation>(columnName: "Name", p => p.Name);
			_excel.AddMapping<Limitation>(columnName: "SerialCode", p => p.SerialCode);
			_excel.AddMapping<Limitation>(columnName: "LowerLimit", p => p.LowerLimit);
			_excel.AddMapping<Limitation>(columnName: "UpperLimit", p => p.UpperLimit);
			_excel.AddMapping<Limitation>(columnName: "TargetValue", p => p.TargetValue);
			_excel.AddMapping<Limitation>(columnName: "IsEnable", p => p.IsEnabled)
							.SetCellUsing((c, o) =>
							{
								if (o is true) c.SetCellValue("☑️"); else c.SetCellValue("");
							})
							.SetPropertyUsing(v =>
							{
								if ((v as string) == "☑️") return true; return false;
							})
							; //"☑️" = true
			_excel.AddMapping<Limitation>(columnName: "ParameterId", p => p.ParamId);
			_excel.AddMapping<Limitation>(columnName: "IsDeleted", p => p.isDelete)
							.SetCellUsing((c, o) =>
							{
								if (o is true) c.SetCellValue("☑️"); else c.SetCellValue("");
							})
							.SetPropertyUsing(v =>
							{
								if ((v as string) == "☑️") return true; return false;
							})
							; //"☑️" = true
			_excel.AddMapping<Limitation>(columnName: "CreatedBy", p => p.CreatedBy);
			_excel.AddMapping<Limitation>(columnName: "UpdatedAt", p => p.UpdateAt);
			_excel.AddMapping<Limitation>(columnName: "UpdatedBy", p => p.UpdatedBy);

			var _objs = _excel.Fetch<Limitation>(sheetName: nameof(Limitation)).ToList();
			return _objs;
		}
	}
}
