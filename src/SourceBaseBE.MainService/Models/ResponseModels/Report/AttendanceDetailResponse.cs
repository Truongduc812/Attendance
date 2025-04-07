using iSoft.Common.Models.ResponseModels;
using iSoft.Database.Extensions;
using SourceBaseBE.Database.Models.ResponseModels;
using System.Collections.Generic;

namespace SourceBaseBE.MainService.Models.ResponseModels.Report
{
    public class AttendanceDetailResponse
    {
        public List<Dictionary<string, object>> EmployeeInformation { get; set; }
        public List<Dictionary<string, object>> SummarizeData { get; set; }
        public DetailAttendancePagingResponseModel AttendanceRecord { get; set; }
    }
    public class AttendanceOTsResponse
    {
        public OTListPagingResponseModel AttendanceRecord { get; set; }
    }

    public class AttendanceEditDetailResponse
    {
        public List<Dictionary<string, object>> EmployeeInformation { get; set; }
        public List<Dictionary<string, object>> SummarizeData { get; set; }
        public DetailAttendanceEditPagingResponseModel AttendanceRecord { get; set; }
    }
}
