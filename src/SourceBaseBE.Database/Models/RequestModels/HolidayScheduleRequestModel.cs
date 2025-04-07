using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels.Generate
{
  public class HolidayScheduleRequestModel : BaseCRUDRequestModel<HolidayScheduleEntity>
  {
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public IFormFile? FileImport { get; set; }
    public ICollection<HolidayWorkingTypeEntity>? HolidayWorkingTypes { get; set; }
    public bool? IsReplace { get; set; }

    public override HolidayScheduleEntity GetEntity(HolidayScheduleEntity entity)
    {
      if (Id != null) entity.Id = (long)Id;
      if (Order != null) entity.Order = Order;
      if (Name != null) entity.Name = Name;

      return entity;
    }

    public override Dictionary<string, (string, IFormFile)> GetFiles()
    {
      Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
      if (this.FileImport != null)
      {
        dicRS.Add(nameof(FileImport), (Path.Combine(ConstFolderPath.Image, ConstFolderPath.Upload), this.FileImport));
      }
      /*[GEN-17]*/
      return dicRS;
    }
  }
}
