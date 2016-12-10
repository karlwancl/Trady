using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public abstract class IndicatorBase : Indicator.IndicatorBase
        {
            protected IndicatorBase(Equity equity, int periodCount, int smaPeriodCountK, int smaPeriodCountD) 
                : base(equity, periodCount, smaPeriodCountK, smaPeriodCountD)
            {
            }

            public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
                => new IndicatorResultTimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

            public IndicatorResult ComputeByDateTime(DateTime dateTime)
                => ComputeResultByDateTime<IndicatorResult>(dateTime);

            public IndicatorResult ComputeByIndex(int index)
                => ComputeResultByIndex<IndicatorResult>(index);
        }
    }
}
