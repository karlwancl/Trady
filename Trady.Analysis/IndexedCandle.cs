using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;
using System;

namespace Trady.Analysis
{
    public class IndexedCandle : Candle, IIndexedOhlcv
    {
        private IAnalyzeContext _context;

        public IndexedCandle(IEnumerable<IOhlcv> candles, int index)
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

        public IEnumerable<IOhlcv> BackingList { get; }

        IIndexedOhlcv IIndexedOhlcv.Prev => Prev;

        IIndexedOhlcv IIndexedOhlcv.Next => Next;

        IOhlcv IIndexedOhlcv.Underlying => Underlying;

        public int Index { get; }

        public IIndexedOhlcv Prev
        {
            get
            {
                if (Index - 1 < 0)
                    return null;
                return new IndexedCandle(BackingList, Index - 1)
                {
                    Context = Context
                };
            }
        }

        public IIndexedOhlcv Next
        {
            get
            {
                if (Index + 1 >= BackingList.Count())
                    return null;
                return new IndexedCandle(BackingList, Index + 1)
                {
                    Context = Context
                };
            }
        }

        public IOhlcv Underlying => BackingList.ElementAt(Index);

        public IAnalyzeContext<IOhlcv> Context
        {
            get => (IAnalyzeContext<IOhlcv>)_context;
            set => _context = value;
        }

        IEnumerable IIndexedObject.BackingList => BackingList;

        IIndexedObject IIndexedObject.Prev => Prev;

        IIndexedObject<IOhlcv> IIndexedObject<IOhlcv>.Prev => Prev;

        IIndexedObject IIndexedObject.Next => Next;

        IIndexedObject<IOhlcv> IIndexedObject<IOhlcv>.Next => Next;

        object IIndexedObject.Underlying => Underlying;

        IAnalyzeContext IIndexedObject.Context
        {
            get => _context;
            set => _context = value;
        }

        [Obsolete]
        public TAnalyzable Get<TAnalyzable>(params object[] @params) where TAnalyzable : IAnalyzable
        {
            if (Context == null)
            {
                return AnalyzableFactory.CreateAnalyzable<TAnalyzable>(BackingList, @params);
            }

            return Context.Get<TAnalyzable>(@params);
        }

        [Obsolete]
        public IFuncAnalyzable<IAnalyzableTick<decimal?>> GetFunc(string name, params decimal[] @params)
        {
            if (Context == null)
            {
                return FuncAnalyzableFactory.CreateAnalyzable(name, BackingList, @params);
            }

            return (IFuncAnalyzable<IAnalyzableTick<decimal?>>)Context.GetFunc(name, @params);
        }

        [Obsolete]
        public bool Eval(string name, params decimal[] @params)
        {
            var func = (Func<IndexedCandle, IReadOnlyList<decimal>, bool>)RuleRegistry.Get(name);
            return func(this, @params);
        }

        public IIndexedOhlcv Before(int count)
        {
            if (Index - count < 0)
                return null;
            return new IndexedCandle(BackingList, Index - count)
            {
                Context = Context
            };
        }

        public IIndexedOhlcv After(int count)
        {
            if (Index + count >= BackingList.Count())
                return null;
            return new IndexedCandle(BackingList, Index + count)
            {
                Context = Context
            };
        }

        public decimal? EvalDecimal<TAnalyzable>(params object[] @params) where TAnalyzable : IAnalyzable
            => Eval<TAnalyzable, decimal?>(@params);

        public bool? EvalBool<TAnalyzable>(params object[] @params) where TAnalyzable : IAnalyzable
            => Eval<TAnalyzable, bool?>(@params);

        private TOutputType Eval<TAnalyzable, TOutputType>(params object[] @params) where TAnalyzable : IAnalyzable
        {
            object obj = Context == null ?
                AnalyzableFactory.CreateAnalyzable<TAnalyzable>(BackingList, @params)[Index] :
                Context.Get<TAnalyzable>(@params)[Index];

            if (obj.GetType().IsAssignableFrom(typeof(AnalyzableTick<TOutputType>)))
            {
                var analyzableObj = (AnalyzableTick<TOutputType>)obj;
                return analyzableObj.Tick;
            }
            else throw new TypeLoadException($"The output is not a type of AnalyzableTick<{typeof(TOutputType).Name}>");
        }

        public decimal? EvalFunc(string name, params decimal[] @params)
        {
            if (Context == null)
                return FuncAnalyzableFactory.CreateAnalyzable(name, BackingList, @params)[Index].Tick;

            var analyzable = (IFuncAnalyzable<IAnalyzableTick<decimal?>>)Context.GetFunc(name, @params);
            return analyzable[Index].Tick;
        }

        IIndexedObject<IOhlcv> IIndexedObject<IOhlcv>.Before(int count) => Before(count);

        IIndexedObject<IOhlcv> IIndexedObject<IOhlcv>.After(int count) => After(count);

        IIndexedObject IIndexedObject.Before(int count) => Before(count);

        IIndexedObject IIndexedObject.After(int count) => After(count);
    }
}
