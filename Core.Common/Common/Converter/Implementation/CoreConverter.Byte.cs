using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static byte? ToByte(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToByte(v);
				case char v:
					return ToByte(v);
				case string v:
					return ToByte(v);

				case float v:
					return ToByte(v);
				case double v:
					return ToByte(v);
				case decimal v:
					return ToByte(v);

				case byte v:
					return ToByte(v);
				case short v:
					return ToByte(v);
				case int v:
					return ToByte(v);
				case long v:
					return ToByte(v);

				case sbyte v:
					return ToByte(v);
				case ushort v:
					return ToByte(v);
				case uint v:
					return ToByte(v);
				case ulong v:
					return ToByte(v);

				case byte[] v:
					return ToByte(v);
				//case DateTime v:
				//	return ToByte(v);
				//case TimeSpan v:
				//	return ToByte(v);
				//case XmlDocument v:
				//	return ToByte(v);
				//case Guid v:
				//	return ToByte(v);
				default:
					return null;
			}
		}

		public static byte? ToByte(bool value) => (byte?)(value ? 1 : 0);
		public static byte? ToByte(char value) => OutOfRangeFloat(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;
		public static byte? ToByte(string value) => byte.TryParse(value, out byte result) ? (byte?)result : null;

		public static byte? ToByte(byte value) => value;
		public static byte? ToByte(short value) => OutOfRangeFloat(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;
		public static byte? ToByte(int value) => OutOfRangeFloat(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;
		public static byte? ToByte(long value) => OutOfRangeFloat(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;

		public static byte? ToByte(sbyte value) => OutOfRangeFloat(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;
		public static byte? ToByte(ushort value) => OutOfRangeFloat(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;
		public static byte? ToByte(uint value) => OutOfRangeFloat(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;
		public static byte? ToByte(ulong value) => OutOfRangeFloat(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;

		public static byte? ToByte(float value) => OutOfRangeFloat(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;
		public static byte? ToByte(double value) => OutOfRangeDouble(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;
		public static byte? ToByte(decimal value) => OutOfRangeDecimal(value, byte.MinValue, byte.MaxValue) ? null : (byte?)value;

		public static byte? ToByte(byte[] value) => OutOfRangeBinary(value, sizeof(byte)) ? null : (byte?)value[0];
	}
}
