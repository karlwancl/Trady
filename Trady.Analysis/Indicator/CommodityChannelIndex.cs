using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class CommodityChannelIndex<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), TOutput>
    {
        public int PeriodCount { get; }

        public CommodityChannelIndex(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < PeriodCount - 1)
                return default;

            var typicalPrices = mappedInputs
                .Skip(index - PeriodCount + 1)
                .Take(PeriodCount)
                .Select(i => (i.High + i.Low + i.Close) / 3);

            var average = typicalPrices.Average();
            var meanDeviation = typicalPrices
                .Select(tp => Math.Abs(tp - average)).Sum() / typicalPrices.Count();

            return meanDeviation == 0 ? default : (typicalPrices.Last() - average) / (0.015m * meanDeviation);
        }
    }

    public class CommodityChannelIndexByTuple : CommodityChannelIndex<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public CommodityChannelIndexByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount)
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
