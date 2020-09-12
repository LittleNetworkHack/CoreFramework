using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Controls
{
    public class TextEditBox : BaseEditBox<string>
    {
		#region Constructors

        public TextEditBox()
		{

		}

		#endregion Constructors

		#region Value

		public override bool TryParsePartialValue(string text)
        {
            return true;
        }

        public override bool TryParseValue(string text, out string value)
        {
            value = text;
            return true;
        }

        #endregion Value
    }
}
