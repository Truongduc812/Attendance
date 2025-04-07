using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  public class ShiftEntity : BaseCRUDEntity
  {
    public ShiftEntity()
    {
      Employees = new HashSet<EmployeeEntity>();
    }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Time)]
    [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime StartTime { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Time)]
    [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime EndTime { get; set; }
    public long? ShiftTypeId { get; set; }
    public ShiftTypeEntity? ShiftType { get; set; }
    public ICollection<EmployeeEntity> Employees { get; set; }
  }
}
