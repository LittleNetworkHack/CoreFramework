using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zero.Win32
{
	public sealed class CoreCursor
	{
		public IntPtr Handle { get; }
		public Win32Cursor CursorID { get; }

		private CoreCursor(Win32Cursor cursor)
		{
			CursorID = cursor;
			//Handle = (IntPtr)(int)cursor;
		}

		#region Cursors

		public static CoreCursor AppStarting { get; }
		public static CoreCursor Arrow { get; }
		public static CoreCursor Cross { get; }
		public static CoreCursor Default { get; }
		public static CoreCursor Hand { get; }
		public static CoreCursor Help { get; }
		public static CoreCursor IBeam { get; }
		public static CoreCursor No { get; }
		public static CoreCursor SizeAll { get; }
		public static CoreCursor SizeNESW { get; }
		public static CoreCursor SizeNS { get; }
		public static CoreCursor SizeNWSE { get; }
		public static CoreCursor SizeWE { get; }
		public static CoreCursor UpArrow { get; }
		public static CoreCursor Wait { get; }

		//public static CoreCursor NoMove2D { get; }
		//public static CoreCursor NoMoveHoriz { get; }
		//public static CoreCursor NoMoveVert { get; }
		//public static CoreCursor PanEast { get; }
		//public static CoreCursor PaneNE { get; }
		//public static CoreCursor PanNorth { get; }
		//public static CoreCursor PanNW { get; }
		//public static CoreCursor PanSE { get; }
		//public static CoreCursor PanSouth { get; }
		//public static CoreCursor PanSW { get; }
		//public static CoreCursor PanWest { get; }
		//public static CoreCursor HSplit { get; }
		//public static CoreCursor VSplit { get; }

		static CoreCursor()
		{
			AppStarting = new CoreCursor(Win32Cursor.APPSTARTING);
			Arrow = new CoreCursor(Win32Cursor.ARROW);
			Cross = new CoreCursor(Win32Cursor.CROSS);
			Default = new CoreCursor(Win32Cursor.ARROW);
			Hand = new CoreCursor(Win32Cursor.HAND);
			Help = new CoreCursor(Win32Cursor.HELP);
			IBeam = new CoreCursor(Win32Cursor.IBEAM);
			No = new CoreCursor(Win32Cursor.NO);
			SizeAll = new CoreCursor(Win32Cursor.SIZEALL);
			SizeNESW = new CoreCursor(Win32Cursor.SIZENESW);
			SizeNS = new CoreCursor(Win32Cursor.SIZENS);
			SizeNWSE = new CoreCursor(Win32Cursor.SIZENWSE);
			SizeWE = new CoreCursor(Win32Cursor.SIZEWE);
			UpArrow = new CoreCursor(Win32Cursor.UPARROW);
			Wait = new CoreCursor(Win32Cursor.WAIT);
		}

		public static CoreCursor ResolveCursor(Win32Cursor cursor)
		{
			switch (cursor)
			{
				case Win32Cursor.APPSTARTING:
					return AppStarting;
				case Win32Cursor.ARROW:
					return Arrow;
				case Win32Cursor.CROSS:
					return Cross;
				case Win32Cursor.HAND:
					return Hand;
				case Win32Cursor.HELP:
					return Help;
				case Win32Cursor.IBEAM:
					return IBeam;
				case Win32Cursor.NO:
					return No;
				case Win32Cursor.SIZEALL:
					return SizeAll;
				case Win32Cursor.SIZENESW:
					return SizeNESW;
				case Win32Cursor.SIZENS:
					return SizeNS;
				case Win32Cursor.SIZENWSE:
					return SizeNWSE;
				case Win32Cursor.SIZEWE:
					return SizeWE;
				case Win32Cursor.UPARROW:
					return UpArrow;
				case Win32Cursor.WAIT:
					return Wait;
				default:
					return null;
			}
		}

		[DllImport("user32", CharSet = CharSet.Unicode)]
		public static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorResource);

		[DllImport("user32", ExactSpelling = true)]
		public static extern bool GetCursorPos(out Point point);

		[DllImport("user32", ExactSpelling = true)]
		public static extern IntPtr SetCursor(IntPtr hCursor);

		//public static void TestSet(Win32Cursor c)
		//{
		//	CoreCursor cc = ResolveCursor(c);
		//	IntPtr p = SetCursor(cc.hCur);
		//	Console.WriteLine(p);
		//}

		#endregion Cursors
	}
}
