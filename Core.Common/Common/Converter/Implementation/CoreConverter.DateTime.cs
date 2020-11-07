using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static DateTime? ToDateTime(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				//case bool v:
				//	return ToUInt64(v);
				//case char v:
				//	return ToUInt64(v);
				case string v:
					return ToDateTime(v);

				//case float v:
				//	return ToUInt64(v);
				//case double v:
				//	return ToUInt64(v);
				//case decimal v:
				//	return ToUInt64(v);

				//case byte v:
				//	return ToUInt64(v);
				//case short v:
				//	return ToUInt64(v);
				//case int v:
				//	return ToUInt64(v);
				case long v:
					return ToDateTime(v);

				//case sbyte v:
				//	return ToUInt64(v);
				//case ushort v:
				//	return ToUInt64(v);
				//case uint v:
				//	return ToUInt64(v);
				//case ulong v:
				//	return ToUInt64(v);

				//case byte[] v:
				//	return ToUInt64(v);
				case DateTime v:
					return ToDateTime(v);
				//case TimeSpan v:
				//	return ToUInt64(v);
				//case XmlDocument v:
				//	return ToUInt64(v);
				//case Guid v:
				//	return ToUInt64(v);
				default:
					return TryCast<DateTime>(value);
			}
		}

		public static DateTime? ToDateTime(DateTime value) => value;
		public static DateTime? ToDateTime(string value) => DateTime.TryParse(value, out DateTime result) ? (DateTime?)result : null;

		public static DateTime? ToDateTime(long value) => new DateTime(value);
	}
}
