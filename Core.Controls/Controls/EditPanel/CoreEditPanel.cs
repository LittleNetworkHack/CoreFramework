using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace Core.Controls
{
	//[Designer(typeof(CoreEditPanelDesigner), typeof(ParentControlDesigner))]
	[ToolboxItem(typeof(CoreEditBoxToolboxItem))]
	public class CoreEditPanel : Control
	{
		#region Properties

		#region Fields

		#endregion Fields

		//public override LayoutEngine LayoutEngine => CoreEditPanelLayout.Instance;

		#endregion Properties

		public ICoreControl EditBox { get; set; }

		#region Constructors

		public CoreEditPanel()
		{

		}

		#endregion Constructors
	}


	#region Designer

	public class CoreEditPanelDesigner : ParentControlDesigner
	{
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			CoreEditPanel panel = (CoreEditPanel)component;
			//EnableDesignMode(panel.EditBox, "EditBox");
		}
	}

	#endregion Designer

	public class CoreEditPanelLayout : LayoutEngine
	{
		private const int _labelWidth = 150;
		private const int _offsetTop = 18;
		private const int _offsetLeft = 18;
		private const int _itemSpace = 6;

		public static CoreEditPanelLayout Instance { get; } = new CoreEditPanelLayout();

		public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
		{
			CoreEditPanel panel = container as CoreEditPanel;
			IEnumerable<ICoreControl> items = panel.Controls.OfType<ICoreControl>();

			int itemX = _labelWidth + _offsetLeft;
			int itemY = _offsetTop;

			int lblX = _offsetLeft;
			int lblY = _offsetTop;


			foreach (ICoreControl ctrl in items)
			{
				if (ctrl.Dock != DockStyle.Fill)
					continue;

				Label lbl = null;//ctrl.DescriptionLabel;
				if (lbl == null)
					continue;

				Size sz = ctrl.Size;

				lbl.AutoSize = false;
				lbl.TextAlign = ContentAlignment.MiddleLeft;
				lbl.SetBounds(lblX, lblY, _labelWidth - _offsetLeft, sz.Height, BoundsSpecified.All);

				ctrl.SetBounds(itemX, itemY, 0, 0, BoundsSpecified.Location);

				int change = sz.Height + _itemSpace;
				lblY += change;
				itemY += change;
			}

			return false;
		}
	}
}
