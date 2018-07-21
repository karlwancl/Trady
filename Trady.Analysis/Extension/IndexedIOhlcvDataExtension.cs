using Trady.Analysis.Indicator;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Extension
{
    public static class IndexedIOhlcvDataExtension
    {
        public static decimal? ClosePriceChange(this IIndexedOhlcv ic)
            => ic.Get<Momentum>(1)[ic.Index].Tick;

        public static decimal? ClosePricePercentageChange(this IIndexedOhlcv ic)
            => ic.Get<RateOfChange>(1)[ic.Index].Tick;

        public static bool IsBullish(this IIndexedOhlcv ic)
            => ic.Get<Momentum>(1)[ic.Index].Tick.IsPositive();

        public static bool IsBearish(this IIndexedOhlcv ic)
            => ic.Get<Momentum>(1)[ic.Index].Tick.IsNegative();

        public static bool IsAccumDistBullish(this IIndexedOhlcv ic)
            => ic.Get<AccumulationDistributionLine>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsAccumDistBearish(this IIndexedOhlcv ic)
            => ic.Get<AccumulationDistributionLine>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsObvBullish(this IIndexedOhlcv ic)
            => ic.Get<OnBalanceVolume>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsObvBearish(this IIndexedOhlcv ic)
            => ic.Get<OnBalanceVolume>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsInBbRange(this IIndexedOhlcv ic, int periodCount, int sdCount)
            => ic.Get<BollingerBands>(periodCount, sdCount)[ic.Index].Tick.IsTrue((low, mid, up) => ic.Close >= low && ic.Close <= up);

        public static bool IsAboveBbUp(this IIndexedOhlcv ic, int periodCount, int sdCount)
            => ic.Get<BollingerBands>(periodCount, sdCount)[ic.Index].Tick.IsTrue((low, mid, up) => ic.Close > up);

        public static bool IsBelowBbLow(this IIndexedOhlcv ic, int periodCount, int sdCount)
            => ic.Get<BollingerBands>(periodCount, sdCount)[ic.Index].Tick.IsTrue((low, mid, up) => ic.Close < low);

        public static bool IsRsiOverbought(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<RelativeStrengthIndex>(periodCount)[ic.Index].Tick.IsTrue(t => t >= 70);

        public static bool IsRsiOversold(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<RelativeStrengthIndex>(periodCount)[ic.Index].Tick.IsTrue(t => t <= 30);

        public static bool IsFastStoOverbought(this IIndexedOhlcv ic, int periodCount, int smaPeriodCount)
            => ic.Get<Stochastics.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick.IsTrue((k, d, j) => k >= 80);

        public static bool IsFullStoOverbought(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<Stochastics.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k >= 80);

        public static bool IsSlowStoOverbought(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountD)
            => ic.Get<Stochastics.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k >= 80);

        public static bool IsFastStoOversold(this IIndexedOhlcv ic, int periodCount, int smaPeriodCount)
            => ic.Get<Stochastics.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick.IsTrue((k, d, j) => k <= 20);

        public static bool IsFullStoOversold(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<Stochastics.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k <= 20);

        public static bool IsSlowStoOversold(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountD)
            => ic.Get<Stochastics.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k <= 20);

        public static bool IsAboveSma(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close > t);

        public static bool IsAboveEma(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close > t);

        public static bool IsBelowSma(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close < t);

        public static bool IsBelowEma(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close < t);

        public static bool IsSmaBullish(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsSmaBearish(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsSmaOscBullish(this IIndexedOhlcv ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsPositive();

        public static bool IsSmaOscBearish(this IIndexedOhlcv ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsNegative();

        public static bool IsEmaBullish(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsEmaBearish(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsEmaOscBullish(this IIndexedOhlcv ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsPositive();

        public static bool IsEmaOscBearish(this IIndexedOhlcv ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsNegative();

        public static bool IsMacdOscBullish(this IIndexedOhlcv ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsMacdOscBearish(this IIndexedOhlcv ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsFastStoOscBullish(this IIndexedOhlcv ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsFastStoOscBearish(this IIndexedOhlcv ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsFullStoOscBullish(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).Diff(ic.Index).Tick.IsPositive();

        public static bool IsFullStoOscBearish(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).Diff(ic.Index).Tick.IsNegative();

        public static bool IsSlowStoOscBullish(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).Diff(ic.Index).Tick.IsPositive();

        public static bool IsSlowStoOscBearish(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).Diff(ic.Index).Tick.IsNegative();

        public static bool IsSmaBullishCross(this IIndexedOhlcv ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsSmaBearishCross(this IIndexedOhlcv ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsEmaBullishCross(this IIndexedOhlcv ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsEmaBearishCross(this IIndexedOhlcv ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsMacdBullishCross(this IIndexedOhlcv ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsMacdBearishCross(this IIndexedOhlcv ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsFastStoBullishCross(this IIndexedOhlcv ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsFastStoBearishCross(this IIndexedOhlcv ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsFullStoBullishCross(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsFullStoBearishCross(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsSlowStoBullishCross(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsSlowStoBearishCross(this IIndexedOhlcv ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsBreakingHistoricalHighestHigh(this IIndexedOhlcv ic)
            => ic.Get<HistoricalHighestHigh>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingHistoricalHighestClose(this IIndexedOhlcv ic)
            => ic.Get<HistoricalHighestClose>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingHistoricalLowestLow(this IIndexedOhlcv ic)
            => ic.Get<HistoricalLowestLow>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsBreakingHistoricalLowestClose(this IIndexedOhlcv ic)
            => ic.Get<HistoricalLowestClose>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsBreakingHighestHigh(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<HighestHigh>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingHighestClose(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<HighestClose>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingLowestLow(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<LowestLow>(periodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsBreakingLowestClose(this IIndexedOhlcv ic, int periodCount)
            => ic.Get<LowestClose>(periodCount).Diff(ic.Index).Tick.IsNegative();
    }
}