using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class AccumulationDistributionLineTrend<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close, decimal Volume), Trend?, TOutput>
    {
        private readonly AccumulationDistributionLineByTuple _accumDist;

        public AccumulationDistributionLineTrend(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close, decimal Volume)> inputMapper) : base(inputs, inputMapper)
        {
            _accumDist = new AccumulationDistributionLineByTuple(inputs.Select(inputMapper));
        }

        protected override Trend? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close, decimal Volume)> mappedInputs, int index)
            => index >= 1 ? StateHelper.IsTrending(_accumDist[index], _accumDist[index - 1]) : null;
    }

    public class AccumulationDistributionLineTrendByTuple : AccumulationDistributionLineTrend<(decimal High, decimal Low, decimal Close, decimal Volume), Trend?>
    {
        public AccumulationDistributionLineTrendByTuple(IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> inputs)
            : base(inputs, i => i)
        {
        }
    }

    public class AccumulationDistributionLineTrend : AccumulationDistributionLineTrend<Candle, AnalyzableTick<Trend?>>
    {
        public AccumulationDistributionLineTrend(IEnumerable<Candle> inputs)
            : base(inputs, i => (i.High, i.Low, i.Close, i.Volume))
        {
        }
    }
}