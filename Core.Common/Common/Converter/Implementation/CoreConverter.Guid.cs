using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public static partial class CoreConverter
	{
		public static Guid? ToGuid(object value)
		{
			if (value.IsNullOrDBNull())
				return null;

			switch (value)
			{
				case Guid v:
					return ToGuid(v);
				case string v:
					return ToGuid(v);
				case byte[] v:
					return ToGuid(v);
				default:
					return null;
			}
		}

		public static Guid? ToGuid(Guid value) => value;
		public static Guid? ToGuid(byte[] value) => OutOfRangeBinary(value, 16) ? null : (Guid?)new Guid(value);
		public static Guid? ToGuid(string value)
		{
			try
			{
				return string.IsNullOrEmpty(value) ? (Guid?)new Guid(value) : null;
			}
			catch
			{
				return null;
			}
		}
	}
}
