using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

using Core.Collections;
using Core.Reflection;

namespace Core.Data
{
	public class CoreProxyCommand : IDisposable
	{
		#region Properties

		public RuntimeMethodHandle Handle => Method.MethodHandle;
		public MethodInfo Method { get; protected set; }
		public string CommandText { get; protected set; }
		public ReadOnlyCollection<string> Parameters { get; protected set; }
		public IObjectActivator Activator { get; protected set; }
		public CoreDataProxy Proxy { get; private set; }

		#endregion Properties

		#region Constructors

		public CoreProxyCommand(CoreDataProxy proxy, MethodInfo info)
		{
			Proxy = proxy;
			Method = info;
			CommandText = GetCommandName(info);

			List<string> names = info.GetParameters().Select(P => "@" + P.Name).ToList();
			Parameters = new ReadOnlyCollection<string>(names);

			try
			{
				Activator = ObjectActivator.GetActivator(info.ReturnType);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Debugger.Break();
			}
		}

		public CoreProxyCommand(CoreProxyCommand item)
		{
			Method = item.Method;
			CommandText = item.CommandText;
			Parameters = new ReadOnlyCollection<string>(item.Parameters);
			Activator = item.Activator;
			Proxy = item.Proxy;
		}

		public void Dispose()
		{
			Method = null;
			CommandText = null;
			Parameters = null;
			Activator = null;
			Proxy = null;
		}

		#endregion Constructors

		#region Create Command

		public SqlCommand CreateCommand(params object[] args)
		{
			if (args == null && Parameters.Count != 0)
				throw new ArgumentNullException(nameof(args));

			if (args.Length != Parameters.Count)
				throw new ArgumentOutOfRangeException(nameof(args));

			SqlCommand command = new SqlCommand();
			command.CommandText = CommandText;
			command.CommandType = CommandType.StoredProcedure;

			for (int i = 0; i < args.Length; i++)
				command.Parameters.AddWithValue(Parameters[i], args[i] ?? DBNull.Value);

			return command;
		}

		public static string GetCommandName(MethodInfo info)
		{
			CoreDataSchemaAttribute sch = info.GetAttribute<CoreDataSchemaAttribute>(true);
			return sch != null ? $"[{sch.Schema}].[{info.Name}]" : $"[dbo].[{info.Name}]";
		}

		#endregion Create Command

		#region Execute Command

		public ValueTripper ExecuteCommand(params object[] args)
		{
			SqlConnection connection = null;
			SqlCommand command = null;

			try
			{
				ValueTripper tripper = (ValueTripper)Activator.InvokeConstructor();
				connection = Proxy.InBatch ? Proxy.Connection : Proxy.CreateConnection(true);
				command = CreateCommand(args);
				command.Connection = connection;
				command.Transaction = Proxy.InTransaction ? Proxy.Transaction : null;

				using (SqlDataReader reader = command.ExecuteReader())
				{
					for (int i = 0; i < tripper.Count; i++)
					{
						Type current = tripper.GetType(i);
						tripper[i] = reader.ReadAuto(current);

						if (!reader.NextResult())
							break;
					}
				}

				return tripper;
			}
			finally
			{
				if (!Proxy.InBatch && !Proxy.InTransaction)
					connection.TryDispose();

				command.TryDispose();
			}
		}

		#endregion Execute Command
	}
}
