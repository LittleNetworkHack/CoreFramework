using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static int? ToInt32(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToInt32(v);
				case char v:
					return ToInt32(v);
				case string v:
					return ToInt32(v);

				case float v:
					return ToInt32(v);
				case double v:
					return ToInt32(v);
				case decimal v:
					return ToInt32(v);

				case byte v:
					return ToInt32(v);
				case short v:
					return ToInt32(v);
				case int v:
					return ToInt32(v);
				case long v:
					return ToInt32(v);

				case sbyte v:
					return ToInt32(v);
				case ushort v:
					return ToInt32(v);
				case uint v:
					return ToInt32(v);
				case ulong v:
					return ToInt32(v);

				case byte[] v:
					return ToInt32(v);
				//case DateTime v:
				//	return ToInt32(v);
				//case TimeSpan v:
				//	return ToInt32(v);
				//case XmlDocument v:
				//	return ToInt32(v);
				//case Guid v:
				//	return ToInt32(v);
				default:
					return TryCast<int>(value);
			}
		}

		public static int? ToInt32(bool value) => (int?)(value ? 1 : 0);
		public static int? ToInt32(char value) => value;
		public static int? ToInt32(string value) => int.TryParse(value, out int result) ? (int?)result : null;

		public static int? ToInt32(byte value) => value;
		public static int? ToInt32(short value) => value;
		public static int? ToInt32(int value) => value;
		public static int? ToInt32(long value) => OutOfRangeFloat(value, int.MinValue, int.MaxValue) ? null : (int?)value;

		public static int? ToInt32(sbyte value) => value;
		public static int? ToInt32(ushort value) => value;
		public static int? ToInt32(uint value) => OutOfRangeFloat(value, int.MinValue, int.MaxValue) ? null : (int?)value;
		public static int? ToInt32(ulong value) => OutOfRangeFloat(value, int.MinValue, int.MaxValue) ? null : (int?)value;

		public static int? ToInt32(float value) => OutOfRangeFloat(value, int.MinValue, int.MaxValue) ? null : (int?)value;
		public static int? ToInt32(double value) => OutOfRangeDouble(value, int.MinValue, int.MaxValue) ? null : (int?)value;
		public static int? ToInt32(decimal value) => OutOfRangeDecimal(value, int.MinValue, int.MaxValue) ? null : (int?)value;

		public static int? ToInt32(byte[] value) => OutOfRangeBinary(value, sizeof(int)) ? null : (int?)BitConverter.ToInt32(value, 0);
	}
}
