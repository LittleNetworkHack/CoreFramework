using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
    [ToolboxItem(false)]
    public class ComboEditDropDown : Control
    {
        #region Properties

        private ComboEditBox box;

        public int PhantomIndex { get; protected set; }

        #endregion Properties

        #region Constructors

        public ComboEditDropDown(ComboEditBox owner)
        {
            box = owner;
            ControlStyles style = ControlStyles.AllPaintingInWmPaint |
                                  ControlStyles.UserPaint |
                                  ControlStyles.ResizeRedraw |
                                  ControlStyles.OptimizedDoubleBuffer |
                                  ControlStyles.Selectable;
            SetStyle(style, true);
            Dock = DockStyle.Top;
            PhantomIndex = -1;
        }

        #endregion Constructors

        #region Layout/Paint

        public void PrepareForDrop()
        {
            PhantomIndex = box.SelectedIndex;
            box.ScrollToIndex(PhantomIndex);
            Invalidate();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, box.ItemCount * box.ItemHeight, specified | BoundsSpecified.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            using (SolidBrush brush = new SolidBrush(CoreDesignTime.EditFocused.Background))
                dc.FillRectangle(brush, ClientRectangle);

            TextFormatFlags txtFlags = TextFormatFlags.Left |
                                       TextFormatFlags.SingleLine |
                                       TextFormatFlags.VerticalCenter |
                                       TextFormatFlags.EndEllipsis;

            Rectangle itemRect = new Rectangle(0, 0, Width, box.ItemHeight);
            for (int i = 0; i < box.ItemCount; i++)
			{
                Color fore = Color.Black;
                if (PhantomIndex == i)
                {
                    fore = Color.White;
                    using (SolidBrush brush = new SolidBrush(Color.DodgerBlue))
                        e.Graphics.FillRectangle(brush, itemRect);
                }

                TextRenderer.DrawText(e.Graphics, box.GetItemText(i), Font, itemRect, fore, txtFlags);
                itemRect.Y += box.ItemHeight;
            }

            base.OnPaint(e);
        }

        #endregion Paint

        #region Mouse Operations

        private Point lastMouse = Point.Empty;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (lastMouse != MousePosition)
            {
                lastMouse = MousePosition;
                PhantomIndex = e.Y / box.ItemHeight;
                Invalidate();
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int idx = e.Y / box.ItemHeight;
            box.SelectedIndex = idx;
            box.CloseDropDown();
            base.OnMouseClick(e);
        }

        #endregion Mouse Operations

        #region Move Operations

		public void MoveUp()
		{
            if (PhantomIndex <= 0)
                return;

            PhantomIndex--;
            box.ScrollToIndex(PhantomIndex);
            Invalidate();
        }

        public void MoveDown()
		{
            if (PhantomIndex >= box.ItemCount - 1)
                return;

            PhantomIndex++;
            box.ScrollToIndex(PhantomIndex);
            Invalidate();
        }

        #endregion Move Operations
    }
}
