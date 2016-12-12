using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Analysis.Pattern
{
    public class IsMatchedMultistateResult<TTristate> : IsMatchedResult
    {
        private TTristate _state;

        public IsMatchedMultistateResult(DateTime dateTime, bool? isMatched, TTristate state) 
            : base(dateTime, isMatched)
        {
            _state = state;
        }

        public TTristate State => _state;
    }
}
