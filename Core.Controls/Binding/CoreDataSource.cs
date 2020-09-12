using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Reflection;

namespace Core.Controls
{
	public class CoreDataSource : Component, INotifyPropertyChanged
	{
		#region Fields

		private object _dataSource;
		private string _coreMember;
		private CoreDataSource _coreSource;

		#endregion Fields

		#region Properties

		public bool IsOwned => CoreSource != null;
		public bool IsNested => IsOwned && CoreMember != string.Empty;

		public Type DataType
		{
			get
			{
				if (IsNested)
					return CoreSource.Properties[CoreMember].PropertyType;
				else if (IsOwned)
					return CoreSource.DataType;

				return DataSource?.GetType();
			}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object DataSource
		{
			get
			{
				if (IsNested)
					return CoreSource.GetValue(CoreMember);
				else if (IsOwned)
					return CoreSource.DataSource;

				return _dataSource;
			}
			set
			{
				if (IsOwned)
					throw new InvalidOperationException("Cannot set DataSource while bound to CoreDataSource!");

				if (ReferenceEquals(_dataSource, value))
					return;

				_dataSource = value;
				Initialize();
			}
		}

		[DefaultValue("")]
		[Editor(typeof(CoreMemberTypeConverter), typeof(UITypeEditor))]
		public string CoreMember
		{
			get => _coreMember ?? string.Empty;
			set
			{
				if (_coreMember == value)
					return;

				_coreMember = value;
				Initialize();
			}
		}

		[DefaultValue(null)]
		public CoreDataSource CoreSource
		{
			get => _coreSource;
			set
			{
				if (ReferenceEquals(this, value))
					throw new Exception("CoreSource cannot be itself!");

				if (ReferenceEquals(_coreSource, value))
					return;

				CoreSourceHook(false);
				_coreSource = value;
				CoreSourceHook(true);
				Initialize();
			}
		}

		[Browsable(false)]
		public PropertyKeyCollection Properties { get; private set; }

		public event EventHandler DataSourceChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Properties

		#region Constructors

		public CoreDataSource()
		{
			_dataSource = new TestModel();
			Initialize();
		}

		#endregion Constructors

		#region Helper Methods

		protected void Initialize()
		{
			CreatePropertyCollection();
			InvokeDataSourceChanged();
			foreach (IPropertyKey key in Properties)
				InvokePropertyChanged(key.Name);
		}

		protected void InvokePropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		protected void InvokeDataSourceChanged()
		{
			DataSourceChanged?.Invoke(this, EventArgs.Empty);
		}

		protected void CreatePropertyCollection()
		{
			Type type = DataType;
			Properties = type == null ? new PropertyKeyCollection() : new PropertyKeyCollection(type);
		}

		#endregion Helper Methods

		#region Wire Methods

		protected void CoreSourceHook(bool hook)
		{
			if (CoreSource == null)
				return;

			if (hook)
			{
				CoreSource.PropertyChanged += CoreSource_PropertyChanged;
				CoreSource.DataSourceChanged += CoreSource_DataSourceChanged;
			}
			else
			{
				CoreSource.PropertyChanged -= CoreSource_PropertyChanged;
				CoreSource.DataSourceChanged -= CoreSource_DataSourceChanged;
			}
		}

		private void CoreSource_DataSourceChanged(object sender, EventArgs args)
		{
			Initialize();
		}

		private void CoreSource_PropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (!IsNested || args.PropertyName != CoreMember)
				return;

			Initialize();
		}

		#endregion Wire Methods

		#region Methods

		public TValue GetValue<TValue>(string name)
		{
			object value = GetValue(name);
			return CoreConverter.ConvertTo<TValue>(value);
		}

		public object GetValue(string name)
		{
			IPropertyKey key = Properties[name];
			return key.GetBoxedValue(DataSource);
		}

		public void SetValue(string name, object value)
		{
			IPropertyKey key = Properties[name];
			object current = key.GetBoxedValue(DataSource);
			value = CoreConverter.ConvertTo(value, key.PropertyType);

			if (key.EqualityComparer.Equals(current, value))
				return;

			key.SetBoxedValue(DataSource, value);
			InvokePropertyChanged(name);
		}

		#endregion Methods
	}

	public class TestModel
	{
		public int ID { get; set; }
		public bool Flag { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public decimal Money { get; set; }

		public TestNestedModel Child { get; set; }

		public TestModel()
		{
			ID = 3;
			Flag = true;
			Name = "aaaa";
			Date = DateTime.Now;
			Money = 2500.23M;
			Child = new TestNestedModel();
		}

	}

	public class TestNestedModel
	{
		public string Something { get; set; }
		public string Nested { get; set; }

		public TestNestedModel()
		{

		}
	}
}
