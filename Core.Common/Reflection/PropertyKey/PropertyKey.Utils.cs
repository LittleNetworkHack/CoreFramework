using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Core.Collections;

namespace Core.Reflection
{
	public static class PropertyKey
	{
		#region Fields

		private static Dictionary<Type, PropertyKeyCollection> Inventory = new Dictionary<Type, PropertyKeyCollection>();

		public static readonly Type GenericType = typeof(PropertyKey<,>);
		public static readonly PropertyKey<IPropertyKey, string> NameKey = new PropertyKey<IPropertyKey, string>("Name");

		#endregion Fields

		#region Create Key

		public static IPropertyKey Create<TClass>(string name)
		{
			PropertyInfo info = typeof(TClass).GetProperty(name);
			if (info == null)
				return null;

			return (IPropertyKey)Activator.CreateInstance(GenericType.MakeGenericType(typeof(TClass), info.PropertyType), info);
		}

		public static IPropertyKey Create(Type classType, string name)
		{
			if (classType == null)
				throw new ArgumentNullException(nameof(classType));
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			PropertyInfo info = classType.GetProperty(name);
			return Create(info);
		}

		public static IPropertyKey Create(PropertyInfo info)
		{
			if (info == null)
				throw new ArgumentNullException(nameof(info));

			Type type = GenericType.MakeGenericType(info.ReflectedType, info.PropertyType);
			return (IPropertyKey)Activator.CreateInstance(type, info);
		}

		#endregion Create Key

		#region Get Properties

		public static PropertyKeyCollection GetPropertyKeys(this Type type)
		{
			if (Inventory.ContainsKey(type))
				return new PropertyKeyCollection(Inventory[type]);

			PropertyKeyCollection properties = new PropertyKeyCollection();
			foreach (PropertyInfo info in type.GetProperties().OrderBy(I => I.Name))
				properties.Add(Create(info));
				

			Inventory[type] = properties;
			return new PropertyKeyCollection(properties);
		}

		public static PropertyKeyCollection GetPropertyKeys<TClass>() 
			=> GetPropertyKeys(typeof(TClass));

		public static IPropertyKey GetPropertyKey<TClass>(string name) 
			=> typeof(TClass).GetPropertyKeys()[name];

		public static IPropertyKey GetPropertyKey(this Type type, string name) 
			=> type.GetPropertyKeys()[name];

		public static IPropertyKey<TProperty> GetPropertyKey<TProperty>(this Type type, string name) 
			=> (IPropertyKey<TProperty>)type.GetPropertyKeys()[name];

		public static PropertyKey<TClass, TProperty> GetPropertyKey<TClass, TProperty>(string name) 
			=> (PropertyKey<TClass, TProperty>)typeof(TClass).GetPropertyKeys()[name];

		#endregion Get Properties

		#region Filters

		public static IEnumerable<IPropertyKey<TProperty>> WherePropertyType<TProperty>(this IEnumerable<IPropertyKey> properties)
		{
			foreach (IPropertyKey key in properties)
			{
				if (key is IPropertyKey<TProperty> casted)
					yield return casted;
			}
		}

		#endregion Filters
	}
}
