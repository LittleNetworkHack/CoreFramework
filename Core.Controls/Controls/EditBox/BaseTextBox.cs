using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Core.Controls.Win32;

namespace Core.Controls
{
	public class BaseTextBox : Control
	{
		#region Constructors

		protected override CreateParams CreateParams
		{
			get
			{
				unchecked
				{
					CreateParams cp = base.CreateParams;
					cp.ClassName = "EDIT";
					//cp.Style |= (int)(WS.BORDER);
					cp.Style |= (int)(ES.AUTOVSCROLL | ES.AUTOHSCROLL | ES.NOHIDESEL | ES.READONLY);
					cp.ExStyle |= (int)(WS_EX.CLIENTEDGE);
					return cp;
				}
			}
		}

		public override Color BackColor { get => Color.White; set => base.BackColor = value; }

		public BaseTextBox()
		{
			SetStyle(ControlStyles.FixedHeight, true);
			SetStyle(ControlStyles.UserPaint, false);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			this.SendMessage(207, 0, 0);
			this.SendMessage((int)EM.SETMARGINS, 3, 0);
			base.OnHandleCreated(e);
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
		}

		#endregion Constructors

	}
}
