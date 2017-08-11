using System.Collections.Generic;
using System.Linq;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Strategy
{
    public class IndexedCandle : Candle
    {
        readonly IEnumerable<Candle> _candles;
        int _index;

        public IndexedCandle(IEnumerable<Candle> candles, int index)
            : base(
                candles.ElementAt(index).DateTime, 
                candles.ElementAt(index).Open, 
                candles.ElementAt(index).High, 
                candles.ElementAt(index).Low, 
                candles.ElementAt(index).Close, 
                candles.ElementAt(index).Volume)
        {
            _candles = candles;
            _index = index;
        }

        public IEnumerable<Candle> Candles => _candles;

        public int Index => _index;

        public Candle Candle => _candles.ElementAt(_index);

        public IndexedCandle Prev => _index - 1 >= 0 ? new IndexedCandle(_candles, _index - 1) : null;

        public IndexedCandle Next => _index + 1 < _candles.Count() ? new IndexedCandle(_candles, _index + 1) : null;

        public TAnalyzable Get<TAnalyzable>(params object[] @params) where TAnalyzable : IAnalyzable
            => _candles.GetOrCreateAnalyzable<TAnalyzable>(@params);
    }
}