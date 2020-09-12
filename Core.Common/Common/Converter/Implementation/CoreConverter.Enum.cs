using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Reflection;

namespace Core
{
	public static partial class CoreConverter
	{
		public static object ToEnum(object value, Type enumType)
		{
			if (value.IsNullOrDBNull())
				return null;

			if (value is string text)
				return ToEnum(text, enumType);

			Type baseType = Enum.GetUnderlyingType(enumType);
			value = ConvertTo(value, baseType);
			if (value.IsNullOrDBNull())
				return null;

			return Enum.ToObject(enumType, value);
		}

		public static object ToEnum(string value, Type enumType)
		{
			if (string.IsNullOrEmpty(value))
				return null;

			try
			{
				return Enum.Parse(enumType, value, true);
			}
			catch
			{
				return null;
			}
		}
	}
}
