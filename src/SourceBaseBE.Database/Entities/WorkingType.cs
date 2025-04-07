using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;
using System.Runtime.CompilerServices;
using SourceBaseBE.Database.Enums;
using iSoft.Common.Enums;
using System.ComponentModel;
using iSoft.Database.Extensions;

namespace SourceBaseBE.Database.Entities
{
	public class WorkingTypeEntity : BaseCRUDEntity
	{
		[Required]
		[DisplayName("Name")]
		[FormDataTypeAttribute(EnumFormDataType.textbox, true)]
		public string Name { get; set; }
		[Required]
		[DisplayName("Code")]
		[FormDataTypeAttribute(EnumFormDataType.textbox, true)]
		public string Code { get; set; }

		[DisplayName("Normal Meal")]
		public int? Normal_Meal { get; set; }

		[DisplayName("OT 150%")]
		public int? OT_150 { get; set; }

		[DisplayName("NS 30%")]
		public int? Normal_Night_30 { get; set; }

		[DisplayName("OT 200%")]
		public int? OT_200 { get; set; }

		[DisplayName("Weekend Meal")]
		public int? Weekend_Meal { get; set; }

		[DisplayName("Weekend OT 200%")]
		public int? Weekend_OT_200 { get; set; }

		[DisplayName("Weekend NS 270%")]
		public int? Weekend_Night_OT_270 { get; set; }

		[DisplayName("Holiday Meal")]
		public int? Holiday_Meal { get; set; }

		[DisplayName("OT 300%")]
		public int? Holiday_OT_300 { get; set; }

		[DisplayName("OT NS 390")]
		public int? Holiday_OT_Night_390 { get; set; }

		[DisplayName("Description")]
		public string? Description { get; set; }
		public string? Note { get; set; }
		public ICollection<WorkingDayEntity> WorkingDayEntities { get; set; }
		[InverseProperty(nameof(WorkingDayUpdateEntity.WorkingType))]
		public ICollection<WorkingDayUpdateEntity> WorkingTypeEntities { get; set; }
		[InverseProperty(nameof(WorkingDayUpdateEntity.OriginWorkingType))]
		public ICollection<WorkingDayUpdateEntity> OriginWorkingDayEntities { get; set; }
		public ICollection<HolidayWorkingTypeEntity> HolidayWorkingTypes { get; set; }
        public ICollection<WorkingTypeDescriptionEntity> WorkingTypeDescriptions { get; set; }


        public override string ToString()
    {
			return $"{this.Id}:{this.Code}";
    }
  }
}