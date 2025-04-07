using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.RequestModels.Generate
{
  public class TimeSheetApprovalRequestModel : BaseCRUDRequestModel<TimeSheetApprovalEntity>
  {
    public EnumApproveStatus? Status { get; set; }
    public string? Notes { get; set; }
    public long? UserEntityId { get; set; }
    public UserEntity? UserEntity { get; set; }
    public long? TimeSheetUpdateId { get; set; }
    public TimeSheetUpdateEntity? TimeSheetUpdate { get; set; }
/*[GEN-18]*/
    public override TimeSheetApprovalEntity GetEntity(TimeSheetApprovalEntity entity)
    {
      if (this.Id != null) entity.Id = (long)this.Id;
      if (this.Order != null) entity.Order = this.Order;
      if (this.Status != null) entity.Status = this.Status;
      if (this.Notes != null) entity.Notes = this.Notes;
      if (this.UserEntityId != null) entity.UserEntityId = this.UserEntityId;
      if (this.UserEntity != null) entity.UserEntity = this.UserEntity;
      if (this.TimeSheetUpdateId != null) entity.TimeSheetUpdateId = this.TimeSheetUpdateId;
      if (this.TimeSheetUpdate != null) entity.TimeSheetUpdate = this.TimeSheetUpdate;
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
