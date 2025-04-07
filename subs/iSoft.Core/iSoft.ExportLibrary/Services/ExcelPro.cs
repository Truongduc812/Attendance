using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.ComponentModel;
using Aspose.Cells;
using System.Runtime.CompilerServices;
using InfluxDB.Client.Core;
using System.Data.Common;

namespace iSoft.ExportLibrary.Services
{
    public class ExcelPro
    {
        public static string _exportPath = "./Report/";
        public static string _templatePath = "Template/ReportTemplate.xlsx";
        public ExcelPro()
        {

        }
        public static void SetCell_SolidBackground(ExcelWorksheet worksheet, int row, int column, object value, ExcelHorizontalAlignment Alignment, Color background_color, Color ForeColor, ExcelBorderStyle excelBorderStyle = ExcelBorderStyle.Thin)
        {
            try
            {
                var cell = worksheet.Cells[row, column];
                var fill = cell.Style.Fill;
                //fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //fill.BackgroundColor.SetColor(background_color);
                cell.Value = value;
                cell.Style.Border.BorderAround(excelBorderStyle, Color.Black);
                cell.Style.HorizontalAlignment = Alignment;
                cell.Style.Font.Color.SetColor(ForeColor);
                //cell.AutoFitColumns();
            }
            catch
            {
            }
        }
        public static ExcelPackage LoadTempFile(string FileName)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return new ExcelPackage(new FileInfo(FileName));
        }

        /// <summary>
        /// Find value in cells and return address
        /// </summary>
        /// <param name="excelPackage"></param>
        /// /// <param name="value_in_cell"></param>
        /// <returns>The cell contain the value</returns>
        public static ExcelRangeBase? FindCells(ExcelPackage excelPackage, string sheetname, string valueInCells)
        {
            foreach (ExcelRangeBase cell in excelPackage.Workbook.Worksheets[sheetname].Cells)
            {
                if (cell.Value != null && cell.Value.ToString().Contains(valueInCells))
                {
                    return cell;
                }
            }
            return null;
        }
        public static void AddNewWorksheet(ref ExcelPackage excelPackage, string sheetName, string template)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            excelPackage.Workbook.Worksheets.Copy(template, sheetName);
        }
        public static bool WorksheetExists(ExcelPackage excelPackage, string sheetName)
        {
            return excelPackage.Workbook.Worksheets.Any(ws => ws.Name == sheetName);
        }
        public static void HideColumn(ExcelWorksheet worksheet, int[] columns)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                worksheet.Columns[columns[i]].Hidden = true;
            }
            //worksheet.Cells.AutoFitColumns();
        }
        public static ExcelPackage CloneWorkbook(ref ExcelPackage excelPackage, string workbookName)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            if (File.Exists(workbookName))
            {
                File.Delete(workbookName);
            }
            excelPackage.SaveAs(new FileInfo(workbookName));
            return new ExcelPackage(new FileInfo(workbookName));
        }
        /// <summary>
        /// Replace the value in cell of ExcelPackage
        /// </summary>
        /// <param name="excelPackage"></param>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public static bool Replace(ref ExcelPackage excelPackage, string sheetName, string oldValue, string newValue)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var _cell = FindCells(excelPackage, sheetName, oldValue))
            {
                if (_cell == null) return false;
                else
                {
                    excelPackage.Workbook.Worksheets[sheetName].Cells[_cell.Address].Value = newValue;
                }
            }
            return true;
        }

        public static bool SetValue(ref ExcelPackage excelPackage, string sheetName, int row, int col, string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                excelPackage.Workbook.Worksheets[sheetName].Cells[row, col].ClearFormulaValues();
                return true;
            }
            var cell = excelPackage.Workbook.Worksheets[sheetName].Cells[row, col];
            var existingStyle = cell.Style.Numberformat.Format; // Clone existing style
                                                                // Set the value while preserving style
            cell.Value = newValue;
            // Restore the original style
            cell.Style.Numberformat.Format = existingStyle;
            return true;
        }
        public static bool SetFormat(ref ExcelPackage excelPackage, string sheetName, int row, int col)
        {
            excelPackage.Workbook.Worksheets[sheetName].Cells[row, col].Style.Numberformat.Format = "_(* #,##0_);_(* (#,##0);_(* \"-\"_);_(@_)";
            //excelPackage.Workbook.Worksheets[sheetName].SelectedRange[row, col].se);
            return true;
        }
        public static bool SetValue(ref ExcelPackage excelPackage, string sheetName, string cell, string newValue)
        {
            excelPackage.Workbook.Worksheets[sheetName].SetValue(cell, newValue);
            return true;
        }
        public static bool SetFormula(ExcelPackage excelPackage, string sheetName, string cell, string fomula)
        {
            excelPackage.Workbook.Worksheets[sheetName].Cells[cell].Formula = fomula;
            return true;
        }
        public static bool SetFormula(ExcelPackage excelPackage, string sheetName, int row, int col, string fomula)
        {
            excelPackage.Workbook.Worksheets[sheetName].Cells[row, col].Formula = fomula;
            return true;
        }
        public static bool SetFormulaRange(ref ExcelPackage excelPackage, string sheetName, int row, int col, int toRow, int toCol, string fomula)
        {
            excelPackage.Workbook.Worksheets[sheetName].Cells[row, col, toRow, toCol].Formula = fomula;
            return true;
        }
        public static string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }
        public static string GetRefExcelColumnName(int columnNumber, int row)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return $"${columnName}${row}";
        }
        public static string GetExcelRangeFromIndex(int fromColumn, int fromRow, int toColumn, int toRow, bool isNeedStaticReference = false)
        {
            var fromColRange = (isNeedStaticReference ? "$" : "") + GetExcelColumnName(fromColumn) + (isNeedStaticReference ? "$" : "") + fromRow.ToString();
            var toColRange = (isNeedStaticReference ? "$" : "") + GetExcelColumnName(toColumn) + (isNeedStaticReference ? "$" : "") + toRow.ToString();
            return $"{fromColRange}:{toColRange}";
        }
        public static object GetValue(ref ExcelPackage excelPackage, string sheetName, int row, int col)
        {
            return excelPackage.Workbook.Worksheets[sheetName].Cells[row, col].Value;
        }
        public static object GetValue(ExcelPackage excelPackage, string sheetName, int row, int col)
        {
            return excelPackage.Workbook.Worksheets[sheetName].Cells[row, col].Value;
        }
        public static object GetValue(ExcelPackage excelPackage, string sheetName, int row, string col)
        {
            var colIndex = CellsHelper.ColumnNameToIndex(col);
            return excelPackage.Workbook.Worksheets[sheetName].Cells[row, colIndex + 1].Value;
        }
        public static bool CopyFormula(ref ExcelPackage excelPackage, string sheetName, int row, int col, int desRow, int desCol)
        {
            excelPackage.Workbook.Worksheets[sheetName].Cells[row, col].Copy(excelPackage.Workbook.Worksheets[sheetName].Cells[desRow, desCol], ExcelRangeCopyOptionFlags.ExcludeDataValidations);
            excelPackage.Workbook.Worksheets[sheetName].Cells[desRow, desCol].Calculate();
            return true;
        }

        public static void CopyStyles(ref ExcelPackage excelPackage, string sheetName, int row, int col, int desRow, int desCol)
        {
            var sourceRange = excelPackage.Workbook.Worksheets[sheetName].Cells[row, col];
            sourceRange.Copy(excelPackage.Workbook.Worksheets[sheetName].Cells[desRow, desCol]);
        }
        public static void CopyStyles(ref ExcelPackage excelPackage, string sheetName, int startRow, int startCol, int endRow, int endCol, int desStartRow, int desStartCol, int desEndRow, int desEndCol, bool isClearFormula = true)
        {
            var sourceRange = excelPackage.Workbook.Worksheets[sheetName].Cells[startRow, startCol, endRow, endCol];
            sourceRange.Copy(excelPackage.Workbook.Worksheets[sheetName].Cells[desStartRow, desStartCol, desEndRow, desEndCol]);
            if (isClearFormula)
            {
                var desRange = excelPackage.Workbook.Worksheets[sheetName].Cells[desStartRow, desStartCol, desEndRow, desEndCol];
                desRange.ClearFormulaValues();
            }
        }
        public static bool SetValueInt(ref ExcelPackage excelPackage, string sheetName, string cell, int? newValue)
        {
            excelPackage.Workbook.Worksheets[sheetName].SetValue(cell, newValue);
            return true;
        }
        public static bool SetCellBackColor(ref ExcelPackage excelPackage, string sheetName, string byValue, Color color)
        {
            foreach (ExcelRangeBase cell in excelPackage.Workbook.Worksheets[$"{sheetName}"].Cells)
            {
                if (cell.Value != null && cell.Value.ToString() == $"{byValue}")
                {
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(color);
                }
            }
            return true;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/13669733/export-datatable-to-excel-with-epplus
        /// </summary>
        /// <param name="excelPackage"></param>
        /// <param name="data"></param>
        public static void InsertTableSource<T>(ref ExcelPackage excelPackage, string sheetname, List<T> data)
        {
            if (data != null)
            {
                string address = excelPackage.Workbook.Worksheets["Report"].Cells.Address;
                //MessageBox.Show($"{"address"} {address}");
                excelPackage.Workbook.Worksheets["Report"].Cells[FindCells(excelPackage, sheetname, "#table")?.Address].LoadFromCollection(data, true, OfficeOpenXml.Table.TableStyles.Dark1);
            }
            else
            {
                Replace(ref excelPackage, sheetname, "#table", "No data");
            }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/13669733/export-datatable-to-excel-with-epplus
        /// </summary>
        /// <param name="excelPackage"></param>
        /// <param name="data"></param>
        public static void InsertTableSource<T>(ref ExcelPackage excelPackage, string sheetname, List<T> data, OfficeOpenXml.Table.TableStyles tableStyles)
        {
            if (data != null)
            {
                string address;
                if (excelPackage.Workbook.Worksheets[sheetname] != null)
                    address = excelPackage.Workbook.Worksheets[sheetname].Cells.Address;
                else
                {
                    throw new Exception($"Template dont have work sheet {sheetname}");
                }
                //MessageBox.Show($"{"address"} {address}");
                excelPackage.Workbook.Worksheets[sheetname].Cells[FindCells(excelPackage, sheetname, "#table").Address].LoadFromCollection(data, true, tableStyles);
            }
            else
            {
                Replace(ref excelPackage, sheetname, "#table", "No data");
            }
        }
        public static void InsertTableSource<T>(ref ExcelPackage excelPackage, string sheetname, DataTable data, OfficeOpenXml.Table.TableStyles tableStyles)
        {
            if (data != null)
            {
                string address;
                if (excelPackage.Workbook.Worksheets[sheetname] != null)
                    address = excelPackage.Workbook.Worksheets[sheetname].Cells.Address;
                else
                {
                    throw new Exception($"Template dont have work sheet {sheetname}");
                }
                //MessageBox.Show($"{"address"} {address}");
                excelPackage.Workbook.Worksheets[sheetname].Cells[FindCells(excelPackage, sheetname, "#table").Address].LoadFromDataTable(data, true, tableStyles);
            }
            else
            {
                Replace(ref excelPackage, sheetname, "#table", "No data");
            }
        }

        public static void MergeCells(ref ExcelPackage excelPackage, string sheetName, string startCellAddress, string endCellAddress, object value)
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[sheetName];
            ExcelRangeBase startCell = worksheet.Cells[startCellAddress];
            ExcelRangeBase endCell = worksheet.Cells[endCellAddress];

            ExcelRange mergedCells = worksheet.Cells[startCell.Address + ":" + endCell.Address];
            mergedCells.Merge = true;
            // Set the value for the first cell in the merged range
            if (value != null)
            {
                worksheet.Cells[startCell.Address].Value = value;
            }
            else
            {
                worksheet.Cells[startCell.Address].Value = "";
            }


        }
        //save object ExcelPackage template to binary file
        //public static void SaveReportFormat(ExcelPackage formatTemp)
        //{
        //  // Create a hashtable of values that will eventually be serialized.
        //  // To serialize the ExcelPackage,
        //  // you must first open a stream for writing.
        //  // In this case, use a file stream.
        //  FileStream fs = new FileStream("ReportFormat_v2.dat", FileMode.Create);
        //  // Construct a BinaryFormatter and use it to serialize the data to the stream.
        //  BinaryFormatter formatter = new BinaryFormatter();
        //  try
        //  {
        //    var stream = new MemoryStream(formatTemp.GetAsByteArray());
        //    formatter.Serialize(fs, stream);
        //  }
        //  catch (SerializationException ex)
        //  {
        //    var msg = $"Source : {ex.Source}, Message : {ex.Message}, HelpLink : {ex.HelpLink}";
        //    //  DataProvider.Ins.Add<AppLog>(new List<AppLog>() { new AppLog()
        //    // {
        //    //   Content= $"Source : {ex.Source}, Message : {ex.Message}, HelpLink : {ex.HelpLink}",
        //    //   Inserted=DateTime.Now
        //    //}
        //    // });
        //    throw;
        //  }
        //  finally
        //  {
        //    fs.Close();
        //  }
        //}

        /// <summary>
        /// If using DotNet5.0, user must:
        /// In Visual Studio:

        //Open Package Manager Console and type in Install-Package System.Text.Encoding.CodePages -Version 4.4.0. Change the version number appropriately.

        //Add this line to your code: Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // the necessary directive if required.
        /// </summary>
        /// <returns></returns>
        public static ExcelPackage LoadReportFormat()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FileInfo template = new FileInfo(_templatePath);
            ExcelPackage result = new ExcelPackage(template);

            try
            {
                // Declare the hashtable reference.

                // Open the file containing the data that you want to deserialize.
                //fs = new FileStream(_templatePath, FileMode.Open);
                //result.Settings.
                //BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and
                // assign the reference to the local variable.
                //result = (ExcelPackage)formatter.Deserialize(fs);
                //result.Load((Stream)formatter.Deserialize(fs));
            }
            catch (SerializationException ex)
            {
                var msg = $"Source : {ex.Source}, Message : {ex.Message}, HelpLink : {ex.HelpLink}";
                //  DataProvider.Ins.Add<AppLog>(new List<AppLog>() { new AppLog()
                // {
                //   Content= $"Source : {ex.Source}, Message : {ex.Message}, HelpLink : {ex.HelpLink}",
                //   Inserted=DateTime.Now
                //}
                // });
                throw ex;
            }
            finally
            {
                //fs?.Close();
            }
            return result;
        }

        public static int GetColumnNumber(string name)
        {
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }

            return number;
        }
        
    }
}
