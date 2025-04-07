using iSoft.Common.Models.ResponseModels;
using Newtonsoft.Json;
using SourceBaseBE.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class HolidaySchedulePagingResponseModel : PagingWithColumnsResponseModel
	{
		[JsonProperty("listData", NullValueHandling = NullValueHandling.Ignore)]
		public new List<HolidayScheduleListResponseModel> ListData { get; set; }
	}
}
