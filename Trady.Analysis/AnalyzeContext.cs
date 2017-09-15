using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public class AnalyzeContext : IAnalyzeContext<Candle>
    {
        private ConcurrentDictionary<string, IAnalyzable> _cache;

        public AnalyzeContext(IEnumerable<Candle> backingList)
        {
            BackingList = backingList;
            _cache = new ConcurrentDictionary<string, IAnalyzable>();
        }

        public TAnalyzable Get<TAnalyzable>(params object[] parameters) where TAnalyzable : IAnalyzable
            => (TAnalyzable)_cache.GetOrAdd($"{typeof(TAnalyzable).Name}#{string.Join("|", parameters)}", 
                                            _ => AnalyzableFactory.CreateAnalyzable<TAnalyzable>(BackingList, parameters));

        public IEnumerable<Candle> BackingList { get; }

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
}
