using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
  public class LineEntity : BaseCRUDEntity, IEntityCategory
  {
    public string? Category { get; set; }

    [ForeignKey(nameof(Entities.WorkshopEntity))]
    public long WorkshopId { get; set; }

    [Required]
    public WorkshopEntity Workshop { get; set; }

    public ICollection<MachineEntity> Machines { get; set; }

    public ICollection<EquipmentEntity> Equipments { get; set; }

    public ICollection<DeviceEntity> Devices { get; set; }

    public ICollection<ParameterEntity> Parameters { get; set; }

    public LineEntity()
    {
      Machines = new HashSet<MachineEntity>();

      Equipments = new HashSet<EquipmentEntity>();

      Parameters = new HashSet<ParameterEntity>();

      Devices = new HashSet<DeviceEntity>();
    }
  }
}
