using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Collections;

namespace Core.Data
{
	public abstract class CoreDataRepository<TDataObject>
		where TDataObject : class
	{
		#region Fields

		private ISqlConnectionProvider provider;
		protected ISqlConnectionProvider Provider => provider ?? SqlConnectionProvider.Default;

		#endregion Fields

		#region Constructors

		public CoreDataRepository(ISqlConnectionProvider provider = null)
		{
			this.provider = provider;
			InitializeProxy();
		}

		#endregion Constructors

		#region Initialize

		protected abstract void InitializeProxy();

		#endregion Initialize

		#region Operations

		public abstract TDataObject Insert(TDataObject item);
		public abstract TDataObject Update(TDataObject item);
		public abstract TDataObject Delete(TDataObject item);
		public abstract TDataObject GetItem(TDataObject item);

		public TDataObject ExecuteOperation(CoreDataOperation operation, TDataObject item)
		{
			switch (operation)
			{
				case CoreDataOperation.Insert:
					return Insert(item);
				case CoreDataOperation.Update:
					return Update(item);
				case CoreDataOperation.Delete:
					return Delete(item);
				case CoreDataOperation.Get:
					return GetItem(item);
				case CoreDataOperation.Default:
				default:
					return null;
			}
		}

		#endregion Operations
	}
}
