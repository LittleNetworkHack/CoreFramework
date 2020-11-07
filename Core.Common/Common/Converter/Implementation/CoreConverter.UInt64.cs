using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Reflection;

namespace Core
{
	public static partial class CoreConverter
	{
		public static ulong? ToUInt64(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToUInt64(v);
				case char v:
					return ToUInt64(v);
				case string v:
					return ToUInt64(v);

				case float v:
					return ToUInt64(v);
				case double v:
					return ToUInt64(v);
				case decimal v:
					return ToUInt64(v);

				case byte v:
					return ToUInt64(v);
				case short v:
					return ToUInt64(v);
				case int v:
					return ToUInt64(v);
				case long v:
					return ToUInt64(v);

				case sbyte v:
					return ToUInt64(v);
				case ushort v:
					return ToUInt64(v);
				case uint v:
					return ToUInt64(v);
				case ulong v:
					return ToUInt64(v);

				case byte[] v:
					return ToUInt64(v);
				//case DateTime v:
				//	return ToUInt64(v);
				//case TimeSpan v:
				//	return ToUInt64(v);
				//case XmlDocument v:
				//	return ToUInt64(v);
				//case Guid v:
				//	return ToUInt64(v);
				default:
					return TryCast<ulong>(value);
			}
		}

		public static ulong? ToUInt64(bool value) => (ulong?)(value ? 1 : 0);
		public static ulong? ToUInt64(char value) => value;
		public static ulong? ToUInt64(string value) => ulong.TryParse(value, out ulong result) ? (ulong?)result : null;

		public static ulong? ToUInt64(byte value) => value;
		public static ulong? ToUInt64(short value) => OutOfRangeFloat(value, ulong.MinValue, ulong.MaxValue) ? null : (ulong?)value;
		public static ulong? ToUInt64(int value) => OutOfRangeFloat(value, ulong.MinValue, ulong.MaxValue) ? null : (ulong?)value;
		public static ulong? ToUInt64(long value) => OutOfRangeFloat(value, ulong.MinValue, ulong.MaxValue) ? null : (ulong?)value;

		public static ulong? ToUInt64(sbyte value) => OutOfRangeFloat(value, ulong.MinValue, ulong.MaxValue) ? null : (ulong?)value;
		public static ulong? ToUInt64(ushort value) => value;
		public static ulong? ToUInt64(uint value) => value;
		public static ulong? ToUInt64(ulong value) => value;

		public static ulong? ToUInt64(float value) => OutOfRangeFloat(value, ulong.MinValue, ulong.MaxValue) ? null : (ulong?)value;
		public static ulong? ToUInt64(double value) => OutOfRangeDouble(value, ulong.MinValue, ulong.MaxValue) ? null : (ulong?)value;
		public static ulong? ToUInt64(decimal value) => OutOfRangeDecimal(value, ulong.MinValue, ulong.MaxValue) ? null : (ulong?)value;

		public static ulong? ToUInt64(byte[] value) => OutOfRangeBinary(value, sizeof(ulong)) ? null : (ulong?)BitConverter.ToUInt64(value, 0);
		
	}
}
