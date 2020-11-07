using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.NControls.Components
{
	public class EventHandlerStore : IDisposable
	{
		#region Fields

		private Dictionary<object, Delegate> list;

		#endregion Fields

		#region Properties

		public IComponent Parent { get; }

		#endregion Properties

		#region Constructors

		public EventHandlerStore() : this(null)
		{

		}

		public EventHandlerStore(IComponent parent)
		{
			Parent = parent;
			list = new Dictionary<object, Delegate>();
		}

		#endregion Constructors

		#region Methods

		public void Add(object key, Delegate value)
		{
			Delegate result = list.TryGetValue(key, out Delegate handler) ? Delegate.Combine(handler, value) : value;
			list[key] = result;
		}

		public void Remove(object key, Delegate value)
		{
			Delegate handler = list.TryGetValue(key, out handler) ? Delegate.Remove(handler, value) : null;
			if (handler == null)
				list.Remove(key);
			else
				list[key] = handler;
		}

		public EventHandler Find(object key) => Find<EventHandler>(key);

		public TDelegate Find<TDelegate>(object key)
			where TDelegate : Delegate
		{
			return (TDelegate)(list.TryGetValue(key, out Delegate handler) ? handler : null);
		}

		public bool Find<TDelegate>(object key, out TDelegate value)
			where TDelegate : Delegate
		{
			bool found = list.TryGetValue(key, out Delegate handler);
			value = (TDelegate)handler;
			return found && value != null;
		}

		public void Dispose()
		{
			list = null;
		}

		#endregion Methods
	}
}
