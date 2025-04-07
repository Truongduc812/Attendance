using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.ComponentModel;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;
//using System.Text.Json.Serialization;

namespace SourceBaseBE.Database.Entities
{
  public class MachineEntity : BaseCRUDEntity, IEntityCategory
  {
    public MachineEntity()
    {
      Equipments = new HashSet<EquipmentEntity>();

      Parameters = new HashSet<ParameterEntity>();

      Devices = new HashSet<DeviceEntity>();
    }
    public string? Category { get; set; }

    [ForeignKey(nameof(Entities.LineEntity))]
    public long LineId { get; set; }

    public LineEntity? Line { get; set; }

    [ForeignKey(nameof(Entities.FileEntity))]
    [DefaultValue(null)]
    public long? MachineOperatingInstructionFileId { set; get; }
    public FileEntity? File { get; set; }

    public ICollection<EquipmentEntity> Equipments { get; set; }

    [JsonIgnore]
    public ICollection<ParameterEntity> Parameters { get; set; }

    public ICollection<DeviceEntity> Devices { get; set; }
  }
}
