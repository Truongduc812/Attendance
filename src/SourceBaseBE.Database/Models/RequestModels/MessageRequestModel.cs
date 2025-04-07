using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.RequestModels.Generate
{
  public class MessageRequestModel : BaseCRUDRequestModel<MessageEntity>
  {
    public string Title { get; set; }
    public string Content { get; set; }
    public string? URL { get; set; }
    public bool IsRead { get; set; }
    public DateTime SendDate { get; set; }
    public long? UserId { get; set; }
    public UserEntity? ItemUser { get; set; }
/*[GEN-18]*/
    public override MessageEntity GetEntity(MessageEntity entity)
    {
      if (this.Id != null) entity.Id = (long)this.Id;
      if (this.Order != null) entity.Order = this.Order;
      if (this.Title != null) entity.Title = this.Title;
      if (this.Content != null) entity.Content = this.Content;
      if (this.URL != null) entity.URL = this.URL;
      if (this.IsRead != null) entity.IsRead = this.IsRead;
      if (this.SendDate != null) entity.SendDate = this.SendDate;
      if (this.UserId != null) entity.UserId = this.UserId;
      if (this.ItemUser != null) entity.ItemUser = this.ItemUser;
/*[GEN-19]*/
      return entity;
    }

    public override Dictionary<string, (string, IFormFile)> GetFiles()
    {
      Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
      
/*[GEN-17]*/
      return dicRS;
    }
  }
}
