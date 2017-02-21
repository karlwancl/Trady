using System;
using Trady.Core;

namespace Trady.Importer.Helper
{
    internal static class ImporterExtension
    {
        public static bool IsRowNullOrWhiteSpace(this object[] row)
        {
            foreach (var r in row)
            {
                if (r == null || string.IsNullOrWhiteSpace(r.ToString()))
                    return true;
            }
            return false;
        }

        public static Candle CreateCandleFromRow(this object[] row)
        {
            return new Candle(
                Convert.ToDateTime(row[0]),
                Convert.ToDecimal(row[1]),
                Convert.ToDecimal(row[2]),
                Convert.ToDecimal(row[3]),
                Convert.ToDecimal(row[4]),
                Convert.ToInt64(row[5]));
        }
    }
}