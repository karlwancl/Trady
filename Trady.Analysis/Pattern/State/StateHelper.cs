namespace Trady.Analysis.Pattern.State
{
    internal static class StateHelper
    {
        public static bool? IsLargerThan(this decimal value1, decimal? value2)
            => IsLargerThan((decimal?)value1, value2);

        public static bool? IsLargerThan(this decimal? value1, decimal? value2)
        {
            if (!value1.HasValue || !value2.HasValue)
                return null;
            return value1.Value > value2.Value;
        }

        //public static Trend? IsTrending(decimal? last, decimal? secondLast)
        //{
        //    if (!last.HasValue || !secondLast.HasValue) return null;
        //    return IsTrending(last.Value - secondLast.Value);
        //}

        //public static Trend? IsTrending(decimal? change)
        //{
        //    if (!change.HasValue) return null;
        //    if (change.Value > 0) return Trend.Bullish;
        //    if (change.Value < 0) return Trend.Bearish;
        //    return Trend.Neutral;
        //}

        public static Overboundary? IsOverbound(decimal? value, decimal? lower, decimal? upper)
        {
            if (!value.HasValue || !lower.HasValue || !upper.HasValue) return null;
            if (value < lower) return Overboundary.BelowLower;
            if (value > upper) return Overboundary.AboveUpper;
            return Overboundary.InRange;
        }

        public static Overtrade? IsOvertrade(decimal? value)
        {
            if (!value.HasValue) return null;
            if (value <= 20) return Overtrade.SeverelyOversold;
            if (value <= 30) return Overtrade.Oversold;
            if (value >= 80) return Overtrade.SeverelyOverbought;
            if (value >= 70) return Overtrade.Overbought;
            return Overtrade.InRange;
        }

        public static Crossover? IsCrossover(decimal? last, decimal? secondLast)
        {
            if (!last.HasValue || !secondLast.HasValue) return null;
            if (last.Value * secondLast.Value > 0) return Crossover.NoCrossover;
            if (last.Value >= 0 && secondLast.Value < 0) return Crossover.BullishCrossover;
            if (last.Value <= 0 && secondLast.Value > 0) return Crossover.BearishCrossover;
            return Crossover.NoCrossover;
        }
    }
}