using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class AccumulationDistributionLineTrend : AnalyzableBase<(decimal High, decimal Low, decimal Close, decimal Volume), Trend?>
    {
        private AccumulationDistributionLine _accumDist;

        public AccumulationDistributionLineTrend(IList<Core.Candle> candles)
            : this(candles.Select(c => (c.High, c.Low, c.Close, c.Volume)).ToList())
        {
        }

        public AccumulationDistributionLineTrend(IList<(decimal High, decimal Low, decimal Close, decimal Volume)> inputs) : base(inputs)
        {
            _accumDist = new AccumulationDistributionLine(inputs);
        }

        protected override Trend? ComputeByIndexImpl(int index)
            => index >= 1 ? StateHelper.IsTrending(_accumDist[index] , _accumDist[index - 1]) : null;
    }
}