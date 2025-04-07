using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels.Generate
{
	public class HolidayWorkingTypeRequestModel : BaseCRUDRequestModel<HolidayWorkingTypeEntity>
	{
		public long? WorkingTypeId { get; set; }
		public WorkingTypeEntity? WorkingType { get; set; }
		public long? HolidayScheduleId { get; set; }
		public HolidayScheduleEntity? HolidaySchedule { get; set; }

		public override HolidayWorkingTypeEntity GetEntity(HolidayWorkingTypeEntity entity)
		{
			if (Id != null) entity.Id = (long)Id;
			if (Order != null) entity.Order = Order;

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
