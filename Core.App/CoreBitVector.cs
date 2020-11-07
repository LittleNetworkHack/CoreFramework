using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.App
{
	public class CoreBitVector
	{
		#region Fields & Properties

		protected const ulong shifter = 1;

		protected ulong bits = 0;
		protected string[] names = new string[64];

		public bool this[int bitIndex]
		{
			get
			{
				ThrowIfOutOfRange(bitIndex);
				ulong mask = shifter << bitIndex;
				return (bits & mask) != 0;
			}
			set
			{
				ThrowIfOutOfRange(bitIndex);
				ulong mask = shifter << bitIndex;
				bits = value ? bits | mask : bits & ~mask;
			}
		}

		#endregion Fields & Properties

		#region Constructors

		public CoreBitVector() : this(0, null)
		{

		}

		public CoreBitVector(ulong bits) : this(bits, null)
		{
			
		}

		public CoreBitVector(ulong bits, params string[] names)
		{
			this.bits = bits;

			int? len = names?.Length;
			if (len.HasValue)
			{
				ThrowIfOutOfRange(len.Value - 1);
				for (int i = 0; i < len.Value; i++)
					this.names[i] = names[i];
			}
		}

		#endregion Constructors

		#region Methods

		public string GetBitName(int bitIndex)
		{
			ThrowIfOutOfRange(bitIndex);
			return names[bitIndex];
		}

		public void SetBitName(int bitIndex, string name)
		{
			ThrowIfOutOfRange(bitIndex);
			names[bitIndex] = name;
		}

		protected void ThrowIfOutOfRange(int index)
		{
			if (index > -1 && index < 64)
				return;

			throw new ArgumentOutOfRangeException(nameof(index));
		}

		#endregion Methods
	}

	public class CoreGridPerm : CoreBitVector
	{
		public bool CanInsert
		{
			get => this[0];
			set => this[0] = value;
		}

		public CoreGridPerm()
		{
			SetBitName(0, "Insert");
		}
	}
}
