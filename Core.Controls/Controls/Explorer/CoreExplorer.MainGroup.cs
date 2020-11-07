using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	[ToolboxItem(false)]
	public class CoreExplorerMainGroup : CoreExplorerBase
	{
		#region Constructors

		public CoreExplorerMainGroup()
		{
			_headerFont = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
		}

		#endregion Constructors

		#region Paint

		private const int _headerTxtLeft = 8;

		private static readonly Color headColor1 = Color.GhostWhite;
		private static readonly Color headColor2 = Color.Silver;

		private static readonly Color headOverColor1 = Color.Orange;
		private static readonly Color headOverColor2 = Color.GhostWhite;

		private static readonly Color pnlColor1 = Color.Linen;
		private static readonly Color pnlColor2 = Color.LightGray;
		private Rectangle FillRectangle => new Rectangle(0, HeaderCut, Width, HeaderHeight - HeaderCut);
		private Rectangle TextRectangle => new Rectangle()
		{
			X = IconRectangle.Right + _headerTxtLeft,
			Y = _headerCut,
			Width = this.Width - (IconRectangle.Right + _headerTxtLeft),
			Height = FillRectangle.Height
		};


		[DefaultValue(typeof(Font), "Microsoft Sans Serif, 10pt, style=Bold")]
		public override Font HeaderFont { get => base.HeaderFont; set => base.HeaderFont = value; }

		protected override void OnPaint(PaintEventArgs e)
		{
			ButtonRenderer.DrawParentBackground(e.Graphics, HeaderRectangle, this);
			DrawHeader(e.Graphics);
			DrawPanel(e.Graphics);
			base.OnPaint(e);
		}

		private void DrawHeader(Graphics g)
		{
			Rectangle fillRect = FillRectangle;
			Color hc1 = IsMosueOverHeader ? headOverColor1 : headColor1;
			Color hc2 = IsMosueOverHeader ? headOverColor2 : headColor2;

			using (LinearGradientBrush brush = new LinearGradientBrush(fillRect, hc1, hc2, 0F))
				g.FillRectangle(brush, fillRect);

			if (BackgroundImage != null)
				g.DrawImageUnscaled(BackgroundImage, IconRectangle);

			TextRenderer.DrawText(g, Text, HeaderFont, TextRectangle, ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
		}

		private void DrawPanel(Graphics g)
		{
			Rectangle pnlRect = PanelRectangle;
			if (pnlRect.Height == 0)
				return;

			using (LinearGradientBrush brush = new LinearGradientBrush(pnlRect, pnlColor1, pnlColor2, 135F))
				g.FillRectangle(brush, pnlRect);
		}

		#endregion Paint
	}
}
