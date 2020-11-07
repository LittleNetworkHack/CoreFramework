using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinApi.DxUtils.Component;
using WinApi.Windows;

namespace Core.NControls.Forms
{
	[Designer(typeof(TestDesigner), typeof(IRootDesigner))]
	public class TestComponent : Component
	{
		protected readonly Dx11Component Dx = new Dx11Component();
	}

	

	public class TestDesigner : ComponentDesigner, IRootDesigner
	{
		//private AppWindow host;
		private System.Windows.Forms.UserControl host;

		public object GetView(ViewTechnology technology)
		{
			if (host == null)
			{
				//host = AppWindow.Create();
				host = new System.Windows.Forms.UserControl();
			}

			return host;
		}

		public ViewTechnology[] SupportedTechnologies => new ViewTechnology[] { ViewTechnology.Default };
	}
}
