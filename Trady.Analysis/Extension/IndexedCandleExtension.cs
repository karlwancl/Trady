using Trady.Analysis.Indicator;

namespace Trady.Analysis
{
    public static class IndexedCandleExtension
    {
        public static decimal? ClosePriceChange(this IndexedCandle ic)
            => ic.Get<Momentum>(1)[ic.Index].Tick;

        public static decimal? ClosePricePercentageChange(this IndexedCandle ic)
            => ic.Get<RateOfChange>(1)[ic.Index].Tick;

        public static bool IsBullish(this IndexedCandle ic)
            => ic.Get<Momentum>(1)[ic.Index].Tick.IsPositive();

        public static bool IsBearish(this IndexedCandle ic)
            => ic.Get<Momentum>(1)[ic.Index].Tick.IsNegative();

        public static bool IsAccumDistBullish(this IndexedCandle ic)
            => ic.Get<AccumulationDistributionLine>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsAccumDistBearish(this IndexedCandle ic)
            => ic.Get<AccumulationDistributionLine>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsObvBullish(this IndexedCandle ic)
            => ic.Get<OnBalanceVolume>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsObvBearish(this IndexedCandle ic)
            => ic.Get<OnBalanceVolume>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsInBbRange(this IndexedCandle ic, int periodCount, int sdCount)
            => ic.Get<BollingerBands>(periodCount, sdCount)[ic.Index].Tick.IsTrue((low, mid, up) => ic.Close >= low && ic.Close <= up);

        public static bool IsAboveBbUp(this IndexedCandle ic, int periodCount, int sdCount)
            => ic.Get<BollingerBands>(periodCount, sdCount)[ic.Index].Tick.IsTrue((low, mid, up) => ic.Close > up);

        public static bool IsBelowBbLow(this IndexedCandle ic, int periodCount, int sdCount)
            => ic.Get<BollingerBands>(periodCount, sdCount)[ic.Index].Tick.IsTrue((low, mid, up) => ic.Close < low);

        public static bool IsRsiOverbought(this IndexedCandle ic, int periodCount)
            => ic.Get<RelativeStrengthIndex>(periodCount)[ic.Index].Tick.IsTrue(t => t >= 70);

        public static bool IsRsiOversold(this IndexedCandle ic, int periodCount)
            => ic.Get<RelativeStrengthIndex>(periodCount)[ic.Index].Tick.IsTrue(t => t <= 30);

        public static bool IsFastStoOverbought(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<Stochastics.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick.IsTrue((k, d, j) => k >= 80);

        public static bool IsFullStoOverbought(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<Stochastics.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k >= 80);

        public static bool IsSlowStoOverbought(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<Stochastics.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k >= 80);

        public static bool IsFastStoOversold(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<Stochastics.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick.IsTrue((k, d, j) => k <= 20);

        public static bool IsFullStoOversold(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<Stochastics.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k <= 20);

        public static bool IsSlowStoOversold(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<Stochastics.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k <= 20);

        public static bool IsAboveSma(this IndexedCandle ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close > t);

        public static bool IsAboveEma(this IndexedCandle ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close > t);

        public static bool IsBelowSma(this IndexedCandle ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close < t);

        public static bool IsBelowEma(this IndexedCandle ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close < t);

        public static bool IsSmaBullish(this IndexedCandle ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsSmaBearish(this IndexedCandle ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsSmaOscBullish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsPositive();

        public static bool IsSmaOscBearish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsNegative();

        public static bool IsEmaBullish(this IndexedCandle ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsEmaBearish(this IndexedCandle ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsEmaOscBullish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsPositive();

        public static bool IsEmaOscBearish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsNegative();

        public static bool IsMacdOscBullish(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsMacdOscBearish(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsFastStoOscBullish(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsFastStoOscBearish(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsFullStoOscBullish(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).Diff(ic.Index).Tick.IsPositive();

        public static bool IsFullStoOscBearish(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).Diff(ic.Index).Tick.IsNegative();

        public static bool IsSlowStoOscBullish(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).Diff(ic.Index).Tick.IsPositive();

        public static bool IsSlowStoOscBearish(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).Diff(ic.Index).Tick.IsNegative();

        public static bool IsSmaBullishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsSmaBearishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsEmaBullishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsEmaBearishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsMacdBullishCross(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsMacdBearishCross(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsFastStoBullishCross(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsFastStoBearishCross(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsFullStoBullishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsFullStoBearishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsSlowStoBullishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsSlowStoBearishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsBreakingHistoricalHighestHigh(this IndexedCandle ic)
            => ic.Get<HistoricalHighestHigh>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingHistoricalHighestClose(this IndexedCandle ic)
            => ic.Get<HistoricalHighestClose>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingHistoricalLowestLow(this IndexedCandle ic)
            => ic.Get<HistoricalLowestLow>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsBreakingHistoricalLowestClose(this IndexedCandle ic)
            => ic.Get<HistoricalLowestClose>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsBreakingHighestHigh(this IndexedCandle ic, int periodCount)
            => ic.Get<HighestHigh>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingHighestClose(this IndexedCandle ic, int periodCount)
            => ic.Get<HighestClose>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingLowestLow(this IndexedCandle ic, int periodCount)
            => ic.Get<LowestLow>(periodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsBreakingLowestClose(this IndexedCandle ic, int periodCount)
            => ic.Get<LowestClose>(periodCount).Diff(ic.Index).Tick.IsNegative();
    }
}