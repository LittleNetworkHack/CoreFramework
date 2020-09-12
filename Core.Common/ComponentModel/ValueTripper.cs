using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Core.Reflection;

namespace Core
{
	public abstract class ValueTripper
	{
		protected Type[] typeList;
		protected object[] innerList;

		public object this[int index]
		{
			get => innerList[index].EnsureTypeSafety(typeList[index]);
			set
			{
				value = value.EnsureTypeSafety(typeList[index]);
				innerList[index] = value;
			}
		}

		public int Count { get; }

		protected ValueTripper(int count)
		{
			Count = count;
			innerList = new object[count];
			typeList = GetTypes();
		}

		public object[] GetValues() => innerList.ToArray();

		public Type GetType(int index) => typeList[index];
		public Type[] GetTypes() => GetTypesCore().ToArray();

		protected virtual List<Type> GetTypesCore() => new List<Type>();
	}

	public class ValueTripper<T1> : ValueTripper
	{
		public T1 Item1
		{
			get => this[0] is T1 v ? v : default(T1);
			set => this[0] = value;
		}

		public ValueTripper() : base(1)
		{

		}

		protected ValueTripper(int count) : base(count)
		{

		}

		protected override List<Type> GetTypesCore()
		{
			List<Type> list = base.GetTypesCore();
			list.Add(typeof(T1));
			return list;
		}
	}

	public class ValueTripper<T1, T2> : ValueTripper<T1>
	{
		public T2 Item2
		{
			get => this[1] is T2 v ? v : default(T2);
			set => this[1] = value;
		}

		public ValueTripper() : base(2)
		{

		}

		protected ValueTripper(int count) : base(count)
		{

		}

		protected override List<Type> GetTypesCore()
		{
			List<Type> list = base.GetTypesCore();
			list.Add(typeof(T2));
			return list;
		}
	}

	public class ValueTripper<T1, T2, T3> : ValueTripper<T1, T2>
	{
		public T3 Item3
		{
			get => this[2] is T3 v ? v : default(T3);
			set => this[2] = value;
		}

		public ValueTripper() : base(3)
		{

		}

		protected ValueTripper(int count) : base(count)
		{

		}

		protected override List<Type> GetTypesCore()
		{
			List<Type> list = base.GetTypesCore();
			list.Add(typeof(T3));
			return list;
		}
	}

	public class ValueTripper<T1, T2, T3, T4> : ValueTripper<T1, T2, T3>
	{
		public T4 Item4
		{
			get => this[3] is T4 v ? v : default(T4);
			set => this[3] = value;
		}

		public ValueTripper() : base(4)
		{

		}

		protected ValueTripper(int count) : base(count)
		{

		}

		protected override List<Type> GetTypesCore()
		{
			List<Type> list = base.GetTypesCore();
			list.Add(typeof(T4));
			return list;
		}
	}

	public class ValueTripper<T1, T2, T3, T4, T5> : ValueTripper<T1, T2, T3, T4>
	{
		public T5 Item5
		{
			get => this[4] is T5 v ? v : default(T5);
			set => this[4] = value;
		}

		public ValueTripper() : base(5)
		{

		}

		protected ValueTripper(int count) : base(count)
		{

		}

		protected override List<Type> GetTypesCore()
		{
			List<Type> list = base.GetTypesCore();
			list.Add(typeof(T5));
			return list;
		}
	}

	public class ValueTripper<T1, T2, T3, T4, T5, T6> : ValueTripper<T1, T2, T3, T4, T5>
	{
		public T6 Item6
		{
			get => this[5] is T6 v ? v : default(T6);
			set => this[5] = value;
		}

		public ValueTripper() : base(6)
		{

		}

		protected ValueTripper(int count) : base(count)
		{

		}

		protected override List<Type> GetTypesCore()
		{
			List<Type> list = base.GetTypesCore();
			list.Add(typeof(T6));
			return list;
		}
	}

	public class ValueTripper<T1, T2, T3, T4, T5, T6, T7> : ValueTripper<T1, T2, T3, T4, T5, T6>
	{
		public T7 Item7
		{
			get => this[6] is T7 v ? v : default(T7);
			set => this[6] = value;
		}

		public ValueTripper() : base(7)
		{

		}

		protected ValueTripper(int count) : base(count)
		{

		}

		protected override List<Type> GetTypesCore()
		{
			List<Type> list = base.GetTypesCore();
			list.Add(typeof(T7));
			return list;
		}
	}

	public class ValueTripper<T1, T2, T3, T4, T5, T6, T7, T8> : ValueTripper<T1, T2, T3, T4, T5, T6, T7>
	{
		public T8 Item8
		{
			get => this[7] is T8 v ? v : default(T8);
			set => this[7] = value;
		}

		public ValueTripper() : base(8)
		{

		}

		protected ValueTripper(int count) : base(count)
		{

		}

		protected override List<Type> GetTypesCore()
		{
			List<Type> list = base.GetTypesCore();
			list.Add(typeof(T8));
			return list;
		}
	}
}
