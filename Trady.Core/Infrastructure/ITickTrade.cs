using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Infrastructure
{
    public interface ITickTrade : ITick
    {        
        decimal Price { get; set; }
        decimal Volume { get; set; }        
    }
}
