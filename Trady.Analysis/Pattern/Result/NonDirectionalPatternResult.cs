using System;
using System.Collections.Generic;

namespace Trady.Analysis.Pattern
{
    public class NonDirectionalPatternResult : PatternResultBase
    {
        private const string IsMatchedTag = "IsMatched";

        public NonDirectionalPatternResult(DateTime dateTime, bool isMatched, IDictionary<string, bool> additionalValues = null)
            : base(dateTime, CreateValuesDictionary(isMatched, additionalValues))
        {
        }

        private static IDictionary<string, bool> CreateValuesDictionary(bool isMatched, IDictionary<string, bool> additionalValues)
        {
            var values = additionalValues ?? new Dictionary<string, bool>();
            values.Add(IsMatchedTag, isMatched);
            return values;
        }

        public bool IsMatched => Values[IsMatchedTag];
    }
}
