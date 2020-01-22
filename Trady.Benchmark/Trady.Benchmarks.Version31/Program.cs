using System;
using BenchmarkDotNet.Running;
using Trady.Core.Infrastructure;
using Trady.Importer.Csv;
using Trady.Analysis.Extension;
using Trady.Analysis;

using System.Linq;
using Trady.Core;
using Trady.Core.Period;
using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Columns;

namespace Trady.Benchmarks.Version31
{
    [Config(typeof(Config))]
    [CoreJob]
    public class Benchmark
    {
        private const int _n = 10000;
        private readonly IOhlcv[] _data;
        private readonly ITickTrade[] _tradeData;


        public Benchmark()
        {
            var config = new CsvImportConfiguration()
            {
                Culture = "en-US",
                Delimiter = ";",
                DateFormat = "yyyyMMdd HHmmss",
                HasHeaderRecord = false
            };
            var importer = new CsvImporter("Data/EURUSD.csv", config);
            _data = importer.ImportAsync("EURUSD").Result.ToArray();

            _tradeData = new ITickTrade[_n];
            var d = DateTimeOffset.Now;
            for(int i = 0; i < _n; i++)
            {                
                _tradeData[i] = new Trade(d.AddSeconds(i), 1, 1);
            }           

        }

        private class Config : ManualConfig
        {
            public Config()
            {
                Add(StatisticColumn.P90);
            }
        }        
     
        [Benchmark]
        public IReadOnlyList<IOhlcv> TransformToMonthly() => _data.Transform<PerMinute, Monthly>();

        [Benchmark]
        public IReadOnlyList<IOhlcv> TransformToDaily() => _data.Transform<PerMinute, Daily>();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> RelativeStrengthIndex() => _data.Rsi(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> ExponentialMovingAverage() => _data.Ema(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> SimpleMovingAverage() => _data.Sma(30);

        [Benchmark]
        public IReadOnlyList<Trady.Analysis.AnalyzableTick<(decimal?, decimal?, decimal?)>> MACD() => _data.Macd(12, 26, 9);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> CommodityChannelIndex() => _data.Cci(20);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> StochasticMomentumIndex() => _data.Smi(15, 6, 6);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> StochasticRelativeStrengthIndex() => _data.StochRsi(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> NetMomentumOscillator() => _data.Nmo(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> RelativeMomentum() => _data.Rm(20, 4);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> RelativeMomentumIndex() => _data.Rmi(20, 4);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> DynamicMomentumIndex() => _data.Dymoi(5, 10, 14, 30, 5);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> ParabolicStopAndReverse() => _data.Sar(0.02m, 0.2m);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Median() => _data.Median(20);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Percentile() => _data.Percentile(30, 0.7m);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> AccumulationDistributionLine() => _data.AccumDist(20);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?)>> Aroon() => _data.Aroon(25);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> AroonOscillator() => _data.AroonOsc(25);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> AverageTrueRange() => _data.Atr(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?)>> BollingerBands() => _data.Bb(20, 2);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> BollingerBandWidth() => _data.BbWidth(20, 2);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?)>> ChandelierExit() => _data.Chandlr(22, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Momentum() => _data.Mtm();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> RateOfChange() => _data.Roc();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> PlusDirectionalIndicator() => _data.Pdi(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> MinusDirectionalIndicator() => _data.Mdi(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> AverageDirectionalIndex() => _data.Adx(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> AverageDirectionalIndexRating() => _data.Adxr(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> EfficiencyRatio() => _data.Er(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> KaufmanAdaptiveMovingAverage() => _data.Kama(10, 2, 30);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> ExponentialMovingAverageOscillator() => _data.EmaOsc(10, 30);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HighestHigh() => _data.HighHigh(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HighestClose() => _data.HighClose(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HistoricalHighestHigh() => _data.HistHighHigh();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HistoricalHighestClose() => _data.HistHighClose();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> LowestLow() => _data.LowLow(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> LowestClose() => _data.LowClose(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HistoricalLowestLow() => _data.HistLowLow();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HistoricalLowestClose() => _data.HistLowClose();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?, decimal?, decimal?)>> IchimokuCloud() => _data.Ichimoku(9, 26, 52);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> ModifiedMovingAverage() => _data.Mma(30);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> MacdHist() => _data.MacdHist(12, 26, 9);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> OnBalanceVolume() => _data.Obv();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> RawStochasticsValue() => _data.Rsv(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> RelativeStrength() => _data.Rs(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> SimpleMovingAverageOscillator() => _data.SmaOsc(10, 30);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> StandardDeviation() => _data.Sd(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?)>> Stochastics_Fast() => _data.FastSto(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?)>> Stochastics_Slow() => _data.SlowSto(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?)>> Stochastics_Full() => _data.FullSto(14, 3, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> StochasticsOscillator_Fast() => _data.FastStoOsc(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> StochasticsOscillator_Slow() => _data.SlowStoOsc(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> StochasticsOscillator_Full() => _data.FullStoOsc(14, 3, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> WeightedMovingAverage() => _data.Wma(20);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HullMovingAverage() => _data.Hma(30);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?)>> KeltnerChannels() => _data.Kc(20, 2, 10);
        [Benchmark]
        public IReadOnlyList<IOhlcv> TransformFromTradesToMinute() => _tradeData.TransformToCandles<PerMinute>();
        [Benchmark]
        public IReadOnlyList<IOhlcv> TransformFromTradesToHourly() => _tradeData.TransformToCandles<Hourly>();
        [Benchmark]
        public IReadOnlyList<IOhlcv> TransformFromTradesToBeHourly() => _tradeData.TransformToCandles<BiHourly>();
        [Benchmark]
        public IReadOnlyList<IOhlcv> TransformFromTradesToDaily() => _tradeData.TransformToCandles<Daily>();
        [Benchmark]
        public IReadOnlyList<IOhlcv> TransformFromTradesToWeekly() => _tradeData.TransformToCandles<Weekly>();
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmark>();
        }
    }
}
