using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;
using System.Runtime.CompilerServices;
using SourceBaseBE.Database.Enums;
using iSoft.Common.Enums;
using MathNet.Numerics.RootFinding;
using iSoft.Database.Extensions;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SourceBaseBE.Database.Attribute;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities
{
    public class WorkingDayEntity : BaseCRUDEntity
    {
        [Key]
        [FormDataTypeAttribute(EnumFormDataType.hidden, true, defaultVal: null)]
        [JsonPropertyName("WorkingDayId")]
        public long? WorkingDayId => this.Id > 0 ? this.Id : null;
        public WorkingDayEntity()
        {
            //ProcessInOutState();
        }
        private bool isNeedCalculate = true;
        [Column("WorkingDate")]
        [FormDataType("Working Date", EnumFormDataType.dateOnly, true)]
        public DateTime? WorkingDate { get; set; }
        private DateTime? time_in;
        [FormDataType("Time In", EnumFormDataType.datetime, false)]
        [Column("time_in")]
        public DateTime? Time_In { get { return time_in; } set { time_in = value;/* ProcessInOutState()*/;/* CalculateDeviation();*/ } }

        private DateTime? time_out;
        [Column("time_out")]
        [FormDataType("Time Out", EnumFormDataType.datetime, false)]
        public DateTime? Time_Out { get { return time_out; } set { time_out = value;/* ProcessInOutState()*/; } }

        [Column("time_deviation")]
        [FormDataType("Time Deviation", EnumFormDataType.hourOnly, false)]
        public double? TimeDeviation { get; set; }
        [NotMapped]
        public EnumWorkingDayHighlight? WorkingDayHighlight { get; set; }

        [Column("working_day_status")]
        [FormDataType("Working Status", EnumFormDataType.select, false)]
        public EnumWorkingDayStatus? WorkingDayStatus { get; set; }
        public EnumInOutTypeStatus InOutState
        {
            get;
            set;
        }

        [Column("notes")]
        [FormDataType("Notes", EnumFormDataType.textarea, false)]
        public string? Notes { get; set; }
        [FormDataType("Employee Name", EnumFormDataType.readonlyType, false)]
        public string? EmployeeName => Employee?.Name;
        [ForeignKey(nameof(EmployeeEntity))]
        [FormDataType("EmployeeId", EnumFormDataType.hidden, true)]
        public long? EmployeeEntityId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public EmployeeEntity? Employee { get; set; }

        [ForeignKey(nameof(WorkingTypeEntity))]
        [Column("working_type_id")]
        [FormDataType("Working Type", EnumFormDataType.select, false)]
        public long? WorkingTypeEntityId { get; set; }
        [Column("working_type")]
        public virtual WorkingTypeEntity? WorkingType { get; set; }
        //[NotMapped]
        [Column("recommendtype")]
        [Filterable("recommendtype", "recommendtype", false)]
        public string? RecommendType { get; set; }
        //[Column("recommendtype_id")]
        //[Filterable("recommendtypeid", "recommendtypeid", false)]
        //public long? RecommendTypeId { get; set; }
        public virtual ICollection<WorkingDayUpdateEntity>? WorkingDayUpdates { get; set; }
        public virtual ICollection<WorkingDayApprovalEntity>? WorkingDayApprovals { get; set; }
        public virtual ICollection<TimeSheetEntity>? TimeSheets { get; set; }

        public override string ToString()
        {
            return $"{this.Id}:{this.WorkingDate}:{this.WorkingDayStatus.ToString()}:{this.RecommendType}";
        }
    }
}