using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls.ChildObjects
{
    public class ComboDropPanel : Control
    {
        #region Properties

        private CustomComboBox box;

        public int PhantomIndex { get; protected set; }

        #endregion Properties

        #region Constructors

        public ComboDropPanel(CustomComboBox owner)
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
            ScrollToIndex(PhantomIndex);
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
                //Debug.WriteLine("Move" + DateTime.Now.Ticks);
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
            if (PhantomIndex == 0)
                return;

            PhantomIndex--;
            ScrollToIndex(PhantomIndex);
            Invalidate();
        }

        public void MoveDown()
		{
            if (PhantomIndex == box.ItemCount - 1)
                return;

            PhantomIndex++;
            ScrollToIndex(PhantomIndex);
            Invalidate();
        }

        protected void ScrollToIndex(int index)
        {
            ScrollableControl parent = Parent as ScrollableControl;
            if (parent == null)
                return;

            Point scPos = parent.AutoScrollPosition;
            int topIdx = Math.Abs(scPos.Y / box.ItemHeight);
            int botIdx = topIdx + box.HeightCount;

            // Ako postoji parcijalno vidljiv item 
            // onda moramo pomaknuti indexe za jedan dolje
            // zbog lakse logike kasnije
            if (scPos.Y % box.ItemHeight != 0)
			{
                topIdx += 1;
                botIdx -= 1;
			}

            // Ako se trazeni index nalazi iznad najvise vidljivog (cijelog) item-a
            //      onda 'najvisi vidljivi' - 'trazeni index' je broj skrivenih
            //      onda oduzimamo 1 jer zelimo "otkrit" jos jedan
            // Ako se trazeni index nalazi ispod najnizeg vidljivog (cijelog) itema-a
            //      onda 'najvisi vidljivi' je broj skrivenih
            //      onda dodajemo razliku 'najnizi vidljivi' - 'trazeni index' je broj potrebnih pomaka gore
            //      onda dodajemo 1 jer zelimo "sakriti" jos jedan
            if (index <= topIdx - 1)
                scPos.Y = box.ItemHeight * (topIdx - index - 1); 
            else if (index >= botIdx)
                scPos.Y = box.ItemHeight * (index - botIdx + topIdx + 1);

           //Debug.WriteLine($"Top: {topIdx}, Idx: {index}, Bot: {botIdx}");

            if (parent.AutoScrollPosition == scPos)
                return;

            parent.AutoScrollPosition = scPos;
        }

        #endregion Move Operations
    }
}
