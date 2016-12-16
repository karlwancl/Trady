using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Helper
{
    internal static class DatetimeExtension
    {
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }
}
