using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class AverageDirectionalIndexRating<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), TOutput>
    {
        private readonly AverageDirectionalIndexByTuple _adx;

        public AverageDirectionalIndexRating(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount, int adxrPeriodCount)
            : base(inputs, inputMapper)
        {
            _adx = new AverageDirectionalIndexByTuple(inputs.Select(inputMapper), periodCount);

            PeriodCount = periodCount;
            AdxrPeriodCount = adxrPeriodCount;
        }

        public int PeriodCount { get; }

        public int AdxrPeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
            => index >= AdxrPeriodCount ? (_adx[index] + _adx[index - AdxrPeriodCount]) / 2 : default;
    }

    public class AverageDirectionalIndexRatingByTuple : AverageDirectionalIndexRating<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public AverageDirectionalIndexRatingByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int adxrPeriodCount)
            : base(inputs, i => i, periodCount, adxrPeriodCount)
        {
        }
    }

    public class AverageDirectionalIndexRating : AverageDirectionalIndexRating<IOhlcv, AnalyzableTick<decimal?>>
    {
        public AverageDirectionalIndexRating(IEnumerable<IOhlcv> inputs, int periodCount, int adxrPeriodCount)
            : base(inputs, i => (i.High, i.Low, i.Close), periodCount, adxrPeriodCount)
        {
        }
    }
}
