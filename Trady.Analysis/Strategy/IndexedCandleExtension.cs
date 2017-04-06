using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Strategy
{
    public static class IndexedCandleExtension
    {
        public static decimal? PriceChange(this IndexedCandle ic)
            => ic.Get<ClosePriceChange>()[ic.Index];

        public static decimal? PricePercentageChange(this IndexedCandle ic)
            => ic.Get<ClosePricePercentageChange>()[ic.Index];

        public static bool IsCandlesBullish(this IndexedCandle ic)
            => ic.Get<ClosePriceChangeTrend>()[ic.Index] == Trend.Bullish;

        public static bool IsCandlesBearish(this IndexedCandle ic)
            => ic.Get<ClosePriceChangeTrend>()[ic.Index] == Trend.Bearish;

        public static bool IsAccumDistBullish(this IndexedCandle ic)
            => ic.Get<AccumulationDistributionLineTrend>()[ic.Index] == Trend.Bullish;

        public static bool IsAccumDistBearish(this IndexedCandle ic)
            => ic.Get<AccumulationDistributionLineTrend>()[ic.Index] == Trend.Bearish;

        public static bool IsObvBullish(this IndexedCandle ic)
            => ic.Get<OnBalanceVolumeTrend>()[ic.Index] == Trend.Bullish;

        public static bool IsObvBearish(this IndexedCandle ic)
            => ic.Get<OnBalanceVolumeTrend>()[ic.Index] == Trend.Bearish;

        public static bool IsInBbRange(this IndexedCandle ic, int periodCount, int sdCount)
            => ic.Get<BollingerBandsInRange>(periodCount, sdCount)[ic.Index] == Overboundary.InRange;

        public static bool IsHighest(this IndexedCandle ic, int periodCount)
            => ic.Get<IsHighestPrice>(periodCount)[ic.Index] == Match.IsMatched;

        public static bool IsLowest(this IndexedCandle ic, int periodCount)
            => ic.Get<IsLowestPrice>(periodCount)[ic.Index] == Match.IsMatched;

        public static bool IsRsiOverbought(this IndexedCandle ic, int periodCount)
            => ic.Get<RelativeStrengthIndexOvertrade>(periodCount)[ic.Index] == Overtrade.Overbought;

        public static bool IsRsiOversold(this IndexedCandle ic, int periodCount)
            => ic.Get<RelativeStrengthIndexOvertrade>(periodCount)[ic.Index] == Overtrade.Oversold;

        public static bool IsFastStoOverbought(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOvertrade.Fast>(periodCount, smaPeriodCount)[ic.Index] == Overtrade.SeverelyOverbought;

        public static bool IsFullStoOverbought(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOvertrade.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index] == Overtrade.SeverelyOverbought;

        public static bool IsSlowStoOverbought(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOvertrade.Slow>(periodCount, smaPeriodCountD)[ic.Index] == Overtrade.SeverelyOverbought;

        public static bool IsFastStoOversold(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOvertrade.Fast>(periodCount, smaPeriodCount)[ic.Index] == Overtrade.SeverelyOversold;

        public static bool IsFullStoOversold(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOvertrade.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index] == Overtrade.SeverelyOversold;

        public static bool IsSlowStoOversold(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOvertrade.Slow>(periodCount, smaPeriodCountD)[ic.Index] == Overtrade.SeverelyOversold;

        public static bool IsAboveSma(this IndexedCandle ic, int periodCount)
            => ic.Get<IsAboveSimpleMovingAverage>(periodCount)[ic.Index] == Match.IsMatched;

        public static bool IsAboveEma(this IndexedCandle ic, int periodCount)
            => ic.Get<IsAboveExponentialMovingAverage>(periodCount)[ic.Index] == Match.IsMatched;

        public static bool IsSmaBullish(this IndexedCandle ic, int periodCount)
            => ic.Get<SimpleMovingAverageTrend>(periodCount)[ic.Index] == Trend.Bullish;

        public static bool IsSmaBearish(this IndexedCandle ic, int periodCount)
            => ic.Get<SimpleMovingAverageTrend>(periodCount)[ic.Index] == Trend.Bearish;

        public static bool IsSmaOscBullish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillatorTrend>(periodCount1, periodCount2)[ic.Index] == Trend.Bullish;

        public static bool IsSmaOscBearish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageOscillatorTrend>(periodCount1, periodCount2)[ic.Index] == Trend.Bearish;

        public static bool IsEmaBullish(this IndexedCandle ic, int periodCount)
            => ic.Get<ExponentialMovingAverageTrend>(periodCount)[ic.Index] == Trend.Bullish;

        public static bool IsEmaBearish(this IndexedCandle ic, int periodCount)
            => ic.Get<ExponentialMovingAverageTrend>(periodCount)[ic.Index] == Trend.Bearish;

        public static bool IsEmaOscBullish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillatorTrend>(periodCount1, periodCount2)[ic.Index] == Trend.Bullish;

        public static bool IsEmaOscBearish(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageOscillatorTrend>(periodCount1, periodCount2)[ic.Index] == Trend.Bearish;

        public static bool IsMacdOscBullish(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceOscillatorTrend>(emaPeriodCount1, emaPeriodCount2, demPeriodCount)[ic.Index] == Trend.Bullish;

        public static bool IsMacdOscBearish(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceOscillatorTrend>(emaPeriodCount1, emaPeriodCount2, demPeriodCount)[ic.Index] == Trend.Bearish;

        public static bool IsFastStoOscBullish(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillatorTrend.Fast>(periodCount, smaPeriodCount)[ic.Index] == Trend.Bullish;

        public static bool IsFastStoOscBearish(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsOscillatorTrend.Fast>(periodCount, smaPeriodCount)[ic.Index] == Trend.Bearish;

        public static bool IsFullStoOscBullish(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillatorTrend.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index] == Trend.Bullish;

        public static bool IsFullStoOscBearish(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsOscillatorTrend.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index] == Trend.Bearish;

        public static bool IsSlowStoOscBullish(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillatorTrend.Slow>(periodCount, smaPeriodCountD)[ic.Index] == Trend.Bullish;

        public static bool IsSlowStoOscBearish(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsOscillatorTrend.Slow>(periodCount, smaPeriodCountD)[ic.Index] == Trend.Bearish;

        public static bool IsSmaBullishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageCrossover>(periodCount1, periodCount2)[ic.Index] == Crossover.BullishCrossover;

        public static bool IsSmaBearishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<SimpleMovingAverageCrossover>(periodCount1, periodCount2)[ic.Index] == Crossover.BearishCrossover;

        public static bool IsEmaBullishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageCrossover>(periodCount1, periodCount2)[ic.Index] == Crossover.BullishCrossover;

        public static bool IsEmaBearishCross(this IndexedCandle ic, int periodCount1, int periodCount2)
            => ic.Get<ExponentialMovingAverageCrossover>(periodCount1, periodCount2)[ic.Index] == Crossover.BearishCrossover;

        public static bool IsMacdBullishCross(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount)[ic.Index] == Crossover.BullishCrossover;

        public static bool IsMacdBearishCross(this IndexedCandle ic, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => ic.Get<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount)[ic.Index] == Crossover.BearishCrossover;

        public static bool IsFastStoBullishCross(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsCrossover.Fast>(periodCount, smaPeriodCount)[ic.Index] == Crossover.BullishCrossover;

        public static bool IsFastStoBearishCross(this IndexedCandle ic, int periodCount, int smaPeriodCount)
            => ic.Get<StochasticsCrossover.Fast>(periodCount, smaPeriodCount)[ic.Index] == Crossover.BearishCrossover;

        public static bool IsFullStoBullishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index] == Crossover.BullishCrossover;

        public static bool IsFullStoBearishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => ic.Get<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD)[ic.Index] == Crossover.BearishCrossover;

        public static bool IsSlowStoBullishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD)[ic.Index] == Crossover.BullishCrossover;

        public static bool IsSlowStoBearishCross(this IndexedCandle ic, int periodCount, int smaPeriodCountD)
            => ic.Get<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD)[ic.Index] == Crossover.BearishCrossover;
    }
}
