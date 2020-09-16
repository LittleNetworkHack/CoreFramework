using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controls
{
	public class CoreEditBoxToolboxItem : ToolboxItem
	{
		public CoreEditBoxToolboxItem(Type toolType) : base(toolType)
		{

		}

		protected override IComponent[] CreateComponentsCore(IDesignerHost host)
		{
			TextEditBox box = (TextEditBox)host.CreateComponent(typeof(TextEditBox));

			return new IComponent[] { box };
		}
	}
}
