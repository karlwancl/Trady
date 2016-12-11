using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public static class IndicatorExtension
    {
        public static TimeSeries<AccumulationDistributionLine.IndicatorResult> AccumDist(this Equity equity)
            => new AccumulationDistributionLine(equity).Compute();

        public static TimeSeries<DirectionalMovementIndex.IndicatorResult> Adx(this Equity equity, int periodCount, int adxrPeriodCount = 0)
            => new DirectionalMovementIndex(equity, periodCount, adxrPeriodCount).Compute();

        public static TimeSeries<AverageTrueRange.IndicatorResult> Atr(this Equity equity, int periodCount)
            => new AverageTrueRange(equity, periodCount).Compute();

        public static TimeSeries<BollingerBands.IndicatorResult> Bb(this Equity equity, int periodCount, int sdCount)
            => new BollingerBands(equity, periodCount, sdCount).Compute();

        public static TimeSeries<ClosePriceChange.IndicatorResult> PriceChange(this Equity equity)
            => new ClosePriceChange(equity).Compute();

        public static TimeSeries<ExponentialMovingAverage.IndicatorResult> Ema(this Equity equity, int periodCount)
            => new ExponentialMovingAverage(equity, periodCount).Compute();

        public static TimeSeries<ExponentialMovingAverageOscillator.IndicatorResult> EmaOsc(this Equity equity, int periodCount1, int periodCount2)
            => new ExponentialMovingAverageOscillator(equity, periodCount1, periodCount2).Compute();

        public static TimeSeries<HighestHigh.IndicatorResult> HighestHigh(this Equity equity, int periodCount)
            => new HighestHigh(equity, periodCount).Compute();

        public static TimeSeries<LowestLow.IndicatorResult> LowestLow(this Equity equity, int periodCount)
            => new LowestLow(equity, periodCount).Compute();

        public static TimeSeries<MovingAverageConvergenceDivergence.IndicatorResult> Macd(this Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => new MovingAverageConvergenceDivergence(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute();

        public static TimeSeries<OnBalanceVolume.IndicatorResult> Obv(this Equity equity)
            => new OnBalanceVolume(equity).Compute();

        public static TimeSeries<RawStochasticsValue.IndicatorResult> Rsv(this Equity equity, int periodCount)
            => new RawStochasticsValue(equity, periodCount).Compute();

        public static TimeSeries<RelativeStrength.IndicatorResult> Rs(this Equity equity, int periodCount)
            => new RelativeStrength(equity, periodCount).Compute();

        public static TimeSeries<RelativeStrengthIndex.IndicatorResult> Rsi(this Equity equity, int periodCount)
            => new RelativeStrengthIndex(equity, periodCount).Compute();

        public static TimeSeries<SimpleMovingAverage.IndicatorResult> Sma(this Equity equity, int periodCount)
            => new SimpleMovingAverage(equity, periodCount).Compute();

        public static TimeSeries<SimpleMovingAverageOscillator.IndicatorResult> SmaOsc(this Equity equity, int periodCount1, int periodCount2)
            => new SimpleMovingAverageOscillator(equity, periodCount1, periodCount2).Compute();

        public static TimeSeries<Stochastics.IndicatorResult> FastSto(this Equity equity, int periodCount, int smaPeriodCount)
            => new Stochastics.Fast(equity, periodCount, smaPeriodCount).Compute();

        public static TimeSeries<Stochastics.IndicatorResult> FullSto(this Equity equity, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => new Stochastics.Full(equity, periodCount, smaPeriodCountK, smaPeriodCountD).Compute();

        public static TimeSeries<Stochastics.IndicatorResult> SlowSto(this Equity equity, int periodCount, int smaPeriodCountD)
            => new Stochastics.Slow(equity, periodCount, smaPeriodCountD).Compute();
    }
}
