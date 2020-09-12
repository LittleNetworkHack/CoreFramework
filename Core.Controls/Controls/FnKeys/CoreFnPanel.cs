using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

using Core.Collections;

namespace Core.Controls
{
	public partial class CoreFnPanel : Control
	{
		public static readonly CoreCollection<Keys> StaticKeyCollection = new CoreCollection<Keys>()
		{
			Keys.F1,
			Keys.F2,
			Keys.F3,
			Keys.F4,
			Keys.F5,
			Keys.F6,
			Keys.F7,
			Keys.F8,
			Keys.F9,
			Keys.F10,
			Keys.F11,
			Keys.F12
		};

		#region Fields

		#endregion Fields

		#region Properties

		protected override Padding DefaultPadding => Padding.Empty;
		protected override Padding DefaultMargin => Padding.Empty;
		protected override Size DefaultSize => new Size(800, 40);
		protected override Size DefaultMinimumSize => new Size(0, 40);
		protected override Size DefaultMaximumSize => new Size(0, 40);

		private CoreFnState _CurrentState;
		public CoreFnState CurrentState
		{
			get => _CurrentState;
			set
			{
				if (value == null)
					throw new ArgumentNullException(nameof(CurrentState));

				if (_CurrentState == value)
					return;

				_CurrentState = value;
				Invalidate();
			}
		}

		public event CoreFnKeyEventHandler ButtonClick;

		#endregion Properties

		#region Constructors

		public CoreFnPanel()
		{
			// Paint Job
			ControlStyles style = ControlStyles.AllPaintingInWmPaint |
								  ControlStyles.OptimizedDoubleBuffer |
								  ControlStyles.UserPaint |
								  ControlStyles.Opaque |
								  ControlStyles.SupportsTransparentBackColor |
								  ControlStyles.ResizeRedraw;
			SetStyle(style, true);

			// Defaults
			Dock = DockStyle.Top;
			Margin = DefaultMargin;
			Padding = DefaultPadding;
			Size = DefaultSize;
			MinimumSize = DefaultMinimumSize;
			MaximumSize = DefaultMaximumSize;
			DefaultState();

			ParentChanged += OnCoreParentChanged;
		}

		#endregion Constructors

		#region Methods

		protected override void OnPaint(PaintEventArgs e)
		{
			this.RenderCurrentState(e.Graphics);
			//base.OnPaint(e);
		}

		private void DefaultState()
		{
			CurrentState = new CoreFnState("DefaultState");
			foreach (Keys key in StaticKeyCollection)
			{
				CoreFnStateItem item = new CoreFnStateItem(key);
				CurrentState.Items.Add(item);
			}
		}

		#region Key Operations

		private Form parentForm;

		private void OnCoreParentChanged(object sender, EventArgs e)
		{
			if (parentForm != null)
				parentForm.KeyDown -= FormKeyDown;

			parentForm = FindForm();
			if (parentForm == null)
				return;

			parentForm.KeyDown += FormKeyDown;
		}

		private void FormKeyDown(object sender, KeyEventArgs e)
		{
			CheckShortcutKey(e.KeyData);
		}

		private void CheckShortcutKey(Keys key)
		{
			CoreFnStateItem item = CurrentState.Items.FirstOrDefault(I => I.Key == key);
			if (item != null)
				OnButtonClick(item.Key);
		}

		protected override void Dispose(bool disposing)
		{
			if (parentForm != null)
				parentForm.KeyDown -= FormKeyDown;

			base.Dispose(disposing);
		}

		#endregion Key Operations

		#region Mouse Operations

		#region Hover

		protected override void OnMouseEnter(EventArgs e)
		{
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Invalidate();
			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			Invalidate();
			base.OnMouseLeave(e);
		}

		#endregion Hover

		#region Click

		private int? lastClickIndex = null;
		protected override void OnMouseDown(MouseEventArgs e)
		{
			Invalidate();
			if (e.Button == MouseButtons.Left)
				lastClickIndex = GetIndexFromPosition(e.Location);

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Invalidate();

				int itemIndex = GetIndexFromPosition(e.Location);
				if (itemIndex != -1 && itemIndex == lastClickIndex)
				{
					CoreFnStateItem item = CurrentState.Items[itemIndex];
					OnButtonClick(item.Key);
				}
			}

			base.OnMouseUp(e);
		}

		protected void OnButtonClick(Keys key) => OnButtonClick(new CoreFnKeyEventArgs(key, CurrentState.Name));

		protected virtual void OnButtonClick(CoreFnKeyEventArgs args)
		{
			ButtonClick?.Invoke(args);
		}

		#endregion Click

		#endregion Mouse Operations

		#region StateItem Operations

		protected int GetIndexFromPosition(Point location)
		{
			int count = CurrentState.Items.Count;
			if (count == 0)
				return -1;

			int itemWidth = Width / count;
			return location.X / itemWidth;
		}

		#endregion StateItem Operations

		#endregion Methods
	}
}
