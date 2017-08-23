using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    public abstract class AnalyzableBase<TInput, TMappedInput, TOutputToMap, TOutput> : IAnalyzable<TOutput>
    {
        readonly IEnumerable<TMappedInput> _mappedInputs;
        readonly IEnumerable<DateTime> _mappedDatetime;

        protected AnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper)
        {
            _mappedInputs = inputs.Select(inputMapper).ToList();    // ToList here to create new list instance for faster computation

            IsTInputCandle = typeof(TInput).Equals(typeof(Candle));
            IsTOutputAnalyzableTick = typeof(TOutput).Equals(typeof(AnalyzableTick<TOutputToMap>));
            if (IsTInputCandle != IsTOutputAnalyzableTick)
                throw new ArgumentException("TInput, TOutput not matched!");

            if (IsTInputCandle)
                _mappedDatetime = inputs.Select(i => (i as Candle).DateTime).ToList();  // ToList here to create new list instance for faster computation

            Cache = new Dictionary<int, TOutputToMap>();
        }

        bool IsTInputCandle { get; }

        bool IsTOutputAnalyzableTick { get; }

        public IList<TOutput> Compute(int? startIndex = null, int? endIndex = null)
        {
            var ticks = new List<TOutput>();

            int computedStartIndex = GetComputeStartIndex(startIndex);
            int computedEndIndex = GetComputeEndIndex(endIndex);

            for (int i = computedStartIndex; i <= computedEndIndex; i++)
                ticks.Add(this[i]);

            return ticks;
        }

        public TOutput this[int index]
        {
            get
            {
                dynamic outputToMap = ComputeByIndex(_mappedInputs, index);
                var datetime = index >= 0 && index < _mappedInputs.Count() ? (_mappedDatetime?.ElementAt(index) ?? default(DateTime?)) : default(DateTime?);
                return IsTOutputAnalyzableTick ? AnalyzableTickMapper(datetime, outputToMap) : outputToMap;
            }
        }

        protected virtual int GetComputeStartIndex(int? startIndex) => startIndex ?? 0;

        protected virtual int GetComputeEndIndex(int? endIndex) => endIndex ?? _mappedInputs.Count() - 1;

        protected TOutputToMap ComputeByIndex(IEnumerable<TMappedInput> mappedInputs, int index)
        {
            if (Cache.TryGetValue(index, out TOutputToMap value))
                return value;
            
            value = ComputeByIndexImpl(mappedInputs, index);
            Cache.AddOrUpdate(index, value);
            return value;
        }

        protected abstract TOutputToMap ComputeByIndexImpl(IEnumerable<TMappedInput> mappedInputs, int index);

        IList IAnalyzable.Compute(int? startIndex, int? endIndex) => (IList)Compute(startIndex, endIndex);

        protected IDictionary<int, TOutputToMap> Cache { get; }

        static ConstructorInfo AnalyzableTickConstructor => typeof(TOutput).GetConstructors().First();

        static Func<DateTime?, TOutputToMap, TOutput> AnalyzableTickMapper => (d, otm) => (TOutput)AnalyzableTickConstructor.Invoke(new object[] { d, otm });
    }

    //public abstract class AnalyzableBase<TInput, TOutput> : AnalyzableBase<TInput, TInput, TOutput, TOutput>
    //{
    //    protected AnalyzableBase(IEnumerable<TInput> inputs) : base(inputs, i => i)
    //    {
    //    }
    //}
}
