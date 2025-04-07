using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class HolidayWorkingTypeResponseModel : BaseCRUDResponseModel<HolidayWorkingTypeEntity>
  {
    public long? WorkingTypeId { get; set; }
    public WorkingTypeEntity? WorkingType { get; set; }
    public long? HolidayScheduleId { get; set; }
    public HolidayScheduleEntity? HolidaySchedule { get; set; }

    public override object SetData(HolidayWorkingTypeEntity entity)
    {
      base.SetData(entity);
      this.WorkingTypeId = entity.WorkingTypeId;
      this.WorkingType = entity.WorkingType;
      this.HolidayScheduleId = entity.HolidayScheduleId;
      this.HolidaySchedule = entity.HolidaySchedule;

      return this;
    }
    public override List<object> SetData(List<HolidayWorkingTypeEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (HolidayWorkingTypeEntity entity in listEntity)
      {
        listRS.Add(new HolidayWorkingTypeResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
