using iSoft.Common;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iSoft.DBLibrary.Entities
{
  public class BaseIdTimeEnity : BaseEntity
  {
    [Key]
    public long Id { get; set; }

    public long? CreatedBy { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.fff}")]
    public DateTime? CreatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.fff}")]
    public DateTime? UpdatedAt { get; set; }

    [DefaultValue(false)]
    public bool? DeletedFlag { get; set; } = false;
  }
}