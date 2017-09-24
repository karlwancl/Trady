using System;
using System.Collections.Generic;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public static class CandlesExtension
    {
        public static IReadOnlyList<AnalyzableTick<decimal?>> Func(this IEnumerable<Candle> candles, Func<IReadOnlyList<Candle>, int, IAnalyzeContext<Candle>, decimal?> func, int? startIndex = null, int? endIndex = null)
            => func.AsAnalyzable(candles).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Func(this IEnumerable<Candle> candles, Func<IReadOnlyList<Candle>, int, decimal, IAnalyzeContext<Candle>, decimal?> func, decimal parameter, int? startIndex = null, int? endIndex = null)
	        => func.AsAnalyzable(candles, parameter).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Func(this IEnumerable<Candle> candles, Func<IReadOnlyList<Candle>, int, decimal ,decimal, IAnalyzeContext<Candle>, decimal?> func, decimal parameter0, decimal parameter1, int? startIndex = null, int? endIndex = null)
	        => func.AsAnalyzable(candles, parameter0, parameter1).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Func(this IEnumerable<Candle> candles, Func<IReadOnlyList<Candle>, int, decimal, decimal, decimal, IAnalyzeContext<Candle>, decimal?> func, decimal parameter0, decimal parameter1, decimal parameter2, int? startIndex = null, int? endIndex = null)
	        => func.AsAnalyzable(candles, parameter0, parameter1, parameter2).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Func(this IEnumerable<Candle> candles, string name, int? startIndex = null, int? endIndex = null)
            => FuncAnalyzableFactory.CreateAnalyzable(name, candles).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Func(this IEnumerable<Candle> inputs, string name, decimal parameter, int? startIndex = null, int? endIndex = null)
			=> FuncAnalyzableFactory.CreateAnalyzable(name, inputs, new object[] { parameter }).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Func(this IEnumerable<Candle> inputs, string name, decimal parameter0, decimal parameter1, int? startIndex = null, int? endIndex = null)
			=> FuncAnalyzableFactory.CreateAnalyzable(name, inputs, new object[] { parameter0, parameter1 }).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Func(this IEnumerable<Candle> inputs, string name, decimal parameter0, decimal parameter1, decimal parameter2, int? startIndex = null, int? endIndex = null)
			=> FuncAnalyzableFactory.CreateAnalyzable(name, inputs, new object[] { parameter0, parameter1, parameter2 }).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> AccumDist(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new AccumulationDistributionLine(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? Up, decimal? Down)>> Aroon(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new Aroon(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> AroonOsc(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AroonOscillator(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Adx(IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AverageDirectionalIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Adxr(this IEnumerable<Candle> candles, int periodCount, int adxrPeriodCount, int? startIndex = null, int? endIndex = null)
            => new AverageDirectionalIndexRating(candles, periodCount, adxrPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Atr(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AverageTrueRange(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)>> Bb(this IEnumerable<Candle> candles, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBands(candles, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> BbWidth(this IEnumerable<Candle> candles, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBandWidth(candles, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? Long, decimal? Short)>> Chandlr(this IEnumerable<Candle> candles, int periodCount, decimal atrCount, int? startIndex = null, int? endIndex = null)
            => new ChandelierExit(candles, periodCount, atrCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> CloseDiff(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new ClosePriceChange(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> CloseDiff(this IEnumerable<Candle> candles, int numberOfDays, int? startIndex = null, int? endIndex = null)
            => new ClosePriceChange(candles, numberOfDays).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> ClosePcDiff(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new ClosePricePercentageChange(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> ClosePcDiff(this IEnumerable<Candle> candles, int numberOfDays, int? startIndex = null, int? endIndex = null)
            => new ClosePricePercentageChange(candles, numberOfDays).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Dmi(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new DirectionalMovementIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Er(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new EfficiencyRatio(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Ema(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> EmaOsc(this IEnumerable<Candle> candles, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverageOscillator(candles, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HighHigh(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new HighestHigh(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HistHighHigh(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new HistoricalHighestHigh(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HistHighClose(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new HistoricalHighestClose(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HighClose(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new HighestClose(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)>> Ichimoku(this IEnumerable<Candle> candles, int shortPeriodCount, int middlePeriodCount, int longPeriodCount, int? startIndex = null, int? endIndex = null)
            => new IchimokuCloud(candles, shortPeriodCount, middlePeriodCount, longPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Kama(this IEnumerable<Candle> candles, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount, int? startIndex = null, int? endIndex = null)
            => new KaufmanAdaptiveMovingAverage(candles, periodCount, emaFastPeriodCount, emaSlowPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> LowLow(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new LowestLow(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HistLowLow(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new HistoricalLowestLow(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HistLowClose(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new HistoricalLowestClose(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> LowClose(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new LowestClose(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Mdi(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalIndicator(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Mdm(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalMovement(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Mma(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ModifiedMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)>> Macd(this IEnumerable<Candle> candles, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int? startIndex = null, int? endIndex = null)
            => new MovingAverageConvergenceDivergence(candles, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> MacdHist(this IEnumerable<Candle> candles, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int? startIndex = null, int? endIndex = null)
	        => new MovingAverageConvergenceDivergenceHistogram(candles, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Obv(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new OnBalanceVolume(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Pdi(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalIndicator(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Pdm(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalMovement(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Rsv(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RawStochasticsValue(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Rs(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrength(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Rsi(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrengthIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Sma(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> SmaOsc(this IEnumerable<Candle> candles, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverageOscillator(candles, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Sd(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new StandardDeviation(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? K, decimal? D, decimal? J)>> FastSto(this IEnumerable<Candle> candles, int periodCount, int smaPeriodCount, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Fast(candles, periodCount, smaPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? K, decimal? D, decimal? J)>> FullSto(this IEnumerable<Candle> candles, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Full(candles, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? K, decimal? D, decimal? J)>> SlowSto(this IEnumerable<Candle> candles, int periodCount, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Slow(candles, periodCount, smaPeriodCountD).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> FastStoOsc(this IEnumerable<Candle> candles, int periodCount, int smaPeriodCount, int? startIndex = null, int? endIndex = null)
	        => new StochasticsOscillator.Fast(candles, periodCount, smaPeriodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> FullStoOsc(this IEnumerable<Candle> candles, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
			=> new StochasticsOscillator.Full(candles, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> SlowStoOsc(this IEnumerable<Candle> candles, int periodCount, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
			=> new StochasticsOscillator.Slow(candles, periodCount, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Tr(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new TrueRange(candles).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Median(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
	        => new Median(candles, periodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Percentile(this IEnumerable<Candle> candles, int periodCount, decimal percent, int? startIndex = null, int? endIndex = null)
	        => new Percentile(candles, periodCount, percent).Compute(startIndex, endIndex);
    }
}