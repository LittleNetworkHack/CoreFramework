using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Core.Collections;
using Core.Controls.ChildObjects;

namespace Core.Controls
{
    public class ComboEditBox : ComboBox
    {

        #region Static

        private static readonly Font ItemFont = new Font("Microsoft Sans Serif", 8.25F);

		#endregion Static

		#region Properties

		protected bool _IsMandatory = false;
        #region Attributes
        [Bindable(true)]
        [Browsable(true)]
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
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        #endregion Attributes
        public bool IsValid
        {
            get => _IsValid;
            set => SetValue(ref _IsValid, value, nameof(IsValid));
        }

		[DefaultValue(typeof(Font), "Microsoft Sans Serif, 7.5pt")]
		public override Font Font { get => base.Font; set => base.Font = value; }

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
            Font = new Font("Microsoft Sans Serif", 7.5F);
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
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;
            TextRenderer.DrawText(e.Graphics, GetItemText(SelectedItem), ItemFont, rect, Color.Black, flags);
            //g.DrawString(GetItemText(SelectedItem), ItemFont, Brushes.Black, rect);

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

            DrawItemEventArgs args = new DrawItemEventArgs(e.Graphics, ItemFont, e.Bounds, e.Index, e.State, Color.Black, CoreDesignTime.EditFocused.Background);

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
            args.DrawFocusRectangle();
            //args.Graphics.DrawString(GetItemText(Items[args.Index]), ItemFont, txtBrush, args.Bounds);
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;
            TextRenderer.DrawText(e.Graphics, "GetItemText(SelectedItem)", ItemFont, args.Bounds, Color.Black, flags);
            
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

    public class CustomComboBox : Control
    {
        #region Fields

        private bool userClose = false;

        private Panel scroller;
        private ComboDropPanel panel;

        private ToolStripControlHost dropHost;
        private ToolStripDropDown dropDown;

        #endregion Fields

        #region Properties
        public bool OverMaxItems => ItemCount > MaxItemsVisible;
        public int HeightCount => OverMaxItems ? MaxItemsVisible : ItemCount;
        public bool IsMouseOver => ClientRectangle.Contains(PointToClient(MousePosition));

        protected override Size DefaultSize => new Size(100, 20);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CoreCollection<string> DataSource { get; set; }

        public bool IsDropOpen { get; private set; }
        public int ItemCount => DataSource.Count;
        public int MaxItemsVisible => 7;
        public int ItemHeight => 16;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex { get; set; }

        #endregion Properties

        #region Constructors

        public CustomComboBox()
        {
            ControlStyles style = ControlStyles.AllPaintingInWmPaint |
                                  ControlStyles.UserPaint |
                                  ControlStyles.ResizeRedraw |
                                  ControlStyles.OptimizedDoubleBuffer |
                                  ControlStyles.Selectable;
            SetStyle(style, true);
            DataSource = new CoreCollection<string>()
            {
                "Item0",
                "Item1",
                "Item2",
                "Item3",
                "Item4",
                "Item5",
                "Item6",
                "Item7",
                "Item8",
                "Item9"
            };
            InitializeDropDown();
        }

        private void InitializeDropDown()
		{
            dropDown = new ToolStripDropDown();
            scroller = new Panel();
            panel = new ComboDropPanel(this);
            dropHost = new ToolStripControlHost(scroller);

            dropHost.AutoSize = false;
            dropHost.Margin = Padding.Empty;
            dropHost.Padding = Padding.Empty;

            scroller.AutoScroll = OverMaxItems;
            scroller.Dock = DockStyle.Fill;
            scroller.Margin = Padding.Empty;
            scroller.Padding = Padding.Empty;
            scroller.Controls.Add(panel);

            dropDown.AutoSize = false;
            dropDown.AutoClose = true;
            dropDown.DropShadowEnabled = true;
            dropDown.Margin = Padding.Empty;
            dropDown.Padding = Padding.Empty;
            dropDown.Items.Add(dropHost);

			dropDown.Closed += (s, e) => DropDownClosed();
			dropDown.KeyDown += DropDownKeyDown;
        }

        #endregion Constructors

        #region Item Operations

        

        #endregion Item Operations


        #region Helper Methods



        #endregion Helper Methods

        #region DropDown Operations

        public void OpenDropDown()
        {
            if (IsDropOpen)
                return;

            ShowDropDown();
        }

        public void CloseDropDown()
        {
            if (!IsDropOpen)
                return;

            HideDropDown();
        }

        protected virtual void DropDownClosed()
		{
            userClose = IsMouseOver;
            IsDropOpen = false;
            Invalidate();
        }

        #endregion DropDown Operations

        #region Keyboard Operations

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Enter:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!IsDropOpen && e.KeyCode == Keys.Down)
                OpenDropDown();

            base.OnKeyDown(e);
        }

        private void DropDownKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    panel.MoveUp();
                    break;
                case Keys.Down:
                    panel.MoveDown();
                    break;
                case Keys.Enter:
                    SelectedIndex = panel.PhantomIndex;
                    CloseDropDown();
                    break;
            }
        }

        #endregion Keyboard Operations

        #region Mouse Operations

		protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (!userClose)
                OpenDropDown();

            userClose = false;
            base.OnClick(e);
        }

        #endregion Mouse Operations

        #region Behavior

        protected override void OnEnter(EventArgs e)
        {
            Focus();
            base.OnEnter(e);
        }

        

        #endregion Behavior

        #region Layout/Paint

        protected override void OnPaint(PaintEventArgs e)
        {
            int btnW = SystemInformation.VerticalScrollBarWidth;
            Rectangle boxRect = new Rectangle(0, 0, Width - btnW, Height);
            Rectangle btnRect = new Rectangle(boxRect.Width - 1, 0, btnW + 1, Height);
            TextFormatFlags txtFlags = TextFormatFlags.Left |
                                       TextFormatFlags.SingleLine |
                                       TextFormatFlags.VerticalCenter |
                                       TextFormatFlags.EndEllipsis;

            e.Graphics.FillRectangle(Brushes.White, boxRect);
            ControlPaint.DrawBorder(e.Graphics, boxRect, Color.Gray, ButtonBorderStyle.Solid);

            ComboBoxRenderer.DrawDropDownButton(e.Graphics, btnRect, ComboBoxState.Normal);
            ControlPaint.DrawBorder(e.Graphics, btnRect, Color.Gray, ButtonBorderStyle.Solid);

            TextRenderer.DrawText(e.Graphics, GetItemText(SelectedIndex), Font, boxRect, Color.Black, txtFlags);

            base.OnPaint(e);
        }

        public string GetItemText(int index)
        {
            if (index > ItemCount || index < 0)
                return string.Empty;

            return DataSource[index];
        }

        protected void ShowDropDown()
		{
            Size size = new Size(Width, HeightCount * ItemHeight);
            Point loc = GetDropLocation(size);

            dropHost.Size = size;
            dropDown.Size = size;
            panel.PrepareForDrop();
            dropDown.Show(loc);
            IsDropOpen = true;
            Invalidate();
        }

        protected void HideDropDown()
		{
            dropDown.Close();
            IsDropOpen = false;
            Invalidate();
        }

        public Point GetDropLocation(Size size)
        {
            Point up = new Point(0, size.Height);
            Point down = new Point(0, Height);
            Rectangle screen = Screen.FromControl(this).WorkingArea;

            Point scDown = PointToScreen(down);
            if (scDown.Y + size.Height > screen.Height)
                return PointToScreen(up);

            return PointToScreen(down);
        }

        #endregion Paint
    }
}
