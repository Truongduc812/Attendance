using iSoft.Common.Enums;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Models.RequestModels
{
    public class WorkingDayUpdateRequestModel : BaseCRUDRequestModel<WorkingDayUpdateEntity>
    {

        public long? WorkingDayId { get; set; }
        public long? WorkingDayUpdateId { get; set; }
        public long? EmployeeId { get; set; }
        public long? EditerId { get; set; }
        public DateTime WorkingDate { get; set; }
        public string? Time_In { get; set; }
        public string? Time_Out { get; set; }
        public string? TimeDeviation { get; set; }
        [Column("workingdaystatus")]
        public string? WorkingDayStatus { get; set; }
        [Column("working_type_id")]
        public long? WorkingTypeId { get; set; }
        public string? Update_Reason { get; set; }
        public string? Notes { get; set; }
        public EnumActionRequest? ActionRequest { get; set; }
        public WorkingDayUpdateEntity GetEntity(WorkingDayEntity workingDayEntity, WorkingDayUpdateRequestModel entity)
        {
            var ret = new WorkingDayUpdateEntity();
            ret.WorkingDayId = workingDayEntity?.Id;
            ret.EditerId = entity.EditerId;
            if (this.WorkingDate != null) ret.WorkingDate = this.WorkingDate;
            if (!string.IsNullOrEmpty(this.Time_In)) ret.Time_In = DateTime.Parse(this.Time_In);
            if (!string.IsNullOrEmpty(this.Time_Out)) ret.Time_Out = DateTime.Parse(this.Time_Out);
            if (!string.IsNullOrEmpty(this.TimeDeviation)) ret.Time_Deviation = long.Parse(this.TimeDeviation);
            if (this.WorkingDayStatus != null) ret.WorkingDayStatus = Enum.Parse<EnumWorkingDayStatus>(this.WorkingDayStatus);
            if (this.WorkingTypeId != null) ret.WorkingTypeId = this.WorkingTypeId;
            if (this.Update_Reason != null) ret.Update_Reason = this.Update_Reason;
            if (this.Notes != null) ret.Notes = this.Notes;
            if(this.ActionRequest != null) ret.ActionRequest = this.ActionRequest;
            ret.Time_Deviation = !string.IsNullOrWhiteSpace(entity.TimeDeviation) ? long.Parse(entity.TimeDeviation) : null;
            ret.EmployeeId = this.EmployeeId;
            ret.Id = entity.WorkingDayUpdateId.GetValueOrDefault();
            ret.OriginalWorkDate = workingDayEntity?.WorkingDate;
            ret.OriginTimeDeviation = workingDayEntity?.TimeDeviation;
            ret.OriginTimeIn = workingDayEntity?.Time_In;
            ret.OriginTimeOut = workingDayEntity?.Time_Out;
            ret.OriginWorkingTypeId = workingDayEntity?.WorkingTypeEntityId;
            ret.OriginWorkingDayStatus = workingDayEntity?.WorkingDayStatus;
            return ret;
        }

        public override Dictionary<string, (string, IFormFile)> GetFiles()
        {
            Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();

            /*[GEN-17]*/
            return dicRS;
        }
    }
}
