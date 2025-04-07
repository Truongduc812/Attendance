using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  [Table("VehicleTypes")]
  public class VehicleTypeEntity : BaseCRUDEntity
  {
    public VehicleTypeEntity() {
      Vehicles = new HashSet<VehicleEntity>();
    }

    [Required]
    [Column("TypeName")]
    [MaxLength(255)]
    public string? TypeName { get; set; }

    public virtual ICollection<VehicleEntity> Vehicles { get; set; }
  }
}
