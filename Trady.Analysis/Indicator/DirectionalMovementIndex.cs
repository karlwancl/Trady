using System;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class DirectionalMovementIndex : IndicatorBase
    {
        private AverageTrueRange _atrIndicator;

        private Ema _pdmEma, _mdmEma, _adx;
        private Func<int, decimal> _pdi, _mdi, _pdm, _mdm, _dx;

        public DirectionalMovementIndex(Equity equity, int periodCount, int adxrPeriodCount) : base(equity, periodCount, adxrPeriodCount)
        {
            _atrIndicator = new AverageTrueRange(equity, periodCount);

            _pdm = i => i > 0 ? Math.Max(Equity[i].High - Equity[i - 1].High, 0) : 0;
            _pdmEma = new Ema(
                i => Equity[i].DateTime,
                i => _pdm(i),
                periodCount,
                Enumerable.Range(0, periodCount).Select(i => _pdm(i)).Average(),
                true);

            _mdm = i => i > 0 ? Math.Max(Equity[i - 1].Low - Equity[i].Low, 0) : 0;
            _mdmEma = new Ema(
                i => Equity[i].DateTime,
                i => _mdm(i),
                periodCount,
                Enumerable.Range(0, periodCount).Select(i => _mdm(i)).Average(),
                true);

            _pdi = i => i >= periodCount - 1 ? _pdmEma.Compute(i) / _atrIndicator.ComputeByIndex(i).Atr * 100 : 0;
            _mdi = i => i >= periodCount - 1 ?  _mdmEma.Compute(i) / _atrIndicator.ComputeByIndex(i).Atr * 100 : 0;
            _dx = i => i >= PeriodCount - 1 ? Math.Abs((_pdi(i) - _mdi(i)) / (_pdi(i) + _mdi(i))) * 100 : 0;
            _adx = new Ema(
                i => Equity[i].DateTime,
                i => _dx(i),
                periodCount,
                Enumerable.Range(0, periodCount).Select(i => _dx(i)).Average(),
                true);
        }

        public int PeriodCount => Parameters[0];

        public int AdxrPeriodCount => Parameters[1];

        protected override TickBase ComputeResultByIndex(int index)
        {
            decimal adx = _adx.Compute(index);
            decimal adxr = index >= AdxrPeriodCount ? (adx + _adx.Compute(index - AdxrPeriodCount)) / 2 : 0;
            return new IndicatorResult(Equity[index].DateTime, _pdi(index), _mdi(index), adx, adxr);
        }

        public TimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new TimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
