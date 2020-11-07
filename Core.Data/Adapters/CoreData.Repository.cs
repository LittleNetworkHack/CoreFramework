using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Core.Collections;

namespace Core.Data
{
	public interface ICoreDataRepository<TDataObject>
		where TDataObject : class
	{
		ValueTripper<TDataObject> Insert(TDataObject item);
		ValueTripper<TDataObject> Update(TDataObject item);
		ValueTripper Delete(TDataObject item);
		ValueTripper<TDataObject> GetItem(TDataObject item);
	}

	public abstract class CoreDataRepository<TProxy, TDataObject> : CoreDataAccessObject<TProxy>, ICoreDataRepository<TDataObject>
		where TProxy : class
		where TDataObject : class
	{
		#region Constructors

		public CoreDataRepository(ISqlConnectionProvider provider = null) : base(provider)
		{
		}

		#endregion Constructors

		#region Operations

		public abstract ValueTripper<TDataObject> Insert(TDataObject item);
		public abstract ValueTripper<TDataObject> Update(TDataObject item);
		public abstract ValueTripper Delete(TDataObject item);
		public abstract ValueTripper<TDataObject> GetItem(TDataObject item);

		public ValueTripper ExecuteOperation(CoreDataOperation operation, TDataObject item)
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
