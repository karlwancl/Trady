using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Trady.Core.Infrastructure
{
    public interface IRuleExecutor<TInput, TIndexed, TOutput> where TIndexed : IIndexedObject<TInput>
    {
        Predicate<TIndexed>[] Rules { get; }

        IReadOnlyList<TOutput> Execute(int? startIndex = null, int? endIndex = null);

        Func<TIndexed, int, TOutput> OutputFunc { get; }
    }
}