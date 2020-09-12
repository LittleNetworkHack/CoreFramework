using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Core.Collections;
using Core.Reflection;

namespace Core.ComponentModel
{
	public class CorePropertyDescriptorCollection : CoreDictionary<string, CorePropertyDescriptor>
	{
		#region Constructors

		public CorePropertyDescriptorCollection() :
			base(nameof(IPropertyKey.Name))
		{

		}

		public CorePropertyDescriptorCollection(Type type) :
			base(nameof(IPropertyKey.Name))
		{
			foreach (IPropertyKey key in PropertyKey.GetPropertyKeys(type))
				Add(new CorePropertyDescriptor(key));
		}

		#endregion Constructors

		#region Methods

		public static explicit operator PropertyDescriptorCollection(CorePropertyDescriptorCollection collection)
		{
			return new PropertyDescriptorCollection(collection.ToArray());
		}

		#endregion Methods
	}
}
