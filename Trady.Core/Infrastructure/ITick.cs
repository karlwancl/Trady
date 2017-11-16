using System;

namespace Trady.Core.Infrastructure
{
    public interface ITick
    {
        DateTimeOffset DateTime { get; }
    }
}