using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	public class CoreDataBinder<TControl>
		where TControl : ICoreEditControl
	{
		protected CoreDataSource Source => null;//Owner?.CoreSource;

		public TControl Owner { get; }
		public string Member { get; set; }

		public CoreDataBinder(TControl owner)
		{
			Owner = owner;
			//Owner.CoreSourceChanged += Owner_CoreSourceChanged;
		}

		private void Owner_CoreSourceChanged(object sender, EventArgs e)
		{
			if (Source == null)
				return;

			Owner.Value = string.IsNullOrEmpty(Member) ? null : Source.GetValue(Member);
		}
	}
}
