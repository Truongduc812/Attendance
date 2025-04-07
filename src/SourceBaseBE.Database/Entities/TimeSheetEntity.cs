using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Common.Enums;
using iSoft.Database.Entities;
using iSoft.Database.Extensions;
using Newtonsoft.Json;
using SourceBaseBE.Database.Enums;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
    [Table("TimeSheets")]
    public class TimeSheetEntity : BaseCRUDEntity
    {

        [Column("status")]
        [DisplayName("Type")]
        [JsonProperty("status")]
        [FormDataType(EnumFormDataType.select, true)]
        public EnumFaceId? Status { get; set; }
        [FormDataType(EnumFormDataType.datetime, true)]

        [Column("recorded_time")]
        [DisplayName("Record Time")]
        [JsonProperty("recordedtime")]
        public DateTime? RecordedTime { get; set; }
        [ForeignKey(nameof(EmployeeEntity))]
        [FormDataType(EnumFormDataType.hidden, true)]
        public long? EmployeeId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public EmployeeEntity? Employee { get; set; }
        [ForeignKey(nameof(WorkingDay))]
        [FormDataType(EnumFormDataType.hidden, false)]
        public long? WorkingDayId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public WorkingDayEntity? WorkingDay { get; set; }
        public virtual ICollection<TimeSheetUpdateEntity> TimeSheetUpdateEntities { get; set; }

        public override string ToString()
        {
            return $"{this.RecordedTime}:{this.Status.ToString()}";
        }
    }
}
