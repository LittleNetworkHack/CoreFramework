using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zero.Drawing
{
	public static class CorePaintLib
	{
		public static void DrawBox(this Graphics context, Brush back, Brush border, Rectangle boxBounds, CoreThickness borderThickness)
		{
			context.FillRectangle(border, boxBounds);
			boxBounds = borderThickness.Apply(boxBounds);
			context.FillRectangle(back, boxBounds);
		}

		public static Rectangle Center(this Rectangle rect, Size size)
		{
			Point loc = rect.Center();
			loc.Offset(size.Width / -2, size.Height / -2);
			return new Rectangle(loc, size);
		}

		public static Point Center(this Rectangle rect)
		{
			return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
		}
	}
}
