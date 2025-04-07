using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iSoft.Common.ConstCommon;

namespace iSoft.Database.Extensions
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ListEntityAttribute : Attribute
	{
		public string EntityTargetName { get; }
		public string IdsAttrName { get; }
		public string Category { get; }

		public ListEntityAttribute(string entityTargetName, string idsAttrName, string category)
		{
			EntityTargetName = entityTargetName;
			IdsAttrName = idsAttrName;
			Category = category;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class FormDataTypeAttribute : Attribute
	{
		public EnumFormDataType TypeName { get; set; }
		public bool IsRequired { get; set; }
		public string? Width { get; set; } = null;
		public string? Title { get; set; } = null;
		public string? Height { get; set; } = null;
		public string? ImageQuality { get; set; } = null;
		public string? DefaultVal { get; set; } = null;
		public string? Min { get; set; } = null;
		public string? Max { get; set; } = null;
		public string? Unit { get; set; } = null;
		public string? Placeholder { get; set; } = null;
		public List<string>? Options { get; set; } = new List<string>();

		public FormDataTypeAttribute(EnumFormDataType typeName, bool isRequired)
		{
			TypeName = typeName;
			IsRequired = isRequired;
			PrepareData();
		}
		public FormDataTypeAttribute(EnumFormDataType typeName, bool isRequired, object defaultVal)
		{
			TypeName = typeName;
			IsRequired = isRequired;
			this.DefaultVal = defaultVal?.ToString();
		}
		public FormDataTypeAttribute(string title, EnumFormDataType typeName, bool isRequired)
		{
			TypeName = typeName;
			IsRequired = isRequired;
			this.Title = title;
			PrepareData();
		}
		public FormDataTypeAttribute(EnumFormDataType typeName, bool isRequired, string placeholder)
		{
			TypeName = typeName;
			IsRequired = isRequired;
			Placeholder = placeholder;
			PrepareData();
		}
		public FormDataTypeAttribute(EnumFormDataType typeName, bool isRequired, string width, string height, double imageQuality)
		{
			TypeName = typeName;
			IsRequired = isRequired;
			Width = width;
			Height = height;
			ImageQuality = imageQuality.ToString();
			PrepareData();
		}
		public FormDataTypeAttribute(EnumFormDataType typeName, bool isRequired, object min, object max, object defaultVal, string? unit)
		{
			TypeName = typeName;
			IsRequired = isRequired;
			Min = min.ToString();
			Max = max.ToString();
			DefaultVal = defaultVal.ToString();
			Unit = unit;
			PrepareData();
		}
		public FormDataTypeAttribute(EnumFormDataType typeName, bool isRequired, object defaultVal, string unit)
		{
			TypeName = typeName;
			IsRequired = isRequired;
			DefaultVal = defaultVal.ToString();
			Unit = unit;
			PrepareData();
		}
		public FormDataTypeAttribute(EnumFormDataType typeName, bool isRequired, object[] options)
		{
			TypeName = typeName;
			IsRequired = isRequired;
			Options = options.Select(x => x.ToString()).ToList();
			PrepareData();
		}
		public FormDataTypeAttribute(EnumFormDataType typeName, bool isRequired, object[] options, object? defaultVal, string? unit)
		{
			TypeName = typeName;
			IsRequired = isRequired;
			Options = options.Select(x => x.ToString()).ToList();
			DefaultVal = defaultVal?.ToString();
			Unit = unit;
			PrepareData();
		}

		private void PrepareData()
		{
			if (this.TypeName == EnumFormDataType.dateOnly)
			{
				this.Min = this.Min?.Replace("{CURRENT}", DateTime.Today.ToString(ConstDateTimeFormat.YYYYMMDD));
				this.Max = this.Max?.Replace("{CURRENT}", DateTime.Today.ToString(ConstDateTimeFormat.YYYYMMDD));
				this.DefaultVal = this.DefaultVal?.Replace("{CURRENT}", DateTime.Today.ToString(ConstDateTimeFormat.YYYYMMDD));
			}
			else if (this.TypeName == EnumFormDataType.datetime)
			{
				this.Min = this.Min?.Replace("{CURRENT}", DateTime.Now.ToString(ConstDateTimeFormat.YYYYMMDDTHHMMSS_FFF));
				this.Max = this.Max?.Replace("{CURRENT}", DateTime.Now.ToString(ConstDateTimeFormat.YYYYMMDDTHHMMSS_FFF));
				this.DefaultVal = this.DefaultVal?.Replace("{CURRENT}", DateTime.Now.ToString(ConstDateTimeFormat.YYYYMMDDTHHMMSS_FFF));
			}
		}
	}
}
