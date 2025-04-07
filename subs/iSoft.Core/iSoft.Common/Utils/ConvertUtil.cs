using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace iSoft.Common.Utils
{
	public class ConvertUtil
	{
		public static List<T> RemoveDuplicate<T>(List<T> list)
		{
			return list.Distinct().ToList();
		}

        public static string GetString(object value)
		{
			if (value == null)
			{
				return "";
			}
			return value.ToString();
		}
		public static byte? ConvertToNullableByte(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is byte byteValue)
			{
				return byteValue;
			}
			else if (value is string stringValue && byte.TryParse(stringValue, out byte parsedValue))
			{
				return parsedValue;
			}
			else if (byte.TryParse(value.ToString(), out byte parsedValue2))
			{
				return parsedValue2;
			}

			return null;
		}
		public static long? ConvertToNullableLong(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is long longValue)
			{
				return longValue;
			}
			else if (value is string stringValue && long.TryParse(stringValue, out long parsedValue))
			{
				return parsedValue;
			}
			else if (long.TryParse(value.ToString(), out long parsedValue2))
			{
				return parsedValue2;
			}

			return null;
		}
		public static DateTime? ConvertToNullableDateTime(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is DateTime datetimeValue)
			{
				return datetimeValue;
			}
			else if (value is string stringValue && DateTime.TryParse(stringValue, out DateTime parsedValue))
			{
				return parsedValue;
			}
			else if (DateTime.TryParse(value.ToString(), out DateTime parsedValue2))
			{
				return parsedValue2;
			}
			else if (double.TryParse(value.ToString(), out double result))
			{
				return DateTime.FromOADate(result);
			}
			return null;
		}
		public static int? ConvertToNullableInt(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is int value2)
			{
				return value2;
			}
			else if (value is string stringValue && int.TryParse(stringValue, out int parsedValue))
			{
				return parsedValue;
			}
			else if (int.TryParse(value.ToString(), out int parsedValue2))
			{
				return parsedValue2;
			}

			return null;
		}
		public static short? ConvertToNullableShort(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is short shortValue)
			{
				return shortValue;
			}
			else if (value is string stringValue && short.TryParse(stringValue, out short parsedValue))
			{
				return parsedValue;
			}
			else if (short.TryParse(value.ToString(), out short parsedValue2))
			{
				return parsedValue2;
			}

			return null;
		}
		public static double? ConvertToNullableDouble(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is double trustValue)
			{
				return trustValue;
			}
			else if (value is string stringValue && double.TryParse(stringValue, out double parsedValue))
			{
				return parsedValue;
			}
			else if (double.TryParse(value.ToString(), out double parsedValue2))
			{
				return parsedValue2;
			}

			return null;
		}
		public static bool? ConvertToNullableBool(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is bool trustValue)
			{
				return trustValue;
			}
			else if (value is int intValue)
			{
				if (intValue == 1)
				{
					return true;
				}
				else if (intValue == 0)
				{
					return false;
				}
			}
			else if (value is string stringValue)
			{
				if (stringValue.ToLower() == "true" || stringValue.ToLower() == "1")
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			return null;
		}


		private static char[] chars = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

		public static int GetColumnIndex(string cellName)
		{
			string result = Regex.Replace(cellName, @"[^a-zA-Z]", "");
			return GetColumnIndexByColumnName(result);
		}
		public static int GetRowIndex(string cellName)
		{
			string result = Regex.Replace(cellName, @"[^\d]", "");
			return int.Parse(result);
		}
		public static string GetCellNameByIndex(int rowInex, int columnIndex)
		{
			return GetColumnNameByColumnIndex(columnIndex) + rowInex;
		}
		public static string GetColumnNameByColumnIndex(int index)
		{
			index -= 1; //adjust so it matches 0-indexed array rather than 1-indexed column

			int quotient = index / 26;
			if (quotient > 0)
				return GetColumnNameByColumnIndex(quotient) + chars[index % 26].ToString();
			else
				return chars[index % 26].ToString();
		}

		public static string GetCellNext(string cellName, Direct direct, int step = 1)
		{
			int rowIndex = GetRowIndex(cellName);
			int columnIndex = GetColumnIndex(cellName);
			switch (direct)
			{
				case Direct.Down:
					rowIndex += step;
					break;
				case Direct.Up:
					rowIndex -= step;
					break;
				case Direct.Right:
					columnIndex += step;
					break;
				case Direct.Left:
					columnIndex -= step;
					break;
			}
			if (rowIndex >= 1 && columnIndex >= 1)
			{
				return GetCellNameByIndex(rowIndex, columnIndex);
			}
			return null;
		}
		public static int GetColumnIndexByColumnName(string columnName)
		{
			int number = 0;
			int pow = 1;
			for (int i = columnName.Length - 1; i >= 0; i--)
			{
				number += (columnName[i] - 'A' + 1) * pow;
				pow *= 26;
			}

			return number;
		}
    }
	public enum Direct
	{
		Up,
		Down,
		Left,
		Right,
	}
}
