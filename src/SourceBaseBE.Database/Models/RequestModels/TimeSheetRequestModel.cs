using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
    public class TimeSheetRequestModel : BaseCRUDRequestModel<TimeSheetEntity>
    {
        public long Id { get; set; }
        public long EditerId { get; set; }
        public DateTime RecordedTime { get; set; }
        public EnumFaceId Status { get; set; }
        public long EmployeeId { get; set; }
        public long? WorkingDayId { get; set; }
        public EnumActionRequest? ActionRequest { get; set; }

        public TimeSheetEntity GetEntity()
        {
            var ret = new TimeSheetEntity();
            if (this.Id != null) ret.Id = (long)this.Id;
            if (this.Status != null) ret.Status = this.Status;
            if (this.RecordedTime != null) ret.RecordedTime = this.RecordedTime;
            if (this.EmployeeId != null) ret.EmployeeId = this.EmployeeId;
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
            if (this.Status != null) ret.Status = this.Status;
            if (this.RecordedTime != null) ret.RecordedTime = this.RecordedTime;
            if (this.EmployeeId != null) ret.EmployeeId = this.EmployeeId;
            if (this.Id != null) ret.TimeSheetEntityId = origin?.Id;
            if(this.ActionRequest != null) ret.ActionRequest = this.ActionRequest;
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
                CreatedAt = DateTime.Now,
                Status = Enums.EnumApproveStatus.PENDING
            });
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
