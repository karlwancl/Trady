using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Pattern
{
    public class IsMatchedResult : TickBase
    {
        public IsMatchedResult(DateTime dateTime, bool isMatched) : base(dateTime)
        {
            IsMatched = isMatched;
        }

        public bool IsMatched { get; private set; }
    }
}
