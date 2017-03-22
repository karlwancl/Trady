using System;
using Trady.Core;
using static Trady.Analysis.Indicator.GenericExponentialMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class GenericExponentialMovingAverage : CummulativeIndicatorBase<IndicatorResult>
    {
        private int _firstValueIndex;
        private Func<int, decimal?> _firstValueFunction;
        private Func<int, decimal?> _indexValueFunction;
        private Func<int, decimal> _smoothingFactorFunction;

        public GenericExponentialMovingAverage(Equity equity, int firstValueIndex, Func<int, decimal?> firstValueFunction, Func<int, decimal?> indexValueFunction, Func<int, decimal> smoothingFactorFunction)
            : base(equity)
        {
            _firstValueIndex = firstValueIndex;
            _firstValueFunction = firstValueFunction;
            _indexValueFunction = indexValueFunction;
            _smoothingFactorFunction = smoothingFactorFunction;
        }

        protected override int FirstValueIndex => _firstValueIndex;

        protected override IndicatorResult ComputeNullValue(int index)
            => new IndicatorResult(Equity[index].DateTime, null);

        protected override IndicatorResult ComputeFirstValue(int index)
            => new IndicatorResult(Equity[index].DateTime, _firstValueFunction(index));

        protected override IndicatorResult ComputeIndexValue(int index, IndicatorResult prevTick)
            => new IndicatorResult(Equity[index].DateTime, prevTick.Ema + (_smoothingFactorFunction(index) * (_indexValueFunction(index) - prevTick.Ema)));
    }
}