using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class DynamicMomentumIndex<TInput, TOutput>: NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public int SdPeriod { get; }
        public int SmoothedSdPeriod { get; }
        public int RsiPeriod { get; }
        public int UpLimit { get; }
        public int LowLimit { get; }

        private IReadOnlyList<decimal?> _v;

        public DynamicMomentumIndex(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int sdPeriod, int smoothedSdPeriod, int rsiPeriod, int upLimit, int lowLimit) : base(inputs, inputMapper)
        {
            LowLimit = lowLimit;
            UpLimit = upLimit;
            RsiPeriod = rsiPeriod;
            SmoothedSdPeriod = smoothedSdPeriod;
            SdPeriod = sdPeriod;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
        {
            _v = _v ?? Enumerable.Range(0, mappedInputs.Count).Select(i =>
            {
                var sd = new StandardDeviationByTuple(mappedInputs, SdPeriod);
                var smoothedSd = new SimpleMovingAverageByTuple(sd.Compute(), SmoothedSdPeriod);
                var currentSmoothedSd = smoothedSd[i];
                return currentSmoothedSd == 0 ? default : sd[i] / currentSmoothedSd;
            }).ToList();

            var currentV = _v[index];
            var t = currentV.GetValueOrDefault() != 0 ? (int?)Math.Floor(RsiPeriod / currentV.Value) : default;
            var tAdjusted = t.HasValue ? (int?)Math.Max(Math.Min(t.Value, UpLimit), LowLimit) : default;
            return tAdjusted.HasValue ? new RelativeStrengthIndexByTuple(mappedInputs, tAdjusted.Value)[index] : default;
        }
    }

    public class DynamicMomentumIndexByTuple : DynamicMomentumIndex<decimal?, decimal?>
    {
        public DynamicMomentumIndexByTuple(IEnumerable<decimal?> inputs, int sdPeriod, int smoothedSdPeriod, int rsiPeriod, int upLimit, int lowLimit) 
            : base(inputs, i => i, sdPeriod, smoothedSdPeriod, rsiPeriod, upLimit, lowLimit)
        {
        }
    }

    public class DynamicMomentumIndex : DynamicMomentumIndex<IOhlcv, AnalyzableTick<decimal?>>
    {
        public DynamicMomentumIndex(IEnumerable<IOhlcv> inputs, int sdPeriod, int smoothedSdPeriod, int rsiPeriod, int upLimit, int lowLimit) 
            : base(inputs, i => i.Close, sdPeriod, smoothedSdPeriod, rsiPeriod, upLimit, lowLimit)
        {
        }
    }
}
