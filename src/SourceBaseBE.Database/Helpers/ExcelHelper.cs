using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Helpers
{
  public class ExcelHelper
  {
    public static string GetCellValue(ExcelWorksheet worksheet, int row, int column)
    {
      string ret = "";
      try
      {
        if ((column > 0) && (row > 0))
        {
          if (worksheet.Cells[row, column].Value != null)
          {
            try
            {
              ret = worksheet.Cells[row, column].Value.ToString().Trim();
            }
            catch
            {
              ret = "";
            }
          }
          else
          {
            ret = "";
          }
        }
      }
      catch
      {
        ret = "";
      }
      return ret;
    }

    public static long GetLongCellValue(ExcelWorksheet worksheet, int row, int column)
    {
      object cellValue = worksheet.Cells[row, column].Value;
      if (cellValue != null && long.TryParse(cellValue.ToString(), out long result))
      {
        return result;
      }
      else
      {
        return 0;
      }
    }

    public static int GetIntCellValue(ExcelWorksheet worksheet, int row, int column)
    {
      object cellValue = worksheet.Cells[row, column].Value;
      if (cellValue != null && int.TryParse(cellValue.ToString(), out int result))
      {
        return result;
      }
      else
      {
        return 0;
      }
    }

    public static DateTime? GetDateCellValue(ExcelWorksheet worksheet, int row, int column)
    {
      DateTime? ret = null;
      try
      {
        if (column > 0 && row > 0)
        {
          var cellValue = worksheet.Cells[row, column].Value;
          if (cellValue != null && DateTime.TryParseExact(cellValue.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
          {
            ret = dateValue;
          }
        }
      }
      catch
      {
        ret = null;
      }
      return ret;
    }
  }
}
