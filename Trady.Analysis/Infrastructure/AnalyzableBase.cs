using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public abstract class AnalyzableBase<TInput, TMappedInput, TOutputToMap, TOutput> : IAnalyzable<TOutput>
    {
        private readonly IReadOnlyList<TMappedInput> _mappedInputs;
        private readonly IReadOnlyList<DateTime> _mappedDatetime;
        private readonly bool _isTInputCandle, _isTOutputAnalyzableTick;

        protected AnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper)
        {
            _isTInputCandle = typeof(TInput).Equals(typeof(Candle));
            _isTOutputAnalyzableTick = typeof(TOutput).Equals(typeof(AnalyzableTick<TOutputToMap>));
            if (_isTInputCandle != _isTOutputAnalyzableTick)
                throw new ArgumentException("TInput, TOutput not matched!");

            _mappedInputs = inputs.Select(inputMapper).ToList();
            if (_isTInputCandle)
                _mappedDatetime = inputs.Select(i => (i as Candle).DateTime).ToList();

            Cache = new ConcurrentDictionary<int, TOutputToMap>();
        }

        internal protected IReadOnlyList<TOutput> Compute(Func<int, TOutput> outputFunc, int? startIndex, int? endIndex)
        {
			int computedStartIndex = GetComputeStartIndex(startIndex);
			int computedEndIndex = GetComputeEndIndex(endIndex);
			return Enumerable.Range(computedStartIndex, computedEndIndex - computedStartIndex + 1)
				.Select(i => outputFunc(i))
				.ToList();      
        }

        public IReadOnlyList<TOutput> Compute(int? startIndex = null, int? endIndex = null)
            => Compute(i => this[i], startIndex, endIndex);

        protected virtual int GetComputeStartIndex(int? startIndex) => startIndex ?? 0;

        protected virtual int GetComputeEndIndex(int? endIndex) => endIndex ?? _mappedInputs.Count - 1;

        internal protected TOutput Get(Func<int, TOutputToMap> otmFunc, int index)
        {
			dynamic outputToMap = otmFunc(index);
			var datetime = index >= 0 && index < _mappedInputs.Count ? (_mappedDatetime?[index] ?? default(DateTime?)) : default(DateTime?);
			return _isTOutputAnalyzableTick ? AnalyzableTickMapper(datetime, outputToMap) : outputToMap;

			TOutput AnalyzableTickMapper(DateTime? d, TOutputToMap otm)
				=> (TOutput)typeof(TOutput).GetConstructors().First().Invoke(new object[] { d, otm });
        }

        public TOutput this[int index] => Get(ComputeByIndex, index);

		internal protected TOutputToMap ComputeByIndex(int index)
	        => Cache.GetOrAdd(index, i => ComputeByIndexImpl(_mappedInputs, i));

		protected abstract TOutputToMap ComputeByIndexImpl(IReadOnlyList<TMappedInput> mappedInputs, int index);

        protected ConcurrentDictionary<int, TOutputToMap> Cache { get; }

        IEnumerable IAnalyzable.Compute(int? startIndex, int? endIndex) => Compute(startIndex, endIndex);

        object IAnalyzable.this[int i] => this[i];
    }

    public abstract class AnalyzableBase<TInput, TOutput> : AnalyzableBase<TInput, TInput, TOutput, TOutput>
    {
        protected AnalyzableBase(IEnumerable<TInput> inputs) : base(inputs, i => i)
        {
        }
    }
}