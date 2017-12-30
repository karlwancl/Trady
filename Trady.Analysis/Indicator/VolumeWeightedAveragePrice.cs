using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    //http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:vwap_intraday
    public class VolumeWeightedAveragePrice<TInput, TOutput>
        : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close, decimal Volume), TOutput>
    {
        private readonly int? _period;
        protected VolumeWeightedAveragePrice(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close, decimal Volume)> inputMapper, int? period = null) : base(inputs, inputMapper)
        {
            _period = period;
        }
        
        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close, decimal Volume)> mappedInputs, int index)
        {
            IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> subset;
            if (_period.HasValue)
            {
                if (index + 1 < _period.Value)
                    return null;

                subset = mappedInputs.Skip(index + 1 - _period.Value).Take(_period.Value);
            }
            else
                subset = mappedInputs.Take(index + 1);

            decimal typicalPrice = subset.Sum(mi => (mi.High + mi.Low + mi.Close) / 3 * mi.Volume);
            decimal totalVolume = subset.Sum(mi => mi.Volume);

            return typicalPrice / totalVolume;
        }
    }

    public class VolumeWeightedAveragePriceByTuple : VolumeWeightedAveragePrice<(decimal High, decimal Low, decimal Close, decimal Volume), decimal?>
    {
        public VolumeWeightedAveragePriceByTuple(IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> inputs, int? period = null)
            : base(inputs, i => i, period)
        {
        }
    }

    public class VolumeWeightedAveragePrice : VolumeWeightedAveragePrice<IOhlcv, AnalyzableTick<decimal?>>
    {
        public VolumeWeightedAveragePrice(IEnumerable<IOhlcv> inputs, int? period = null)
            : base(inputs, c => (c.High, c.Low, c.Close, c.Volume), period)
        {
        }
    }
}