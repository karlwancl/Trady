using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Infrastructure
{
    public interface IAnalyzable<TInput, TOutput>
    {
        /// <summary>
        /// Gets the inputs.
        /// </summary>
        /// <value>The inputs.</value>
        IEnumerable<TInput> Inputs { get; }

        /// <summary>
        /// Compute the target output list by the specified startIndex and endIndex.
        /// </summary>
        /// <returns>The computed outputs.</returns>
        /// <param name="startIndex">Start index.</param>
        /// <param name="endIndex">End index.</param>
        IList<TOutput> Compute(int? startIndex = null, int? endIndex = null);

        /// <summary>
        /// Gets the <see cref="T:Trady.Core.Infrastructure.IAnalyzable2`2"/> at the specified index.
        /// </summary>
        /// <param name="i">Index.</param>
		TOutput this[int i] { get; }
	}
}
