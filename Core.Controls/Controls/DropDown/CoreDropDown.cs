using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	public class CoreDropDown
	{
		#region Fields

		private ToolStripDropDown dropDown;
		private ToolStripControlHost dropHost;

		#endregion Fields

		#region Properties

		public Panel Container { get; } 
		public bool IsOpen => dropDown.Visible;
		
		public event EventHandler Opened
		{
			add => dropDown.Opened += value;
			remove => dropDown.Opened -= value;
		}

		public event ToolStripDropDownClosedEventHandler Closed
		{
			add => dropDown.Closed += value;
			remove => dropDown.Closed -= value;
		}

		#endregion Properties

		#region Constructors

		public CoreDropDown()
		{
			Container = CreateContainer();
			dropHost = CreateDropHost(Container);
			dropDown = CreateDropDown(dropHost);
		}

		#endregion Constructors

		#region Methods

		public void ShowDropDown(Control activeControl, Rectangle bounds)
		{
			ShowDropDown(activeControl, bounds.Location, bounds.Size);
		}

		public void ShowDropDown(Control activeControl, Point location, Size size)
		{
			if (IsOpen)
				return;

			dropHost.Size = size;
			dropDown.Size = size;

			dropDown.Show(location);

			if (activeControl.CanFocus)
				activeControl.Focus();
			else if (activeControl.CanSelect)
				activeControl.Select();
		}

		public void CloseDropDown()
		{
			if (!IsOpen)
				return;

			dropDown.Close();
		}

		#endregion Methods

		#region Create Controls

		private static Panel CreateContainer() => new Panel()
		{
			AutoScroll = true,
			Dock = DockStyle.Fill,
			Margin = Padding.Empty,
			Padding = Padding.Empty
		};

		private static ToolStripControlHost CreateDropHost(Panel scrollPanel) => new ToolStripControlHost(scrollPanel)
		{
			AutoSize = false,
			Margin = Padding.Empty,
			Padding = Padding.Empty
		};

		private static ToolStripDropDown CreateDropDown(ToolStripControlHost dropHost) => new ToolStripDropDown()
		{
			AutoSize = false,
			AutoClose = true,
			DropShadowEnabled = true,
			Margin = Padding.Empty,
			Padding = Padding.Empty,
			Items =
			{
				dropHost
			}
		};

		#endregion Create Controls
	}
}
