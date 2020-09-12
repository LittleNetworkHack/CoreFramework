using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Core.Controls
{
    public partial class CoreExceptionDialog : Form
    {
        public CoreExceptionDialog()
        {
            InitializeComponent();
        }

        public static DialogResult ShowDialog(string message, Exception ex)
        {
            using (CoreExceptionDialog dialog = new CoreExceptionDialog())
            {
                dialog.Text = "Error - " + ex?.GetType().Name;
                dialog.txtDesc.Text = message;
                dialog.txtDetails.Text = ex?.ToString();

                return dialog.ShowDialog();
            }
        }

        private void OKClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
