using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AverageTrueRange : IndicatorBase
    {
        private readonly Func<int, decimal> _tr;
        private Ema _trEma;

        public AverageTrueRange(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _tr = i => i > 0 ? new List<decimal> { Math.Abs(Equity[i].High - Equity[i].Low), Math.Abs(Equity[i].High - Equity[i - 1].Close), Math.Abs(Equity[i].Low - Equity[i - 1].Close) }.Max() : 0;

            _trEma = new Ema(
                i => Equity[i].DateTime,
                i => _tr(i),
                periodCount,
                Enumerable.Range(0, periodCount).Average(i => _tr(i)),
                true);
        }

        protected override TickBase ComputeResultByIndex(int index)
            => new IndicatorResult(Equity[index].DateTime, _trEma.Compute(index));

        public TimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new TimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
