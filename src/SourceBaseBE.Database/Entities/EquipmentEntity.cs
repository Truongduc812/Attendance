using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
  public class EquipmentEntity : BaseCRUDEntity, IEntityCategory, IMaintenance
  {
    public EquipmentEntity()
    {
      Parameters = new HashSet<ParameterEntity>();
      Devices = new HashSet<DeviceEntity>();
    }

    [MaxLength(255)] 
    [Column(TypeName = "VARCHAR(255)")]
    public string? Supplier {set; get;}

    [MaxLength(100)] 
    [Column(TypeName = "VARCHAR(100)")]
    public string? Model {set; get;}

    public long? MaxOperationTime {set; get; }

    public long? RunTime {set; get; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? OperationStartDate {set; get; }

    [MaxLength(255)]
    [Column(TypeName = "VARCHAR(255)")]
    public string? Manufacturer {set; get;}

    [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? ExpiryDate {set; get; }

    [MaxLength(255)]
    [Column(TypeName = "VARCHAR(255)")]
    public string? Group {set; get;}

    [MaxLength(255)]
    [Column(TypeName = "VARCHAR(255)")]
    public string? Category { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR(50)")]
    public string SerialCode { get; set; }

    [ForeignKey(nameof(Entities.LineEntity))]
    public long? LineId { get; set; }
    public LineEntity? Line { get; set; }

    [ForeignKey(nameof(Entities.MachineEntity))]
    public long? MachineId { get; set; }
    public MachineEntity? Machine { get; set; }

    //[InverseProperty("Equipment")]
    public ICollection<ParameterEntity> Parameters { get; set; }

    public ICollection<DeviceEntity> Devices { get; set; }
  }
}
