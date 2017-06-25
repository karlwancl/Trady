# Trady
[![Build status](https://ci.appveyor.com/api/projects/status/kqwo74gn5ms3h0n2?svg=true)](https://ci.appveyor.com/project/lppkarl/trady)
[![NuGet](https://img.shields.io/nuget/v/Trady.Analysis.svg)](https://www.nuget.org/packages/Trady.Analysis/)
[![license](https://img.shields.io/github/license/lppkarl/Trady.svg)](https://github.com/lppkarl/Trady/blob/master/LICENSE)

Trady is a handy library for computing technical indicators, and targets to be an automated trading system that provides stock data feeding, indicator computing, strategy building and automatic trading. It is built based on .NET Standard 1.6.1.

### Read Before You Use
This library is intended for personal use, use with care for production environment.

### Currently Available Features
* Stock data feeding (via [Quandl.NET](https://github.com/salmonthinlion/Quandl.NET), [YahooFinanceApi](https://github.com/salmonthinlion/YahooFinanceApi), [StooqApi](https://github.com/salmonthinlion/StooqApi))
* Indicator computing (including SMA, EMA, RSI, MACD, BB, etc.)
* Strategy building & backtesting

### Recent Update (25th, June 2017)
* (Breaking-Change) Equity class is removed! IList\<Candle> is used directly as a replacement, make sure the input candle list is in time-ascending order.
* (Breaking-Change) ITick interface is removed! Indicator result no longer includes DateTime property, please map the DateTime from the source array.
* (Breaking-Change) Some indicator result object has been simplified. e.g. You can directly call candles.Sma(30).First() instead of candles.Sma(30).First().Sma to get the sma result.
* Simplified custom indicator building, you can now implement by inheriting from AnalyzableBase<TInput, TOutput>, no extra IndicatorResult class is needed.
* (Breaking-Change) Removed GetOrCreateAnalytic<T>() for indicator instance creation, just use 'new' keyword for all the things.
* (Breaking-Change) TickProvider class is removed.
* (Breaking-Change) IndexCandle is renamed as IndexedCandle
* StooqImporter is added.

### Supported Platforms
* .NET Core 1.0 or above
* .NET Framework 4.6.1 or above
* Xamarin.iOS
* Xamarin.Android
* Universal Windows Platform 10.0 or above

### Currently Supported Indicators
| AccumDist | Aroon | AroonOsc | Adxr | Atr | Bb | BbWidth | Chandlr | ClosePriceChange | ClosePricePercentageChange |
| Dmi | Er | Ema | EmaOsc | HighestHigh | Ichimoku | Kama | LowestLow | Mema | Macd |
| Obv | Rsv | Rs | Rsi | Sma | SmaOsc | Sd | FastSto | FullSto | SlowSto |

### How To Install
Nuget package is available in modules, please install the package according to the needs

    // For importing
    PM > Install-Package Trady.Importer 

    // For computing & backtesting
    PM > Install-Package Trady.Analysis

### How To Use
<a name="Content"></a>
* Tldr
    * [Tldr](#tldr)

* Importing
    * [Import Stock Data](#ImportStockData)
    * [Implement Your Own Importer](#ImplementYourOwnImporter)
    
* Computing
    * [Transform Stock Data](#TransformStockData)
    * [Compute Indicators](#ComputeIndicators)
    * [Implement Your Own Indicator - Simple Type](#ImplementYourOwnIndicatorSimpleType)
    * [Implement Your Own Indicator - Cummulative Type](#ImplementYourOwnIndicatorCummulativeType)
    * [Implement Your Own Indicator - Moving Average Type](#ImplementYourOwnIndicatorMovingAverageType)
    
* Backtesting
    * [Strategy Building And Backtesting](#StrategyBuildingAndBacktesting)
    * [Implement Your Own Pattern](#ImplementYourOwnPattern)

<a name="tldr"></a>
### Tldr
    var importer = new QuandlWikiImporter(apiKey);
    var candles = await importer.ImportAsync("FB");
    Console.WriteLine(candles.Sma(30).Last());
[Back to content](#Content)

<a name="ImportStockData"></a>
#### Import stock data
    // From Quandl wiki database
    var importer = new QuandlWikiImporter(apiKey);

    // From Yahoo Finance
    var importer = new YahooFinanceImporter();

    // From Stooq
    var importer = new StooqImporter();

    // Or from dedicated csv file
    var importer = new CsvImporter("FB.csv");

    // Get stock data from the above importer
    var candles = await importer.ImportAsync("FB");
[Back to content](#Content)

<a name="ImplementYourOwnImporter"></a>
#### Implement your own importer
    // You can also implement your own importer by implementing the IImporter interface
    public class MyImporter : IImporter
    {
        public async Task<IList<Candle>> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            // Your implementation to return a list of candles
        }
    }
    
    // Use case
    var importer = new MyImporter();
    var candles = await importer.ImportAsync("FB");
[Back to content](#Content)

<a name="TransformStockData"></a>
#### Transform stock data to specified period before computation
    // Transform the series for computation, downcast is forbidden
    // Supported period: PerSecond, PerMinute, Per15Minutes, Per30Minutes, Hourly, BiHourly, Daily, Weekly, Monthly

    var transformedCandles = candles.Transform<Daily, Weekly>();
[Back to content](#Content)

<a name="ComputeIndicators"></a>
#### Compute indicators
    // There are 2 ways to compute indicators
    1. By indicator extension (Some common indicators only)
        var smaTs = candles.Sma(30, startIndex, endIndex);
        
    2. By instance creation
        var smaTs = new SimplemMovingAverage(candles, 30).Compute(startIndex, endIndex);
[Back to content](#Content)

<a name="ImplementYourOwnIndicatorSimpleType"></a>
#### Implement your own indicator - Simple Type
    // You can also implement your own indicator by extending the AnalyzableBase<TInput, TOutput> class
    public class MyIndicator : AnalyzableBase<decimal, decimal?>
    {
        private SimpleMovingAverage _smaIndicator;

        // You should provide the following 2 constructors for the indicator class
        public MyIndicator(IList<decimal> closes, int param1): base(closes)
        {
            // Your construction code here

            // You can make use of other indicators for computing your own indicator
            _smaIndicator = new SimpleMovingAverage(closes, param1);
        }

        public MyIndicator(IList<Candle> candles, int param1) 
            : this(candles.Select(c => c.Close).ToList(), param1)
        {
        }

        protected override decimal? ComputeByIndexImpl(int index)
        {
            // Your indicator implementation to return the result
        }
    }

    // Use case
    var results = new MyIndicator(candles, 30).Compute();
    foreach (var r in results)
    {
        Console.WriteLine($"{r.Value}");
    }
[Back to content](#Content)

<a name="ImplementYourOwnIndicatorCummulativeType"></a>
#### Implement your own indicator - Cummulative Type
    // You can implement your own indicator by extending the CummulativeAnalyzableBase<TInput, TOutput> class
    public class MyCummulativeIndicator : CummulativeAnalyzableBase<decimal, decimal?>
    {
        // You should provide the following 2 constructors for the indicator class
        public MyIndicator(IList<decimal> closes): base(closes)
        {
            // Your construction code here
        }

        public MyIndicator(IList<Candle> candles) 
            : this(candles.Select(c => c.Close).ToList())
        {
        }

        // The first index value that needs calculation
        protected override int InitialValueIndex => 0;

        // The computation method for computing indicator when index < FirstIndexValue
        protected override decimal? ComputeNullValue(int index)
        {
            // Your implementation to return IndicatorResult
            // Typically, it should return null
        }

        // The computation method for computing indicator when index == FirstIndexValue
        protected override decimal? ComputeInitialValue(int index)
        {
            // Your implementation to return IndicatorResult
            // Typically, it should be calculated by the previous n terms
        }

        // The computation method for computing indicator when index > FirstIndexValue
        protected override decimal? ComputeIndexValue(int index, decimal? prevOutput)
        {
            // Your implementation to return value
            // You can use the prevOutput instance provided by the cache to calculate the new value here
        }
    }

    // Use case
    var results = new MyCumulativeIndicator(candles).compute();
    foreach (var r in results)
    {
        Console.WriteLine($"{r.Value}");
    }   
[Back to content](#Content)

<a name="ImplementYourOwnIndicatorMovingAverageType"></a>
#### Implement your own indicator - Moving-Average Type
    // You can make use of the GenericExponentialMovingAverage class to get rid of implementing MA-related indicator on your own
    public class MyMaIndicator : AnalyzableBase<decimal, decimal?>
    {
        private GenericExponentialMovingAverage _gemaIndicator;

        public MyMaIndicator(IList<Candle> candles) 
            : this(candles.Select(c => c.Close).ToList())
        {
        }

        public MyMaIndicator(IList<decimal> closes) : base(closes)
        {
            // The constructor intakes the following parameters:
            //  1. IList<TInput> instance (in this example, a IList<decimal>)
            //  2. Initial Value Index
            //  3. Inital Value Function
            //  4. Index Value Function
            //  5. Smoothing Factor Function (1.0m / periodCount for Mema, 2.0m / (periodCount + 1.0m) for Ema)

            _gemaIndicator = new GenericExponentialMovingAverage(
                closes,
                firstValueIndex,
                i => firstValueFunction,
                i => indexValueFunction,
                i => smoothingFactorFunction
            );
        }

        protected override decimal? ComputeByIndexImpl(int index)
        {
            // Your indicator implementation to return the result
        }
    }
[Back to content](#Content)

<a name="StrategyBuildingAndBacktesting"></a>
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
        .Add(equity, 10)
        .Add(equity2, 30)
        .Buy(buyRule)
        .Sell(sellRule)
        .Build();
    
    // Start backtesting with the portfolio
    var result = await portfolio.RunBacktestAsync(10000, 1);

    // Get backtest result for the portfolio
    Console.WriteLine(string.Format("Transaction count: {0:#.##}, P/L ratio: {1:0.##}%, Principal: {2:#}, Total: {3:#}",
        result.TransactionCount,
        result.ProfitLossRatio * 100,
        result.Principal,
        result.Total));
[Back to content](#Content)

<a name="ImplementYourOwnPattern"></a>
#### Implement your own pattern through Extension
    // Implement your pattern by creating a static class for extending IndexedCandle class
    public static class IndexedCandleExtension
    {
        public static bool IsSma30LargerThanSma10(this IndexedCandle ic)
        {
            var sma30 = ic.Get<SimpleMovingAverage>(30).ComputeByIndex(ic.Index);
            var sma10 = ic.Get<SimpleMovingAverage>(10).ComputeByIndex(ic.Index);
            return sma30 > sma10;
        }
        
        public static bool IsSma10LargerThanSma30(this IndexedCandle ic)
        {
            var sma30 = ic.Get<SimpleMovingAverage>(30).ComputeByIndex(ic.Index);
            var sma10 = ic.Get<SimpleMovingAverage>(10).ComputeByIndex(ic.Index);
            return sma10 > sma30;
        }
    }

    // Use case
    var buyRule = Rule.Create(c => c.IsSma10LargerThanSma30());
    var sellRule = Rule.Create(c => c.IsSma30LargerThanSma10());
    var portfolio = new PortfolioBuilder().Add(candles, 10).Buy(buyRule).Sell(sellRule).Build();
    var result = await portfolio.RunBacktestAsync(10000, 1);
[Back to content](#Content)

### Backlog
* Complete other indicators (e.g. Keltner Channels, MA Envelopes, etc.)
* Complete candlestick patterns (Low priority)
* Add more indicator filtering patterns (Add patterns on demand)
* Add broker adaptor for real-time trade (e.g. interactive broker, etc.)
* MORE, MORE AND MORE!!!!

### Powered by
* [CsvHelper](https://github.com/JoshClose/CsvHelper) ([@JoshClose](https://github.com/JoshClose)) : Great library for reading/ writing CSV file

### License
This library is under [Apache-2.0 License](https://github.com/lppkarl/Trady/blob/master/LICENSE)
