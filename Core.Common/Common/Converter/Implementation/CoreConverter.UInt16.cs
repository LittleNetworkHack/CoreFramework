using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static ushort? ToUInt16(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToUInt16(v);
				case char v:
					return ToUInt16(v);
				case string v:
					return ToUInt16(v);

				case float v:
					return ToUInt16(v);
				case double v:
					return ToUInt16(v);
				case decimal v:
					return ToUInt16(v);

				case byte v:
					return ToUInt16(v);
				case short v:
					return ToUInt16(v);
				case int v:
					return ToUInt16(v);
				case long v:
					return ToUInt16(v);

				case sbyte v:
					return ToUInt16(v);
				case ushort v:
					return ToUInt16(v);
				case uint v:
					return ToUInt16(v);
				case ulong v:
					return ToUInt16(v);

				case byte[] v:
					return ToUInt16(v);
				//case DateTime v:
				//	return ToUInt16(v);
				//case TimeSpan v:
				//	return ToUInt16(v);
				//case XmlDocument v:
				//	return ToUInt16(v);
				//case Guid v:
				//	return ToUInt16(v);
				default:
					return null;
			}
		}

		public static ushort? ToUInt16(bool value) => (ushort?)(value ? 1 : 0);
		public static ushort? ToUInt16(char value) => value;
		public static ushort? ToUInt16(string value) => ushort.TryParse(value, out ushort result) ? (ushort?)result : null;

		public static ushort? ToUInt16(byte value) => value;
		public static ushort? ToUInt16(short value) => OutOfRangeFloat(value, ushort.MinValue, ushort.MaxValue) ? null : (ushort?)value;
		public static ushort? ToUInt16(int value) => OutOfRangeFloat(value, ushort.MinValue, ushort.MaxValue) ? null : (ushort?)value;
		public static ushort? ToUInt16(long value) => OutOfRangeFloat(value, ushort.MinValue, ushort.MaxValue) ? null : (ushort?)value;

		public static ushort? ToUInt16(sbyte value) => OutOfRangeFloat(value, ushort.MinValue, ushort.MaxValue) ? null : (ushort?)value;
		public static ushort? ToUInt16(ushort value) => value;
		public static ushort? ToUInt16(uint value) => OutOfRangeFloat(value, ushort.MinValue, ushort.MaxValue) ? null : (ushort?)value;
		public static ushort? ToUInt16(ulong value) => OutOfRangeFloat(value, ushort.MinValue, ushort.MaxValue) ? null : (ushort?)value;

		public static ushort? ToUInt16(float value) => OutOfRangeFloat(value, ushort.MinValue, ushort.MaxValue) ? null : (ushort?)value;
		public static ushort? ToUInt16(double value) => OutOfRangeDouble(value, ushort.MinValue, ushort.MaxValue) ? null : (ushort?)value;
		public static ushort? ToUInt16(decimal value) => OutOfRangeDecimal(value, ushort.MinValue, ushort.MaxValue) ? null : (ushort?)value;

		public static ushort? ToUInt16(byte[] value) => OutOfRangeBinary(value, sizeof(ushort)) ? null : (ushort?)BitConverter.ToUInt16(value, 0);
	}
}
