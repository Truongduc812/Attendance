using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  [Table("Vehicles")]
  public class VehicleEntity : BaseCRUDEntity
  {
    public VehicleEntity()
    {
      EntryRequests = new HashSet<EntryRequestEntity>();
    }

    [Required]
    [ForeignKey(nameof(this.VehicleType))]
    [Column("VehicleTypeId")]
    public long VehicleTypeId { get; set; }
    public VehicleTypeEntity VehicleType {  get; set; }

    [Required]
    [ForeignKey(nameof(this.OwnerDriver))]
    [Column("OwnerDriverId")]
    public long OwnerDriverId { get; set; }
    public EmployeeEntity OwnerDriver { get; set; }

    [ForeignKey(nameof(this.ImageFile))]
    [Column("ImageFileId")]
    public long? ImageFileId { get; set; }
    public FileEntity? ImageFile { get; set; }

    [Required]
    [Column("TankNumber")]
    public int TankNumber { get; set; }

    [Required]
    [Column("SealNumber")]
    [MaxLength(50)]
    public string SealNumber { get; set; }

    [Required]
    [Column("LicensePlate")]
    [MaxLength(255)]
    public string LicensePlate { get; set; }

    [Required]
    [Column("LicensePlateImage", TypeName = "TEXT")]
    [MaxLength(255)]
    public string LicensePlateImage { get; set; }

    //* Last entry request of vehicle
    [ForeignKey(nameof(this.LastEntryRequest))]
    public long? LastEntryRequestId { get; set; }
    public EntryRequestEntity? LastEntryRequest { get; set; }

    [InverseProperty("Vehicle")]
    public virtual ICollection<EntryRequestEntity> EntryRequests { get; set; }
  }
}
