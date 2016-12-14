using System;
using Trady.Core;
using static Trady.Analysis.Indicator.GenericExponentialMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class GenericExponentialMovingAverage: CacheIndicatorBase<IndicatorResult>
    {
        private int _firstValueIndex;
        private Func<int, decimal?> _firstValueFunction;
        private Func<int, decimal?> _indexValueFunction;
        private bool _modified;

        public GenericExponentialMovingAverage(Equity equity, int firstValueIndex, Func<int, decimal?> firstValueFunction, Func<int, decimal?> indexValueFunction, int periodCount, bool modified = false) 
            : base(equity, periodCount)
        {
            _firstValueIndex = firstValueIndex;
            _firstValueFunction = firstValueFunction;
            _indexValueFunction = indexValueFunction;
            _modified = modified;
        }

        private int PeriodCount => Parameters[0];

        public decimal SmoothingFactor => _modified ? 1.0m / PeriodCount : 2.0m / (PeriodCount + 1);

        protected override int FirstValueIndex => _firstValueIndex;

        protected override IndicatorResult ComputeNullValue(int index)
            => new IndicatorResult(Equity[index].DateTime, null);

        protected override IndicatorResult ComputeFirstValue(int index)
            => new IndicatorResult(Equity[index].DateTime, _firstValueFunction(index));

        protected override IndicatorResult ComputeIndexValue(int index, IndicatorResult prevTick)
            => new IndicatorResult(Equity[index].DateTime, prevTick.Ema + (SmoothingFactor * (_indexValueFunction(index) - prevTick.Ema)));

    }
}
