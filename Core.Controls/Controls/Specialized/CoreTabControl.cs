using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{ 
	public class CoreTabControl : TabControl
	{
        private bool _ShowHeaders = false;

        [DefaultValue(false)]
        public bool ShowHeaders
		{
            get => _ShowHeaders;
            set
			{
                if (_ShowHeaders == value)
                    return;

                _ShowHeaders = value;
                Invalidate();
			}
		}

        protected override void WndProc(ref Message m)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message
            if (m.Msg == 0x1328 && !DesignMode && !ShowHeaders)
                m.Result = (IntPtr)1;
            else
                base.WndProc(ref m);
        }
    }
}
