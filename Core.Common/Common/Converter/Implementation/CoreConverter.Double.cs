using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static double? ToDouble(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToDouble(v);
				case char v:
					return ToDouble(v);
				case string v:
					return ToDouble(v);

				case float v:
					return ToDouble(v);
				case double v:
					return ToDouble(v);
				case decimal v:
					return ToDouble(v);

				case byte v:
					return ToDouble(v);
				case short v:
					return ToDouble(v);
				case int v:
					return ToDouble(v);
				case long v:
					return ToDouble(v);

				case sbyte v:
					return ToDouble(v);
				case ushort v:
					return ToDouble(v);
				case uint v:
					return ToDouble(v);
				case ulong v:
					return ToDouble(v);

				case byte[] v:
					return ToDouble(v);
				//case DateTime v:
				//	return ToDouble(v);
				//case TimeSpan v:
				//	return ToDouble(v);
				//case XmlDocument v:
				//	return ToDouble(v);
				//case Guid v:
				//	return ToDouble(v);
				default:
					return null;
			}
		}

		public static double? ToDouble(bool value) => (double?)(value ? 1 : 0);
		public static double? ToDouble(char value) => value;
		public static double? ToDouble(string value) => double.TryParse(value, out double result) ? (double?)result : null;

		public static double? ToDouble(byte value) => value;
		public static double? ToDouble(short value) => value;
		public static double? ToDouble(int value) => value;
		public static double? ToDouble(long value) => value;

		public static double? ToDouble(sbyte value) => value;
		public static double? ToDouble(ushort value) => value;
		public static double? ToDouble(uint value) => value;
		public static double? ToDouble(ulong value) => value;

		public static double? ToDouble(float value) => value;
		public static double? ToDouble(double value) => value;
		public static double? ToDouble(decimal value) => (double?)value;

		public static double? ToDouble(byte[] value) => OutOfRangeBinary(value, sizeof(double)) ? null : (double?)BitConverter.ToDouble(value, 0);
	}
}
