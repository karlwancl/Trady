using System.Threading.Tasks;
using Trady.Core;

namespace Trady.Analysis
{
    public interface IIndicator : IAnalyzable
    {
        int[] Parameters { get; }
    }
}