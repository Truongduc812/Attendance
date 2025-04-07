using iSoft.Common.Utils;
using Newtonsoft.Json;

namespace iSoft.ElasticSearch.Models
{
	public class ChartConnectionDataModel
  {
    public long ConnectionId { get; set; }
    public List<ChartDataModel> ListData { get; set; }
    public long? TotalRecord {  get; set; }

    public ChartConnectionDataModel(long connectionId, List<ChartDataModel> listData, long? totalRecord)
    {
      ConnectionId = connectionId;
      ListData = listData;
      TotalRecord = totalRecord;
    }

    public override string ToString()
    {
      return $"{ConnectionId}: ({ListData.Count} / {TotalRecord})";
    }
  }
}
