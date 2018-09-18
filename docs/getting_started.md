[Home](index.md) | [Release Notes](release_notes.md) | [Indicators](indicators.md) | [Candlestick patterns](candlesticks.md) | [Rule patterns](rule_patterns.md)

# Getting Started

## How To Install
Nuget package is available in modules, please install the package according to the needs

    // For importing
    PM > Install-Package Trady.Importer

    // For computing & backtesting
    PM > Install-Package Trady.Analysis

    // The Core package
    PM > Install-Package Trady.Core


## Step 1: Import Candlestick data from Feed
    var importer = new YahooFinanceImporter();
    var candles = await importer.ImportAsync("FB");

## Step 2: Transform Candlestick Data (optional)

    var monthlyCandles = candles.Transform<Daily, Monthly>();

## Step 3: Calculate Indicators

    var last = candles.Sma(30).Last();
    Console.WriteLine($"{last.DateTime}, {last.Tick}");

## Step 4: Create Buy/Sell Rules

    var buyRule = Rule.Create(c => c.IsFullStoBullishCross(14, 3, 3))
        .And(c => c.IsMacdOscBullish(12, 26, 9))
        .And(c => c.IsAccumDistBullish());

    var sellRule = Rule.Create(c => c.IsFullStoBearishCross(14, 3, 3))
        .Or(c => c.IsMacdBearishCross(12, 24, 9));

## Step 5: Create Portfolio Builder    
    var runner = new Builder()
        .Add(fb, 10)
        .Buy(buyRule)
        .Sell(sellRule)
        .Build();
    
## Step 6: Start Backtesting with the Portfolio    
    var result = await runner.RunAsync(10000, 1);

## Step 7: Review Backtest Results
    Console.WriteLine(string.Format("Transaction count: {0:#.##}, P/L ratio: {1:0.##}%, Principal: {2:#}, Total: {3:#}",
        result.Transactions.Count(),
        result.CorrectedProfitLoss * 100,
        result.Principal,
        result.CorrectedBalance));


