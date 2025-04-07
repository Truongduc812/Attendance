using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class WorkingDayUpdateResponseModel : BaseCRUDResponseModel<WorkingDayUpdateEntity>
	{
		public long? WorkingDayId { get; set; }
		public WorkingDayEntity? WorkingDay { get; set; }
		public long? UserId { get; set; }
		public UserEntity? Editer { get; set; }
		public DateTime? WorkingDate { get; set; }
		public DateTime? Time_In { get; set; }
		public DateTime? Time_Out { get; set; }
		public long? Time_Deviation { get; set; }
		public EnumWorkingDayStatus? WorkingDayStatus { get; set; }
		public long? WorkingTypeId { get; set; }
		public WorkingTypeEntity? WorkingType { get; set; }
		public string? Update_Reason { get; set; }
		public string? Notes { get; set; }
		public DateTime? OriginalWorkDate { get; set; }
		public DateTime? OriginTimeIn { get; set; }
		public DateTime? OriginTimeOut { get; set; }
		public double? OriginTimeDeviation { get; set; }
		public EnumWorkingDayStatus? OriginWorkingDayStatus { get; set; }
		public WorkingTypeEntity? OriginWorkingType { get; set; }
		public ICollection<WorkingDayApprovalEntity>? WorkingDayApprovals { get; set; }

		public override object SetData(WorkingDayUpdateEntity entity)
		{
			base.SetData(entity);
			this.WorkingDayId = entity.WorkingDayId;
			this.WorkingDay = entity.WorkingDay;
			this.UserId = entity.EditerId;
			this.Editer = entity.Editer;
			this.WorkingDate = entity.WorkingDate;
			this.Time_In = entity.Time_In;
			this.Time_Out = entity.Time_Out;
			this.Time_Deviation = entity.Time_Deviation;
			this.WorkingDayStatus = entity.WorkingDayStatus;
			this.WorkingTypeId = entity.WorkingTypeId;
			this.WorkingType = entity.WorkingType;
			this.Update_Reason = entity.Update_Reason;
			this.Notes = entity.Notes;
			this.OriginalWorkDate = entity.OriginalWorkDate;
			this.OriginTimeIn = entity.OriginTimeIn;
			this.OriginTimeOut = entity.OriginTimeOut;
			this.OriginTimeDeviation = entity.OriginTimeDeviation;
			this.OriginWorkingDayStatus = entity.OriginWorkingDayStatus;
			this.OriginWorkingType = entity.OriginWorkingType;

			this.WorkingDayApprovals = entity.WorkingDayApprovals;

			return this;
		}
		public override List<object> SetData(List<WorkingDayUpdateEntity> listEntity)
		{
			List<Object> listRS = new List<object>();
			foreach (WorkingDayUpdateEntity entity in listEntity)
			{
				listRS.Add(new WorkingDayUpdateResponseModel().SetData(entity));
			}
			return listRS;
		}
	}
}
