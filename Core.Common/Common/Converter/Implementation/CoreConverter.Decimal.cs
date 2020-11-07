using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static decimal? ToDecimal(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToDecimal(v);
				case char v:
					return ToDecimal(v);
				case string v:
					return ToDecimal(v);

				case float v:
					return ToDecimal(v);
				case double v:
					return ToDecimal(v);
				case decimal v:
					return ToDecimal(v);

				case byte v:
					return ToDecimal(v);
				case short v:
					return ToDecimal(v);
				case int v:
					return ToDecimal(v);
				case long v:
					return ToDecimal(v);

				case sbyte v:
					return ToDecimal(v);
				case ushort v:
					return ToDecimal(v);
				case uint v:
					return ToDecimal(v);
				case ulong v:
					return ToDecimal(v);

				case byte[] v:
					return ToDecimal(v);
				//case DateTime v:
				//	return ToDecimal(v);
				//case TimeSpan v:
				//	return ToDecimal(v);
				//case XmlDocument v:
				//	return ToDecimal(v);
				//case Guid v:
				//	return ToDecimal(v);
				default:
					return TryCast<decimal>(value);
			}
		}

		public static decimal? ToDecimal(bool value) => value ? 1 : 0;
		public static decimal? ToDecimal(char value) => value;
		public static decimal? ToDecimal(string value) => decimal.TryParse(value, out decimal result) ? (decimal?)result : null;

		public static decimal? ToDecimal(byte value) => value;
		public static decimal? ToDecimal(short value) => value;
		public static decimal? ToDecimal(int value) => value;
		public static decimal? ToDecimal(long value) => value;

		public static decimal? ToDecimal(sbyte value) => value;
		public static decimal? ToDecimal(ushort value) => value;
		public static decimal? ToDecimal(uint value) => value;
		public static decimal? ToDecimal(ulong value) => value;

		public static decimal? ToDecimal(float value) => OutOfRangeFloat(value, (float)decimal.MinValue, (float)decimal.MaxValue) ? null : (decimal?)value;
		public static decimal? ToDecimal(double value) => OutOfRangeDouble(value, (double)decimal.MinValue, (double)decimal.MaxValue) ? null : (decimal?)value;
		public static decimal? ToDecimal(decimal value) => value;

		public static decimal? ToDecimal(byte[] value)
		{
			try
			{
				if (OutOfRangeBinary(value, 16))
					return null;

				int[] arr = new int[4];
				arr[0] = BitConverter.ToInt32(value, 0);
				arr[1] = BitConverter.ToInt32(value, 4);
				arr[2] = BitConverter.ToInt32(value, 8);
				arr[3] = BitConverter.ToInt32(value, 12);
				return new decimal(arr);
			}
			catch
			{
				return null;
			}
		}
	}
}
