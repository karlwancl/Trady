[Home](index.md) | [Release Notes](release_notes.md) | [Indicators](indicators.md) | [Candlestick patterns](candlesticks.md) | [Rule patterns](rule_patterns.md)

# Indicator Usage

This library supports computing indicators from tuples or candles, extensions methods are recommended for computing indicators from an IEnumberable.

    var candles = new List<IOhlcv>();
    var results = candles.Sma(30); //Extension methods on List<IOhlcv>

### There are various ways to calculate indicators.

    var closes = new List<decimal>{ ... };
    var smaTs = closes.Sma(30);
    var sma = closes.Sma(30)[index];


### Using the indicator class itself with Tuples

    var sma = new SimpleMovingAverageByTuple(closes, 30)[index];
    
### The corresponding version of candle
    var sma = new SimpleMovingAverage(candles, 30)[index];

## [Supported Indicators](indicators.md)

