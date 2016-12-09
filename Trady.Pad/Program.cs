using System;
using System.Collections.Generic;
using Trady.Analysis;
using Trady.Analysis.Indicator;
using Trady.Core;
using Trady.Exporter;
using Trady.Importer;
using Trady.Strategy;
using Trady.Strategy.Rule;

class Program
{
    static void Main(string[] args)
    {
        //CalculateIndicators();
        PlayWithStrategy();
    }

    private static void CalculateIndicators()
    {
        Console.WriteLine("Importing data...");
        var importer = new CsvImporter("data\\FB.csv");
        var cs = importer.ImportAsync("FB").Result;

        Console.WriteLine("Computing data...");
        var startTime = DateTime.Now;
        var smaTs = new SimpleMovingAverage(cs, 30).Compute();
        var emaTs = new ExponentialMovingAverage(cs, 30).Compute();
        var rsiTs = new RelativeStrengthIndex(cs, 14).Compute();
        var macdTs = new MovingAverageConvergenceDivergence(cs, 12, 26, 9).Compute();
        var stoTs = new Stochastics.Full(cs, 14, 3, 3).Compute();
        var obvTs = new OnBalanceVolume(cs).Compute();
        var accumDistTs = new AccumulationDistributionLine(cs).Compute();
        var bbTs = new BollingerBands(cs, 20, 2).Compute();
        var endTime = DateTime.Now;
        Console.WriteLine($"Data computed: time elapsed: {(endTime - startTime).TotalMilliseconds}ms");

        var tsList = new List<IAnalyticResultTimeSeries>();
        tsList.Add(smaTs);
        tsList.Add(emaTs);
        tsList.Add(rsiTs);
        tsList.Add(macdTs);
        tsList.Add(stoTs);
        tsList.Add(obvTs);
        tsList.Add(accumDistTs);
        tsList.Add(bbTs);

        Console.WriteLine("Exporting results...");
        var exporter = new CsvExporter("result\\FB.csv");
        bool success = exporter.ExportAsync(cs, tsList).Result;

        Console.WriteLine("Process completed!");
        Console.ReadLine();
    }

    private static void PlayWithStrategy()
    {
        Console.WriteLine("Importing data...");
        var importer = new CsvImporter("data\\AMAT.csv");
        var equity = importer.ImportAsync("AMAT").Result;
        equity.MaxTickCount = 256;

        Console.WriteLine("Setting rules...");
        var buyRule = new Rule<Equity>((e, i) => e.IsFullStoBullishCross(14, 3, 3, i))
            .And((e, i) => e.IsMacdOscBullish(12, 26, 9, i))
            .And((e, i) => e.IsSmaOscBullish(10, 30, i))
            .And((e, i) => e.IsAccumDistBullish(i));

        var sellRule = new Rule<Equity>((e, i) => e.IsFullStoBearishCross(14, 3, 3, i))
            .Or((e, i) => e.IsMacdBearishCross(12, 24, 9, i))
            .Or((e, i) => e.IsSmaBearishCross(10, 30, i));

        Console.WriteLine("Creating portfolio...");
        var portfolio = new PortfolioBuilder()
            .AddEquity(equity)
            .BuyWhen(buyRule)
            .SellWhen(sellRule)
            .Build();

        var startTime = DateTime.Now;
        Console.WriteLine($"Runnning backtest... @ {startTime}");
        var result = portfolio.RunAsync(10000).Result;
        var endTime = DateTime.Now;
        Console.WriteLine($"Backtest completed @ {endTime}. Time elapsed: {(endTime - startTime).TotalSeconds} s.");

        Console.WriteLine(string.Format("Transaction count: {0:#.##}, P/L ratio: {1:0.##}%, Principal: {2:#}, Total: {3:#}",
            result.TransactionCount,
            result.ProfitLossRatio * 100,
            result.Principal,
            result.Total));

        Console.ReadLine();
    }
}