using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static uint? ToUInt32(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToUInt32(v);
				case char v:
					return ToUInt32(v);
				case string v:
					return ToUInt32(v);

				case float v:
					return ToUInt32(v);
				case double v:
					return ToUInt32(v);
				case decimal v:
					return ToUInt32(v);

				case byte v:
					return ToUInt32(v);
				case short v:
					return ToUInt32(v);
				case int v:
					return ToUInt32(v);
				case long v:
					return ToUInt32(v);

				case sbyte v:
					return ToUInt32(v);
				case ushort v:
					return ToUInt32(v);
				case uint v:
					return ToUInt32(v);
				case ulong v:
					return ToUInt32(v);

				case byte[] v:
					return ToUInt32(v);
				//case DateTime v:
				//	return ToUInt32(v);
				//case TimeSpan v:
				//	return ToUInt32(v);
				//case XmlDocument v:
				//	return ToUInt32(v);
				//case Guid v:
				//	return ToUInt32(v);
				default:
					return null;
			}
		}

		public static uint? ToUInt32(bool value) => (uint?)(value ? 1 : 0);
		public static uint? ToUInt32(char value) => value;
		public static uint? ToUInt32(string value) => uint.TryParse(value, out uint result) ? (uint?)result : null;

		public static uint? ToUInt32(byte value) => value;
		public static uint? ToUInt32(short value) => OutOfRangeFloat(value, uint.MinValue, uint.MaxValue) ? null : (uint?)value;
		public static uint? ToUInt32(int value) => OutOfRangeFloat(value, uint.MinValue, uint.MaxValue) ? null : (uint?)value;
		public static uint? ToUInt32(long value) => OutOfRangeFloat(value, uint.MinValue, uint.MaxValue) ? null : (uint?)value;

		public static uint? ToUInt32(sbyte value) => OutOfRangeFloat(value, uint.MinValue, uint.MaxValue) ? null : (uint?)value;
		public static uint? ToUInt32(ushort value) => value;
		public static uint? ToUInt32(uint value) => value;
		public static uint? ToUInt32(ulong value) => OutOfRangeFloat(value, uint.MinValue, uint.MaxValue) ? null : (uint?)value;

		public static uint? ToUInt32(float value) => OutOfRangeFloat(value, uint.MinValue, uint.MaxValue) ? null : (uint?)value;
		public static uint? ToUInt32(double value) => OutOfRangeDouble(value, uint.MinValue, uint.MaxValue) ? null : (uint?)value;
		public static uint? ToUInt32(decimal value) => OutOfRangeDecimal(value, uint.MinValue, uint.MaxValue) ? null : (uint?)value;

		public static uint? ToUInt32(byte[] value) => OutOfRangeBinary(value, sizeof(uint)) ? null : (uint?)BitConverter.ToUInt32(value, 0);
	}
}
