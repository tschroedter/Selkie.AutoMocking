using AutoFixture.Kernel ;

namespace Selkie.AutoMocking
{
    /// <summary>
    ///     A <see cref="ISpecimenBuilder" /> that always returns null.
    /// </summary>
    public class NullBuilder : ISpecimenBuilder
    {
        /// <summary>
        ///     Returns null specimen every time.
        /// </summary>
        /// <param name="request">The request that describes what to create. Ignored.</param>
        /// <param name="context">
        ///     A context that can be used to create other specimens. Ignored.
        /// </param>
        /// <returns>
        ///     The specimen supplied to the instance in the constructor.
        /// </returns>
        /// <seealso cref="FixedBuilder(object)" />
        public object Create ( object request ,
                               ISpecimenContext context )
        {
            return null ;
        }
    }
}