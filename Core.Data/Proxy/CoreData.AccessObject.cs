using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace Core.Data
{
	public abstract class CoreDataAccessObject<TProxy> : ISqlConnectionProvider
		where TProxy : class
	{
		#region Declarations

		private ISqlConnectionProvider provider;
		protected ISqlConnectionProvider Provider => provider ?? SqlConnectionProvider.Default;

		protected TProxy Proxy { get; private set; }

		#endregion Declarations

		#region Constructors

		public CoreDataAccessObject(string connectionString) : this(new CoreConnectionProvider(connectionString)) { }
		public CoreDataAccessObject(ISqlConnectionProvider provider = null)
		{
			this.provider = provider;
			Proxy = CoreDataProxy.CreateTransparentProxy<TProxy>(this);
		}

		#endregion Constructors

		#region ISqlConnectionProvider

		public virtual string GetConnectionString() => Provider.GetConnectionString();
		public virtual SqlConnection CreateConnection(bool open = true) => Provider.CreateConnection(open);

		#endregion ISqlConnectionProvider
	}
}
