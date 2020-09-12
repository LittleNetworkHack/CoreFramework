using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Reflection
{
	public interface IKeyOwner
	{
		IPropertyKey Key { get; }
	}
}
