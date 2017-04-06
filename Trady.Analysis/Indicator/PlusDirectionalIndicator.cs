using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class PlusDirectionalIndicator : IndicatorBase<(decimal High, decimal Low, decimal Close), decimal?>
    {
        private PlusDirectionalMovement _pdm;
        private MinusDirectionalMovement _mdm;
        private GenericExponentialMovingAverage<(decimal High, decimal Low, decimal Close)> _tpdmEma;
        private AverageTrueRange _atr;

        public PlusDirectionalIndicator(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => (c.High, c.Low, c.Close)).ToList(), periodCount)
        {
        }

        public PlusDirectionalIndicator(IList<(decimal High, decimal Low, decimal Close)> inputs, int periodCount) : base(inputs, periodCount)
        {
            _pdm = new PlusDirectionalMovement(inputs.Select(i => i.High).ToList());
            _mdm = new MinusDirectionalMovement(inputs.Select(i => i.Low).ToList());

            Func<int, decimal?> tpdm = i => (_pdm[i] > 0 && _pdm[i] > _mdm[i]) ? _pdm[i] : 0;

            _tpdmEma = new GenericExponentialMovingAverage<(decimal High, decimal Low, decimal Close)>(
                inputs,
                periodCount,
                i => Enumerable.Range(i - periodCount + 1, periodCount).Select(j => tpdm(j)).Average(),
                i => tpdm(i),
                i => 1.0m / periodCount);

            _atr = new AverageTrueRange(inputs, periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index)
            => _tpdmEma[index] / _atr[index] * 100;
    }
}