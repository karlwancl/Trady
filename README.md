# Trady (Under development)
Trady targets to be an automated trading system that provides stock data feeding, indicator computing, strategy building and automatic trading. It is built based on .NET Standard 1.6.1.

### Read Before You Use
This system is still in early-stage, and it's still immature to use in production environment, please use it wisely.

### Currently Available Features
* Stock data feeding (via [Quandl.NET](https://github.com/salmonthinlion/Quandl.NET))
* Indicator computing (including SMA, EMA, RSI, MACD, BB, etc.)
* Strategy building & backtesting

### Supported Platforms
* .NET Core 1.0 or above
* .NET Framework 4.6.1 or above
* Xamarin.iOS
* Xamarin.Android
* Universal Windows Platform 10.0 or above

### How To Install
There is no nuget package available yet. You can check out the project and use it directly.

### How To Use
#### Import stock data
    // From Quandl wiki database
    var importer = new QuandlWikiImporter(apiKey);

    // Or from dedicated csv file
    var importer = new CsvImporter("FB.csv");

    // Get stock data from the above importer
    var equity = await importer.ImportAsync("FB");

#### Compute indicators
    // Currently available indicators: SMA, EMA, RSI, MACD, BB, Stochastics, OBV, AccumDist, ATR, ADX
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

#### Strategy building & backtesting
    // Build buy rule & sell rule based on various patterns
    var buyRule = Rule.Create(c => c.IsFullStoBullishCross(14, 3, 3))
        .And(c => c.IsMacdOscBullish(12, 26, 9))
        .And(c => c.IsSmaOscBullish(10, 30))
        .And(c => c.IsAccumDistBullish());

    var sellRule = Rule.Create(c => c.IsFullStoBearishCross(14, 3, 3))
        .Or(c => c.IsMacdBearishCross(12, 24, 9))
        .Or(c => c.IsSmaBearishCross(10, 30));

    // Create portfolio instance by using PortfolioBuilder
    var portfolio = new PortfolioBuilder()
        .Add(equity)
        .Buy(buyRule)
        .Sell(sellRule)
        .Build();
    
    // Start backtesting with the portfolio
    var result = await portfolio.RunAsync(10000);

    // Get backtest result for the portfolio
    Console.WriteLine(string.Format("Transaction count: {0:#.##}, P/L ratio: {1:0.##}%, Principal: {2:#}, Total: {3:#}",
        result.TransactionCount,
        result.ProfitLossRatio * 100,
        result.Principal,
        result.Total));

### Backlog
* Complete other indicators (e.g. ADX, Aroon, etc.)
* Complete candlestick patterns
* Add more indicator filtering patterns (e.g. IsOverSma(30), IsBelowSma(30), etc.)
* Add more rule for strategy building
* Add broker adaptor for real-time trade (e.g. interactive broker, etc.)
* Add stock feed provider for real-time trade (e.g. Yahoo!, etc.)
* Maybe reporting for backtesting result/ real-time trade result
* MORE, MORE AND MORE!!!!

### Powered by
* [CsvHelper](https://github.com/JoshClose/CsvHelper) ([@JoshClose](https://github.com/JoshClose)) : Great library for reading/ writing CSV file

### License
This library is under [Apache-2.0 License](https://github.com/salmonthinlion/Trady/blob/master/LICENSE)
