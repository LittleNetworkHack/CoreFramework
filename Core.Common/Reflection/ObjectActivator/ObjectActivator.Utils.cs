using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Reflection
{
	public static class ObjectActivator
	{
		#region Cache Object

		private static Dictionary<Type, IObjectActivator> ctorCache = new Dictionary<Type, IObjectActivator>();

		#endregion Cache Object

		#region Create

		private static void CompileNew(Type type)
		{
			Type at = typeof(ObjectActivator<>).MakeGenericType(type);
			ctorCache[type] = (IObjectActivator)Activator.CreateInstance(at);
		}

		public static TCast CreateDirty<TCast>(Type type, params Type[] typeArguments) => (TCast)CreateDirty(type, typeArguments);
		public static object CreateDirty(Type type, params Type[] typeArguments)
		{
			if (typeArguments != null && typeArguments.Length != 0)
				type = type.MakeGenericType(typeArguments);

			if (ctorCache.ContainsKey(type))
				return ctorCache[type].InvokeConstructor();

			return Activator.CreateInstance(type);
		}

		public static T Create<T>(params Type[] typeArguments)
		{
			Type type = typeof(T);
			if (typeArguments != null && typeArguments.Length != 0)
				type = type.MakeGenericType(typeArguments);

			if (ctorCache.ContainsKey(type) == false)
				CompileNew(type);

			return GetActivator<T>().InvokeConstructor();
		}

		public static object Create(Type type, params Type[] typeArguments)
		{
			if (typeArguments != null && typeArguments.Length != 0)
				type = type.MakeGenericType(typeArguments);

			if (ctorCache.ContainsKey(type) == false)
				CompileNew(type);

			return GetActivator(type).InvokeConstructor();
		}

		public static TCast Create<TCast>(Type type, params Type[] typeArguments)
		{
			return (TCast)Create(type, typeArguments);
		}

		#endregion Create

		#region GetActivator

		public static IObjectActivator<T> GetActivator<T>()
		{
			Type type = typeof(T);
			if (ctorCache.ContainsKey(type) == false)
				CompileNew(type);

			return (IObjectActivator<T>)ctorCache[type];
		}

		public static IObjectActivator GetActivator(Type type)
		{
			if (ctorCache.ContainsKey(type) == false)
				CompileNew(type);

			return ctorCache[type];
		}

		#endregion GetActivator

		#region Helpers

		public static object GetDefaultOrNull(Type type)
		{
			return GetActivator(type).DefaultValue;

			//if (type.IsEnum)
			//{
			//	object value = GetActivator(Enum.GetUnderlyingType(type)).InvokeConstructor();
			//	return Enum.ToObject(type, value);
			//}

			//if (type.IsValueType)
			//	return GetActivator(type).InvokeConstructor();

			//return null;
		}

		public static object EnsureTypeSafety(this object value, Type type)
		{
			if (value.IsNullOrDBNull())
				return GetDefaultOrNull(type);

			if (type.IsAssignableFrom(value.GetType()))
				return value;

			return GetDefaultOrNull(type);
		}

		#endregion Helpers
	}
}
