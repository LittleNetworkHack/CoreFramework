using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	public interface ICoreControl
	{
		CoreDataSource CoreSource { get; }

		object Value { get; set; }
		Type ValueType { get; }
		string DataPath { get; set; }
		Label DescriptionLabel { get; }

		Size Size { get; }
		Padding Margin { get; }
		DockStyle Dock { get; }
		void SetBounds(int x, int y, int width, int height, BoundsSpecified specified);

		
	}
}
