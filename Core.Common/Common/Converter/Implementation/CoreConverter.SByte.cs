using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static sbyte? ToSByte(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToSByte(v);
				case char v:
					return ToSByte(v);
				case string v:
					return ToSByte(v);

				case float v:
					return ToSByte(v);
				case double v:
					return ToSByte(v);
				case decimal v:
					return ToSByte(v);

				case byte v:
					return ToSByte(v);
				case short v:
					return ToSByte(v);
				case int v:
					return ToSByte(v);
				case long v:
					return ToSByte(v);

				case sbyte v:
					return ToSByte(v);
				case ushort v:
					return ToSByte(v);
				case uint v:
					return ToSByte(v);
				case ulong v:
					return ToSByte(v);

				case byte[] v:
					return ToSByte(v);
				//case DateTime v:
				//	return ToSByte(v);
				//case TimeSpan v:
				//	return ToSByte(v);
				//case XmlDocument v:
				//	return ToSByte(v);
				//case Guid v:
				//	return ToSByte(v);
				default:
					return null;
			}
		}

		public static sbyte? ToSByte(bool value) => (sbyte?)(value ? 1 : 0);
		public static sbyte? ToSByte(char value) => OutOfRangeFloat(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;
		public static sbyte? ToSByte(string value) => sbyte.TryParse(value, out sbyte result) ? (sbyte?)result : null;

		public static sbyte? ToSByte(byte value) => OutOfRangeFloat(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;
		public static sbyte? ToSByte(short value) => OutOfRangeFloat(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;
		public static sbyte? ToSByte(int value) => OutOfRangeFloat(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;
		public static sbyte? ToSByte(long value) => OutOfRangeFloat(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;

		public static sbyte? ToSByte(sbyte value) => value;
		public static sbyte? ToSByte(ushort value) => OutOfRangeFloat(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;
		public static sbyte? ToSByte(uint value) => OutOfRangeFloat(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;
		public static sbyte? ToSByte(ulong value) => OutOfRangeFloat(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;

		public static sbyte? ToSByte(float value) => OutOfRangeFloat(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;
		public static sbyte? ToSByte(double value) => OutOfRangeDouble(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;
		public static sbyte? ToSByte(decimal value) => OutOfRangeDecimal(value, sbyte.MinValue, sbyte.MaxValue) ? null : (sbyte?)value;

		public static sbyte? ToSByte(byte[] value) => OutOfRangeBinary(value, sizeof(sbyte)) ? null : (sbyte?)value[0];
	}
}
