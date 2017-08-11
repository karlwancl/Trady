using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Period;

namespace Trady.Analysis.Indicator
{
    public static class CandlesExtension
    {
        #region Candles

        public static IList<AnalyzableTick<decimal?>> AccumDist(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new AccumulationDistributionLine(candles).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<(decimal? Up, decimal? Down)>> Aroon(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new Aroon(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> AroonOsc(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AroonOscillator(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Adx(IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AverageDirectionalIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Adxr(this IEnumerable<Candle> candles, int periodCount, int adxrPeriodCount, int? startIndex = null, int? endIndex = null)
            => new AverageDirectionalIndexRating(candles, periodCount, adxrPeriodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Atr(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AverageTrueRange(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<(decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)>> Bb(this IEnumerable<Candle> candles, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBands(candles, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> BbWidth(this IEnumerable<Candle> candles, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBandWidth(candles, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<(decimal? Long, decimal? Short)>> Chandlr(this IEnumerable<Candle> candles, int periodCount, decimal atrCount, int? startIndex = null, int? endIndex = null)
            => new ChandelierExit(candles, periodCount, atrCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> ClosePriceChange(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new ClosePriceChange(candles).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> ClosePriceChange(this IEnumerable<Candle> candles, int numberOfDays, int? startIndex = null, int? endIndex = null)
            => new ClosePriceChange(candles, numberOfDays).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> ClosePricePercentageChange(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new ClosePricePercentageChange(candles).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> ClosePricePercentageChange(this IEnumerable<Candle> candles, int numberOfDays, int? startIndex = null, int? endIndex = null)
            => new ClosePricePercentageChange(candles, numberOfDays).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Dmi(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new DirectionalMovementIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Er(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new EfficiencyRatio(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Ema(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> EmaOsc(this IEnumerable<Candle> candles, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverageOscillator(candles, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> HighestHigh(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new HighestHigh(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<(decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)>> Ichimoku(this IEnumerable<Candle> candles, int shortPeriodCount, int middlePeriodCount, int longPeriodCount, int? startIndex = null, int? endIndex = null)
            => new IchimokuCloud(candles, shortPeriodCount, middlePeriodCount, longPeriodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Kama(this IEnumerable<Candle> candles, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount, int? startIndex = null, int? endIndex = null)
            => new KaufmanAdaptiveMovingAverage(candles, periodCount, emaFastPeriodCount, emaSlowPeriodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> LowestLow(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new LowestLow(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Mdi(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalIndicator(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Mdm(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalMovement(candles).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Mema(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ModifiedExponentialMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<(decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)>> Macd(this IEnumerable<Candle> candles, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int? startIndex = null, int? endIndex = null)
            => new MovingAverageConvergenceDivergence(candles, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Obv(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new OnBalanceVolume(candles).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Pdi(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalIndicator(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Pdm(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalMovement(candles).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Rsv(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RawStochasticsValue(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Rs(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrength(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Rsi(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrengthIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Sma(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> SmaOsc(this IEnumerable<Candle> candles, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverageOscillator(candles, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Sd(this IEnumerable<Candle> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new StandardDeviation(candles, periodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<(decimal? K, decimal? D, decimal? J)>> FastSto(this IEnumerable<Candle> candles, int periodCount, int smaPeriodCount, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Fast(candles, periodCount, smaPeriodCount).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<(decimal? K, decimal? D, decimal? J)>> FullSto(this IEnumerable<Candle> candles, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Full(candles, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<(decimal? K, decimal? D, decimal? J)>> SlowSto(this IEnumerable<Candle> candles, int periodCount, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Slow(candles, periodCount, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IList<AnalyzableTick<decimal?>> Tr(this IEnumerable<Candle> candles, int? startIndex = null, int? endIndex = null)
            => new TrueRange(candles).Compute(startIndex, endIndex);

		#endregion

		#region Tuples

		public static IList<decimal?> AccumDist(this IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> inputs, int? startIndex = null, int? endIndex = null)
	        => new AccumulationDistributionLineByTuple(inputs).Compute(startIndex, endIndex);

		public static IList<(decimal? Up, decimal? Down)> Aroon(this IEnumerable<(decimal High, decimal Low)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new AroonByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> AroonOsc(this IEnumerable<(decimal High, decimal Low)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new AroonOscillatorByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Adx(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new AverageDirectionalIndexByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Adxr(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int adxrPeriodCount, int? startIndex = null, int? endIndex = null)
			=> new AverageDirectionalIndexRatingByTuple(inputs, periodCount, adxrPeriodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Atr(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new AverageTrueRangeByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<(decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)> Bb(this IEnumerable<decimal> inputs, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
			=> new BollingerBandsByTuple(inputs, periodCount, sdCount).Compute(startIndex, endIndex);

		public static IList<decimal?> BbWidth(this IEnumerable<decimal> inputs, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
			=> new BollingerBandWidthByTuple(inputs, periodCount, sdCount).Compute(startIndex, endIndex);

		public static IList<(decimal? Long, decimal? Short)> Chandlr(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, decimal atrCount, int? startIndex = null, int? endIndex = null)
			=> new ChandelierExitByTuple(inputs, periodCount, atrCount).Compute(startIndex, endIndex);

		public static IList<decimal?> ClosePriceChange(this IEnumerable<decimal> inputs, int? startIndex = null, int? endIndex = null)
			=> new ClosePriceChangeByTuple(inputs).Compute(startIndex, endIndex);

		public static IList<decimal?> ClosePriceChange(this IEnumerable<decimal> inputs, int numberOfDays, int? startIndex = null, int? endIndex = null)
			=> new ClosePriceChangeByTuple(inputs, numberOfDays).Compute(startIndex, endIndex);

		public static IList<decimal?> ClosePricePercentageChange(this IEnumerable<decimal> inputs, int? startIndex = null, int? endIndex = null)
			=> new ClosePricePercentageChangeByTuple(inputs).Compute(startIndex, endIndex);

		public static IList<decimal?> ClosePricePercentageChange(this IEnumerable<decimal> inputs, int numberOfDays, int? startIndex = null, int? endIndex = null)
			=> new ClosePricePercentageChangeByTuple(inputs, numberOfDays).Compute(startIndex, endIndex);

		public static IList<decimal?> Dmi(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new DirectionalMovementIndexByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Er(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new EfficiencyRatioByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Ema(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new ExponentialMovingAverageByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> EmaOsc(this IEnumerable<decimal> inputs, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
			=> new ExponentialMovingAverageOscillatorByTuple(inputs, periodCount1, periodCount2).Compute(startIndex, endIndex);

		public static IList<decimal?> HighestHigh(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new HighestHighByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<(decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)> Ichimoku(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int shortPeriodCount, int middlePeriodCount, int longPeriodCount, int? startIndex = null, int? endIndex = null)
			=> new IchimokuCloudByTuple(inputs, shortPeriodCount, middlePeriodCount, longPeriodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Kama(this IEnumerable<decimal> inputs, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount, int? startIndex = null, int? endIndex = null)
			=> new KaufmanAdaptiveMovingAverageByTuple(inputs, periodCount, emaFastPeriodCount, emaSlowPeriodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> LowestLow(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new LowestLowByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Mdi(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new MinusDirectionalIndicatorByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Mdm(this IEnumerable<decimal> inputs, int? startIndex = null, int? endIndex = null)
			=> new MinusDirectionalMovementByTuple(inputs).Compute(startIndex, endIndex);

		public static IList<decimal?> Mema(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new ModifiedExponentialMovingAverageByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<(decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)> Macd(this IEnumerable<decimal> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int? startIndex = null, int? endIndex = null)
			=> new MovingAverageConvergenceDivergenceByTuple(inputs, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Obv(this IEnumerable<(decimal Close, decimal Volume)> inputs, int? startIndex = null, int? endIndex = null)
			=> new OnBalanceVolumeByTuple(inputs).Compute(startIndex, endIndex);

        public static IList<decimal?> Pdi(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new PlusDirectionalIndicatorByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Pdm(this IEnumerable<decimal> inputs, int? startIndex = null, int? endIndex = null)
			=> new PlusDirectionalMovementByTuple(inputs).Compute(startIndex, endIndex);

        public static IList<decimal?> Rsv(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new RawStochasticsValueByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Rs(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new RelativeStrengthByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Rsi(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new RelativeStrengthIndexByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> Sma(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new SimpleMovingAverageByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<decimal?> SmaOsc(this IEnumerable<decimal> inputs, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
			=> new SimpleMovingAverageOscillatorByTuple(inputs, periodCount1, periodCount2).Compute(startIndex, endIndex);

		public static IList<decimal?> Sd(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new StandardDeviationByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IList<(decimal? K, decimal? D, decimal? J)> FastSto(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCount, int? startIndex = null, int? endIndex = null)
			=> new Stochastics.FastByTuple(inputs, periodCount, smaPeriodCount).Compute(startIndex, endIndex);

		public static IList<(decimal? K, decimal? D, decimal? J)> FullSto(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
			=> new Stochastics.FullByTuple(inputs, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startIndex, endIndex);

		public static IList<(decimal? K, decimal? D, decimal? J)> SlowSto(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
			=> new Stochastics.SlowByTuple(inputs, periodCount, smaPeriodCountD).Compute(startIndex, endIndex);

		public static IList<decimal?> Tr(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int? startIndex = null, int? endIndex = null)
			=> new TrueRangeByTuple(inputs).Compute(startIndex, endIndex);

        #endregion  
    }
}