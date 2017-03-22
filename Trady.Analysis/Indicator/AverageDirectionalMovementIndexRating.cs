using Trady.Core;
using static Trady.Analysis.Indicator.AverageDirectionalMovementIndexRating;

namespace Trady.Analysis.Indicator
{
    public partial class AverageDirectionalMovementIndexRating : IndicatorBase<IndicatorResult>
    {
        private DirectionalMovementIndex _dmiIndicator;

        public AverageDirectionalMovementIndexRating(Equity equity, int periodCount, int adxrPeriodCount) : base(equity, periodCount, adxrPeriodCount)
        {
            _dmiIndicator = new DirectionalMovementIndex(equity, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public int AdxrPeriodCount => Parameters[1];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            if (index < PeriodCount && index < AdxrPeriodCount)
                return new IndicatorResult(Equity[index].DateTime, null);

            var dmi = _dmiIndicator.ComputeByIndex(index);
            var priorDmi = _dmiIndicator.ComputeByIndex(index - AdxrPeriodCount);
            var adxr = (dmi.Adx + priorDmi.Adx) / 2;
            return new IndicatorResult(Equity[index].DateTime, adxr);
        }
    }
}