using System;
using JetBrains.Annotations;

namespace Selkie.AutoMocking.Interfaces
{
    public interface ICreator
    {
        /// <summary>
        ///     Create the requested type using the given <see cref="IArgumentsGenerator" />.
        /// </summary>
        /// <param name="generator">
        ///     The generator used to create the type.
        /// </param>
        /// <param name="type">
        ///     The type to create.
        /// </param>
        /// <returns>
        ///     An instance of the given type as an object.
        /// </returns>
        object Construct([NotNull] IArgumentsGenerator generator,
                         [NotNull] Type                type);
    }
}