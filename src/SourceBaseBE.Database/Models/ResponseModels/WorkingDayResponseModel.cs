using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class WorkingDayResponseModel : BaseCRUDResponseModel<WorkingDayEntity>
	{
		public DateTime? working_date { get; set; }
		public DateTime? time_in { get; set; }
		public DateTime? time_out { get; set; }
		public double? TimeDeviation { get; set; }
		public EnumWorkingDayStatus? WorkingDayStatus { get; set; }
		public string? Notes { get; set; }
	
		public long? EmployeeEntityId { get; set; }
		public EmployeeEntity? ItemEmployeeEntity { get; set; }

		public override object SetData(WorkingDayEntity entity)
		{
			base.SetData(entity);
			this.working_date = entity.WorkingDate;
			this.time_in = entity.Time_In;
			this.time_out = entity.Time_Out;
			this.TimeDeviation = entity.TimeDeviation;
			this.WorkingDayStatus = entity.WorkingDayStatus;
			this.Notes = entity.Notes;
	
			this.EmployeeEntityId = entity.EmployeeEntityId;
			this.ItemEmployeeEntity = entity.Employee;

			return this;
		}
		public override List<object> SetData(List<WorkingDayEntity> listEntity)
		{
			List<Object> listRS = new List<object>();
			foreach (WorkingDayEntity entity in listEntity)
			{
				listRS.Add(new WorkingDayResponseModel().SetData(entity));
			}
			return listRS;
		}
	}
}
