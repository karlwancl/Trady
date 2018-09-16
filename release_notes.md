# Release Notes

[v3.2](#v32) | [v3.1](#v31) | [v3.0.1](#v301)

<a name="v32"></a>
## v3.2
* Added WeightedMovingAverage, HullMovingAverage & KeltnerChannels
* Added AlphaVantage importer (thanks to @irperez)
* Reactivate support for QuandlImporter
* Boost performance on date time transformation (thanks to @pavlexander)
* Boost performance for various indicators (HistoricalHighest/HistoricalLowest/EmaOsc/Macd/ADLine/Obv/ParabolicSar, etc.)
* Update dependencies for importers
* Remove redundant sdCount parameter for Sd operation
* EffeciencyRatio & Kama now accepts nullable decimal as input
* Allow use of simple operation for ParabolicSar
* Renamed simple operation "PcDiff" to "RDiff"

<a name="v31"></a>
## v3.1
* Fix StooqImporter, migrated to .NET Standard 2.0
* Temporarily remove support for QuandlImporter & GoogleFinanceImporter
* Fix divide by zero issue for various indicators
* Fix null reference to diff/pcdiff/sma, etc.
* Fix YahooFinanceImporter to use local time for query
* Update dependencies for csvImporter
* Boost performance for RuleExecutor & Backtesting
* Added Harami (thanks to @richardsjoberg)
* Added indicators: ParabolicStopAndReverse (Sar), DynamicMomentumIndex(Dymoi), RelativeMomentumIndex (Rmi), NetMomentumOscillator (Nmo), StochasticsRsiOscillator (StochRsi), StochasticsMomentumIndex (Smi), CommodityChannelIndex (Cci)
* IOhlcv interface is extracted, any class that implements IOhlcv interface can be used to calculate indicators (thanks to @LadislavBohm)
* DateTimeOffset is used as default instead of DateTime
* Renamed IndexedCandle.Execute to IndexedCandle.Eval
* Renamed ClosePriceChange to Momentum (Mtm), ClosePricePercentageChange to RateOfChange (Roc)
* Fix potential crash when computing EfficiencyRatio (thanks to @Mike-EEE)

<a name="v301"></a>
## v3.0.1
* Fixed potential crash when doing backtest (Thanks for @LadislavBohm)
