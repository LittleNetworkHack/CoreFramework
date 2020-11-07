using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Zero.ComponentModel;

namespace Core.Zero.Controls
{
	public class CoreControlCollection : CoreBaseContainer<CoreControl, CoreControlSite>
	{
		public CoreControlCollection(CoreControl owner) : base(owner)
		{

		}

		protected override CoreControlSite CreateSite(CoreControl component, string name)
		{
			return new CoreControlSite(this, component, name);
		}
	}
}
