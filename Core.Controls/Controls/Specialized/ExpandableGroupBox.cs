using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Core.Controls
{
    [ToolboxItem(false)]
    public class ExpandableGroupBox : GroupBox, INotifyPropertyChanged
    {
        #region Fields

        protected static readonly Bitmap bmpPlus = Properties.Resources.toggle_expand;
        protected static readonly Bitmap bmpMinus = Properties.Resources.toggle;

        protected Rectangle TextRectangle;
        protected Rectangle BorderRectangle;
        protected Rectangle ToggleRectangle;
        protected Rectangle ToggleHighlightRectangle;

        protected bool HandleVisibility = false;
        protected Dictionary<Control, bool> VisibleControls = new Dictionary<Control, bool>();

        protected override Padding DefaultPadding => new Padding(3, 8, 3, 3);

        #endregion Fields

        #region Properties

        protected bool _AutoSize = false;
        public override bool AutoSize 
        { 
            get => _AutoSize;
            set => _AutoSize = value;
        }

        public bool IsMouseOverToggle => ToggleHighlightRectangle.Contains(PointToClient(Cursor.Position));
        public bool IsMouseDownToggle => IsMouseOverToggle && (MouseButtons & MouseButtons.Left) != 0;

        protected bool _Expanded = true;
        #region Attributes
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        #endregion Attributes
        public bool Expanded
        {
            get => _Expanded;
            set => SetValue(ref _Expanded, value, nameof(Expanded));
        }

        protected int _HeightExpanded = 100;
        #region Attributes
        [Browsable(true)]
        [Category("Layout")]
        [DefaultValue(100)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        #endregion Attributes
        public int HeightExpanded
        {
            get => _HeightExpanded;
            set => SetValue(ref _HeightExpanded, value, nameof(HeightExpanded));
        }

        protected int _HeightCollapsed = 30;
        #region Attributes
        [Browsable(true)]
        [Category("Layout")]
        [DefaultValue(30)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        #endregion Attributes
        public int HeightCollapsed
        {
            get => _HeightCollapsed;
            set => SetValue(ref _HeightCollapsed, value, nameof(HeightCollapsed));
        }

        #endregion Properties

        #region Constructors

        public ExpandableGroupBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        #endregion Constructors

        #region ExpandableGroupBox Property Changed

        protected virtual void OnInternalPropertyChanged(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(Expanded):
                    OnExpandedChanged();
                    break;
                case nameof(HeightExpanded):
                case nameof(HeightCollapsed):
                    OnHeightChanged();
                    break;
            }
        }

        public void OnExpandedChanged()
        {
            HandleVisibility = true;

            foreach (Control c in Controls)
                c.Visible = Expanded ? VisibleControls[c] : false;

            HandleVisibility = false;

            OnHeightChanged();
        }

        public void OnHeightChanged()
        {
            if (Expanded)
                Height = HeightExpanded;
            else
                Height = HeightCollapsed;
        }

        #endregion ExpandableGroupBox Property Changed

        #region Child Visibility

        protected override void OnControlAdded(ControlEventArgs e)
        {
            VisibleControls.Add(e.Control, e.Control.Visible);
            e.Control.VisibleChanged += ChildVisibleChanged;
            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            VisibleControls.Remove(e.Control);
            e.Control.VisibleChanged -= ChildVisibleChanged;
            base.OnControlRemoved(e);
        }

        private void ChildVisibleChanged(object sender, EventArgs e)
        {
            if (sender is Control c && HandleVisibility == false)
                VisibleControls[c] = c.Visible;
        }

        #endregion Child Visibility

        #region Mouse Events

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (IsMouseOverToggle)
                Expanded = !Expanded;

            Invalidate(ToggleHighlightRectangle);
            base.OnMouseClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Invalidate(ToggleHighlightRectangle);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Invalidate(ToggleHighlightRectangle);
            base.OnMouseMove(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            Invalidate(ToggleHighlightRectangle);
            base.OnMouseHover(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Invalidate(ToggleHighlightRectangle);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Invalidate(ToggleHighlightRectangle);
            base.OnMouseLeave(e);
        }

        #endregion Mouse Events

        #region Paint

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (e.ClipRectangle == ToggleHighlightRectangle)
            {
                DrawToggle(g);
            }
            else
            {
                CalculateDrawing(g);
                DrawBackground(g);
                DrawText(g);
                DrawToggle(g);
            }
        }

        protected void DrawBackground(Graphics g)
        {
            g.Clear(BackColor);
            ControlPaint.DrawBorder(g, BorderRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }

        protected void DrawText(Graphics g)
        {
            Rectangle space = TextRectangle;
            //space.X -= 1;
            space.Width -= 2;
            using (SolidBrush brush = new SolidBrush(BackColor))
                g.FillRectangle(brush, space);

            TextRenderer.DrawText(g, Text, Font, TextRectangle, ForeColor, GetBaseFormatFlags());
        }

        protected void DrawToggle(Graphics g)
        {
            if (IsMouseDownToggle && !DesignMode)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(192, Color.DodgerBlue)))
                    g.FillRectangle(brush, ToggleHighlightRectangle);

                ControlPaint.DrawBorder(g, ToggleHighlightRectangle, Color.DodgerBlue, ButtonBorderStyle.Solid);
            }
            else if (IsMouseOverToggle && !DesignMode)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, Color.DodgerBlue)))
                    g.FillRectangle(brush, ToggleHighlightRectangle);

                ControlPaint.DrawBorder(g, ToggleHighlightRectangle, Color.DodgerBlue, ButtonBorderStyle.Solid);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                    g.FillRectangle(brush, ToggleHighlightRectangle);
            }

            g.DrawImage(Expanded ? bmpMinus : bmpPlus, ToggleRectangle);
        }

        protected void CalculateDrawing(Graphics g)
        {
            TextFormatFlags flags = GetBaseFormatFlags();

            TextRectangle = ClientRectangle;
            TextRectangle.X = 8;
            TextRectangle.Y = 3;
            TextRectangle.Width = Width - 16;
            TextRectangle.Size = TextRenderer.MeasureText(g, Text, Font, TextRectangle.Size, flags);

            BorderRectangle = ClientRectangle;
            BorderRectangle.X = 1;
            BorderRectangle.Y = TextRectangle.Top + TextRectangle.Height / 2;
            BorderRectangle.Width -= 2;
            BorderRectangle.Height -= 3 + TextRectangle.Height / 2;

            ToggleRectangle = new Rectangle(BorderRectangle.Right - 5 - 16, BorderRectangle.Y - 7, 16, 16);

            ToggleHighlightRectangle = ToggleRectangle;
            ToggleHighlightRectangle.X -= 1;
            ToggleHighlightRectangle.Y -= 2;
            ToggleHighlightRectangle.Width += 3;
            ToggleHighlightRectangle.Height += 3;
        }

        protected TextFormatFlags GetBaseFormatFlags()
        {
            TextFormatFlags flags = TextFormatFlags.TextBoxControl |
                                    TextFormatFlags.WordBreak |
                                    TextFormatFlags.PreserveGraphicsClipping |
                                    TextFormatFlags.PreserveGraphicsTranslateTransform;
            if (!ShowKeyboardCues)
                flags |= TextFormatFlags.HidePrefix;
            if (RightToLeft == RightToLeft.Yes)
                flags |= (TextFormatFlags.Right | TextFormatFlags.RightToLeft);

            return flags;
        }

        #endregion Paint

        #region Component Model Pattern
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName, object value)
        {
            OnInternalPropertyChanged(propertyName, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;

            field = value;
            OnPropertyChanged(propertyName, value);
        }
        #endregion Component Model Pattern
    }
}
