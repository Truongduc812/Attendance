using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class MessageResponseModel : BaseCRUDResponseModel<MessageEntity>
  {
    public string Title { get; set; }
    public string Content { get; set; }
    public string? URL { get; set; }
    public bool IsRead { get; set; }
    public DateTime SendDate { get; set; }
    public long? UserId { get; set; }
    public UserEntity? ItemUser { get; set; }
/*[GEN-20]*/
    public override object SetData(MessageEntity entity)
    {
      base.SetData(entity);
      this.Title = entity.Title;
      this.Content = entity.Content;
      this.URL = entity.URL;
      this.IsRead = entity.IsRead;
      this.SendDate = entity.SendDate;
      this.UserId = entity.UserId;
      this.ItemUser = entity.ItemUser;
/*[GEN-21]*/
      return this;
    }
    public override List<object> SetData(List<MessageEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (MessageEntity entity in listEntity)
      {
        listRS.Add(new MessageResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
