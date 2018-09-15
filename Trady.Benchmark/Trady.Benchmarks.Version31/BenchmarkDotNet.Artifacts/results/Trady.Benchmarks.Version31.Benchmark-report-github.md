``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.228 (1803/April2018Update/Redstone4)
AMD FX(tm)-8350 Eight-Core Processor (Max: 4.34GHz), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.402
  [Host] : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  Core   : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                             Method |           Mean |         Error |       StdDev |         Median |            P90 |
|----------------------------------- |---------------:|--------------:|-------------:|---------------:|---------------:|
|                 TransformToMonthly |       625.6 us |      5.919 us |     5.536 us |       624.2 us |       632.2 us |
|                   TransformToDaily |       635.5 us |     11.125 us |    10.406 us |       636.0 us |       646.0 us |
|              RelativeStrengthIndex |     8,882.1 us |    110.469 us |    92.246 us |     8,879.8 us |     8,974.0 us |
|           ExponentialMovingAverage |     3,823.1 us |     74.262 us |    91.200 us |     3,761.9 us |     3,968.8 us |
|                SimpleMovingAverage |     2,519.6 us |     47.665 us |    46.813 us |     2,497.3 us |     2,594.7 us |
|                               MACD |     8,686.9 us |    170.576 us |   255.311 us |     8,632.2 us |     9,044.1 us |
|              CommodityChannelIndex |    10,907.2 us |    208.864 us |   223.483 us |    10,896.0 us |    11,174.0 us |
|            StochasticMomentumIndex |    15,634.7 us |    306.085 us |   271.336 us |    15,516.7 us |    15,980.1 us |
|    StochasticRelativeStrengthIndex |    15,331.4 us |    303.144 us |   297.728 us |    15,247.2 us |    15,760.8 us |
|              NetMomentumOscillator |     9,445.1 us |    143.353 us |   127.079 us |     9,388.5 us |     9,650.3 us |
|                   RelativeMomentum |     8,142.1 us |     27.371 us |    22.856 us |     8,141.6 us |     8,162.3 us |
|              RelativeMomentumIndex |     9,470.3 us |    149.076 us |   132.152 us |     9,426.3 us |     9,643.5 us |
|               DynamicMomentumIndex | 2,480,814.0 us | 11,569.665 us | 9,661.194 us | 2,480,457.7 us | 2,493,470.1 us |
|            ParabolicStopAndReverse |     2,803.4 us |     61.585 us |    57.607 us |     2,784.4 us |     2,875.7 us |
|                             Median |     6,022.6 us |     91.746 us |    85.819 us |     6,001.0 us |     6,156.1 us |
|                         Percentile |     7,359.1 us |     89.903 us |    75.073 us |     7,343.6 us |     7,418.5 us |
|       AccumulationDistributionLine |     2,331.2 us |     45.629 us |    42.681 us |     2,329.5 us |     2,374.3 us |
|                              Aroon |    10,054.9 us |     61.110 us |    54.173 us |    10,054.5 us |    10,110.5 us |
|                    AroonOscillator |    11,022.4 us |    219.079 us |   204.927 us |    11,021.2 us |    11,272.1 us |
|                   AverageTrueRange |     4,643.4 us |     63.295 us |    56.110 us |     4,621.2 us |     4,721.0 us |
|                     BollingerBands |     8,008.1 us |    159.128 us |   228.217 us |     7,947.7 us |     8,366.9 us |
|                 BollingerBandWidth |     8,471.0 us |     23.707 us |    18.509 us |     8,473.7 us |     8,486.3 us |
|                     ChandelierExit |     7,839.3 us |    166.563 us |   216.579 us |     7,811.2 us |     8,053.2 us |
|                           Momentum |     1,501.9 us |     33.420 us |    31.261 us |     1,486.3 us |     1,552.6 us |
|                       RateOfChange |     1,970.7 us |     38.953 us |    85.503 us |     1,930.8 us |     2,080.6 us |
|           PlusDirectionalIndicator |     9,247.9 us |    226.091 us |   324.253 us |     9,094.6 us |     9,626.1 us |
|          MinusDirectionalIndicator |     8,851.6 us |    170.597 us |   209.509 us |     8,750.6 us |     9,156.6 us |
|            AverageDirectionalIndex |    20,275.1 us |    226.527 us |   211.893 us |    20,194.4 us |    20,535.2 us |
|      AverageDirectionalIndexRating |    20,990.1 us |    176.606 us |   156.556 us |    20,912.7 us |    21,201.2 us |
|                    EfficiencyRatio |     2,807.2 us |     12.648 us |    11.831 us |     2,805.2 us |     2,823.7 us |
|       KaufmanAdaptiveMovingAverage |     7,778.7 us |     72.186 us |    63.991 us |     7,758.4 us |     7,890.8 us |
| ExponentialMovingAverageOscillator |     6,408.9 us |     31.350 us |    24.476 us |     6,409.7 us |     6,436.5 us |
|                        HighestHigh |     2,051.2 us |     39.246 us |    41.993 us |     2,033.7 us |     2,107.0 us |
|                       HighestClose |     2,009.7 us |     11.103 us |     9.271 us |     2,006.1 us |     2,022.8 us |
|              HistoricalHighestHigh |     1,742.1 us |     22.284 us |    20.844 us |     1,738.7 us |     1,771.0 us |
|             HistoricalHighestClose |     1,715.1 us |     27.549 us |    25.770 us |     1,703.6 us |     1,756.5 us |
|                          LowestLow |     2,017.3 us |      9.569 us |     8.483 us |     2,018.0 us |     2,027.2 us |
|                        LowestClose |     2,038.1 us |     37.906 us |    33.603 us |     2,040.0 us |     2,064.2 us |
|                HistoricalLowestLow |     1,742.8 us |     31.045 us |    27.521 us |     1,740.5 us |     1,777.2 us |
|              HistoricalLowestClose |     1,751.7 us |     34.189 us |    38.001 us |     1,745.1 us |     1,794.5 us |
|                      IchimokuCloud |    11,722.8 us |     81.607 us |    72.342 us |    11,736.9 us |    11,797.0 us |
|              ModifiedMovingAverage |     3,844.4 us |    105.944 us |    99.100 us |     3,802.9 us |     3,933.7 us |
|                           MacdHist |     8,714.6 us |     27.107 us |    22.636 us |     8,710.3 us |     8,736.8 us |
|                    OnBalanceVolume |     1,948.0 us |     31.619 us |    29.577 us |     1,932.8 us |     1,993.8 us |
|                RawStochasticsValue |     3,946.3 us |     35.122 us |    27.421 us |     3,956.2 us |     3,970.0 us |
|                   RelativeStrength |     7,966.9 us |     91.760 us |    85.833 us |     7,937.7 us |     8,109.0 us |
|      SimpleMovingAverageOscillator |     3,805.4 us |     34.014 us |    30.152 us |     3,805.1 us |     3,840.1 us |
|                  StandardDeviation |     4,243.9 us |     81.038 us |    86.710 us |     4,219.5 us |     4,350.0 us |
|                   Stochastics_Fast |     5,730.6 us |     43.938 us |    34.304 us |     5,730.9 us |     5,767.0 us |
|                   Stochastics_Slow |     7,984.4 us |     57.388 us |    47.922 us |     7,981.7 us |     8,030.1 us |
|                   Stochastics_Full |     7,685.3 us |    147.693 us |   151.670 us |     7,631.7 us |     7,911.4 us |
|         StochasticsOscillator_Fast |     6,158.1 us |     26.776 us |    22.360 us |     6,151.9 us |     6,188.4 us |
|         StochasticsOscillator_Slow |     8,483.8 us |    164.591 us |   161.650 us |     8,412.0 us |     8,736.7 us |
|         StochasticsOscillator_Full |     7,927.3 us |    151.127 us |   141.364 us |     7,860.8 us |     8,175.4 us |
|              WeightedMovingAverage |     5,664.9 us |    110.213 us |   103.093 us |     5,612.0 us |     5,825.0 us |
|                  HullMovingAverage |    14,775.9 us |     61.306 us |    51.193 us |    14,789.8 us |    14,819.4 us |
|                    KeltnerChannels |     7,048.6 us |     25.131 us |    19.620 us |     7,052.2 us |     7,062.9 us |
