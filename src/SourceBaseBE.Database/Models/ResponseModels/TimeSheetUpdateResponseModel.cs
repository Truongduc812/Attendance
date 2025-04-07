using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class TimeSheetUpdateResponseModel : BaseCRUDResponseModel<TimeSheetUpdateEntity>
  {
    public EnumFaceId? Status { get; set; }
    public DateTime? RecordedTime { get; set; }
    public EnumFaceId? OriginStatus { get; set; }
    public DateTime? OriginRecordedTime { get; set; }
    public string? UpdateReason { get; set; }
    public string? Notes { get; set; }
    public EnumActionRequest? ActionRequest { get; set; }
    public long? TimeSheetEntityId { get; set; }
    public TimeSheetEntity? TimeSheetEntity { get; set; }
    public long? EmployeeId { get; set; }
    public EmployeeEntity? Employee { get; set; }
    public long? UserEntityId { get; set; }
    public UserEntity? UserEntity { get; set; }
    public ICollection<TimeSheetApprovalEntity> TimeSheetApprovalEntities { get; set; }
/*[GEN-20]*/
    public override object SetData(TimeSheetUpdateEntity entity)
    {
      base.SetData(entity);
      this.Status = entity.Status;
      this.RecordedTime = entity.RecordedTime;
      this.OriginStatus = entity.OriginStatus;
      this.OriginRecordedTime = entity.OriginRecordedTime;
      this.UpdateReason = entity.UpdateReason;
      this.Notes = entity.Notes;
      this.ActionRequest = entity.ActionRequest;
      this.TimeSheetEntityId = entity.TimeSheetEntityId;
      this.TimeSheetEntity = entity.TimeSheetEntity;
      this.EmployeeId = entity.EmployeeId;
      this.Employee = entity.Employee;
      this.UserEntityId = entity.UserEntityId;
      this.UserEntity = entity.UserEntity;
      this.TimeSheetApprovalEntities = entity.TimeSheetApprovalEntities;
/*[GEN-21]*/
      return this;
    }
    public override List<object> SetData(List<TimeSheetUpdateEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (TimeSheetUpdateEntity entity in listEntity)
      {
        listRS.Add(new TimeSheetUpdateResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
