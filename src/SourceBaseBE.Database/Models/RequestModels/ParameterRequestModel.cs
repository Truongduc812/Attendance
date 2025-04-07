using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
	public class ParameterRequestModel : BaseCRUDRequestModel<ParameterEntity>
	{
		public string? Category { get; set; }
		public string? EnviromentVarName { get; set; }
		public string? LastUpdatedValue { get; set; }
		public string? StandardValue { get; set; }
		public string? DefaultValue { get; set; }
		public string? UnitOfMeasurement { get; set; }
		public bool IsEnabled { get; set; }
		public EnumReadWrite? ReadWrite { get; set; }
		public string? Publisher { get; set; }
		public string? Tags { get; set; }
		public int? MinTimeSavingDataIntervals { get; set; }
		public long? DataTypeId { get; set; }
		public long? LineId { get; set; }
		public long? MachineId { get; set; }
		public long? EquipmentId { get; set; }
		public long? DeviceId { get; set; }

		public ParameterEntity GetEntity()
		{
			var ret = new ParameterEntity();
			if (this.Id != null) ret.Id = (long)this.Id;
			if (this.Order != null) ret.Order = this.Order;
			if (this.Category != null) ret.Category = this.Category;
			if (this.EnviromentVarName != null) ret.EnviromentVarName = this.EnviromentVarName;
			if (this.LastUpdatedValue != null) ret.LastUpdatedValue = this.LastUpdatedValue;
			if (this.StandardValue != null) ret.StandardValue = this.StandardValue;
			if (this.DefaultValue != null) ret.DefaultValue = this.DefaultValue;
			if (this.UnitOfMeasurement != null) ret.UnitOfMeasurement = this.UnitOfMeasurement;
			if (this.IsEnabled != null) ret.IsEnabled = this.IsEnabled;
			if (this.ReadWrite != null) ret.ReadWrite = this.ReadWrite;
			if (this.Publisher != null) ret.Publisher = this.Publisher;
			if (this.Tags != null) ret.Tags = this.Tags;
			if (this.MinTimeSavingDataIntervals != null) ret.MinTimeSavingDataIntervals = this.MinTimeSavingDataIntervals;
			if (this.DataTypeId != null) ret.DataTypeId = this.DataTypeId;
			if (this.LineId != null) ret.LineId = this.LineId;
			if (this.MachineId != null) ret.MachineId = this.MachineId;
			if (this.EquipmentId != null) ret.EquipmentId = this.EquipmentId;
			if (this.DeviceId != null) ret.DeviceId = this.DeviceId;

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
