using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	public class CoreExplorerItemGroup : CoreExplorerBase
	{
		#region Override Properties

		[DefaultValue(24)]
		public override int HeaderHeight { get => base.HeaderHeight; set => base.HeaderHeight = value; }

		[DefaultValue(0)]
		public override int HeaderCut { get => base.HeaderCut; set => base.HeaderCut = value; }

		[DefaultValue(0)]
		public override int ItemSpace { get => base.ItemSpace; set => base.ItemSpace = value; }

		#endregion Override Properties

		#region Constructors

		public CoreExplorerItemGroup()
		{
			ControlStyles style = ControlStyles.AllPaintingInWmPaint |
								  ControlStyles.UserPaint |
								  ControlStyles.ResizeRedraw |
								  ControlStyles.Opaque |
								  ControlStyles.SupportsTransparentBackColor |
								  ControlStyles.OptimizedDoubleBuffer;
			SetStyle(style, true);
			_headerHeight = 24;
			_headerCut = 0;
			_itemSpace = 0;
		}

		#endregion Constructors

		#region Paint

		private const int _headerTxtLeft = 4;

		private static readonly Color highBackColor = Color.FromArgb(192, Color.White);
		private static readonly Color highBorderColor = Color.Black;

		private Rectangle TextRectangle => new Rectangle()
		{
			X = IconRectangle.Right + _headerTxtLeft,
			Y = 0,
			Width = this.Width - (IconRectangle.Right + _headerTxtLeft),
			Height = HeaderHeight
		};

		protected override Rectangle IconRectangle => new Rectangle(2, (_headerHeight - 16) / 2, 16, 16);

		protected override void OnPaint(PaintEventArgs e)
		{
			ButtonRenderer.DrawParentBackground(e.Graphics, ClientRectangle, this);
			
			if (IsMosueOverHeader)
			{
				using (SolidBrush brush = new SolidBrush(highBackColor))
					e.Graphics.FillRectangle(brush, HeaderRectangle);
				ControlPaint.DrawBorder(e.Graphics, HeaderRectangle, highBorderColor, ButtonBorderStyle.Solid);
			}

			if (BackgroundImage != null)
				e.Graphics.DrawImageUnscaled(BackgroundImage, IconRectangle);

			TextRenderer.DrawText(e.Graphics, Text, HeaderFont, TextRectangle, ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);

			base.OnPaint(e);
		}

		#endregion Paint
	}
}
