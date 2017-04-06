using System;

namespace Trady.Core.Exception
{
    public class InvalidTimeframeException : System.Exception
    {
        private DateTime _invalidDateTime;

        public InvalidTimeframeException(DateTime invalidDateTime)
        {
            _invalidDateTime = invalidDateTime;
        }

        public override string Message => $"Invalid timeframe: {_invalidDateTime}";
    }
}