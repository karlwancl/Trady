using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Strategy
{
    public class IndexedCandle : Candle
    {
        private IList<Candle> _candles;
        private int _index;

        public IndexedCandle(IList<Candle> candles, int index)
            : base(candles[index].DateTime, candles[index].Open, candles[index].High, candles[index].Low, candles[index].Close, candles[index].Volume)
        {
            _candles = candles;
            _index = index;
        }

        public IList<Candle> Candles => _candles;

        public int Index => _index;

        public IndexedCandle Prev => _index - 1 >= 0 ? new IndexedCandle(_candles, _index - 1) : null;

        public IndexedCandle Next => _index + 1 < _candles.Count ? new IndexedCandle(_candles, _index + 1) : null;

        public TAnalyzable Get<TAnalyzable>(params int[] @params) where TAnalyzable : IAnalyzable
            => _candles.GetOrCreateAnalyzable<TAnalyzable>(@params);
    }
}