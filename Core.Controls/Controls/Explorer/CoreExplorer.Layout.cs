using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Layout;
using System.Windows.Forms;

namespace Core.Controls
{
	public class CoreExplorerLayout : LayoutEngine
	{
		public static CoreExplorerLayout Instance { get; } = new CoreExplorerLayout();
		public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
		{
			CoreExplorerBase group = container as CoreExplorerBase;

			bool isRoot = group is CoreExplorer;
			if (!group.IsExpanded && !isRoot)
			{
				group.Height = group.HeaderHeight;
				return false;
			}

			int itemX = group.ItemIndent;
			int itemY = group.HeaderHeight + group.ItemSpace;
			int itemW = group.Width - group.ItemIndent;
			int barHeight = itemY + group.ItemSpace;
			if (isRoot)
				itemW -= group.ItemIndent;
			else
				itemW -= group.ItemIndent / 2;

			foreach (Control ctrl in group.Controls)
			{
				ctrl.SetBounds(itemX, itemY, itemW, 0, BoundsSpecified.Location | BoundsSpecified.Width);
				itemY += ctrl.Height;
				barHeight += ctrl.Height;
			}

			if (isRoot && group.Parent != null && barHeight < group.Parent.Height)
				group.Height = group.Parent.ClientRectangle.Height;
			else if (!isRoot)
				group.Height = barHeight;

			return false;
		}
	}
}
