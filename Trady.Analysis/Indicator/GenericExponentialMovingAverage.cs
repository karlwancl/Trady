using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
    class GenericExponentialMovingAverage: CumulativeAnalyzableBase<decimal?, decimal?>
    {
        readonly int _initialValueIndex;
        readonly Func<int, decimal?> _inputFunction;
        readonly Func<int, decimal> _smoothingFactorFunction;

        // TODO: Hacking with empty array, may improve the implementation later
        public GenericExponentialMovingAverage(int initialValueIndex, Func<int, decimal?> initialValueFunction, Func<int, decimal?> indexValueFunction, Func<int, decimal> smoothingFactorFunction, int inputCount)
            : base(new decimal?[inputCount])
        {
            _initialValueIndex = initialValueIndex;
            _inputFunction = i => i < initialValueIndex ? null : initialValueIndex == i ? initialValueFunction(i) : indexValueFunction(i);
            _smoothingFactorFunction = smoothingFactorFunction;
        }

        protected override int InitialValueIndex => _initialValueIndex;

        protected override decimal? ComputeInitialValue(IEnumerable<decimal?> mappedInputs, int index) => _inputFunction(index);

        protected override decimal? ComputeCumulativeValue(IEnumerable<decimal?> mappedInputs, int index, decimal? prevOutputToMap)
        {
            var smooth = _smoothingFactorFunction(index);
            var input = _inputFunction(index);
            var result = prevOutputToMap + (smooth * (input - prevOutputToMap));
            return result;
        }
            //=> prevOutputToMap + (_smoothingFactorFunction(index) * (_inputFunction(index) - prevOutputToMap));
	}
}