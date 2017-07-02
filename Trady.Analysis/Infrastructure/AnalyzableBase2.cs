using System;
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
    public abstract class AnalyzableBase2<TInput, TMappedInput, TOutputToMap, TOutput> : IAnalyzable2<TInput, TOutput>
    {
        readonly Func<TInput, TMappedInput> _inputMapper;
        readonly Func<TInput, TOutputToMap, TOutput> _outputMapper;

        protected AnalyzableBase2(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper, Func<TInput, TOutputToMap, TOutput> outputMapper)
        {
            _inputMapper = inputMapper;
            _outputMapper = outputMapper;
            Cache = new Dictionary<int, TOutputToMap>();
            Inputs = inputs;
        }

        public IEnumerable<TInput> Inputs { get; }

        /// <summary>
        /// Gets the mapped inputs, instantiate when it's needed
        /// </summary>
        /// <value>The mapped inputs.</value>
        Lazy<IEnumerable<TMappedInput>> MappedInputs => new Lazy<IEnumerable<TMappedInput>>(() => Inputs.Select(_inputMapper));

        public IList<TOutput> Compute(int? startIndex = null, int? endIndex = null)
        {
            var ticks = new List<TOutput>();

            int computedStartIndex = GetComputeStartIndex(startIndex);
            int computedEndIndex = GetComputeEndIndex(endIndex);

            for (int i = computedStartIndex; i <= computedEndIndex; i++)
                ticks.Add(this[i]);

            return ticks;
        }

        public TOutput this[int i]
        {
            get
            {
                var outputToMap = ComputeByIndex(MappedInputs.Value, i);
                var input = (i >= 0 && i < Inputs.Count()) ? Inputs.ElementAt(i) : default(TInput); // Special case for inputs count < outputs count (e.g. Ichimoku Cloud)
                return _outputMapper(input, outputToMap);
            }
        }

        protected virtual int GetComputeStartIndex(int? startIndex) => startIndex ?? 0;

        protected virtual int GetComputeEndIndex(int? endIndex) => endIndex ?? Inputs.Count() - 1;

        // Change signature here because we don't want dev to use "Inputs" directly for computation but we want them to use the mapped one
        // It de-couples more from the state of an object (i.e. de-couple from "Inputs" variable)
        protected TOutputToMap ComputeByIndex(IEnumerable<TMappedInput> mis, int index)
        {
            if (Cache.TryGetValue(index, out TOutputToMap value))
                return value;

            value = ComputeByIndexImpl(mis, index);
            Cache.AddOrUpdate(index, value);
            return value;
        }

        protected abstract TOutputToMap ComputeByIndexImpl(IEnumerable<TMappedInput> mis, int index);

        IDictionary<int, TOutputToMap> Cache { get; }
    }

    public abstract class AnalyzableBase2<TInput, TOutput> : AnalyzableBase2<TInput, TInput, TOutput, TOutput>
    {
        protected AnalyzableBase2(IEnumerable<TInput> inputs) : base(inputs, c => c, (c, otm) => otm) { }
    }
}
