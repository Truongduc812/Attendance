using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
	public class DeviceRequestModel : BaseCRUDRequestModel<DeviceEntity>
	{
		public string? Name { get; set; }
		public string? Supplier { get; set; }
		public string? Model { get; set; }
		public long? MaxOperationTime { get; set; }
		public string? Manufacturer { get; set; }
		public DateTime? ExpiryDate { get; set; }
		public string? Group { get; set; }
		public string? Category { get; set; }
		public long? LineId { get; set; }
		public long? MachineId { get; set; }
		public string? MachineName { get; set; }
		public long? EquipmentId { get; set; }

		public DeviceEntity GetEntity()
		{
			var ret = new DeviceEntity();
			if (this.Id != null) ret.Id = (long)this.Id;
			if (this.Order != null) ret.Order = this.Order;
			if (this.Name != null) ret.Name = this.Name;
			if (this.Supplier != null) ret.Supplier = this.Supplier;
			if (this.Model != null) ret.Model = this.Model;
			if (this.MaxOperationTime != null) ret.MaxOperationTime = this.MaxOperationTime;
			if (this.Manufacturer != null) ret.Manufacturer = this.Manufacturer;
			if (this.ExpiryDate != null) ret.ExpiryDate = this.ExpiryDate;
			if (this.Group != null) ret.Group = this.Group;
			if (this.Category != null) ret.Category = this.Category;
			if (this.LineId != null) ret.LineId = this.LineId;
			if (this.MachineId != null) ret.MachineId = this.MachineId;
			if (this.MachineName != null) ret.MachineName = this.MachineName;
			if (this.EquipmentId != null) ret.EquipmentId = this.EquipmentId;

			return ret;
		}

		public override Dictionary<string, (string, IFormFile)> GetFiles()
		{
			Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();

			/*[GEN-17]*/
			return dicRS;
		}
	}
}
