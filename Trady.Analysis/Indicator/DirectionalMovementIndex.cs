using System;
using System.Linq;
using Trady.Core;
using static Trady.Analysis.Indicator.DirectionalMovementIndex;

namespace Trady.Analysis.Indicator
{
    public partial class DirectionalMovementIndex : IndicatorBase<IndicatorResult>
    {
        private AverageTrueRange _atrIndicator;

        private GenericExponentialMovingAverage _adx;
        private Func<int, decimal?> _pdi, _mdi, _dx;

        public DirectionalMovementIndex(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _atrIndicator = new AverageTrueRange(equity, periodCount);

            Func<int, decimal?> pdm = i => i > 0 ? Equity[i].High - Equity[i - 1].High : (decimal?)null;
            Func<int, decimal?> mdm = i => i > 0 ? Equity[i - 1].Low - Equity[i].Low : (decimal?)null;

            Func<int, decimal?> tpdm = i => pdm(i) > 0 && pdm(i) > mdm(i) ? pdm(i) : 0;
            Func<int, decimal?> tmdm = i => mdm(i) > 0 && pdm(i) < mdm(i) ? mdm(i) : 0;

            var tpdmEma = new GenericExponentialMovingAverage(
                equity,
                periodCount,
                i => Enumerable.Range(i - periodCount + 1, periodCount).Select(j => tpdm(j)).Average(),
                i => tpdm(i),
                i => 1.0m / periodCount);

            var tmdmEma = new GenericExponentialMovingAverage(
                equity,
                periodCount,
                i => Enumerable.Range(i - periodCount + 1, periodCount).Select(j => tmdm(j)).Average(),
                i => tmdm(i),
                i => 1.0m / periodCount);

            _pdi = i => tpdmEma.ComputeByIndex(i).Ema / _atrIndicator.ComputeByIndex(i).Atr * 100;
            _mdi = i => tmdmEma.ComputeByIndex(i).Ema / _atrIndicator.ComputeByIndex(i).Atr * 100;
            _dx = i =>
            {
                var value = (_pdi(i) - _mdi(i)) / (_pdi(i) + _mdi(i));
                return value.HasValue ? Math.Abs(value.Value) * 100 : (decimal?)null;
            };

            _adx = new GenericExponentialMovingAverage(
                equity,
                periodCount,
                i => Enumerable.Range(i - periodCount + 1, periodCount).Select(j => _dx(j)).Average(),
                i => _dx(i),
                i => 1.0m / periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            var adx = _adx.ComputeByIndex(index).Ema;
            return new IndicatorResult(Equity[index].DateTime, _pdi(index), _mdi(index), adx);
        }
    }
}