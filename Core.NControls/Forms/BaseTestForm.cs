using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.NControls.Components;

namespace Core.NControls.Forms
{
	public partial class BaseTestForm : Form
	{
		protected CoreControl mainCtrl;
		protected List<CoreControl> CoreControls { get; }
		

		public BaseTestForm()
		{
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			CoreControls = new List<CoreControl>();
			
			mainCtrl = new CoreControl();
			mainCtrl.SetBounds(50, 50, 200, 200);

			CoreControl cc;

			cc = new CoreControl();
			cc.SetBounds(10, 10, 20, 30);
			mainCtrl.CoreControls.Add(cc);

			cc = new CoreControl();
			cc.SetBounds(50, 50, 30, 20);
			mainCtrl.CoreControls.Add(cc);

			CoreControls.Add(mainCtrl);
		}

		private int timeCount = 0;
		private decimal timeSum = 0;
		Stopwatch sw = new Stopwatch();

		protected override void OnPaint(PaintEventArgs e)
		{
			StartTimer();
			try
			{
				foreach (CoreControl ctrl in CoreControls)
					ctrl.ExecPaint(e.Graphics);
			}
			catch (Exception ex)
			{
				TextRenderer.DrawText(e.Graphics, ex.ToString(), Font, new Point(0, 0), Color.Black);
			}
			StopTimer();
			base.OnPaint(e);
		}

		private void StartTimer()
		{
			sw.Reset();
			sw.Start();
			
		}

		private void StopTimer()
		{
			sw.Stop();
			timeCount++;
			timeSum += sw.ElapsedTicks;
			decimal avg = timeSum / timeCount;
			TimeSpan sp = new TimeSpan((long)avg);
			Debug.WriteLine($"Run: [{sw.Elapsed}], Avg: [{sp}]");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			mainCtrl.XOff += 10;
			Invalidate();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			mainCtrl.XOff -= 10;
			Invalidate();
		}

		private void BtnKeyDown(object sender, KeyEventArgs args)
		{
			Debug.WriteLine(args.KeyData);
		}

	}
}
