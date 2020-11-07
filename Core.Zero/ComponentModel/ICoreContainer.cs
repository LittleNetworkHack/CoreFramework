using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zero.ComponentModel
{
	public interface ICoreContainer<TComponent> : ICoreContainer<TComponent, TComponent>
		where TComponent : ICoreComponent
	{

	}

	public interface ICoreContainer<TOwner, TComponent> : IContainer, IEnumerable<TComponent>, INestedContainer, IServiceProvider
		where TOwner : ICoreComponent
		where TComponent : ICoreComponent
	{
		new TOwner Owner { get; }
		string OwnerName { get; }
		void Add(TComponent component);
		void Add(TComponent component, string name);
		void Remove(TComponent component);
		void ValidateName(TComponent component, string name);
	}
}
