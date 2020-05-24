using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Selkie.AutoMocking.Interfaces;

namespace Selkie.AutoMocking
{
    public class SutLazyInstanceCreator
        : ISutLazyInstanceCreator
    {
        private static readonly MethodInfo FactoryMethod =
            typeof(SutLazyInstanceCreator).GetMethod(nameof(Factory),
                                                     BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly IArgumentNullExceptionFinder _finder;

        public SutLazyInstanceCreator([NotNull] IArgumentNullExceptionFinder finder)
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

            return CreateInstance(generator,
                                  type);
        }

        private object CreateInstance(IArgumentsGenerator generator,
                                      Type                type)
        {
            var methodCall = Expression.Call(Expression.Constant(this),
                                             FactoryMethod,
                                             new Expression[]
                                             {
                                                 Expression.Constant(generator),
                                                 Expression.Constant(type)
                                             });

            var cast = Expression.Convert(methodCall,
                                          type);

            var lambda = Expression.Lambda(cast).Compile();

            var lazyType = typeof(Lazy<>).MakeGenericType(type);

            return Activator.CreateInstance(lazyType, lambda);
        }

        private object Factory(IArgumentsGenerator generator,
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