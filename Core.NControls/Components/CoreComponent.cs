using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.NControls.Components
{
	public class CoreComponent : IComponent, IDisposable
	{
		#region Fields

		private static readonly object EventDisposed = new object();

		#endregion Fields

		#region Properties

		protected EventHandlerStore EventStore { get; }

		public ISite Site { get; set; }

		public IContainer Container => Site?.Container;

		#endregion Properties

		#region Constructors

		public CoreComponent()
		{
			EventStore = new EventHandlerStore();
		}

		public CoreComponent(IContainer container) : this()
		{
			container.Add(this);
		}

		#endregion Constructors

		#region Methods

		public override string ToString()
		{
			string name = Site?.Name;
			if (!string.IsNullOrEmpty(name))
				return $"{name} [{GetType().FullName}]";

			return GetType().FullName;
		}

		#endregion Methods

		#region IDisposable

		public event EventHandler Disposed
		{
			add => EventStore.Add(EventDisposed, value);
			remove => EventStore.Remove(EventDisposed, value);
		}

		~CoreComponent()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
				return;

			lock (this)
			{
				Site?.Container?.Remove(this);
				EventStore.Find(EventDisposed)?.Invoke(this, EventArgs.Empty);
			}
		}

		#endregion IDisposable
	}
}
