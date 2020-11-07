using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.NControls.Components;

namespace Core.NControls.Drawing
{
	public class CoreScrollPaintObject
	{
		#region Static

		protected static readonly int bar_len = 19;
		protected static readonly int bar_len_2 = 17;
		protected static readonly Font arr_font = new Font("Segoe UI Symbol", 7F, FontStyle.Regular);
		protected static readonly StringFormat arr_format = new StringFormat()
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center,
			FormatFlags = StringFormatFlags.NoWrap,
			Trimming = StringTrimming.None
		};
		protected static readonly string arr_left = "\uE016";
		protected static readonly string arr_right = "\uE017";
		protected static readonly string arr_up = "\uE018";
		protected static readonly string arr_down = "\uE019";
		protected static readonly Size arr_size = new Size(10, 10);
		protected static readonly Size btn_size = new Size(bar_len, bar_len);

		protected static readonly Brush bar_back = Brushes.WhiteSmoke;
		protected static readonly Brush bar_fore = Brushes.LightGray;

		protected static readonly Brush bar_back_over = Brushes.Gainsboro;
		protected static readonly Brush bar_fore_over = Brushes.Gray;

		protected static readonly Brush bar_back_down = Brushes.Gainsboro;
		protected static readonly Brush bar_fore_down = Brushes.Gray;

		protected static readonly Brush btn_back = Brushes.WhiteSmoke;
		protected static readonly Brush btn_fore = Brushes.Gray;

		protected static readonly Brush btn_back_over = Brushes.LightGray;
		protected static readonly Brush btn_fore_over = Brushes.Black;

		protected static readonly Brush btn_back_down = Brushes.Gray;
		protected static readonly Brush btn_fore_down = Brushes.White;

		#endregion Static

		#region Properties

		public Rectangle Bounds { get; protected set; }

		public Rectangle BtnUpBounds { get; protected set; }
		public Rectangle BtnDownBounds { get; protected set; }
		public Rectangle BtnLeftBounds { get; protected set; }
		public Rectangle BtnRightBounds { get; protected set; }

		public Rectangle BarHorBounds { get; protected set; }
		public Rectangle BarVerBounds { get; protected set; }
		public Rectangle BarCornerBounds { get; protected set; }

		#endregion Properties

		#region Constructors

		public CoreScrollPaintObject(CoreScroll owner)
		{

		}

		#endregion Constructors

		#region Paint

		protected virtual void DoPaintCore(Graphics context)
		{
			DoPaintBars(context);
			DoPaintButtons(context);
		}

		protected virtual void DoPaintBars(Graphics dc)
		{
			if (!BarHorBounds.IsEmpty)
				dc.FillRectangle(bar_back, BarHorBounds);
			if (!BarVerBounds.IsEmpty)
				dc.FillRectangle(bar_back, BarVerBounds);
			if (!BarCornerBounds.IsEmpty)
				dc.FillRectangle(Brushes.WhiteSmoke, BarCornerBounds);
		}

		protected virtual void DoPaintButtons(Graphics dc)
		{
			if (!BtnUpBounds.IsEmpty)
			{
				dc.FillRectangle(btn_back, BtnUpBounds);
				dc.DrawString(arr_up, arr_font, Brushes.Black, BtnUpBounds, arr_format);
			}
			if (!BtnDownBounds.IsEmpty)
			{
				dc.FillRectangle(btn_back, BtnDownBounds);
				dc.DrawString(arr_down, arr_font, Brushes.Black, BtnDownBounds, arr_format);
			}
			if (!BtnLeftBounds.IsEmpty)
			{
				dc.FillRectangle(btn_back, BtnLeftBounds);
				dc.DrawString(arr_left, arr_font, Brushes.Black, BtnLeftBounds, arr_format);
			}
			if (!BtnRightBounds.IsEmpty)
			{
				dc.FillRectangle(btn_back, BtnRightBounds);
				dc.DrawString(arr_right, arr_font, Brushes.Black, BtnRightBounds, arr_format);
			}
		}

		#endregion Paint

		#region Calculations

		protected virtual void DoCalculationsCore(object context)
		{
			//if (!IsDirty)
			//	return;

			DoCalcBounds();
			DoCalcButtons();
			DoCalcBars();
		}

		protected virtual void DoCalcBounds()
		{
			//Bounds = Owner.Owner.Bounds;
		}

		protected virtual void DoCalcButtons()
		{
			CoreOrientation bars = CoreOrientation.HorizontalAndVertical;
			switch (bars)
			{
				default:
				case CoreOrientation.None:
					BtnUpBounds = Rectangle.Empty;
					BtnDownBounds = Rectangle.Empty;
					BtnLeftBounds = Rectangle.Empty;
					BtnRightBounds = Rectangle.Empty;
					break;
				case CoreOrientation.Horizontal:
					BtnUpBounds = Rectangle.Empty;
					BtnDownBounds = Rectangle.Empty;
					BtnLeftBounds = new Rectangle(Bounds.Left, Bounds.Bottom - bar_len, bar_len_2, bar_len);
					BtnRightBounds = new Rectangle(Bounds.Right - bar_len_2, Bounds.Bottom - bar_len, bar_len_2, bar_len);
					break;
				case CoreOrientation.Vertical:
					BtnUpBounds = new Rectangle(Bounds.Right - bar_len, Bounds.Top, bar_len, bar_len_2);
					BtnDownBounds = new Rectangle(Bounds.Right - bar_len, Bounds.Bottom - bar_len_2, bar_len, bar_len_2);
					BtnLeftBounds = Rectangle.Empty;
					BtnRightBounds = Rectangle.Empty;
					break;
				case CoreOrientation.HorizontalAndVertical:
					BtnUpBounds = new Rectangle(Bounds.Right - bar_len, Bounds.Top, bar_len, bar_len_2);
					BtnDownBounds = new Rectangle(Bounds.Right - bar_len, Bounds.Bottom - bar_len - bar_len_2, bar_len, bar_len_2);
					BtnLeftBounds = new Rectangle(Bounds.Left, Bounds.Bottom - bar_len, bar_len_2, bar_len);
					BtnRightBounds = new Rectangle(Bounds.Right - bar_len - bar_len_2, Bounds.Bottom - bar_len, bar_len_2, bar_len);
					break;
			}
		}

		protected virtual void DoCalcBars()
		{
			CoreOrientation bars = CoreOrientation.HorizontalAndVertical;
			switch (bars)
			{
				case CoreOrientation.None:
					BarHorBounds = Rectangle.Empty;
					BarVerBounds = Rectangle.Empty;
					BarCornerBounds = Rectangle.Empty;
					break;
				case CoreOrientation.Horizontal:
					BarHorBounds = new Rectangle(BtnLeftBounds.Right, BtnLeftBounds.Top, BtnRightBounds.Left - BtnLeftBounds.Right, bar_len);
					BarVerBounds = Rectangle.Empty;
					BarCornerBounds = Rectangle.Empty;
					break;
				case CoreOrientation.Vertical:
					BarHorBounds = Rectangle.Empty;
					BarVerBounds = new Rectangle(BtnUpBounds.Left, BtnUpBounds.Bottom, bar_len, BtnDownBounds.Top - BtnUpBounds.Bottom);
					BarCornerBounds = Rectangle.Empty;
					break;
				case CoreOrientation.HorizontalAndVertical:
					BarHorBounds = new Rectangle(BtnLeftBounds.Right, BtnLeftBounds.Top, BtnRightBounds.Left - BtnLeftBounds.Right, bar_len);
					BarVerBounds = new Rectangle(BtnUpBounds.Left, BtnUpBounds.Bottom, bar_len, BtnDownBounds.Top - BtnUpBounds.Bottom);
					BarCornerBounds = new Rectangle(BtnRightBounds.Right, BtnDownBounds.Bottom, bar_len, bar_len);
					break;
			}
		}

		#endregion Calculations
	}
}
