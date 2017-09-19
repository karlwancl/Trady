# Trady
[![Build status](https://ci.appveyor.com/api/projects/status/kqwo74gn5ms3h0n2?svg=true)](https://ci.appveyor.com/project/lppkarl/trady)
[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Trady.Analysis.svg)](https://www.nuget.org/packages/Trady.Analysis/2.1.0-beta6)
[![license](https://img.shields.io/github/license/lppkarl/Trady.svg)](LICENSE)

Trady is a handy library for computing technical indicators, and targets to be an automated trading system that provides stock data feeding, indicator computing, strategy building and automatic trading. It is built based on .NET Standard 1.6.1.

### Read Before You Use
This library is intended for personal use, use with care for production environment.

### Currently Available Features
* Stock data feeding (via CSV File, [Quandl.NET](https://github.com/lppkarl/Quandl.NET), [YahooFinanceApi](https://github.com/lppkarl/YahooFinanceApi), [StooqApi](https://github.com/lppkarl/StooqApi), [Nuba.Finance.Google](https://github.com/nubasoftware/Nuba.Finance.Google))
* Indicator computing (including SMA, EMA, RSI, MACD, BB, etc.)
* Signal capturing by rules
* Strategy backtesting by buy/sell rule

### Updates from 2.0.x to 2.1.0 (Currently 2.1.0-alpha6)
** Trady.Analysis module **
* Change: 
    * The indicator now intakes IEnumerable<T> instance instead of IList<T>
    * The indicator now outputs IReadOnlyList<T> instance instead of IList<T>
    * Computes from different input now have different output 
        * Tuples => decimal?/Tuples 
        * Candles => AnalyzableTick\<decimal?/Tuples>
    * As of the above change, "Tick" property is used for the indicator result's value if it's computed from candles
    * Backtest-related items have been moved under Trady.Analysis.Backtest namespace. Classes & methods are renamed
        * Portfolio => Builder + Runner
        * RunBackTest() => Run()
        * RunBackTestAsync() => RunAsync()
    * Candlestick-related items have been moved under Trady.Analysis.Candlestick namespace
    * Modified Exponential Moving Average is renamed as Modified Moving Average; Short form will be changed from Mema => Mma
    * Generic Exponential Moving Average is renamed as Generic Moving Average
    * Basic operations (Diff, PercentDiff, Highest, Lowest, Median, Percentile, Sma, Ema, Mema, Sd) can now intakes IEnumerable\<decimal?>
* Added new feature: 
    * Added IndexedObject & RuleExecutor for signal capturing by rules
    * Introduce NumericAnalyzable for extending simple operations on an indicator, i.e. you can now use .Diff(index) to get the diff value of an indicator on a particular index directly
    * Decision helper for pattern construction (IsTrue, IsPositive, IsNegative)
    * AsAnalyzable extension for converting a Func to Analyzable, Func can now benefits from the Analyzable infrastructure
* Added new indicator: 
    * (Highest/Lowest)Close
    * HistoricalHighest(High/Close)
    * HistoricalLowest(Close/Low)
    * Median
    * Percentile
    * Difference
    * PercentageDifference
    * MacdHistogram
* Added new pattern: 
    * IsBreakingHighest(High/Close)
    * IsBreakingLowest(Close/Low)
    * IsBreakingHistoricalHighest(High/Close)
    * IsBreakingHistoricalLowest(Close/Low) 
* Bugfix: 
    * Removed AnalyzableLocator. AnalyzeContext is in replacement for sharing indicator within a scope

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
* Mono 4.6 or above
* Xamarin.iOS 10.0 or above
* Xamarin.Android 7.0 or above
* Xamarin.Mac 3.0 or above

### Currently Supported Indicators
Please refer to another markdown document [here](supported_indicators.md)

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
    
* Computing
    * [Transform Stock Data](#TransformStockData)
    * [Compute Indicator](#ComputeIndicators)
    * [Compute Simple Operations on Indicator](#ComputeIndicatorsOperation)
    * [Convert Func to Indicator](#ConvertFunctionToAnalyzable)
    
* Backtesting
    * [Capture Signals by Rules](#CaptureSignalByRules)
    * [Strategy Building and Backtesting](#StrategyBuildingAndBacktesting)
    * [Implement Rule Pattern](#ImplementYourOwnPattern)

* Advanced
    * [Implement Your Own Importer](#ImplementYourOwnImporter)
    * [Implement Your Own Indicator - Simple Type](#ImplementYourOwnIndicatorSimpleType)
    * [Implement Your Own Indicator - Cummulative Type](#ImplementYourOwnIndicatorCummulativeType)
    * [Implement Your Own Indicator - Moving Average Type](#ImplementYourOwnIndicatorMovingAverageType)

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

<a name="TransformStockData"></a>
#### Transform stock data to specified period before computation
    // Transform the series for computation, downcast is forbidden
    // Supported period: PerSecond, PerMinute, Per15Minutes, Per30Minutes, Hourly, BiHourly, Daily, Weekly, Monthly

    var transformedCandles = candles.Transform<Daily, Weekly>();
[Back to content](#Content)

<a name="ComputeIndicators"></a>
#### Compute indicator
    // This library supports computing from tuples or candles, extensions are recommended for computing
    var closes = new List<decimal>{ ... };
    var smaTs = closes.Sma(30, startIndex, endIndex);
    var sma = closes.Sma(30)[index];

    // or, traditional call
    var sma = new SimpleMovingAverageByTuple(closes, 30)[index];
    
    // the corresponding version of candle
    var sma = new SimpleMovingAverage(candles, 30)[index];
[Back to content](#Content)

<a name="ComputeIndicatorsOperation"></a>
#### Compute simple operation on an indicator
    // Simple operation on indicator is supported, now supports only diff & sma
    var closes = new List<decimal>{ ... };
    var smaDiff = closes.Sma(30).Diff(index);
    var smaSma = closes.Sma(30).Sma(10, index);

    // This also applies to candles
[Back to content](#Content)

<a name="ConvertFunctionToAnalyzable"></a>
#### Convert function to indicator
    // Sometimes, we want to utilize the Analyzable infra for some simple indicators but doesn't want to implement a new class, we are adding AsAnalyzable for conversion from Func

    // Before conversion, on the very top of your file, you should add
    using AFunc = System.Func<System.Collections.Generic.IReadOnlyList<Trady.Core.Candle>, int, decimal?>;

    // And use it in your code
    AFunc aFunc = (c, i) => c[i].High - c[i].Low;
    var aFuncInstance = aFunc.AsAnalyzable(candles);
    var a = aFuncInstance[index];

    // You may also combine with simple operation computation
    var aSma = aFuncInstance.Sma(index);
[Back to content](#Content)

<a name="CaptureSignalByRules"></a>
#### Capture signals by rules
    // The following shows the number of candles that fulfill both the IsAboveSma(30) & IsAboveSma(10) rule
    var rule = Rule.Create(c => c.IsAboveSma(30))
        .And(c => c.IsAboveSma(10));

    // Use context here for caching indicator results
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
        public static bool IsEma10Rising(this IndexedCandle ic)
            => ic.Get<ExponentialMovingAverage>(10).Diff(ic.Index).Tick.IsPositive();

        public static bool IsEma10Dropping(this IndexedCandle ic)
            => ic.Get<ExponentialMovingAverage>(10).Diff(ic.Index).Tick.IsNegative();

        public static bool IsEma10BullishCrossEma30(this IndexedCandle ic)
            => ic.Get<ExponentialMovingAverageOscillator>(10, 30).ComputeNeighbour(ic.Index).IsTrue(
                (prev, current, next) => prev.Tick.IsNegative() && current.Tick.IsPositive());

        public static bool IsEma10BearishCrossEma30(this IndexedCandle ic)
            => ic.Get<ExponentialMovingAverageOscillator>(10, 30).ComputeNeighbour(ic.Index).IsTrue(
                (prev, current, next) => prev.Tick.IsPositive() && current.Tick.IsNegative());
    }

    // Use case
    var buyRule = Rule.Create(c => c.IsEma10BullishCrossEma30()).And(c.IsEma10Rising());
    var sellRule = Rule.Create(c => c.IsEma10BearishCrossEma30()).Or(c.IsEma10Dropping());
    var runner = new Builder().Add(candles, 10).Buy(buyRule).Sell(sellRule).Build();
    var result = await runner.RunAsync(10000, 1);
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

### Backlog
* (High priority) Dynamically create indicator & rule patterns from text file, allows internal call to dynamic generated stuffs
* Complete other indicators (e.g. Keltner Channels, MA Envelopes, etc.)
* (Low priority) Complete candlestick patterns
* Add more rule patterns
* Integrate with machine learning for creating prediction model?
* Add broker adaptor for real-time trade (e.g. interactive broker, etc.)
* MORE, MORE AND MORE!!!!

### Powered by
* [CsvHelper](https://github.com/JoshClose/CsvHelper) ([@JoshClose](https://github.com/JoshClose)) : Great library for reading/ writing CSV file

### License
This library is under [Apache-2.0 License](https://github.com/lppkarl/Trady/blob/master/LICENSE)
