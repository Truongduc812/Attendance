using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
    public class SystemSettingResponseModel : BaseCRUDResponseModel<SystemSettingEntity>
    {
        public long? UpdateStateEmployeeInterval { get; set; }
        public long? TimeSwitchInterval { get; set; }
        /*[GEN-20]*/
        public override object SetData(SystemSettingEntity entity)
        {
            base.SetData(entity);
            this.UpdateStateEmployeeInterval = entity.UpdateStateEmployeeInterval;
            this.TimeSwitchInterval = entity.TimeSwitchInterval;
            /*[GEN-21]*/
            return this;
        }
        public override List<object> SetData(List<SystemSettingEntity> listEntity)
        {
            List<Object> listRS = new List<object>();
            foreach (SystemSettingEntity entity in listEntity)
            {
                listRS.Add(new SystemSettingResponseModel().SetData(entity));
            }
            return listRS;
        }
    }
}
