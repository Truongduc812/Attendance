using iSoft.ExportLibrary.Services;

namespace iSoft.ExportLibrary.Models
{
  public class ExportSheetDataModel
  {
    public int SheetIndex { get; set; }

    public Dictionary<string, object> DicCellName2Value = new Dictionary<string, object>();
    public List<Dictionary<object, object>> ListChartDataModel = new List<Dictionary<object, object>>();
    public Dictionary<int, string> DicColumnIndex2EnvKey = new Dictionary<int, string>();
    public int BeginRowIndex { get; set; }
    public int BeginNoNumber { get; set; }
    public List<ExcelRangeStyle> ListExcelRangeStyle = new List<ExcelRangeStyle>();
    public Dictionary<string, ExcelRangeStyle> DicEnvKey2Style = new Dictionary<string, ExcelRangeStyle>();
    public void ClearData()
    {
      bool gcFlag = false;
      if (this.ListChartDataModel.Count >= 1000)
      {
        gcFlag = true;
      }
      this.SheetIndex = 0;
      this.DicCellName2Value.Clear();
      this.ListChartDataModel.Clear();
      this.DicColumnIndex2EnvKey.Clear();
      this.BeginRowIndex = 0;
      this.ListExcelRangeStyle.Clear();
      this.DicEnvKey2Style.Clear();

      if (gcFlag)
      {
        GC.Collect();
      }
    }
  }
}
