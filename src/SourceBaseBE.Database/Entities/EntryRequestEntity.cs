using iSoft.Database.Entities;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Entities
{
  [Table("EntryRequests")]
  [Microsoft.EntityFrameworkCore.Index(nameof(EntryRequestEntity.QrCode), IsUnique = true)]
  public class EntryRequestEntity : BaseCRUDEntity
  {
    public EntryRequestEntity()
    {
      DriverRegistrations = new HashSet<DriverRegistrationEntity>();
      Alarms = new HashSet<AlarmEntity>();
      EntryRequestLogs = new HashSet<EntryRequestLogEntity>();
      GoodRegistrations = new HashSet<GoodRegistrationEntity>();
      Guests = new HashSet<GuestEntity>();
      EntryCaptures = new HashSet<EntryCaptureEntity>();
      AreaCodes = new HashSet<AreaCodeEntity>();
    }

    [Column("QrCode", TypeName = "CHAR(6)")]
    public string? QrCode { get; set; }

    [Required]
    [Column("OwnerDriverId")]
    [ForeignKey(nameof(this.EmployeeOwnerDriver))]
    public long OwnerDriverId { get; set; }
    public EmployeeEntity EmployeeOwnerDriver { get; set; }

    [Required]
    [ForeignKey(nameof(this.Vehicle))]
    [Column("VehicleId")]
    public long VehicleId { get; set; }
    public VehicleEntity Vehicle { get; set; }

    [Required]
    [Column("RecordedTime", TypeName = "TIMESTAMP")]
    public DateTime? TimeIn { get; set; }

    [Required]
    [Column("TimeOut", TypeName = "TIMESTAMP")]
    public DateTime? TimeOut { get; set; }

    [Required]
    [Column("Weight")]
    public decimal? Weight { get; set; }

    [Column("Note", TypeName = "TEXT")]
    public string? Note { get; set; }

    [Column("ApprovedBy")]
    [ForeignKey(nameof(this.EmployeeApproved))]
    public long? ApprovedBy { get; set; }
    public EmployeeEntity? EmployeeApproved { get; set; }

    [Column("TimeApproved", TypeName = "TIMESTAMP")]
    public DateTime? TimeApproved { get; set; }

    [Required]
    [Column("RegistrationStatus")]
    [DefaultValue(EnumRegistrationStatus.INITIALIZED)]
    public string RegistrationStatus { get; set; }

    [Required]
    [Column("EntryRequestTypeId")]
    [ForeignKey(nameof(this.EntryRequestType))]
    public long EntryRequestTypeId { get; set; }
    public EntryRequestTypeEntity EntryRequestType { get; set; }

    [Required]
    [Column("EntryTransactionTypeId")]
    [ForeignKey(nameof(this.EntryTransactionType))]
    public long EntryTransactionTypeId { get; set; }
    public EntryTransactionTypeEntity EntryTransactionType { get; set; }

    [Column("IsGuest")]
    [DefaultValue(false)]
    public bool? IsGuest { get; set; }

    public ICollection<DriverRegistrationEntity> DriverRegistrations { get; set; }
    public virtual ICollection<AlarmEntity> Alarms { get; set; }
    public virtual ICollection<EntryRequestLogEntity> EntryRequestLogs { get; set; }
    public virtual ICollection<GoodRegistrationEntity> GoodRegistrations { get; set; }
    public virtual ICollection<GuestEntity> Guests { get; set; }
    public virtual ICollection<EntryCaptureEntity> EntryCaptures { get; set; }
    public virtual ICollection<AreaCodeEntity> AreaCodes { get; set; }
  }
}
