using iSoft.Common.Enums;
using iSoft.Database.Entities;
using iSoft.Database.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Entities
{
  public class AuthPermissionEntity : BaseCRUDEntity
  {
    [FormDataTypeAttribute(EnumFormDataType.textbox, true)]
    public string Name { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.textarea, false)]
    public string? Description { get; set; }


    [NotMapped]
    public List<long>? AuthGroupIds { get; set; } = new List<long>();
    [ListEntityAttribute(nameof(AuthGroupEntity), nameof(AuthGroupIds), "")]
    public List<AuthGroupEntity>? ListAuthGroup { get; set; } = new();


    [NotMapped]
    public List<long>? UserIds { get; set; } = new List<long>();
    [ListEntityAttribute(nameof(UserEntity), nameof(UserIds), "")]
    public List<UserEntity>? ListUser { get; set; } = new();
  }
}