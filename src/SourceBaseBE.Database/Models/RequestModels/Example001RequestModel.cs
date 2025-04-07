using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class Example001RequestModel : BaseCRUDRequestModel<Example001Entity>
  {
    public string Name { get; set; }
    public string? Description { get; set; }

    DateTime? _startDate { get; set; }
    public string? StartDate
    {
      get
      {
        if (_startDate.HasValue)
        {
          return _startDate.Value.ToString(ConstDateTimeFormat.YYYYMMDD);
        }
        return null;
      }
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          try
          {
            _startDate = DateTimeUtil.GetDateTimeFromString(value, ConstDateTimeFormat.YYYYMMDD);
          }
          catch
          {
            _startDate = null;
          }
        }
      }
    }
    public DateTime? StartDateTime { get; set; }
    public int? RefreshTime1 { get; set; }
    public int? RefreshTime2 { get; set; }
    public int? RefreshTime3 { get; set; }
    public int? RefreshTime4 { get; set; }
    public double? Price { get; set; }
    public int? Gender { get; set; }
    public bool? Enable { get; set; }
    public IFormFile? Avatar { get; set; }
    public long? GenTemplateId { get; set; }
    public GenTemplateEntity? ItemGenTemplate { get; set; }
    public List<long>? ListGenTemplate { get; set; }

    public override Example001Entity GetEntity(Example001Entity entity)
    {
      if (Id != null)
      {
        entity.Id = (long)Id;
      }
      entity.Order = Order;
      entity.Name = this.Name;
      entity.Description = this.Description;
      entity.StartDate = this._startDate;
      entity.StartDateTime = this.StartDateTime;
      entity.RefreshTime1 = this.RefreshTime1;
      entity.RefreshTime2 = this.RefreshTime2;
      entity.RefreshTime3 = this.RefreshTime3;
      entity.RefreshTime4 = this.RefreshTime4;
      entity.Price = this.Price;
      entity.Gender = this.Gender;
      entity.Enable = this.Enable;
      entity.GenTemplateId = this.GenTemplateId;
      entity.ItemGenTemplate = this.ItemGenTemplate;

      if (this.ListGenTemplate != null)
      {
        entity.GenTemplateIds = this.ListGenTemplate.Select(x => x).ToList();
      }

      return entity;
    }

    public override Dictionary<string, (string, IFormFile)> GetFiles()
    {
      Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
      if (this.Avatar != null)
      {
        dicRS.Add(nameof(Avatar), (Path.Combine(ConstFolderPath.Image, ConstFolderPath.Upload), this.Avatar));
      }
/*[GEN-17]*/
      return dicRS;
    }
  }
}
