using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Trady.Core;
using Trady.Core.Infrastructure;
using Trady.Analysis.Extension;

namespace Trady.Analysis.Infrastructure
{
    /// <summary>
    /// Generic base class for indicators & pattern matchers with in/out map
    /// </summary>
    /// <typeparam name="TInput">Source input type</typeparam>
    /// <typeparam name="TMappedInput">Mapped input type</typeparam>
    /// <typeparam name="TOutputToMap">Output type computed by mapped input type</typeparam>
    /// <typeparam name="TOutput">Target (Mapped) output type</typeparam>
    public abstract class AnalyzableBase<TInput, TMappedInput, TOutputToMap, TOutput> : IAnalyzable<TOutput>
    {
        private readonly bool _isTInputIOhlcvData, _isTOutputAnalyzableTick;

        internal protected readonly IReadOnlyList<TMappedInput> _mappedInputs;
        private readonly IReadOnlyList<DateTimeOffset> _mappedDateTimes;

        protected AnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper)
        {
            _isTInputIOhlcvData = typeof(IOhlcv).IsAssignableFrom(typeof(TInput));
            _isTOutputAnalyzableTick = typeof(IAnalyzableTick<TOutputToMap>).IsAssignableFrom(typeof(TOutput));
            if (_isTInputIOhlcvData != _isTOutputAnalyzableTick)
            {
                throw new ArgumentException("TInput, TOutput not matched!");
            }

            _mappedInputs = inputs.Select(inputMapper).ToList();
            if (_isTInputIOhlcvData)
            {
                _mappedDateTimes = inputs.Cast<IOhlcv>().Select(c => c.DateTime).ToList();
            }

            Cache = new Dictionary<int, TOutputToMap>();
        }

		public TOutput this[int index] => Map(ComputeByIndex, index);

		public IReadOnlyList<TOutput> Compute(int? startIndex = null, int? endIndex = null) => Compute(i => this[i], startIndex, endIndex);

        public IReadOnlyList<TOutput> Compute(IEnumerable<int> indexes) => Compute(i => this[i], indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbour(int index) => Compute(i => this[i], index);

		protected abstract TOutputToMap ComputeByIndexImpl(IReadOnlyList<TMappedInput> mappedInputs, int index);

        protected virtual int GetComputeStartIndex(int? startIndex) => startIndex ?? 0;

        protected virtual int GetComputeEndIndex(int? endIndex) => endIndex ?? _mappedInputs.Count - 1;

		protected IDictionary<int, TOutputToMap> Cache { get; }

		#region IAnalyzable implementation

		IEnumerable IAnalyzable.Compute(int? startIndex, int? endIndex) => Compute(startIndex, endIndex);

        IEnumerable IAnalyzable.Compute(IEnumerable<int> indexes) => Compute(indexes);

        (object Prev, object Current, object Next) IAnalyzable.ComputeNeighbour(int index) => ComputeNeighbour(index);

        object IAnalyzable.this[int i] => this[i];

        #endregion

        #region internal protected

		internal protected IReadOnlyList<TOutput> Compute(Func<int, TOutput> outputFunc, int? startIndex, int? endIndex)
		{
			int computedStartIndex = GetComputeStartIndex(startIndex);
			int computedEndIndex = GetComputeEndIndex(endIndex);
            return Enumerable.Range(computedStartIndex, computedEndIndex - computedStartIndex + 1).Select(outputFunc).ToList();
		}

		internal protected IReadOnlyList<TOutput> Compute(Func<int, TOutput> outputFunc, IEnumerable<int> indexes) => indexes.Select(outputFunc).ToList();

        // Can only get in-range values from IchimokuCloud
        internal protected (TOutput Prev, TOutput Current, TOutput Next) Compute(Func<int, TOutput> outputFunc, int index)
        {
            var prev = index > 0 ? outputFunc(index - 1) : default;
            var current = outputFunc(index);
            var next = index < _mappedInputs.Count - 1 ? outputFunc(index + 1) : default;

            return (prev, current, next);
        }

		internal protected TOutput Map(Func<int, TOutputToMap> otmFunc, int index)
		{
			dynamic outputToMap = otmFunc(index);
            var datetime = index >= 0 && index < _mappedInputs.Count ? (_mappedDateTimes?[index] ?? default(DateTime?)) : default(DateTime?);
			return _isTOutputAnalyzableTick ? AnalyzableTickMapper(datetime, outputToMap) : outputToMap;

			TOutput AnalyzableTickMapper(DateTimeOffset? d, TOutputToMap otm)
            {
                Type concreteType = _isTOutputAnalyzableTick ? typeof(AnalyzableTick<TOutputToMap>) : typeof(TOutput);
                return (TOutput)concreteType.GetConstructors().First().Invoke(new object[] { d, otm });
            }
		}

        protected internal TOutputToMap ComputeByIndex(int index) => Cache.GetOrAdd(index, i => ComputeByIndexImpl(_mappedInputs, i));

        #endregion
    }

    public abstract class AnalyzableBase<TInput, TOutput> : AnalyzableBase<TInput, TInput, TOutput, TOutput>
    {
        protected AnalyzableBase(IEnumerable<TInput> inputs) : base(inputs, i => i)
        {
        }
    }
}
