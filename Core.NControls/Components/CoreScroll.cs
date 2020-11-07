using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.NControls.Drawing;

namespace Core.NControls.Components
{
	public class CoreScroll : CoreComponent
	{
		#region Properties

		public CoreControl Owner { get; }

		public CoreOrientation VisibleBars { get; set; }

		#endregion Properties

		#region Constructors

		public CoreScroll(CoreControl owner, CoreOrientation orientation)
		{
			Owner = owner;
			VisibleBars = orientation;
		}

		#endregion Constructors
	}
}
