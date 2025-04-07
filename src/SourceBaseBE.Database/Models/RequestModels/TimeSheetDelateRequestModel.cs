using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
    public class TimeSheetDelateRequestModel : BaseCRUDRequestModel<TimeSheetEntity>
    {
        public long Id { get; set; }
        public long EditerId { get; set; }
        public long? WorkingDayId { get; set; }
        public EnumActionRequest? ActionRequest { get; set; }

        public TimeSheetEntity GetEntity()
        {
            var ret = new TimeSheetEntity();
            if (this.Id != null) ret.Id = (long)this.Id;
            if (this.WorkingDayId != null) ret.WorkingDayId = this.WorkingDayId;
            if (Id <= 0)
            {
                ret.CreatedAt = DateTime.Now;
                ret.UpdatedAt = DateTime.Now;
                ret.CreatedBy = this.EditerId;
            }
            return ret;
        }
        public TimeSheetUpdateEntity GetTimeSheetUpdateEntity(TimeSheetEntity origin, UserEntity edited)
        {
            var ret = new TimeSheetUpdateEntity();
            if (this.Id != null) ret.TimeSheetEntityId = origin?.Id;
            ret.RecordedTime = origin?.RecordedTime;
            ret.Status = origin?.Status;
            ret.EmployeeId = origin?.EmployeeId;
            if (this.ActionRequest != null) ret.ActionRequest = this.ActionRequest;
            ret.UserEntityId = edited?.Id;
            if (origin != null)
            {
                ret.OriginStatus = origin.Status;
                ret.OriginRecordedTime = origin.RecordedTime;
            }
            ret.CreatedAt = DateTime.Now;
            ret.UpdatedAt = DateTime.Now;
            ret.CreatedBy = this.EditerId;
            ret.TimeSheetApprovalEntities = new List<TimeSheetApprovalEntity>();
            ret.TimeSheetApprovalEntities.Add(new TimeSheetApprovalEntity()
            {
                Status = Enums.EnumApproveStatus.PENDING
            });
            return ret;
        }
    }

}
