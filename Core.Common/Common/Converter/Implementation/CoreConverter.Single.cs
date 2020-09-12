using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static float? ToSingle(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToSingle(v);
				case char v:
					return ToSingle(v);
				case string v:
					return ToSingle(v);

				case float v:
					return ToSingle(v);
				case double v:
					return ToSingle(v);
				case decimal v:
					return ToSingle(v);

				case byte v:
					return ToSingle(v);
				case short v:
					return ToSingle(v);
				case int v:
					return ToSingle(v);
				case long v:
					return ToSingle(v);

				case sbyte v:
					return ToSingle(v);
				case ushort v:
					return ToSingle(v);
				case uint v:
					return ToSingle(v);
				case ulong v:
					return ToSingle(v);

				case byte[] v:
					return ToSingle(v);
				//case DateTime v:
				//	return ToSingle(v);
				//case TimeSpan v:
				//	return ToSingle(v);
				//case XmlDocument v:
				//	return ToSingle(v);
				//case Guid v:
				//	return ToSingle(v);
				default:
					return null;
			}
		}

		public static float? ToSingle(bool value) => (float?)(value ? 1 : 0);
		public static float? ToSingle(char value) => value;
		public static float? ToSingle(string value) => float.TryParse(value, out float result) ? (float?)result : null;

		public static float? ToSingle(byte value) => value;
		public static float? ToSingle(short value) => value;
		public static float? ToSingle(int value) => value;
		public static float? ToSingle(long value) => value;

		public static float? ToSingle(sbyte value) => value;
		public static float? ToSingle(ushort value) => value;
		public static float? ToSingle(uint value) => value;
		public static float? ToSingle(ulong value) => value;

		public static float? ToSingle(float value) => value;
		public static float? ToSingle(double value) => OutOfRangeDouble(value, float.MinValue, float.MaxValue) ? null : (float?)value;
		public static float? ToSingle(decimal value) => (float?)value;

		public static float? ToSingle(byte[] value) => OutOfRangeBinary(value, sizeof(float)) ? null : (float?)BitConverter.ToSingle(value, 0);
	}
}
