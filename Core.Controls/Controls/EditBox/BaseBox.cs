using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	public abstract class BaseBox<TValue> : TextBox
	{
		#region Properties

		#region Logical

		public bool IsValid { get; set; }
		public bool IsMandatory { get; set; }
		public string DisplayFormat { get; set; }
		public string EditFormat { get; set; }

		public TValue Value { get; set; }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text 
		{
			get => FormatValue(Value);
			set => base.Text = value;
		}

		#endregion Logical

		#region Visual

		private Color _borderColor = Color.Gray;
		[DefaultValue(typeof(Color), "Gray")]
		public Color BorderColor
		{
			get
			{
				ColorTriplet? clr = GetColors();
				return clr.HasValue ? clr.Value.Border : _borderColor;
			}
			set
			{
				if (_borderColor == value)
					return;

				_borderColor = value;
				Invalidate();
			}
		}

		public override Color BackColor
		{
			get
			{
				ColorTriplet? clr = GetColors();
				return clr.HasValue ? clr.Value.Background : base.BackColor;
			}
			set => base.BackColor = value;
		}

		public override Color ForeColor
		{
			get
			{
				ColorTriplet? clr = GetColors();
				return clr.HasValue ? clr.Value.Foreground : base.ForeColor;
			}
			set => base.ForeColor = value;
		}

		#endregion Visual

		#endregion Properties

		#region Constructors

		protected BaseBox()
		{

		}

		#endregion Constructors

		#region Methods

		#region ParseValue

		public abstract bool TryParsePartialValue(string text);
		public abstract bool TryParseValue(string text, out TValue value);

		#endregion ParseValue


		#region FormatValue

		public virtual string FormatValue(TValue value)
		{
			if (value == null)
				return string.Empty;
			else if (value is IFormattable f)
				return FormatValue(f, Focused);
			return value.ToString();
		}

		public virtual string FormatValue(IFormattable value, bool focus)
		{
			if (focus)
				return value.ToString(EditFormat, CultureInfo.CurrentCulture);
			else
				return value.ToString(DisplayFormat, CultureInfo.CurrentCulture);
		}

		#endregion FormatValue

		#region Paint

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid);
		}

		protected ColorTriplet? GetColors()
		{
			if (Enabled == false)
				return CoreDesignTime.EditDisabled;
			else if (ReadOnly)
				return CoreDesignTime.EditReadOnly;
			else if (Focused)
				return CoreDesignTime.EditFocused;
			else if (IsValid == false)
				return CoreDesignTime.EditError;
			else if (IsMandatory)
				return CoreDesignTime.EditMandatory;
			//else
			//return CoreDesignTime.EditDefault;

			return null;
		}



		#endregion Paint

		#endregion Methods
	}
}
