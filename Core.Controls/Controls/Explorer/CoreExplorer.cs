using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Core.Controls
{
	public class CoreExplorer : CoreExplorerBase
	{
		[DefaultValue(0)]
		public override int HeaderHeight { get => base.HeaderHeight; set => base.HeaderHeight = value; }

		[DefaultValue(10)]
		public override int ItemIndent { get => base.ItemIndent; set => base.ItemIndent = value; }

		[DefaultValue(0)]
		public override int ItemSpace { get => base.ItemSpace; set => base.ItemSpace = value; }

		public CoreExplorer()
		{
			
			ControlStyles style = ControlStyles.AllPaintingInWmPaint |
								  ControlStyles.UserPaint |
								  ControlStyles.ResizeRedraw |
								  ControlStyles.OptimizedDoubleBuffer;
			SetStyle(style, true);
			_headerHeight = 0;
			_itemIndent = 10;
			_itemSpace = 0;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (Parent is CoreScrollPanel)
			{
				ButtonRenderer.DrawParentBackground(e.Graphics, ClientRectangle, this);
			}
			else
			{
				using (LinearGradientBrush brush = new LinearGradientBrush(DisplayRectangle, Color.FromArgb(78, 135, 183), Color.GhostWhite, 90F))
					e.Graphics.FillRectangle(brush, DisplayRectangle);
			}

			base.OnPaint(e);
		}
	}
}
