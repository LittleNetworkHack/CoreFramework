using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Collections;

namespace Core.Controls
{
	[ToolboxItem(false)]
	public class CoreGridViewModel<TDataObject> : CoreViewModel<CoreCollection<TDataObject>>
		where TDataObject : class
	{
		#region Constructors

		protected CoreGridViewModel()
		{
		}

		protected CoreGridViewModel(CoreCollection<TDataObject> data) : base(data)
		{

		}

		#endregion Constructors
	}
}
