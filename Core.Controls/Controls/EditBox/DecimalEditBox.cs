using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Controls
{
    public class DecimalEditBox : BaseEditBox<decimal?>
    {
        #region Value

        protected string CurrentDecimalSeparator => CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public override bool TryParsePartialValue(string text)
        {
            if (String.IsNullOrEmpty(text))
                return true;
            else if (text == "-")
                return true;
            else if (text == "-" + CurrentDecimalSeparator)
                return true;
            else if (text.Contains(" "))
                return false;
            else if (Decimal.TryParse(text, out decimal v))
                return true;

            return false;
        }

        public override bool TryParseValue(string text, out decimal? value)
        {
            bool flag = Decimal.TryParse(text, out decimal v);
            value = flag ? (decimal?)v : null;
            return flag;
        }

        #endregion Value
    }
}
