using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class WorkingTypeDescriptionResponseModel : BaseCRUDResponseModel<WorkingTypeDescriptionEntity>
  {
    public string? Name { get; set; }
    public long? WorkingTypeId { get; set; }

    public override object SetData(WorkingTypeDescriptionEntity entity)
    {
      base.SetData(entity);
      this.Name = entity.Name;
      this.WorkingTypeId = entity.WorkingTypeId; 
      return this;
    }
    public override List<object> SetData(List<WorkingTypeDescriptionEntity> listEntity)
    {
      List<object> listRS = new List<object>();
      foreach (WorkingTypeDescriptionEntity entity in listEntity)
      {
        listRS.Add(new WorkingTypeDescriptionResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
