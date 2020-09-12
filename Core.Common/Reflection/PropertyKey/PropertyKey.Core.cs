using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Core.Collections;

namespace Core.Reflection
{
	public class PropertyKey<TClass, TProperty> : 
		IPropertyKey, IPropertyKey<TProperty>, IPropertyKey<TClass, TProperty>,
		IEquatable<IPropertyKey>, IEqualityComparer<TClass>
	{
		#region Fields

		protected PropertyInfo propertyInfo;
		protected PropertyGetter<TClass, TProperty> getter;
		protected PropertySetter<TClass, TProperty> setter;

		#endregion Fields

		#region Properties

		public string Name => propertyInfo.Name;
		public Type ClassType => typeof(TClass);
		public Type PropertyType => typeof(TProperty);
		public PropertyInfo Info => propertyInfo;

		public PropertyGetter<TClass, TProperty> GetMethod => getter;
		public PropertySetter<TClass, TProperty> SetMethod => setter;

		public virtual bool IsReadOnly => setter == null;
		public virtual TProperty DefaultValue => default(TProperty);

		public virtual CoreComparer<TProperty> EqualityComparer => CoreComparer<TProperty>.Comparer;

		#endregion Properties

		#region Constructors

		public PropertyKey(string name)
		{
			propertyInfo = typeof(TClass).GetProperty(name);
			InitializeDelegates();
		}

		public PropertyKey(PropertyInfo info)
		{
			this.propertyInfo = info;
			if (info.GetIndexParameters().Length != 0)
				return;
			InitializeDelegates();
		}

		protected virtual void InitializeDelegates()
		{
			MethodInfo mi_get = Info.GetGetMethod();
			MethodInfo mi_set = Info.GetSetMethod();

			if (mi_get != null)
				getter = (PropertyGetter<TClass, TProperty>)Delegate.CreateDelegate(typeof(PropertyGetter<TClass, TProperty>), mi_get);

			if (mi_set != null)
				setter = (PropertySetter<TClass, TProperty>)Delegate.CreateDelegate(typeof(PropertySetter<TClass, TProperty>), mi_set);
		}

		#endregion Constructors

		#region IPropertyKey

		IEqualityComparer IPropertyKey.EqualityComparer => EqualityComparer;
		object IPropertyKey.DefaultValue => DefaultValue;

		public virtual object GetBoxedValue(object instance) => getter((TClass)instance);

		public virtual void SetBoxedValue(object instance, object value) => setter((TClass)instance, (TProperty)value);

		#endregion IPropertyKey

		#region IPropertyKey<TProperty>

		IEqualityComparer<TProperty> IPropertyKey<TProperty>.EqualityComparer => EqualityComparer;
		TProperty IPropertyKey<TProperty>.DefaultValue => DefaultValue;

		public virtual TProperty GetValue(object instance) => getter((TClass)instance);

		public virtual void SetValue(object instance, TProperty value) => setter((TClass)instance, value);

		#endregion IPropertyKey<TProperty>

		#region IPropertyKey<TClass, TProperty>

		public virtual TProperty GetValue(TClass instance) => getter(instance);

		public virtual void SetValue(TClass instance, TProperty value) => setter(instance, value);

		#endregion IPropertyKey<TClass, TProperty>

		#region IEquatable<IPropertyKey>

		public bool Equals(IPropertyKey other)
		{
			if (other == null)
				return false;

			if (other is PropertyKey<TClass, TProperty> casted)
				return Info == casted.Info;

			return false;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj as IPropertyKey);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		#endregion IEquatable<IPropertyKey>

		#region IEqualityComparer<TClass>

		public bool Equals(TClass x, TClass y)
		{
			if (getter == null)
				return true;

			TProperty vx = GetValue(x);
			TProperty vy = GetValue(y);

			return EqualityComparer.Equals(vx, vy);
		}

		int IEqualityComparer<TClass>.GetHashCode(TClass value)
		{
			return value?.GetHashCode() ?? 0;
		}

		#endregion IEqualityComparer<TClass>

		#region Methods

		public PropertyDescriptor GetDescriptor() => TypeDescriptor.CreateProperty(ClassType, Name, PropertyType);

		public override string ToString()
		{
			return $"PropertyKey<{ClassType.Name}, {PropertyType.Name}>[Name: \"{Name}\"]";
		}

		#endregion Methods
	}
}
