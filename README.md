# Trady
[![Build status](https://ci.appveyor.com/api/projects/status/kqwo74gn5ms3h0n2?svg=true)](https://ci.appveyor.com/project/lppkarl/trady)
[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Trady.Analysis.svg)](https://img.shields.io/nuget/vpre/Trady.Analysis.svg)
[![license](https://img.shields.io/github/license/lppkarl/Trady.svg)](https://github.com/lppkarl/Trady/blob/master/LICENSE)

Trady is a handy library for computing technical indicators, and targets to be an automated trading system that provides stock data feeding, indicator computing, strategy building and automatic trading. It is built based on .NET Standard 1.6.1.

### Read Before You Use
This library is intended for personal use, use with care for production environment.

### Currently Available Features
* Stock data feeding (via CSV File, [Quandl.NET](https://github.com/lppkarl/Quandl.NET), [YahooFinanceApi](https://github.com/lppkarl/YahooFinanceApi), [StooqApi](https://github.com/lppkarl/StooqApi), [Nuba.Finance.Google](https://github.com/nubasoftware/Nuba.Finance.Google))
* Indicator computing (including SMA, EMA, RSI, MACD, BB, etc.)
* Strategy building & backtesting

### Updates from 2.0.x to 2.1.0 (Currently 2.1.0-beta3)
** Trady.Analysis module **
* The indicator now intakes IEnumerable<T> instance instead of IList<T>
* The indicator now outputs IReadOnlyList<T> instance instead of IList<T>
* Computes from different input now have different output 
    * Tuples => decimal?/Tuples 
    * Candles => AnalyzableTick\<decimal?>/AnalyzableTick\<Tuples> where AnalyzableTick<T> includes DateTime in its property 
* As of the above change, "Tick" property is used for the indicator result's value if it's computed from candles
* Portfolio-related items have been moved under Trady.Analysis.Backtest namespace. Classes & methods are renamed
    * Portfolio => Builder/Runner
    * RunBackTest() => Run()
    * RunBackTestAsync() => RunAsync()
* Added IndexedObject & RuleExecutor for signal capturing by rules
* Bugfix: Removed AnalyzableLocator to fix memory issue, replaced with AnalyzeContext for indicator caching

** Trady.Importer module **
* The candles are exported as IReadOnlyList<Candle> instead of IList<Candle>
* Added GoogleFinanceImporter (adapter to [Nuba.Google.Finance](https://github.com/nubasoftware/Nuba.Finance.Google), thanks to [@fernaramburu](https://github.com/fernaramburu))
* Separated importers are also available for modular installation:
    * Trady.Importer.Csv
    * Trady.Importer.Yahoo
    * Trady.Importer.Quandl
    * Trady.Importer.Stooq
    * Trady.Importer.Google
* Align importer's behavior, startTime & endTime is inclusive now for all importers

### Supported Platforms
* .NET Core 1.0 or above
* .NET Framework 4.6.1 or above
* Xamarin.iOS
* Xamarin.Android
* Universal Windows Platform 10.0 or above

### Currently Supported Indicators
| AccumDist | Aroon | AroonOsc | Adxr | Atr | Bb | BbWidth | Chandlr | ClosePriceChange | ClosePricePercentageChange |
 Dmi | Er | Ema | EmaOsc | HighestHigh | HighestClose | HistoricalHighestHigh | HistoricalHighestClose | Ichimoku | 
 Kama | LowestLow | LowestClose | HistoricalLowestLow | HistoricalLowestClose | Mema | Macd | Obv | Rsv | Rs | Rsi | 
 Sma | SmaOsc | Sd | FastSto | FullSto | SlowSto |

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
    * [Capture signals by rules](#CaptureSignalByRules)
    * [Strategy Building And Backtesting](#StrategyBuildingAndBacktesting)
    * [Implement Your Own Pattern](#ImplementYourOwnPattern)

<a name="tldr"></a>
### Tldr
    var importer = new YahooFinanceImporter();
    var candles = await importer.ImportAsync("FB");
    var last = candles.Sma(30).Last();
    Console.WriteLine($"{last.DateTime}, {last.Tick}");
[Back to content](#Content)

<a name="ImportStockData"></a>
#### Import stock data
    // From Quandl wiki database
    var importer = new QuandlWikiImporter(apiKey);

    // From Yahoo Finance
    var importer = new YahooFinanceImporter();

    // From Stooq
    var importer = new StooqImporter();

    // From Google Finance
    var importer = new GoogleFinanceImporter();

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
        public async Task<IReadOnlyList<Candle>> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
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
    // This library supports computing from tuples or candles
    var closes = new List<decimal>{
        2428.370117,
        2425.550049,
        2430.01001,
        2468.110107
    };
    var smaTs = closes.Sma(30, startIndex, endIndex);
    Or, 
    var smaTs = new SimpleMovingAverageByTuple(closes, 30).Compute(startIndex, endIndex);

    var candles = await importer.ImportAsync("FB");
    var smaTs = candles.Sma(30, startIndex, endIndex);
    Or,
    var smaTs = new SimpleMovingAverage(candles, 30).Compute(startIndex, endIndex);

[Back to content](#Content)

<a name="ImplementYourOwnIndicatorSimpleType"></a>
#### Implement your own indicator - Simple Type
    // You can also implement your own indicator by extending the AnalyzableBase<TInput, TOutput> class
    public class MyIndicator : AnalyzableBase<Candle, AnalyzableTick<decimal?>>
    {
        // Backing indicator for the indicator
        SimpleMovingAverageByTuple _sma;

        public MyIndicator(IEnumerable<Candle> inputs, int periodCount) : base(inputs)
        {
            // Initialize reference indicators, or other required stuff here
            _sma = new SimpleMovingAverageByTuple(inputs.Select(i => i.Close), periodCount);
        }

        // Implement the compute algorithm, uses mappedInputs, index & backing indicators for computation here
        protected override AnalyzableTick<decimal?> ComputeByIndexImpl(IReadOnlyList<Candle> mappedInputs, int index)
        {
			return new AnalyzableTick<decimal?>(mappedInputs[index].DateTime, _sma[index]);
		}
    }

    // Use case
    var results = new MyIndicator(candles, 30).Compute();
    foreach (var r in results)
    {
        Console.WriteLine($"{r.DateTime}, {r.Tick.Value}");
    }
[Back to content](#Content)

<a name="ImplementYourOwnIndicatorCummulativeType"></a>
#### Implement your own indicator - Cummulative Type
    // You can implement your own indicator by extending the CummulativeAnalyzableBase<TInput, TOutput> class
    public class MyCumulativeIndicator : CumulativeAnalyzableBase<Candle, AnalyzableTick<decimal?>>
    {
        // Backing indicator for the indicator
        SimpleMovingAverageByTuple _sma;

        public MyCumulativeIndicator(IEnumerable<Candle> inputs, int periodCount) : base(inputs)
        {
            // Initialize reference indicators, or other required stuff here
            _sma = new SimpleMovingAverageByTuple(inputs.Select(i => i.Close), periodCount);
        }

        // Implement the compute algorithm for index > InitialValueIndex, uses mappedInputs, index, prev analyzable tick & backing indicators for computation here
        protected override AnalyzableTick<decimal?> ComputeCumulativeValue(IReadOnlyList<Candle> mappedInputs, int index, AnalyzableTick<decimal?> prevOutputToMap)
        {
            return new AnalyzableTick<decimal?>(mappedInputs[index].DateTime, _sma[index] + prevOutputToMap.Tick);
        }

        // Same for the above but for index = InitialValueIndex
        protected override AnalyzableTick<decimal?> ComputeInitialValue(IReadOnlyList<Candle> mappedInputs, int index)
        {
            return new AnalyzableTick<decimal?>(mappedInputs[index].DateTime, _sma[index]);
        }

        // You can also override the InitialValueIndex property & ComputeNullValue method if needed
    }

    // Use case
    var results = new MyCumulativeIndicator(candles).Compute();
    foreach (var r in results)
    {
        Console.WriteLine($"{r.DateTime}, {r.Tick.Value}");
    }   
[Back to content](#Content)

<a name="ImplementYourOwnIndicatorMovingAverageType"></a>
#### Implement your own indicator - Moving-Average Type
    // You can make use of the GenericExponentialMovingAverage class to get rid of implementing MA-related indicator on your own
     public class MyGemaIndicator : AnalyzableBase<Candle, AnalyzableTick<decimal?>>
    {
        GenericExponentialMovingAverage _gema;

        public MyGemaIndicator(IEnumerable<Candle> inputs, int periodCount) : base(inputs)
        {
            // parameters: initialValueIndex, initialValueFunction, indexValueFunction, smoothingFactorFunction
			_gema = new GenericExponentialMovingAverage(
				0,
				i => inputs.Select(ip => ip.Close).ElementAt(i),
				i => inputs.Select(ip => ip.Close).ElementAt(i),
				i => 2.0m / (periodCount + 1),
				inputs.Count());
        }

        protected override AnalyzableTick<decimal?> ComputeByIndexImpl(IReadOnlyList<Candle> mappedInputs, int index)
        {
            return new AnalyzableTick<decimal?>(mappedInputs[index].DateTime, _gema[index]);
        }
    }
[Back to content](#Content)

<a name="CaptureSignalByRules"></a>
#### Capture signals by rules
    // The following shows the number of candles that fulfill both the IsAboveSma(30) & IsAboveSma(10) rule
    var rule = Rule.Create(c => c.IsAboveSma(30))
        .And(c => c.IsAboveSma(10));

    using (var ctx = new AnalyzeContext(candles))
    {
        var validObjects = new SimpleRuleExecutor(ctx, rule).Execute();
        Console.WriteLine(validObjects.Count());
    }

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
    var runner = new Builder()
        .Add(equity, 10)
        .Add(equity2, 30)
        .Buy(buyRule)
        .Sell(sellRule)
        .Build();
    
    // Start backtesting with the portfolio
    var result = await runner.RunAsync(10000, 1);

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
            var sma30 = ic.Get<SimpleMovingAverage>(30)[ic.Index];
            var sma10 = ic.Get<SimpleMovingAverage>(10)[ic.Index];
            return sma30 > sma10;
        }
        
        public static bool IsSma10LargerThanSma30(this IndexedCandle ic)
        {
            var sma30 = ic.Get<SimpleMovingAverage>(30)[ic.Index];
            var sma10 = ic.Get<SimpleMovingAverage>(10)[ic.Index];
            return sma10 > sma30;
        }
    }

    // Use case
    var buyRule = Rule.Create(c => c.IsSma10LargerThanSma30());
    var sellRule = Rule.Create(c => c.IsSma30LargerThanSma10());
    var runner = new Builder().Add(candles, 10).Buy(buyRule).Sell(sellRule).Build();
    var result = await runner.RunAsync(10000, 1);
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
