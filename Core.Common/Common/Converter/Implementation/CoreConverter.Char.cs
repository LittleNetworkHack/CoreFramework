using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static char? ToChar(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToChar(v);
				case char v:
					return ToChar(v);
				case string v:
					return ToChar(v);

				case float v:
					return ToChar(v);
				case double v:
					return ToChar(v);
				case decimal v:
					return ToChar(v);

				case byte v:
					return ToChar(v);
				case short v:
					return ToChar(v);
				case int v:
					return ToChar(v);
				case long v:
					return ToChar(v);

				case sbyte v:
					return ToChar(v);
				case ushort v:
					return ToChar(v);
				case uint v:
					return ToChar(v);
				case ulong v:
					return ToChar(v);

				case byte[] v:
					return ToChar(v);
				//case DateTime v:
				//	return ToChar(v);
				//case TimeSpan v:
				//	return ToChar(v);
				//case XmlDocument v:
				//	return ToChar(v);
				//case Guid v:
				//	return ToChar(v);
				default:
					return null;
			}
		}

		public static char? ToChar(bool value) => value ? '\u0001' : '\u0000';
		public static char? ToChar(char value) => value;
		public static char? ToChar(string value) => string.IsNullOrEmpty(value) ? null : (char?)value[0];

		public static char? ToChar(byte value) => (char?)value;
		public static char? ToChar(short value) => OutOfRangeFloat(value, char.MinValue, char.MaxValue) ? null : (char?)value;
		public static char? ToChar(int value) => OutOfRangeFloat(value, char.MinValue, char.MaxValue) ? null : (char?)value;
		public static char? ToChar(long value) => OutOfRangeFloat(value, char.MinValue, char.MaxValue) ? null : (char?)value;

		public static char? ToChar(sbyte value) => OutOfRangeFloat(value, char.MinValue, char.MaxValue) ? null : (char?)value;
		public static char? ToChar(ushort value) => (char?)value;
		public static char? ToChar(uint value) => OutOfRangeFloat(value, char.MinValue, char.MaxValue) ? null : (char?)value;
		public static char? ToChar(ulong value) => OutOfRangeFloat(value, char.MinValue, char.MaxValue) ? null : (char?)value;

		public static char? ToChar(float value) => OutOfRangeFloat(value, char.MinValue, char.MaxValue) ? null : (char?)value;
		public static char? ToChar(double value) => OutOfRangeDouble(value, char.MinValue, char.MaxValue) ? null : (char?)value;
		public static char? ToChar(decimal value) => OutOfRangeDecimal(value, char.MinValue, char.MaxValue) ? null : (char?)value;

		public static char? ToChar(byte[] value) => OutOfRangeBinary(value, sizeof(char)) ? null : (char?)BitConverter.ToChar(value, 0);
	}
}
