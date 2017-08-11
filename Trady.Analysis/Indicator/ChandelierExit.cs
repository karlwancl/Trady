using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class ChandelierExit<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), (decimal? Long, decimal? Short), TOutput>
    {
        readonly HighestHighByTuple _hh;
        readonly LowestLowByTuple _ll;
        readonly AverageTrueRangeByTuple _atr;

        public ChandelierExit(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, Func<TInput, (decimal? Long, decimal? Short), TOutput> outputMapper, int periodCount, decimal atrCount) : base(inputs, inputMapper, outputMapper)
        {
			_hh = new HighestHighByTuple(inputs.Select(i => inputMapper(i).High), periodCount);
			_ll = new LowestLowByTuple(inputs.Select(i => inputMapper(i).Low), periodCount);
			_atr = new AverageTrueRangeByTuple(inputs.Select(inputMapper), periodCount);

			PeriodCount = periodCount;
			AtrCount = atrCount;
        }

        public int PeriodCount { get; }

        public decimal AtrCount { get; }

        protected override (decimal? Long, decimal? Short) ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
			var atr = _atr[index];
			var @long = _hh[index] - atr * AtrCount;
			var @short = _ll[index] + atr * AtrCount;
			return (@long, @short);
        }
    }

    public class ChandelierExitByTuple : ChandelierExit<(decimal High, decimal Low, decimal Close), (decimal? Long, decimal? Short)>
    {
        public ChandelierExitByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, decimal atrCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount, atrCount)
        {
        }
    }

    public class ChandelierExit : ChandelierExit<Candle, AnalyzableTick<(decimal? Long, decimal? Short)>>
    {
        public ChandelierExit(IEnumerable<Candle> inputs, int periodCount, decimal atrCount) 
            : base(inputs, i => (i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<(decimal? Long, decimal? Short)>(i.DateTime, otm), periodCount, atrCount)
        {
        }
    }
}