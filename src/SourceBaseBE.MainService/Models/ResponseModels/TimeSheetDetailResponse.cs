using SourceBaseBE.Database.Models.ResponseModels;
using System.Collections.Generic;

namespace SourceBaseBE.MainService.Models.ResponseModels
{
    public class TimeSheetDetailResponse
    {
        public List<Dictionary<string, object>> EmployeeInformation { get; set; }
        public List<Dictionary<string, object>> SummarizeData { get; set; }
        public DetailTimeSheetPagingResponseModel AttendanceRecord { get; set; }
    }
}
