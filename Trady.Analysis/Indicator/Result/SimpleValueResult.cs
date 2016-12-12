using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    internal class SimpleValueResult<T> : TickBase
    {
        private T _value;

        public SimpleValueResult(DateTime dateTime, T value) : base(dateTime)
        {
            _value = value;
        }

        public T Value => _value;
    }
}
