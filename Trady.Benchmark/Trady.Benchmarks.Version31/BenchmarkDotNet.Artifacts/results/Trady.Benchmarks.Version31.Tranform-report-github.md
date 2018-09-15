``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.228 (1803/April2018Update/Redstone4)
AMD FX(tm)-8350 Eight-Core Processor (Max: 4.34GHz), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.402
  [Host]     : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT


```
|                   Method |       Mean |      Error |     StdDev |
|------------------------- |-----------:|-----------:|-----------:|
|       TransformToMonthly |   628.8 us |   1.341 us |   1.120 us |
|         TransformToDaily |   644.0 us |  10.684 us |   9.994 us |
|    RelativeStrengthIndex | 8,842.9 us | 176.876 us | 181.639 us |
| ExponentialMovingAverage | 3,881.4 us |  74.397 us |  79.604 us |
|      SimpleMovingAverage | 2,498.0 us |   8.992 us |   7.509 us |
|                     MACD | 8,363.9 us | 125.862 us | 117.731 us |
