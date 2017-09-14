using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
    internal class GenericExponentialMovingAverage : CumulativeAnalyzableBase<decimal?, decimal?>
    {
        private readonly int _initialValueIndex;
        private readonly Func<int, decimal?> _inputFunction;
        private readonly Func<int, decimal> _smoothingFactorFunction;

        // TODO: Hacking with empty array, may improve the implementation later
        public GenericExponentialMovingAverage(int initialValueIndex, Func<int, decimal?> initialValueFunction, Func<int, decimal?> indexValueFunction, Func<int, decimal> smoothingFactorFunction, int inputCount)
            : base(new decimal?[inputCount])
        {
            _initialValueIndex = initialValueIndex;
            _inputFunction = i => i < initialValueIndex ? null : initialValueIndex == i ? initialValueFunction(i) : indexValueFunction(i);
            _smoothingFactorFunction = smoothingFactorFunction;
        }

        protected override int InitialValueIndex => _initialValueIndex;

        protected override decimal? ComputeInitialValue(IReadOnlyList<decimal?> mappedInputs, int index) => _inputFunction(index);

        protected override decimal? ComputeCumulativeValue(IReadOnlyList<decimal?> mappedInputs, int index, decimal? prevOutputToMap)
            => prevOutputToMap + (_smoothingFactorFunction(index) * (_inputFunction(index) - prevOutputToMap));
    }
}