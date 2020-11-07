using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.NControls.Components;

namespace Core.NControls.Drawing
{
	public static class CorePaintLib
	{
		public static void DrawTriangle(this Graphics dc, Brush brush, Rectangle rect, CoreDirection direction)
		{
			int hw = rect.Width / 2;
			int hh = rect.Height / 2;
			Point p0, p1, p2;

			bool skip = false;

			switch (direction)
			{
				case CoreDirection.Up:
					p0 = new Point(rect.Left + hw, rect.Top);
					p1 = new Point(rect.Left, rect.Bottom);
					p2 = new Point(rect.Right, rect.Bottom);
					break;
				case CoreDirection.Down:
					p0 = new Point(rect.Left + hw, rect.Bottom);
					p1 = new Point(rect.Left, rect.Top);
					p2 = new Point(rect.Right, rect.Top);
					break;
				case CoreDirection.Left:
					p0 = new Point(rect.Left, rect.Top + hh);
					p1 = new Point(rect.Right, rect.Top);
					p2 = new Point(rect.Right, rect.Bottom);
					break;
				case CoreDirection.Right:
					p0 = new Point(rect.Right + hw, rect.Top + hh);
					p1 = new Point(rect.Left, rect.Bottom);
					p2 = new Point(rect.Left, rect.Top);
					break;
				default:
					skip = true;
					p0 = Point.Empty;
					p1 = Point.Empty;
					p2 = Point.Empty;
					break;
			}

			if (skip)
				return;

			dc.FillPolygon(brush, new Point[] { p0, p1, p2 });
		}



		public static void DrawBox(this Graphics context, Brush back, Brush border, Rectangle boxBounds, CoreThickness borderThickness)
		{
			context.FillRectangle(border, boxBounds);
			boxBounds = borderThickness.Apply(boxBounds);
			context.FillRectangle(back, boxBounds);
		}

		#region Helpers

		public static Rectangle Center(this Rectangle rect, Size size)
		{
			Point loc = rect.Center();
			loc.Offset(size.Width / -2, size.Height / -2);
			return new Rectangle(loc, size);
		}

		public static Point Center(this Rectangle rect)
		{
			return new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
		}

		#endregion Helpers
	}
}
