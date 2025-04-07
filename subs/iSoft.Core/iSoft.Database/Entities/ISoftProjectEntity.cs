using iSoft.Common.Enums;
using iSoft.Common.Utils;
using iSoft.Database.Entities;
using iSoft.Database.Entities.Interface;
using iSoft.Database.Extensions;
using iSoft.DBLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;
using static iSoft.Common.ConstCommon;

namespace iSoft.Database.Entities
{
  [Table("ISoftProjects")]
  public class ISoftProjectEntity : BaseCRUDEntity
  {
    [Required]
    [FormDataTypeAttribute(EnumFormDataType.textbox, true)]
    [StringLength(255)]
    public string Name { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.textarea, false)]
    public string? Description { get; set; }


    [NotMapped]
    public List<long>? UserIds { get; set; } = new List<long>();
    [ListEntityAttribute(nameof(UserEntity), nameof(UserIds), "")]
    public List<UserEntity>? ListUser { get; set; } = new();


    [FormDataTypeAttribute(EnumFormDataType.checkbox, false)]
    public EnumActiveStatus? Status { get; set; } = EnumActiveStatus.Actived;
  }
}