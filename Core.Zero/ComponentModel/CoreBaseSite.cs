using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zero.ComponentModel
{
	public abstract class CoreBaseSite<TComponent> : CoreBaseSite<TComponent, TComponent>
		where TComponent : ICoreComponent
	{
		protected CoreBaseSite(ICoreContainer<TComponent> container, TComponent component, string name) : base(container, component, name)
		{
		}
	}

	public abstract class CoreBaseSite<TOwner, TComponent> : ICoreSite, INestedSite, ISite
		where TOwner : ICoreComponent
		where TComponent : ICoreComponent
	{
		private string _name;

		public TComponent Component { get; }
		public ICoreContainer<TOwner, TComponent> Container { get; }

		public ICoreComponent Owner => Container.Owner;
		public string FullName => _name != null && Container.OwnerName != null ? $"{Container.OwnerName}.{_name}" : _name;
		public bool DesignMode => Container.Owner?.Site?.DesignMode ?? false;
		public string Name
		{
			get => _name;
			set
			{
				if (_name == value)
					return;

				Container.ValidateName(Component, value);
				_name = value;
			}
		}

		protected CoreBaseSite(ICoreContainer<TOwner, TComponent> container, TComponent component, string name)
		{
			Container = container;
			Component = component;
			_name = name;
		}

		public virtual object GetService(Type serviceType)
		{
			if (serviceType == typeof(ISite) || serviceType == typeof(INestedSite))
				return this;

			return Container.GetService(serviceType);
		}

		#region ISite

		IComponent ISite.Component => Component;
		IContainer ISite.Container => Container;

		#endregion ISite
	}
}
