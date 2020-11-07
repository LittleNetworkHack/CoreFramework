using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zero.Drawing
{
	public struct CoreThickness
	{
		#region Fields

		public static readonly CoreThickness Empty;

		#endregion Fields

		#region Properties

		public int Left { get; set; }
		public int Top { get; set; }
		public int Right { get; set; }
		public int Bottom { get; set; }

		public int Horizontal => Left + Right;
		public int Vertical => Top + Bottom;

		public bool IsEmpty => Left == 0 && Top == 0 && Right == 0 && Bottom == 0;

		#endregion Properties

		#region Constructors

		public CoreThickness(int all)
		{
			Left = all;
			Top = all;
			Right = all;
			Bottom = all;
		}

		public CoreThickness(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		#endregion Constructors

		#region Methods

		public Rectangle Apply(Rectangle bounds)
		{
			return new Rectangle(bounds.X + Left, bounds.Y + Top, bounds.Width - Horizontal, bounds.Height - Vertical);
		}

		#endregion Methods
	}
}
