using iSoft.Database.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SourceBaseBE.Database.Entities
{
  public class NotificationEntity : BaseCRUDEntity
  {
    [StringLength(255)]
    public string Title { get; set; }

    [StringLength(511)]
    public string Content { get; set; }

    [DefaultValue(false)]
    public bool IsRead { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? SendDate { get; set; }

    public bool DeletedFlag { get; set; }

    public long UserId { get; set; }
  }
}