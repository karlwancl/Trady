# Update from 2.0.x to 3.0.0

## Trady.Analysis module
* Change: 
    * The library is re-targeted to .NET Standard 2.0
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
    * Introduce FuncRegistry & RuleRegistry for global access to dynamically generated indicator/rule, it will be extending to provide flexibility on loading indicator/rule from external files (in plain text)
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

## Trady.Importer module
* The library is re-targeted to .NET Standard 2.0
* The candles are exported as IReadOnlyList<Candle> instead of IList<Candle>
* Added GoogleFinanceImporter (adapter to [Nuba.Google.Finance](https://github.com/nubasoftware/Nuba.Finance.Google), thanks to [@fernaramburu](https://github.com/fernaramburu))
* Separated importers are also available for modular installation:
    * Trady.Importer.Csv
    * Trady.Importer.Yahoo
    * Trady.Importer.Quandl
    * Trady.Importer.Stooq
    * Trady.Importer.Google
* Align importer's behavior, startTime & endTime is inclusive now for all importers
