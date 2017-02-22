using System.Threading.Tasks;

namespace Trady.Analysis
{
    public interface IIndicator : IAnalyzable
    {
        Task InitWithIndicatorResultProviderAsync(IIndicatorResultProvider provider);

        int[] Parameters { get; }
    }
}