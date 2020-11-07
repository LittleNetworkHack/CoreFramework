using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetCoreEx.Geometry;

using SharpDX.Mathematics.Interop;

namespace Core.NControls
{
	public static class ConvertExtensions
	{
		public static RawRectangleF ToRawRectangleF(this Rectangle rect)
			=> new RawRectangleF(rect.Left, rect.Top, rect.Right, rect.Bottom);
	}
}
