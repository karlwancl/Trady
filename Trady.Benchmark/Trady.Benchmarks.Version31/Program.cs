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
        public IReadOnlyList<Trady.Analysis.AnalyzableTick<(decimal?, decimal?, decimal?)>> MACD() => _data.Macd(12,26,9);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> CommodityChannelIndex() => _data.Cci(20);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> StochasticMomentumIndex() => _data.Smi(15,6,6);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> StochasticRelativeStrengthIndex() => _data.StochRsi(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> NetMomentumOscillator() => _data.Nmo(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> RM() => _data.Rm(20, 4);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> RMI() => _data.Rmi(20, 4);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> DYMOI() => _data.Dymoi(5, 10, 14, 30, 5);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> SAR() => _data.Sar(0.02m, 0.2m);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Median() => _data.Median(20);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Percentile() => _data.Percentile(30, 0.7m);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> AccumDist() => _data.AccumDist(20);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?)>> Aroon() => _data.Aroon(25);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> AroonOsc() => _data.AroonOsc(25);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Atr() => _data.Atr(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?)>> Bb() => _data.Bb(20, 2);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> BbWidth() => _data.BbWidth(20, 2);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?)>> Chandlr() => _data.Chandlr(22, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Mtm() => _data.Mtm();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Roc() => _data.Roc();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Pdi() => _data.Pdi(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Mdi() => _data.Mdi(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Adx() => _data.Adx(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Adxr() => _data.Adxr(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Er() => _data.Er(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Kama() => _data.Kama(10, 2, 30);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> EmaOsc() => _data.EmaOsc(10, 30);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HighHigh() => _data.HighHigh(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HighClose() => _data.HighClose(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HistHighHigh() => _data.HistHighHigh();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HistHighClose() => _data.HistHighClose();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> LowLow() => _data.LowLow(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> LowClose() => _data.LowClose(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HistLowLow() => _data.HistLowLow();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> HistLowClose() => _data.HistLowClose();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?, decimal?, decimal?)>> Ichimoku() => _data.Ichimoku(9, 26, 52);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Mma() => _data.Mma(30);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> MacdHist() => _data.MacdHist(12, 26, 9);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Obv() => _data.Obv();

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Rsv() => _data.Rsv(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Rs() => _data.Rs(14);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> SmaOsc() => _data.SmaOsc(10, 30);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> Sd() => _data.Sd(10);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?)>> FastSto() => _data.FastSto(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?)>> SlowSto() => _data.SlowSto(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<(decimal?, decimal?, decimal?)>> FullSto() => _data.FullSto(14, 3, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> FastStoOsc() => _data.FastStoOsc(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> SlowStoOsc() => _data.SlowStoOsc(14, 3);

        [Benchmark]
        public IReadOnlyList<IAnalyzableTick<decimal?>> FullStoOsc() => _data.FullStoOsc(14, 3, 3);
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmark>();
        }
    }
}
