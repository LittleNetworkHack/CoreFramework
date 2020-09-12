using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static short? ToInt16(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToInt16(v);
				case char v:
					return ToInt16(v);
				case string v:
					return ToInt16(v);

				case float v:
					return ToInt16(v);
				case double v:
					return ToInt16(v);
				case decimal v:
					return ToInt16(v);

				case byte v:
					return ToInt16(v);
				case short v:
					return ToInt16(v);
				case int v:
					return ToInt16(v);
				case long v:
					return ToInt16(v);

				case sbyte v:
					return ToInt16(v);
				case ushort v:
					return ToInt16(v);
				case uint v:
					return ToInt16(v);
				case ulong v:
					return ToInt16(v);

				case byte[] v:
					return ToInt16(v);
				//case DateTime v:
				//	return ToInt16(v);
				//case TimeSpan v:
				//	return ToInt16(v);
				//case XmlDocument v:
				//	return ToInt16(v);
				//case Guid v:
				//	return ToInt16(v);
				default:
					return null;
			}
		}

		public static short? ToInt16(bool value) => (short?)(value ? 1 : 0);
		public static short? ToInt16(char value) => OutOfRangeFloat(value, short.MinValue, short.MaxValue) ? null : (short?)value;
		public static short? ToInt16(string value) => short.TryParse(value, out short result) ? (short?)result : null;

		public static short? ToInt16(byte value) => value;
		public static short? ToInt16(short value) => value;
		public static short? ToInt16(int value) => OutOfRangeFloat(value, short.MinValue, short.MaxValue) ? null : (short?)value;
		public static short? ToInt16(long value) => OutOfRangeFloat(value, short.MinValue, short.MaxValue) ? null : (short?)value;

		public static short? ToInt16(sbyte value) => value;
		public static short? ToInt16(ushort value) => OutOfRangeFloat(value, short.MinValue, short.MaxValue) ? null : (short?)value;
		public static short? ToInt16(uint value) => OutOfRangeFloat(value, short.MinValue, short.MaxValue) ? null : (short?)value;
		public static short? ToInt16(ulong value) => OutOfRangeFloat(value, short.MinValue, short.MaxValue) ? null : (short?)value;

		public static short? ToInt16(float value) => OutOfRangeFloat(value, short.MinValue, short.MaxValue) ? null : (short?)value;
		public static short? ToInt16(double value) => OutOfRangeDouble(value, short.MinValue, short.MaxValue) ? null : (short?)value;
		public static short? ToInt16(decimal value) => OutOfRangeDecimal(value, short.MinValue, short.MaxValue) ? null : (short?)value;

		public static short? ToInt16(byte[] value) => OutOfRangeBinary(value, sizeof(short)) ? null : (short?)BitConverter.ToInt16(value, 0);
	}
}
