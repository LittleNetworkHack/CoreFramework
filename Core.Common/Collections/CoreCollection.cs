using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ComponentModel;
using Core.Reflection;

namespace Core.Collections
{
	public class CoreCollection<T> : NotifyDecriptorBase,
	 	ICoreCollection<T>, ICoreCollection,
		IList<T>, IList,
		ICollection<T>, ICollection,
		IEnumerable<T>, IEnumerable
	{
		#region Fields

		protected int addNewPosition;
		protected bool isNullable;
		protected bool? canCreateNew;

		protected bool _IsReadOnly = false;
		protected bool _AllowNull = false;
		protected List<T> Items { get; }

		#endregion Fields

		#region Properties

		public Type ItemType => typeof(T);
		public int Count => Items.Count;
		public virtual IEqualityComparer<T> EqualityComparer { get; }

		public virtual bool IsReadOnly => IsLocked();

		public bool AllowNull
		{
			get => isNullable ? _AllowNull : false;
			set => SetValue(ref _AllowNull, value, nameof(AllowNull));
		}

		#endregion Properties

		#region Constructors

		public CoreCollection()
		{
			isNullable = default(T) == null;
			Items = new List<T>();
			EqualityComparer = EqualityComparer<T>.Default;
		}

		public CoreCollection(IEnumerable<T> items)
		{
			isNullable = default(T) == null;
			Items = new List<T>(items);
			EqualityComparer = EqualityComparer<T>.Default;
		}

		#endregion Constructors

		#region Throw Helpers

		protected virtual void ThrowIfInvalidConstructor()
		{
			if (canCreateNew == null)
			{
				try
				{
					CreateNewItem();
					canCreateNew = true;
				}
				catch
				{
					canCreateNew = false;
				}
			}

			if (canCreateNew == true)
				return;

			throw new InvalidOperationException("Cannot create 'new()' item.");
		}

		protected void ThrowIfReadOnly()
		{
			if (IsReadOnly)
				throw new InvalidOperationException("Collection is readonly!");
		}

		protected void ThrowIfInvalidValue(object item)
		{
			if (item == null && !isNullable)
				throw new ArgumentNullException("Item cannot be null.");

			if (item != null && item is T == false)
				throw new ArgumentException($"Item not supported by collection. Must be of type '{typeof(T).FullName}'.");

			if (!AllowNull && item == null)
				throw new ArgumentNullException("Item cannot be null.");

			ThrowIfInvalidValue((T)item);
		}

		protected virtual void ThrowIfInvalidValue(T item)
		{
			if (!AllowNull && item == null)
				throw new ArgumentNullException("Item cannot be null.");
		}

		protected void ThrowIfInvalidValues(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				if (!AllowNull && item == null)
					throw new ArgumentNullException("Item cannot be null.");

				ThrowIfInvalidValue(item);
			}
		}

		protected void ThrowIfInvalidIndex(int index, bool inclusive)
		{
			if (index < 0 || index > Count || (inclusive && index == Count))
				throw new ArgumentOutOfRangeException("Index out of range!");
		}

		#endregion Throw Helpers

		#region Validate Operations

		protected virtual void ValidateGetItem(int index)
		{
			ThrowIfInvalidIndex(index, false);
		}

		protected virtual void ValidateSetItem(int index, T item)
		{
			ThrowIfReadOnly();
			ThrowIfInvalidIndex(index, false);
			ThrowIfInvalidValue(item);
		}

		protected virtual void ValidateCreateNewItem()
		{
			ThrowIfInvalidConstructor();
		}

		protected virtual void ValidateInsertItem(T item, int index)
		{
			ThrowIfReadOnly();
			ThrowIfInvalidIndex(index, false);
			ThrowIfInvalidValue(item);
		}

		protected virtual void ValidateInsertItemRange(int index, IEnumerable<T> items)
		{
			ThrowIfReadOnly();
			ThrowIfInvalidIndex(index, false);
			ThrowIfInvalidValues(items);
		}

		protected virtual void ValidateRemoveItem(int index)
		{
			ThrowIfReadOnly();
			ThrowIfInvalidIndex(index, false);
		}

		protected virtual void ValidateClearItems()
		{
			ThrowIfReadOnly();
		}

		#endregion Validate Operations

		#region Core Methods

		protected virtual T CreateNewItem()
		{
			return ObjectActivator.Create<T>();
		}

		protected virtual void InsertItemRange(int index, IEnumerable<T> items)
		{
			Items.InsertRange(index, items);
		}

		protected virtual void InsertItem(int index, T item)
		{
			Items.Insert(index, item);
		}

		protected virtual void RemoveItem(int index)
		{
			Items.RemoveAt(index);
		}

		protected virtual T GetItem(int index)
		{
			return Items[index];
		}

		protected virtual void SetItem(int index, T item)
		{
			Items[index] = item;
		}

		protected virtual void ClearItems()
		{
			Items.Clear();
		}

		public override object Clone() => throw new InvalidOperationException();

		#endregion Core Methods

		#region Public Methods

		public T this[int index]
		{
			get => IndexGet(index);
			set => IndexSet(index, value);
		}

		public T IndexGet(int index)
		{
			ValidateGetItem(index);
			return GetItem(index);
		}

		public void IndexSet(int index, T item)
		{
			ValidateSetItem(index, item);
			SetItem(index, item);
		}

		public T CreateNew()
		{
			ValidateCreateNewItem();
			return CreateNewItem();
		}

		public T AddNew()
		{
			T item = CreateNew();
			Add(item);
			return item;
		}

		public int Add(T item)
		{
			int index = Count;
			InsertItem(index, item);
			return index;
		}

		public void AddRange(IEnumerable<T> items) => InsertItemRange(0, items);

		public void AddRange(params T[] items) => AddRange((IEnumerable<T>)items);

		public void Insert(int index, T item)
		{
			ValidateInsertItem(item, index);
			InsertItem(index, item);
		}

		public void InsertRange(int index, IEnumerable<T> items)
		{
			ValidateInsertItemRange(index, items);
			InsertItemRange(index, items);
		}

		public void InsertRange(int index, params T[] items) => InsertRange(index, (IEnumerable<T>)items);

		public bool Remove(T item)
		{
			int index = IndexOf(item);
			if (index == -1)
				return false;
			RemoveItem(index);
			return true;
		}

		public void RemoveAt(int index)
		{
			ValidateRemoveItem(index);
			RemoveItem(index);
		}

		public void Clear()
		{
			ValidateClearItems();
			ClearItems();
		}

		public bool Contains(T item)
		{
			for (int i = 0; i < Count; i++)
			{
				if (EqualityComparer.Equals(item, Items[i]))
					return true;
			}
			return false;
		}

		public int IndexOf(T item)
		{
			for (int i = 0; i < Count; i++)
			{
				if (EqualityComparer.Equals(item, Items[i]))
					return i;
			}
			return -1;
		}

		public override string ToString()
		{
			return $"CoreCollection<{typeof(T).Name}>[Count: {Count}]";
		}

		#endregion Public Methods

		#region Core Search

		public int FindIndex<TProperty>(IPropertyKey<T, TProperty> key, TProperty value)
		{
			IEqualityComparer<TProperty> comparer = CoreComparer<TProperty>.Comparer;
			for (int i = 0; i < Count; i++)
			{
				T item = Items[i];
				if (comparer.Equals(key.GetValue(item), value))
					return i;
			}

			return -1;
		}

		public T Find<TProperty>(IPropertyKey<T, TProperty> key, TProperty value)
		{
			IEqualityComparer<TProperty> comparer = CoreComparer<TProperty>.Comparer;
			for (int i = 0; i < Count; i++)
			{
				T item = Items[i];
				if (comparer.Equals(key.GetValue(item), value))
					return item;
			}
			return default(T);
		}

		public IEnumerable<T> FindAll<TProperty>(IPropertyKey<T, TProperty> key, TProperty value)
		{
			IEqualityComparer<TProperty> comparer = CoreComparer<TProperty>.Comparer;
			for (int i = 0; i < Count; i++)
			{
				T item = Items[i];
				if (comparer.Equals(key.GetValue(item), value))
					yield return item;
			}

			yield break;
		}

		#endregion Core Search

		#region IEnumerable

		public virtual IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

		#endregion IEnumerable

		#region ICollection

		int ICollection.Count => Count;
		bool ICollection.IsSynchronized => false;
		object ICollection.SyncRoot => (Items as ICollection).SyncRoot;
		void ICollection.CopyTo(Array array, int index) => (Items as ICollection).CopyTo(array, index);

		int ICollection<T>.Count => Count;
		bool ICollection<T>.IsReadOnly => IsReadOnly;
		void ICollection<T>.Add(T item) => Add(item);
		bool ICollection<T>.Remove(T item) => Remove(item);
		bool ICollection<T>.Contains(T item) => Contains(item);
		void ICollection<T>.Clear() => Clear();
		void ICollection<T>.CopyTo(T[] array, int arrayIndex) => (Items as ICollection<T>).CopyTo(array, arrayIndex);

		#endregion ICollection

		#region IList

		bool IList.IsReadOnly => IsReadOnly;
		bool IList.IsFixedSize => false;
		object IList.this[int index]
		{
			get => IndexGet(index);
			set => IndexSet(index, (T)value);
		}
		int IList.Add(object value) => Add((T)value);
		bool IList.Contains(object value) => Contains((T)value);
		void IList.Clear() => Clear();
		int IList.IndexOf(object value) => IndexOf((T)value);
		void IList.Insert(int index, object value) => Insert(index, (T)value);
		void IList.Remove(object value) => Remove((T)value);
		void IList.RemoveAt(int index) => RemoveAt(index);

		T IList<T>.this[int index]
		{
			get => IndexGet(index);
			set => IndexSet(index, value);
		}
		int IList<T>.IndexOf(T item) => IndexOf(item);
		void IList<T>.Insert(int index, T item) => Insert(index, item);
		void IList<T>.RemoveAt(int index) => RemoveAt(index);

		#endregion IList
	}
}
