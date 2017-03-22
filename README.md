# Trady
Trady is a handy library for computing technical indicators, and targets to be an automated trading system that provides stock data feeding, indicator computing, strategy building and automatic trading. It is built based on .NET Standard 1.6.1.

### Read Before You Use
This library is intended for personal use, use with care for production environment.

### Currently Available Features
* Stock data feeding (via [Quandl.NET](https://github.com/salmonthinlion/Quandl.NET), [YahooFinanceApi](https://github.com/salmonthinlion/YahooFinanceApi))
* Indicator computing (including SMA, EMA, RSI, MACD, BB, etc.)
* Strategy building & backtesting

### Recent Update (22nd Mar, 2017)
* Added test project
* (Breaking-Change) Candle's volume property uses decimal instead of long
* (Breaking-Change) GenericExponentialMovingAverage now intakes smoothing factor function instead of modified flag & periodCount
* (Breaking-Change) GetValueTicks from TickProviderBase now intakes startTime instead of nothing, GetAllAsync method from ITickProvider is also changed to GetAsync(DateTime startTime)
* Fix indicators: BbWidth, Adxr, IchimokuCloud, Atr
* Add indicators: Er, Kama, Mema, Sd

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
    * [IndicatorResult cache through TickProviderBase](#DataProviderCache)
    
* Backtesting
    * [Strategy Building And Backtesting](#StrategyBuildingAndBacktesting)
    * [Implement Your Own Pattern](#ImplementYourOwnPattern)

<a name="tldr"></a>
### Tldr
    var importer = new QuandlWikiImporter(apiKey);
    var equity = await importer.ImportAsync("FB");
    Console.WriteLine(equity.Sma(30)[equity.Count - 1].Sma);
[Back to content](#Content)

<a name="ImportStockData"></a>
#### Import stock data
    // From Quandl wiki database
    var importer = new QuandlWikiImporter(apiKey);

    // From Yahoo Finance
    var importer = new YahooFinanceImporter();

    // Or from dedicated csv file
    var importer = new CsvImporter("FB.csv");

    // Get stock data from the above importer
    var equity = await importer.ImportAsync("FB");
[Back to content](#Content)

<a name="ImplementYourOwnImporter"></a>
#### Implement your own importer
    // You can also implement your own importer by implementing the IImporter interface
    public class MyImporter : IImporter
    {
        public async Task<Equity> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            // Your implementation to return Equity instance
        }
    }
    
    // Use case
    var importer = new MyImporter();
    var equity = await importer.ImportAsync("FB");
[Back to content](#Content)

<a name="TransformStockData"></a>
#### Transform stock data to specified period before computation
    // Transform the series for computation, downcast is forbidden
    // Supported period: PerSecond, PerMinute, Per15Minutes, Per30Minutes, Hourly, BiHourly, Daily, Weekly, Monthly

    var transformedEquity = equity.Transform(PeriodOption.Weekly);
[Back to content](#Content)

<a name="ComputeIndicators"></a>
#### Compute indicators
    // There are several ways to compute indicators
    1. By indicator extension (Some common indicators only, provide caching for indicator instance)
        var smaTs = equity.Sma(30, startTime, endTime);
        
    2. By indicator locator (provide caching for indicator instance)
        var smaTs = equity.GetOrCreateAnalytic<SimpleMovingAverage>(30).Compute(startTime, endTime);
        // or with tick provider
        var smaTs = await equity.GetOrCreateAnalyticWithTickProviderAsync<SimpleMovingAverage>(30).Compute(startTime, endTime);

    3. By instantiation (no caching for indicator instance, better for one-off usage)
        var smaTs = new SimplemMovingAverage(equity, 30).Compute(startTime, endTime);
[Back to content](#Content)

<a name="ImplementYourOwnIndicatorSimpleType"></a>
#### Implement your own indicator - Simple Type
    // You can also implement your own indicator by extending the IndicatorBase<IndicatorResult> class
    public class MyIndicator : IndicatorBase<IndicatorResult>
    {
        private SimpleMovingAverage _smaIndicator;

        // The constructor should have an equity parameter and the indicator integer parameters
        public MyIndicator(Equity equity, int param1, int param2, int param3) : base(equity, param1, param2, param3)
        {
            // Your construction code here

            // You can make use of other indicators for computing your own indicator
            _smaIndicator = new SimpleMovingAverage(equity, param1);
        }

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            // Your indicator implementation to return your own IndicatorResult
        }

        // IndicatorResult class that store the result from the indicator (depends on outer class)
        public class IndicatorResult : IndicatorResultBase
        {
            // The constructor should be public and have an dateTime parameter and the values (must be in decimal? type)
            public IndicatorResult(DateTime dateTime, decimal? value1, decimal? value2) : base(dateTime, value1, value2)
            {
            }

            // Must have public property for getting the indicator value
            public decimal? Value1 => Values[0];

            public decimal? Value2 => Values[1];
        }
    }

    // Use case
    var myIndicator = equity.GetOrCreateAnalytic<MyIndicator>(1, 2, 3);
    var myIndicatorTs = myIndicator.Compute(startTime, endTime);
    foreach (var value in myIndicatorTs)
    {
        Console.WriteLine($"{value.Value1},{value.Value2}");
    }
[Back to content](#Content)

<a name="ImplementYourOwnIndicatorCummulativeType"></a>
#### Implement your own indicator - Cummulative Type
    // You can implement your own indicator by extending the CummulativeIndicatorBase<IndicatorResult> class
    public class MyCummulativeIndicator : CummulativeIndicatorBase<IndicatorResult>
    {
        // The constructor should have an equity parameter and the indicator integer parameters
        public MyCummulativeIndicator(Equity equity, int param1): base(equity, param1)
        {
            // Your construction code here
        }

        // The first index value that needs calculation
        protected override int FirstIndexValue => 0;

        // The computation method for computing indicator when index < FirstIndexValue
        protected override IndicatorResult ComputeNullValue(int index)
        {
            // Your implementation to return IndicatorResult
            // Typically, it should return new IndicatorResult(Equity[index].DateTime, null);
        }

        // The computation method for computing indicator when index == FirstIndexValue
        protected override IndicatorResult ComputeFirstValue(int index)
        {
            // Your implementation to return IndicatorResult
            // Typically, it should be calculated by the previous n terms
        }

        // The computation method for computing indicator when index > FirstIndexValue
        protected override IndicatorResult ComputeIndexValue(int index, IndicatorResult prevTick)
        {
            // Your implementation to return IndicatorResult
            // You can use the prevTick instance provided by the cache to calculate the new IndicatorResult here
        }
        
        // The rest is the same as Simple Type...
    }

    // Use case
    var myIndicator = equity.GetOrCreateAnalytic<MyCummulativeIndicator>(1);
    var myIndicatorTs = myIndicator.Compute(startTime, endTime);
    foreach (var value in myIndicatorTs)
    {
        Console.WriteLine($"{value.Value1},{value.Value2}");
    }   
[Back to content](#Content)

<a name="ImplementYourOwnIndicatorMovingAverageType"></a>
#### Implement your own indicator - Moving-Average Type
    // You can make use of the GenericExponentialMovingAverage class to get rid of implementing MA-related indicator on your own
    public class MyMAIndicator : IndicatorBase<IndicatorResult>
    {
        private GenericExponentialMovingAverage _gemaIndicator;

        public MyMAIndicator(Equity equity, int periodCount) : base(equity, periodCount)
        {
            // The constructor intakes the following parameters:
            //  1. Equity instance
            //  2. Initial Value Index
            //  3. Inital Value Function
            //  4. Index Value Function
            //  5. Smoothing Factor Function (1.0m / periodCount for Mema, 2.0m / (periodCount + 1.0m) for Ema)

            _gemaIndicator = new GenericExponentialMovingAverage(
                equity,
                firstValueIndex,
                i => firstValueFunction,
                i => indexValueFunction,
                i => smoothingFactorFunction
            );
        }

        // The rest is the same as Simple Type...
    }
[Back to content](#Content)

<a name="DataProviderCache"></a>
#### IndicatorResult cache through TickProviderBase
    // You may want to extend the TickProviderBase class to do the following things:
    // 1. Compute cummulative indicator that make use of pre-calculated values from external data source (e.g. database)
    // 2. Directly retrieve the indicator results from external data source (e.g. database) without re-calculation

    // Let's say we want to retrieve computed data from database, for accessing database, we assume there is an UnitOfWork instance to inject
    public class TickProvider : TickProviderBase
    {
        private DbEquity _dbEquity;
        private DbIndicator _dbIndicator;

        // Indicate if database is ready for data retrieval
        public override bool IsReady => _dbEquity != null && _dbIndicator != null;

        // Must put all the input parameters to base class for cloning
        public TickProvider(IUnitOfWork uow)
            : base(uow)
        {
        }

        // You can get the input values from _parameters protected field
        private IUnitOfWork Uow => (IUnitOfWork)_parameters[0];

        // Do what you want for initialization i.e. check if the equity is available for querying, etc. Must call base method before any additional implementation
        // The analyzable object here will either be an indicator instance or pattern instance
        public override async Task InitWithAnalyzableAsync(IAnalyzable analyzable)
        {
            await base.InitWithAnalyzableAsync(analyzable);
            _dbEquity = await Uow.EquityRepository.GetEquityAsync(_analyzable.Equity.Name);
            
            // For pattern instance, the Parameters property is absent while indicator instance has the Parameters property
            int[] @params = typeof(IIndicator).GetTypeInfo().IsAssignableFrom(analyzable.GetType()) ? ((IIndicator)analyzable).Parameters : new int[] { };

            _dbIndicator = await Uow.IndicatorRepository.GetIndicatorAsync(_analyzable.GetType().Name, @params);
        }

        // Retrieve all values from database for an equity with an indicator, you must also implement IValueTick interface for your value class
        protected override async Task<IEnumerable<IValueTick>> GetValueTicks(DateTime startTime)
            => await Uow.IndicatorRepository.GetValuesInDateTimeRangeAsync(_dbEquity, _dbIndicator, startTime, null);     
    }

    // Use case
    var context = _serviceProvider.GetService<IUnitOfWork>();  // Get IUnitOfWork instance through service locator
    var smaIndicator = equity.GetOrCreateAnalytic<SimpleMovingAverage>(30);
    var provider = new TickProvider(context);
    await smaIndicator.InitWithTickProviderAsync(provider);
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
    // Implement your pattern by creating a static class for extending IndexCandle class
    public static class IndexCandleExtension
    {
        public static bool IsSma30LargerThanSma10(this IndexCandle candle)
        {
            var sma30 = candle.Equity.GetOrCreateAnalytic<SimpleMovingAverage>(30).ComputeByIndex(candle.Index);
            var sma10 = candle.Equity.GetOrCreateAnalytic<SimpleMovingAverage>(10).ComputeByIndex(candle.Index);
            return sma30.Sma > sma10.Sma;
        }
        
        public static bool IsSma10LargerThanSma30(this IndexCandle candle)
        {
            var sma30 = candle.Equity.GetOrCreateAnalytic<SimpleMovingAverage>(30).ComputeByIndex(candle.Index);
            var sma10 = candle.Equity.GetOrCreateAnalytic<SimpleMovingAverage>(10).ComputeByIndex(candle.Index);
            return sma10.Sma > sma30.Sma;
        }
    }

    // Use case
    var buyRule = Rule.Create(c => c.IsSma10LargerThanSma30());
    var sellRule = Rule.Create(c => c.IsSma30LargerThanSma10());
    var portfolio = new PortfolioBuilder().Add(equity, 10).Buy(buyRule).Sell(sellRule).Build();
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
This library is under [Apache-2.0 License](https://github.com/salmonthinlion/Trady/blob/master/LICENSE)
