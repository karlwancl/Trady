using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class BollingerBandWidth<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        BollingerBandsByTuple _bb;

        public BollingerBandWidth(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, Func<TInput, decimal?, TOutput> outputMapper, int periodCount, decimal sdCount) : base(inputs, inputMapper, outputMapper)
        {
			_bb = new BollingerBandsByTuple(inputs.Select(inputMapper), periodCount, sdCount);

			PeriodCount = periodCount;
			SdCount = sdCount;
        }

        public int PeriodCount { get; }

        public decimal SdCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
        {
			var bb = _bb[index];
			return (bb.UpperBand - bb.LowerBand) / bb.MiddleBand * 100;
        }
    }

    public class BollingerBandWidthByTuple : BollingerBandWidth<decimal, decimal?>
    {
        public BollingerBandWidthByTuple(IEnumerable<decimal> inputs, int periodCount, decimal sdCount) 
            : base(inputs, i => i, (i, otm) => otm, periodCount, sdCount)
        {
        }
    }

    public class BollingerBandWidth : BollingerBandWidth<Candle, AnalyzableTick<decimal?>>
    {
        public BollingerBandWidth(IEnumerable<Candle> inputs, int periodCount, decimal sdCount) 
            : base(inputs, i => i.Close, (i, otm) => new AnalyzableTick<decimal?>(i.DateTime, otm), periodCount, sdCount)
        {
        }
    }
}