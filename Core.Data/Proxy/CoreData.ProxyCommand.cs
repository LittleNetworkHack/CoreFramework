using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
		public CoreCollection<string> Parameters { get; protected set; }
		public IObjectActivator Activator { get; protected set; }
		public CoreDataProxy Proxy { get; private set; }

		#endregion Properties

		#region Constructors

		public CoreProxyCommand(CoreDataProxy proxy, MethodInfo info)
		{
			Proxy = proxy;
			Method = info;
			CommandText = GetCommandName(info);

			IEnumerable<string> names = info.GetParameters().Select(P => "@" + P.Name);
			Parameters = new CoreCollection<string>(names);
			Parameters.LockObject();
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
			Parameters = new CoreCollection<string>(item.Parameters);
			Parameters.LockObject();
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

		public ValueTripper ExecuteCommand(object[] args)
		{
			SqlConnection connection = null;
			SqlCommand command = null;
			ValueTripper tripper = (ValueTripper)Activator.InvokeConstructor();
			SqlInfoMessageEventHandler handler = (s, e) => tripper.Exceptions.Add(new WarningException(e.Message));
			try
			{
				connection = Proxy.InBatch ? Proxy.Connection : Proxy.CreateConnection(true);
				connection.InfoMessage += handler;

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
			catch (Exception ex)
			{
				tripper.Exceptions.Add(ex);
				return tripper;
			}
			finally
			{
				if (connection != null)
					connection.InfoMessage -= handler;

				if (!Proxy.InBatch && !Proxy.InTransaction)
					connection.TryDispose();

				command.TryDispose();
			}
		}

		#endregion Execute Command
	}
}
