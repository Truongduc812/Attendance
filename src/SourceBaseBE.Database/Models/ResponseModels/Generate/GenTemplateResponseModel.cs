using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class GenTemplateResponseModel : BaseCRUDResponseModel<GenTemplateEntity>
  {
    /*[GEN-20]*/
    public override object SetData(GenTemplateEntity entity)
    {
      base.SetData(entity);
      /*[GEN-21]*/
      return this;
    }
    public override List<object> SetData(List<GenTemplateEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (GenTemplateEntity entity in listEntity)
      {
        listRS.Add(new GenTemplateResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
