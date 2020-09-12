using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;

using Core.Collections;
using Core.Reflection;

namespace Core.Data
{
	#region Interface

	//public interface ICoreDataProxy : ISqlConnectionProvider
	//{
	//	bool InBatch { get; }
	//	bool InTransaction { get; }
	//	SqlConnection Connection { get; }
	//	SqlTransaction Transaction { get; }

	//	void BeginTransaction();
	//	void CommitTransaction();
	//	void RollbackTransaction();

	//	void BeginBatch();
	//	void EndBatch();
	//}

	#endregion Interface

	public class CoreDataProxy : RealProxy, ISqlConnectionProvider//, ICoreDataProxy
	{
		#region Private/Protected

		private ISqlConnectionProvider provider;
		protected CoreDictionary<RuntimeMethodHandle, CoreProxyCommand> methodCache;

		#endregion Private/Protected

		#region Properties

		/// <summary>
		/// SqlConnectionProvider used by this proxy
		/// </summary>
		public ISqlConnectionProvider Provider
		{
			get => provider ?? SqlConnectionProvider.Default;
			set => provider = value;
		}

		public bool InBatch { get; private set; }
		public bool InTransaction { get; private set; }

		public SqlConnection Connection { get; private set; }
		public SqlTransaction Transaction { get; private set; }

		#endregion Properties

		#region Constructors

		public CoreDataProxy(Type type, ISqlConnectionProvider provider = null) :
			base(type)
		{
			Provider = provider;
			methodCache = new CoreDictionary<RuntimeMethodHandle, CoreProxyCommand>("Handle");
		}

		#endregion Constructors

		#region Invoke Proxy

		public override IMessage Invoke(IMessage msg)
		{
			IMethodCallMessage message = msg as IMethodCallMessage;
			try
			{
				CoreProxyCommand cache = GetCache(message);
				ValueTripper tripper = cache.ExecuteCommand(message.Args);
				return new ReturnMessage(tripper, null, 0, message.LogicalCallContext, message);
			}
			catch (Exception ex)
			{
				return new ReturnMessage(ex, message);
			}
		}

		protected CoreProxyCommand GetCache(IMethodCallMessage msg)
		{
			CoreProxyCommand result;
			RuntimeMethodHandle h = msg.MethodBase.MethodHandle;
			if (methodCache.TryGetValue(h, out result))
				return result;

			if (msg.MethodBase is MethodInfo info)
			{
				result = new CoreProxyCommand(this, info);
				methodCache.Add(result);
			}

			return result;
		}

		#endregion Invoke Proxy

		#region Batch/Transaction

		public void BeginTransaction()
		{
			if (InTransaction)
				throw new InvalidOperationException("Cannot start transaction, already in transaction");

			if (InBatch)
				throw new InvalidOperationException("Cannot use transaction while batch active, use only transaction which is also batch");

			Connection = CreateConnection(true);
			Transaction = Connection.BeginTransaction();

			InBatch = true;
			InTransaction = true;
		}

		public void CommitTransaction()
		{
			if (!InTransaction)
				throw new InvalidOperationException("Cannot commit transaction, not in transaction");

			Transaction.Commit();
			Transaction.TryDispose();
			Transaction = null;

			Connection.TryDispose();
			Connection = null;

			InTransaction = false;
			InBatch = false;
		}

		public void RollbackTransaction()
		{
			if (!InTransaction)
				throw new InvalidOperationException("Cannot rollback transaction, not in transaction");

			Transaction.Rollback();
			Transaction.TryDispose();
			Transaction = null;

			Connection.TryDispose();
			Connection = null;

			InTransaction = false;
			InBatch = false;
		}

		public void BeginBatch()
		{
			if (InBatch)
				throw new InvalidOperationException("Cannot start batch, already in batch or transaction");

			Connection = CreateConnection(true);
			Connection = null;

			InBatch = true;
		}

		public void EndBatch()
		{
			if (InTransaction)
				throw new InvalidOperationException("Cannot end batch while transaction is active, use CommitTransaction or RollbackTransaction");

			if (!InBatch)
				throw new InvalidOperationException("Cannot end batch, not in batch");

			Connection.TryDispose();
			Connection = null;

			InBatch = false;
		}

		#endregion Batch/Transaction

		#region ISqlConnectionProvider

		/// <summary>
		/// Used for creating SqlConnection
		/// </summary>
		/// <param name="open"></param>
		/// <returns></returns>
		public virtual SqlConnection CreateConnection(bool open = true) => Provider?.CreateConnection(open);
		public virtual string GetConnectionString() => Provider?.GetConnectionString();

		#endregion ISqlConnectionProvider

		#region Helpers

		public static TProxy CreateTransparentProxy<TProxy>(ISqlConnectionProvider provider)
		{
			CoreDataProxy proxy = new CoreDataProxy(typeof(TProxy), provider);
			return (TProxy)proxy.GetTransparentProxy();
		}

		#endregion Helpers
	}
}
