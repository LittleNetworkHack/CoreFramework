using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class StateComponent<T>
    {
        public bool Processing { get; protected set; }

        public virtual bool Enter()
        {
            if (Processing)
                return true;

            Processing = true;
            return false;
        }

        public virtual void Leave()
        {
            Processing = false;
        }

		public virtual void ApplyState(T value) { }
        public virtual void SetState(T value) { }
    }
}
