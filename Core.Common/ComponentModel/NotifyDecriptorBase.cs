using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Core.ComponentModel
{
    public abstract class NotifyDecriptorBase : INotifyPropertyChanged, ILockable, ICloneable
    {
		#region ILockable

		private bool isLocked = false;

		public virtual void LockObject() => isLocked = true;
		public virtual bool IsLocked() => isLocked;

		#endregion ILockable

		#region PropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected void InvokePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			InvokePropertyChanged(propertyName);
		}

		protected void OnPropertiesChanged(params string[] properties)
		{
			if (properties == null)
				return;

			foreach (string name in properties)
				OnPropertyChanged(name);
		}

		#endregion PropertyChanged

		#region Methods

		protected bool AreEqual<T>(T current, T value) => EqualityComparer<T>.Default.Equals(current, value);

		protected void SetValue<T>(ref T field, T value, string name, params string[] properties)
		{
			if (isLocked)
				throw new InvalidOperationException("Object is locked.");

			if (EqualityComparer<T>.Default.Equals(field, value))
				return;

			field = value;
			OnPropertyChanged(name);
			OnPropertiesChanged(properties);
		}

		#endregion Methods

		#region ICloneable

		public virtual object Clone() => Activator.CreateInstance(GetType(), this);

		#endregion ICloneable
	}
}
