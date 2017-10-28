using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;
using System;

namespace Trady.Analysis
{
    public class IndexedCandle : Candle, IIndexedOhlcvData
    {
        private IAnalyzeContext _context;

        public IndexedCandle(IEnumerable<IOhlcvData> candles, int index)
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

        public IEnumerable<IOhlcvData> BackingList { get; }

        IIndexedOhlcvData IIndexedOhlcvData.Prev => Prev;

        IIndexedOhlcvData IIndexedOhlcvData.Next => Next;

        IOhlcvData IIndexedOhlcvData.Underlying => Underlying;

        public int Index { get; }

        public IIndexedOhlcvData Prev => Index - 1 >= 0 ? new IndexedCandle(BackingList, Index - 1) : null;

        public IIndexedOhlcvData Next => Index + 1 < BackingList.Count() ? new IndexedCandle(BackingList, Index + 1) : null;

        public IOhlcvData Underlying => BackingList.ElementAt(Index);

        public IAnalyzeContext<IOhlcvData> Context
        {
            get => (IAnalyzeContext<IOhlcvData>)_context;
            set => _context = value;
        }

        IEnumerable IIndexedObject.BackingList => BackingList;

        IIndexedObject IIndexedObject.Prev => Prev;

        IIndexedObject<IOhlcvData> IIndexedObject<IOhlcvData>.Prev => Prev;

        IIndexedObject IIndexedObject.Next => Next;

        IIndexedObject<IOhlcvData> IIndexedObject<IOhlcvData>.Next => Next;

        object IIndexedObject.Underlying => Underlying;

        IAnalyzeContext IIndexedObject.Context
        {
            get => _context;
            set => _context = value;
        }

        public TAnalyzable Get<TAnalyzable>(params object[] @params) where TAnalyzable : IAnalyzable
        {
            if (Context == null)
                return AnalyzableFactory.CreateAnalyzable<TAnalyzable>(BackingList, @params);
            return Context.Get<TAnalyzable>(@params);
        }

        public IFuncAnalyzable<IAnalyzableTick<decimal?>> GetFunc(string name, params decimal[] @params)
        {
            if (Context == null)
                return FuncAnalyzableFactory.CreateAnalyzable(name, BackingList, @params);
            return (IFuncAnalyzable<IAnalyzableTick<decimal?>>)Context.GetFunc(name, @params);
        }

        public bool Eval(string name, params decimal[] @params)
        {
            var func = (Func<IndexedCandle, IReadOnlyList<decimal>, bool>)RuleRegistry.Get(name);
            return func(this, @params);
        }
    }
}