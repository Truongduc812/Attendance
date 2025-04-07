using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.DBLibrary.Entities;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iSoft.Database.Entities
{
    [Table("FCM")]
    public class FCMEntity: BaseCRUDEntity
  {
        [Required]
        [Column("UserId")]
        public long UserId { get; set; }

        [Column("Token")]
        [StringLength(255)]
        public string Token { get; set; }

        [Column("Status")]
        [DefaultValue(EnumActiveStatus.Actived)]
        public EnumActiveStatus Status { get; set; }
    }
}