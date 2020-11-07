using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Zero.ComponentModel;
using Core.Zero.Components;
using Core.Zero.Drawing;

namespace Core.Zero.Controls
{
	public class CoreControl : CoreComponent
	{
		#region Fields

		// Bounds Rectangle
		private int core_x;
		private int core_y;
		private int core_w;
		private int core_h;

		private CoreThickness borderThickness = new CoreThickness(1);

		#endregion Fields

		#region Properties

		public Point Location
		{
			get => new Point(core_x, core_y);
			set => SetBounds(value.X, value.Y, null, null);
		}
		public Size Size
		{
			get => new Size(core_w, core_h);
			set => SetBounds(null, null, value.Width, value.Height);
		}
		public Rectangle Bounds
		{
			get => new Rectangle(core_x, core_y, core_w, core_h);
			set => SetBounds(value.X, value.Y, value.Width, value.Height);
		}
		public CoreThickness BorderThickness
		{
			get => borderThickness;
			set => borderThickness = value;
		}

		public Rectangle ClientRectangle => GetClientRectangle();

		public CoreControlCollection Controls { get; }

		#endregion Properties

		#region Constructors

		public CoreControl()
		{
			Controls = new CoreControlCollection(this);
		}

		#endregion Constructors

		#region Methods

		#region OnPaint

		public List<CoreControl> CoreControls { get; } = new List<CoreControl>();

		public int XOff { get; set; }
		public int YOff { get; set; }

		public virtual void ExecPaint(Graphics context)
		{
			using (CorePaintObject po = new CorePaintObject(context, this))
			{
				OnPaint(context);

				foreach (CoreControl ctrl in CoreControls)
					ctrl.ExecPaint(context);
			}
		}

		protected virtual void OnPaint(Graphics context)
		{
			context.Clear(Color.Black);
			context.FillRectangle(Brushes.White, ClientRectangle);
		}

		#endregion OnPaint

		#region SetBounds

		public virtual void SetBounds(int? x, int? y, int? w, int? h) => SetBoundsCore(x, y, w, h, true);
		
		protected void SetBoundsCore(int? x, int? y, int? w, int? h, bool notify = true)
		{
			x = x.HasValue ? x.Value : core_x;
			y = y.HasValue ? y.Value : core_y;
			w = w.HasValue ? w.Value : core_w;
			h = h.HasValue ? h.Value : core_h;
			SetBoundsCore(x.Value, y.Value, w.Value, h.Value, notify);
		}

		private void SetBoundsCore(int x, int y, int w, int h, bool notify = true)
		{
			w = Math.Max(0, w);
			h = Math.Max(0, h);
			int changed = 0;

			if (core_x != x || core_y != y)
			{
				core_x = x;
				core_y = y;
				changed |= 1;
			}

			if (core_w != w || core_h != h)
			{
				core_w = w;
				core_h = h;
				changed |= 2;
			}

			if (!notify)
				return;

			switch (changed)
			{
				case 1:
					OnBoundsChanged(EventArgs.Empty);
					OnLocationChanged(EventArgs.Empty);
					break;
				case 2:
					OnBoundsChanged(EventArgs.Empty);
					OnSizeChanged(EventArgs.Empty);
					break;
				case 3:
					OnBoundsChanged(EventArgs.Empty);
					OnLocationChanged(EventArgs.Empty);
					OnSizeChanged(EventArgs.Empty);
					break;
			}
		}

		#endregion SetBounds

		#region Client/Display/Bounds Rectangle

		protected virtual Rectangle GetClientRectangle()
		{
			int x = Math.Max(0, borderThickness.Left);
			int y = Math.Max(0, borderThickness.Top);
			int w = Math.Max(0, core_w - borderThickness.Horizontal);
			int h = Math.Max(0, core_h - borderThickness.Vertical);
			return new Rectangle(x, y, w, h);
		}

		#endregion Client/Display/Bounds Rectangle

		#endregion Methods

		#region Events

		#region Location

		private static readonly object EventLocationChanged = new object();

		public event EventHandler LocationChanged
		{
			add => EventStore.Add(EventLocationChanged, value);
			remove => EventStore.Remove(EventLocationChanged, value);
		}

		protected virtual void OnLocationChanged(EventArgs args)
		{
			EventStore.Find(EventLocationChanged)?.Invoke(this, args);
		}

		#endregion Location

		#region Size

		private static readonly object EventSizeChanged = new object();

		public event EventHandler SizeChanged
		{
			add => EventStore.Add(EventSizeChanged, value);
			remove => EventStore.Remove(EventSizeChanged, value);
		}

		protected virtual void OnSizeChanged(EventArgs args)
		{
			EventStore.Find(EventSizeChanged)?.Invoke(this, args);
		}

		#endregion Size

		#region Bounds

		private static readonly object EventBoundsChanged = new object();

		public event EventHandler BoundsChanged
		{
			add => EventStore.Add(EventBoundsChanged, value);
			remove => EventStore.Remove(EventBoundsChanged, value);
		}

		protected virtual void OnBoundsChanged(EventArgs args)
		{
			EventStore.Find(EventBoundsChanged)?.Invoke(this, args);
		}

		#endregion Bounds

		#endregion Events
	}
}
