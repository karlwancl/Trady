using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Extension;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public class AnalyzeContext<TInput> : IAnalyzeContext<TInput>
    {
        private IDictionary<string, IAnalyzable> _cache;

        public AnalyzeContext(IEnumerable<TInput> backingList)
        {
            BackingList = backingList;
            _cache = new Dictionary<string, IAnalyzable>();
        }

        public TAnalyzable Get<TAnalyzable>(params object[] parameters) where TAnalyzable : IAnalyzable
        {
            var cacheKey = $"{typeof(TAnalyzable).Name}#{string.Join("|", parameters)}";
            IAnalyzable analyzable() => AnalyzableFactory.CreateAnalyzable<TAnalyzable, TInput>(BackingList, parameters);
            return (TAnalyzable)_cache.GetOrAdd(cacheKey, analyzable);
        }

		IFuncAnalyzable IAnalyzeContext.GetFunc(string name, params decimal[] parameters) => GetFunc(name, parameters);

        public IFuncAnalyzable<dynamic> GetFunc(string name, params decimal[] parameters)
        {
            var cacheKey = $"_Func_{name}#{string.Join("|", parameters)}";
            IFuncAnalyzable<dynamic> analyzable() => FuncAnalyzableFactory.CreateAnalyzable<TInput, dynamic>(name, BackingList, parameters);
            return (IFuncAnalyzable<dynamic>)_cache.GetOrAdd(cacheKey, analyzable);
        }

		public Predicate<T> GetRule<T>(string name, params decimal[] parameters) where T : IIndexedObject<TInput>
		{
			var func = (Func<T, IReadOnlyList<decimal>, bool>)RuleRegistry.Get(name);
			return ic => func(ic, parameters);
		}

        public IEnumerable<TInput> BackingList { get; }

        IEnumerable IAnalyzeContext.BackingList => BackingList;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AnalyzeContext() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class AnalyzeContext : AnalyzeContext<IOhlcv>
    {
        public AnalyzeContext(IEnumerable<IOhlcv> backingList) : base(backingList)
        {
        }

        public Predicate<IIndexedOhlcv> GetRule(string name, params decimal[] parameters)
            => GetRule<IIndexedOhlcv>(name, parameters);
    }
}
