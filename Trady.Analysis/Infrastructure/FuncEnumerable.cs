using System;
using System.Collections;
using System.Collections.Generic;

namespace Trady.Analysis.Infrastructure
{
    public class FuncEnumerable<TInput>: IEnumerable<TInput>
    {
        public Func<int, TInput> Func { get; }

        public FuncEnumerable(Func<int, TInput> func)
        {
            Func = func;
        }

        public IEnumerator<TInput> GetEnumerator() => new FuncEnumerator<TInput>(Func);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
