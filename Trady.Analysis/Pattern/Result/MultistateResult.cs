using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Pattern
{
    public class MultistateResult<TTristate> : PatternResultBase
    {
        private TTristate _state;

        public MultistateResult(DateTime dateTime, TTristate state) : base(dateTime, null)
        {
            _state = state;
        }

        public TTristate State => _state;
    }
}
