using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Trady.Analysis.Candlestick;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class ParabolicStopAndReverse<TInput, TOutput> : CumulativeAnalyzableBase<TInput, (decimal High, decimal Low), decimal?, TOutput>
    {
        const int LongShortDeterminationPeriod = 4;

        public decimal Step { get; }
        public decimal MaximumStep { get; }

        bool _isUptrend;
        decimal _extremePoint;
        decimal _acceleratingFactor;

        public ParabolicStopAndReverse(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low)> inputMapper, decimal step = 0.02m, decimal maximumStep = 0.2m) : base(inputs, inputMapper)
        {
            if (step > maximumStep)
                throw new ArgumentException($"{nameof(step)} must be smaller than {nameof(maximumStep)}");

            MaximumStep = maximumStep;
            Step = step;
        }

        protected override decimal? ComputeCumulativeValue(IReadOnlyList<(decimal High, decimal Low)> mappedInputs, int index, decimal? prevOutputToMap)
            => _isUptrend ? ComputeUptrend(mappedInputs, index, prevOutputToMap) : ComputeDowntrend(mappedInputs, index, prevOutputToMap);

        decimal? ComputeDowntrend(IReadOnlyList<(decimal High, decimal Low)> mappedInputs, int index, decimal? prevOutputToMap)
        {
            decimal? sar;
            var isTrendUnchanged = mappedInputs[index - 1].High < prevOutputToMap;
            if (isTrendUnchanged)
            {
                // Current downtrend
                sar = prevOutputToMap + _acceleratingFactor * (_extremePoint - prevOutputToMap);
                sar = new decimal[] { sar.Value, mappedInputs[index - 1].High, mappedInputs[index - 2].High }.Max();

                var isNewExtreme = mappedInputs[index].Low < _extremePoint;
                _extremePoint = Math.Min(_extremePoint, mappedInputs[index].Low);
                if (mappedInputs[index].High < sar && isNewExtreme)
                    _acceleratingFactor += Math.Min(Step, MaximumStep - _acceleratingFactor);
            }
            else
            {
                // Current uptrend
                sar = _extremePoint;
                _extremePoint = Math.Max(_extremePoint, mappedInputs[index].High);
                _acceleratingFactor = Step;
            }

            _isUptrend = !isTrendUnchanged;
            return sar;
        }

        decimal? ComputeUptrend(IReadOnlyList<(decimal High, decimal Low)> mappedInputs, int index, decimal? prevOutputToMap)
        {
            decimal? sar;
            var isTrendUnchanged = mappedInputs[index - 1].Low > prevOutputToMap;
            if (isTrendUnchanged)
            {
                // Current uptrend
                sar = prevOutputToMap + _acceleratingFactor * (_extremePoint - prevOutputToMap);
                sar = new decimal[] { sar.Value, mappedInputs[index - 1].Low, mappedInputs[index - 2].Low }.Min();

                var isNewExtreme = mappedInputs[index].High > _extremePoint;
                _extremePoint = Math.Max(_extremePoint, mappedInputs[index].High);
                if (mappedInputs[index].Low > sar && isNewExtreme)
                    _acceleratingFactor += Math.Min(Step, MaximumStep - _acceleratingFactor);
            }
            else
            {
                // Current downtrend
                sar = _extremePoint;
                _extremePoint = Math.Min(_extremePoint, mappedInputs[index].Low);
                _acceleratingFactor = Step;
            }

            _isUptrend = isTrendUnchanged;
            return sar;
        }

        protected override decimal? ComputeInitialValue(IReadOnlyList<(decimal High, decimal Low)> mappedInputs, int index)
        {
            decimal sar;

            var subsetInputs = mappedInputs.Take(index + 1).ToList().AsReadOnly();
            _isUptrend = IsUptrend(subsetInputs, index);
            if (_isUptrend)
            {
                sar = subsetInputs.Min(i => i.Low);
                _extremePoint = subsetInputs.Max(i => i.High);
            }
            else
            {
                sar = subsetInputs.Max(i => i.High);
                _extremePoint = subsetInputs.Min(i => i.Low);
            }

            _acceleratingFactor = Step;
            return sar;
        }

        static bool IsUptrend(ReadOnlyCollection<(decimal High, decimal Low)> inputs, int index)
        {
            const int Strictness = 1;

            var isUptrend = new UpTrendByTuple(inputs, Strictness)[index].GetValueOrDefault(false);
            if (isUptrend)
                return true;

            var isDowntrend = new DownTrendByTuple(inputs, Strictness)[index].GetValueOrDefault(false);
            if (isDowntrend)
                return false;

            decimal highLowMean(int i) => (inputs[i].High + inputs[i].Low) / 2;
            return highLowMean(index) > highLowMean(index - 1);
        }

        protected override int InitialValueIndex => LongShortDeterminationPeriod;
    }

    public class ParabolicStopAndReverseByTuple : ParabolicStopAndReverse<(decimal High, decimal Low), decimal?>
    {
        public ParabolicStopAndReverseByTuple(IEnumerable<(decimal High, decimal Low)> inputs, decimal step = 0.02M, decimal maximumStep = 0.2M) 
            : base(inputs, i => i, step, maximumStep)
        {
        }
    }

    public class ParabolicStopAndReverse : ParabolicStopAndReverse<IOhlcv, AnalyzableTick<decimal?>>
    {
        public ParabolicStopAndReverse(IEnumerable<IOhlcv> inputs, decimal step = 0.02M, decimal maximumStep = 0.2M) 
            : base(inputs, i => (i.High, i.Low), step, maximumStep)
        {
        }
    }
}
