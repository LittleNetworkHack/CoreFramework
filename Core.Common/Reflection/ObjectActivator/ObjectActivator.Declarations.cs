using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Collections;

namespace Core.Reflection
{
	public interface IObjectActivator
	{
		object DefaultValue { get; }
		Type TargetType { get; }

		object InvokeConstructor();
		object InvokeCloner(object instance);
		ICoreCollection InvokeCollection();
	}

	public interface IObjectActivator<T>
	{
		T DefaultValue { get; }
		T InvokeConstructor();
		T InvokeCloner(T instance);
		CoreCollection<T> InvokeCollection();
	}
}
