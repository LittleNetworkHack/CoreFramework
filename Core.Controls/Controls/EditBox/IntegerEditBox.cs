using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Controls
{
    public class IntegerEditBox : BaseEditBox<int?>
    {
        #region Value

        public override bool TryParsePartialValue(string text)
        {
            if (String.IsNullOrEmpty(text))
                return true;
            else if (text == "-")
                return true;
            else if (text.Contains(" "))
                return false;
            else if (Int32.TryParse(text, out int v))
                return true;

            return false;
        }

        public override bool TryParseValue(string text, out int? value)
        {
            bool flag = Int32.TryParse(text, out int v);
            value = flag ? (int?)v : null;
            return flag;
        }
        #endregion Value
    }
}
