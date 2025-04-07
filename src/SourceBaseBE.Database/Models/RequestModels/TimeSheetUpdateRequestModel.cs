using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.RequestModels.Generate
{
    public class TimeSheetUpdateRequestModel : BaseCRUDRequestModel<TimeSheetUpdateEntity>
    {
        public EnumFaceId? Status { get; set; }
        public DateTime? RecordedTime { get; set; }
        public EnumFaceId? OriginStatus { get; set; }
        public DateTime? OriginRecordedTime { get; set; }
        public string? UpdateReason { get; set; }
        public string? Notes { get; set; }
        public EnumActionRequest ActionRequest { get; set; }
        public long? TimeSheetEntityId { get; set; }
        public TimeSheetEntity? TimeSheetEntity { get; set; }
        public long? EmployeeId { get; set; }
        public EmployeeEntity? Employee { get; set; }
        public long? UserEntityId { get; set; }
        public UserEntity? UserEntity { get; set; }
        public List<TimeSheetApprovalEntity> TimeSheetApprovalEntities { get; set; }
        /*[GEN-18]*/
        public override TimeSheetUpdateEntity GetEntity(TimeSheetUpdateEntity entity)
        {
            if (this.Id != null) entity.Id = (long)this.Id;
            if (this.Order != null) entity.Order = this.Order;
            if (this.Status != null) entity.Status = this.Status;
            if (this.RecordedTime != null) entity.RecordedTime = this.RecordedTime;
            if (this.OriginStatus != null) entity.OriginStatus = this.OriginStatus;
            if (this.OriginRecordedTime != null) entity.OriginRecordedTime = this.OriginRecordedTime;
            if (this.UpdateReason != null) entity.UpdateReason = this.UpdateReason;
            if (this.Notes != null) entity.Notes = this.Notes;
            if(this.ActionRequest != null) entity.ActionRequest = this.ActionRequest;   
            if (this.TimeSheetEntityId != null) entity.TimeSheetEntityId = this.TimeSheetEntityId;
            if (this.TimeSheetEntity != null) entity.TimeSheetEntity = this.TimeSheetEntity;
            if (this.EmployeeId != null) entity.EmployeeId = this.EmployeeId;
            if (this.Employee != null) entity.Employee = this.Employee;
            if (this.UserEntityId != null) entity.UserEntityId = this.UserEntityId;
            if (this.UserEntity != null) entity.UserEntity = this.UserEntity;
            if (this.TimeSheetApprovalEntities != null) entity.TimeSheetApprovalEntities = this.TimeSheetApprovalEntities;
            /*[GEN-19]*/
            return entity;
        }

        public override Dictionary<string, (string, IFormFile)> GetFiles()
        {
            Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();

            /*[GEN-17]*/
            return dicRS;
        }
    }
}
