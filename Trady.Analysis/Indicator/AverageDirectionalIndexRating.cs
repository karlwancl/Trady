using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class AverageDirectionalIndexRating<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), decimal?, TOutput>
    {
        readonly AverageDirectionalIndexByTuple _adx;

        public AverageDirectionalIndexRating(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount, int adxrPeriodCount) : base(inputs, inputMapper, outputMapper)
        {
			_adx = new AverageDirectionalIndexByTuple(inputs.Select(inputMapper), periodCount);

			PeriodCount = periodCount;
			AdxrPeriodCount = adxrPeriodCount;
        }

        public int PeriodCount { get; private set; }

        public int AdxrPeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
			=> index >= AdxrPeriodCount ? (_adx[index] + _adx[index - AdxrPeriodCount]) / 2 : null;
	}

    public class AverageDirectionalIndexRatingByTuple : AverageDirectionalIndexRating<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public AverageDirectionalIndexRatingByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int adxrPeriodCount)
            : base(inputs, i => i, (i, otm) => otm, periodCount, adxrPeriodCount)
        {
        }
    }

    public class AverageDirectionalIndexRating : AverageDirectionalIndexRating<Candle, AnalyzableTick<decimal?>>
    {
        public AverageDirectionalIndexRating(IEnumerable<Candle> inputs, int periodCount, int adxrPeriodCount)
            : base(inputs, i => (i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount, adxrPeriodCount)
        {
        }
    }
}