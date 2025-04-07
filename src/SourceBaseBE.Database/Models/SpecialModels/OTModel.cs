using SourceBaseBE.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.SpecialModels
{
    public class OTModel
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string DepartmentStr { get; set; }
        public string JobTitleStr { get; set; }
        public string EmpCode { get; set; }
        public int OT150Hours { get; set; }
        public int OT200Hours { get; set; }
        public int OT270NightHours { get; set; }
        public int OT300Hours { get; set; }
        public int OT390Hours { get; set; }
        public int Total { get; set; }
        public List<WorkingDayEntity> AfterProcessWds { get; set; }
    }
}
