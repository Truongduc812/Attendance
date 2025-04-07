using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class LanguageResponseModel : BaseCRUDResponseModel<LanguageEntity>
  {
    public string? Key { get; set; }
    public string? DisplayName { get; set; }
    public string? Category { get; set; }
    public string? Language { get; set; }

    public override object SetData(LanguageEntity entity)
    {
      base.SetData(entity);
      this.Key = entity.Key;
      this.DisplayName = entity.DisplayName;
      this.Category = entity.Category;
      this.Language = entity.Language;

      return this;
    }
    public override List<object> SetData(List<LanguageEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (LanguageEntity entity in listEntity)
      {
        listRS.Add(new LanguageResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
