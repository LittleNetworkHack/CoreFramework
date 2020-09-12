using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Core.ComponentModel
{
	public class CoreTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => true;
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => true;

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			Type destType = context?.PropertyDescriptor?.PropertyType;
			if (destType != null)
				return CoreConverter.ConvertTo(value, destType);

			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return CoreConverter.ConvertTo(value, destinationType);
		}
	}
}
