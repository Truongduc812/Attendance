using iSoft.Common.Enums;
using System;
using System.Collections.Generic;

namespace SourceBaseBE.MainService.Helper
{
	public static class ProjectStringToDictionaryHelper
	{
		public static Dictionary<ENumWorkingFilterKey, object> ToStringAndEnumWorkingDayFilter(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return new Dictionary<ENumWorkingFilterKey, object>();
			}

			string[] pairs = str.Split(',');

			Dictionary<ENumWorkingFilterKey, object> dictionary = new Dictionary<ENumWorkingFilterKey, object>();

			foreach (string pair in pairs)
			{
				long num;
				string[] keyValue = pair.Split(":");

				if (long.TryParse(keyValue[1], out num))
				{
					dictionary.Add(Enum.Parse<ENumWorkingFilterKey>(keyValue[0].ToLower()), num);
				}
				else
				{
					dictionary.Add(Enum.Parse<ENumWorkingFilterKey>(keyValue[0].ToLower()), keyValue[1]);
				}
			}

			return dictionary;
		}
		public static Dictionary<ENumWorkingSearchKey, object> ToStringAndEnumWorkingDaySearch(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return new Dictionary<ENumWorkingSearchKey, object>();
			}

			string[] pairs = str.Split(',');

			Dictionary<ENumWorkingSearchKey, object> dictionary = new Dictionary<ENumWorkingSearchKey, object>();

			foreach (string pair in pairs)
			{
				long num;
				string[] keyValue = pair.Split(":");

				if (long.TryParse(keyValue[1], out num))
				{
					dictionary.Add(Enum.Parse<ENumWorkingSearchKey>(keyValue[0].ToLower()), num);
				}
				else
				{
					dictionary.Add(Enum.Parse<ENumWorkingSearchKey>(keyValue[0].ToLower()), keyValue[1]);
				}
			}

			return dictionary;
		}
		public static Dictionary<ENumWorkingSortKey, object> ToStringAndEnumWorkingDaySort(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return new Dictionary<ENumWorkingSortKey, object>();
			}

			string[] pairs = str.Split(',');

			Dictionary<ENumWorkingSortKey, object> dictionary = new Dictionary<ENumWorkingSortKey, object>();

			foreach (string pair in pairs)
			{
				long num;
				string[] keyValue = pair.Split(":");

				if (long.TryParse(keyValue[1], out num))
				{
					dictionary.Add(Enum.Parse<ENumWorkingSortKey>(keyValue[0].ToLower()), num);
				}
				else
				{
					dictionary.Add(Enum.Parse<ENumWorkingSortKey>(keyValue[0].ToLower()), keyValue[1]);
				}
			}

			return dictionary;
		}
	}
}
