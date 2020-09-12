using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Reflection
{
	public delegate void PropertySetter<TClass, TMember>(TClass instance, TMember value);
	public delegate TMember PropertyGetter<TClass, TMember>(TClass instance);

	public interface IPropertyKey
	{
		string Name { get; }
		Type ClassType { get; }
		Type PropertyType { get; }
		PropertyInfo Info { get; }
		
		bool IsReadOnly { get; }
		IEqualityComparer EqualityComparer { get; }
		object DefaultValue { get; }

		object GetBoxedValue(object instance);
		void SetBoxedValue(object instance, object value);
	}

	public interface IPropertyKey<TProperty> : IPropertyKey
	{
		new IEqualityComparer<TProperty> EqualityComparer { get; }
		new TProperty DefaultValue { get; }

		TProperty GetValue(object instance);
		void SetValue(object instance, TProperty value);
	}

	public interface IPropertyKey<TClass, TProperty> : IPropertyKey<TProperty>, IPropertyKey
	{
		TProperty GetValue(TClass instance);
		void SetValue(TClass instance, TProperty value);
	}
}
