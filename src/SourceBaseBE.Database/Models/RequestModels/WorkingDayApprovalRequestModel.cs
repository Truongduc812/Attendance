using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class WorkingDayApprovalRequestModel : BaseCRUDRequestModel<WorkingDayApprovalEntity>
  {
    public long? WorkingDayId { get; set; }
    public WorkingDayEntity? WorkingDay { get; set; }
    public long? WorkingDayUpdateId { get; set; }
    public WorkingDayUpdateEntity? WorkingDayUpdate { get; set; }
    public EnumApproveStatus ApproveStatus { get; set; }
    public string? Approve_Reason { get; set; }
    public string? Notes { get; set; }

    public override WorkingDayApprovalEntity GetEntity(WorkingDayApprovalEntity entity)
    {
      if (this.Id != null) entity.Id = (long)this.Id;
      if (this.Order != null) entity.Order = this.Order;
      if (this.WorkingDayId != null) entity.WorkingDayId = this.WorkingDayId;
      if (this.WorkingDay != null) entity.WorkingDay = this.WorkingDay;
      if (this.WorkingDayUpdateId != null) entity.WorkingDayUpdateId = this.WorkingDayUpdateId;
      if (this.WorkingDayUpdate != null) entity.WorkingDayUpdate = this.WorkingDayUpdate;
      if (this.ApproveStatus != null) entity.ApproveStatus = this.ApproveStatus;
      if (this.Approve_Reason != null) entity.Approve_Reason = this.Approve_Reason;
      if (this.Notes != null) entity.Notes = this.Notes;

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
