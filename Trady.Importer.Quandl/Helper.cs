using System;
using System.Globalization;
using Trady.Core;

namespace Trady.Importer.Helper
{
    static class Helper
    {
        public static bool IsNullOrWhitespace(this object[] row)
        {
            foreach (var r in row)
            {
                if (r == null || string.IsNullOrWhiteSpace(r.ToString()))
                    return true;
            }
            return false;
        }

        public static Candle CreateCandle(this object[] row)
        {
            return new Candle(
                Convert.ToDateTime(row[0], CultureInfo.InvariantCulture),
                Convert.ToDecimal(row[1], CultureInfo.InvariantCulture),
                Convert.ToDecimal(row[2], CultureInfo.InvariantCulture),
                Convert.ToDecimal(row[3], CultureInfo.InvariantCulture),
                Convert.ToDecimal(row[4], CultureInfo.InvariantCulture),
                Convert.ToDecimal(row[5], CultureInfo.InvariantCulture));
        }
    }
}