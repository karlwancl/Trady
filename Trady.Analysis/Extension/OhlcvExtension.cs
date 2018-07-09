using System;
using System.Collections.Generic;

using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Extension
{
    public static class OhlcvExtension
    {
        public static IReadOnlyList<AnalyzableTick<decimal?>> Func(this IEnumerable<IOhlcv> candles, Func<IReadOnlyList<IOhlcv>, int, IReadOnlyList<decimal>, IAnalyzeContext<IOhlcv>, decimal?> func, params decimal[] parameters)
            => func.AsAnalyzable(candles, parameters).Compute();

        public static IReadOnlyList<IAnalyzableTick<decimal?>> Func(this IEnumerable<IOhlcv> candles, string name, params decimal[] parameters)
            => FuncAnalyzableFactory.CreateAnalyzable(name, candles, parameters).Compute();

        public static IReadOnlyList<AnalyzableTick<decimal?>> AccumDist(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new AccumulationDistributionLine(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? Up, decimal? Down)>> Aroon(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new Aroon(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> AroonOsc(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AroonOscillator(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Adx(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AverageDirectionalIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Adxr(this IEnumerable<IOhlcv> candles, int periodCount, int adxrPeriodCount, int? startIndex = null, int? endIndex = null)
            => new AverageDirectionalIndexRating(candles, periodCount, adxrPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Atr(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AverageTrueRange(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)>> Bb(this IEnumerable<IOhlcv> candles, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBands(candles, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> BbWidth(this IEnumerable<IOhlcv> candles, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBandWidth(candles, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Cci(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new CommodityChannelIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? Long, decimal? Short)>> Chandlr(this IEnumerable<IOhlcv> candles, int periodCount, decimal atrCount, int? startIndex = null, int? endIndex = null)
            => new ChandelierExit(candles, periodCount, atrCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Mtm(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new Momentum(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Mtm(this IEnumerable<IOhlcv> candles, int numberOfDays, int? startIndex = null, int? endIndex = null)
            => new Momentum(candles, numberOfDays).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Roc(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new RateOfChange(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Roc(this IEnumerable<IOhlcv> candles, int numberOfDays, int? startIndex = null, int? endIndex = null)
            => new RateOfChange(candles, numberOfDays).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Dmi(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new DirectionalMovementIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Dymoi(this IEnumerable<IOhlcv> candles, int sdPeriod, int smoothedSdPeriod, int rsiPeriod, int upLimit, int lowLimit, int? startIndex = null, int? endIndex = null)
            => new DynamicMomentumIndex(candles, sdPeriod, smoothedSdPeriod, rsiPeriod, upLimit, lowLimit).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Er(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new EfficiencyRatio(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Ema(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> EmaOsc(this IEnumerable<IOhlcv> candles, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverageOscillator(candles, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HighHigh(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new HighestHigh(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HistHighHigh(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new HistoricalHighestHigh(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HistHighClose(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new HistoricalHighestClose(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HighClose(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new HighestClose(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)>> Ichimoku(this IEnumerable<IOhlcv> candles, int shortPeriodCount, int middlePeriodCount, int longPeriodCount, int? startIndex = null, int? endIndex = null)
            => new IchimokuCloud(candles, shortPeriodCount, middlePeriodCount, longPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Kama(this IEnumerable<IOhlcv> candles, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount, int? startIndex = null, int? endIndex = null)
            => new KaufmanAdaptiveMovingAverage(candles, periodCount, emaFastPeriodCount, emaSlowPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> LowLow(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new LowestLow(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HistLowLow(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new HistoricalLowestLow(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> HistLowClose(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new HistoricalLowestClose(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> LowClose(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new LowestClose(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Mdi(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalIndicator(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Mdm(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalMovement(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Mma(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ModifiedMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)>> Macd(this IEnumerable<IOhlcv> candles, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int? startIndex = null, int? endIndex = null)
            => new MovingAverageConvergenceDivergence(candles, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> MacdHist(this IEnumerable<IOhlcv> candles, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int? startIndex = null, int? endIndex = null)
	        => new MovingAverageConvergenceDivergenceHistogram(candles, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Nmo(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new NetMomentumOscillator(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Obv(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new OnBalanceVolume(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Pdi(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalIndicator(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Pdm(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalMovement(candles).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Rm(this IEnumerable<IOhlcv> candles, int rmiPeriod, int mtmPeriod, int? startIndex = null, int? endIndex = null)
            => new RelativeMomentum(candles, rmiPeriod, mtmPeriod).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Rmi(this IEnumerable<IOhlcv> candles, int rmiPeriod, int mtmPeriod, int? startIndex = null, int? endIndex = null)
            => new RelativeMomentumIndex(candles, rmiPeriod, mtmPeriod).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Rsv(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RawStochasticsValue(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Rs(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrength(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Rsi(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrengthIndex(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Sma(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverage(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> SmaOsc(this IEnumerable<IOhlcv> candles, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverageOscillator(candles, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Sd(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new StandardDeviation(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? K, decimal? D, decimal? J)>> FastSto(this IEnumerable<IOhlcv> candles, int periodCount, int smaPeriodCount, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Fast(candles, periodCount, smaPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? K, decimal? D, decimal? J)>> FullSto(this IEnumerable<IOhlcv> candles, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Full(candles, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<(decimal? K, decimal? D, decimal? J)>> SlowSto(this IEnumerable<IOhlcv> candles, int periodCount, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.Slow(candles, periodCount, smaPeriodCountD).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> FastStoOsc(this IEnumerable<IOhlcv> candles, int periodCount, int smaPeriodCount, int? startIndex = null, int? endIndex = null)
	        => new StochasticsOscillator.Fast(candles, periodCount, smaPeriodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> FullStoOsc(this IEnumerable<IOhlcv> candles, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
			=> new StochasticsOscillator.Full(candles, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> SlowStoOsc(this IEnumerable<IOhlcv> candles, int periodCount, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
			=> new StochasticsOscillator.Slow(candles, periodCount, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> StochRsi(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
            => new StochasticsRsiOscillator(candles, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Tr(this IEnumerable<IOhlcv> candles, int? startIndex = null, int? endIndex = null)
            => new TrueRange(candles).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Median(this IEnumerable<IOhlcv> candles, int periodCount, int? startIndex = null, int? endIndex = null)
	        => new Median(candles, periodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<AnalyzableTick<decimal?>> Percentile(this IEnumerable<IOhlcv> candles, int periodCount, decimal percent, int? startIndex = null, int? endIndex = null)
	        => new Percentile(candles, periodCount, percent).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Sar(this IEnumerable<IOhlcv> candles, decimal step, decimal maximumStep, int? startIndex = null, int? endIndex = null)
            => new ParabolicStopAndReverse(candles, step, maximumStep).Compute(startIndex, endIndex);

        public static IReadOnlyList<AnalyzableTick<decimal?>> Smi(this IEnumerable<IOhlcv> candles, int periodCount, int smoothingPeriodA, int smoothingPeriodB, int? startIndex = null, int? endIndex = null)
            => new StochasticsMomentumIndex(candles, periodCount, smoothingPeriodA, smoothingPeriodB).Compute(startIndex, endIndex);
    }
}
