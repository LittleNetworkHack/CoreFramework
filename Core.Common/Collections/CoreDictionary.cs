using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Reflection;

namespace Core.Collections
{
	public class CoreDictionary<TKey, TValue> : CoreCollection<TValue>
	{
		#region Properties

		protected Dictionary<TKey, TValue> Items2 { get; private set; }
		protected virtual IPropertyKey<TValue, TKey> KeyResolver { get; set; }

		public IEnumerable<TKey> Keys => Items2.Keys;

		#endregion Properties

		#region Constructors

		public CoreDictionary(string propertyName)
		{
			isNullable = false;
			Items2 = new Dictionary<TKey, TValue>();
			KeyResolver = PropertyKey.GetPropertyKey<TValue, TKey>(propertyName);
		}

		public CoreDictionary(IPropertyKey<TValue, TKey> keyResolver)
		{
			isNullable = false;
			Items2 = new Dictionary<TKey, TValue>();
			KeyResolver = keyResolver;
		}

		#endregion Constructors

		#region Core Methods

		protected override void InsertItemRange(int index, IEnumerable<TValue> items)
		{
			int transIndex = index;
			int transCount = 0;
			try
			{
				foreach (TValue item in items)
				{
					TKey key = KeyResolver.GetValue(item);
					if (key != null)
						Items2.Add(key, item);
					Items.Insert(index, item);
					transCount++;
					index++;
				}
			}
			catch
			{
				for (int i = 0; i < transCount; i++)
					Items.RemoveAt(transIndex);

				throw;
			}
		}

		protected override void InsertItem(int index, TValue item)
		{
			TKey key = KeyResolver.GetValue(item);
			if (key != null)
				Items2.Add(key, item);
			Items.Insert(index, item);
		}

		protected override void RemoveItem(int index)
		{
			TKey key = KeyResolver.GetValue(base[index]);
			if (key != null)
				Items2.Remove(key);
			Items.RemoveAt(index);
		}

		protected override void SetItem(int index, TValue item)
		{
			TKey key = KeyResolver.GetValue(item);
			if (key != null)
				Items2[key] = item;
			Items[index] = item;
		}

		protected override void ClearItems()
		{
			Items2.Clear();
			Items.Clear();
			base.ClearItems();
		}

		#endregion Core Methods

		#region Public Methods

		public TValue this[TKey key]
		{
			get => Items2[key];
		}

		public int GetIndexFromKey(TKey key) => Items.IndexOf(Items2[key]);
		public bool ContainsKey(TKey key) => Items2.ContainsKey(key);

		public bool RemoveKey(TKey key)
		{
			if (Items2.ContainsKey(key))
			{
				TValue item = Items2[key];
				Items.Remove(item);
				Items2.Remove(key);
				return true;
			}

			return Items2.Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value) => Items2.TryGetValue(key, out value);

		#endregion Public Methods
	}
}
