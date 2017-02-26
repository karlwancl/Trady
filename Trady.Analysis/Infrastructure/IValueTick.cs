using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public interface IValueTick : ITick
    {
        string Name { get; }
        object Value { get; }
    }
}