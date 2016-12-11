using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOvertrade
    {
        public abstract class PatternBase : PatternBase<MultistateResult<Overtrade>>
        {
            private Stochastics.IndicatorBase _stoIndicator;

            public PatternBase(Equity equity, Stochastics.IndicatorBase stoIndicator) : base(equity)
            {
                _stoIndicator = stoIndicator;
            }

            protected override TickBase ComputeResultByIndex(int index)
            {
                var result = _stoIndicator.ComputeByIndex(index);
                return new MultistateResult<Overtrade>(Equity[index].DateTime, GetOvertrade(result.K));
            }

            private Overtrade GetOvertrade(decimal value)
            {
                if (value <= 20) return Overtrade.Oversold;
                if (value >= 80) return Overtrade.Overbought;
                return Overtrade.Normal;
            }
        }
    }
}
