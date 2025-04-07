using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using iSoft.Common.Models.ResponseModels;
using Newtonsoft.Json;
using System.ComponentModel;

namespace SourceBaseBE.Database.Models.ResponseModels
{

	public class TimeSheetPagingResponse : PagingWithColumnsResponseModel
	{
		[JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
		public new List<TimesheetListResponseModel> ListData { get; set; }
	}
    public class DetailTimeSheetPagingResponseModel : PagingWithColumnsResponseModel
    {
        [JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
        public new List<TimesheetEditListResponseModel> ListData { get; set; }
        [JsonProperty("listIncommingData", NullValueHandling = NullValueHandling.Ignore)]
        public new List<TimesheetEditListResponseModel> ListIncomingData { get; set; }
        [JsonIgnore]
        [Browsable(false)]
        public List<TimeSheetEntity> rawDatas { get; set; }
    }
}
