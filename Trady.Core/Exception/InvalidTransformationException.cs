using Trady.Core.Period;

namespace Trady.Core.Exception
{
    public class InvalidTransformationException : System.Exception
    {
        private PeriodOption _sourcePeriod, _targetPeriod;

        public InvalidTransformationException(PeriodOption sourcePeriod, PeriodOption targetPeriod)
        {
            _sourcePeriod = sourcePeriod;
            _targetPeriod = targetPeriod;
        }

        public override string Message => $"Invalid transformation from {_sourcePeriod} to {_targetPeriod}";
    }
}