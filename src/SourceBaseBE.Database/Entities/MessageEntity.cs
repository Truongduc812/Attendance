using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using iSoft.Database.Entities;
using iSoft.Common.Enums;
using iSoft.Database.Extensions;

namespace SourceBaseBE.Database.Entities
{
  public class MessageEntity : BaseCRUDEntity
  {
    [FormDataTypeAttribute(EnumFormDataType.textbox, true)]
    public string Title { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.textarea, false)]
    public string Content { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.textbox, false)]
    public string? URL { get; set; }


    public bool IsRead { get; set; } = false;


    [FormDataTypeAttribute(EnumFormDataType.datetime, false)]
    public DateTime SendDate { get; set; }


    [ForeignKey(nameof(UserEntity))]
    public long? UserId { get; set; }
    public UserEntity? ItemUser { get; set; }

    public MessageEntity Clone()
    {
      return new MessageEntity
      {
        Title = this.Title,
        Content = this.Content,
        URL = this.URL,
        IsRead = this.IsRead,
        SendDate = this.SendDate,
        UserId = this.UserId,
        // TODO: ItemUser.Clone()
      };
    }
  }
}
