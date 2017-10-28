using Trady.Analysis.Indicator;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Extension
{
    public static class IndexedIOhlcvDataExtension
    {
        public static decimal? ClosePriceChange(this IIndexedOhlcvData ic)
            => ic.Get<Momentum>(1)[ic.Index].Tick;

        public static decimal? ClosePricePercentageChange(this IIndexedOhlcvData ic)
            => ic.Get<RateOfChange>(1)[ic.Index].Tick;

        public static bool IsBullish(this IIndexedOhlcvData ic)
            => ic.Get<Momentum>(1)[ic.Index].Tick.IsPositive();

        public static bool IsBearish(this IIndexedOhlcvData ic)
            => ic.Get<Momentum>(1)[ic.Index].Tick.IsNegative();

        public static bool IsAccumDistBullish(this IIndexedOhlcvData ic)
            => ic.Get<AccumulationDistributionLine>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsAccumDistBearish(this IIndexedOhlcvData ic)
            => ic.Get<AccumulationDistributionLine>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsObvBullish(this IIndexedOhlcvData ic)
            => ic.Get<OnBalanceVolume>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsObvBearish(this IIndexedOhlcvData ic)
            => ic.Get<OnBalanceVolume>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsInBbRange(this IIndexedOhlcvData ic, int periodCount, int sdCount)
            => ic.Get<BollingerBands>(periodCount, sdCount)[ic.Index].Tick.IsTrue((low, mid, up) => ic.Close >= low && ic.Close <= up);

        public static bool IsAboveBbUp(this IIndexedOhlcvData ic, int periodCount, int sdCount)
            => ic.Get<BollingerBands>(periodCount, sdCount)[ic.Index].Tick.IsTrue((low, mid, up) => ic.Close > up);

        public static bool IsBelowBbLow(this IIndexedOhlcvData ic, int periodCount, int sdCount)
            => ic.Get<BollingerBands>(periodCount, sdCount)[ic.Index].Tick.IsTrue((low, mid, up) => ic.Close < low);

        public static bool IsRsiOverbought(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<RelativeStrengthIndex>(periodCount)[ic.Index].Tick.IsTrue(t => t >= 70);

        public static bool IsRsiOversold(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<RelativeStrengthIndex>(periodCount)[ic.Index].Tick.IsTrue(t => t <= 30);

        public static bool IsFastStoOverbought(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCount)
            => ic.Get<Stochastics.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick.IsTrue((k, d, j) => k >= 80);

        public static bool IsFullStoOverbought(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<Stochastics.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k >= 80);

        public static bool IsSlowStoOverbought(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountD)
            => ic.Get<Stochastics.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k >= 80);

        public static bool IsFastStoOversold(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCount)
            => ic.Get<Stochastics.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick.IsTrue((k, d, j) => k <= 20);

        public static bool IsFullStoOversold(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<Stochastics.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k <= 20);

        public static bool IsSlowStoOversold(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountD)
            => ic.Get<Stochastics.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick.IsTrue((k, d, j) => k <= 20);

        public static bool IsAboveSma(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close > t);

        public static bool IsAboveEma(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close > t);

        public static bool IsBelowSma(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close < t);

        public static bool IsBelowEma(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount)[ic.Index].Tick.IsTrue(t => ic.Close < t);

        public static bool IsSmaBullish(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsSmaBearish(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<SimpleMovingAverage>(periodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsSmaOscBullish(this IIndexedOhlcvData ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsPositive();

        public static bool IsSmaOscBearish(this IIndexedOhlcvData ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsNegative();

        public static bool IsEmaBullish(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsEmaBearish(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<ExponentialMovingAverage>(periodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsEmaOscBullish(this IIndexedOhlcvData ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsPositive();

        public static bool IsEmaOscBearish(this IIndexedOhlcvData ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).Diff(ic.Index).Tick.IsNegative();

        public static bool IsMacdOscBullish(this IIndexedOhlcvData ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsMacdOscBearish(this IIndexedOhlcvData ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsFastStoOscBullish(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsFastStoOscBearish(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsFullStoOscBullish(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).Diff(ic.Index).Tick.IsPositive();

        public static bool IsFullStoOscBearish(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).Diff(ic.Index).Tick.IsNegative();

        public static bool IsSlowStoOscBullish(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).Diff(ic.Index).Tick.IsPositive();

        public static bool IsSlowStoOscBearish(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).Diff(ic.Index).Tick.IsNegative();

        public static bool IsSmaBullishCross(this IIndexedOhlcvData ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsSmaBearishCross(this IIndexedOhlcvData ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsEmaBullishCross(this IIndexedOhlcvData ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsEmaBearishCross(this IIndexedOhlcvData ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsMacdBullishCross(this IIndexedOhlcvData ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsMacdBearishCross(this IIndexedOhlcvData ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceHistogram>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsFastStoBullishCross(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsFastStoBearishCross(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillator.Fast>(periodCount, smaPeriodCount).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsFullStoBullishCross(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsFullStoBearishCross(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsSlowStoBullishCross(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsSlowStoBearishCross(this IIndexedOhlcvData ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillator.Slow>(periodCount, smaPeriodCountD).ComputeNeighbour(ic.Index)
                 .IsTrue((prev, current, _) => prev.Tick.IsPositive() && current.Tick.IsNegative());

        public static bool IsBreakingHistoricalHighestHigh(this IIndexedOhlcvData ic)
            => ic.Get<HistoricalHighestHigh>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingHistoricalHighestClose(this IIndexedOhlcvData ic)
            => ic.Get<HistoricalHighestClose>().Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingHistoricalLowestLow(this IIndexedOhlcvData ic)
            => ic.Get<HistoricalLowestLow>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsBreakingHistoricalLowestClose(this IIndexedOhlcvData ic)
            => ic.Get<HistoricalLowestClose>().Diff(ic.Index).Tick.IsNegative();

        public static bool IsBreakingHighestHigh(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<HighestHigh>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingHighestClose(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<HighestClose>(periodCount).Diff(ic.Index).Tick.IsPositive();

        public static bool IsBreakingLowestLow(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<LowestLow>(periodCount).Diff(ic.Index).Tick.IsNegative();

        public static bool IsBreakingLowestClose(this IIndexedOhlcvData ic, int periodCount)
            => ic.Get<LowestClose>(periodCount).Diff(ic.Index).Tick.IsNegative();
    }
}