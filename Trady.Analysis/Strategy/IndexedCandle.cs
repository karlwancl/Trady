using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;
using Trady.Core.Infrastructure;
using Trady.Analysis.Strategy.Rule;

namespace Trady.Analysis.Strategy
{
    public class IndexedCandle : Candle, IIndexedObject<Candle>
    {
        public IndexedCandle(IEnumerable<Candle> candles, int index)
            : base(candles.ElementAt(index).DateTime, 
                   candles.ElementAt(index).Open, 
                   candles.ElementAt(index).High, 
                   candles.ElementAt(index).Low, 
                   candles.ElementAt(index).Close, 
                   candles.ElementAt(index).Volume)
        {
            BackingList = candles;
            Index = index;
        }

        public IEnumerable<Candle> BackingList {get;}

        public int Index {get;}

        public IndexedCandle Prev => Index - 1 >= 0 ? new IndexedCandle(BackingList, Index - 1) : null;

        public IndexedCandle Next => Index + 1 < BackingList.Count() ? new IndexedCandle(BackingList, Index + 1) : null;

        public Candle Underlying => BackingList.ElementAt(Index);

        IEnumerable IIndexedObject.BackingList => BackingList;

        IIndexedObject IIndexedObject.Prev => Prev;

        IIndexedObject<Candle> IIndexedObject<Candle>.Prev => Prev;

        IIndexedObject IIndexedObject.Next => Next;

        IIndexedObject<Candle> IIndexedObject<Candle>.Next => Next;

        object IIndexedObject.Underlying => Underlying;

        public TAnalyzable Get<TAnalyzable>(params object[] @params) where TAnalyzable : IAnalyzable
            => BackingList.GetOrCreateAnalyzable<TAnalyzable>(@params);
    }
}