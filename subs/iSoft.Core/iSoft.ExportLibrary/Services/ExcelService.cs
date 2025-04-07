using iSoft.Common;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Microsoft.Extensions.Logging;
using System.Drawing;
using iSoft.ExportLibrary.Models;
using Aspose.Cells;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace iSoft.ExportLibrary.Services
{
    public class ExportExcelService
    {
        public static int SetFormatExcelHistory(ExcelWorksheets worksheets, ExportSheetDataModel model, object defaultVal = null)
        {
            int insertedRowCount = 0;
            var worksheet = worksheets[model.SheetIndex];
            int end_row = 1;
            if (worksheet != null)
            {
                int row = model.BeginRowIndex;

                foreach (var keyVal in model.DicCellName2Value)
                {
                    ExcelRange range = worksheet.Cells[keyVal.Key];
                    if (keyVal.Value != null && keyVal.Value.GetType() == typeof(DateTime))
                    {
                        range.Value = keyVal.Value;
                        //range.Value = ((DateTime)(keyVal.Value)).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        range.Value = keyVal.Value;
                    }
                }


                int columnBeginIndex = int.MaxValue;
                int columnEndIndex = 0;
                for (int i = 0; i < model.ListChartDataModel.Count; i++)
                {
                    var dataLog = model.ListChartDataModel[i];
                    foreach (var keyVal in model.DicColumnIndex2EnvKey)
                    {
                        if (dataLog.ContainsKey(keyVal.Value) || keyVal.Value == "[No.]")
                        {
                            if (columnBeginIndex > keyVal.Key + 1)
                            {
                                columnBeginIndex = keyVal.Key + 1;
                            }
                            if (columnEndIndex < keyVal.Key + 1)
                            {
                                columnEndIndex = keyVal.Key + 1;
                            }
                        }
                    }
                }
                if (model.ListChartDataModel.Count >= 1 && columnEndIndex >= columnBeginIndex)
                {

                    //var maxKeyValuePair = model.DicEnvKey2ColumnIndex.OrderByDescending(kv => kv.Value).First();
                    object[,] values = new object[model.ListChartDataModel.Count, columnEndIndex - columnBeginIndex + 1];
                    if (defaultVal != null)
                    {
                        for (int i2 = 0; i2 < model.ListChartDataModel.Count; i2++)
                        {
                            for (int j2 = 0; j2 < columnEndIndex - columnBeginIndex + 1; j2++)
                            {
                                values[i2, j2] = defaultVal;
                            }
                        }
                    }

                    int noIndex = -1;
                    foreach (var keyVal in model.DicColumnIndex2EnvKey)
                    {
                        if (keyVal.Value == "[No.]")
                        {
                            noIndex = keyVal.Key;
                        }
                    }

                    int j = 0;
                    for (int i = 0; i < model.ListChartDataModel.Count; i++)
                    {
                        var dataLog = model.ListChartDataModel[i];
                        try
                        {
                            //values[i, 0] = i + 1;
                            //values[i, 1] = dataLog.ExecuteAtData.ToString("yyyy-MM-dd HH:mm:ss");

                            if (noIndex != -1)
                            {
                                values[i, noIndex - columnBeginIndex + 1] = i + model.BeginNoNumber;
                            }
                            foreach (var keyVal in model.DicColumnIndex2EnvKey)
                            {
                                if (dataLog.ContainsKey(keyVal.Value) && keyVal.Value == "Avatar")
                                {
                                    string imagePath = dataLog[keyVal.Value]?.ToString();
                                    if (string.IsNullOrEmpty(imagePath))
                                    {
                                        int cellRow = row + i;
                                        int cellColumn = keyVal.Key + 1;

                                        string pictureKey = $"Image_{keyVal.Key}_{i}";
                                        var existingPicture = worksheet.Drawings.FirstOrDefault(d => d.Name == pictureKey);
                                        if (existingPicture != null)
                                        {
                                            worksheet.Drawings.Remove(existingPicture);
                                        }
                                        worksheet.Cells[cellRow, cellColumn].Clear();
                                        continue;
                                    }

                                    try
                                    {
                                        using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(imagePath))
                                        using (MemoryStream memoryStream = new MemoryStream())
                                        {
                                            image.SaveAsPng(memoryStream);
                                            memoryStream.Seek(0, SeekOrigin.Begin);

                                            string pictureKey = $"Image_{keyVal.Key}_{i}";

                                            var existingPicture = worksheet.Drawings.FirstOrDefault(d => d.Name == pictureKey);
                                            if (existingPicture != null)
                                            {
                                                worksheet.Drawings.Remove(existingPicture);
                                            }

                                            // Add a new picture
                                            var picture = worksheet.Drawings.AddPicture(pictureKey, memoryStream);

                                            // Set default dimensions for the picture
                                            int portraitWidth = 100;
                                            int portraitHeight = 150;
                                            picture.SetSize(portraitWidth, portraitHeight);

                                            // Position the picture in the worksheet
                                            int cellRow = row + i;
                                            int cellColumn = keyVal.Key + 1;

                                            picture.SetPosition(cellRow - 1, 0, cellColumn - 1, 0);

                                            // Adjust the row height and column width
                                            worksheet.Row(cellRow).Height = portraitHeight * 0.75;
                                            double columnWidth = (portraitWidth / 7.5) + 0.71;
                                            worksheet.Column(cellColumn).Width = columnWidth;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception($"Error handling image file: {ex.Message}");
                                    }
                                }


                                if (dataLog.ContainsKey(keyVal.Value) && keyVal.Value != "Avatar")
                                {
                                    values[i, keyVal.Key - columnBeginIndex + 1] = dataLog[keyVal.Value];
                                }
                            }
                            insertedRowCount++;

                            //foreach (var keyVal in dataLog.DicValue)
                            //{
                            //  if (model.DicEnvKey2ColumnIndex.ContainsKey(keyVal.Key))
                            //  {
                            //    values[i, model.DicEnvKey2ColumnIndex[keyVal.Key] - columnBeginIndex + 1] = keyVal.Value;
                            //  }
                            //}
                        }
                        catch (Exception ex)
                        {
                            //logger.LogMsg(Messages.ErrException, ex);
                            throw ex;
                        }
                    }

                    end_row = row + model.ListChartDataModel.Count - 1;
                    if (end_row >= row)
                    {
                        ExcelRange targetRange = worksheet.Cells[row, columnBeginIndex, end_row, columnEndIndex];
                        //targetRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //targetRange.Style.Font.Color.SetColor(Color.Black);
                        //targetRange.Style.Fill.PatternType = ExcelFillStyle.None;
                        //targetRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //targetRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //targetRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //targetRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        targetRange.Value = values;
                        ExcelRangeStyle excellStyle;
                        int columnIndex2 = 1;
                        foreach (var keyVal in model.DicEnvKey2Style)
                        {
                            columnIndex2 = getColumnByEnvKey(keyVal.Key, model.DicColumnIndex2EnvKey) + 1;
                            excellStyle = keyVal.Value;
                            excellStyle.setRange(row,
                                                  columnIndex2,
                                                  end_row,
                                                  columnIndex2);
                            excellStyle.ApplyStyle(worksheet);
                        }
                    }
                }

                foreach (var excelStyle in model.ListExcelRangeStyle)
                {
                    excelStyle.ApplyStyle(worksheet);
                }
            }
            return insertedRowCount;
        }

        private static int getColumnByEnvKey(string envKey, Dictionary<int, string> dicColumnIndex2EnvKey)
        {
            foreach (var keyVal in dicColumnIndex2EnvKey)
            {
                if (keyVal.Value == envKey)
                {
                    return keyVal.Key;
                }
            }
            return -1;
        }

    }
}
