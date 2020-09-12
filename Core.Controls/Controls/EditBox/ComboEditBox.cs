using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Core.Controls
{
    public class ComboEditBox : ComboBox
    {
        #region Properties

        protected bool _IsMandatory = false;
        #region Attributes
        [Bindable(true)]
        [Browsable(true)]
        [Category("KOR Component Model")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        #endregion Attributes
        public bool IsMandatory
        {
            get => _IsMandatory;
            set => SetValue(ref _IsMandatory, value, nameof(IsMandatory));
        }

        protected bool _IsValid = true;
        #region Attributes
        [Bindable(true)]
        [Browsable(true)]
        [Category("KOR Component Model")]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        #endregion Attributes
        public bool IsValid
        {
            get => _IsValid;
            set => SetValue(ref _IsValid, value, nameof(IsValid));
        }

        #endregion Properties

        #region Constructors

        public ComboEditBox()
        {
            ControlStyles style = ControlStyles.AllPaintingInWmPaint |
                                  ControlStyles.OptimizedDoubleBuffer |
                                  ControlStyles.ResizeRedraw |
                                  ControlStyles.UserPaint;
            SetStyle(style, true);
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        #endregion Constructors

        #region ComponentModel

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            InternalPropertyChanged(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void SetValue<T>(ref T property, T value, string name)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
                return;

            property = value;
            OnPropertyChanged(name);
        }

        #endregion ComponentModel

        protected virtual void InternalPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IsValid):
                case nameof(IsMandatory):
                    Invalidate();
                    break;
            }
        }

        #region Paint

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Graphics g = e.Graphics;

            ColorTriplet style = GetColors();
            g.Clear(Color.White);

            Rectangle rect = ClientRectangle;
            rect.Width -= Height - 1;
            g.FillRectangle(Brushes.White, rect);
            ControlPaint.DrawBorder(g, rect, style.Border, ButtonBorderStyle.Solid);

            rect.Inflate(-2, -2);
            using (SolidBrush brush = new SolidBrush(style.Background))
                g.FillRectangle(brush, rect);

            rect.Offset(0, 2);
            g.DrawString(GetItemText(SelectedItem), Font, Brushes.Black, rect);

            Rectangle btnRect = new Rectangle(Width - Height, 0, Height, Height);

            if (ComboBoxRenderer.IsSupported)
            {
                ComboBoxState state;
                if (DroppedDown)
                    state = ComboBoxState.Pressed;
                else if (ContainsFocus)
                    state = ComboBoxState.Hot;
                else if (Enabled == false)
                    state = ComboBoxState.Disabled;
                else
                    state = ComboBoxState.Normal;

                ComboBoxRenderer.DrawDropDownButton(e.Graphics, btnRect, state);
            }
            else
            {
                ControlPaint.DrawComboButton(e.Graphics, btnRect, ButtonState.Inactive);
            }

            ControlPaint.DrawBorder(g, btnRect, style.Border, ButtonBorderStyle.Solid);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //base.OnDrawItem(e);

            DrawItemEventArgs args = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State, Color.Black, CoreDesignTime.EditFocused.Background);

            if (args.Index < 0 || args.Index >= Items.Count)
                return;

            Brush txtBrush;

            if ((args.State & DrawItemState.Selected) == DrawItemState.Selected)
                txtBrush = Brushes.White;
            else if ((args.State & DrawItemState.HotLight) == DrawItemState.HotLight)
                txtBrush = Brushes.White;
            else
                txtBrush = Brushes.Black;

            args.DrawBackground();
            args.Graphics.DrawString(GetItemText(Items[args.Index]), Font, txtBrush, args.Bounds);
            args.DrawFocusRectangle();
        }

        protected ColorTriplet GetColors()
        {
            if (Enabled == false)
                return CoreDesignTime.EditDisabled;
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

        protected override void OnLostFocus(EventArgs e)
        {
            OnPaint(new PaintEventArgs(CreateGraphics(), ClientRectangle));
            base.OnLostFocus(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            OnPaint(new PaintEventArgs(CreateGraphics(), ClientRectangle));
            base.OnGotFocus(e);
        }

    }
}
