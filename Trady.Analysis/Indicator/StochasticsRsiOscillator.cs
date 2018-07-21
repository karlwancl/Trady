using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class StochasticsRsiOscillator<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        private RelativeStrengthIndexByTuple _rsi;
        private Func<int, decimal?> _rsiLow, _rsiHigh;

        public int PeriodCount { get; }

        public StochasticsRsiOscillator(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _rsi = new RelativeStrengthIndexByTuple(inputs.Select(inputMapper), periodCount);

            _rsiLow = i => Enumerable.Range(i - periodCount + 1, periodCount).Min(j => _rsi[j]);
            _rsiHigh = i => Enumerable.Range(i - periodCount + 1, periodCount).Max(j => _rsi[j]);

            PeriodCount = periodCount;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
        {
            if (index < PeriodCount - 1)
                return default;

            var rsiHigh = _rsiHigh(index);
            var rsiLow = _rsiLow(index);

            return rsiHigh == rsiLow ? 0.5m : (_rsi[index] - rsiLow) / (rsiHigh - rsiLow);
        }
    }

    public class StochasticsRsiOscillatorByTuple : StochasticsRsiOscillator<decimal?, decimal?>
    {
        public StochasticsRsiOscillatorByTuple(IEnumerable<decimal?> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class StochasticsRsiOscillator : StochasticsRsiOscillator<IOhlcv, AnalyzableTick<decimal?>>
    {
        public StochasticsRsiOscillator(IEnumerable<IOhlcv> inputs, int periodCount) 
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}
