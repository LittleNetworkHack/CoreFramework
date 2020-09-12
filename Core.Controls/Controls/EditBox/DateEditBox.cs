using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Controls
{
    public class DateEditBox : BaseEditBox<DateTime?>
    {
        public override bool TryParsePartialValue(string text)
        {
            return true;
        }

        public override bool TryParseValue(string text, out DateTime? value)
        {
            DateTime v;

            if (DateTime.TryParseExact(text, "ddMM", CultureInfo.CurrentCulture, DateTimeStyles.None, out v))
            {
                value = v;
                return true;
            }
            else if (DateTime.TryParseExact(text, "ddMMyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out v))
            {
                value = v;
                return true;
            }
            else if (DateTime.TryParseExact(text, "ddMMyyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out v))
            {
                value = v;
                return true;
            }

            bool flag = DateTime.TryParse(text, out v);
            value = flag ? (DateTime?)v : null;
            return flag;
        }
    }
}
