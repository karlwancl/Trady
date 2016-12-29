using System;
using System.Collections.Generic;
using System.Text;
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

        public override string Message => $"Transformation From {_sourcePeriod} To {_targetPeriod} Is Invalid";
    }
}
