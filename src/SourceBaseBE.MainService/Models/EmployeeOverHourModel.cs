using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using System;

namespace SourceBaseBE.Database.Models.RequestModels
{
    public class EmployeeOverHourModel
    {
        public string? Name { get; set; }
        public string? EmployeeCode { get; set; }
        public int? OTOVer40 { get; set; }
        public string? DepartmentStr { get; set; }
        public string? JobTitleStr { get; set; }
        public string? Note { get; set; }


        public EmployeeOverHourModel SetModel(EmployeeEntity entity, string note, int over)
        {
            this.Name = entity.Name;
            this.DepartmentStr = entity.Department?.Name;
            this.JobTitleStr = entity.JobTitle?.Name;
            this.EmployeeCode = entity.EmployeeCode;
            this.Note = note;
            this.OTOVer40 = over;
            return this;
        }
    }
}
