using iSoft.Common.Enums;
using iSoft.Database.Entities;
using iSoft.Database.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Entities
{
  public class AuthGroupEntity : BaseCRUDEntity
  {

    [FormDataTypeAttribute(EnumFormDataType.textbox, true)]
    public string Name { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.textarea, false)]
    public string? Description { get; set; }


    [NotMapped]
    public List<long>? AuthPermissionIds { get; set; } = new List<long>();
    [ListEntityAttribute(nameof(AuthPermissionEntity), nameof(AuthPermissionIds), "")]
    public List<AuthPermissionEntity>? ListAuthPermission { get; set; } = new();


    [NotMapped]
    public List<long>? UserIds { get; set; } = new List<long>();
    [ListEntityAttribute(nameof(UserEntity), nameof(UserIds), "")]
    public List<UserEntity>? ListUser { get; set; } = new();
  }
}
