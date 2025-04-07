using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
	public class WorkingDayDeleteRequestModel
    {
        public long Id { get; set; } 
        public long? EditerId { get; set; } 
        public EnumActionRequest ActionRequest { get; set; }
        public WorkingDayUpdateEntity GetEntity(WorkingDayEntity workingDayEntity)
        {
            var ret = new WorkingDayUpdateEntity();
            ret.WorkingDayId = workingDayEntity?.Id;
            ret.EditerId = EditerId;
            ret.WorkingDate = workingDayEntity?.WorkingDate;
            ret.Time_In = workingDayEntity?.Time_In;
            ret.Time_Out = workingDayEntity?.Time_Out;
            ret.WorkingDayStatus = workingDayEntity?.WorkingDayStatus;
            ret.WorkingTypeId = workingDayEntity?.WorkingTypeEntityId;
            ret.Update_Reason = EnumActionRequest.Delete.ToString();
            ret.Notes = workingDayEntity?.Notes;
            ret.ActionRequest = this.ActionRequest;
            double? timeDeviation = workingDayEntity?.TimeDeviation;
            ret.Time_Deviation = (long?) timeDeviation ?? null;  
            ret.EmployeeId = workingDayEntity?.EmployeeEntityId;
            //ret.Id = entity.WorkingDayUpdateId.GetValueOrDefault();
            ret.OriginalWorkDate = workingDayEntity?.WorkingDate;
            ret.OriginTimeDeviation = workingDayEntity?.TimeDeviation;
            ret.OriginTimeIn = workingDayEntity?.Time_In;
            ret.OriginTimeOut = workingDayEntity?.Time_Out;
            ret.OriginWorkingTypeId = workingDayEntity?.WorkingTypeEntityId;
            ret.OriginWorkingDayStatus = workingDayEntity?.WorkingDayStatus;
            return ret;
        }

    }
}
