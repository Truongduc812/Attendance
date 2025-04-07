using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Database.Entities;
using iSoft.Database.Extensions;
using Newtonsoft.Json;
using SourceBaseBE.Database.Interfaces;
using iSoft.Common.Enums;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Entities
{
	[Table("HolidayWorkingTypes")]
	public class HolidayWorkingTypeEntity : BaseCRUDEntity, IEntityUpdatedAt, IEntityUpdatedBy, IEnityCreatedAt, IEnityCreatedBy
	{

		[ForeignKey(nameof(WorkingTypeEntity))]
		public long? WorkingTypeId { get; set; }
		public WorkingTypeEntity? WorkingType { get; set; }
		[ForeignKey(nameof(HolidayScheduleEntity))]
		public long? HolidayScheduleId { get; set; }
		public HolidayScheduleEntity? HolidaySchedule { get; set; }
	}
}

