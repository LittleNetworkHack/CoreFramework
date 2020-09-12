using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controls
{
	public class CoreEditViewModel<TDataObject> : CoreViewModel<TDataObject>
		where TDataObject : class
	{
		#region Declarations

		private CoreEditType _EditType = CoreEditType.Default;
		public CoreEditType EditType
		{
			get => _EditType;
			set => SetValue(ref _EditType, value, nameof(EditType));
		}

		#endregion Declarations

		#region Constructors

		protected CoreEditViewModel()
		{

		}

		protected CoreEditViewModel(TDataObject data) : this(data, CoreEditType.Default)
		{

		}

		protected CoreEditViewModel(TDataObject data, CoreEditType type) : base(data)
		{
			_EditType = type;
		}

		#endregion Constructors
	}
}
