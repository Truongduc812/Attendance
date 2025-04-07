using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Common.Enums;
using iSoft.Database.Entities;
using iSoft.Database.Extensions;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
    [Table("WorkingTypeDescriptions")]
    public class WorkingTypeDescriptionEntity : BaseCRUDEntity
    {
        [Column("name")]
        [FormDataTypeAttribute(EnumFormDataType.textbox, true)]
        public string? Name { get; set; }

        [ForeignKey(nameof(WorkingTypeEntity))]
        [FormDataType("WorkingType", iSoft.Common.Enums.EnumFormDataType.select, true)]
        public long? WorkingTypeId { get; set; }
        public WorkingTypeEntity? WorkingTypeItem { get; set; }

    }
}
