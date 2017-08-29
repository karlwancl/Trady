using System;
using System.Collections.Generic;

namespace Trady.Analysis.Strategy.Rule
{
    public interface IRuleExecutor<TInput, TIndexed, TOutput> where TIndexed: IIndexedObject<TInput>
    {
        Func<IRule<TIndexed>>[] Rules { get; }
        
        IEnumerable<TOutput> Execute(IEnumerable<TInput> inputs, int? startIndex = null, int? endIndex = null);

        Func<TIndexed, int, TOutput> OutputFunc { get; }
    }
}