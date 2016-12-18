using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Period
{
    public interface IPeriod
    {
        DateTime TimestampAt(DateTime dateTime, int periodCount);

        DateTime PrevTimestamp(DateTime dateTime);

        DateTime NextTimestamp(DateTime dateTime);

        bool IsTimestamp(DateTime dateTime);
    }
}
