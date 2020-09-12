using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Core.Reflection;

namespace Core.ComponentModel
{
	public class CorePropertyDescriptor : PropertyDescriptor, IKeyOwner
	{
		#region Fields

		#endregion Fields

		#region Properties

		public IPropertyKey Key { get; protected set; }
		public override bool IsReadOnly => Key.IsReadOnly;
		public override Type ComponentType => Key.ClassType;
		public override Type PropertyType => Key.PropertyType;

		#endregion Properties

		#region Constructors

		public CorePropertyDescriptor(IPropertyKey key) :
			base(key.Name, ReflectAttributes(key.Info))
		{
			Key = key;
		}

		#endregion Constructors

		#region Core Methods

		protected static Attribute[] ReflectAttributes(PropertyInfo info)
		{
			return info.GetCustomAttributes(true).OfType<Attribute>().ToArray();
		}

		#endregion Core Methods

		#region Override

		public override bool CanResetValue(object component)
		{
			return true;
		}

		public override object GetValue(object component)
		{
			return Key.GetBoxedValue(component);
		}

		public override void ResetValue(object component)
		{
			Key.SetBoxedValue(component, Key.DefaultValue);
		}

		public override void SetValue(object component, object value)
		{
			Key.SetBoxedValue(component, value);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		#endregion Override

	}
}
