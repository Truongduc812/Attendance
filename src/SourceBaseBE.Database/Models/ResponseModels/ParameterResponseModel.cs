using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class ParameterResponseModel : BaseCRUDResponseModel<ParameterEntity>
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
		public DataTypeEntity? DataType { get; set; }
		public long? LineId { get; set; }
		public LineEntity? Line { get; set; }
		public long? MachineId { get; set; }
		public MachineEntity? Machine { get; set; }
		public long? EquipmentId { get; set; }
		public EquipmentEntity? Equipment { get; set; }
		public long? DeviceId { get; set; }
		public DeviceEntity? Device { get; set; }
		public ICollection<LimitationEntity> Limitations { get; set; }

		public override object SetData(ParameterEntity entity)
		{
			base.SetData(entity);
			this.Category = entity.Category;
			this.EnviromentVarName = entity.EnviromentVarName;
			this.LastUpdatedValue = entity.LastUpdatedValue;
			this.StandardValue = entity.StandardValue;
			this.DefaultValue = entity.DefaultValue;
			this.UnitOfMeasurement = entity.UnitOfMeasurement;
			this.IsEnabled = entity.IsEnabled;
			this.ReadWrite = entity.ReadWrite;
			this.Publisher = entity.Publisher;
			this.Tags = entity.Tags;
			this.MinTimeSavingDataIntervals = entity.MinTimeSavingDataIntervals;
			this.DataTypeId = entity.DataTypeId;
			this.DataType = entity.DataType;
			this.LineId = entity.LineId;
			this.Line = entity.Line;
			this.MachineId = entity.MachineId;
			this.Machine = entity.Machine;
			this.EquipmentId = entity.EquipmentId;
			this.Equipment = entity.Equipment;
			this.DeviceId = entity.DeviceId;
			this.Limitations = entity.Limitations;

			return this;
		}
		public override List<object> SetData(List<ParameterEntity> listEntity)
		{
			List<Object> listRS = new List<object>();
			foreach (ParameterEntity entity in listEntity)
			{
				listRS.Add(new ParameterResponseModel().SetData(entity));
			}
			return listRS;
		}
	}
}
