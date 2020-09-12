using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Core.ComponentModel;

namespace Core.Data
{
	public class CoreConnectionProvider : NotifyDecriptorBase, ISqlConnectionProvider
	{
		#region Fields

		protected SqlConnectionStringBuilder builder;

		protected string _Server;
		protected string _Database;
		protected string _Username;
		protected string _Password;
		protected string _ConnectionString;

		#endregion Fields

		#region Properties

		public virtual string ConnectionString
		{
			get => builder.ConnectionString;
			set
			{
				if (AreEqual(builder.ConnectionString, value))
					return;

				builder.ConnectionString = value;
				OnPropertyChanged(nameof(ConnectionString));
			}
		}

		public virtual string Server
		{
			get => builder.DataSource;
			set
			{
				if (AreEqual(builder.DataSource, value))
					return;

				builder.DataSource = value;
				OnPropertyChanged(nameof(Server));
			}
		}

		public virtual string Database
		{
			get => builder.InitialCatalog;
			set
			{
				if (AreEqual(builder.InitialCatalog, value))
					return;

				builder.InitialCatalog = value;
				OnPropertyChanged(nameof(Database));
			}
		}

		public virtual string Username
		{
			get => builder.UserID;
			set
			{
				if (AreEqual(builder.UserID, value))
					return;

				builder.UserID = value;
				OnPropertyChanged(nameof(Username));
			}
		}

		public virtual string Password
		{
			get => builder.Password;
			set
			{
				if (AreEqual(builder.Password, value))
					return;

				builder.Password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

		public virtual bool Trusted
		{
			get => builder.IntegratedSecurity;
			set
			{
				if (AreEqual(builder.IntegratedSecurity, value))
					return;

				builder.IntegratedSecurity = value;
				OnPropertyChanged(nameof(Trusted));
			}
		}

		#endregion Properties

		#region Constructors

		public CoreConnectionProvider()
		{
			builder = new SqlConnectionStringBuilder();
		}

		public CoreConnectionProvider(string connString)
		{
			builder = new SqlConnectionStringBuilder(connString);
			ConnectionString = connString;
		}

		/// <summary>
		/// Standard Security SQL Connection
		/// </summary>
		/// <param name="server">IP or Hostname</param>
		/// <param name="database">Initial Catalog</param>
		/// <param name="username">SQL User ID</param>
		/// <param name="password">SQL User Password</param>
		public CoreConnectionProvider(string server, string database, string username, string password)
		{
			builder = new SqlConnectionStringBuilder();
			builder.DataSource = server;
			builder.InitialCatalog = database;
			builder.UserID = username;
			builder.Password = password;
		}

		/// <summary>
		/// Trusted Security SQL Connection
		/// </summary>
		/// <param name="server">IP or Hostname</param>
		/// <param name="database">Initial Catalog</param>
		/// <param name="trusted">Flag for 'Trusted_Connection' (should be true)</param>
		public CoreConnectionProvider(string server, string database, bool trusted = true)
		{
			builder.DataSource = server;
			builder.InitialCatalog = database;
			builder.IntegratedSecurity = true;
		}


		/// <summary>
		/// Clone Constructor
		/// </summary>
		/// <param name="item"></param>
		public CoreConnectionProvider(CoreConnectionProvider item) : this(item.GetConnectionString())
		{
		}

		public CoreConnectionProvider(SqlConnectionStringBuilder builder) : this(builder.ConnectionString)
		{
		}

		#endregion Constructors

		#region Methods

		public virtual SqlConnection CreateConnection(bool open = true)
		{
			string connectionString = GetConnectionString();
			SqlConnection connection = new SqlConnection(connectionString);
			if (open)
				connection.Open();
			return connection;
		}

		public virtual string GetConnectionString() => builder.ConnectionString;

		#endregion Methods
	}
}
