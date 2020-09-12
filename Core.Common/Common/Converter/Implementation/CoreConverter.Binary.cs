using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Core
{
	public static partial class CoreConverter
	{
		public static byte[] ToByteArray(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case bool v:
					return ToByteArray(v);
				case char v:
					return ToByteArray(v);
				case string v:
					return ToByteArray(v);

				case float v:
					return ToByteArray(v);
				case double v:
					return ToByteArray(v);
				//case decimal v:
				//	return ToByteArray(v);

				case byte v:
					return ToByteArray(v);
				case short v:
					return ToByteArray(v);
				case int v:
					return ToByteArray(v);
				case long v:
					return ToByteArray(v);

				case sbyte v:
					return ToByteArray(v);
				case ushort v:
					return ToByteArray(v);
				case uint v:
					return ToByteArray(v);
				case ulong v:
					return ToByteArray(v);

				case byte[] v:
					return ToByteArray(v);
				//case DateTime v:
				//	return ToByteArray(v);
				//case TimeSpan v:
				//	return ToByteArray(v);
				//case XmlDocument v:
				//	return ToByteArray(v);
				//case Guid v:
				//	return ToByteArray(v);
				default:
					return null;
			}
		}
		public static byte[] ToByteArray(bool value) => BitConverter.GetBytes(value);
		public static byte[] ToByteArray(char value) => BitConverter.GetBytes(value);
		public static byte[] ToByteArray(string value) => value != null ? Encoding.UTF8.GetBytes(value) : null;

		public static byte[] ToByteArray(byte value) => new byte[] { value };
		public static byte[] ToByteArray(short value) => BitConverter.GetBytes(value);
		public static byte[] ToByteArray(int value) => BitConverter.GetBytes(value);
		public static byte[] ToByteArray(long value) => BitConverter.GetBytes(value);

		public static byte[] ToByteArray(sbyte value) => new byte[] { (byte)value };
		public static byte[] ToByteArray(ushort value) => BitConverter.GetBytes(value);
		public static byte[] ToByteArray(uint value) => BitConverter.GetBytes(value);
		public static byte[] ToByteArray(ulong value) => BitConverter.GetBytes(value);

		public static byte[] ToByteArray(float value) => BitConverter.GetBytes(value);
		public static byte[] ToByteArray(double value) => BitConverter.GetBytes(value);


		public static byte[] ToByteArray(byte[] value) => value;
	}
}
