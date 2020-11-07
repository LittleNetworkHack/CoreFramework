using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Core.Reflection;

namespace Core.Collections
{
	public static class CoreCollection
	{
		/// <summary>
		/// Vraca razliku izmedu kolekcija gdje se element moze pojavit vise puta
		/// </summary>
		/// <param name="original"></param>
		/// <param name="modified"></param>
		/// <returns></returns>
		public static ICollection Diff(this ICollection original, ICollection modified)
		{
			Dictionary<object, int> counter = new Dictionary<object, int>();
			// Prebrojavamo originalne
			foreach (object item in original)
			{
				if (counter.ContainsKey(item))
					counter[item] += 1;
				else
					counter[item] = 1;
			}

			List<object> result = new List<object>();
			// Oduzimamo koji su ostali
			foreach (object item in modified)
			{
				// Ako postoji smanjujemo brojac, kad padne na 0 remove iz dictionary
				if (counter.TryGetValue(item, out int c))
				{
					if (c == 1)
						counter.Remove(item);
					else
						counter[item] = c - 1;
				}
				else
				{
					// Nemamo takav item u originalu
					result.Add(item);
				}
			}

			return result;
		}

		/// <summary>
		/// Vraca razliku izmedu kolekcija gdje se element moze ponovit samo jednom
		/// </summary>
		/// <param name="original"></param>
		/// <param name="modified"></param>
		/// <returns></returns>
		public static ICollection DiffDistinct(this ICollection original, ICollection modified)
		{
			HashSet<object> set = new HashSet<object>();
			foreach (object item in original)
				set.Add(item);

			foreach (object item in modified)
				set.Add(item);

			return set.ToList();
		}

		public static CoreCollection<T> ToCollection<T>(this IEnumerable<T> items) => new CoreCollection<T>(items);

		public static DataTable ToDataTable<T>(this ICoreCollection<T> items)
		{
			DataTable table = new DataTable();
			PropertyKeyCollection keys = typeof(T).GetPropertyKeys();

			foreach (IPropertyKey key in keys)
				table.Columns.Add(key.Name, Nullable.GetUnderlyingType(key.PropertyType) ?? key.PropertyType);

			foreach (T item in items)
			{
				DataRow row = table.NewRow();
				foreach (IPropertyKey key in keys)
					row[key.Name] = key.GetBoxedValue(item) ?? DBNull.Value;
				table.Rows.Add(row);
			}

			return table;
		}
	}
}

