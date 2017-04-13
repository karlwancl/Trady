using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class OnBalanceVolumeTrend : AnalyzableBase<(decimal Close, decimal Volume), Trend?>
    {
        private OnBalanceVolume _obv;

        public OnBalanceVolumeTrend(IList<Core.Candle> candles)
            : this(candles.Select(c => (c.Close, c.Volume)).ToList())
        {
        }

        public OnBalanceVolumeTrend(IList<(decimal Close, decimal Volume)> inputs) : base(inputs)
        {
            _obv = new OnBalanceVolume(inputs);
        }

        protected override Trend? ComputeByIndexImpl(int index)
            => index >= 1 ? StateHelper.IsTrending(_obv[index] , _obv[index - 1]) : null;
    }
}