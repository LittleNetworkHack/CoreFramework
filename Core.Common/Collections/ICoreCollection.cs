using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Collections
{
	public interface ICoreCollection : IList, ICollection, IEnumerable
	{
		Type ItemType { get; }
	}

	public interface ICoreCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>
	{
		Type ItemType { get; }
		void AddRange(IEnumerable<T> items);
	}
}
