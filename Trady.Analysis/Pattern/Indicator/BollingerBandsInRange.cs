using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class BollingerBandsInRange : AnalyzableBase<decimal, Overboundary?>
    {
        private BollingerBands _bb;

        public BollingerBandsInRange(IList<Core.Candle> candles, int periodCount, int sdCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount, sdCount)
        {
        }

        public BollingerBandsInRange(IList<decimal> closes, int periodCount, int sdCount)
            : base(closes)
        {
            _bb = new BollingerBands(closes, periodCount, sdCount);
        }

        protected override Overboundary? ComputeByIndexImpl(int index)
        {
            var result = _bb[index];
            return StateHelper.IsOverbound(Inputs[index], result.LowerBand, result.UpperBand);
        }
    }
}