using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

using Core.ComponentModel;
using Core.Reflection;

namespace Core.Controls
{
	[Designer("System.Windows.Forms.Design.TextBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public abstract class BaseEditBox<TValue> : TextBox
	{
		#region Properties

		#region Fields

		private bool _isValid = true;
		private bool _isMandatory = false;

		private TValue _value;

		#endregion Fields

		[DefaultValue(null)]
		[Category("_Core")]
		[TypeConverter(typeof(CoreTypeConverter))]
		public TValue Value
		{
			get => _value;
			set => SetPropValue(ref _value, value);
		}

		[DefaultValue(true)]
		[Category("_Core")]
		public bool IsValid
		{
			get => _isValid;
			set => SetPropValue(ref _isValid, value, ForcePaint);
		}

		[DefaultValue(false)]
		[Category("_Core")]
		public bool IsMandatory
		{
			get => _isMandatory;
			set => SetPropValue(ref _isMandatory, value, ForcePaint);
		}

		#endregion Properties

		#region Constructors

		protected BaseEditBox()
		{
		}

		#endregion Constructors

		#region Core

		const int WM_CUT = 0x0300;
		const int WM_PASTE = 0x0302;

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WM_CUT:
					WmCut(ref m);
					break;
				case WM_PASTE:
					WmPaste(ref m);
					break;
				default:
					base.WndProc(ref m);
					break;
			}
		}

		protected bool SetPropValue<T>(ref T field, T value, Action method = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
				return false;

			field = value;
			method?.Invoke();
			return true;
		}

		protected override void OnEnter(EventArgs e)
		{
			if (IsValidValue)
				Text = FormatValue(Value, true);
			ForcePaint();
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			if (IsValidValue)
				Text = FormatValue(Value, false);
			ForcePaint();
			base.OnLeave(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			ForcePaint();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			ForcePaint();
			base.OnLostFocus(e);
		}

		public void SetValue(TValue value)
		{
			if (EqualityComparer<TValue>.Default.Equals(_value, value))
				return;

			_value = value;
			Text = FormatValue(Value, Focused);
			Invalidate();
		}

		#endregion Core

		#region Validate

		private ToolTip ttValid = new ToolTip();
		protected bool IsValidValue => Text == string.Empty || TryParseValue(Text, out _);

		protected override void OnValidating(CancelEventArgs e)
		{
			if (!IsValidValue)
			{
				e.Cancel = true;
				ttValid.Show("Neispravna vrijednost!", this, Width + 4, 0, 2500);
			}

			base.OnValidating(e);
		}

		#endregion Validate

		#region Process Text

		public abstract bool TryParsePartialValue(string text);
		public abstract bool TryParseValue(string text, out TValue value);
		public virtual bool IsValueEmtpy(TValue value) => value == null;

		protected bool ProcessText(string input) => ProcessText(input, out _);
		protected bool ProcessText(string input, out string previewText)
		{
			previewText = GetPreviewText(input);
			bool pValid = TryParsePartialValue(previewText);
			bool fValid = TryParseValue(previewText, out TValue v);
			if (pValid && fValid)
				_value = v;
			else if (pValid)
				_value = default(TValue);

			return pValid;
		}

		protected string GetPreviewText(string input)
		{
			string current = base.Text;
			int ss = SelectionStart;
			int sl = SelectionLength;

			if (input == "\b")
			{
				if (current.Length == 0)
					return current;

				ss = ss > 0 ? ss - 1 : 0;
				sl = sl > 0 ? sl : 1;

				current = current.Remove(ss, sl);
				return current;
			}
			else if (ss != -1 && sl != 0)
			{
				current = current.Remove(ss, sl);
			}

			current = current.Insert(ss, input);
			return current;
		}

		#region Overrides

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get => base.Text;
			set => base.Text = value;
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (!ProcessText(e.KeyChar.ToString()))
				e.Handled = true;

			base.OnKeyPress(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				e.Handled = true;

			base.OnKeyDown(e);
		}

		protected virtual void WmPaste(ref Message m)
		{
			if (Clipboard.ContainsText() && ProcessText(Clipboard.GetText()))
				base.WndProc(ref m);
		}

		protected virtual void WmCut(ref Message m)
		{
			if (ProcessText(string.Empty))
				base.WndProc(ref m);
		}

		#endregion Overrides

		#endregion Process Text

		#region Format Text

		private string _editFormat = null;
		private string _displayFormat = null;

		[DefaultValue(null)]
		[Category("_Core")]
		public virtual string EditFormat
		{
			get => _editFormat;
			set => SetPropValue(ref _editFormat, value, Invalidate);
		}

		[DefaultValue(null)]
		[Category("_Core")]
		public virtual string DisplayFormat
		{
			get => _displayFormat;
			set => SetPropValue(ref _displayFormat, value, Invalidate);
		}

		public virtual string FormatValue(TValue value, bool isEditing)
		{
			if (value == null)
				return string.Empty;
			else if (value is IFormattable f)
				return FormatValue(f, isEditing);
			return value.ToString();
		}

		public virtual string FormatValue(IFormattable value, bool isEditing)
		{
			if (isEditing)
				return value.ToString(EditFormat, CultureInfo.CurrentCulture);
			else
				return value.ToString(DisplayFormat, CultureInfo.CurrentCulture);
		}

		#endregion Format Text

		#region Paint

		protected Color BorderColor => this.GetBorderColor();

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
		{
			get => this.GetBackgroundColor();
			set => base.BackColor = value;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get => this.GetForegroundColor();
			set => base.ForeColor = value;
		}

		protected void ForcePaint()
		{
			BackColor = BackColor;
			OnBackColorChanged(EventArgs.Empty);
			ForeColor = ForeColor;
			OnForeColorChanged(EventArgs.Empty);
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid);
		}

		#endregion Paint
	}
}
