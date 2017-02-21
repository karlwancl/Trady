namespace Trady.Analysis.Pattern.Helper
{
    internal static class Decision
    {
        public static bool? IsLargerThan(this decimal value1, decimal? value2)
            => IsLargerThan((decimal?)value1, value2);

        public static bool? IsLargerThan(this decimal? value1, decimal? value2)
        {
            if (!value1.HasValue || !value2.HasValue)
                return null;
            return value1.Value > value2.Value;
        }

        public static bool? IsCrossOver(decimal? value1, decimal? value2)
        {
            if (!value1.HasValue || !value2.HasValue)
                return null;
            return value1.Value * value2.Value < 0;
        }

        public static Trend? IsTrending(decimal? change)
        {
            if (!change.HasValue)
                return null;
            if (change > 0) return Trend.Bullish;
            if (change < 0) return Trend.Bearish;
            return Trend.NonTrended;
        }

        public static Overboundary? IsOverbound(decimal? value, decimal? lower, decimal? upper)
        {
            if (!value.HasValue || !lower.HasValue || !upper.HasValue) return null;
            if (value < lower) return Overboundary.BelowLower;
            if (value > upper) return Overboundary.AboveUpper;
            return Overboundary.InRange;
        }

        public static SevereOvertrade? IsSeverelyOvertrade(decimal? value)
        {
            if (!value.HasValue) return null;
            if (value <= 20) return SevereOvertrade.SevereOversold;
            if (value <= 30) return SevereOvertrade.Oversold;
            if (value >= 80) return SevereOvertrade.SevereOverbought;
            if (value >= 70) return SevereOvertrade.Overbought;
            return SevereOvertrade.Normal;
        }

        public static Overtrade? IsOvertrade(decimal? value)
        {
            if (!value.HasValue) return null;
            if (value <= 20) return Overtrade.Oversold;
            if (value >= 80) return Overtrade.Overbought;
            return Overtrade.Normal;
        }
    }
}