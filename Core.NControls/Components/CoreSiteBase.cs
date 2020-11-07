using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.NControls.Components
{
	public abstract class CoreSiteBase<TOwner, TComponent> : INestedSite, ISite
		where TOwner : IComponent
		where TComponent : IComponent
	{
		private string _name;

		public TComponent Component { get; }
		public ICoreContainer<TOwner, TComponent> Container { get; }

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


		public CoreSiteBase(ICoreContainer<TOwner, TComponent> container, TComponent component, string name)
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
