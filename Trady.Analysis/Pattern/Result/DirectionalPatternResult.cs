using System;
using System.Collections.Generic;

namespace Trady.Analysis.Pattern
{
    public class DirectionalPatternResult : NonDirectionalPatternResult
    {
        private const string IsBullishTag = "IsBullishTag";
        private const string IsBearishTag = "IsBearishTag";

        public DirectionalPatternResult(DateTime dateTime, bool isMatched, bool isBullish, bool isBearish, IDictionary<string, bool> additionalValues = null) 
            : base(dateTime, isMatched, CreateValuesDictionary(isBullish, isBearish, additionalValues))
        {
        }

        private static IDictionary<string, bool> CreateValuesDictionary(bool isBullish, bool isBearish, IDictionary<string, bool> additionalValues)
        {
            var values = additionalValues ?? new Dictionary<string, bool>();
            values.Add(IsBullishTag, isBullish);
            values.Add(IsBearishTag, isBearish);
            return values;
        }

        public bool IsBullish => Values[IsBullishTag];

        public bool IsBearish => Values[IsBearishTag];
    }
}
