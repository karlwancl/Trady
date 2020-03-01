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
            var firstNonNullCciIndex = 2 * (PeriodCount - 1);

            if (index < firstNonNullCciIndex)
                return default;

            var typicalPrices = mappedInputs
                .Skip(index - firstNonNullCciIndex)
                .Take(firstNonNullCciIndex + 1)
                .Select(i => (i.High + i.Low + i.Close) / 3)
                .ToList();

            var typicalPricesSmas = Enumerable
                .Range(PeriodCount - 1, PeriodCount)
                .Select(i => typicalPrices.Skip(i - (PeriodCount - 1)).Take(PeriodCount).Average())
                .ToList();

            var deviation = Enumerable
                .Range(0, PeriodCount)
                .Select(i => Math.Abs(typicalPrices.ElementAt(i + PeriodCount - 1) - typicalPricesSmas.ElementAt(i)))
                .ToList();

            var meanDeviation = deviation.Average();

            return meanDeviation == 0 ? default : (typicalPrices.Last() - typicalPricesSmas.Last()) / (0.015m * meanDeviation);
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
