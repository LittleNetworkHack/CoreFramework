using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Core.Data;

namespace Core.App
{
	public abstract class CoreAppManager
	{
		//public abstract ISqlConnectionProvider ConnectionProvider { get; }

		public abstract void InitializeApp();
	}
}
