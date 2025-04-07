using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class DeviceResponseModel : BaseCRUDResponseModel<DeviceEntity>
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
    public LineEntity? Line { get; set; }
    public long? MachineId { get; set; }
    public MachineEntity? Machine { get; set; }
    public string MachineName { get; set; }
    public long? EquipmentId { get; set; }
    public EquipmentEntity? Equipment { get; set; }
    public ICollection<ParameterEntity> Parameters { get; set; }

    public override object SetData(DeviceEntity entity)
    {
      base.SetData(entity);
      this.Name = entity.Name;
      this.Supplier = entity.Supplier;
      this.Model = entity.Model;
      this.MaxOperationTime = entity.MaxOperationTime;
      this.Manufacturer = entity.Manufacturer;
      this.ExpiryDate = entity.ExpiryDate;
      this.Group = entity.Group;
      this.Category = entity.Category;
      this.LineId = entity.LineId;
      this.Line = entity.Line;
      this.MachineId = entity.MachineId;
      this.Machine = entity.Machine;
      this.MachineName = entity.MachineName;
      this.EquipmentId = entity.EquipmentId;
      this.Equipment = entity.Equipment;
      this.Parameters = entity.Parameters;

      return this;
    }
    public override List<object> SetData(List<DeviceEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (DeviceEntity entity in listEntity)
      {
        listRS.Add(new DeviceResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
