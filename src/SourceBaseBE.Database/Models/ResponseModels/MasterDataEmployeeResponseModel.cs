using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class MasterDataEmployeeResponseModel : BaseCRUDResponseModel<MasterDataEmployeeEntity>
  {

    public override object SetData(MasterDataEmployeeEntity entity)
    {
      base.SetData(entity);

      return this;
    }
    public override List<object> SetData(List<MasterDataEmployeeEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (MasterDataEmployeeEntity entity in listEntity)
      {
        listRS.Add(new MasterDataEmployeeResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
