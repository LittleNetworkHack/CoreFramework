using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zero.Win32
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct Win32ClassEx
	{
		public uint Size;
		public Win32Styles Styles;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public WindowProc WndProc;
		public int ClassExtraBytes;
		public int WindowExtraBytes;
		public IntPtr InstanceHandle;
		public IntPtr IconHandle;
		public IntPtr CursorHandle;
		public IntPtr BackgroundBrushHandle;
		public string MenuName;
		public string ClassName;
		public IntPtr SmallIconHandle;
	}
}
