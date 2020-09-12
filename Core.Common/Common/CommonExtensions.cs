using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using Core.ComponentModel;

namespace Core
{
    public static class CommonExtensions
	{
		public static void TryDispose(this object value)
		{
			try
			{
				if (value == null)
					return;

				if (value is IDisposable d)
					d.Dispose();
			}
			catch { }
		}

		public static bool IsNullOrDBNull(this object value) => value == null || value == DBNull.Value;

		public static ServiceHost CreateServiceHost(this Type serviceType, Type endpointType, Uri baseAddress, string address, params Binding[] bindings)
		{
			ServiceHost host = new ServiceHost(serviceType, baseAddress);
			foreach (Binding bind in bindings)
				host.AddServiceEndpoint(endpointType, bind, address);

			return host;
		}

		public static TService GetService<TService>(this IServiceProvider provider)
        {
            return (TService)provider?.GetService(typeof(TService));
        }

		public static T Copy<T>(this T instance) where T : NotifyDecriptorBase
		{
			return (T)instance.Clone();
		}

		public static Dictionary<TEnum, string> GetEnumDescriptions<TEnum>()
			where TEnum : struct
		{
			Type type = typeof(TEnum);
			Dictionary<TEnum, string> result = new Dictionary<TEnum, string>();
			IEnumerable<TEnum> values = Enum.GetValues(type).Cast<TEnum>();

			foreach (TEnum value in values)
			{
				DescriptionAttribute atr = type.GetField(value.ToString())?.GetAttribute<DescriptionAttribute>();
				result.Add(value, atr?.Description ?? value.ToString());
			}

			return result;
		}

		public static Dictionary<object, string> GetEnumDescriptions(Type type)
		{
			Dictionary<object, string> result = new Dictionary<object, string>();
			IEnumerable<object> values = Enum.GetValues(type).Cast<object>();

			foreach (object value in values)
			{
				DescriptionAttribute atr = type.GetField(value.ToString())?.GetAttribute<DescriptionAttribute>();
				result.Add(value, atr?.Description ?? value.ToString());
			}

			return result;
		}

		public static IEnumerable<Atr> GetAttributes<Atr>(this MemberInfo info, bool inherit = false)
			where Atr : Attribute
		{
			return info.GetCustomAttributes(typeof(Atr), inherit).Cast<Atr>();
		}

		public static Atr GetAttribute<Atr>(this MemberInfo info, bool inherit = false)
			where Atr : Attribute
		{
			return info.GetAttributes<Atr>(inherit).FirstOrDefault();
		}

		public static IEnumerable<Atr> GetAttributes<Atr>(this ParameterInfo info, bool inherit = false)
			where Atr : Attribute
		{
			return info.GetCustomAttributes(typeof(Atr), inherit).Cast<Atr>();
		}

		public static Atr GetAttribute<Atr>(this ParameterInfo info, bool inherit = false)
			where Atr : Attribute
		{
			return info.GetAttributes<Atr>(inherit).FirstOrDefault();
		}
	}
}
