using System;
using JetBrains.Annotations;
using Selkie.AutoMocking.Interfaces;

namespace Selkie.AutoMocking
{
    public class SutInstanceCreator
        : ISutInstanceCreator
    {
        private readonly IArgumentNullExceptionFinder _finder;

        public SutInstanceCreator([NotNull] IArgumentNullExceptionFinder finder)
        {
            Guard.ArgumentNotNull(finder,
                                  nameof(finder));

            _finder = finder;
        }

        public object Construct(IArgumentsGenerator generator,
                                Type                type)
        {
            Guard.ArgumentNotNull(generator,
                                  nameof(generator));
            Guard.ArgumentNotNull(type,
                                  nameof(type));

            return CreateInstance(generator, type);
        }

        private object CreateInstance(IArgumentsGenerator generator,
                                      Type                type)
        {
            try
            {
                return generator.CreateArgument(type);
            }
            catch (Exception e)
            {
                if (!_finder.TryFindArgumentNullException(e, out var nullException))
                    throw;

                throw nullException;
            }
        }
    }
}