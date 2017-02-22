# Trady
Trady is a handy library for computing technical indicators, and targets to be an automated trading system that provides stock data feeding, indicator computing, strategy building and automatic trading. It is built based on .NET Standard 1.6.1.

### Read Before You Use
This library is intended for personal use, use with care for production environment.

### Currently Available Features
* Stock data feeding (via [Quandl.NET](https://github.com/salmonthinlion/Quandl.NET), [YahooFinanceApi](https://github.com/salmonthinlion/YahooFinanceApi))
* Indicator computing (including SMA, EMA, RSI, MACD, BB, etc.)
* Strategy building & backtesting

### Supported Platforms
* .NET Core 1.0 or above
* .NET Framework 4.6.1 or above
* Xamarin.iOS
* Xamarin.Android
* Universal Windows Platform 10.0 or above

### How To Install
Nuget package is available in modules, please install the package according to the needs

    PM > Install-Package Trady.XXXXX

### How To Use
<a name="Content"></a>
* Importing (Requires Trady.Importer)
    * [Import Stock Data](#ImportStockData)
    * [Implement Your Own Importer](#ImplementYourOwnImporter)
    
* Computing (Requires Trady.Analysis)
    * [Transform Stock Data](#TransformStockData)
    * [Compute Indicators](#ComputeIndicators)
    * [Implement Your Own Indicator - Simple Type](#ImplementYourOwnIndicatorSimpleType)
    * [Implement Your Own Indicator - Cummulative Type](#ImplementYourOwnIndicatorCummulativeType)
    * [Implement Your Own Indicator - Moving Average Type](#ImplementYourOwnIndicatorMovingAverageType)
    * [IndicatorResult cache through IIndicatorResultProvider](#DataProviderCache)
    
* Exporting (Requires Trady.Exporter)
    * [Export Indicators](#ExportIndicators)
    * [Implement Your Own Exporter](#ImplementYourOwnExporter)
    
* Backtesting (Requires Trady.Strategy)
    * [Strategy Building And Backtesting](#StrategyBuildingAndBacktesting)
    * [Implement Your Own Pattern](#ImplementYourOwnPattern)


<a name="ImportStockData"></a>
#### Import stock data (Requires Trady.Importer module)
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
#### Implement your own importer (Requires Trady.Importer module)
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
#### Compute indicators (Requires Trady.Analysis module)
    // Supported indicators: Sma, Ema, Rsi, Macd, Sto(Fast, Full, Slow), Obv, AccumDist, Bb, Atr, Dmi, Aroon, Chandlr, Ichimoku & some related indicators (i.e. HighestHigh, LowestLow, etc.)

    // There are several ways to compute indicators
    1. By indicator extension (Some common indicators only, provide caching for indicator instance)
        var smaTs = equity.Sma(30, startTime, endTime);
        
    2. By indicator locator (provide caching for indicator instance)
        var smaTs = equity.GetOrCreateAnalytic<SimpleMovingAverage>(30).Compute(startTime, endTime);

    3. By instantiation (no caching for indicator instance, better for one-off usage)
        var smaTs = new SimplemMovingAverage(equity, 30).Compute(startTime, endTime);
[Back to content](#Content)

<a name="ImplementYourOwnIndicatorSimpleType"></a>
#### Implement your own indicator - Simple Type (Requires Trady.Analysis module)
    // You can also implement your own indicator by extending the IndicatorBase<IndicatorResult> class
    public class MyIndicator : IndicatorBase<IndicatorResult>
    {
        private SimpleMovingAverage _smaIndicator;

        // The constructor should have an equity parameter and the indicator integer parameters
        public MyIndicator(Equity equity, int param1, int param2, int param3) : base(equity, param1, param2, param3)
        {
            // Your construction code here

            // You can make use of other indicators for computing your own indicator, and you should register your first-level dependencies through RegisterDependents method
            _smaIndicator = new SimpleMovingAverage(equity, param1);
            RegisterDependents(_smaIndicator);
        }

        protected override IndicatorResult ComputeByIndexImpl(int index)
        {
            // Your indicator implementation to return your own IndicatorResult
        }

        // IndicatorResult class that store the result from the indicator (depends on outer class)
        public class IndicatorResult : TickBase
        {
            // The constructor should be public and have an dateTime parameter and the values (must be in decimal? type)
            public IndicatorResult(DateTime dateTime, decimal? value1, decimal? value2) : base(dateTime)
            {
                Value1 = value1;
                Value2 = value2;
            }

            // Must have public property for getting the indicator value
            public decimal? Value1 { get; private set; }

            public decimal? Value2 { get; private set; }
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
#### Implement your own indicator - Cummulative Type (Requires Trady.Analysis module)
    // You can implement your own indicator by extending the CachedIndicatorBase<IndicatorResult> class
    public class MyCummulativeIndicator : CachedIndicatorBase<IndicatorResult>
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
#### Implement your own indicator - Moving-Average Type (Requires Trady.Analysis module)
    // You can make use of the GenericExponentialMovingAverage class to get rid of implementing MA-related indicator on your own
    public class MyMAIndicator : IndicatorBase<IndicatorResult>
    {
        private GenericExponentialMovingAverage _gemaIndicator;

        public MyMAIndicator(Equity equity, int periodCount) : base(equity, periodCount)
        {
            // The constructor intakes the following parameters:
            //  1. Equity instance
            //  2. First Value Index
            //  3. First Value Function
            //  4. Index Value Function
            //  5. Period Count for Moving Average
            //  6. Moving Average Type (true = MMA, false = EMA)

            _gemaIndicator = new GenericExponentialMovingAverage(
                equity,
                firstValueIndex,
                i => firstValueFunction,
                i => indexValueFunction,
                periodCount,
                true
            );

            RegisterDependents(_gemaIndicator);
        }

        // The rest is the same as Simple Type...
    }
[Back to content](#Content)

<a name="DataProviderCache"></a>
#### IndicatorResult cache through IIndicatorResultProvider (Requires Trady.Analysis module)
    // You may want to implement the IIndicatorResultProvider interface to do the following things:
    // 1. Compute cummulative indicator that make use of pre-calculated values from external data source (e.g. database)
    // 2. Directly retrieve the indicator results from external data source (e.g. database) without re-calculation

    // Let's say we want to retrieve computed data from database, for accessing database, we will use EntityFramework here as an example
    public class MyIndicatorResultProvider : IIndicatorResultProvider
    {
        private IIndicator _indicator;
        private ApplicationDbContext _context;
        private DatabaseEquity _dbEquity;
        private DatabaseIndicator _dbIndicator;

        public MyIndicatorResultProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        // Indicate if the data store (database) contains the equity record
        bool IsEquityExists => _dbEquity != null;

        // Indicate if the data store (database) contains the indicator record
        bool IsIndicatorExists => _dbIndicator != null;

        // For use when the indicator has dependencies registered, the provider is cloned along the hierarchy
        public IIndicatorResultProvider Clone() => new MyIndicatorResultProvider(_context);

        // Initialization stub, intends to reduce the number of data query from external data source (database)
        public async Task InitWithIndicatorAsync(IIndicator indicator)
        {
            _indicator = indicator;
            _dbEquity = await _context.Equities.FirstOrDefaultAsync(e => e.Name == indicator.Equity.Name);
            _dbIndicator = await _context.Indicators.FirstOrDefaultAsync(i => i.Name == indicator.GetType().Name && i.Parameter1 == indicator.Parameters[0]);
        }

        // Main stub for data retrieval, all async method uses in this method should add a ConfigureAwait(false) to prevent deadlocks
        // The TTick generic parameter here is for Indicator's IndicatorResult class
        public async Task<(bool HasValue, TTick Value)> GetAsync<TTick>(DateTime dateTime) where TTick : ITick
        {
            // It returns a list of value because there may be more than one property value for an indicator, for instance, BollingerBands has 4 property values - LowerBand, MiddleBand, UpperBand, BandWidth
            var values = await _context.Values.Where(v => v.Equity == _dbEquity && v.Indicator == _dbIndicator && v.DateTime == dateTime).ToListAsync().ConfigureAwait(false);
            if (!values.Any())
                return (false, default(TTick));
            
            // Create arguments based on the retrieved values
            var args = new List<object> { dateTime };

            // Reflection is used here for getting the default constructor of the TTick class
            var ctor = typeof(TTick).GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).First();

            // Get parameters in the constructor & map the values to the parameters
            foreach (var @param in ctor.GetParameters())
            {
                if (@param.Name.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                    continue;
                var value = values.FirstOrDefault(v => @param.Name.Replace("@","").Equals(v.Name, StringComparison.OrdinalIgnoreCase));
                if (value != null)
                    args.Add(value.Value);
            }

            // Create and return TTick(IndicatorResult) instance indicator for further computation
            return (true, (TTick)ctor.Invoke(args.ToArray()));
        }
    }

    // Use case
    var context = _serviceProvider.GetService<ApplicationDbContext>();  // Get ApplicationDbContext through service locator
    var smaIndicator = equity.GetOrCreateAnalytic<SimpleMovingAverage>(30);
    var provider = new MyIndicatorResultProvider(context);
    await smaIndicator.InitWithIndicatorResultProviderAsync(provider);
[Back to content](#Content)

<a name="ExportIndicators"></a>
#### Export computed indicators to CSV (Requires Trady.Exporter module)
    var tsList = new List<ITimeSeries> {smaTs, emaTs, bbTs};
    var exporter = new CsvExporter("tss.csv");
    bool success = await exporter.ExportAsync(equity, tsList, ascending: false);
[Back to content](#Content)

<a name="ImplementYourOwnExporter"></a>
#### Implement your own exporter (Requires Trady.Exporter module)
    // You can also implement your own exporter by extending the ExporterBase abstract class
    public class MyExporter
    {
        public async Task<bool> ExportAsync(Equity equity, IList<ITimeSeries> resultTimeSeriesList, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), bool ascending = false, CancellationToken token = default(CancellationToken))
        {
            // Your implementation to export indicators values
        }
    }
    
    // Use case
    var exporter = new MyExporter();
    bool success = await exporter.ExportAsync(equity, tsList, ascending: false);
[Back to content](#Content)

<a name="StrategyBuildingAndBacktesting"></a>
#### Strategy building & backtesting (Requires Trady.Strategy module)
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
    var result = await portfolio.RunAsync(10000, 1);

    // Get backtest result for the portfolio
    Console.WriteLine(string.Format("Transaction count: {0:#.##}, P/L ratio: {1:0.##}%, Principal: {2:#}, Total: {3:#}",
        result.TransactionCount,
        result.ProfitLossRatio * 100,
        result.Principal,
        result.Total));
[Back to content](#Content)

<a name="ImplementYourOwnPattern"></a>
#### Implement your own pattern through Extension (Requires Trady.Strategy module)
    // Implement your pattern by creating a static class for extending AnalyzableCandle class
    public static class AnalyzableCandleExtension
    {
        public static bool IsSma30LargerThanSma10(this AnalyzableCandle candle)
        {
            var sma30 = candle.Equity.GetOrCreateAnalytic<SimpleMovingAverage>(30).ComputeByIndex(candle.Index);
            var sma10 = candle.Equity.GetOrCreateAnalytic<SimpleMovingAverage>(10).ComputeByIndex(candle.Index);
            return sma30.Sma > sma10.Sma;
        }
        
        public static bool IsSma10LargerThanSma30(this AnalyzableCandle candle)
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
    var result = await portfolio.RunAsync(10000, 1);
[Back to content](#Content)

### Backlog
* Complete other indicators (e.g. Keltner Channels, KAMA, MA Envelopes, etc.)
* Complete candlestick patterns (Low priority)
* Add more indicator filtering patterns (Add patterns on demand)
* Add broker adaptor for real-time trade (e.g. interactive broker, etc.)
* MORE, MORE AND MORE!!!!

### Powered by
* [CsvHelper](https://github.com/JoshClose/CsvHelper) ([@JoshClose](https://github.com/JoshClose)) : Great library for reading/ writing CSV file

### License
This library is under [Apache-2.0 License](https://github.com/salmonthinlion/Trady/blob/master/LICENSE)
