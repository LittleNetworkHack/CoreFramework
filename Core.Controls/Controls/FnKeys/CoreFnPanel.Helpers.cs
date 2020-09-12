using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Collections;

namespace Core.Controls
{
    #region Enums

    public enum CoreButtonState
    {
        Default,
        MouseOver,
        MouseDown
    }

    #endregion Enums

    #region Renderer

    public static class CoreFnPanelRenderer
    {
        private static readonly Bitmap _cachedImage = Properties.Resources.FnBtnBase;
        private static readonly Color _PanelBackColor1 = Color.FromArgb(172, 168, 153);
        private static readonly Color _PanelBackColor2 = Color.FromArgb(208, 208, 208);
        private static readonly SolidBrush _ButtonMouseDownBackBrush = new SolidBrush(Color.FromArgb(140, 165, 177));
        private static readonly SolidBrush _ButtonMouseOverBackBrush = new SolidBrush(Color.FromArgb(128, 140, 165, 177));
        private static readonly Font _TextFont = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font _NumberFont = new Font("Segoe UI", 16F, FontStyle.Bold);

        public static void RenderCurrentState(this CoreFnPanel panel, Graphics g)
        {
            panel.RenderBackground(g);
            CoreFnState state = panel.CurrentState;
            if (state == null)
                return;

            int count = state.Items.Count;
            if (count == 0)
                return;

            Rectangle rect = new Rectangle(0, 0, panel.Width / count, panel.Height);
            int error = panel.Width - (rect.Width * count);

            for (int i = 0; i < count; i++)
            {
                if (i + 1 == count)
                    rect.Width += error;

                CoreFnStateItem item = state.Items[i];
                CoreButtonState btnState = panel.GetButtonState(rect);
                item.RenderButton(g, rect, btnState);
                rect.Offset(rect.Width, 0);
            }
        }

        private static void RenderBackground(this CoreFnPanel panel, Graphics g)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(panel.ClientRectangle, _PanelBackColor1, _PanelBackColor2, 90F))
                g.FillRectangle(brush, panel.ClientRectangle);
        }



        private static CoreButtonState GetButtonState(this CoreFnPanel panel, Rectangle rect)
        {
            bool isOwer = rect.Contains(panel.PointToClient(Control.MousePosition));
            bool isDown = isOwer && Control.MouseButtons == MouseButtons.Left;

            if (isDown)
                return CoreButtonState.MouseDown;
            else if (isOwer)
                return CoreButtonState.MouseOver;

            return CoreButtonState.Default;
        }

        private static void RenderButton(this CoreFnStateItem item, Graphics g, Rectangle rect, CoreButtonState state)
        {
            if (state == CoreButtonState.MouseDown)
            {
                g.FillRectangle(_ButtonMouseDownBackBrush, rect);
                ControlPaint.DrawBorder(g, rect, Color.SlateGray, ButtonBorderStyle.Solid);
            }
            else if (state == CoreButtonState.MouseOver)
            {
                g.FillRectangle(_ButtonMouseOverBackBrush, rect);
                ControlPaint.DrawBorder(g, rect, Color.LightSlateGray, ButtonBorderStyle.Solid);
            }
            Rectangle imgRect = new Rectangle(rect.X + 4, rect.Y + 4, 32, 32);
            g.DrawImage(_cachedImage, imgRect);

            TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
                                    TextFormatFlags.VerticalCenter;

            imgRect.Offset(2, -1);
            TextRenderer.DrawText(g, item.Number, _NumberFont, imgRect, Color.Black, flags);

            Rectangle labelRect = new Rectangle(rect.X + 36, rect.Y, rect.Width - 36, rect.Height);

            flags = TextFormatFlags.Left |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.WordBreak;
            TextRenderer.DrawText(g, item.Text, _TextFont, labelRect, Color.White, flags);
        }
    }

    #endregion Renderer

    #region State Objects

    public class CoreFnStateItem
    {
        public Keys Key { get; }
        public string Text { get; set; }
        public string Number { get; set; }

        public CoreFnStateItem(Keys key, string text, string number)
        {
            Key = key;
            Text = text;
            Number = number;
        }

        public CoreFnStateItem(Keys key) : this(key, "Text")
        {
        }

        public CoreFnStateItem(Keys key, string text)
        {
            Key = key;
            Text = text;
            Number = (key >= Keys.F1 && key <= Keys.F12) ? key.ToString().Replace("F", "") : "0";
        }
    }

    public class CoreFnState
    {
        #region Properties

        public string Name { get; }

        public CoreDictionary<Keys, CoreFnStateItem> Items { get; }

        #endregion Properties

        public CoreFnState(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Argument 'name' cannot be null or empty!");

            Name = name;
            Items = new CoreDictionary<Keys, CoreFnStateItem>(nameof(CoreFnStateItem.Key));
        }
    }

    #endregion State Objects

    #region Event Objects

    public delegate void CoreFnKeyEventHandler(CoreFnKeyEventArgs args);

    public class CoreFnKeyEventArgs : EventArgs
    {
        public Keys Key { get; }
        public string State { get; }

        public CoreFnKeyEventArgs(Keys key, string state)
        {
            Key = key;
            State = state;
        }
    }

    #endregion Event Objects
}
