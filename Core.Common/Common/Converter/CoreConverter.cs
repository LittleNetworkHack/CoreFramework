using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using Core.Reflection;

namespace Core
{
	public static partial class CoreConverter
	{
		private static Type[] primitives = new Type[]
		{
			typeof(bool),
			typeof(char),
			typeof(string),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(byte),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(sbyte),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(byte[]),
			typeof(DateTime),
			typeof(Guid)
		};

		public static bool IsPrimitive(Type type) => type.IsEnum || primitives.Contains(type) || primitives.Contains(Nullable.GetUnderlyingType(type));

		public static T ConvertTo<T>(object value) => (T)ConvertTo(value, typeof(T));

		public static object ConvertTo(object value, Type destType)
		{
			object defVal = ObjectActivator.GetDefaultOrNull(destType);
			if (value.IsNullOrDBNull())
				return defVal;

			Type srcType = value.GetType();
			if (srcType == destType)
				return value;

			destType = Nullable.GetUnderlyingType(destType) ?? destType;

			if (destType.IsEnum)
				value = ToEnum(value, destType);
			else if (destType == typeof(bool))
				value = ToBoolean(value);
			else if (destType == typeof(char))
				value = ToChar(value);
			else if (destType == typeof(string))
				value = ToString(value);

			else if (destType == typeof(float))
				value = ToSingle(value);
			else if (destType == typeof(double))
				value = ToDouble(value);
			else if (destType == typeof(decimal))
				value = ToDecimal(value);

			else if (destType == typeof(byte))
				value = ToByte(value);
			else if (destType == typeof(short))
				value = ToInt16(value);
			else if (destType == typeof(int))
				value = ToInt32(value);
			else if (destType == typeof(long))
				value = ToInt64(value);

			else if (destType == typeof(sbyte))
				value = ToSByte(value);
			else if (destType == typeof(ushort))
				value = ToUInt16(value);
			else if (destType == typeof(uint))
				value = ToUInt32(value);
			else if (destType == typeof(ulong))
				value = ToUInt64(value);

			else if (destType == typeof(byte[]))
				value = ToByteArray(value);
			else if (destType == typeof(DateTime))
				value = ToDateTime(value);
			//else if (destType == typeof(TimeSpan))
			//	value = ToUInt32(value);
			else if (destType == typeof(Guid))
				value = ToGuid(value);

			return value ?? defVal;
		}

		#region OutOfRange

		public static bool OutOfRangeFloat(float value, float min, float max) => value < min || value > max;
		public static bool OutOfRangeDouble(double value, double min, double max) => value < min || value > max;
		public static bool OutOfRangeDecimal(decimal value, decimal min, decimal max) => value < min || value > max;
		public static bool OutOfRangeBinary(byte[] value, int size)
		{
			if (value == null)
				return true;

			if (size == 0)
				return false;

			return value.Length != size;
		}

		#endregion OutOfRange

		public static void TestConversion()
		{
			List<object> list = new List<object>()
			{
				null,
				DBNull.Value,
				new byte[] {},
				new byte[] {1, 2},
				true,
				false,
				"true",
				"false",
				"True",
				"False",
				0,
				1,
				byte.MinValue,
				(byte)5,
				byte.MaxValue,
				char.MinValue,
				(char)5,
				char.MaxValue,
				DateTime.MinValue,
				DateTime.MaxValue,
				decimal.MinValue,
				(decimal)5.56,
				decimal.MaxValue,
				double.MinValue,
				(double)5.56,
				double.MaxValue,
				FileOptions.SequentialScan,
				(FileOptions)(-2),
				Guid.Empty,
				Guid.NewGuid(),
				"1127925e-685a-4275-86ca-64f249dd5aba",
				short.MinValue,
				(short)5,
				short.MaxValue,
				int.MinValue,
				(int)5,
				int.MaxValue,
				long.MinValue,
				(char)5,
				long.MaxValue,
				sbyte.MinValue,
				(sbyte)5,
				sbyte.MaxValue,
				ushort.MinValue,
				(ushort)5,
				ushort.MaxValue,
				uint.MinValue,
				(uint)5,
				uint.MaxValue,
				ulong.MinValue,
				(ulong)5,
				ulong.MaxValue,
				float.MinValue,
				(float)5.56,
				float.MaxValue,
				"asdf",
				"",
				"123",
				"123.456"
			};

			foreach (Type p in primitives)
			{
				foreach (object value in list)
				{
					try
					{
						object result = ConvertTo(value, p);
						Console.WriteLine($"VALUE: {value ?? "NULL"}, RESULT: {result ?? "NULL"}, TYPE: {value?.GetType().Name ?? "NULL"}");
					}
					catch
					{
						Console.WriteLine($"FAIL! VALUE: {value ?? "NULL"}, TYPE: {value?.GetType().Name ?? "NULL"}");
					}
				}
			}
		}
	}
}
