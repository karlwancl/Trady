using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class FuncAnalyzable<TInput, TOutput> : NumericAnalyzableBase<TInput, TInput, TOutput>, IFuncAnalyzable<TOutput>
    {
        private readonly Func<IReadOnlyList<TInput>, int, IReadOnlyList<decimal>, IAnalyzeContext<TInput>, decimal?> _func;
        private readonly IAnalyzeContext<TInput> _ctx;

        public object Func => _func;

        public decimal[] Parameters { get; }

        public FuncAnalyzable(IEnumerable<TInput> inputs, params decimal[] parameters) : base(inputs, i => i)
        {
            Parameters = parameters;
        }

        private FuncAnalyzable(IEnumerable<TInput> inputs, Func<IReadOnlyList<TInput>, int, IReadOnlyList<decimal>, IAnalyzeContext<TInput>, decimal?> func, params decimal[] parameters)
            : this(inputs)
        {
            _func = func;
            Parameters = parameters;
            _ctx = new AnalyzeContext<TInput>(inputs);
        }

        public FuncAnalyzable<TInput, TOutput> Init(Func<IReadOnlyList<TInput>, int, IReadOnlyList<decimal>, IAnalyzeContext<TInput>, decimal?> func)
            => new FuncAnalyzable<TInput, TOutput>(_mappedInputs, func, Parameters);

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<TInput> mappedInputs, int index)
        {
            if (_func == null || _ctx == null)
                throw new NullReferenceException("No func is found for the analyzable, please ensure you have called Init method to init");

            return _func(mappedInputs, index, Parameters, _ctx);
        }

		public void Dispose()
		{
			_ctx?.Dispose();
		}
    }

    public class FuncAnalyzable : FuncAnalyzable<IOhlcv, AnalyzableTick<decimal?>>
    {
        public FuncAnalyzable(IEnumerable<IOhlcv> inputs, params decimal[] parameters) : base(inputs, parameters)
        {
        }
    }
}
