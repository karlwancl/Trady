using System;

namespace Trady.Core.Exception
{
    public class InvalidTimeframeException : System.Exception
    {
        private DateTimeOffset _invalidDateTime;

        public InvalidTimeframeException(DateTimeOffset invalidDateTime)
        {
            _invalidDateTime = invalidDateTime;
        }

        public override string Message => $"Invalid timeframe: {_invalidDateTime}";
    }
}