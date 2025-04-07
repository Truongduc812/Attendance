using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class LanguageRequestModel : BaseCRUDRequestModel<LanguageEntity>
  {
    public string? Key { get; set; }
    public string? DisplayName { get; set; }
    public string? Category { get; set; }
    public string? Language { get; set; }

    public override LanguageEntity GetEntity(LanguageEntity entity)
    {
      if (this.Id != null) entity.Id = (long)this.Id;
      if (this.Order != null) entity.Order = this.Order;
      if (this.Key != null) entity.Key = this.Key;
      if (this.DisplayName != null) entity.DisplayName = this.DisplayName;
      if (this.Category != null) entity.Category = this.Category;
      if (this.Language != null) entity.Language = this.Language;

      return entity;
    }

    public override Dictionary<string, (string, IFormFile)> GetFiles()
    {
      Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
      
/*[GEN-17]*/
      return dicRS;
    }
  }
}
