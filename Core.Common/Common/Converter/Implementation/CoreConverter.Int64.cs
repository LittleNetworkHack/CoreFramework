using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static long? ToInt64(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToInt64(v);
				case char v:
					return ToInt64(v);
				case string v:
					return ToInt64(v);

				case float v:
					return ToInt64(v);
				case double v:
					return ToInt64(v);
				case decimal v:
					return ToInt64(v);

				case byte v:
					return ToInt64(v);
				case short v:
					return ToInt64(v);
				case int v:
					return ToInt64(v);
				case long v:
					return ToInt64(v);

				case sbyte v:
					return ToInt64(v);
				case ushort v:
					return ToInt64(v);
				case uint v:
					return ToInt64(v);
				case ulong v:
					return ToInt64(v);

				case byte[] v:
					return ToInt64(v);
				//case DateTime v:
				//	return ToInt64(v);
				//case TimeSpan v:
				//	return ToInt64(v);
				//case XmlDocument v:
				//	return ToInt64(v);
				//case Guid v:
				//	return ToInt64(v);
				default:
					return TryCast<long>(value);
			}
		}

		public static long? ToInt64(bool value) => (long?)(value ? 1 : 0);
		public static long? ToInt64(char value) => value;
		public static long? ToInt64(string value) => long.TryParse(value, out long result) ? (long?)result : null;

		public static long? ToInt64(byte value) => value;
		public static long? ToInt64(short value) => value;
		public static long? ToInt64(int value) => value;
		public static long? ToInt64(long value) => value;

		public static long? ToInt64(sbyte value) => value;
		public static long? ToInt64(ushort value) => value;
		public static long? ToInt64(uint value) => value;
		public static long? ToInt64(ulong value) => OutOfRangeFloat(value, int.MinValue, int.MaxValue) ? null : (long?)value;

		public static long? ToInt64(float value) => OutOfRangeFloat(value, int.MinValue, int.MaxValue) ? null : (long?)value;
		public static long? ToInt64(double value) => OutOfRangeDouble(value, int.MinValue, int.MaxValue) ? null : (long?)value;
		public static long? ToInt64(decimal value) => OutOfRangeDecimal(value, long.MinValue, long.MaxValue) ? null : (long?)value;

		public static long? ToInt64(byte[] value) => OutOfRangeBinary(value, sizeof(long)) ? null : (long?)BitConverter.ToInt64(value, 0);
		public static long? ToInt64(DateTime value) => value.Ticks;
	}
}
