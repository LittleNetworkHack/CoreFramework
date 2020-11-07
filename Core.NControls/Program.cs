using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.NControls.Components;
using Core.NControls.Forms;

using WinApi.Desktop;
using WinApi.User32;
using WinApi.Windows;
using WinApi.Windows.Controls;
using WinApi.Windows.Helpers;

namespace Core.NControls
{
	internal static class Program
	{
		static int Main(string[] args)
		{
            bool runForms = false;

            if (runForms)
			{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new BaseTestForm());
            }
            else
			{
                ApplicationHelpers.SetupDefaultExceptionHandlers();
                try
                {
                    using (var win = AppWindow.Create())
                    {
                        win.CenterToScreen();
                        win.Show();
                        return new EventLoop().Run(win);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxHelpers.ShowError(ex);
                    return 1;
                }
            }

            return 0;
        }
	}
}
