using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Collections;

namespace Core.Reflection
{
	public class PropertyKeyCollection : CoreDictionary<string, IPropertyKey>
	{
		#region Constructors

		public PropertyKeyCollection() : 
			base(PropertyKey.NameKey)
		{

		}

		public PropertyKeyCollection(Type type) : this()
		{
			AddRange(PropertyKey.GetPropertyKeys(type));
		}

		public PropertyKeyCollection(PropertyKeyCollection collection) : this()
		{
			AddRange(collection);
		}


		#endregion Constructors
	}
}
