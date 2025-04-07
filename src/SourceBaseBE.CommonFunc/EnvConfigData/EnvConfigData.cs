using iSoft.Common.Enums;
using iSoft.Common.Util;
using iSoft.Common.Utils;
using iSoft.ExcelHepler;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceBaseBE.Database.Entities;

namespace SourceBaseBE.CommonFunc.EnvConfigData
{

	public class EnvConfigData
	{
		private static object lockObj = new object();
		private static EnvConfigData ins;
		private bool isLoadingEnvConfig = false;
		private List<EnvConfigModel> arrExampleEnvConfig;
		private Dictionary<string, EnvConfigModel> dicEnvConfig = new Dictionary<string, EnvConfigModel>();
		private EnvConfigData() { }
		public static EnvConfigData Ins
		{
			get
			{
				lock (lockObj)
				{
					if (ins == null)
					{
						ins = new EnvConfigData();
					}
					if (!ins.isLoadingEnvConfig)
						ins.GetEnvConfigFromExcel().Wait();
					return ins;
				}
			}
		}
		public List<EnvConfigModel> GetListEnvConfigModel() => this.arrExampleEnvConfig;
		private async Task<List<EnvConfigModel>> GetEnvConfigFromExcel(string filePath = "./setting/mapping.xlsx")
		{
			try
			{
				List<EnvConfigModel> listParams = new List<EnvConfigModel>();
				//var path 
				var mapping = (await ExcelHepler.GetSheet<EnvConfigModel>(filePath, "Parameter", "A1")).ToList();
				for (int i = mapping.Count - 1; i >= 0; i--)
				{
					if (!mapping[i].IsValid())
					{
						mapping.RemoveAt(i);
						continue;
					}
					mapping[i].EnviromentVarName = mapping[i].EnviromentVarName;
				}
				arrExampleEnvConfig = mapping;

				foreach (var item in arrExampleEnvConfig)
				{
					if (!dicEnvConfig.ContainsKey(item.GetKey()))
					{
						dicEnvConfig.Add(item.GetKey(), item);
					}
				}

				isLoadingEnvConfig = true;
				return arrExampleEnvConfig;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public EnvConfigModel GetEnvConfigBySearchField(string searchField, long connectionId)
		{
			var key = searchField.ConvertToESField(connectionId);
			if (dicEnvConfig.ContainsKey(key))
			{
				return dicEnvConfig[key];
			}
			return null;
		}
		public string GetSearchPatternByEnvESFieldName(string searchField, long connectionId)
		{
			return arrExampleEnvConfig.FirstOrDefault(s => s.GetESFieldName2() == searchField.ConvertToESField(connectionId))?.GetESPatternSearch();
		}
	}

}
