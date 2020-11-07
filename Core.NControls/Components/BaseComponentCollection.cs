using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Core.Collections;
using Core.Reflection;

namespace Core.NControls.Components
{
	public interface ICoreContainer<TOwner, TComponent> : IContainer, IEnumerable<TComponent>, INestedContainer, IServiceProvider
		where TOwner : IComponent
		where TComponent : IComponent
	{
		new TOwner Owner { get; }
		string OwnerName { get; }
		void Add(TComponent component);
		void Add(TComponent component, string name);
		void Remove(TComponent component);
		void ValidateName(TComponent component, string name);
	}

	public abstract class CoreContainerBase<TOwner, TComponent, TSite> : ICoreContainer<TOwner, TComponent>
		where TOwner : IComponent
		where TComponent : IComponent
		where TSite : CoreSiteBase<TOwner, TComponent>
	{
		protected readonly object syncObj = new object();
		protected CoreDictionary<string, TSite> store = new CoreDictionary<string, TSite>("Name");

		public TOwner Owner { get; }
		public string OwnerName => Owner.Site?.Name;

		protected CoreContainerBase(TOwner owner)
		{
			Owner = owner;
		}

		#region Methods

		public void Add(TComponent component) => Add(component, component.Site?.Name);

		public virtual void Add(TComponent component, string name)
		{
			lock (syncObj)
			{
				ISite old = component?.Site;
				if (component == null || old?.Container == this)
					return;

				ValidateName(component, name);
				old?.Container.Remove(component);
				TSite site = CreateSite(component, name);
				component.Site = site;
				store.Add(site);
				ClearCache();
			}
		}

		public virtual void Remove(TComponent component)
		{
			lock (syncObj)
			{
				TSite site = component?.Site as TSite;
				if (component == null || site?.Container != this)
					return;

				component.Site = null;
				store.Remove(site);
				ClearCache();
			}
		}

		protected abstract TSite CreateSite(TComponent component, string name);

		public void ValidateName(TComponent component, string name)
		{
			if (component == null)
				throw new ArgumentNullException(nameof(component));

			if (name == null)
				return;

			if (!store.TryGetValue(name, out TSite value))
				return;

			if (ReferenceEquals(component, value.Component))
				return;

			throw new ArgumentException($"Duplicate component name! Value: '{name}'");
		}

		protected void ClearCache() => componentsCache = null;

		public object GetService(Type service)
		{
			if (service == typeof(INestedContainer) || service == typeof(IContainer))
				return this;

			return (Owner as IServiceProvider)?.GetService(service);
		}

		public IEnumerator<TComponent> GetEnumerator()
		{
			foreach (TSite site in store)
				yield return site.Component;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			store = null;
		}

		#endregion Methods

		#region INestedContainer

		IComponent INestedContainer.Owner => Owner;

		void IContainer.Add(IComponent component) => Add((TComponent)component);

		void IContainer.Add(IComponent component, string name) => Add((TComponent)component, name);

		void IContainer.Remove(IComponent component) => Remove((TComponent)component);

		private ComponentCollection componentsCache = null;
		ComponentCollection IContainer.Components
		{
			get
			{
				if (componentsCache == null)
					componentsCache = new ComponentCollection(store.Cast<IComponent>().ToArray());

				return componentsCache;
			}
		}

		#endregion INestedContainer

		#region Enumerator

		private class SiteEnumerator
		{

		}

		#endregion Enumerator
	}
}
