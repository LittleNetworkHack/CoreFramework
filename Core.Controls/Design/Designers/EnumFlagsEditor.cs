using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Core.Collections;

namespace Core.Controls
{
	[ToolboxItem(false)]
	public class EnumFlagsEditor : CheckedListBox
	{
		protected bool inSetting;
		protected Dictionary<object, long> longValues;
		public Type EnumType { get; protected set; }

		public EnumFlagsEditor()
		{
			CheckOnClick = true;
		}

		public void InitializeEditor(Type type)
		{
			if (!type.IsEnum)
				throw new ArgumentException("Type must be enum!", nameof(type));

			EnumType = type;
			longValues = new Dictionary<object, long>();

			Items.Clear();
			foreach (var value in Enum.GetValues(EnumType))
			{
				long v = CoreConverter.ConvertTo<long>(value);
				longValues.Add(value, v);
				Items.Add(value);
			}
		}

		public object GetValue()
		{
			long result = GetLongValue();
			return Enum.ToObject(EnumType, result);
		}

		public void SetValue(object value)
		{
			long combined = CoreConverter.ConvertTo<long>(value);
			SetLongValue(combined);
		}

		public long GetLongValue()
		{
			long result = 0;
			foreach (object value in CheckedItems)
			{
				if (!longValues.TryGetValue(value, out long v) || v == 0)
					continue;

				result |= v;
			}
			return result;
		}

		public void SetLongValue(long value)
		{
			inSetting = true;

			foreach (int i in CheckedIndices)
				SetItemChecked(i, false);

			if (value == 0)
			{
				object v = Enum.ToObject(EnumType, value);
				int idx = Items.IndexOf(v);
				if (idx != -1)
					SetItemChecked(idx, true);
			}
			else
			{
				foreach (var pair in longValues)
				{
					if (pair.Value == 0)
						continue;

					bool defined = (value & pair.Value) == pair.Value;
					int idx = Items.IndexOf(pair.Key);
					SetItemChecked(idx, defined && idx != -1);
				}
			}

			inSetting = false;
		}

		protected override void OnItemCheck(ItemCheckEventArgs ice)
		{
			if (inSetting)
				return;

			if (ice.Index == -1)
				goto Skip;

			object value = Items[ice.Index];
			if (value == null)
				goto Skip;

			if (!longValues.TryGetValue(value, out long v))
				goto Skip;

			long combined = GetLongValue();

			if (v == 0)
				combined = 0;
			else if (ice.NewValue == CheckState.Checked)
				combined |= v;
			else
				combined ^= v;

			SetLongValue(combined);

			Skip:
			base.OnItemCheck(ice);
		}
	}

	public class EnumFlagsUIEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.DropDown;

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			try
			{
				Type type = context?.PropertyDescriptor?.PropertyType;
				if (type?.IsEnum != true)
					return value;

				IWindowsFormsEditorService svc = provider.GetService<IWindowsFormsEditorService>();
				if (svc == null)
					return value;

				EnumFlagsEditor editor = new EnumFlagsEditor();
				editor.InitializeEditor(type);
				editor.SetValue(value);
				editor.Height = editor.PreferredHeight > 200 ? 200 : editor.PreferredHeight;

				svc.DropDownControl(editor);
				return editor.GetValue();
			}
			catch (Exception ex)
			{
				CoreExceptionDialog.ShowDialog("EnumFlagsUIEditor.EditValue() Exception", ex);
				return value;
			}
		}
	}
}
