using iSoft.Common.Enums;
using iSoft.Common.Utils;
using iSoft.Database.Entities;
using iSoft.Database.Entities.Interface;
using iSoft.Database.Extensions;
using iSoft.DBLibrary.Entities;
using MathNet.Numerics.Differentiation;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;
using static iSoft.Common.ConstCommon;

namespace SourceBaseBE.Database.Entities
{
    [Table("SystemSettings")]
    public class SystemSettingEntity : BaseCRUDEntity
    {
        [FormDataTypeAttribute(EnumFormDataType.textbox, true)]
        public long? UpdateStateEmployeeInterval { get; set; }

        [FormDataTypeAttribute(EnumFormDataType.textbox, true)]
        public long? TimeSwitchInterval { get; set; }
    }
}
