using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Core.Collections;

namespace Core.Reflection
{
	public class ObjectActivator<T> : IObjectActivator<T>, IObjectActivator
	{
		
		protected Func<T> CtorInvoker;
		protected Func<T, T> CloneInvoker;

		public T DefaultValue { get; } = default(T);
		public Type TargetType { get; } = typeof(T);

		public ObjectActivator()
		{
			CompileDefault();
			CompileCloner();
		}

		protected void CompileDefault()
		{
			ConstructorInfo info = TargetType.GetConstructor(Type.EmptyTypes);
			if (info != null)
			{
				NewExpression expr = Expression.New(info);
				CtorInvoker = Expression.Lambda<Func<T>>(expr).Compile();
			}
		}

		protected void CompileCloner()
		{
			ConstructorInfo info = TargetType.GetConstructor(new Type[] { TargetType });
			if (info != null)
			{
				ParameterExpression par = Expression.Parameter(TargetType, "item");
				NewExpression expr = Expression.New(info, par);
				CloneInvoker = Expression.Lambda<Func<T, T>>(expr, par).Compile();
			}
		}

		public T InvokeConstructor() => CtorInvoker != null ? CtorInvoker() : DefaultValue;
		public T InvokeCloner(T instance) => instance == null || CloneInvoker == null ? default(T) : CloneInvoker(instance);
		public CoreCollection<T> InvokeCollection() => new CoreCollection<T>();


		object IObjectActivator.DefaultValue => DefaultValue;
		object IObjectActivator.InvokeConstructor() => CtorInvoker();
		object IObjectActivator.InvokeCloner(object instance) => instance is T casted ? InvokeCloner(casted) : default(T);
		ICoreCollection IObjectActivator.InvokeCollection() => new CoreCollection<T>();

	}
}
