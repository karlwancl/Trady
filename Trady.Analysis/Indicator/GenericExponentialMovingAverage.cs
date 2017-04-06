using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class GenericExponentialMovingAverage<TInput> : CummulativeIndicatorBase<TInput, decimal?>
    {
        private int _initialValueIndex;
        private Func<int, decimal?> _initialValueFunction;
        private Func<int, decimal?> _indexValueFunction;
        private Func<int, decimal> _smoothingFactorFunction;

        public GenericExponentialMovingAverage(IList<TInput> inputs, int initialValueIndex, Func<int, decimal?> initialValueFunction, Func<int, decimal?> indexValueFunction, Func<int, decimal> smoothingFactorFunction)
            : base(inputs)
        {
            _initialValueIndex = initialValueIndex;
            _initialValueFunction = initialValueFunction;
            _indexValueFunction = indexValueFunction;
            _smoothingFactorFunction = smoothingFactorFunction;
        }

        protected override int InitialValueIndex => _initialValueIndex;

        protected override decimal? ComputeNullValue(int index) => null;

        protected override decimal? ComputeInitialValue(int index) => _initialValueFunction(index);

        protected override decimal? ComputeCummulativeValue(int index, decimal? prevOutput)
            => prevOutput + (_smoothingFactorFunction(index) * (_indexValueFunction(index) - prevOutput));
    }
}