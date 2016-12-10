using System;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AverageDirectionalIndex : IndicatorBase
    {
        private const string AdxTag = "Adx";
        private AverageTrueRange _atrIndicator;
        private Ema _pdmEma, _mdmEma, _adx;
        private Func<int, decimal> _pdi, _mdi;

        public AverageDirectionalIndex(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _atrIndicator = new AverageTrueRange(equity, periodCount);
            _pdmEma = new Ema(i => Equity[i].DateTime, i => i > 0 ? Math.Max(Equity[i].High - Equity[i - 1].High, 0) : 0, periodCount);
            _mdmEma = new Ema(i => Equity[i].DateTime, i => i > 0 ? Math.Max(Equity[i - 1].Low - Equity[i].Low, 0) : 0, periodCount);
            _pdi = i => i >= periodCount - 1 ? _pdmEma.Compute(i) / _atrIndicator.ComputeByIndex(i).Atr * 100 : 0;
            _mdi = i => i >= periodCount - 1 ?  _mdmEma.Compute(i) / _atrIndicator.ComputeByIndex(i).Atr * 100 : 0;
            _adx = new Ema(
                i => Equity[i].DateTime,
                i => i >= periodCount - 1 ? Math.Abs((_pdi(i) - _mdi(i)) / (_pdi(i) + _mdi(i))) * 100 : 0,
                periodCount,
                null,
                true);
        }

        public int PeriodCount => Parameters[0];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
            => new IndicatorResult(Equity[index].DateTime, _adx.Compute(index));

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
