using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Core.Data
{ 
    public static class SqlConnectionProvider
    {
		#region DevEnv Providers

		public static readonly CoreConnectionProvider Provider_LNH_Core = new CoreConnectionProvider();
		public static readonly CoreConnectionProvider Provider_LNH_Ref = new CoreConnectionProvider();
		
		#endregion DevEnv Providers

		#region SqlConnectionProvider

		public static ISqlConnectionProvider Default { get; set; }

		static SqlConnectionProvider()
		{
			
		}

		public static void SetDefault()
		{
			Default = Provider_LNH_Ref;
		}

		public static SqlConnection CreateConnection()
		{
			if (Default == null)
				throw new NullReferenceException($"{nameof(Default)} in {nameof(SqlConnectionProvider)} not set!");

			return Default.CreateConnection(true);
		}

		#endregion SqlConnectionProvider

		#region SqlConnection Helpers

		public static readonly string ConnectionTemplate = "Server={0};Database={1};User Id={2};Password={3};";

		public static string FormatConnectionString(string server, string database, string username, string password)
		{
			return new CoreConnectionProvider(server, database, username, password).ConnectionString;
		}

		#endregion SqlConnection Helpers

		#region SqlCommand Helpers

		public static SqlCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			SqlCommand command = new SqlCommand(commandText);
			command.CommandType = commandType;

			if (parameters == null)
				return command;

			foreach (SqlParameter parameter in parameters)
				command.Parameters.Add(parameter);

			return command;
		}

		public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			SqlCommand command = connection.CreateCommand();
			command.CommandText = commandText;
			command.CommandType = commandType;

			if (parameters == null)
				return command;

			foreach (SqlParameter parameter in parameters)
				command.Parameters.Add(parameter);

			return command;
		}

		public static SqlCommand WithConnection(this SqlCommand command, SqlConnection connection)
		{
			command.Connection = connection;
			return command;
		}

		#endregion SqlCommand Helpers
	}
}
