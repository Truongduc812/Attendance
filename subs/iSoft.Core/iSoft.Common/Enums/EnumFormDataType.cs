namespace iSoft.Common.Enums
{
	public enum EnumFormDataType
	{
		hidden,
		readonlyType,
		image,
		dateOnly,
		hourOnly,
		datetime,
		timespan,
		label,
		textarea,
		textbox,
		password,
		email,
		phoneNumber,
		integerNumber,
		floatNumber,
		checkbox,
		radio,
		select,
		selectMulti,
		file,
		listImage,
		listFile,
	}

	public static class EnumExtensions
	{
		public static string ToStringValue(this EnumFormDataType enumValue)
		{
			switch (enumValue)
			{
				case EnumFormDataType.readonlyType:
					return "readonly";
				default:
					return enumValue.ToString();
			}
		}
	}
}
