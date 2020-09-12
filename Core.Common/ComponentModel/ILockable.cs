using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.ComponentModel
{
	public interface ILockable
	{
		bool IsLocked();
		void LockObject();
	}
}
