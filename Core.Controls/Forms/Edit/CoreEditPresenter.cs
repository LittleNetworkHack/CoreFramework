using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Data;
using Core.Reflection;

namespace Core.Controls
{
	public class CoreEditPresenter<TViewModel, TRepository, TDataObject> : CoreEditView
	where TDataObject : class
		where TViewModel : CoreEditViewModel<TDataObject>
		where TRepository : ICoreDataRepository<TDataObject>
	{
		#region Declarations

		public TRepository Repository { get; }

		public TViewModel ViewModel
		{
			get => coreSource.DataSource as TViewModel;
			set => coreSource.DataSource = value;
		}

		public TDataObject DataObject
		{
			get => ViewModel.Data;
			set => ViewModel.Data = value;
		}

		#endregion Declarations

		#region Constructors

		protected CoreEditPresenter()
		{
			ViewModel = ObjectActivator.Create<TViewModel>();
			Repository = ObjectActivator.Create<TRepository>();
		}

		protected CoreEditPresenter(TViewModel model)
		{
			ViewModel = model;
			Repository = ObjectActivator.Create<TRepository>();
		}

		#endregion Constructors

		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			coreEditManager.ApplyBehaviors(ViewModel.EditType);
			GoToState(ViewModel.EditType.ToString());
		}

		#endregion Methods

		#region FnPanel Operations

		protected override IEnumerable<CoreFnStateItem> GetNextStateItems(string current, string next)
		{
			yield return new CoreFnStateItem(Keys.F1, "Pomoć");
			yield return new CoreFnStateItem(Keys.F2, "U redu");

			if (next == "Insert" || next == "Update" || next == "Delete")
				yield return new CoreFnStateItem(Keys.F8, "Odustani");

			yield return new CoreFnStateItem(Keys.F12, "Glavna");
		}

		protected override void OnF2Clicked(string state)
		{
			ExecuteOperation();
			Close();
		}

		protected override void OnF8Clicked(string state)
		{
			Close();
		}

		#endregion FnPanel Operations

		#region Repository Operations

		protected virtual void ExecuteOperation()
		{
			coreSource.EndEdit();

			switch (ViewModel.EditType)
			{
				case CoreEditType.Insert:
					ExecuteInsert();
					break;
				case CoreEditType.Update:
					ExecuteUpdate();
					break;
				case CoreEditType.Delete:
					ExecuteDelete();
					break;
				case CoreEditType.View:
					break;
				case CoreEditType.Default:
				default:
					throw new InvalidOperationException("Operation not specified!");
			}
		}

		protected virtual bool ValidateInsert() => true;
		protected virtual TDataObject ExecuteInsert() => ValidateInsert() ? Repository.Insert(ViewModel.Data) : null;

		protected virtual bool ValidateUpdate() => true;
		protected virtual TDataObject ExecuteUpdate() => ValidateUpdate() ? Repository.Update(ViewModel.Data) : null;

		protected virtual bool ValidateDelete() => true;
		protected virtual TDataObject ExecuteDelete() => ValidateDelete() ? Repository.Delete(ViewModel.Data) : null;

		#endregion Repository Operations
	}
}
