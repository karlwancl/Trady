using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Period;

namespace Trady.Analysis.Indicator
{
    public static class CandlesExtension
    {
        public static IList<decimal?> AccumDist(this IList<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new AccumulationDistributionLine(candles).Compute(startIndex, endIndex);

        public static IList<(decimal? Up, decimal? Down)> Aroon(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new Aroon(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> AroonOsc(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AroonOscillator(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Adx(IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new DirectionalMovementIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Adxr(this IList<Candle> candles, int periodCount, int adxrPeriodCount, int? startIndex = null, int? endIndex = null)
            => new AverageDirectionalIndexRating(candles, periodCount, adxrPeriodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Atr(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AverageTrueRange(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<(decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)> Bb(this IList<Candle> candles, int periodCount, int sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBands(candles, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IList<decimal?> BbWidth(this IList<Candle> candles, int periodCount, int sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBandWidth(candles, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IList<(decimal? Long, decimal? Short)> Chandlr(this IList<Candle> candles, int periodCount, int atrCount, int? startIndex = null, int? endIndex = null)
            => new ChandelierExit(candles, periodCount, atrCount).Compute(startIndex, endIndex);

        public static IList<decimal?> ClosePriceChange(this IList<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new ClosePriceChange(candles).Compute(startIndex, endIndex);

        public static IList<decimal?> ClosePricePercentageChange(this IList<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new ClosePricePercentageChange(candles).Compute(startIndex, endIndex);

        public static IList<decimal?> Dmi(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new DirectionalMovementIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Er(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new EfficiencyRatio(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Ema(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> EmaOsc(this IList<Candle> candles, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverageOscillator(candles, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IList<decimal?> HighestHigh(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new HighestHigh(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<(decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)> Ichimoku(this IList<Candle> candles, int shortPeriodCount, int middlePeriodCount, int longPeriodCount, Country? country = null, int? startIndex = null, int? endIndex = null)
            => new IchimokuCloud(candles, shortPeriodCount, middlePeriodCount, longPeriodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Kama(this IList<Candle> candles, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount, int? startIndex = null, int? endIndex = null)
            => new KaufmanAdaptiveMovingAverage(candles, periodCount, emaFastPeriodCount, emaSlowPeriodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> LowestLow(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new LowestLow(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Mdi(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalIndicator(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Mdm(this IList<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalMovement(candles).Compute(startIndex, endIndex);

        public static IList<decimal?> Mema(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ModifiedExponentialMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<(decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)> Macd(this IList<Candle> candles, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int? startIndex = null, int? endIndex = null)
            => new MovingAverageConvergenceDivergence(candles, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Obv(this IList<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new OnBalanceVolume(candles).Compute(startIndex, endIndex);

        public static IList<decimal?> Pdi(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalIndicator(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Pdm(this IList<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalMovement(candles).Compute(startIndex, endIndex);

        public static IList<decimal?> Rsv(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RawStochasticsValue(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Rs(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrength(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Rsi(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrengthIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> Sma(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<decimal?> SmaOsc(this IList<Candle> candles, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverageOscillator(candles, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IList<decimal?> Sd(this IList<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new StandardDeviation(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<(decimal? K, decimal? D, decimal? J)> FastSto(this IList<Candle> candles, int periodCount, int smaPeriodCount, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Fast(candles, periodCount, smaPeriodCount).Compute(startIndex, endIndex);

        public static IList<(decimal? K, decimal? D, decimal? J)> FullSto(this IList<Candle> candles, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Full(candles, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IList<(decimal? K, decimal? D, decimal? J)> SlowSto(this IList<Candle> candles, int periodCount, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Slow(candles, periodCount, smaPeriodCountD).Compute(startIndex, endIndex);
    }
}