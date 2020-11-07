using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WinApi.Windows;

namespace Core.NControls.Controls
{
	public class SimpleControl : EventedWindowCore, IConstructionParamsProvider
	{
		protected override void OnPaint(ref PaintPacket packet)
		{
			using (Graphics g = Graphics.FromHwnd(packet.Hwnd))
			{
				var sz = GetClientSize();
				g.FillRectangle(Brushes.LightCoral, new Rectangle(0, 0, sz.Width, sz.Height));
			}
		}

		public static Lazy<WindowFactory> ClassFactory =
			new Lazy<WindowFactory>(() => WindowFactory.Create());

		public static SimpleControl Create()
		{
			return ClassFactory.Value.CreateWindowEx(() => new SimpleControl(), exStyles: 0);
		}

		public IConstructionParams GetConstructionParams() => new VisibleChildConstructionParams();
	}
}
