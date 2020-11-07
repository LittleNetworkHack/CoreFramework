using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	public static class ControlExtensions
	{
		#region Esc Form Exit

		public static void OnEscClose(this Form view, bool flag)
		{
			if (flag)
			{
				view.KeyPreview = true;
				view.Disposed += EscFormDisposed;
				view.PreviewKeyDown += EscFormKeyDown;
			}
			else
			{
				view.Disposed -= EscFormDisposed;
				view.PreviewKeyDown -= EscFormKeyDown;
			}
		}

		private static void EscFormDisposed(object sender, EventArgs e)
		{
			Form view = sender as Form;
			if (view == null)
				return;

			view.Disposed -= EscFormDisposed;
			view.PreviewKeyDown -= EscFormKeyDown;
		}

		private static void EscFormKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode != Keys.Escape)
				return;

			Form view = sender as Form;
			if (view == null || view.IsDisposed)
				return;

			view.Close();
		}

		#endregion Esc Form Exit
	}
}
