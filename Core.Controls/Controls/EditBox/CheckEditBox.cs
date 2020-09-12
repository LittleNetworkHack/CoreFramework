using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	public class CheckEditBox : CheckBox
	{
		#region Value

		public bool? Value
		{
			get
			{
				switch (CheckState)
				{
					case CheckState.Checked:
						return true;
					case CheckState.Unchecked:
						return false;
					case CheckState.Indeterminate:
					default:
						return null;
				}
			}
			set
			{
				if (value == null)
					CheckState = CheckState.Indeterminate;
				else if (value.Value)
					CheckState = CheckState.Checked;
				else
					CheckState = CheckState.Unchecked;
			}
		}

		#endregion Value
	}
}
