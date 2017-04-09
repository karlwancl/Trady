using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public interface IIndicator : IAnalyzable
    {
        decimal[] Parameters { get; }
    }
}