using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.NControls.Controls;

using SharpDX.Mathematics.Interop;

using WinApi.DxUtils;
using WinApi.DxUtils.Component;
using WinApi.User32;
using WinApi.Windows;
using WinApi.Windows.Controls;

namespace Core.NControls.Forms
{
	public class AppWindow : DxWindow, IConstructionParamsProvider, System.Windows.Forms.IWin32Window
	{
		private DesignControl m_designCtrl;

		protected override void OnCreate(ref CreateWindowPacket packet)
		{
			try
			{
				m_designCtrl = DesignControl.Create("Something", exStyles: 0, x: 10, y: 10, width: 200, height:200, hParent: Handle);
				m_designCtrl.SetPosition(10, 10);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			base.OnCreate(ref packet);
		}

		protected override void OnDxPaint(Dx11Component resource)
		{
			var context = resource.D2D.Context;
			context.BeginDraw();
			context.Clear(new RawColor4(128, 128, 128, 255));
			context.EndDraw();
			base.OnDxPaint(resource);
		}

		protected override void OnSize(ref SizePacket packet)
		{
			//Debug.WriteLine("OnSize");
			base.OnSize(ref packet);
		}

		protected override void OnNcCalcSize(ref NcCalcSizePacket packet)
		{
			//Debug.WriteLine("OnCalcSize");
			base.OnNcCalcSize(ref packet);
		}

		#region Constructors

		public IConstructionParams GetConstructionParams() => new DxWindowConstructionParams();

		public class DxWindowConstructionParams : FrameWindowConstructionParams
		{
			public override WindowExStyles ExStyles => base.ExStyles |
													   WindowExStyles.WS_EX_LEFT | 
													   WindowExStyles.WS_EX_APPWINDOW |
													   WindowExStyles.WS_EX_NOREDIRECTIONBITMAP;

		}

		public static Lazy<WindowFactory> ClassFactory => new Lazy<WindowFactory>(() => WindowFactory.Create());

		public static AppWindow Create() => ClassFactory.Value.CreateWindowEx(() => new AppWindow(), "Hello");

		#endregion Constructors
	}
}
