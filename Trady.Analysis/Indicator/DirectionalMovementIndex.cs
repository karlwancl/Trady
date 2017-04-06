using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class DirectionalMovementIndex : IndicatorBase<(decimal High, decimal Low, decimal Close), decimal?>
    {
        private PlusDirectionalIndicator _pdi;
        private MinusDirectionalIndicator _mdi;

        public DirectionalMovementIndex(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount)
        {
        }

        public DirectionalMovementIndex(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount) : base(inputs, periodCount)
        {
            _pdi = new PlusDirectionalIndicator(inputs, periodCount);
            _mdi = new MinusDirectionalIndicator(inputs, periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index)
        {
            var value = (_pdi[index] - _mdi[index]) / (_pdi[index] + _mdi[index]);
            return value.HasValue ? Math.Abs(value.Value) * 100 : (decimal?)null;
        }
    }
}