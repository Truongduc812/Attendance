using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.RequestModels.Generate
{
  public class GenTemplateRequestModel : BaseCRUDRequestModel<GenTemplateEntity>
  {
    /*[GEN-18]*/
    public override GenTemplateEntity GetEntity(GenTemplateEntity entity)
    {
      if (this.Id != null) entity.Id = (long)this.Id;
      if (this.Order != null) entity.Order = this.Order;
      /*[GEN-19]*/
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
