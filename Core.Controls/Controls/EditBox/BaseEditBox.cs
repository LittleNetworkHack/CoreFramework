using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

using Core.Reflection;

namespace Core.Controls
{
	[Designer("System.Windows.Forms.Design.TextBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public abstract class BaseEditBox<TValue> : TextBox, ICoreControl
    {
        #region Internal

        public class EditBoxStateManager : StateComponent<TextBox>
        {
            public string Text { get; set; }
            public int SelectionStart { get; set; }
            public int SelectionLength { get; set; }

            public EditBoxStateManager()
            {

            }

            public override void SetState(TextBox box)
            {
                Text = box.Text;
                SelectionStart = box.SelectionStart;
                SelectionLength = box.SelectionLength;
            }

            public override void ApplyState(TextBox box)
            {
                box.Text = Text;
                box.Select(SelectionStart, SelectionLength);
            }
        }

        #endregion Internal

        #region Private/Protected

        private TValue _Value;
        private Color _BorderColor = Color.Gray;

        protected EditBoxStateManager BoxCache;

        #endregion Private/Protected

        #region Public Properties

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text { get => base.Text; set => base.Text = value; }

		public event EventHandler ValueChanged;
        public event EventHandler ValidChanged;
        
        [DefaultValue(null)]
        [Bindable(true)]
        [Category(CoreDesignTime.C_Component)]
        public TValue Value
        {
            get => _Value;
            set
			{
                if (!SetValue(ref _Value, value, ValueChanged))
                    return;

                SetTextFromValue();
            }
        }

        protected bool _IsValid = true;
        [DefaultValue(true)]
        [Category(CoreDesignTime.C_Component)]
        public bool IsValid
        {
            get => _IsValid;
            set
            {
                if (!SetValue(ref _IsValid, value, ValidChanged))
                    return;

                Invalidate();
            }
        }

        protected bool _IsMandatory = false;
        [DefaultValue(false)]
        [Category(CoreDesignTime.C_Component)]
        public bool IsMandatory
        {
            get => _IsMandatory;
            set
            {
                if (!SetValue(ref _IsMandatory, value))
                    return;

                Invalidate();
            }
        }

        #region Appearance

        [DefaultValue(typeof(Font), "Microsoft Sans Serif, 8.25pt")]
        [Category(CoreDesignTime.C_Appearance)]
        public override Font Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        [DefaultValue(typeof(Color), "Black")]
        [Category(CoreDesignTime.C_Appearance)]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        [DefaultValue(typeof(Color), "White")]
        [Category(CoreDesignTime.C_Appearance)]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        [DefaultValue(typeof(Color), "Gray")]
        [Category(CoreDesignTime.C_Appearance)]
        public Color BorderColor
        {
            get => _BorderColor;
            set
            {
                if (!SetValue(ref _BorderColor, value))
                    return;

                Invalidate();
            }
        }

        #endregion Appearance

        #region Format

        protected string _EditFormat;
        [DefaultValue(null)]
        [Category(CoreDesignTime.C_Appearance)]
        public string EditFormat
        {
            get => _EditFormat;
            set
            {
                if (!SetValue(ref _EditFormat, value))
                    return;

                SetTextFromValue();
            }
        }

        protected string _DisplayFormat;
        [DefaultValue(null)]
        [Category(CoreDesignTime.C_Appearance)]
        public string DisplayFormat
        {
            get => _DisplayFormat;
            set
            {
                if (!SetValue(ref _DisplayFormat, value))
                    return;

                SetTextFromValue();
            }
        }

        #endregion Format

        #endregion Public Properties

        #region Constructors

        public BaseEditBox()
        {
			//ControlStyles style = ControlStyles.AllPaintingInWmPaint |
			//                      ControlStyles.OptimizedDoubleBuffer |
			//                      ControlStyles.ResizeRedraw |
			//                      ControlStyles.UserPaint;
			//SetStyle(style, true);
			Font = new Font("Microsoft Sans Serif", 8.25F, GraphicsUnit.Point);
			BackColor = Color.White;
			ForeColor = Color.Black;
            BoxCache = new EditBoxStateManager();
            DebugInit();
        }

        #endregion Constructors

        #region Component Model Pattern

        protected bool SetValue<TField>(ref TField field, TField value, EventHandler handler = null, EventArgs args = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, value))
                return false;

            field = value;
            args = args ?? EventArgs.Empty;
            handler?.Invoke(this, args);

            return true;
        }

        #endregion Component Model Pattern

        #region Paint

        protected ColorTriplet GetColors()
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
            else
                return CoreDesignTime.EditDefault;
        }

        #endregion Paint

        #region Value

        public abstract bool TryParsePartialValue(string text);
        public abstract bool TryParseValue(string text, out TValue value);

        #endregion Value

        #region Input Logic

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            BoxCache.SetState(this);
            base.OnPreviewKeyDown(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (BoxCache.Enter())
                return;

            if (TryParsePartialValue(Text))
            {
                BoxCache.SetState(this);
                if (TryParseValue(Text, out TValue val))
                    Value = val;
                base.OnTextChanged(e);
            }
            else
            {
                BoxCache.ApplyState(this);
            }

            BoxCache.Leave();
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            OnTextChanged(e);
            SetTextFromValue();
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (TryParseValue(Text, out TValue val))
                Value = val;

            SetTextFromValue();
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnReadOnlyChanged(EventArgs e)
        {
            Invalidate();
            base.OnReadOnlyChanged(e);
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            ColorTriplet state = GetColors();

            ForeColor = state.Foreground;
            BackColor = state.Background;

            base.OnInvalidated(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid);
        }

        #endregion Input Logic

        #region Format Value

        public void SetTextFromValue()
        {
            if (BoxCache.Enter())
                return;

            Text = FormatValue(Value);

            BoxCache.Leave();
        }

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

        #endregion Format Value

        #region ICoreControl

        object ICoreControl.Value
		{
            get => Value;
            set => Value = (TValue)value;
        }
        Type ICoreControl.ValueType => typeof(TValue);
        string ICoreControl.DataPath { get; set; }

        [DefaultValue(null)]
        public Label DescriptionLabel { get; set; }


        public event EventHandler CoreSourceChanged;
        private CoreDataSource _coreSource;
        [DefaultValue(null)]
        public CoreDataSource CoreSource
		{
            get => _coreSource;
            set
			{
                if (ReferenceEquals(_coreSource, value))
                    return;

                _coreSource = value;
                CoreSourceChanged.Invoke(this, EventArgs.Empty);
            }
		}

        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public CoreDataBinder ValueBind { get; private set; }

        private void DebugInit()
		{
            //ValueBind = new CoreDataBinder<(this);
        }

        #endregion ICoreControl
    }
}
