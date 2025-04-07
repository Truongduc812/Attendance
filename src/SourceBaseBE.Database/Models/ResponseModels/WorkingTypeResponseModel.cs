using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class WorkingTypeResponseModel : BaseCRUDResponseModel<WorkingTypeEntity>
	{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public int? Normal_Meal { get; set; }
    public int? OT_150 { get; set; }
    public int? OT_Night_30 { get; set; }
    public int? OT_200 { get; set; }
    public int? Weekend_Meal { get; set; }
    public int? Weekend_OT_200 { get; set; }
    public int? Weekend_Night_OT_270 { get; set; }
    public int? Holiday_Meal { get; set; }
    public int? Holiday_OT_300 { get; set; }
    public int? Holiday_OT_Night_390 { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public IFormFile? FileImport { get; set; }
    public List<WorkingDayEntity>? WorkingDays { get; set; }
    public List<WorkingDayEntity>? OriginWorkingDays { get; set; }
    public List<HolidayWorkingTypeEntity>? HolidayWorkingTypes { get; set; }

    
     

  }
}
