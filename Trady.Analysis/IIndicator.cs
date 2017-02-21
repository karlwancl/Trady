using System.Threading.Tasks;

namespace Trady.Analysis
{
    public interface IIndicator : IAnalyzable
    {
        Task InitWithDataProviderAsync(IDataProvider provider);

        int[] Parameters { get; }
    }
}