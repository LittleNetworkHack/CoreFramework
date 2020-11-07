using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Core.Reflection;

namespace Core.Controls
{
	public class CoreMemberTypeConverter : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.DropDown;

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			CoreDataSource source = context.Instance as CoreDataSource;
			if (source == null)
				return value;

			IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (svc == null)
				return value;

			MessageBox.Show(svc.GetType().FullName);

			string[] items;
			if (source.IsOwned)
				items = source.CoreSource.Properties.Select(K => K.Name).ToArray();
			else
				items = source.Properties.Select(K => K.Name).ToArray();

			ListBox box = new ListBox();
			box.Dock = DockStyle.Fill;
			box.Items.AddRange(items);
			box.Tag = svc;
			box.SelectedIndexChanged += Box_SelectedIndexChanged;

			svc.DropDownControl(box);

			int idx = box.SelectedIndex;
			return idx != -1 ? items[idx] : value;
		}

		private void Box_SelectedIndexChanged(object sender, EventArgs e)
		{
			IWindowsFormsEditorService svc = (sender as ListBox).Tag as IWindowsFormsEditorService;
			svc.CloseDropDown();
		}
	}
}
