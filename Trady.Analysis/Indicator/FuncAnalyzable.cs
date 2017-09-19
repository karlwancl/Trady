using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class FuncAnalyzable<TInput, TOutput> : NumericAnalyzableBase<TInput, TInput, TOutput>
    {
        private readonly Func<IReadOnlyList<TInput>, int, decimal?> _func;

        public decimal[] Parameters { get; }

        public FuncAnalyzable(IEnumerable<TInput> inputs, params decimal[] parameters) : base(inputs, i => i)
        {
            Parameters = parameters;
        }

        private FuncAnalyzable(IEnumerable<TInput> inputs, Func<IReadOnlyList<TInput>, int, decimal?> func, params decimal[] parameters)
            : this(inputs, parameters)
        {
            _func = func;
        }

        public FuncAnalyzable<TInput, TOutput> Init(Func<IReadOnlyList<TInput>, int, decimal?> func)
            => new FuncAnalyzable<TInput, TOutput>(_mappedInputs, func, Parameters);

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<TInput> mappedInputs, int index)
        {
            if (_func == null)
                throw new NullReferenceException("No func is found for the analyzable, please ensure you have called Init method to init");

            Console.WriteLine(index);
            return _func(mappedInputs, index);
        }
    }

    public class FuncAnalyzable : FuncAnalyzable<Candle, AnalyzableTick<decimal?>>
    {
        public FuncAnalyzable(IEnumerable<Candle> inputs, params decimal[] parameters) : base(inputs, parameters)
        {
        }
    }
}
