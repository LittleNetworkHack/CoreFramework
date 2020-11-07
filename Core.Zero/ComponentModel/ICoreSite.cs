using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zero.ComponentModel
{
	public interface ICoreSite : INestedSite
	{
		ICoreComponent Owner { get; }
	}
}
