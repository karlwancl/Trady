using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class IchimokuCloud<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), (decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan), TOutput>
    {
        LowestLowByTuple _shortLowestLow, _middleLowestLow;
        Func<int, decimal?> _conversionLine, _baseLine, _leadingSpanB;

        public IchimokuCloud(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int shortPeriodCount, int middlePeriodCount, int longPeriodCount) : base(inputs, inputMapper)
        {
            var highs = inputs.Select(i => inputMapper(i).High);
            var lows = inputs.Select(i => inputMapper(i).Low);

			var shortHighestHigh = new HighestHighByTuple(highs, shortPeriodCount);
			_shortLowestLow = new LowestLowByTuple(lows, shortPeriodCount);
			_conversionLine = i => (shortHighestHigh[i] + _shortLowestLow[i]) / 2;

			var middleHighestHigh = new HighestHighByTuple(highs, middlePeriodCount);
			_middleLowestLow = new LowestLowByTuple(lows, middlePeriodCount);
			_baseLine = i => (middleHighestHigh[i] + _middleLowestLow[i]) / 2;

			var longHighestHigh = new HighestHighByTuple(highs, longPeriodCount);
			var longLowestLow = new LowestLowByTuple(lows, longPeriodCount);
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

        protected override (decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan) ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
			// Current
			bool dataInRange = index >= 0 && index < mappedInputs.Count();
			var conversionLine = dataInRange ? _conversionLine(index) : null;
			var baseLine = dataInRange ? _baseLine(index) : null;

			// Leading
			bool dataAfterMiddlePeriodCount = index >= MiddlePeriodCount;
			var leadingSpanA = dataAfterMiddlePeriodCount ? (_conversionLine(index - MiddlePeriodCount) + _baseLine(index - MiddlePeriodCount)) / 2 : null;
			var leadingSpanB = dataAfterMiddlePeriodCount ? _leadingSpanB(index - MiddlePeriodCount) : null;

			// Lagging
			bool dataBeforeEquityCountMinusMiddlePeriodCount = index <= mappedInputs.Count() - MiddlePeriodCount;
            var laggingSpan = dataBeforeEquityCountMinusMiddlePeriodCount ? mappedInputs.ElementAt(index + MiddlePeriodCount - 1).Close : (decimal?)null;

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

    public class IchimokuCloud : IchimokuCloud<Candle, AnalyzableTick<(decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)>>
    {
        public IchimokuCloud(IEnumerable<Candle> inputs, int shortPeriodCount, int middlePeriodCount, int longPeriodCount) 
            : base(inputs, i => (i.High, i.Low, i.Close), shortPeriodCount, middlePeriodCount, longPeriodCount)
        {
        }
    }
}