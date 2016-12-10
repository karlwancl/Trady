using System;
using System.Collections.Generic;

namespace Trady.Analysis.Pattern
{
    public class IsMatchedResult : PatternResultBase
    {
        private const string IsMatchedTag = "IsMatched";

        public IsMatchedResult(DateTime dateTime, bool isMatched)
            : base(dateTime, new Dictionary<string, bool> { { IsMatchedTag, isMatched } })
        {
        }

        public bool IsMatched => Values[IsMatchedTag];
    }
}
