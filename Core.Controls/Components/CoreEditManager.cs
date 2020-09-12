using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Collections;

namespace Core.Controls.Components
{
	[ProvideProperty("OnInsert", typeof(Control))]
	[ProvideProperty("OnUpdate", typeof(Control))]
	[ProvideProperty("OnDelete", typeof(Control))]
	[ProvideProperty("OnView", typeof(Control))]
	public class CoreEditManager : Component, IExtenderProvider
	{
		#region Fields

		protected Dictionary<Control, EditControlBehavior> bInsert;
		protected Dictionary<Control, EditControlBehavior> bUpdate;
		protected Dictionary<Control, EditControlBehavior> bDelete;
		protected Dictionary<Control, EditControlBehavior> bView;

		#endregion Fields

		#region Constructors

		public CoreEditManager()
		{
			bInsert = new Dictionary<Control, EditControlBehavior>();
			bUpdate = new Dictionary<Control, EditControlBehavior>();
			bDelete = new Dictionary<Control, EditControlBehavior>();
			bView = new Dictionary<Control, EditControlBehavior>();
		}

		public CoreEditManager(IContainer container) : this()
		{
			container.Add(this);
		}

		#endregion Constructors

		#region Methods

		public void ApplyBehaviors(CoreEditType type)
		{
			Dictionary<Control, EditControlBehavior> dict;

			switch (type)
			{
				case CoreEditType.Insert:
					dict = bInsert;
					break;
				case CoreEditType.Update:
					dict = bUpdate;
					break;
				case CoreEditType.Delete:
					dict = bDelete;
					break;
				case CoreEditType.View:
					dict = bView;
					break;
				case CoreEditType.Default:
				default:
					dict = null;
					break;
			}

			if (dict == null)
				return;

			foreach (KeyValuePair<Control, EditControlBehavior> pair in dict)
				ApplyControlBehavior(pair.Key, pair.Value);
		}

		protected void ApplyControlBehavior(Control ctrl, EditControlBehavior value)
		{
			switch (value)
			{
				case EditControlBehavior.Enabled:
					ctrl.Enabled = true;
					break;
				case EditControlBehavior.Disabled:
					ctrl.Enabled = false;
					break;
				case EditControlBehavior.ReadOnly:
				case EditControlBehavior.Default:
				default:
					break;
			}
		}

		#endregion Methods

		#region IExtenderProvider

		bool IExtenderProvider.CanExtend(object extendee)
		{
			return extendee is Control;
		}

		#region Shared

		protected EditControlBehavior GetBehavior(Dictionary<Control, EditControlBehavior> dict, Control ctrl)
		{
			if (dict.TryGetValue(ctrl, out EditControlBehavior value))
				return value;

			return EditControlBehavior.Default;
		}

		protected void SetBehavior(Dictionary<Control, EditControlBehavior> dict, Control ctrl, EditControlBehavior value)
		{
			switch (value)
			{
				case EditControlBehavior.Enabled:
				case EditControlBehavior.Disabled:
				case EditControlBehavior.ReadOnly:
					dict[ctrl] = value;
					break;
				case EditControlBehavior.Default:
				default:
					dict.Remove(ctrl);
					break;
			}
		}

		#endregion Shared

		#region Insert

		[DefaultValue(EditControlBehavior.Default)]
		[Category(CoreDesignTime.C_Component)]
		public EditControlBehavior GetOnInsert(Control ctrl) => GetBehavior(bInsert, ctrl);

		public void SetOnInsert(Control ctrl, EditControlBehavior value) => SetBehavior(bInsert, ctrl, value);

		#endregion Insert

		#region Update

		[DefaultValue(EditControlBehavior.Default)]
		[Category(CoreDesignTime.C_Component)]
		public EditControlBehavior GetOnUpdate(Control ctrl) => GetBehavior(bUpdate, ctrl);

		public void SetOnUpdate(Control ctrl, EditControlBehavior value) => SetBehavior(bUpdate, ctrl, value);

		#endregion Update

		#region Delete

		[DefaultValue(EditControlBehavior.Default)]
		[Category(CoreDesignTime.C_Component)]
		public EditControlBehavior GetOnDelete(Control ctrl) => GetBehavior(bDelete, ctrl);

		public void SetOnDelete(Control ctrl, EditControlBehavior value) => SetBehavior(bDelete, ctrl, value);

		#endregion Delete

		#region View

		[DefaultValue(EditControlBehavior.Default)]
		[Category(CoreDesignTime.C_Component)]
		public EditControlBehavior GetOnView(Control ctrl) => GetBehavior(bView, ctrl);

		public void SetOnView(Control ctrl, EditControlBehavior value) => SetBehavior(bView, ctrl, value);

		#endregion View

		#endregion IExtenderProvider
	}

	public enum EditControlBehavior
	{
		Default,
		Enabled,
		Disabled,
		ReadOnly
	}
}
