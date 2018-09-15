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
    [ClrJob, CoreJob, MonoJob]
    [LegacyJitX86Job, LegacyJitX64Job, RyuJitX64Job]
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
                Add(StatisticColumn.P90,
                    StatisticColumn.P95);
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
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmark>();
        }
    }
}
