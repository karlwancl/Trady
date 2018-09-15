using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class KeltnerChannels<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), (decimal? LowerChannel, decimal? Middle, decimal? UpperChannel), TOutput>
    {
        private readonly ExponentialMovingAverageByTuple _ema;
        private readonly AverageTrueRangeByTuple _atr;

        public KeltnerChannels(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount, decimal sdCount, int atrPeriodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            SdCount = sdCount;
            AtrPeriodCount = atrPeriodCount;
            _ema = new ExponentialMovingAverageByTuple(inputs.Select(inputMapper).Select(i => i.Close).ToList(), periodCount);
            _atr = new AverageTrueRangeByTuple(inputs.Select(inputMapper).ToList(), atrPeriodCount);
        }

        public int PeriodCount { get; }
        public decimal SdCount { get; }
        public int AtrPeriodCount { get; }

        protected override (decimal? LowerChannel, decimal? Middle, decimal? UpperChannel) ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index < Math.Max(PeriodCount, AtrPeriodCount) - 1)
                return default;

            var ema = _ema[index];
            var atr = _atr[index];
            return (ema - SdCount * atr, ema, ema + SdCount * atr);
        }
    }

    public class KeltnerChannelsByTuple : KeltnerChannels<(decimal High, decimal Low, decimal Close), (decimal? LowerChannel, decimal? Middle, decimal? UpperChannel)>
    {
        public KeltnerChannelsByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, decimal sdCount, int atrPeriodCount)
            : base(inputs, i => i, periodCount, sdCount, atrPeriodCount)
        {
        }
    }

    public class KeltnerChannels : KeltnerChannels<IOhlcv, AnalyzableTick<(decimal? LowerChannel, decimal? Middle, decimal? UpperChannel)>>
    {
        public KeltnerChannels(IEnumerable<IOhlcv> inputs, int periodCount, decimal sdCount, int atrPeriodCount)
            : base(inputs, i => (i.High, i.Low, i.Close), periodCount, sdCount, atrPeriodCount)
        {
        }
    }
}
