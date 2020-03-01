using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;
using System;

namespace Trady.Analysis
{
    public abstract class IndexedCandleBase : Candle, IIndexedOhlcv
    {
        private IAnalyzeContext _context;

        protected IndexedCandleBase(IEnumerable<IOhlcv> candles, int index)
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
            => Index - 1 >= 0 ? IndexedCandleConstructor(Index - 1) : null;

        public IIndexedOhlcv Next
            => Index + 1 < BackingList.Count() ? IndexedCandleConstructor(Index + 1) : null;

        public IIndexedOhlcv First
            => BackingList.Any() ? IndexedCandleConstructor(0) : null;

        public IIndexedOhlcv Last
            => BackingList.Any() ? IndexedCandleConstructor(BackingList.Count() - 1) : null;

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

        IIndexedObject IIndexedObject.First => First;

        IIndexedObject<IOhlcv> IIndexedObject<IOhlcv>.First => First;

        IIndexedObject IIndexedObject.Last => Last;

        IIndexedObject<IOhlcv> IIndexedObject<IOhlcv>.Last => Last;

        object IIndexedObject.Underlying => Underlying;

        IAnalyzeContext IIndexedObject.Context
        {
            get => _context;
            set => _context = value;
        }

        [Obsolete]
        public TAnalyzable Get<TAnalyzable>(params object[] @params) where TAnalyzable : IAnalyzable
            => Context == null ?
            AnalyzableFactory.CreateAnalyzable<TAnalyzable>(BackingList, @params) :
            Context.Get<TAnalyzable>(@params);

        [Obsolete]
        public IFuncAnalyzable<IAnalyzableTick<decimal?>> GetFunc(string name, params decimal[] @params)
            => Context == null ?
            FuncAnalyzableFactory.CreateAnalyzable(name, BackingList, @params) :
            (IFuncAnalyzable<IAnalyzableTick<decimal?>>)Context.GetFunc(name, @params);

        [Obsolete]
        public bool Eval(string name, params decimal[] @params)
        {
            var func = (Func<IIndexedOhlcv, IReadOnlyList<decimal>, bool>)RuleRegistry.Get(name);
            return func(this, @params);
        }

        public IIndexedOhlcv Before(int count)
            => Index >= count ? IndexedCandleConstructor(Index - count) : null;

        public IIndexedOhlcv After(int count)
            => Index + count < BackingList.Count() ? IndexedCandleConstructor(Index + count) : null;

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

        protected abstract IIndexedOhlcv IndexedCandleConstructor(int index);
    }
}
