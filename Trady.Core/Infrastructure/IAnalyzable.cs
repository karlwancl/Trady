using System.Collections;
using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IAnalyzable
    {
        IEnumerable Compute(int? startIndex = null, int? endIndex = null);

        IEnumerable Compute(IEnumerable<int> indexes);

        (object Prev, object Current, object Next) ComputeNeighbour(int index);

        object this[int i] { get; }
    }

    public interface IAnalyzable<TOutput> : IAnalyzable
    {
        /// <summary>
        /// Compute the full list by the specified startIndex and endIndex.
        /// </summary>
        /// <returns>The computed outputs.</returns>
        /// <param name="startIndex">Start index.</param>
        /// <param name="endIndex">End index.</param>
        new IReadOnlyList<TOutput> Compute(int? startIndex = null, int? endIndex = null);

		/// <summary>
		/// Compute the target list by a list of index
		/// </summary>
		/// <returns>The select.</returns>
		/// <param name="indexes">Indexes.</param>
		new IReadOnlyList<TOutput> Compute(IEnumerable<int> indexes);

        /// <summary>
        /// Compute the target by index
        /// </summary>
        /// <param name="indexes">Indexes.</param>
        /// <returns>The select.</returns>
        TOutput Compute(params int[] indexes);

        /// <summary>
        /// Compute the prev & the current by an index
        /// </summary>
        /// <returns>The compute.</returns>
        /// <param name="index">Index.</param>
        new (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbour(int index);

        /// <summary>
        /// Gets the <see cref="T:Trady.Core.Infrastructure.IAnalyzable2`2"/> at the specified index.
        /// </summary>
        /// <param name="i">Index.</param>
		new TOutput this[int i] { get; }
    }
}
