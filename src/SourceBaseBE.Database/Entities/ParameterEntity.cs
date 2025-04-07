using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Newtonsoft.Json;
using iSoft.Common.Enums;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
	public class ParameterEntity : BaseCRUDEntity, IEnityIsEnabled, IEntityCategory
	{


		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? Category { get; set; }

		//[Required]
		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? EnviromentVarName { get; set; }

		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? LastUpdatedValue { get; set; }

		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? StandardValue { get; set; }

		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? DefaultValue { get; set; }

		[MaxLength(100)]
		[Column(TypeName = "VARCHAR(100)")]
		public string? UnitOfMeasurement { get; set; }

		[DefaultValue(true)]
		public bool IsEnabled { get; set; }

		/// <summary>
		/// ReadOnly, WriteOnly, ReadWrite, None
		/// </summary>
		[DefaultValue(EnumReadWrite.ReadOnly)]
		[Column(TypeName = "INT")]
		public EnumReadWrite? ReadWrite { get; set; }

		[DefaultValue("")]
		[Column(TypeName = "VARCHAR(100)")]
		public string? Publisher { get; set; }

		[DefaultValue(null)]
		[Column(TypeName = "VARCHAR(100)")]
		public string? Tags { get; set; }

		[DefaultValue(28)]
		public int? MinTimeSavingDataIntervals { get; set; }

		[ForeignKey(nameof(Entities.DataTypeEntity))]
		public long? DataTypeId { get; set; }
		public DataTypeEntity? DataType { get; set; }

		[ForeignKey(nameof(Entities.LineEntity))]
		[DefaultValue(null)]
		public long? LineId { get; set; }
		public LineEntity? Line { get; set; }

		[ForeignKey(nameof(Entities.MachineEntity))]
		[DefaultValue(null)]
		public long? MachineId { get; set; }
		[JsonIgnore]
		public MachineEntity? Machine { get; set; }

		[ForeignKey(nameof(Entities.EquipmentEntity))]
		[DefaultValue(null)]
		public long? EquipmentId { get; set; }
		public EquipmentEntity? Equipment { get; set; }

		[ForeignKey(nameof(DeviceEntity))]
		[DefaultValue(null)]
		public long? DeviceId { get; set; }
		public DeviceEntity? Device { get; set; }

		public ICollection<LimitationEntity> Limitations { get; set; }
		public override string ToString()
		{
			return this.EnviromentVarName.Trim();
		}
	}
}
