using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public static class EquityExtension
    {
        public static TimeSeries<AccumulationDistributionLine.IndicatorResult> AccumDist(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< AccumulationDistributionLine>().Compute(startTime, endTime);

        public static TimeSeries<DirectionalMovementIndex.IndicatorResult> Adx(this Equity equity, int periodCount, int adxrPeriodCount = 0, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< DirectionalMovementIndex>( periodCount, adxrPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<AverageTrueRange.IndicatorResult> Atr(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< AverageTrueRange>( periodCount).Compute(startTime, endTime);

        public static TimeSeries<BollingerBands.IndicatorResult> Bb(this Equity equity, int periodCount, int sdCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< BollingerBands>( periodCount, sdCount).Compute(startTime, endTime);

        public static TimeSeries<ClosePriceChange.IndicatorResult> PriceChange(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< ClosePriceChange>().Compute(startTime, endTime);

        public static TimeSeries<ExponentialMovingAverage.IndicatorResult> Ema(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< ExponentialMovingAverage>( periodCount).Compute(startTime, endTime);

        public static TimeSeries<ExponentialMovingAverageOscillator.IndicatorResult> EmaOsc(this Equity equity, int periodCount1, int periodCount2, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< ExponentialMovingAverageOscillator>( periodCount1, periodCount2).Compute(startTime, endTime);

        public static TimeSeries<HighestHigh.IndicatorResult> HighestHigh(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< HighestHigh>( periodCount).Compute(startTime, endTime);

        public static TimeSeries<LowestLow.IndicatorResult> LowestLow(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< LowestLow>( periodCount).Compute(startTime, endTime);

        public static TimeSeries<MovingAverageConvergenceDivergence.IndicatorResult> Macd(this Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< MovingAverageConvergenceDivergence>( emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<OnBalanceVolume.IndicatorResult> Obv(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< OnBalanceVolume>().Compute(startTime, endTime);

        public static TimeSeries<RawStochasticsValue.IndicatorResult> Rsv(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< RawStochasticsValue>( periodCount).Compute(startTime, endTime);

        public static TimeSeries<RelativeStrength.IndicatorResult> Rs(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< RelativeStrength>( periodCount).Compute(startTime, endTime);

        public static TimeSeries<RelativeStrengthIndex.IndicatorResult> Rsi(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< RelativeStrengthIndex>( periodCount).Compute(startTime, endTime);

        public static TimeSeries<SimpleMovingAverage.IndicatorResult> Sma(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< SimpleMovingAverage>( periodCount).Compute(startTime, endTime);

        public static TimeSeries<SimpleMovingAverageOscillator.IndicatorResult> SmaOsc(this Equity equity, int periodCount1, int periodCount2, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< SimpleMovingAverageOscillator>( periodCount1, periodCount2).Compute(startTime, endTime);

        public static TimeSeries<Stochastics.IndicatorResult> FastSto(this Equity equity, int periodCount, int smaPeriodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< Stochastics.Fast>( periodCount, smaPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<Stochastics.IndicatorResult> FullSto(this Equity equity, int periodCount, int smaPeriodCountK, int smaPeriodCountD, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< Stochastics.Full>( periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startTime, endTime);

        public static TimeSeries<Stochastics.IndicatorResult> SlowSto(this Equity equity, int periodCount, int smaPeriodCountD, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnayltic< Stochastics.Slow>( periodCount, smaPeriodCountD).Compute(startTime, endTime);
    }
}
