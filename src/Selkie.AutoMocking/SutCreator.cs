using System;
using System.Linq;
using JetBrains.Annotations;
using Selkie.AutoMocking.Interfaces;

namespace Selkie.AutoMocking
{
    public class SutCreator
        : ISutCreator
    {
        private readonly ISutInstanceCreator     _creator;
        private readonly ISutLazyInstanceCreator _lazyCreator;

        public SutCreator([NotNull] ISutInstanceCreator     creator,
                          [NotNull] ISutLazyInstanceCreator lazyCreator)
        {
            Guard.ArgumentNotNull(creator,
                                  nameof(creator));
            Guard.ArgumentNotNull(lazyCreator,
                                  nameof(lazyCreator));

            _creator     = creator;
            _lazyCreator = lazyCreator;
        }

        public object Construct(IArgumentsGenerator generator,
                                Type                type)
        {
            Guard.ArgumentNotNull(generator,
                                  nameof(generator));
            Guard.ArgumentNotNull(type,
                                  nameof(type));

            if (IsLazy(type))
                return _lazyCreator.Construct(generator,
                                              type.GenericTypeArguments
                                                  .First());

            return _creator.Construct(generator,
                                      type);
        }

        private static bool IsLazy(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Lazy<>);
        }
    }
}