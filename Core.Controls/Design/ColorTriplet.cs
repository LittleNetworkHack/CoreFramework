using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Core.Controls
{
    public struct ColorTriplet
    {
        public Color Border { get; }
        public Color Background { get; }
        public Color Foreground { get; }

        public ColorTriplet(Color border, Color back, Color fore)
        {
            Border = border;
            Background = back;
            Foreground = fore;
        }

        public static Color HSVGray(int lvl)
        {
            if (lvl <= 0)
                return Color.FromArgb(0, 0, 0);
            else if (lvl >= 100)
                return Color.FromArgb(255, 255, 255);

            byte v = (byte)(lvl * (255d / 100d));
            return Color.FromArgb(v, v, v);
        }
    }
}
