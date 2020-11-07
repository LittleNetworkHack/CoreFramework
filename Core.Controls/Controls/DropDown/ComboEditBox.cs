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

namespace Core.Controls
{
    public class ComboEditBox : Control, ICoreControl
    {
        #region Fields

        private bool userClose = false;
        private bool _isReadOnly = false;
        private bool _isMandatory = false;

        private int _itemHeight = 16;
        private int _maxDropItems = 5;

        private object _dataSource = null;

        private ComboEditDropDown panel;
        private CoreDropDown dropDown;

        #endregion Fields

        #region Properties

        #region Calculators

        protected bool OverMaxItems => ItemCount > MaxDropItems;
        protected int VisibleCount => OverMaxItems ? MaxDropItems : ItemCount;
        protected bool IsMouseOver => ClientRectangle.Contains(PointToClient(MousePosition));

        protected int DropButtonCut => SystemInformation.VerticalScrollBarWidth;
        protected Rectangle EditBoxRectangle => new Rectangle(0, 0, Width - DropButtonCut, Height);
        protected Rectangle DropButtonRectangle => new Rectangle(Width - DropButtonCut - 1, 0, DropButtonCut + 1, Height);

        #endregion Calculators

        #region Visual Style

        protected override Size DefaultSize => new Size(100, 20);

        public CoreControlStates CoreState
		{
            get
            {
                CoreControlStates value = CoreControlStates.Default;

                if (Enabled)
                    value |= CoreControlStates.Enabled;
                if (IsReadOnly)
                    value |= CoreControlStates.ReadOnly;
                if (Focused)
                    value |= CoreControlStates.Focused;
                if (IsValid)
                    value |= CoreControlStates.Valid;
                if (IsMandatory)
                    value |= CoreControlStates.Mandatory;

                return value;
            }
        }

        [DefaultValue(5)]
        public int MaxDropItems
        {
            get => _maxDropItems;
            set
            {
                if (_maxDropItems == value || value <= 0)
                    return;

                _maxDropItems = value;
            }
        }

        [DefaultValue(16)]
        public int ItemHeight
		{
            get => _itemHeight;
            set
			{
                if (_itemHeight == value || value <= 0)
                    return;

                _itemHeight = value;
			}
		}

        #endregion Visual Style

        #region Data

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CoreCollection<string> DataSource { get; }

        public int ItemCount => DataSource.Count;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex { get; set; }

        [DefaultValue(null)]
        public object DataSource2
		{
            get => _dataSource;
            set
			{
                if (Equals(_dataSource, value))
                    return;


			}
		}

        #endregion Data

        #region ICoreControl

        Type ICoreControl.ValueType => DataSource?.GetType();
        object ICoreControl.Value
		{
            get;
            set;
		}

        #endregion ICoreControl

        #region Behavior

        [DefaultValue(false)]
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => SetPropValue(ref _isReadOnly, value, Invalidate);
        }

        [DefaultValue(false)]
        public bool IsValid { get; set; } = true;

        [DefaultValue(false)]
        public bool IsMandatory
        {
            get => _isMandatory;
            set => SetPropValue(ref _isMandatory, value, Invalidate);
        }

        #endregion Behavior

        #endregion Properties

        #region Constructors

        public ComboEditBox()
        {
            ControlStyles style = ControlStyles.AllPaintingInWmPaint |
                                  ControlStyles.UserPaint |
                                  ControlStyles.ResizeRedraw |
                                  ControlStyles.OptimizedDoubleBuffer |
                                  ControlStyles.Selectable;
            SetStyle(style, true);
            DataSource = new CoreCollection<string>();
            DebugInit();
            InitializeDropDown();
        }

        protected void DebugInit()
		{
            for (int i = 0; i < 10; i++)
                DataSource.Add("Item" + i);
		}

        #endregion Constructors

        #region DropDown Operations

        private void InitializeDropDown()
        {
            panel = new ComboEditDropDown(this);
            panel.KeyDown += DropDownKeyDown;
            dropDown = new CoreDropDown();
            dropDown.Container.Controls.Add(panel);
            dropDown.Closed += OnDropDownClosed;
        }

        public void OpenDropDown()
        {
            if (IsReadOnly)
                return;

            if (dropDown.IsOpen)
                return;

            Rectangle bounds = GetDropDownBounds();

            panel.PrepareForDrop();
            //dropDown.Container.AutoScroll = OverMaxItems;
            dropDown.ShowDropDown(panel, bounds);
            Invalidate();
        }

        public void CloseDropDown()
        {
            if (IsReadOnly)
                return;

            if (!dropDown.IsOpen)
                return;

            dropDown.CloseDropDown();
            Invalidate();
        }

        #endregion DropDown Operations

        #region Focus/Invalidate

        protected override void OnEnter(EventArgs e)
        {
            if (IsReadOnly)
                return;

            Focus();
            Invalidate();
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            Invalidate();
            base.OnLeave(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Invalidate();
            base.OnMouseMove(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Invalidate();
            base.OnMouseLeave(e);
        }

        #endregion Focus/Invalidate

        #region User Events

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (IsReadOnly)
                return;

            if (e.Button.CheckFlag(MouseButtons.Middle))
            {
                SelectedIndex = -1;
                Invalidate();
                return;
            }

            Focus();
            if (!userClose)
                OpenDropDown();

            userClose = false;
            base.OnClick(e);
        }

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
            if (IsReadOnly)
                return;

            if (!dropDown.IsOpen && e.KeyCode == Keys.Down)
                OpenDropDown();

            base.OnKeyDown(e);
        }

        private void DropDownKeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly)
                return;

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

        protected virtual void OnDropDownClosed(object sender, ToolStripDropDownClosedEventArgs args)
        {
            userClose = IsMouseOver;
            Invalidate();
        }

        #endregion User Events

        #region Paint

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle boxRect = EditBoxRectangle;
            Rectangle btnRect = DropButtonRectangle;
            ColorTriplet colors = CoreState.GetColors();
            
            DrawBackground(e.Graphics, colors.Background, boxRect, btnRect);
            DrawBorders(e.Graphics, colors.Border, boxRect, btnRect);
            DrawText(e.Graphics, colors.Foreground, boxRect);

            base.OnPaint(e);
        }

        protected void DrawBackground(Graphics dc, Color color, Rectangle boxRect, Rectangle btnRect)
		{
            using (SolidBrush brush = new SolidBrush(color))
                dc.FillRectangle(brush, boxRect);

            ComboBoxState comboState;
            if (!Enabled || IsReadOnly)
                comboState = ComboBoxState.Disabled;
            else if (dropDown.IsOpen)
                comboState = ComboBoxState.Pressed;
            else if (btnRect.Contains(PointToClient(MousePosition)))
                comboState = ComboBoxState.Hot;
            else
                comboState = ComboBoxState.Normal;

            ComboBoxRenderer.DrawDropDownButton(dc, btnRect, comboState);
        }

        protected void DrawBorders(Graphics dc, Color color, Rectangle boxRect, Rectangle btnRect)
		{
            ControlPaint.DrawBorder(dc, boxRect, color, ButtonBorderStyle.Solid);
            ControlPaint.DrawBorder(dc, btnRect, color, ButtonBorderStyle.Solid);
        }

        protected void DrawText(Graphics dc, Color color, Rectangle boxRect)
		{
            TextFormatFlags txtFlags = TextFormatFlags.Left |
                                       TextFormatFlags.SingleLine |
                                       TextFormatFlags.VerticalCenter |
                                       TextFormatFlags.EndEllipsis;
            string text = GetItemText(SelectedIndex);
            TextRenderer.DrawText(dc, text, Font, boxRect, color, txtFlags);
        }

        #endregion Paint

        #region Helper Methods

        protected bool SetPropValue<T>(ref T field, T value, Action method = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            method?.Invoke();
            return true;
        }

        public void ScrollToIndex(int index)
        {
            Panel scroller = dropDown.Container;
            Point scPos = scroller.AutoScrollPosition;

            // (1)  Ako nam ostatak vrati != 0 onda imamo parcijalni slucaj
            bool part = scPos.Y % ItemHeight != 0;

            // (2)  Prvi i zadnji vidljivi elementi
            //      u parcijalnom slucaju za prvi dobijemo parcijalno vidljiv element
            //      u parcijalnom slucaju za zadnji dobijemo cijeli vidljivi element
            int topIdx = Math.Abs(scPos.Y / ItemHeight);
            int botIdx = topIdx + VisibleCount - 1;

            // Logika radi na temelju offseta koji se odreduje prema "skrivenim" elementima
            int? hidden = null;

            // (3)  Provjeravamo da li je 'index' iznad prvog (topIdx - 1)
            //      Za (1) provjeravamo da li je 'index' zapravo parcijalni element te se fokusiramo na njega
            //      Prema ovome broj skrivenih je 'index'
            // (4)  Provjeravamo da li je 'index' ispod zadnjeg (botIdx + 1)
            //      Za (1) nema jer div izbaci zadnji parcijalni element
            //      Prema ovome broj skrivenih je trenutni broj skrivenih (topIdx) + pomak (index - botIdx)
            if (index <= topIdx - (part ? 0 : 1))
                hidden = index;
            else if (index >= botIdx + 1)
                hidden = topIdx + (index - botIdx);

            Debug.WriteLine($"Top: {topIdx}, Idx: {index}, Bot: {botIdx}");

            if (hidden == null)
                return;

            scroller.AutoScrollPosition = new Point(0, ItemHeight * hidden.Value);
        }

        public Rectangle GetDropDownBounds()
        {
            Point location = PointToScreen(new Point(0, Height));
            Size size = new Size(Width, VisibleCount * ItemHeight);
            Rectangle screen = Screen.FromControl(this).WorkingArea;
            if (location.Y + size.Height > screen.Height)
                location = PointToScreen(new Point(0, size.Height));

            return new Rectangle(location, size);
        }

        public string GetItemText(int index)
        {
            if (index > ItemCount || index < 0)
                return string.Empty;

            return DataSource[index];
        }

        #endregion Helper Methods
    }
}
