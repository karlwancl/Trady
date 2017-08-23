using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class OnBalanceVolumeTrend<TInput, TOutput> : AnalyzableBase<TInput, (decimal Close, decimal Volume), Trend?, TOutput>
    {
        readonly OnBalanceVolumeByTuple _obv;

        public OnBalanceVolumeTrend(IEnumerable<TInput> inputs, Func<TInput, (decimal Close, decimal Volume)> inputMapper) : base(inputs, inputMapper)
        {
			_obv = new OnBalanceVolumeByTuple(inputs.Select(inputMapper));
		}

        protected override Trend? ComputeByIndexImpl(IEnumerable<(decimal Close, decimal Volume)> mappedInputs, int index)
			=> index >= 1 ? StateHelper.IsTrending(_obv[index], _obv[index - 1]) : null;
	}

    public class OnBalanceVolumeTrendByTuple : OnBalanceVolumeTrend<(decimal Close, decimal Volume), Trend?>
    {
        public OnBalanceVolumeTrendByTuple(IEnumerable<(decimal Close, decimal Volume)> inputs) 
            : base(inputs, i => i)
        {
        }
    }

    public class OnBalanceVolumeTrend : OnBalanceVolumeTrend<Candle, AnalyzableTick<Trend?>>
    {
        public OnBalanceVolumeTrend(IEnumerable<Candle> inputs) 
            : base(inputs, i => (i.Close, i.Volume))
        {
        }
    }
}