using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.RequestModels.Generate
{
    public class SystemSettingRequestModel : BaseCRUDRequestModel<SystemSettingEntity>
    {
        public long? UpdateStateEmployeeInterval { get; set; }
        public long? TimeSwitchInterval { get; set; }
        /*[GEN-18]*/
        public override SystemSettingEntity GetEntity(SystemSettingEntity entity)
        {
            if (this.Id != null) entity.Id = (long)this.Id;
            if (this.Order != null) entity.Order = this.Order;
            if (this.UpdateStateEmployeeInterval != null) entity.UpdateStateEmployeeInterval = this.UpdateStateEmployeeInterval;
            if (this.TimeSwitchInterval != null) entity.TimeSwitchInterval = this.TimeSwitchInterval;
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
