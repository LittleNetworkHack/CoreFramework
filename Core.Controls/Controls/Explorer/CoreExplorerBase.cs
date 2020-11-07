using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Layout;
using System.Windows.Forms;

namespace Core.Controls
{
	public abstract class CoreExplorerBase : ContainerControl
	{
		#region Override Properties

		public override LayoutEngine LayoutEngine => CoreExplorerLayout.Instance;

		protected override Size DefaultSize => new Size(200, _headerHeight);
		protected override Size DefaultMinimumSize => new Size(2 * _headerHeight, _headerHeight);

		[DefaultValue(typeof(Color), "Transparent")]
		public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

		[DefaultValue(typeof(Color), "DodgerBlue")]
		public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

		#endregion Override Properties

		#region Properties

		private bool _isExpanded = true;
		[DefaultValue(true)]
		public bool IsExpanded
		{
			get => _isExpanded;
			set
			{
				if (_isExpanded == value)
					return;

				_isExpanded = value;
				Invalidate();
				PerformLayout();
			}
		}

		protected int _headerHeight = 32;
		[DefaultValue(32)]
		public virtual int HeaderHeight
		{
			get => _headerHeight;
			set
			{
				if (_headerHeight == value)
					return;

				_headerHeight = value;
				Invalidate();
				PerformLayout();
			}
		}

		protected int _headerCut = 10;
		[DefaultValue(10)]
		public virtual int HeaderCut
		{
			get => _headerCut;
			set
			{
				if (_headerCut == value)
					return;

				_headerCut = value;
				Invalidate();
				PerformLayout();
			}
		}

		protected int _itemIndent = 20;
		[DefaultValue(20)]
		public virtual int ItemIndent
		{
			get => _itemIndent;
			set
			{
				if (_itemIndent == value)
					return;

				_itemIndent = value;
				Invalidate();
				PerformLayout();
			}
		}

		protected int _itemSpace = 10;
		[DefaultValue(10)]
		public virtual int ItemSpace
		{
			get => _itemSpace;
			set
			{
				if (_itemSpace == value)
					return;

				_itemSpace = value;
				Invalidate();
				PerformLayout();
			}
		}

		protected Font _headerFont = new Font("Microsoft Sans Serif", 8F);
		[DefaultValue(typeof(Font), "Microsoft Sans Serif, 8pt")]
		public virtual Font HeaderFont
		{
			get => _headerFont;
			set
			{
				if (_headerFont == value)
					return;

				_headerFont = value;
				Invalidate();
				PerformLayout();
			}
		}

		protected virtual Rectangle IconRectangle => new Rectangle(2, 0, 32, 32);
		protected virtual Rectangle PanelRectangle => new Rectangle(0, HeaderHeight, Width, Height - HeaderHeight);
		protected virtual Rectangle HeaderRectangle => new Rectangle(0, 0, Width, HeaderHeight);

		protected bool IsMosueOverHeader => HeaderRectangle.Contains(PointToClient(MousePosition));

		#endregion Properties

		#region Constructors

		public CoreExplorerBase()
		{
			ControlStyles style = ControlStyles.AllPaintingInWmPaint |
								  ControlStyles.UserPaint |
								  ControlStyles.ResizeRedraw |
								  ControlStyles.Opaque |
								  ControlStyles.SupportsTransparentBackColor |
								  ControlStyles.OptimizedDoubleBuffer;
			SetStyle(style, true);

			BackColor = Color.Transparent;
			ForeColor = Color.DodgerBlue;
		}

		#endregion Constructors

		#region Override

		protected override void OnTextChanged(EventArgs e)
		{
			Invalidate();
			base.OnTextChanged(e);
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			Invalidate();
			PerformLayout();
			base.OnControlAdded(e);
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			Invalidate();
			PerformLayout();
			base.OnControlRemoved(e);
		}

		#endregion Override

		#region Mouse Actions

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && HeaderRectangle.Contains(e.Location))
				IsExpanded = !IsExpanded;

			base.OnMouseClick(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Cursor = HeaderRectangle.Contains(e.Location) ? Cursors.Hand : Cursors.Default;
			Invalidate();
			base.OnMouseMove(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			Invalidate();
			base.OnMouseLeave(e);
		}

		#endregion Mouse Actions
	}
}
