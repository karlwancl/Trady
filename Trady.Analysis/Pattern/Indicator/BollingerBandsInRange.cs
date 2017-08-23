using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class BollingerBandsInRange<TInput, TOutput> : AnalyzableBase<TInput, decimal, Overboundary?, TOutput>
    {
        readonly BollingerBandsByTuple _bb;

        public BollingerBandsInRange(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount, decimal sdCount) : base(inputs, inputMapper)
        {
			_bb = new BollingerBandsByTuple(inputs.Select(inputMapper), periodCount, sdCount);
		}

        protected override Overboundary? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
        {
			var result = _bb[index];
            return StateHelper.IsOverbound(mappedInputs.ElementAt(index), result.LowerBand, result.UpperBand);
        }
    }

    public class BollingerBandsInRangeByTuple : BollingerBandsInRange<decimal, Overboundary?>
    {
        public BollingerBandsInRangeByTuple(IEnumerable<decimal> inputs, int periodCount, decimal sdCount) 
            : base(inputs, i => i, periodCount, sdCount)
        {
        }
    }

    public class BollingerBandsInRange : BollingerBandsInRange<Candle, AnalyzableTick<Overboundary?>>
    {
        public BollingerBandsInRange(IEnumerable<Candle> inputs, int periodCount, decimal sdCount) 
            : base(inputs, i => i.Close, periodCount, sdCount)
        {
        }
    }
}