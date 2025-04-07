using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Common.Enums;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Enums;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
    [Table("TimeSheetApprovals")]
    public class TimeSheetApprovalEntity : BaseCRUDEntity
    {
        [Column("status")]
        public EnumApproveStatus? Status { get; set; }
        [Column("notes")]
        public string? Notes { get; set; }
        [ForeignKey(nameof(UserEntity))]
        public long? UserEntityId { get; set; }
        public UserEntity? UserEntity { get; set; }

        [ForeignKey(nameof(TimeSheetUpdateEntity))]
        public long? TimeSheetUpdateId { get; set; }
        public TimeSheetUpdateEntity? TimeSheetUpdate { get; set; }
    }
}
