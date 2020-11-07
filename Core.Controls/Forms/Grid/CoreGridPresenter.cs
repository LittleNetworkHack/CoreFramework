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
	public class CoreGridPresenter<TViewModel, TRepository, TDataObject> : CoreGridView
		where TDataObject : class
		where TViewModel : CoreGridViewModel<TDataObject>
		where TRepository : ICoreDataRepository<TDataObject>
	{
		#region Properties

		public TRepository Repository { get; }

		public TViewModel ViewModel
		{
			get => coreSource.DataSource as TViewModel;
			set => coreSource.DataSource = value;
		}

		public TDataObject SelectedItem { get; private set; }

		#endregion Properties

		#region Constructors

		protected CoreGridPresenter()
		{
			ViewModel = ObjectActivator.Create<TViewModel>();
			Repository = ObjectActivator.Create<TRepository>();
			InitializeSource();
			GoToState("Default");

			ctrlGrid.SelectionChanged += GridSelectionChanged;
		}

		#endregion Constructors

		#region Methods

		private void InitializeSource()
		{
			ctrlGrid.DataSource = coreSource;
			ctrlGrid.DataMember = nameof(CoreViewModel<TDataObject>.Data);
		}

		protected TDataObject GetSelectedItem()
		{
			var selected = ctrlGrid.SelectedRows;
			if (selected.Count == 0)
				return null;

			TDataObject data = selected[0].DataBoundItem as TDataObject;
			return data;
		}

		public TDataObject LoadSelectedItem()
		{
			if (SelectedItem == null)
				return null;

			var tripper = Repository.GetItem(SelectedItem);

			return tripper.Exceptions.Count > 0 ? null : tripper.Item1;
		}

		protected override IEnumerable<CoreFnStateItem> GetNextStateItems(string current, string next)
		{
			yield return new CoreFnStateItem(Keys.F1, "Pomoć");
			yield return new CoreFnStateItem(Keys.F2, "U redu");
			yield return new CoreFnStateItem(Keys.F4, "Novi");

			if (next == "DataLoaded")
			{
				yield return new CoreFnStateItem(Keys.F9, "Reset");
				yield return new CoreFnStateItem(Keys.F11, "Ispis");
			}
			else if (next == "ItemSelected")
			{
				yield return new CoreFnStateItem(Keys.F5, "Uredi");
				yield return new CoreFnStateItem(Keys.F6, "Detalji");
				yield return new CoreFnStateItem(Keys.F7, "Brisanje");
				yield return new CoreFnStateItem(Keys.F9, "Reset");
				yield return new CoreFnStateItem(Keys.F11, "Ispis");
			}

			yield return new CoreFnStateItem(Keys.F12, "Glavna");
		}

		private void GridSelectionChanged(object sender, EventArgs e)
		{
			TDataObject item = GetSelectedItem();
			if (item != null)
			{
				SelectedItem = item;
				GoToState("ItemSelected");
				return;
			}
		}

		#endregion Methods
	}
}
