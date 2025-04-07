using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
namespace iSoft.ExcelHepler
{
	public class ExcelHepler
	{
		public async static Task<IEnumerable<T>> GetSheet<T>(string path, string sheetName) where T : class, new()
		{
			try
			{
				using (var stream = File.OpenRead(path))
				{
					return await stream.QueryAsync<T>(sheetName);
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public static Task<IEnumerable<T>> GetSheet<T>(string path, string sheetName, string startCell) where T : class, new()
		{
			try
			{
				return MiniExcel.QueryAsync<T>(path, sheetName: sheetName, startCell: startCell);
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public async static Task<DataTable> GetSheetAsDataTable<T>(string path, string sheetName) where T : class, new()
		{
			try
			{
				using (var stream = File.OpenRead(path))
				{
					return await stream.QueryAsDataTableAsync(sheetName: sheetName);
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public async static Task<DataTable> GetSheetAsDataTable<T>(string path, string sheetName, string startCell) where T : class, new()
		{
			try
			{
				using (var stream = File.OpenRead(path))
				{
					return await stream.QueryAsDataTableAsync(sheetName: sheetName, startCell: startCell);
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		/// <summary>
		///  Sample values:
		///  var value = new Dictionary<string, object>()

		//["title"] = "FooCompany",
		//["managers"] = new[] {
		//    new {name="Jack",department="HR"},
		//    new {name="Loan",department="IT"}
		//},
		//["employees"] = new[] {
		//    new {name="Wade",department="HR"},
		//    new {name="Felix",department="HR"},
		//    new {name="Eric",department="IT"},
		//    new {name="Keaton",department="IT"}
		//}

		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <param name="templatePath"></param>
		/// <param name="values"></param>
		public static void SaveWithTemplate(string path, string templatePath, Dictionary<string, object> values) 
		{
			MiniExcel.SaveAsByTemplate(path, templatePath, values);
		}
	}
}
