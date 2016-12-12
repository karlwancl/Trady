using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public static class EquityExtension
    {
        public static TimeSeries<AccumulationDistributionLine.IndicatorResult> AccumDist(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => new AccumulationDistributionLine(equity).Compute(startTime, endTime);

        public static TimeSeries<DirectionalMovementIndex.IndicatorResult> Adx(this Equity equity, int periodCount, int adxrPeriodCount = 0, DateTime? startTime = null, DateTime? endTime = null)
            => new DirectionalMovementIndex(equity, periodCount, adxrPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<AverageTrueRange.IndicatorResult> Atr(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new AverageTrueRange(equity, periodCount).Compute(startTime, endTime);

        public static TimeSeries<BollingerBands.IndicatorResult> Bb(this Equity equity, int periodCount, int sdCount, DateTime? startTime = null, DateTime? endTime = null)
            => new BollingerBands(equity, periodCount, sdCount).Compute(startTime, endTime);

        public static TimeSeries<ClosePriceChange.IndicatorResult> PriceChange(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => new ClosePriceChange(equity).Compute(startTime, endTime);

        public static TimeSeries<ExponentialMovingAverage.IndicatorResult> Ema(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new ExponentialMovingAverage(equity, periodCount).Compute(startTime, endTime);

        public static TimeSeries<ExponentialMovingAverageOscillator.IndicatorResult> EmaOsc(this Equity equity, int periodCount1, int periodCount2, DateTime? startTime = null, DateTime? endTime = null)
            => new ExponentialMovingAverageOscillator(equity, periodCount1, periodCount2).Compute(startTime, endTime);

        public static TimeSeries<HighestHigh.IndicatorResult> HighestHigh(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new HighestHigh(equity, periodCount).Compute(startTime, endTime);

        public static TimeSeries<LowestLow.IndicatorResult> LowestLow(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new LowestLow(equity, periodCount).Compute(startTime, endTime);

        public static TimeSeries<MovingAverageConvergenceDivergence.IndicatorResult> Macd(this Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new MovingAverageConvergenceDivergence(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<OnBalanceVolume.IndicatorResult> Obv(this Equity equity, DateTime? startTime = null, DateTime? endTime = null)
            => new OnBalanceVolume(equity).Compute(startTime, endTime);

        public static TimeSeries<RawStochasticsValue.IndicatorResult> Rsv(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new RawStochasticsValue(equity, periodCount).Compute(startTime, endTime);

        public static TimeSeries<RelativeStrength.IndicatorResult> Rs(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new RelativeStrength(equity, periodCount).Compute(startTime, endTime);

        public static TimeSeries<RelativeStrengthIndex.IndicatorResult> Rsi(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new RelativeStrengthIndex(equity, periodCount).Compute(startTime, endTime);

        public static TimeSeries<SimpleMovingAverage.IndicatorResult> Sma(this Equity equity, int periodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new SimpleMovingAverage(equity, periodCount).Compute(startTime, endTime);

        public static TimeSeries<SimpleMovingAverageOscillator.IndicatorResult> SmaOsc(this Equity equity, int periodCount1, int periodCount2, DateTime? startTime = null, DateTime? endTime = null)
            => new SimpleMovingAverageOscillator(equity, periodCount1, periodCount2).Compute(startTime, endTime);

        public static TimeSeries<Stochastics.IndicatorResult> FastSto(this Equity equity, int periodCount, int smaPeriodCount, DateTime? startTime = null, DateTime? endTime = null)
            => new Stochastics.Fast(equity, periodCount, smaPeriodCount).Compute(startTime, endTime);

        public static TimeSeries<Stochastics.IndicatorResult> FullSto(this Equity equity, int periodCount, int smaPeriodCountK, int smaPeriodCountD, DateTime? startTime = null, DateTime? endTime = null)
            => new Stochastics.Full(equity, periodCount, smaPeriodCountK, smaPeriodCountD).Compute(startTime, endTime);

        public static TimeSeries<Stochastics.IndicatorResult> SlowSto(this Equity equity, int periodCount, int smaPeriodCountD, DateTime? startTime = null, DateTime? endTime = null)
            => new Stochastics.Slow(equity, periodCount, smaPeriodCountD).Compute(startTime, endTime);
    }
}
