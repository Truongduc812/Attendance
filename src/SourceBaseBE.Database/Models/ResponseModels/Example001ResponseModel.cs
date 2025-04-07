using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class Example001ResponseModel : BaseCRUDResponseModel<Example001Entity>
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
    public string? Avatar { get; set; }
    public long? GenTemplateId { get; set; }
    public GenTemplateEntity? ItemGenTemplate { get; set; }
    public List<GenTemplateEntity>? ListGenTemplate { get; set; }

    public override object SetData(Example001Entity entity)
    {
      base.SetData(entity);
      this.Name = entity.Name;
      this.Description = entity.Description;
      this._startDate = entity.StartDate;
      this.StartDateTime = entity.StartDateTime;
      this.RefreshTime1 = entity.RefreshTime1;
      this.RefreshTime2 = entity.RefreshTime2;
      this.RefreshTime3 = entity.RefreshTime3;
      this.RefreshTime4 = entity.RefreshTime4;
      this.Price = entity.Price;
      this.Gender = entity.Gender;
      this.Enable = entity.Enable;
      this.Avatar = entity.Avatar;
      this.GenTemplateId = entity.GenTemplateId;
      this.ItemGenTemplate = entity.ItemGenTemplate;

      if (entity.ListGenTemplate != null)
      {
        this.ListGenTemplate = entity.ListGenTemplate.Select(x => x).ToList();
      }

      return this;
    }
    public override List<object> SetData(List<Example001Entity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (Example001Entity entity in listEntity)
      {
        listRS.Add(new Example001ResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
