using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class Aroon<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low), (decimal? Up, decimal? Down), TOutput>
    {
        readonly HighestHighByTuple _hh;
        readonly LowestLowByTuple _ll;

        public Aroon(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low)> inputMapper, Func<TInput, (decimal? Up, decimal? Down), TOutput> outputMapper, int periodCount) : base(inputs, inputMapper, outputMapper)
        {
			_hh = new HighestHighByTuple(inputs.Select(i => inputMapper(i).High), periodCount);
			_ll = new LowestLowByTuple(inputs.Select(i => inputMapper(i).Low), periodCount);
			PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override (decimal? Up, decimal? Down) ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low)> mappedInputs, int index)
        {
			if (index < PeriodCount - 1)
				return (null, null);

			var nearestIndexToHighestHigh = index - PeriodCount + 1 + mappedInputs
				.Skip(index - PeriodCount + 1)
				.Take(PeriodCount)
				.FindLastIndexOrDefault(i => i.High == _hh[index]);

			var nearestIndexToLowestLow = index - PeriodCount + 1 + mappedInputs
				.Skip(index - PeriodCount + 1)
				.Take(PeriodCount)
				.FindLastIndexOrDefault(i => i.Low == _ll[index]);

			var up = 100.0m * (PeriodCount - (index - nearestIndexToHighestHigh)) / PeriodCount;
			var down = 100.0m * (PeriodCount - (index - nearestIndexToLowestLow)) / PeriodCount;

			return (up, down);        
        }
    }

    public class AroonByTuple : Aroon<(decimal High, decimal Low), (decimal? Up, decimal? Down)>
    {
        public AroonByTuple(IEnumerable<(decimal High, decimal Low)> inputs, int periodCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount)
        {
        }
    }

    public class Aroon : Aroon<Candle, AnalyzableTick<(decimal? Up, decimal? Down)>>
    {
        public Aroon(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => (i.High, i.Low), (i, otm) => new AnalyzableTick<(decimal? Up, decimal? Down)>(i.DateTime, otm), periodCount)
        {
        }
    }
}