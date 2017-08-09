using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_bearish_reversal_patterns#bearish_engulfing
    /// </summary>
    public class BearishEngulfingPattern<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        UpTrendByTuple _upTrend;
        BullishByTuple _bullish;
        BearishByTuple _bearish;

        public BearishEngulfingPattern(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, Func<TInput, bool?, TOutput> outputMapper, int upTrendPeriodCount = 3) : base(inputs, inputMapper, outputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            _upTrend = new UpTrendByTuple(mappedInputs.Select(i => (i.High, i.Low)), upTrendPeriodCount);

            var ocs = mappedInputs.Select(i => (i.Open, i.Close));
            _bullish = new BullishByTuple(ocs);
            _bearish = new BearishByTuple(ocs);

            UpTrendPeriodCount = upTrendPeriodCount;
        }

        public int UpTrendPeriodCount { get; }

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
			if (index < 1) return null;
            bool isEngulf = mappedInputs.ElementAt(index).Open > mappedInputs.ElementAt(index - 1).Close && mappedInputs.ElementAt(index).Close < mappedInputs.ElementAt(index - 1).Open;
			return (_upTrend[index - 1] ?? false) && _bullish[index - 1] && _bearish[index] && isEngulf;
        }
    }

    public class BearishEngulfingPatternByTuple : BearishEngulfingPattern<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public BearishEngulfingPatternByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int upTrendPeriodCount = 3) 
            : base(inputs, i => i, (i, otm) => otm, upTrendPeriodCount)
        {
        }
    }

    public class BearishEngulfingPattern : BearishEngulfingPattern<Candle, AnalyzableTick<bool?>>
    {
        public BearishEngulfingPattern(IEnumerable<Candle> inputs, int upTrendPeriodCount = 3)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<bool?>(i.DateTime, otm), upTrendPeriodCount)
        {
        }
    }
}
