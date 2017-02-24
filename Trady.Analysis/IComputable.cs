using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

namespace Trady.Analysis
{
    public interface IComputable<TTick> where TTick : ITick
    {
        TimeSeries<TTick> Compute(DateTime? startTime, DateTime? endTime);

        TTick ComputeByDateTime(DateTime dateTime);

        TTick ComputeByIndex(int index);
    }
}
