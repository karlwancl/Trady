using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis
{
    public static class IndexedCandleExtension
    {
        public static decimal? ClosePriceChange(this IndexedCandle ic)
            => ic.Get<ClosePriceChange>()[ic.Index].Tick;

        public static decimal? ClosePricePercentageChange(this IndexedCandle ic)
            => ic.Get<ClosePricePercentageChange>()[ic.Index].Tick;

        public static bool IsBullish(this IndexedCandle ic)
            => ic.Get<ClosePriceChangeTrend>()[ic.Index].Tick == Trend.Bullish;

        public static bool IsBearish(this IndexedCandle ic)
            => ic.Get<ClosePriceChangeTrend>()[ic.Index].Tick == Trend.Bearish;

        public static bool IsAccumDistBullish(this IndexedCandle ic)
            => ic.Get<AccumulationDistributionLineTrend>()[ic.Index].Tick == Trend.Bullish;

        public static bool IsAccumDistBearish(this IndexedCandle ic)
            => ic.Get<AccumulationDistributionLineTrend>()[ic.Index].Tick == Trend.Bearish;

        public static bool IsObvBullish(this IndexedCandle ic)
            => ic.Get<OnBalanceVolumeTrend>()[ic.Index].Tick == Trend.Bullish;

        public static bool IsObvBearish(this IndexedCandle ic)
            => ic.Get<OnBalanceVolumeTrend>()[ic.Index].Tick == Trend.Bearish;

        public static bool IsInBbRange(this IndexedCandle ic, int periodCount, int sdCount)
            => ic.Get<BollingerBandsInRange>(periodCount, sdCount)[ic.Index].Tick == Overboundary.InRange;

        public static bool IsRsiOverbought(this IndexedCandle ic, int periodCount)
            => ic.Get<RelativeStrengthIndexOvertrade>(periodCount)[ic.Index].Tick == Overtrade.Overbought;

        public static bool IsRsiOversold(this IndexedCandle ic, int periodCount)
            => ic.Get<RelativeStrengthIndexOvertrade>(periodCount)[ic.Index].Tick == Overtrade.Oversold;

        public static bool IsFastStoOverbought(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOvertrade.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick == Overtrade.SeverelyOverbought;

        public static bool IsFullStoOverbought(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOvertrade.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick == Overtrade.SeverelyOverbought;

        public static bool IsSlowStoOverbought(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOvertrade.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick == Overtrade.SeverelyOverbought;

        public static bool IsFastStoOversold(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOvertrade.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick == Overtrade.SeverelyOversold;

        public static bool IsFullStoOversold(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOvertrade.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick == Overtrade.SeverelyOversold;

        public static bool IsSlowStoOversold(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOvertrade.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick == Overtrade.SeverelyOversold;

        public static bool IsAboveSma(this IndexedCandle ic, int periodCount)
            => ic.Get<IsAboveSimpleMovingAverage>(periodCount)[ic.Index].Tick ?? false;

        public static bool IsAboveEma(this IndexedCandle ic, int periodCount)
            => ic.Get<IsAboveExponentialMovingAverage>(periodCount)[ic.Index].Tick ?? false;

        public static bool IsSmaBullish(this IndexedCandle ic, int periodCount)
            => ic.Get<SimpleMovingAverageTrend>(periodCount)[ic.Index].Tick == Trend.Bullish;

        public static bool IsSmaBearish(this IndexedCandle ic, int periodCount)
            => ic.Get<SimpleMovingAverageTrend>(periodCount)[ic.Index].Tick == Trend.Bearish;

        public static bool IsSmaOscBullish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillatorTrend>(periodCount1, periodCount2)[ic.Index].Tick == Trend.Bullish;

        public static bool IsSmaOscBearish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillatorTrend>(periodCount1, periodCount2)[ic.Index].Tick == Trend.Bearish;

        public static bool IsEmaBullish(this IndexedCandle ic, int periodCount)
            => ic.Get<ExponentialMovingAverageTrend>(periodCount)[ic.Index].Tick == Trend.Bullish;

        public static bool IsEmaBearish(this IndexedCandle ic, int periodCount)
            => ic.Get<ExponentialMovingAverageTrend>(periodCount)[ic.Index].Tick == Trend.Bearish;

        public static bool IsEmaOscBullish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillatorTrend>(periodCount1, periodCount2)[ic.Index].Tick == Trend.Bullish;

        public static bool IsEmaOscBearish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillatorTrend>(periodCount1, periodCount2)[ic.Index].Tick == Trend.Bearish;

        public static bool IsMacdOscBullish(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceOscillatorTrend>(emaPeriodCount1, emaPeriodCount2, demPeriodCount)[ic.Index].Tick == Trend.Bullish;

        public static bool IsMacdOscBearish(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceOscillatorTrend>(emaPeriodCount1, emaPeriodCount2, demPeriodCount)[ic.Index].Tick == Trend.Bearish;

        public static bool IsFastStoOscBullish(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillatorTrend.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick == Trend.Bullish;

        public static bool IsFastStoOscBearish(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillatorTrend.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick == Trend.Bearish;

        public static bool IsFullStoOscBullish(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillatorTrend.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick == Trend.Bullish;

        public static bool IsFullStoOscBearish(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillatorTrend.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick == Trend.Bearish;

        public static bool IsSlowStoOscBullish(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillatorTrend.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick == Trend.Bullish;

        public static bool IsSlowStoOscBearish(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillatorTrend.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick == Trend.Bearish;

        public static bool IsSmaBullishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageCrossover>(periodCount1, periodCount2)[ic.Index].Tick == Crossover.BullishCrossover;

        public static bool IsSmaBearishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageCrossover>(periodCount1, periodCount2)[ic.Index].Tick == Crossover.BearishCrossover;

        public static bool IsEmaBullishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageCrossover>(periodCount1, periodCount2)[ic.Index].Tick == Crossover.BullishCrossover;

        public static bool IsEmaBearishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageCrossover>(periodCount1, periodCount2)[ic.Index].Tick == Crossover.BearishCrossover;

        public static bool IsMacdBullishCross(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount)[ic.Index].Tick == Crossover.BullishCrossover;

        public static bool IsMacdBearishCross(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount)[ic.Index].Tick == Crossover.BearishCrossover;

        public static bool IsFastStoBullishCross(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsCrossover.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick == Crossover.BullishCrossover;

        public static bool IsFastStoBearishCross(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsCrossover.Fast>(periodCount, smaPeriodCount)[ic.Index].Tick == Crossover.BearishCrossover;

        public static bool IsFullStoBullishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick == Crossover.BullishCrossover;

        public static bool IsFullStoBearishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index].Tick == Crossover.BearishCrossover;

        public static bool IsSlowStoBullishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick == Crossover.BullishCrossover;

        public static bool IsSlowStoBearishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD)[ic.Index].Tick == Crossover.BearishCrossover;

        public static bool IsBreakingHistoricalHighestHigh(this IndexedCandle ic)
            => ic.Get<IsBreakingHistoricalHighestHigh>()[ic.Index]?.Tick ?? false;

        public static bool IsBreakingHistoricalHighestClose(this IndexedCandle ic)
            => ic.Get<IsBreakingHistoricalHighestClose>()[ic.Index]?.Tick ?? false;

        public static bool IsBreakingHistoricalLowestLow(this IndexedCandle ic)
            => ic.Get<IsBreakingHistoricalLowestLow>()[ic.Index]?.Tick ?? false;

        public static bool IsBreakingHistoricalLowestClose(this IndexedCandle ic)
            => ic.Get<IsBreakingHistoricalLowestClose>()[ic.Index]?.Tick ?? false;

        public static bool IsBreakingHighestHigh(this IndexedCandle ic, int periodCount)
            => ic.Get<IsBreakingHighestHigh>(periodCount)[ic.Index]?.Tick ?? false;

        public static bool IsBreakingHighestClose(this IndexedCandle ic, int periodCount)
            => ic.Get<IsBreakingHighestClose>(periodCount)[ic.Index]?.Tick ?? false;

        public static bool IsBreakingLowestLow(this IndexedCandle ic, int periodCount)
            => ic.Get<IsBreakingLowestLow>(periodCount)[ic.Index]?.Tick ?? false;

        public static bool IsBreakingLowestClose(this IndexedCandle ic, int periodCount)
            => ic.Get<IsBreakingLowestClose>(periodCount)[ic.Index]?.Tick ?? false;
    }
}