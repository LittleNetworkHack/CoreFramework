using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.App
{
	public class CoreFormActions
	{
		private int lastMask = 0;
		protected BitVector32 flags;

		protected int flInsert;
		protected int flUpdate;
		protected int flDelete;
		protected int flReadOnly;
		protected int flPrint;

		public bool CanInsert => flags[flInsert];
		public bool CanUpdate => flags[flUpdate];
		public bool CanDelete => flags[flDelete];
		public bool CanReadOnly => flags[flReadOnly];
		public bool CanPrint => flags[flPrint];

		public CoreFormActions() : this(0)
		{

		}

		public CoreFormActions(int data)
		{
			flags = new BitVector32(data);
			CreateFlags();
		}

		protected virtual void CreateFlags()
		{
			flInsert = RegisterFlag();
			flUpdate = RegisterFlag();
			flDelete = RegisterFlag();
			flReadOnly = RegisterFlag();
			flPrint = RegisterFlag();
		}

		protected int RegisterFlag()
		{
			lastMask = BitVector32.CreateMask(lastMask);
			return lastMask;
		}


		private static long data = 0;
		public static long Get64(int bitIndex)
		{
			long mask = 1 << bitIndex;
			return data & mask;
		}

		public static void Set64(int bitIndex, bool value)
		{
			long mask = 1 << bitIndex;
			data = value ? data | mask : data & ~mask;
		}

		public static void Print64()
		{
			Console.WriteLine(data);
		}
	}
}
