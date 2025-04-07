using InfluxDB.Client.Api.Domain;
using iSoft.Common.CommonFunctionNS;
using iSoft.Common.Enums;
using iSoft.Common.Utils;
using iSoft.Database.Entities;
using iSoft.Database.Entities.Interface;
using iSoft.Database.Extensions;
using iSoft.DBLibrary.Entities;
using MathNet.Numerics.Differentiation;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Entities
{
	[Table("WorkingDayUpdates")]
	public class WorkingDayUpdateEntity : BaseCRUDEntity
	{
		[FormDataTypeAttribute(EnumFormDataType.select, false)]
		[DisplayName("Employee")]
		public long? EmployeeId { get; set; }
		public EmployeeEntity? Employee { get; set; }

		[ForeignKey(nameof(WorkingDayEntity))]
		[FormDataTypeAttribute(EnumFormDataType.hidden, false)]
		[DisplayName("WorkingDayId")]
		public long? WorkingDayId { get; set; }
		public WorkingDayEntity? WorkingDay { get; set; }
		[ForeignKey(nameof(UserEntity))]
		[FormDataTypeAttribute(EnumFormDataType.hidden, true)]
		[Browsable(false)]
		[DisplayName("EditerId")]
		public long? EditerId { get; set; }
		public UserEntity? Editer { get; set; }
		[Column("WorkingDate")]
		[FormDataTypeAttribute(EnumFormDataType.datetime, false)]
		[DisplayName("WorkingDate")]
		public DateTime? WorkingDate { get; set; }
		[DisplayName("Time In")]
		[FormDataTypeAttribute(EnumFormDataType.datetime, false)]
		public DateTime? Time_In { get; set; }
		[DisplayName("Time out")]
		[FormDataTypeAttribute(EnumFormDataType.datetime, false)]
		public DateTime? Time_Out { get; set; }
		[Column("time_deviation")]
		[FormDataTypeAttribute(EnumFormDataType.integerNumber, false, Unit = "s")]
		[DisplayName("Time Deviation")]
		public long? Time_Deviation { get; set; }
		[DisplayName("Status")]
		[FormDataTypeAttribute(EnumFormDataType.select, false)]
		public EnumWorkingDayStatus? WorkingDayStatus { get; set; }
		[ForeignKey(nameof(WorkingTypeEntity))]
		[DisplayName("Type")]
		[FormDataTypeAttribute(EnumFormDataType.select, false)]
		public long? WorkingTypeId { get; set; }
		public WorkingTypeEntity? WorkingType { get; set; }
		[FormDataTypeAttribute(EnumFormDataType.textarea, false)]
		[DisplayName("Update Reason")]
		public string? Update_Reason { get; set; }
		[Column("notes")]
		[FormDataTypeAttribute(EnumFormDataType.textarea, false)]
		[DisplayName("Notes")]
		public string? Notes { get; set; }

        //[DisplayName("Request")]
        public EnumActionRequest? ActionRequest { get; set; }
        public DateTime? OriginalWorkDate { get; set; }

		public DateTime? OriginTimeIn { get; set; }

		public DateTime? OriginTimeOut { get; set; }

		public double? OriginTimeDeviation { get; set; }

		public EnumWorkingDayStatus? OriginWorkingDayStatus { get; set; }

		[ForeignKey("Origin" + nameof(WorkingTypeEntity))]
		public long? OriginWorkingTypeId { get; set; }

		public virtual WorkingTypeEntity? OriginWorkingType { get; set; }
		public virtual ICollection<WorkingDayApprovalEntity>? WorkingDayApprovals { get; set; }
	}
}
