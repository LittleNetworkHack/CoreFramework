using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Zero.ComponentModel;

namespace Core.Zero.Components
{
	public class CoreComponentSite : CoreBaseSite<CoreComponent>
	{
		public CoreComponentSite(ICoreContainer<CoreComponent> container, CoreComponent component, string name) : 
			base(container, component, name)
		{

		}
	}
}
