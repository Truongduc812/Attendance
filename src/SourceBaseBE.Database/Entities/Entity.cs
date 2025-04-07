using iSoft.DBLibrary.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SourceBaseBE.Database.Interfaces;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
  [Serializable]
  public class Entity : BaseEntity, IEntityId, IEnityCreatedAt, IEntityUpdatedAt, IEnityCreatedBy, IEntityUpdatedBy, IEntityDescription, IEnityDeletedFlag, IEntityName
  {
    [Key] // Khai báo Id là khóa chính
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required] // ConnectionKey là trường bắt buộc
    [MaxLength(100)] // Độ dài tối đa cho ConnectionKey là 100 ký tự
    [Column(TypeName = "NVARCHAR(100)")] // Xác định kiểu dữ liệu cơ sở dữ liệu là NVARCHAR(100)
    public string Name { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "NVARCHAR(255)")]
    public string? SerialCode { get; set; }


    [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? CreatedAt { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? UpdatedAt { get; set; }

    [MaxLength(255)] // Độ dài tối đa cho Description là 255 ký tự
    [Column(TypeName = "NVARCHAR(255)")] // Xác định kiểu dữ liệu cơ sở dữ liệu là NVARCHAR(255)
    public string? Description { get; set; }

    [DefaultValue(false)]
    public bool DeletedFlag { get; set; }

    [NotMapped]
    public override bool? isDelete { get => base.isDelete; set => base.isDelete = value; }

    [NotMapped]
    public override bool? isEnable { get => base.isEnable; set => base.isEnable = value; }

    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }
    public override string ToString()
    {
      return this.Name;
    }
  }
}
