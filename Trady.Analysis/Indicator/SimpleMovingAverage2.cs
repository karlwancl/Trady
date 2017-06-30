using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class SimpleMovingAverage2<TInput> : AnalyzableBase2<TInput, decimal?>
    {
        public int PeriodCount { get; }

        public SimpleMovingAverage2(IEnumerable<TInput> inputs, Func<TInput, decimal> mappingFunction, int periodCount)
            : base(inputs, mappingFunction)
        {
            PeriodCount = periodCount;
        }

        /// <summary>
        /// Compute Logic for the Simple Moving Average is handled here, without thinking on how do I get the decimal
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns></returns>
        protected override decimal? ComputeByIndexImpl(int index)
        {
            return Inputs.Select(this.MappingFunction)
                .Avg(PeriodCount, index);
        }

        /// <summary>
        /// Facilitator for creating a SimpleMovingAverage Indicator from  the same parameter as the original implementation
        /// </summary>
        /// <param name="inputs">IEnumerable of Candles</param>
        /// <param name="periodCount">Period Count</param>
        /// <returns></returns>
        public static SimpleMovingAverage2<Candle> GetSimpleMovingAverage(IEnumerable<Candle> inputs, int periodCount)
        {
            return new SimpleMovingAverage2<Candle>(inputs, c => c.Close, periodCount);
        }

        /// <summary>
        /// Facilitator for creating a SimpleMovingAverage Indicator from  the same parameter as the implementation of V2.0
        /// </summary>
        /// <param name="inputs">IEnumerable of decimal</param>
        /// <param name="periodCount">Period Count</param>
        /// <returns></returns>
        public static SimpleMovingAverage2<decimal> GetSimpleMovingAverage(IEnumerable<decimal> inputs, int periodCount)
        {
            return new SimpleMovingAverage2<decimal>(inputs, c => c, periodCount);
        }


    }

    /// <summary>
    /// If you want to implement a class instead of a method, you could also do this.
    /// On the constructor you just simply pass a funcion T -> Decimal and that's all
    /// </summary>
    public class DecimalSimpleMovingAverage : SimpleMovingAverage2<decimal>
    {
        public DecimalSimpleMovingAverage(IEnumerable<decimal> values, int periodCount)
            : base(values, c => c, periodCount)
        {
        }
    }

    public class CandleSimpleMovingAverage : SimpleMovingAverage2<Candle>
    {
        public CandleSimpleMovingAverage(IEnumerable<Candle> candles, int periodCount)
            : base(candles, c => c.Close, periodCount)
        {
        }
    }

    /// <summary>
    /// Extension methods as you implemented for IList. I changed to IEnumerable as I didn't find any specific logic 
    /// for IList needed and IEnumerable was enough.
    /// </summary>
    public static class CandleExtension
    {
        public static SimpleMovingAverage2<Candle> SimpleMovingAverage(this IEnumerable<Candle> candles,
            int periodCount)
        {
            return SimpleMovingAverage2<Candle>.GetSimpleMovingAverage(candles, periodCount);
        }

        public static SimpleMovingAverage2<decimal> SimpleMovingAverage(this IEnumerable<decimal> values,
            int periodCount)
        {
            return SimpleMovingAverage2<Candle>.GetSimpleMovingAverage(values, periodCount);
        }
    }

}
