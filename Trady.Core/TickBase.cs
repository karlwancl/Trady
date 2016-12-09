using System;

namespace Trady.Core
{
    public abstract class TickBase : ITick
    {
        protected TickBase(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DateTime DateTime { get; private set; }
    }
}
