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

        public const string C_Appearance = "_Core Appearance";
        public const string C_Component = "_Core Component";


        #endregion Category

        #region EditBox Triplets

        public static readonly ColorTriplet EditDefault = new ColorTriplet("Default", HSVGray(50), Color.White, Color.Black);
        public static readonly ColorTriplet EditDisabled = new ColorTriplet("Disabled", HSVGray(70), HSVGray(85), HSVGray(45));
        public static readonly ColorTriplet EditReadOnly = new ColorTriplet("ReadOnly", HSVGray(60), HSVGray(85), Color.Black);
        public static readonly ColorTriplet EditFocused = new ColorTriplet("Focused", Color.DodgerBlue, Color.FromArgb(204, 255, 204), Color.Black);
        public static readonly ColorTriplet EditError = new ColorTriplet("Error", HSVGray(50), Color.FromArgb(240, 200, 200), Color.Black);
        public static readonly ColorTriplet EditMandatory = new ColorTriplet("Mandatory", HSVGray(50), Color.FromArgb(255, 246, 174), Color.Black);

        #endregion EditBox Triplets

        public static Color HSVGray(int lvl)
        {
            return ColorTriplet.HSVGray(lvl);
        }

        public static ColorTriplet GetColors<TValue>(this BaseEditBox<TValue> box)
		{
            if (box.Enabled == false)
                return EditDisabled;
            else if (box.ReadOnly)
                return EditReadOnly;
            else if (box.Focused)
                return EditFocused;
            else if (box.IsValid == false)
                return EditError;
            else if (box.IsMandatory)
                return EditMandatory;

            return EditDefault;
        }

        public static Color GetBackgroundColor<TValue>(this BaseEditBox<TValue> box) => box.GetColors().Background;
        public static Color GetForegroundColor<TValue>(this BaseEditBox<TValue> box) => box.GetColors().Foreground;
        public static Color GetBorderColor<TValue>(this BaseEditBox<TValue> box) => box.GetColors().Border;
    }

    public enum CoreDesignTheme
    {
        Default,
        Light,
        Dark
    }
}
