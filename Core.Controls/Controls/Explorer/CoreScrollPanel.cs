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
	public class CoreScrollPanel : Panel
	{
		protected override Size DefaultSize => new Size(200, 300);


		[DefaultValue(typeof(Color), "75, 133, 182")]
		public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

		private Color _backColor2 = Color.GhostWhite;
		[DefaultValue(typeof(Color), "GhostWhite")]
		public Color BackColor2
		{
			get => _backColor2;
			set
			{
				if (_backColor2 == value)
					return;

				_backColor2 = value;
				Invalidate();
			}
		}

		private float _backAngle = 90F;
		[DefaultValue(90F)]
		public float BackAngle
		{
			get => _backAngle;
			set
			{
				if (_backAngle == value)
					return;

				_backAngle = value;
				Invalidate();
			}
		}

		public CoreScrollPanel()
		{
			ControlStyles style = ControlStyles.AllPaintingInWmPaint |
								  ControlStyles.UserPaint |
								  ControlStyles.ResizeRedraw |
								  ControlStyles.OptimizedDoubleBuffer;
			SetStyle(style, true);
			BackColor = Color.FromArgb(75, 133, 182);
			AutoScroll = true;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (BackColor2 == Color.Transparent)
			{
				e.Graphics.Clear(BackColor);
			}
			else
			{
				using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, BackColor, BackColor2, BackAngle))
					e.Graphics.FillRectangle(brush, ClientRectangle);
			}
			
			base.OnPaint(e);
		}
	}
}
