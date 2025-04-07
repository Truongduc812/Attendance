using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class TimeSheetApprovalResponseModel : BaseCRUDResponseModel<TimeSheetApprovalEntity>
  {
    public EnumApproveStatus? Status { get; set; }
    public string? Notes { get; set; }
    public long? UserEntityId { get; set; }
    public UserEntity? UserEntity { get; set; }
    public long? TimeSheetUpdateId { get; set; }
    public TimeSheetUpdateEntity? TimeSheetUpdate { get; set; }
/*[GEN-20]*/
    public override object SetData(TimeSheetApprovalEntity entity)
    {
      base.SetData(entity);
      this.Status = entity.Status;
      this.Notes = entity.Notes;
      this.UserEntityId = entity.UserEntityId;
      this.UserEntity = entity.UserEntity;
      this.TimeSheetUpdateId = entity.TimeSheetUpdateId;
      this.TimeSheetUpdate = entity.TimeSheetUpdate;
/*[GEN-21]*/
      return this;
    }
    public override List<object> SetData(List<TimeSheetApprovalEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (TimeSheetApprovalEntity entity in listEntity)
      {
        listRS.Add(new TimeSheetApprovalResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
