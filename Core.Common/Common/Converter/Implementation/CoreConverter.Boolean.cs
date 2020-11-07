using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Core
{
	public static partial class CoreConverter
	{
		public static bool? ToBoolean(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToBoolean(v);
				case char v:
					return ToBoolean(v);
				case string v:
					return ToBoolean(v);

				case float v:
					return ToBoolean(v);
				case double v:
					return ToBoolean(v);
				case decimal v:
					return ToBoolean(v);

				case byte v:
					return ToBoolean(v);
				case short v:
					return ToBoolean(v);
				case int v:
					return ToBoolean(v);
				case long v:
					return ToBoolean(v);

				case sbyte v:
					return ToBoolean(v);
				case ushort v:
					return ToBoolean(v);
				case uint v:
					return ToBoolean(v);
				case ulong v:
					return ToBoolean(v);

				case byte[] v:
					return ToBoolean(v);
				//case DateTime v:
				//	return ToBoolean(v);
				//case TimeSpan v:
				//	return ToBoolean(v);
				//case XmlDocument v:
				//	return ToBoolean(v);
				//case Guid v:
				//	return ToBoolean(v);
				default:
					return TryCast<bool>(value);
			}
		}

		public static bool? ToBoolean(bool value) => value;
		public static bool? ToBoolean(char value) => value != '\0' ? true : false;
		public static bool? ToBoolean(string value) => bool.TryParse(value, out bool result) ? (bool?)result : null;

		public static bool? ToBoolean(byte value) => value != 0 ? true : false;
		public static bool? ToBoolean(short value) => value != 0 ? true : false;
		public static bool? ToBoolean(int value) => value != 0 ? true : false;
		public static bool? ToBoolean(long value) => value != 0 ? true : false;

		public static bool? ToBoolean(sbyte value) => value != 0 ? true : false;
		public static bool? ToBoolean(ushort value) => value != 0 ? true : false;
		public static bool? ToBoolean(uint value) => value != 0 ? true : false;
		public static bool? ToBoolean(ulong value) => value != 0 ? true : false;

		public static bool? ToBoolean(float value) => value != 0 ? true : false;
		public static bool? ToBoolean(double value) => value != 0 ? true : false;
		public static bool? ToBoolean(decimal value) => value != 0 ? true : false;

		public static bool? ToBoolean(byte[] value) => OutOfRangeBinary(value, sizeof(bool)) ? null : (bool?)BitConverter.ToBoolean(value, 0);
	}
}
