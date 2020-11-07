using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Zero.ComponentModel;

namespace Core.Zero.Controls
{
	public class CoreControlSite : CoreBaseSite<CoreControl>
	{
		public CoreControlSite(ICoreContainer<CoreControl> container, CoreControl component, string name) : 
			base(container, component, name)
		{
		}
	}
}
