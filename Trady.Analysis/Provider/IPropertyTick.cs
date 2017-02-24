using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

namespace Trady.Analysis.Provider
{
    public interface IPropertyTick : ITick
    {
        string Name { get; }
        object Value { get; }
    }
}
