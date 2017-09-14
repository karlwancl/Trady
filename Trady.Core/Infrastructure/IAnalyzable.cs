using System.Collections;
using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IAnalyzable
    {
        IEnumerable Compute(int? startIndex = null, int? endIndex = null);

        object this[int i] { get; }
    }

    public interface IAnalyzable<TOutput> : IAnalyzable
    {
        /// <summary>
        /// Compute the target output list by the specified startIndex and endIndex.
        /// </summary>
        /// <returns>The computed outputs.</returns>
        /// <param name="startIndex">Start index.</param>
        /// <param name="endIndex">End index.</param>
        new IReadOnlyList<TOutput> Compute(int? startIndex = null, int? endIndex = null);

        /// <summary>
        /// Gets the <see cref="T:Trady.Core.Infrastructure.IAnalyzable2`2"/> at the specified index.
        /// </summary>
        /// <param name="i">Index.</param>
		new TOutput this[int i] { get; }
    }
}