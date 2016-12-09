using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Period
{
    public interface IPeriod
    {
        DateTime NextTimestamp(DateTime dateTime);

        bool IsTimestamp(DateTime dateTime);
    }
}
