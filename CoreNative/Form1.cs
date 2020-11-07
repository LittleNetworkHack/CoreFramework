using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.NControls.Controls;

namespace CoreNative
{
	public partial class Form1 : Form
	{
		SimpleControl m_designCtrl;

		public Form1()
		{
			InitializeComponent();
			try
			{
				m_designCtrl = SimpleControl.Create();
				m_designCtrl.SetPosition(10, 10);
				Control dcc = Control.FromHandle(m_designCtrl.Handle);
			}
			catch (Exception ex)
			{

			}
			
		}
	}
}
