using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class BollingerBandWidth<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        private BollingerBandsByTuple _bb;

        public BollingerBandWidth(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount, decimal sdCount)
            : base(inputs, inputMapper)
        {
            _bb = new BollingerBandsByTuple(inputs.Select(inputMapper), periodCount, sdCount);

            PeriodCount = periodCount;
            SdCount = sdCount;
        }

        public int PeriodCount { get; }

        public decimal SdCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
        {
            var (LowerBand, MiddleBand, UpperBand) = _bb[index];
            return MiddleBand == 0 ? default : (UpperBand - LowerBand) / MiddleBand * 100;
        }
    }

    public class BollingerBandWidthByTuple : BollingerBandWidth<decimal, decimal?>
    {
        public BollingerBandWidthByTuple(IEnumerable<decimal> inputs, int periodCount, decimal sdCount)
            : base(inputs, i => i, periodCount, sdCount)
        {
        }
    }

    public class BollingerBandWidth : BollingerBandWidth<IOhlcv, AnalyzableTick<decimal?>>
    {
        public BollingerBandWidth(IEnumerable<IOhlcv> inputs, int periodCount, decimal sdCount)
            : base(inputs, i => i.Close, periodCount, sdCount)
        {
        }
    }
}
