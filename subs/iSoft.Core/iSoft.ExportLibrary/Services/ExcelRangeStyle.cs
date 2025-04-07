using iSoft.Common.Utils;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace iSoft.ExportLibrary.Services
{
  public class ExcelRangeStyle
  {
    public string CellNameFrom { get; set; }
    public string CellNameTo { get; set; }
    public Color? TextColor { get; set; } = null;
    public Color? BackgroundColor { get; set; } = null;
    public Color? BorderColor { get; set; } = null;
    public ExcelFillStyle? FillStyle { get; set; } = null;
    public ExcelBorderStyle? BorderTop { get; set; } = null;
    public ExcelBorderStyle? BorderLeft { get; set; } = null;
    public ExcelBorderStyle? BorderBottom { get; set; } = null;
    public ExcelBorderStyle? BorderRight { get; set; } = null;
    public ExcelHorizontalAlignment? TextAlign { get; set; } = null;
    public bool? IsMerged { get; set; } = null;
    public bool? isWrapText { get; set; } = null;
    public void ApplyStyle(ExcelWorksheet worksheet)
    {
      ExcelRange targetRange = worksheet.Cells[ConvertUtil.GetRowIndex(CellNameFrom),
                                              ConvertUtil.GetColumnIndex(CellNameFrom),
                                              ConvertUtil.GetRowIndex(CellNameTo),
                                              ConvertUtil.GetColumnIndex(CellNameTo)];
      if (this.TextAlign != null) targetRange.Style.HorizontalAlignment = (ExcelHorizontalAlignment)this.TextAlign;
      if (this.TextColor != null) targetRange.Style.Font.Color.SetColor((Color)this.TextColor);
      if (this.FillStyle != null) targetRange.Style.Fill.PatternType = (ExcelFillStyle)this.FillStyle;
      if (this.BackgroundColor != null) targetRange.Style.Fill.SetBackground((Color)this.BackgroundColor);
      if (this.BorderColor != null) targetRange.Style.Border.Top.Color.SetColor((Color)this.BorderColor);
      if (this.BorderColor != null) targetRange.Style.Border.Right.Color.SetColor((Color)this.BorderColor);
      if (this.BorderColor != null) targetRange.Style.Border.Bottom.Color.SetColor((Color)this.BorderColor);
      if (this.BorderColor != null) targetRange.Style.Border.Left.Color.SetColor((Color)this.BorderColor);
      if (this.BorderTop != null) targetRange.Style.Border.Top.Style = (ExcelBorderStyle)this.BorderTop;
      if (this.BorderLeft != null) targetRange.Style.Border.Left.Style = (ExcelBorderStyle)this.BorderLeft;
      if (this.BorderRight != null) targetRange.Style.Border.Right.Style = (ExcelBorderStyle)this.BorderRight;
      if (this.BorderBottom != null) targetRange.Style.Border.Bottom.Style = (ExcelBorderStyle)this.BorderBottom;
      if (this.IsMerged != null) targetRange.Merge = (bool)this.IsMerged;
      if (this.isWrapText != null) targetRange.Style.WrapText = (bool)this.isWrapText;
    }

    public void SetBorderAll()
    {
      this.FillStyle = ExcelFillStyle.Solid;
      this.BorderTop = ExcelBorderStyle.Thin;
      this.BorderLeft = ExcelBorderStyle.Thin;
      this.BorderBottom = ExcelBorderStyle.Thin;
      this.BorderRight = ExcelBorderStyle.Thin;
    }
    public ExcelRangeStyle Clone()
    {
      return new ExcelRangeStyle
      {
        CellNameFrom = this.CellNameFrom,
        CellNameTo = this.CellNameTo,
        TextColor = this.TextColor,
        BackgroundColor = this.BackgroundColor,
        FillStyle = this.FillStyle,
        BorderTop = this.BorderTop,
        BorderLeft = this.BorderLeft,
        BorderBottom = this.BorderBottom,
        BorderRight = this.BorderRight,
        TextAlign = this.TextAlign,
        IsMerged = this.IsMerged,
        isWrapText = this.isWrapText
      };
    }

    public void setRange(string cellNameFrom, string cellNameTo)
    {
      this.CellNameFrom = cellNameFrom;
      this.CellNameTo = cellNameTo;
    }

    public void setRange(int row1, int col1, int row2, int col2)
    {
      this.CellNameFrom = ConvertUtil.GetCellNameByIndex(row1, col1);
      this.CellNameTo = ConvertUtil.GetCellNameByIndex(row2, col2);
    }
  }
}
