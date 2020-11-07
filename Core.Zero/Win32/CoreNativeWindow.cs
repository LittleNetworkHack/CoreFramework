using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zero.Win32
{
	public interface ICoreNativeWindow
	{
		void Attach(IntPtr handle);
		IntPtr Detach();

	}

	public class CoreNativeWindow 
	{
		public IntPtr Handle { get; protected set; }
	}
}
