using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Controls
{
	[ToolboxItem(false)]
	public class CoreValidatorBox : Control
	{
		#region Image

		private static readonly string tick_b64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAWVJREFUeNpi/P//PwNdgVsnw38QJkuzYxvD/3c/JoIxiE2y5idfmv/P2sQAxk+/NoJdwkKs5sV5zQzbj9QyMLAyMDACxb7/f4eqyB1omjsWvzkBNd/+XPR/1h6G/zOBePZehv+XPsX/d4J4wRiu6MqnOCCGS0DEW4GKPyb8n30Q6OwDQM2HQJoTUDWDbD37we//7GNABUcZ/p967//foZkBjEFskPgsoPgcIH36gz+qZiBggfuHmYHhH1Dqwq2NDLVpNmBJEJuBCehnIFZTsWOomLaRYV8VgwlQ6izMAJB+Y5c2hjOlqboMj59cZvj3j4EBlrYYgbJMQM2yMroMXbMvM+xF0wwCQHsZnt/by7DlheCrNAd7WYYfPz4x/AfZCpRhBsaRiIQsw4Q5t7FqRgcgl/xf+Zz7/9wbDGAMYrug+ZkQMHYFalj9khuMXUnUDDfEExgznp3kaYYbQqxmRkqzM0CAAQBWbMG1YQFlxwAAAABJRU5ErkJggg==";
		private static readonly string cross_b64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAXtJREFUeNpi+P//PwMMHHEw+A/CDDgAujxYL8wAkMS3w7vAGJsh2OThBoAEvu7e8P+ppy4Yg9jIhuCSB+llgVvx4zsDEwsrmPlpUhODYXE5wxGGTrAhIDZIDCYPUgsDjCBTGBkZjYEmnjHIzmP4tmwmXJIrKh1Mo4tdmDqJwebABROg3rMwA0ByYEMMk9MZvm9aijUQOf2iGc7PnQnWDOSeBetFMgBhSGw8w889m1A0s7v4MZxfvBCuGRaILGiWgCUYf/5E+BfmV6AYshr0MICHtlFgEMPfCyexeoHZwJzh3Pp1IFcwwlwANwCk2djHm+HfzStwDUzqOmAaXezslq1gQ+BeAGk2cXdj+H/3JtzpjHJKDGeACkEALPfoHsQEoBoQ/wjQASBl8DBg+v2b4T8rVLOIBMOpnbtgAQZSfMbMyZHh/5sXEHmgWqzpwMLOBix44tARlNCGxQ66PCgdMCBlJmOkzGKMJQwx5FEyE0wRDs1Y5eGxQAkACDAArAsJ2PYrD9QAAAAASUVORK5CYII=";

		protected static readonly Image img_tick = Base64ToImage(tick_b64);
		protected static readonly Image img_cross = Base64ToImage(cross_b64);

		private static Image Base64ToImage(string base64)
		{
			byte[] arr = Convert.FromBase64String(base64);
			using (MemoryStream ms = new MemoryStream(arr, 0, arr.Length))
			{
				Image image = Image.FromStream(ms, true);
				return image;
			}
		}

		#endregion Image

		#region Properties

		protected override Size DefaultSize => new Size(16, 16);

		private bool _isValid = false;
		[DefaultValue(false)]
		public bool IsValid
		{
			get => _isValid;
			set
			{
				if (_isValid == value)
					return;

				_isValid = value;
				Invalidate();
			}
		}

		#endregion Properties

		#region Constructors

		public CoreValidatorBox()
		{
			ControlStyles style = ControlStyles.AllPaintingInWmPaint |
								  ControlStyles.UserPaint |
								  ControlStyles.ResizeRedraw |
								  ControlStyles.Opaque |
								  ControlStyles.OptimizedDoubleBuffer |
								  ControlStyles.FixedHeight |
								  ControlStyles.FixedWidth;
			SetStyle(style, true);
		}

		#endregion Constructors

		#region Paint

		protected override void OnPaint(PaintEventArgs e)
		{
			ButtonRenderer.DrawParentBackground(e.Graphics, ClientRectangle, this);
			Image img = IsValid ? img_tick : img_cross;
			e.Graphics.DrawImageUnscaled(img, 0, 0, 16, 16);
			base.OnPaint(e);
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, 16, 16, specified);
		}

		#endregion Paint

	}
}
