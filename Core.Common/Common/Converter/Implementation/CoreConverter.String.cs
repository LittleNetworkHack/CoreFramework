using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Core.Reflection;

namespace Core
{
	public static partial class CoreConverter
	{
		public static string ToString(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToString(v);
				case char v:
					return ToString(v);
				case string v:
					return ToString(v);

				case float v:
					return ToString(v);
				case double v:
					return ToString(v);
				case decimal v:
					return ToString(v);

				case byte v:
					return ToString(v);
				case short v:
					return ToString(v);
				case int v:
					return ToString(v);
				case long v:
					return ToString(v);

				case sbyte v:
					return ToString(v);
				case ushort v:
					return ToString(v);
				case uint v:
					return ToString(v);
				case ulong v:
					return ToString(v);

				case byte[] v:
					return ToString(v);
				case DateTime v:
					return ToString(v);
				//case TimeSpan v:
				//	return ToString(v);
				//case XmlDocument v:
				//	return ToString(v);
				//case Guid v:
				//	return ToString(v);
				default:
					return value.ToString();
			}
		}

		public static string ToString(bool value) => value ? bool.TrueString : bool.FalseString;
		public static string ToString(char value) => new string(value, 1);
		public static string ToString(string value) => value;

		public static string ToString(byte value) => value.ToString();
		public static string ToString(short value) => value.ToString();
		public static string ToString(int value) => value.ToString();
		public static string ToString(long value) => value.ToString();

		public static string ToString(sbyte value) => value.ToString();
		public static string ToString(ushort value) => value.ToString();
		public static string ToString(uint value) => value.ToString();
		public static string ToString(ulong value) => value.ToString();

		public static string ToString(float value) => value.ToString();
		public static string ToString(double value) => value.ToString();
		public static string ToString(decimal value) => value.ToString();

		public static string ToString(byte[] value) => OutOfRangeBinary(value, 0) ? null : Encoding.UTF8.GetString(value);
		public static string ToString(DateTime value) => value.ToString();
	}
}
