using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class UpTrend : AnalyzableBase<(decimal High, decimal Low), bool?>
    {
        public UpTrend(IList<Candle> inputs, int periodCount = 3) 
            : this(inputs.Select(i => (i.High, i.Low)).ToList(), periodCount)
        {
        }

        public UpTrend(IList<(decimal High, decimal Low)> inputs, int periodCount = 3) : base(inputs)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override bool? ComputeByIndexImpl(int index)
        {
            if (index < PeriodCount - 1)
                return null;

            for (int i = 0; i < PeriodCount; i++)
            {
                bool isHighIncreasing = Inputs[index - i].High > Inputs[index - i - 1].High;
                bool isLowIncreasing = Inputs[index - i].Low > Inputs[index - i - 1].Low;
                if (!isHighIncreasing || !isLowIncreasing)
                    return false;
            }

            return true;
        }
    }
}
