``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.228 (1803/April2018Update/Redstone4)
AMD FX(tm)-8350 Eight-Core Processor (Max: 4.34GHz), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.402
  [Host]    : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  Core      : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  RyuJitX64 : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT


```
|                          Method |          Job |       Jit | Platform | Runtime |           Mean |         Error |        StdDev |            P90 |            P95 |
|-------------------------------- |------------- |---------- |--------- |-------- |---------------:|--------------:|--------------:|---------------:|---------------:|
|              TransformToMonthly |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                TransformToDaily |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|           RelativeStrengthIndex |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|        ExponentialMovingAverage |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|             SimpleMovingAverage |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                            MACD |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|           CommodityChannelIndex |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|         StochasticMomentumIndex |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
| StochasticRelativeStrengthIndex |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|           NetMomentumOscillator |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                              RM |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                             RMI |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                           DYMOI |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                             SAR |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                          Median |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                      Percentile |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                       AccumDist |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                           Aroon |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                        AroonOsc |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                             Atr |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                              Bb |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                         BbWidth |          Clr |    RyuJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|              TransformToMonthly |         Core |    RyuJit |      X64 |    Core |       617.6 us |      4.470 us |      3.962 us |       622.1 us |       624.5 us |
|                TransformToDaily |         Core |    RyuJit |      X64 |    Core |       623.6 us |      1.397 us |      1.307 us |       625.1 us |       625.4 us |
|           RelativeStrengthIndex |         Core |    RyuJit |      X64 |    Core |     8,788.2 us |     38.652 us |     32.276 us |     8,826.5 us |     8,838.3 us |
|        ExponentialMovingAverage |         Core |    RyuJit |      X64 |    Core |     3,744.3 us |     28.096 us |     23.462 us |     3,771.9 us |     3,785.0 us |
|             SimpleMovingAverage |         Core |    RyuJit |      X64 |    Core |     2,532.1 us |     63.585 us |     62.449 us |     2,619.4 us |     2,651.6 us |
|                            MACD |         Core |    RyuJit |      X64 |    Core |     8,445.1 us |    206.271 us |    253.319 us |     8,790.6 us |     8,865.9 us |
|           CommodityChannelIndex |         Core |    RyuJit |      X64 |    Core |    10,566.6 us |     43.610 us |     38.659 us |    10,624.0 us |    10,640.1 us |
|         StochasticMomentumIndex |         Core |    RyuJit |      X64 |    Core |    15,965.4 us |    311.800 us |    485.434 us |    16,818.5 us |    16,931.3 us |
| StochasticRelativeStrengthIndex |         Core |    RyuJit |      X64 |    Core |    15,661.4 us |    446.565 us |    458.590 us |    16,467.1 us |    16,499.2 us |
|           NetMomentumOscillator |         Core |    RyuJit |      X64 |    Core |     9,606.6 us |    224.640 us |    267.418 us |     9,940.0 us |    10,088.3 us |
|                              RM |         Core |    RyuJit |      X64 |    Core |     8,107.0 us |     23.845 us |     19.911 us |     8,122.5 us |     8,135.6 us |
|                             RMI |         Core |    RyuJit |      X64 |    Core |     9,431.3 us |    198.517 us |    185.692 us |     9,743.3 us |     9,763.7 us |
|                           DYMOI |         Core |    RyuJit |      X64 |    Core | 2,461,830.3 us | 21,463.469 us | 20,076.941 us | 2,491,578.2 us | 2,494,853.7 us |
|                             SAR |         Core |    RyuJit |      X64 |    Core |     2,808.5 us |     27.176 us |     22.693 us |     2,831.4 us |     2,846.2 us |
|                          Median |         Core |    RyuJit |      X64 |    Core |     5,980.1 us |    116.800 us |    119.945 us |     6,160.5 us |     6,197.5 us |
|                      Percentile |         Core |    RyuJit |      X64 |    Core |     7,318.3 us |     40.218 us |     31.400 us |     7,339.5 us |     7,362.7 us |
|                       AccumDist |         Core |    RyuJit |      X64 |    Core |     2,279.3 us |     43.912 us |     52.275 us |     2,367.0 us |     2,367.2 us |
|                           Aroon |         Core |    RyuJit |      X64 |    Core |    10,401.9 us |    196.372 us |    233.767 us |    10,738.7 us |    10,772.6 us |
|                        AroonOsc |         Core |    RyuJit |      X64 |    Core |    10,933.4 us |    233.662 us |    207.135 us |    11,236.0 us |    11,353.1 us |
|                             Atr |         Core |    RyuJit |      X64 |    Core |     4,575.3 us |     15.177 us |     14.196 us |     4,590.1 us |     4,594.5 us |
|                              Bb |         Core |    RyuJit |      X64 |    Core |     7,706.5 us |    124.177 us |    116.155 us |     7,838.7 us |     7,868.2 us |
|                         BbWidth |         Core |    RyuJit |      X64 |    Core |     8,228.9 us |    110.915 us |    103.750 us |     8,349.4 us |     8,356.0 us |
|              TransformToMonthly | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                TransformToDaily | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|           RelativeStrengthIndex | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|        ExponentialMovingAverage | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|             SimpleMovingAverage | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                            MACD | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|           CommodityChannelIndex | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|         StochasticMomentumIndex | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
| StochasticRelativeStrengthIndex | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|           NetMomentumOscillator | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                              RM | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                             RMI | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                           DYMOI | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                             SAR | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                          Median | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                      Percentile | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                       AccumDist | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                           Aroon | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                        AroonOsc | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                             Atr | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                              Bb | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                         BbWidth | LegacyJitX64 | LegacyJit |      X64 |     Clr |             NA |            NA |            NA |             NA |             NA |
|              TransformToMonthly | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                TransformToDaily | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|           RelativeStrengthIndex | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|        ExponentialMovingAverage | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|             SimpleMovingAverage | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                            MACD | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|           CommodityChannelIndex | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|         StochasticMomentumIndex | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
| StochasticRelativeStrengthIndex | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|           NetMomentumOscillator | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                              RM | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                             RMI | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                           DYMOI | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                             SAR | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                          Median | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                      Percentile | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                       AccumDist | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                           Aroon | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                        AroonOsc | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                             Atr | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                              Bb | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|                         BbWidth | LegacyJitX86 | LegacyJit |      X86 |     Clr |             NA |            NA |            NA |             NA |             NA |
|              TransformToMonthly |    RyuJitX64 |    RyuJit |      X64 |    Core |       624.8 us |      1.264 us |      1.120 us |       626.1 us |       626.7 us |
|                TransformToDaily |    RyuJitX64 |    RyuJit |      X64 |    Core |       644.1 us |     14.917 us |     15.319 us |       667.9 us |       670.4 us |
|           RelativeStrengthIndex |    RyuJitX64 |    RyuJit |      X64 |    Core |     9,011.1 us |    177.019 us |    203.856 us |     9,295.1 us |     9,358.3 us |
|        ExponentialMovingAverage |    RyuJitX64 |    RyuJit |      X64 |    Core |     3,766.4 us |     10.483 us |      9.293 us |     3,774.7 us |     3,776.1 us |
|             SimpleMovingAverage |    RyuJitX64 |    RyuJit |      X64 |    Core |     2,501.8 us |     40.158 us |     37.564 us |     2,561.5 us |     2,567.6 us |
|                            MACD |    RyuJitX64 |    RyuJit |      X64 |    Core |     8,355.0 us |     48.526 us |     43.017 us |     8,422.2 us |     8,424.7 us |
|           CommodityChannelIndex |    RyuJitX64 |    RyuJit |      X64 |    Core |    10,661.2 us |    169.561 us |    158.607 us |    10,883.6 us |    10,962.3 us |
|         StochasticMomentumIndex |    RyuJitX64 |    RyuJit |      X64 |    Core |    15,446.7 us |    197.882 us |    185.099 us |    15,657.9 us |    15,676.9 us |
| StochasticRelativeStrengthIndex |    RyuJitX64 |    RyuJit |      X64 |    Core |    15,066.7 us |     90.695 us |     70.809 us |    15,167.0 us |    15,180.7 us |
|           NetMomentumOscillator |    RyuJitX64 |    RyuJit |      X64 |    Core |     9,434.1 us |     51.352 us |     42.881 us |     9,489.3 us |     9,503.9 us |
|                              RM |    RyuJitX64 |    RyuJit |      X64 |    Core |     8,097.7 us |     31.829 us |     24.850 us |     8,132.1 us |     8,135.9 us |
|                             RMI |    RyuJitX64 |    RyuJit |      X64 |    Core |     9,373.9 us |     65.590 us |     54.771 us |     9,425.4 us |     9,435.8 us |
|                           DYMOI |    RyuJitX64 |    RyuJit |      X64 |    Core | 2,448,177.9 us |  7,310.800 us |  6,838.527 us | 2,456,877.2 us | 2,460,907.2 us |
|                             SAR |    RyuJitX64 |    RyuJit |      X64 |    Core |     2,798.9 us |     26.875 us |     23.824 us |     2,829.7 us |     2,839.5 us |
|                          Median |    RyuJitX64 |    RyuJit |      X64 |    Core |     5,939.1 us |     19.814 us |     17.565 us |     5,962.5 us |     5,968.0 us |
|                      Percentile |    RyuJitX64 |    RyuJit |      X64 |    Core |     7,298.7 us |     24.682 us |     20.611 us |     7,320.3 us |     7,329.4 us |
|                       AccumDist |    RyuJitX64 |    RyuJit |      X64 |    Core |     2,248.3 us |      7.664 us |      6.794 us |     2,256.2 us |     2,256.7 us |
|                           Aroon |    RyuJitX64 |    RyuJit |      X64 |    Core |    10,481.1 us |     29.730 us |     24.826 us |    10,507.5 us |    10,515.7 us |
|                        AroonOsc |    RyuJitX64 |    RyuJit |      X64 |    Core |    10,648.6 us |     54.335 us |     48.167 us |    10,689.2 us |    10,696.1 us |
|                             Atr |    RyuJitX64 |    RyuJit |      X64 |    Core |     4,703.9 us |     90.148 us |     88.538 us |     4,828.4 us |     4,858.7 us |
|                              Bb |    RyuJitX64 |    RyuJit |      X64 |    Core |     7,864.3 us |    156.204 us |    197.548 us |     8,100.4 us |     8,109.5 us |
|                         BbWidth |    RyuJitX64 |    RyuJit |      X64 |    Core |     8,287.3 us |     31.184 us |     24.347 us |     8,301.9 us |     8,325.6 us |

Benchmarks with issues:
  Benchmark.TransformToMonthly: Clr(Runtime=Clr)
  Benchmark.TransformToDaily: Clr(Runtime=Clr)
  Benchmark.RelativeStrengthIndex: Clr(Runtime=Clr)
  Benchmark.ExponentialMovingAverage: Clr(Runtime=Clr)
  Benchmark.SimpleMovingAverage: Clr(Runtime=Clr)
  Benchmark.MACD: Clr(Runtime=Clr)
  Benchmark.CommodityChannelIndex: Clr(Runtime=Clr)
  Benchmark.StochasticMomentumIndex: Clr(Runtime=Clr)
  Benchmark.StochasticRelativeStrengthIndex: Clr(Runtime=Clr)
  Benchmark.NetMomentumOscillator: Clr(Runtime=Clr)
  Benchmark.RM: Clr(Runtime=Clr)
  Benchmark.RMI: Clr(Runtime=Clr)
  Benchmark.DYMOI: Clr(Runtime=Clr)
  Benchmark.SAR: Clr(Runtime=Clr)
  Benchmark.Median: Clr(Runtime=Clr)
  Benchmark.Percentile: Clr(Runtime=Clr)
  Benchmark.AccumDist: Clr(Runtime=Clr)
  Benchmark.Aroon: Clr(Runtime=Clr)
  Benchmark.AroonOsc: Clr(Runtime=Clr)
  Benchmark.Atr: Clr(Runtime=Clr)
  Benchmark.Bb: Clr(Runtime=Clr)
  Benchmark.BbWidth: Clr(Runtime=Clr)
  Benchmark.TransformToMonthly: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.TransformToDaily: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.RelativeStrengthIndex: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.ExponentialMovingAverage: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.SimpleMovingAverage: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.MACD: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.CommodityChannelIndex: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.StochasticMomentumIndex: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.StochasticRelativeStrengthIndex: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.NetMomentumOscillator: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.RM: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.RMI: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.DYMOI: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.SAR: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.Median: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.Percentile: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.AccumDist: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.Aroon: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.AroonOsc: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.Atr: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.Bb: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.BbWidth: LegacyJitX64(Jit=LegacyJit, Platform=X64, Runtime=Clr)
  Benchmark.TransformToMonthly: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.TransformToDaily: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.RelativeStrengthIndex: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.ExponentialMovingAverage: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.SimpleMovingAverage: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.MACD: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.CommodityChannelIndex: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.StochasticMomentumIndex: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.StochasticRelativeStrengthIndex: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.NetMomentumOscillator: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.RM: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.RMI: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.DYMOI: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.SAR: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.Median: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.Percentile: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.AccumDist: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.Aroon: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.AroonOsc: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.Atr: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.Bb: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
  Benchmark.BbWidth: LegacyJitX86(Jit=LegacyJit, Platform=X86, Runtime=Clr)
