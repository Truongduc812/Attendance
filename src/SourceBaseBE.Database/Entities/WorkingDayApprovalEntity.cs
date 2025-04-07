using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;
using System.Runtime.CompilerServices;
using SourceBaseBE.Database.Enums;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Entities
{
	[Table("WorkingDayApprovals")]
	public class WorkingDayApprovalEntity : BaseCRUDEntity
	{
		public WorkingDayApprovalEntity()
		{
			//ProcessInOutState();
		}
		[ForeignKey(nameof(WorkingDayEntity))]
		public long? WorkingDayId { get; set; }
		public WorkingDayEntity? WorkingDay { get; set; }
		[ForeignKey(nameof(WorkingDayUpdateEntity))]
		public long? WorkingDayUpdateId { get; set; }
		public WorkingDayUpdateEntity? WorkingDayUpdate { get; set; }
		[ForeignKey(nameof(UserEntity))]
		public long? ApproverId { get; set; }
		public UserEntity? Approver { get; set; }
		[Column("approve_status")]
		public EnumApproveStatus ApproveStatus { get; set; }
		[Column("approve_reasion")]
		public string? Approve_Reason { get; set; }
		[Column("notes")]
		public string? Notes { get; set; }
		
	}
}