using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	public static partial class Win32
	{
		#region Extern

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

		#endregion Extern

		#region Control Extensions

		public static IntPtr SendMessage(this Control ctrl, int msg, int wParam, int lParam)
		{
			return SendMessage(new HandleRef(ctrl, ctrl.Handle), msg, wParam, lParam);
		}

		#endregion Control Extensions

		#region Util

		public static int MakeLong(int low, int high) => (high << 16) | (low & 0xFFFF);

		public static int ToHiWord(this int n) => (n >> 16) & 0xFFFF;

		public static int ToSigHiWord(this int n) => (short)((n >> 16) & 0xFFFF);

		public static int ToLoWord(this int n) => n & 0xFFFF;

		public static int ToSigLoWord(this int n) => (short)(n & 0xFFFF);

		public static Point ToPoint(this int n) => new Point(n.ToSigLoWord(), n.ToSigHiWord());

		public static Size ToSize(this int n) => new Size(n.ToSigLoWord(), n.ToSigHiWord());

		#endregion Util
	}
}
