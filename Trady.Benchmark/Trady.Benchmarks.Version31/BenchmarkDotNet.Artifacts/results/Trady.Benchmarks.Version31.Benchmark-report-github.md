``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18363
Intel Core i5-9400 CPU 2.90GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=3.1.100
  [Host]     : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
  Job-QQTCKZ : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                             Method |         Mean |       Error |      StdDev |            P90 |
|----------------------------------- |-------------:|------------:|------------:|---------------:|
|                 TransformToMonthly |     380.2 us |     2.15 us |     2.01 us |       382.4 us |
|                   TransformToDaily |     386.8 us |     3.23 us |     3.02 us |       390.1 us |
|              RelativeStrengthIndex |   3,138.0 us |    35.73 us |    31.67 us |     3,169.0 us |
|           ExponentialMovingAverage |   1,232.6 us |     4.59 us |     3.58 us |     1,237.3 us |
|                SimpleMovingAverage |   1,112.7 us |    24.71 us |    33.82 us |     1,160.1 us |
|                               MACD |   2,931.1 us |    31.76 us |    29.71 us |     2,958.8 us |
|              CommodityChannelIndex |  12,855.9 us |   161.08 us |   150.68 us |    13,060.9 us |
|            StochasticMomentumIndex |   5,697.7 us |    43.13 us |    40.34 us |     5,746.6 us |
|    StochasticRelativeStrengthIndex |   6,553.4 us |   131.48 us |   216.03 us |     6,911.7 us |
|              NetMomentumOscillator |   3,429.9 us |    42.72 us |    37.87 us |     3,488.9 us |
|                   RelativeMomentum |   2,752.1 us |    22.59 us |    21.13 us |     2,782.1 us |
|              RelativeMomentumIndex |   3,205.0 us |    63.36 us |    67.79 us |     3,286.7 us |
|               DynamicMomentumIndex | 999,726.4 us | 9,389.40 us | 8,782.85 us | 1,007,662.7 us |
|            ParabolicStopAndReverse |     967.2 us |     9.93 us |     9.29 us |       980.0 us |
|                             Median |   2,474.2 us |    33.02 us |    30.89 us |     2,511.5 us |
|                         Percentile |   3,176.2 us |    39.41 us |    36.86 us |     3,227.2 us |
|       AccumulationDistributionLine |     874.8 us |     7.07 us |     6.61 us |       882.9 us |
|                              Aroon |   4,595.1 us |    64.90 us |    60.71 us |     4,674.2 us |
|                    AroonOscillator |   4,837.2 us |    66.59 us |    62.29 us |     4,903.6 us |
|                   AverageTrueRange |   1,539.3 us |    14.30 us |    13.37 us |     1,557.0 us |
|                     BollingerBands |   3,234.2 us |     8.54 us |     7.57 us |     3,245.8 us |
|                 BollingerBandWidth |   3,526.0 us |    17.95 us |    14.99 us |     3,540.1 us |
|                     ChandelierExit |   2,843.6 us |    30.17 us |    28.22 us |     2,882.1 us |
|                           Momentum |     608.1 us |     3.25 us |     2.88 us |       611.8 us |
|                       RateOfChange |     684.1 us |     5.30 us |     4.96 us |       690.4 us |
|           PlusDirectionalIndicator |   2,974.9 us |    22.82 us |    20.23 us |     2,989.3 us |
|          MinusDirectionalIndicator |   2,994.1 us |    12.01 us |    10.64 us |     3,008.7 us |
|            AverageDirectionalIndex |   6,906.1 us |    65.08 us |    60.87 us |     6,963.4 us |
|      AverageDirectionalIndexRating |   7,302.0 us |    53.71 us |    50.24 us |     7,379.1 us |
|                    EfficiencyRatio |   1,179.1 us |     4.31 us |     4.03 us |     1,183.6 us |
|       KaufmanAdaptiveMovingAverage |   2,571.1 us |    15.97 us |    14.94 us |     2,587.0 us |
| ExponentialMovingAverageOscillator |   2,202.2 us |    13.10 us |    10.94 us |     2,212.6 us |
|                        HighestHigh |     774.7 us |     4.09 us |     3.41 us |       777.1 us |
|                       HighestClose |     776.4 us |     7.08 us |     6.62 us |       785.3 us |
|              HistoricalHighestHigh |     672.6 us |     3.29 us |     2.74 us |       675.5 us |
|             HistoricalHighestClose |     685.6 us |    10.92 us |    10.21 us |       700.2 us |
|                          LowestLow |     814.6 us |     4.73 us |     4.42 us |       819.1 us |
|                        LowestClose |     764.7 us |     9.64 us |     9.02 us |       776.2 us |
|                HistoricalLowestLow |     692.3 us |    11.23 us |    10.50 us |       701.4 us |
|              HistoricalLowestClose |     688.2 us |     4.83 us |     4.52 us |       693.6 us |
|                      IchimokuCloud |   5,308.7 us |   104.71 us |   116.39 us |     5,468.8 us |
|              ModifiedMovingAverage |   1,273.5 us |    10.93 us |    10.22 us |     1,283.9 us |
|                           MacdHist |   3,141.8 us |    13.99 us |    12.40 us |     3,155.8 us |
|                    OnBalanceVolume |     748.2 us |     7.30 us |     6.10 us |       755.6 us |
|                RawStochasticsValue |   1,536.9 us |    13.28 us |    11.09 us |     1,546.0 us |
|                   RelativeStrength |   2,766.9 us |    16.95 us |    15.86 us |     2,780.0 us |
|      SimpleMovingAverageOscillator |   1,674.5 us |    26.48 us |    20.67 us |     1,701.5 us |
|                  StandardDeviation |   1,684.5 us |    25.35 us |    21.17 us |     1,713.7 us |
|                   Stochastics_Fast |   2,107.9 us |    46.06 us |    45.24 us |     2,171.8 us |
|                   Stochastics_Slow |   3,069.7 us |    56.49 us |    65.05 us |     3,131.8 us |
|                   Stochastics_Full |   2,800.4 us |    39.44 us |    32.93 us |     2,831.9 us |
|         StochasticsOscillator_Fast |   2,413.2 us |    35.65 us |    31.60 us |     2,450.9 us |
|         StochasticsOscillator_Slow |   3,368.0 us |    47.12 us |    44.08 us |     3,436.3 us |
|         StochasticsOscillator_Full |   3,069.8 us |    20.76 us |    19.42 us |     3,099.2 us |
|              WeightedMovingAverage |   2,440.0 us |    35.94 us |    33.62 us |     2,485.8 us |
|                  HullMovingAverage |   6,180.2 us |    35.96 us |    30.03 us |     6,214.7 us |
|                    KeltnerChannels |   2,520.4 us |    49.59 us |    97.88 us |     2,656.6 us |
|        TransformFromTradesToMinute |   2,280.2 us |    35.28 us |    33.00 us |     2,320.8 us |
|        TransformFromTradesToHourly |   2,140.0 us |    15.35 us |    13.61 us |     2,160.1 us |
|      TransformFromTradesToBeHourly |   2,208.4 us |    29.37 us |    24.52 us |     2,224.4 us |
|         TransformFromTradesToDaily |   2,194.7 us |    29.23 us |    27.34 us |     2,232.5 us |
|        TransformFromTradesToWeekly |   2,179.7 us |    18.46 us |    16.37 us |     2,194.2 us |
