using System;
using System.Collections.Generic;

using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Extension
{
    public static class TuplesExtension
    {
        public static IReadOnlyList<decimal?> Func<TInput>(this IEnumerable<TInput> inputs, Func<IReadOnlyList<TInput>, int, IReadOnlyList<decimal>, IAnalyzeContext<TInput>, decimal?> func, params decimal[] parameters)
	        => func.AsAnalyzable(inputs, parameters).Compute();

        public static IReadOnlyList<decimal?> Func<TInput>(this IEnumerable<TInput> inputs, string name, params decimal[] parameters)
			=> FuncAnalyzableFactory.CreateAnalyzable<TInput, decimal?>(name, inputs, parameters).Compute();

        public static IReadOnlyList<decimal?> AccumDist(this IEnumerable<(decimal High, decimal Low, decimal Close, decimal Volume)> inputs, int? startIndex = null, int? endIndex = null)
             => new AccumulationDistributionLineByTuple(inputs).Compute(startIndex, endIndex);

        public static IReadOnlyList<(decimal? Up, decimal? Down)> Aroon(this IEnumerable<(decimal High, decimal Low)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AroonByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> AroonOsc(this IEnumerable<(decimal High, decimal Low)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AroonOscillatorByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Adx(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AverageDirectionalIndexByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Adxr(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int adxrPeriodCount, int? startIndex = null, int? endIndex = null)
            => new AverageDirectionalIndexRatingByTuple(inputs, periodCount, adxrPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Atr(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new AverageTrueRangeByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<(decimal? LowerBand, decimal? MiddleBand, decimal? UpperBand)> Bb(this IEnumerable<decimal> inputs, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBandsByTuple(inputs, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> BbWidth(this IEnumerable<decimal> inputs, int periodCount, decimal sdCount, int? startIndex = null, int? endIndex = null)
            => new BollingerBandWidthByTuple(inputs, periodCount, sdCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<(decimal? Long, decimal? Short)> Chandlr(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, decimal atrCount, int? startIndex = null, int? endIndex = null)
            => new ChandelierExitByTuple(inputs, periodCount, atrCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Diff(this IEnumerable<decimal> inputs, int numberOfDays = 1, int? startIndex = null, int? endIndex = null)
            => new DifferenceByTuple(inputs, numberOfDays).Compute(startIndex, endIndex);

		public static IReadOnlyList<decimal?> Diff(this IEnumerable<decimal?> inputs, int numberOfDays = 1, int? startIndex = null, int? endIndex = null)
	        => new DifferenceByTuple(inputs, numberOfDays).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> PcDiff(this IEnumerable<decimal> inputs, int numberOfDays = 1, int? startIndex = null, int? endIndex = null)
            => new PercentageDifferenceByTuple(inputs, numberOfDays).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> PcDiff(this IEnumerable<decimal?> inputs, int numberOfDays = 1, int? startIndex = null, int? endIndex = null)
	        => new PercentageDifferenceByTuple(inputs, numberOfDays).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Dmi(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new DirectionalMovementIndexByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Dymoi(this IEnumerable<decimal?> inputs, int sdPeriod, int smoothedSdPeriod, int rsiPeriod, int upLimit, int lowLimit, int? startIndex = null, int? endIndex = null)
            => new DynamicMomentumIndexByTuple(inputs, sdPeriod, smoothedSdPeriod, rsiPeriod, upLimit, lowLimit).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Er(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new EfficiencyRatioByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Ema(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverageByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<decimal?> Ema(this IEnumerable<decimal?> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new ExponentialMovingAverageByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> EmaOsc(this IEnumerable<decimal> inputs, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new ExponentialMovingAverageOscillatorByTuple(inputs, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Highest(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new HighestByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<decimal?> Highest(this IEnumerable<decimal?> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
	        => new HighestByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<(decimal? ConversionLine, decimal? BaseLine, decimal? LeadingSpanA, decimal? LeadingSpanB, decimal? LaggingSpan)> Ichimoku(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int shortPeriodCount, int middlePeriodCount, int longPeriodCount, int? startIndex = null, int? endIndex = null)
            => new IchimokuCloudByTuple(inputs, shortPeriodCount, middlePeriodCount, longPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Kama(this IEnumerable<decimal> inputs, int periodCount, int emaFastPeriodCount, int emaSlowPeriodCount, int? startIndex = null, int? endIndex = null)
            => new KaufmanAdaptiveMovingAverageByTuple(inputs, periodCount, emaFastPeriodCount, emaSlowPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Lowest(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new LowestByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<decimal?> Lowest(this IEnumerable<decimal?> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
	        => new LowestByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Mdi(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalIndicatorByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Mdm(this IEnumerable<decimal> inputs, int? startIndex = null, int? endIndex = null)
            => new MinusDirectionalMovementByTuple(inputs).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Mema(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new ModifiedMovingAverageByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<decimal?> Mema(this IEnumerable<decimal?> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new ModifiedMovingAverageByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<(decimal? MacdLine, decimal? SignalLine, decimal? MacdHistogram)> Macd(this IEnumerable<decimal> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int? startIndex = null, int? endIndex = null)
            => new MovingAverageConvergenceDivergenceByTuple(inputs, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> MacdHist(this IEnumerable<decimal> inputs, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int? startIndex = null, int? endIndex = null)
            => new MovingAverageConvergenceDivergenceHistogramByTuple(inputs, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Obv(this IEnumerable<(decimal Close, decimal Volume)> inputs, int? startIndex = null, int? endIndex = null)
            => new OnBalanceVolumeByTuple(inputs).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Pdi(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalIndicatorByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Pdm(this IEnumerable<decimal> inputs, int? startIndex = null, int? endIndex = null)
            => new PlusDirectionalMovementByTuple(inputs).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Rm(this IEnumerable<decimal?> inputs, int rmiPeriod, int mtmPeriod, int? startIndex = null, int? endIndex = null)
            => new RelativeMomentumByTuple(inputs, rmiPeriod, mtmPeriod).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Rmi(this IEnumerable<decimal?> inputs, int rmiPeriod, int mtmPeriod, int? startIndex = null, int? endIndex = null)
            => new RelativeMomentumIndexByTuple(inputs, rmiPeriod, mtmPeriod).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Rsv(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RawStochasticsValueByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Rs(this IEnumerable<decimal?> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrengthByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Rsi(this IEnumerable<decimal?> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new RelativeStrengthIndexByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<decimal?> Sma(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
	        => new SimpleMovingAverageByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Sma(this IEnumerable<decimal?> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverageByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> SmaOsc(this IEnumerable<decimal> inputs, int periodCount1, int periodCount2, int? startIndex = null, int? endIndex = null)
            => new SimpleMovingAverageOscillatorByTuple(inputs, periodCount1, periodCount2).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Sd(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new StandardDeviationByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<decimal?> Sd(this IEnumerable<decimal?> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
			=> new StandardDeviationByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<(decimal? K, decimal? D, decimal? J)> FastSto(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCount, int? startIndex = null, int? endIndex = null)
            => new Stochastics.FastByTuple(inputs, periodCount, smaPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<(decimal? K, decimal? D, decimal? J)> FullSto(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.FullByTuple(inputs, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IReadOnlyList<(decimal? K, decimal? D, decimal? J)> SlowSto(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new Stochastics.SlowByTuple(inputs, periodCount, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> FastStoOsc(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCount, int? startIndex = null, int? endIndex = null)
            => new StochasticsOscillator.FastByTuple(inputs, periodCount, smaPeriodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> FullStoOsc(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new StochasticsOscillator.FullByTuple(inputs, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> SlowStoOsc(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount, int smaPeriodCountD, int? startIndex = null, int? endIndex = null)
            => new StochasticsOscillator.SlowByTuple(inputs, periodCount, smaPeriodCountD).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Tr(this IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int? startIndex = null, int? endIndex = null)
            => new TrueRangeByTuple(inputs).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Median(this IEnumerable<decimal> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
            => new MedianByTuple(inputs, periodCount).Compute(startIndex, endIndex);

		public static IReadOnlyList<decimal?> Median(this IEnumerable<decimal?> inputs, int periodCount, int? startIndex = null, int? endIndex = null)
	        => new MedianByTuple(inputs, periodCount).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Percentile(this IEnumerable<decimal> inputs, int periodCount, decimal percent, int? startIndex = null, int? endIndex = null)
            => new PercentileByTuple(inputs, periodCount, percent).Compute(startIndex, endIndex);

		public static IReadOnlyList<decimal?> Percentile(this IEnumerable<decimal?> inputs, int periodCount, decimal percent, int? startIndex = null, int? endIndex = null)
	        => new PercentileByTuple(inputs, periodCount, percent).Compute(startIndex, endIndex);

        public static IReadOnlyList<decimal?> Sar(this IEnumerable<(decimal High, decimal Low)> inputs, decimal step, decimal maximumStep, int? startIndex = null, int? endIndex = null)
            => new ParabolicStopAndReverseByTuple(inputs, step, maximumStep).Compute(startIndex, endIndex);
    }
}
