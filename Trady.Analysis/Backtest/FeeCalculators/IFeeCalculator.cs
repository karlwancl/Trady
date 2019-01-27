using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest.FeeCalculators
{
    /// <summary>
    /// Interface to calculate buy/sell logic.
    /// </summary>
    /// <remarks>
    /// You can implement your own calculator for any particular broker's rules.
    /// For example: some brokers charge differently for larger orders...  You can implement that logic in in your own IAssetCalculator
    /// </remarks>
    public interface IFeeCalculator
    {
        Transaction BuyAsset(IIndexedOhlcv indexedCandle, decimal cash, IIndexedOhlcv nextCandle, bool buyInCompleteQuantities);
        Transaction SellAsset(IIndexedOhlcv indexedCandle, Transaction lastTransaction);
    }
}
