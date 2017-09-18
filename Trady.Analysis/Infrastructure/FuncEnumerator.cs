using System;
using System.Collections;
using System.Collections.Generic;

namespace Trady.Analysis.Infrastructure
{
    public class FuncEnumerator<TInput>: IEnumerator<TInput>
    {
        public Func<int, TInput> Func { get; }

        private int _index;

        public FuncEnumerator(Func<int, TInput> func)
        {
            Func = func;
        }

        public TInput Current => Func(_index);

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _index++;
            return true;
        }

        public void Reset()
        {
            _index = 0;
        }

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
        // ~FuncEnumerator() {
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
