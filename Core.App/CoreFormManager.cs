using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Collections;
using Core.Reflection;

namespace Core.App
{
	public static class CoreFormManager
	{

	}

	public sealed class CoreFormInfo
	{
		public Guid FormID { get; }
		public Type FormType { get; }
		public string FormName { get; }

	}
}
