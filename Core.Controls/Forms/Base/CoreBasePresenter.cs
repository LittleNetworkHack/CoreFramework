using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	public class CoreBasePresenter : CoreBaseView
	{
		#region Constructors

		protected CoreBasePresenter()
		{

		}

		#endregion Constructors

		#region Initialize Methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			InitializeDelegates();
			GoToState("Default");
		}

		private void InitializeDelegates()
		{
			ctrlKeys.ButtonClick += OnFnButtonClick;
			//ctrlKeys.F1.Click += OnF1Clicked;
			//ctrlKeys.F2.Click += OnF2Clicked;
			//ctrlKeys.F3.Click += OnF3Clicked;
			//ctrlKeys.F4.Click += OnF4Clicked;
			//ctrlKeys.F5.Click += OnF5Clicked;
			//ctrlKeys.F6.Click += OnF6Clicked;
			//ctrlKeys.F7.Click += OnF7Clicked;
			//ctrlKeys.F8.Click += OnF8Clicked;
			//ctrlKeys.F9.Click += OnF9Clicked;
			//ctrlKeys.F10.Click += OnF10Clicked;
			//ctrlKeys.F11.Click += OnF11Clicked;
			//ctrlKeys.F12.Click += OnF12Clicked;
		}

		#endregion Initialize Methods

		#region FnStates

		protected void GoToState(string state)
		{
			CoreFnState next = new CoreFnState(state);
			CoreFnState current = ctrlKeys.CurrentState;
			IEnumerable<CoreFnStateItem> items = GetNextStateItems(current.Name, state);
			next.Items.AddRange(items);
			ctrlKeys.CurrentState = next;
		}

		protected virtual IEnumerable<CoreFnStateItem> GetNextStateItems(string current, string next)
		{
			if (next == "Default")
			{
				yield return new CoreFnStateItem(Keys.F1, "Pomoć");
				yield return new CoreFnStateItem(Keys.F12, "Glavna");
				yield break;
			}
		}

		#endregion FnStates

		#region OnFnClicked

		protected virtual void OnFnButtonClick(CoreFnKeyEventArgs args)
		{
			switch (args.Key)
			{
				case Keys.F1:
					OnF1Clicked(args.State);
					break;
				case Keys.F2:
					OnF2Clicked(args.State);
					break;
				case Keys.F3:
					OnF3Clicked(args.State);
					break;
				case Keys.F4:
					OnF4Clicked(args.State);
					break;
				case Keys.F5:
					OnF5Clicked(args.State);
					break;
				case Keys.F6:
					OnF6Clicked(args.State);
					break;
				case Keys.F7:
					OnF7Clicked(args.State);
					break;
				case Keys.F8:
					OnF8Clicked(args.State);
					break;
				case Keys.F9:
					OnF9Clicked(args.State);
					break;
				case Keys.F10:
					OnF10Clicked(args.State);
					break;
				case Keys.F11:
					OnF11Clicked(args.State);
					break;
				case Keys.F12:
					OnF12Clicked(args.State);
					break;
			}
		}

		protected virtual void OnF1Clicked(string state)
		{

		}

		protected virtual void OnF2Clicked(string state)
		{

		}

		protected virtual void OnF3Clicked(string state)
		{

		}

		protected virtual void OnF4Clicked(string state)
		{

		}

		protected virtual void OnF5Clicked(string state)
		{

		}

		protected virtual void OnF6Clicked(string state)
		{

		}

		protected virtual void OnF7Clicked(string state)
		{

		}

		protected virtual void OnF8Clicked(string state)
		{

		}

		protected virtual void OnF9Clicked(string state)
		{

		}

		protected virtual void OnF10Clicked(string state)
		{

		}

		protected virtual void OnF11Clicked(string state)
		{

		}

		protected virtual void OnF12Clicked(string state)
		{

		}

		#endregion OnFnClicked
	}
}
