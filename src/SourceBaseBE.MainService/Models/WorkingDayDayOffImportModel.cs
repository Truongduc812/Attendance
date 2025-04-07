using iSoft.Common.Enums;
using SourceBaseBE.Database.Entities;
using System;

namespace SourceBaseBE.Database.Models.RequestModels
{
    public class WorkingDayDayOffImportModel
    {
        public DateTime? DateDayOff { get; set; }
        public string? DayOfWeek { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? TimeOffTable { get; set; }
        public string? LeaveType { get; set; }
        public double? Unit { get; set; }
        public string? TimeUnit { get; set; } 
    }
}
