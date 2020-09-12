using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Core.Controls
{
    public static class CoreDesignTime
    {

        #region Category

        public const string C_Appearance = "_KOR Appearance";
        public const string C_Component = "_KOR Component";


        #endregion Category

        #region EditBox Triplets

        public static readonly ColorTriplet EditDefault = new ColorTriplet(HSVGray(50), Color.White, Color.Black);
        public static readonly ColorTriplet EditDisabled = new ColorTriplet(HSVGray(70), HSVGray(85), HSVGray(45));
        public static readonly ColorTriplet EditReadOnly = new ColorTriplet(HSVGray(60), HSVGray(85), Color.Black);
        public static readonly ColorTriplet EditFocused = new ColorTriplet(Color.DodgerBlue, Color.FromArgb(204, 255, 204), Color.Black);
        public static readonly ColorTriplet EditError = new ColorTriplet(HSVGray(50), Color.FromArgb(240, 180, 180), Color.Black);
        public static readonly ColorTriplet EditMandatory = new ColorTriplet(HSVGray(50), Color.FromArgb(255, 246, 174), Color.Black);

        #endregion EditBox Triplets

        public static Color HSVGray(int lvl)
        {
            return ColorTriplet.HSVGray(lvl);
        }
    }

    public enum KorDesignTheme
    {
        Default,
        Light,
        Dark
    }
}
