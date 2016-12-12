using Quandl.NET;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trady.Analysis;
using Trady.Analysis.Indicator;
using Trady.Core;
using Trady.Exporter;
using Trady.Importer;
using Trady.Strategy;
using Trady.Strategy.Helper;
using Trady.Strategy.MValue;
using Trady.Strategy.Rule;

class Program
{
    static void Main(string[] args)
    {
        //DownloadData();
        //CalculateIndicators();
        //PlayWithStrategy();
        PlayWithMValue();
        //DownloadSpx();
        //TestBetweenUsingRecursiveOrPrecache();
    }

    private static void TestBetweenUsingRecursiveOrPrecache()
    {
        Console.WriteLine("Importing data...");
        var importer = new CsvImporter("data\\SPX.csv");
        var equity = importer.ImportAsync("SPX").Result;

        var startTime1 = DateTime.Now;
        Console.WriteLine("Start computing by using recursion: {0}", startTime1);
        var result1 = new List<ExponentialMovingAverage.IndicatorResult>();
        for (int i = 0; i < equity.Count(); i++)
        {
            result1.Add(new ExponentialMovingAverage(equity, 30).ComputeByIndex(i));
        }
        var endTime1 = DateTime.Now;
        Console.WriteLine("End computing by using recursion: {0}, Time elapsed: {1:0.##}s", endTime1, (endTime1-startTime1).TotalSeconds);
        Console.WriteLine("Value @ i=256: {0}", result1.ElementAt(256).Ema);
        Console.WriteLine();

        var startTime2 = DateTime.Now;
        Console.WriteLine("Start computing by using iteration: {0}", startTime2);
        var ts = new ExponentialMovingAverage(equity, 30).Compute(null, null);
        var endTime2 = DateTime.Now;
        Console.WriteLine("End computing by using iteration: {0}, Time elapsed: {1:0.##}s", endTime2, (endTime2 - startTime2).TotalSeconds);
        Console.WriteLine("Value @ i=256: {0}", ts.ElementAt(256).Ema);

        Console.ReadLine();
    }

    private static void DownloadSpx()
    {
        Console.WriteLine("Downloading Spx...");
        const string ApiKey = "M185pFZuSebc4Qr5MRz2";

        var importer = new QuandlYahooImporter(ApiKey);
        var spx = importer.ImportAsync("INDEX_GSPC").Result;

        var exporter = new CsvExporter("data\\SPX.csv");
        bool? success = exporter.ExportAsync(spx).Result;
    }

    private static void DownloadData()
    {
        Console.WriteLine("Downloading data...");
        var sp500Constituents = UsefulDataAndLists.GetSP500IndexConstituentsAsync().Result;

        const string ApiKey = "M185pFZuSebc4Qr5MRz2";

        var importer = new QuandlWikiImporter(ApiKey);
        var equities = new List<Equity>();

        Directory.CreateDirectory("data");

        var startTime = DateTime.Now;
        Console.WriteLine($"Download start: {startTime}");

        int completedCount = 0;
        Parallel.ForEach(sp500Constituents, new ParallelOptions { MaxDegreeOfParallelism = 4 }, c =>
        {
            var ticker = c.Ticker.Replace("-", "_");

            bool downloadSuccess = false;
            Equity equity = null;
            while (!downloadSuccess)
            {
                try
                {
                    Console.WriteLine($"Downloading data: {ticker} ({completedCount}/{sp500Constituents.Count()})...");
                    equity = importer.ImportAsync(ticker).Result;
                    downloadSuccess = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fails to download history: {ex.Message}, Retry after 10 seconds...");
                    Thread.Sleep(10000);
                }
            }

            Interlocked.Add(ref completedCount, 1);
            equities.Add(equity);
            var exporter = new CsvExporter($"data\\{equity.Name}.csv");
            bool success = exporter.ExportAsync(equity).Result;
        });

        var endTime = DateTime.Now;
        Console.WriteLine($"End: {endTime}");

        File.WriteAllText("DownloadTime.txt", $"start: {startTime}, end: {endTime}");
    }

    private static void CalculateIndicators()
    {
        Console.WriteLine("Importing data...");
        var importer = new CsvImporter("data\\FB.csv");
        var equity = importer.ImportAsync("FB").Result;
        //var importer = new YahooFinanceImporter();
        //var equity = importer.ImportAsync("00392.HK").Result;

        Console.WriteLine("Computing data...");
        var startTime = DateTime.Now;
        var smaTs = equity.Sma(30);
        var emaTs = equity.Ema(30);
        var rsiTs = equity.Rsi(14);
        var macdTs = equity.Macd(12, 26, 9);
        var stoTs = equity.FullSto(14, 3, 3);
        var obvTs = equity.Obv();
        var accumDistTs = equity.AccumDist();
        var bbTs = equity.Bb(20, 2);
        var atrTs = equity.Atr(14);
        var adxTs = equity.Adx(14);
        var endTime = DateTime.Now;
        Console.WriteLine($"Data computed: time elapsed: {(endTime - startTime).TotalMilliseconds}ms");

        var tsList = new List<ITimeSeries>();
        tsList.Add(smaTs);
        tsList.Add(emaTs);
        tsList.Add(rsiTs);
        tsList.Add(macdTs);
        tsList.Add(stoTs);
        tsList.Add(obvTs);
        tsList.Add(accumDistTs);
        tsList.Add(bbTs);
        tsList.Add(atrTs);
        tsList.Add(adxTs);

        Console.WriteLine("Exporting results...");
        var exporter = new CsvExporter("result\\FB.csv");
        bool success = exporter.ExportAsync(equity, tsList).Result;

        Console.WriteLine("Process completed!");
        Console.ReadLine();
    }

    private static void PlayWithMValue()
    {
        Console.WriteLine("Importing data...");
        var importer = new CsvImporter("data\\SPX.csv");
        var equity = importer.ImportAsync("SPX").Result;

        var list = Enumerable.Range(4, 17).ToList();
        list.AddRange(Enumerable.Range(5, 44).Select(v => v * 5));

        var oscList = new List<Tuple<int, int>>();
        oscList.Add(new Tuple<int, int>(10, 20));
        oscList.Add(new Tuple<int, int>(10, 30));
        oscList.Add(new Tuple<int, int>(20, 40));
        oscList.Add(new Tuple<int, int>(20, 50));
        oscList.Add(new Tuple<int, int>(30, 50));
        oscList.Add(new Tuple<int, int>(30, 100));
        oscList.Add(new Tuple<int, int>(30, 150));
        oscList.Add(new Tuple<int, int>(50, 100));
        oscList.Add(new Tuple<int, int>(50, 150));
        oscList.Add(new Tuple<int, int>(50, 200));
        oscList.Add(new Tuple<int, int>(50, 250));
        oscList.Add(new Tuple<int, int>(100, 200));
        oscList.Add(new Tuple<int, int>(100, 250));

        var mValueResults = new ConcurrentDictionary<string, MValueResult>();
        var allStartTime = DateTime.Now;
        Console.WriteLine($"Start process @ {allStartTime} ...");
        list.ForEach(p =>
        {
            var startTime = DateTime.Now;
            Console.WriteLine($"Processing SMA({p}) @ {startTime}...");
            Parallel.ForEach(oscList, pair =>
            {
                string name = $"AboveSma({p})&SmaOscBullish({pair.Item1},{pair.Item2})";
                Console.WriteLine($"Processing {name}...");
                var mValue = equity.ComputeMValue(c => c.IsAboveSma(p) && c.IsSmaBullish(50) && c.IsSmaOscBullish(pair.Item1, pair.Item2));
                bool addedSuccess = mValueResults.TryAdd(name, mValue);
                if (!addedSuccess)
                    Console.WriteLine($"{p} add fails");
                Console.WriteLine("{0}: {1:0.####}%, {2:0.####}%, {3:0.####}%, {4}, {5}", name, mValue.MValue1, mValue.MValue2, mValue.MValue3, mValue.EventCount, mValue.SampleCount);
            });

            var endTime = DateTime.Now;
            Console.WriteLine($"Processed SMA({p}) @ {endTime}, Time elapsed: {(endTime - startTime).TotalSeconds}s");
        });
        var allEndTime = DateTime.Now;
        Console.WriteLine($"All processed @ {allEndTime}. Time elapsed: {(allEndTime - allStartTime).TotalSeconds}s. Start saving result...");
        var exporter = new MValueExporter("mValueResult.csv");
        bool success = exporter.ExportAsync(mValueResults.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)).Result;
        Console.WriteLine("Result saved");

        Console.ReadLine();
    }

    private static void PlayWithStrategy()
    {
        Console.WriteLine("Importing data...");
        //var importer = new CsvImporter("data\\SPX.csv");
        //var equity = importer.ImportAsync("SPX").Result;
        var importer = new YahooFinanceImporter();
        var equity = importer.ImportAsync("0392.HK").Result;
        //equity.MaxTickCount = 256;

        Console.WriteLine("Setting rules...");
        //var buyRule = Rule.Create(c => c.IsFullStoBullishCross(14, 3, 3))
        //    .And(c => c.IsMacdOscBullish(12, 26, 9))
        //    .And(c => c.IsSmaOscBullish(10, 30))
        //    .And(c => c.IsAccumDistBullish());
        var buyRule = Rule.Create(c => c.IsEmaBullish(30));

        //var sellRule = Rule.Create(c => c.IsFullStoBearishCross(14, 3, 3))
        //    .Or(c => c.IsMacdBearishCross(12, 24, 9))
        //    .Or(c => c.IsSmaBearishCross(10, 30));
        var sellRule = Rule.Create(c => !c.IsEmaBearish(30));

        Console.WriteLine("Creating portfolio...");
        var portfolio = new PortfolioBuilder()
            .Add(equity)
            .Buy(buyRule)
            .Sell(sellRule)
            .Build();

        var startTime = DateTime.Now;
        Console.WriteLine($"Runnning backtest... @ {startTime}");

        PortfolioResult result;
        try
        {
            result = portfolio.RunAsync(10000).Result;
            var endTime = DateTime.Now;
            Console.WriteLine($"Backtest completed @ {endTime}. Time elapsed: {(endTime - startTime).TotalSeconds} s.");

            Console.WriteLine(string.Format("Transaction count: {0:#.##}, P/L ratio: {1:0.##}%, Principal: {2:#}, Total: {3:#}",
                result.TransactionCount,
                result.ProfitLossRatio * 100,
                result.Principal,
                result.Total));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        Console.ReadLine();
    }
}