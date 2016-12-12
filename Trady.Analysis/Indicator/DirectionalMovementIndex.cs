using System;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;
using static Trady.Analysis.Indicator.DirectionalMovementIndex;

namespace Trady.Analysis.Indicator
{
    public partial class DirectionalMovementIndex : IndicatorBase<IndicatorResult>
    {
        private AverageTrueRange _atrIndicator;

        private Ema _pdmEma, _mdmEma, _adx;
        private Func<int, decimal?> _pdi, _mdi, _pdm, _mdm, _dx;

        public DirectionalMovementIndex(Equity equity, int periodCount, int adxrPeriodCount) : base(equity, periodCount, adxrPeriodCount)
        {
            _atrIndicator = new AverageTrueRange(equity, periodCount);

            _pdm = i => i > 0 ? Math.Max(Equity[i].High - Equity[i - 1].High, 0) : (decimal?)null;
            _pdmEma = new Ema(
                i => Equity[i].DateTime,
                i => _pdm(i),
                i => Enumerable.Range(i - periodCount + 1, periodCount).Select(j => _pdm(j)).Average(),
                periodCount,
                periodCount,
                true);

            _mdm = i => i > 0 ? Math.Max(Equity[i - 1].Low - Equity[i].Low, 0) : (decimal?)null;
            _mdmEma = new Ema(
                i => Equity[i].DateTime,
                i => _mdm(i),
                i => Enumerable.Range(i - periodCount + 1, periodCount).Select(j => _mdm(j)).Average(),
                periodCount,
                periodCount,
                true);

            _pdi = i => _pdmEma.Compute(i) / _atrIndicator.ComputeByIndex(i).Atr * 100;
            _mdi = i =>  _mdmEma.Compute(i) / _atrIndicator.ComputeByIndex(i).Atr * 100;
            _dx = i => ((_pdi(i) - _mdi(i)) / (_pdi(i) + _mdi(i))).Abs() * 100;
            _adx = new Ema(
                i => Equity[i].DateTime,
                i => _dx(i),
                i => Enumerable.Range(i - periodCount + 1, periodCount).Select(j => _dx(j)).Average(),
                periodCount,
                periodCount,
                true);
        }

        public int PeriodCount => Parameters[0];

        public int AdxrPeriodCount => Parameters[1];

        public override IndicatorResult ComputeByIndex(int index)
        {
            var adx = _adx.Compute(index);
            var adxr = index >= AdxrPeriodCount ? (adx + _adx.Compute(index - AdxrPeriodCount)) / 2 : null;
            return new IndicatorResult(Equity[index].DateTime, _pdi(index), _mdi(index), adx, adxr);
        }
    }
}
