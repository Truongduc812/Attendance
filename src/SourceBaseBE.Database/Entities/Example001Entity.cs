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
  [Table("Example001s")]
  public class Example001Entity : BaseCRUDEntity
  {
    [FormDataTypeAttribute(EnumFormDataType.textbox, true)]
    public string Name { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.textarea, false)]
    public string? Description { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.dateOnly, false, min: "2023-01-10", max: "2024-12-10", "2024-01-01", null)]
    public DateTime? StartDate { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.datetime, false, min: "2023-01-10T23:58:00.000", max: "2024-12-10T23:58:00.000", "2024-01-01T23:58:00.000", null)]
    public DateTime? StartDateTime { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.timespan, false)]
    public int? RefreshTime1 { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.integerNumber, false, defaultVal: 5, "Seconds")]
    public int? RefreshTime2 { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.integerNumber, false, min: 1, max: 100, defaultVal: 5, "Seconds")]
    public int? RefreshTime3 { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.select, false, new object[] { 1, 5, 10, 20 }, 5, "Seconds")]
    public int? RefreshTime4 { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.floatNumber, false, 0, 100000000000000, 0, "VND")]
    public double? Price { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.radio, false, new object[] { 1, 2 }, 1, null)]
    public int? Gender { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.checkbox, false, true, null)]
    public bool? Enable { get; set; }


    [FormDataTypeAttribute(EnumFormDataType.image, false, "100px", "100px", 1.0)]
    public string? Avatar { get; set; }


    [ForeignKey(nameof(GenTemplateEntity))]
    public long? GenTemplateId { get; set; }
    public GenTemplateEntity? ItemGenTemplate { get; set; }


    [NotMapped]
    public List<long>? GenTemplateIds { get; set; } = new List<long>();
    [ListEntityAttribute(nameof(GenTemplateEntity), nameof(GenTemplateIds), "")]
    public List<GenTemplateEntity>? ListGenTemplate { get; set; } = new();

    public override void SetFileURL(Dictionary<string, string> dicImagePath)
    {
      if (dicImagePath.ContainsKey(nameof(Avatar)))
      {
        this.Avatar = dicImagePath[nameof(Avatar)];
      }
    }
  }
}
