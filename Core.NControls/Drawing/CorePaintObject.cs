using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.NControls.Components;

namespace Core.NControls.Drawing
{
	public class CorePaintObject : IDisposable
	{
		protected GraphicsContainer ContextCache { get; private set; }

		public Graphics Context { get; }
		public CoreControl Control { get; }

		public CorePaintObject(Graphics context)
		{
			Context = context ?? throw new ArgumentNullException(nameof(context));
			ContextCache = context.BeginContainer();
		}

		public CorePaintObject(Graphics context, CoreControl control) : this(context)
		{
			Control = control ?? throw new ArgumentNullException(nameof(control));
			InitializeContext();
		}

		protected virtual void InitializeContext()
		{
			Rectangle rect = Control.Bounds;
			Context.TranslateTransform(rect.X, rect.Y);
			rect.X = rect.Y = 0;
			Context.SetClip(rect);
		}

		public void Dispose()
		{
			Context.EndContainer(ContextCache);
		}
	}
}
