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
	[Table("HolidaySchedule")]
	public class HolidayScheduleEntity : BaseCRUDEntity, IEntityUpdatedAt, IEntityUpdatedBy, IEnityCreatedAt, IEnityCreatedBy
	{
		[Column("name")]
    [DisplayName("Name")]
    [FormDataTypeAttribute(EnumFormDataType.textbox, true)]
    public string Name { get; set; }

    [DisplayName("Start Date")]
    [Column("start_date")]
    [FormDataType("Start Date", EnumFormDataType.datetime, true)]
    public DateTime StartDate { get; set; }

		[Column("end_date")]
    [FormDataType("End Date", EnumFormDataType.datetime, true)]
    [DisplayName("End Date")]
    public DateTime EndDate { get; set; }

		[Column("description")]
    [FormDataTypeAttribute(EnumFormDataType.textbox, false)]
    [DisplayName("Description")]
    public string? Description { get; set; }

		[Column("notes")]
    [DisplayName("Note")]
    public string? Note { get; set; }


    [Column("holiday_Type")]
    [DisplayName("Holiday Type")]
    [FormDataType("Holiday Type", EnumFormDataType.select, true)]
    public EnumHolidayCode? HolidayType { get; set; }

    public ICollection<HolidayWorkingTypeEntity> HolidayWorkingTypes { get; set; }
	}
}

