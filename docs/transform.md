[Home](index.md) | [Release Notes](release_notes.md) | [Indicators](indicators.md) | [Candlestick patterns](candlesticks.md) | [Rule patterns](rule_patterns.md)

# Transforming Stock Data

Candlestick data can be converted to a different period type (Monthly, Weekly, Daily, etc) before any indicator calculations are done.  This ability allows for you to save on costly API calls.

NOTE: Transforming the data in a downcast manner is forbidden (Monthly -> Daily)

Supported options are:
* PerSecond
* PerMinute
* Per15Minutes
* Per30Minutes
* Hourly
* BiHourly
* Daily
* Weekly
* Monthly



        // Transform<Source, Target>        
        var weeklyCandles = dailyCandles.Transform<Daily, Weekly>();