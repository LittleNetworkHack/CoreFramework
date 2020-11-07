using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetCoreEx.Geometry;

using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

using WinApi.DxUtils;
using WinApi.DxUtils.Component;
using WinApi.User32;
using WinApi.Windows;

namespace Core.NControls.Controls
{
	public class DesignControl : DxWindow, IConstructionParamsProvider
	{
		private static readonly RawColor4 clrBack = new RawColor4(255, 255, 255, 255);
		private static readonly RawColor4 clrBorder = new RawColor4(0, 0, 0, 255);

		protected override void OnDxPaint(Dx11Component resource)
		{
			RawRectangleF rect = GetClientRect().ToRawRectangleF();
			DeviceContext context = resource.D2D.Context;

			context.BeginDraw();
			context.Clear(clrBack);

			using (SolidColorBrush brush = new SolidColorBrush(context, clrBorder))
			{
				context.DrawRectangle(rect, brush);
				string text = GetText();
				using (TextFormat tf = new TextFormat(Dx.TextFactory, "Segoe UI", 12F))
					context.DrawText(text, tf, rect, brush);
			}

			context.EndDraw();
		}

		protected override void OnDestroy(ref Packet packet)
		{
			Dispose();
			base.OnDestroy(ref packet);
		}

		public IConstructionParams GetConstructionParams() => new VisibleChildConstructionParams();

		public static Lazy<WindowFactory> ClassFactory =
			new Lazy<WindowFactory>(() => WindowFactory.Create());

		protected DesignControl() { }


		public static DesignControl Create(string text = null,
			WindowStyles? styles = null,
			WindowExStyles? exStyles = null, int? x = null, int? y = null,
			int? width = null, int? height = null, IntPtr? hParent = null, IntPtr? hMenu = null,
			WindowFactory factory = null)
		{
			return (factory ?? ClassFactory.Value).CreateWindowEx(() => new DesignControl(), text, styles, exStyles, x, y,
				width, height, hParent, hMenu);
		}
	}
}
