using System;

namespace Trady.Core.Infrastructure
{
    public interface INumericAnalyzable<TOutput> 
        : IDiffAnalyzable<TOutput>, ISmaAnalyzable<TOutput>, IEmaAnalyzable<TOutput>, IMemaAnalyzable<TOutput>
    {
    }
}