using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    /// <summary>
    /// Generic base class for indicators & pattern matchers with in/out map
    /// <typeparam name="TInput">Source input type</typeparam>
    /// <typeparam name="TMappedInput">Mapped input type</typeparam>
    /// <typeparam name="TOutputToMap">Output type computed by mapped input type</typeparam>
    /// <typeparam name="TOutput">Target (Mapped) output type</typeparam>
    /// </summary>
    public abstract class AnalyzableBase<TInput, TMappedInput, TOutputToMap, TOutput> : IAnalyzable<TInput, TOutput>
    {
		readonly IEnumerable<TInput> _inputs;
		readonly Func<TInput, TMappedInput> _inputMapper;
        readonly Func<TInput, TOutputToMap, TOutput> _outputMapper;

        protected AnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper, Func<TInput, TOutputToMap, TOutput> outputMapper)
        {
			_inputs = inputs;
			_inputMapper = inputMapper;
			_outputMapper = outputMapper;
			Cache = new Dictionary<int, TOutputToMap>();
        }

        public IEnumerable<TInput> Inputs => _inputs;

        Lazy<IEnumerable<TMappedInput>> MappedInputs => new Lazy<IEnumerable<TMappedInput>>(() => _inputs.Select(_inputMapper));

        public IList<TOutput> Compute(int? startIndex = null, int? endIndex = null)
        {
            var ticks = new List<TOutput>();

            int computedStartIndex = GetComputeStartIndex(startIndex);
            int computedEndIndex = GetComputeEndIndex(endIndex);

            for (int i = computedStartIndex; i <= computedEndIndex; i++)
                ticks.Add(this[i]);

            return ticks;
        }

        // TODO: using static to provide functional style for calculation
        public TOutput this[int index]
        {
            get
            {
                var outputToMap = ComputeByIndex(MappedInputs.Value, index);
                var input = index >= 0 && index < _inputs.Count() ? _inputs.ElementAt(index) : default(TInput); // Special case for inputs count < outputs count (e.g. Ichimoku Cloud)
                return _outputMapper(input, outputToMap);
            }
        }

        protected virtual int GetComputeStartIndex(int? startIndex) => startIndex ?? 0;

        protected virtual int GetComputeEndIndex(int? endIndex) => endIndex ?? _inputs.Count() - 1;

        protected TOutputToMap ComputeByIndex(IEnumerable<TMappedInput> mappedInputs, int index)
        {
            if (Cache.TryGetValue(index, out TOutputToMap value))
                return value;

            value = ComputeByIndexImpl(mappedInputs, index);
            Cache.AddOrUpdate(index, value);
            return value;
        }

        protected abstract TOutputToMap ComputeByIndexImpl(IEnumerable<TMappedInput> mappedInputs, int index);

        protected IDictionary<int, TOutputToMap> Cache { get; }
    }

    public abstract class AnalyzableBase<TInput, TOutput> : AnalyzableBase<TInput, TInput, TOutput, TOutput>
    {
        protected AnalyzableBase(IEnumerable<TInput> inputs) : base(inputs, c => c, (c, otm) => otm) { }
    }
}
