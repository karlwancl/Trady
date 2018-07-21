using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class BollingerBands<TInput, TOutput> : AnalyzableBase<TInput, decimal, (decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand), TOutput>
    {
        private readonly SimpleMovingAverageByTuple _sma;
        private readonly StandardDeviationByTuple _sd;

        public BollingerBands(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount, decimal sdCount)
            : base(inputs, inputMapper)
        {
            _sma = new SimpleMovingAverageByTuple(inputs.Select(inputMapper), periodCount);
            _sd = new StandardDeviationByTuple(inputs.Select(inputMapper), periodCount);

            PeriodCount = periodCount;
            SdCount = sdCount;
        }

        public int PeriodCount { get; }

        public decimal SdCount { get; }

        protected override (decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand) ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
        {
            var middleBand = _sma[index];
            var sd = _sd[index];
            return (middleBand - SdCount * sd, middleBand, middleBand + SdCount * sd);
        }
    }

    public class BollingerBandsByTuple : BollingerBands<decimal, (decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)>
    {
        public BollingerBandsByTuple(IEnumerable<decimal> inputs, int periodCount, decimal sdCount)
            : base(inputs, i => i, periodCount, sdCount)
        {
        }
    }

    public class BollingerBands : BollingerBands<IOhlcv, AnalyzableTick<(decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)>>
    {
        public BollingerBands(IEnumerable<IOhlcv> inputs, int periodCount, decimal sdCount)
            : base(inputs, i => i.Close, periodCount, sdCount)
        {
        }
    }
}
