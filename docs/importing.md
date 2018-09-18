[Home](index.md) | [Release Notes](release_notes.md) | [Indicators](indicators.md) | [Candlestick patterns](candlesticks.md) | [Rule patterns](rule_patterns.md)

# Importing Data
Importing data usually consists of candle stick (Open, High, Close, Low, Volume) data from some data source such as a CSV file, a web service or API, or from a local database.  Trady does not care about the source and offers flexible solutions to importing your data.

## The IOhlcv Interface
Everything revolves around the IOhlcv interface in the Trady.Core package.  The requires you to create a class that implements this interface.  This allows you to customize this class to your needs without any hard requirements from Trady regarding customized candlestick logic.  You have control over implementation.

### Step 1: Create Your Custom Candlestick Class
    public class Candle : IOhlcv
    {
        public Candle(DateTimeOffset dateTime, decimal open, decimal high, decimal low, decimal close, decimal volume)
        {
            DateTime = dateTime;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public DateTimeOffset DateTime { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }
    }

### Step 2: Use one the of the built-in importers to import your candlestick data.

    // From Quandl wiki database
    var importer = new QuandlWikiImporter(apiKey);

    // From Yahoo! Finance
    var importer = new YahooFinanceImporter();

    // From Stooq
    var importer = new StooqImporter();

    // From AlphaVantage
    var importer = new AlphaVantageImporter(apiKey);

    // From Google Finance (Temporarily not available now)
    // var importer = new GoogleFinanceImporter();

    // Or from dedicated csv file
    var importer = new CsvImporter("FB.csv");

### Step 3: Import the data to a local variable

    // Get stock data from the above importer
    var candles = await importer.ImportAsync("FB");

## Create your own custom importer
You can implement your own importer by implementing the IImporter interface found in the Trady.Core package (Trady.Core.Infrastructure namespace).

    public class MyImporter : IImporter
    {
        public async Task<IReadOnlyList<IOhlcv>> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            // Your implementation to return a list of candles
        }
    }
    
    // Use case
    var importer = new MyImporter();
    var candles = await importer.ImportAsync("FB");

## Using the CSV Importer
The CSV importer allows for custom configuration regarding the format of the CSV file.

The CsvImportConfiguration class allows options for date formats, culture, delimiter and header records.

    var config = new CsvImportConfiguration()
    {
        Culture = "en-US",
        Delimiter = ";",
        DateFormat = "yyyyMMdd HHmmss",
        HasHeaderRecord = false
    };

Once the configuration is defined, pass in the configuration to the CsvImporter's constructor.

    var importer = new CsvImporter("EURUSD.csv", config);
    var candles = await importer.ImportAsync("EURUSD");