using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Reflection;

namespace Core.Controls
{
	public class CoreViewModel<TData> : INotifyPropertyChanged
		where TData : class
	{
		#region Declarations

		private TData _Data;

		public TData Data
		{
			get => _Data;
			set => SetValue(ref _Data, value, nameof(Data));
		}

		#endregion Declarations

		#region Constructors

		protected CoreViewModel()
		{
			Data = ObjectActivator.Create<TData>();
		}

		protected CoreViewModel(TData data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			Data = data;
		}

		#endregion Constructors

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void InvokePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		protected virtual void OnPropertyChanged(string propertyName, params string[] properties)
		{
			InvokePropertyChanged(propertyName);
			if (properties == null)
				return;
			foreach (string property in properties)
				InvokePropertyChanged(property);
		}
		protected void SetValue<T>(ref T field, T value, string propertyName, params string[] properties)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
				return;

			field = value;
			OnPropertyChanged(propertyName, properties);

		}
		#endregion INotifyPropertyChanged
	}
}
