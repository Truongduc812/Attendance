using iSoft.Common.Utils;
using NPOI.Util;
using SourceBaseBE.Database.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SourceBaseBE.Database.Models
{
    public class TimeSheetExcel
    {
        public string MNV { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Date { get; set; }
        public string DOW { get; set; }
        public string RecordedTime { get; set; }
        public string Type { get; set; }

        public static List<TimeSheetExcel> SetDatas(IEnumerable<IGrouping<long?, TimeSheetEntity>> timeSheets)
        {
            var ret = new List<TimeSheetExcel>();
            foreach (var empTs in timeSheets)
            {
                var curInd = 0;
                ret.AddRange(empTs.OrderBy(x => x.Id)
                    .Select(x => new TimeSheetExcel()
                    {
                        MNV = x.Employee?.EmployeeMachineCode,
                        Date = x.RecordedTime?.ToString("dd/MM/yyyy"),
                        DOW = x.RecordedTime?.DayOfWeek.ToString(),
                        Name = x.Employee?.Name,
                        RecordedTime = x.RecordedTime?.ToString("HH:mm:ss"),
                        Department = x.Employee?.Department?.Name,
                        Type = x.Status.ToString()
                    }));
            }

            return ret;
        }
    }
}
