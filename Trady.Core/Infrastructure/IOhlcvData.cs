using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Infrastructure
{
    public interface IOhlcvData : ITick
    {
        decimal Open { get; }

        decimal High { get; }

        decimal Low { get; }

        decimal Close { get; }

        decimal Volume { get; }
    }
}
