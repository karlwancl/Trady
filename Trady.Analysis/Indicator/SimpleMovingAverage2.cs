using System;
using System.Collections.Generic;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class SimpleMovingAverage2<TInput, TOutput> : AnalyzableBase2<TInput, decimal, decimal?, TOutput>
    {
		public int PeriodCount { get; }

        public SimpleMovingAverage2(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount)
            : base(inputs, inputMapper, outputMapper)
        {
            PeriodCount = periodCount;
        }

		/// <summary>
		/// Compute Logic for the Simple Moving Average is handled here, without thinking on how do I get the decimal
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns></returns>
		protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mis, int index)
            => mis.Avg(PeriodCount, index);

        // Not using facilitator here because function call may be too long, and there are extension methods that does similar thing
    }

    /// <summary>
    /// Use "SimpleMovingAverageByTuple" as class name here, plain value/tuple is used as the input type, use "xxxByTuple" for naming
    /// </summary>
    public class SimpleMovingAverageByTuple : SimpleMovingAverage2<decimal, decimal?>
    {
        public SimpleMovingAverageByTuple(IEnumerable<decimal> values, int periodCount)
            : base(values, c => c, (c, otm) => otm, periodCount) { }
    }

    /// <summary>
    /// Use "SimpleMovingAverage" as class name here, candle is the first-class citizen of Trady class, indicators using candle should have the 'default' name
    /// </summary>
    public class SimpleMovingAverage2 : SimpleMovingAverage2<Candle, AnalyzableTick<decimal?>>
    {
        public SimpleMovingAverage2(IEnumerable<Candle> candles, int periodCount)
            : base(candles, c => c.Close, (c, otm) => new AnalyzableTick<decimal?>(c.DateTime, otm), periodCount) { }
    }

    /// <summary>
    /// Extension methods as you implemented for IList. I changed to IEnumerable as I didn't find any specific logic 
    /// for IList needed and IEnumerable was enough.
    /// Yea, you're right, IEnumerable is enough for the interface, we may convert the others too :)
    /// </summary>
    public static class CandleExtension
    {
        public static SimpleMovingAverage2<Candle, AnalyzableTick<decimal?>> Sma2(this IEnumerable<Candle> candles, int periodCount)
            => new SimpleMovingAverage2(candles, periodCount);

        public static SimpleMovingAverage2<decimal, decimal?> Sma2(this IEnumerable<decimal> values, int periodCount)
            => new SimpleMovingAverageByTuple(values, periodCount);
    }
}
