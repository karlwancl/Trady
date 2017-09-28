using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;
using System;

namespace Trady.Analysis
{
    public class IndexedCandle : Candle, IIndexedObject<Candle>
    {
        private IAnalyzeContext _context;

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

        public IEnumerable<Candle> BackingList { get; }

        public int Index { get; }

        public IndexedCandle Prev => Index - 1 >= 0 ? new IndexedCandle(BackingList, Index - 1) : null;

        public IndexedCandle Next => Index + 1 < BackingList.Count() ? new IndexedCandle(BackingList, Index + 1) : null;

        public Candle Underlying => BackingList.ElementAt(Index);

        public IAnalyzeContext<Candle> Context
        {
            get => (IAnalyzeContext<Candle>)_context;
            set {
                _context = value;
            }
        }

        IEnumerable IIndexedObject.BackingList => BackingList;

        IIndexedObject IIndexedObject.Prev => Prev;

        IIndexedObject<Candle> IIndexedObject<Candle>.Prev => Prev;

        IIndexedObject IIndexedObject.Next => Next;

        IIndexedObject<Candle> IIndexedObject<Candle>.Next => Next;

        object IIndexedObject.Underlying => Underlying;

        IAnalyzeContext IIndexedObject.Context
        {
            get => _context;
            set {
                _context = value;
            }
        }

        public TAnalyzable Get<TAnalyzable>(params object[] @params) where TAnalyzable : IAnalyzable
        {
            if (Context == null)
                return AnalyzableFactory.CreateAnalyzable<TAnalyzable>(BackingList, @params);
            return Context.Get<TAnalyzable>(@params);
        }

        public IFuncAnalyzable<AnalyzableTick<decimal?>> GetFunc(string name, params decimal[] @params)
        {
            if (Context == null)
                return FuncAnalyzableFactory.CreateAnalyzable(name, BackingList, @params);
            return (IFuncAnalyzable<AnalyzableTick<decimal?>>)Context.GetFunc(name, @params);
        }

        public bool Execute(string name, params decimal[] @params)
        {
            var func = (Func<IndexedCandle, IReadOnlyList<decimal>, bool>)RuleRegistry.Get(name);
            return func(this, @params);
        }
    }
}