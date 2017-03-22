using System;
using Trady.Core;
using Trady.Core.Period;

namespace Trady.Analysis.Indicator
{
    public static class EquityExtension
    {
        public static TimeSeries<AccumulationDistributionLine.IndicatorResult> AccumDist(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<AccumulationDistributionLine>().Compute(startTime, endTime);

        public static TimeSeries<DirectionalMovementIndex.IndicatorResult> Dmi(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<DirectionalMovementIndex>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<AverageDirectionalMovementIndexRating.IndicatorResult> Adxr(this Equity equity, int periodCount, int adxrPeriodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<AverageDirectionalMovementIndexRating>(periodCount, adxrPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<AverageTrueRange.IndicatorResult> Atr(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<AverageTrueRange>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<BollingerBands.IndicatorResult> Bb(this Equity equity, int periodCount, int sdCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<BollingerBands>(periodCount, sdCount).Compute(startTime, endTime);

        public static TimeSeries<BollingerBandWidth.IndicatorResult> BbWidth(this Equity equity, int periodCount, int sdCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<BollingerBandWidth>(periodCount, sdCount).Compute(startTime, endTime);

        public static TimeSeries<ClosePriceChange.IndicatorResult> PriceChange(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<ClosePriceChange>().Compute(startTime, endTime);

        public static TimeSeries<ExponentialMovingAverage.IndicatorResult> Ema(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<ExponentialMovingAverage>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<ExponentialMovingAverageOscillator.IndicatorResult> EmaOsc(this Equity equity, int periodCount1, int periodCount2, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<ExponentialMovingAverageOscillator>(periodCount1, periodCount2).Compute(startTime, endTime);

        public static TimeSeries<HighestHigh.IndicatorResult> HighestHigh(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<HighestHigh>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<LowestLow.IndicatorResult> LowestLow(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<LowestLow>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<MovingAverageConvergenceDivergence.IndicatorResult> Macd(this Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<MovingAverageConvergenceDivergence>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<OnBalanceVolume.IndicatorResult> Obv(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<OnBalanceVolume>().Compute(startTime, endTime);

        public static TimeSeries<RawStochasticsValue.IndicatorResult> Rsv(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<RawStochasticsValue>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<RelativeStrength.IndicatorResult> Rs(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<RelativeStrength>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<RelativeStrengthIndex.IndicatorResult> Rsi(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<RelativeStrengthIndex>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<SimpleMovingAverage.IndicatorResult> Sma(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<SimpleMovingAverage>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<SimpleMovingAverageOscillator.IndicatorResult> SmaOsc(this Equity equity, int periodCount1, int periodCount2, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<SimpleMovingAverageOscillator>(periodCount1, periodCount2).Compute(startTime, endTime);

        public static TimeSeries<Stochastics.IndicatorResult> FastSto(this Equity equity, int periodCount, int smaPeriodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<Stochastics.Fast>(periodCount, smaPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<Stochastics.IndicatorResult> FullSto(this Equity equity, int periodCount, int smaPeriodCountK, int smaPeriodCountD, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<Stochastics.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startTime, endTime);

        public static TimeSeries<Stochastics.IndicatorResult> SlowSto(this Equity equity, int periodCount, int smaPeriodCountD, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<Stochastics.Slow>(periodCount, smaPeriodCountD).Compute(startTime, endTime);

        public static TimeSeries<Aroon.IndicatorResult> Aroon(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<Aroon>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<AroonOscillator.IndicatorResult> AroonOsc(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<AroonOscillator>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<ChandelierExit.IndicatorResult> Chandlr(this Equity equity, int periodCount, int atrCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<ChandelierExit>(periodCount, atrCount).Compute(startTime, endTime);

        public static TimeSeries<ClosePriceChange.IndicatorResult> ClosePriceChange(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<ClosePriceChange>().Compute(startTime, endTime);

        public static TimeSeries<ClosePricePercentageChange.IndicatorResult> ClosePricePercentageChange(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<ClosePricePercentageChange>().Compute(startTime, endTime);

        public static TimeSeries<EfficiencyRatio.IndicatorResult> Er(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<EfficiencyRatio>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<IchimokuCloud.IndicatorResult> Ichimoku(this Equity equity, int shortPeriodCount, int middlePeriodCount, int longPeriodCount, Country? country = null, DateTime? startTime = null, DateTime? endTime = null)
        {
            var ic = equity.GetOrCreateAnalytic<IchimokuCloud>(shortPeriodCount, middlePeriodCount, longPeriodCount);
            if (country.HasValue)
                ic.InitWithCountry(country.Value);
            return ic.Compute(startTime, endTime);
        }

        public static TimeSeries<KaufmanAdaptiveMovingAverage.IndicatorResult> Kama(this Equity equity, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<KaufmanAdaptiveMovingAverage>(periodCount, emaFastPeriodCount, emaSlowPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<ModifiedExponentialMovingAverage.IndicatorResult> Mema(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<ModifiedExponentialMovingAverage>(periodCount).Compute(startTime, endTime);

        public static TimeSeries<StandardDeviation.IndicatorResult> Sd(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => equity.GetOrCreateAnalytic<StandardDeviation>(periodCount).Compute(startTime, endTime);
    }
}