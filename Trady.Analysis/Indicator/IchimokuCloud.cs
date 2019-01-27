using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class IchimokuCloud<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), (decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan), TOutput>
    {
        private LowestByTuple _shortLowestLow, _middleLowestLow;
        private readonly Func<int, decimal?> _leadingSpanB;
        private readonly Func<int, decimal?> _baseLine;
        private readonly Func<int, decimal?> _conversionLine;

        public IchimokuCloud(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int shortPeriodCount, int middlePeriodCount, int longPeriodCount) : base(inputs, inputMapper)
        {
            var highs = inputs.Select(i => inputMapper(i).High);
            var lows = inputs.Select(i => inputMapper(i).Low);

            var shortHighestHigh = new HighestByTuple(highs, shortPeriodCount);
            _shortLowestLow = new LowestByTuple(lows, shortPeriodCount);
            _conversionLine = i => (shortHighestHigh[i] + _shortLowestLow[i]) / 2;

            var middleHighestHigh = new HighestByTuple(highs, middlePeriodCount);
            _middleLowestLow = new LowestByTuple(lows, middlePeriodCount);
            _baseLine = i => (middleHighestHigh[i] + _middleLowestLow[i]) / 2;

            var longHighestHigh = new HighestByTuple(highs, longPeriodCount);
            var longLowestLow = new LowestByTuple(lows, longPeriodCount);
            _leadingSpanB = i => (longHighestHigh[i] + longLowestLow[i]) / 2;

            ShortPeriodCount = shortPeriodCount;
            MiddlePeriodCount = middlePeriodCount;
            LongPeriodCount = longPeriodCount;
        }

        public int ShortPeriodCount { get; }

        public int MiddlePeriodCount { get; }

        public int LongPeriodCount { get; }

        protected override int GetComputeStartIndex(int? startIndex)
            => base.GetComputeStartIndex(startIndex) - MiddlePeriodCount + 1;

        protected override int GetComputeEndIndex(int? endIndex)
            => base.GetComputeEndIndex(endIndex) + MiddlePeriodCount;

        protected override (decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan) ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            // Current
            var dataInRange = index >= 0 && index < mappedInputs.Count;
            var conversionLine = dataInRange ? _conversionLine(index) : default;
            var baseLine = dataInRange ? _baseLine(index) : default;

            // Leading
            var dataAfterMiddlePeriodCount = index >= MiddlePeriodCount;
            var leadingSpanA = dataAfterMiddlePeriodCount ? (_conversionLine(index - MiddlePeriodCount) + _baseLine(index - MiddlePeriodCount)) / 2 : default;
            var leadingSpanB = dataAfterMiddlePeriodCount ? _leadingSpanB(index - MiddlePeriodCount) : default;

            // Lagging
            var dataBeforeEquityCountMinusMiddlePeriodCount = index <= mappedInputs.Count - MiddlePeriodCount;
            var laggingSpan = dataBeforeEquityCountMinusMiddlePeriodCount ? (decimal?)mappedInputs[index + MiddlePeriodCount - 1].Close : default;

            return (conversionLine, baseLine, leadingSpanA, leadingSpanB, laggingSpan);
        }
    }

    public class IchimokuCloudByTuple : IchimokuCloud<(decimal High, decimal Low, decimal Close), (decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)>
    {
        public IchimokuCloudByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int shortPeriodCount, int middlePeriodCount, int longPeriodCount)
            : base(inputs, i => i, shortPeriodCount, middlePeriodCount, longPeriodCount)
        {
        }
    }

    public class IchimokuCloud : IchimokuCloud<IOhlcv, AnalyzableTick<(decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)>>
    {
        public IchimokuCloud(IEnumerable<IOhlcv> inputs, int shortPeriodCount, int middlePeriodCount, int longPeriodCount)
            : base(inputs, i => (i.High, i.Low, i.Close), shortPeriodCount, middlePeriodCount, longPeriodCount)
        {
        }
    }
}
