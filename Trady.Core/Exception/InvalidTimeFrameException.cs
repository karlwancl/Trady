using System;

namespace Trady.Core.Exception
{
    public class InvalidTimeFrameException : System.Exception
    {
        private DateTime _invalidDateTime;

        public InvalidTimeFrameException(DateTime invalidDateTime)
        {
            _invalidDateTime = invalidDateTime;
        }

        public override string Message => $"Invalid Time Frame: {_invalidDateTime}";
    }
}