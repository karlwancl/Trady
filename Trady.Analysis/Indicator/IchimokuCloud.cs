using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class IchimokuCloud : AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)>
    {
        private LowestLow _shortLowestLow, _middleLowestLow;
        private Func<int, decimal?> _conversionLine, _baseLine, _leadingSpanB;

        public IchimokuCloud(IList<Candle> candles, int shortPeriodCount, int middlePeriodCount, int longPeriodCount) :
            this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), shortPeriodCount, middlePeriodCount, longPeriodCount)
        {
        }

        public IchimokuCloud(IList<(decimal High, decimal Low, decimal Close)> inputs, int shortPeriodCount, int middlePeriodCount, int longPeriodCount)
            : base(inputs)
        {
            var highs = inputs.Select(i => i.High).ToList();
            var lows = inputs.Select(i => i.Low).ToList();

            var shortHighestHigh = new HighestHigh(highs, shortPeriodCount);
            _shortLowestLow = new LowestLow(lows, shortPeriodCount);
            _conversionLine = i => (shortHighestHigh[i] + _shortLowestLow[i]) / 2;

            var middleHighestHigh = new HighestHigh(highs, middlePeriodCount);
            _middleLowestLow = new LowestLow(lows, middlePeriodCount);
            _baseLine = i => (middleHighestHigh[i] + _middleLowestLow[i]) / 2;

            var longHighestHigh = new HighestHigh(highs, longPeriodCount);
            var longLowestLow = new LowestLow(lows, longPeriodCount);
            _leadingSpanB = i => (longHighestHigh[i] + longLowestLow[i]) / 2;

            ShortPeriodCount = shortPeriodCount;
            MiddlePeriodCount = middlePeriodCount;
            LongPeriodCount = longPeriodCount;
        }

        public int ShortPeriodCount { get; private set; }

        public int MiddlePeriodCount { get; private set; }

        public int LongPeriodCount { get; private set; }

        protected override (decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan) ComputeByIndexImpl(int index)
        {
            // Current
            bool dataInRange = index >= 0 && index < Inputs.Count;
            var conversionLine = dataInRange ? _conversionLine(index) : null;
            var baseLine = dataInRange ? _baseLine(index) : null;

            // Leading
            bool dataAfterMiddlePeriodCount = index >= MiddlePeriodCount;
            var leadingSpanA = dataAfterMiddlePeriodCount ? (_conversionLine(index - MiddlePeriodCount) + _baseLine(index - MiddlePeriodCount)) / 2 : null;
            var leadingSpanB = dataAfterMiddlePeriodCount ? _leadingSpanB(index - MiddlePeriodCount) : null;

            // Lagging
            bool dataBeforeEquityCountMinusMiddlePeriodCount = index <= Inputs.Count - MiddlePeriodCount;
            var laggingSpan = dataBeforeEquityCountMinusMiddlePeriodCount ? Inputs[index + MiddlePeriodCount - 1].Close : (decimal?)null;

            return (conversionLine, baseLine, leadingSpanA, leadingSpanB, laggingSpan);
        }

        protected override int GetComputeStartIndex(int? startIndex)
            => base.GetComputeStartIndex(startIndex) - MiddlePeriodCount + 1;

        protected override int GetComputeEndIndex(int? endIndex)
            => base.GetComputeEndIndex(endIndex) + MiddlePeriodCount;
    }
}