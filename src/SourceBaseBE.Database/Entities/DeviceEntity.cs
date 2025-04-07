using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Common.Utils;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
	public class DeviceEntity : BaseCRUDEntity, IEntityCategory
	{
		public DeviceEntity()
		{
			this.Parameters = new HashSet<ParameterEntity>();
		}
		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? Name { set; get; }
		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? Supplier { set; get; }

		[MaxLength(100)]
		[Column(TypeName = "VARCHAR(100)")]
		public string? Model { set; get; }

		public long? MaxOperationTime { set; get; }

		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? Manufacturer { set; get; }

		[DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime? ExpiryDate { set; get; }

		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? Group { set; get; }

		[MaxLength(255)]
		[Column(TypeName = "VARCHAR(255)")]
		public string? Category { get; set; }

		[ForeignKey(nameof(Entities.LineEntity))]
		public long? LineId { get; set; }
		public LineEntity? Line { get; set; }

		[ForeignKey(nameof(Entities.MachineEntity))]
		public long? MachineId { get; set; }
		public MachineEntity? Machine { get; set; }
		[NotMapped]
		public string MachineName { get; set; }

		[ForeignKey(nameof(Entities.EquipmentEntity))]
		public long? EquipmentId { get; set; }
		public EquipmentEntity? Equipment { get; set; }
		public ICollection<ParameterEntity> Parameters { get; set; }

	}
}
