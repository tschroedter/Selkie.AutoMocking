using System;
using JetBrains.Annotations;

namespace Selkie.AutoMocking.Interfaces
{
    /// <summary>
    ///     This class tries to find a 'hidden' <see cref="ArgumentNullException" />
    ///     in the given exception and its inner exceptions. The dynamic creation
    ///     can fail and throw general exception hiding the real reason in inner-
    ///     exceptions. This calls tries to locate certain 'hidden' exceptions.
    /// </summary>
    public interface IArgumentNullExceptionFinder
    {
        /// <summary>
        ///     Try to find a 'hidden' <see cref="ArgumentNullException" /> in the
        ///     given exception.
        /// </summary>
        /// <param name="exception">
        ///     The exception to check.
        /// </param>
        /// <param name="argumentNullException">
        ///     If found, this will contain the <see cref="ArgumentNullException" />.
        /// </param>
        /// <param name="maxDepth">
        ///     Maximum depth to dig-down in the inner-exceptions.
        /// </param>
        /// <returns>
        ///     'true' if an <see cref="ArgumentNullException" /> was found, otherwise 'false'.
        /// </returns>
        bool TryFindArgumentNullException([NotNull] Exception             exception,
                                          out       ArgumentNullException argumentNullException,
                                          int                             maxDepth = 10);
    }
}