using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class WorkingDayApprovalResponseModel : BaseCRUDResponseModel<WorkingDayApprovalEntity>
  {
    public long? WorkingDayId { get; set; }
    public WorkingDayEntity? WorkingDay { get; set; }
    public long? WorkingDayUpdateId { get; set; }
    public WorkingDayUpdateEntity? WorkingDayUpdate { get; set; }
    public EnumApproveStatus ApproveStatus { get; set; }
    public string? Approve_Reason { get; set; }
    public string? Notes { get; set; }

    public override object SetData(WorkingDayApprovalEntity entity)
    {
      base.SetData(entity);
      this.WorkingDayId = entity.WorkingDayId;
      this.WorkingDay = entity.WorkingDay;
      this.WorkingDayUpdateId = entity.WorkingDayUpdateId;
      this.WorkingDayUpdate = entity.WorkingDayUpdate;
      this.ApproveStatus = entity.ApproveStatus;
      this.Approve_Reason = entity.Approve_Reason;
      this.Notes = entity.Notes;

      return this;
    }
    public override List<object> SetData(List<WorkingDayApprovalEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (WorkingDayApprovalEntity entity in listEntity)
      {
        listRS.Add(new WorkingDayApprovalResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
