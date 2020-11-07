using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Zero.ComponentModel;

namespace Core.Zero.Components
{
	public class CoreComponent : ICoreComponent, IComponent, IDisposable
	{
		#region Fields

		private static readonly object EventDisposed = new object();

		#endregion Fields

		#region Properties

		protected EventHandlerStore EventStore { get; }

		public ISite Site { get; set; }
		public ICoreSite CoreSite
		{
			get => Site as ICoreSite;
			set => Site = value;
		}

		public IContainer Container => Site?.Container;

		public CoreComponentCollection Components { get; }

		#endregion Properties

		#region Constructors

		public CoreComponent()
		{
			EventStore = new EventHandlerStore();
			Components = new CoreComponentCollection(this);
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
