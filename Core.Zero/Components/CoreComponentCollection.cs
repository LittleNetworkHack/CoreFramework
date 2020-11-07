using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Zero.ComponentModel;

namespace Core.Zero.Components
{
	public class CoreComponentCollection : CoreBaseContainer<CoreComponent, CoreComponentSite>
	{
		public CoreComponentCollection(CoreComponent owner) : base(owner)
		{
		}

		protected override CoreComponentSite CreateSite(CoreComponent component, string name)
		{
			return new CoreComponentSite(this, component, name);
		}
	}
}
