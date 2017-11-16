using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class CommodityChannelIndex<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal CLose), TOutput>
    {
        public int PeriodCount { get; }

        public CommodityChannelIndex(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal CLose)> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal CLose)> mappedInputs, int index)
        {
            if (index < PeriodCount - 1)
                return null;

            var typicalPrices = mappedInputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Select(i => (i.High + i.Low + i.CLose) / 3);
            var average = typicalPrices.Average();
            var meanDeviation = typicalPrices.Select(tp => Math.Abs(tp - average)).Sum() / typicalPrices.Count();

            return (typicalPrices.Last() - average) / (0.015m * meanDeviation);
        }
    }

    public class CommodityChannelIndexByTuple : CommodityChannelIndex<(decimal High, decimal Low, decimal CLose), decimal?>
    {
        public CommodityChannelIndexByTuple(IEnumerable<(decimal High, decimal Low, decimal CLose)> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class CommodityChannelIndex : CommodityChannelIndex<IOhlcv, AnalyzableTick<decimal?>>
    {
        public CommodityChannelIndex(IEnumerable<IOhlcv> inputs, int periodCount) 
            : base(inputs, i => (i.High, i.Low, i.Close), periodCount)
        {
        }
    }
}
