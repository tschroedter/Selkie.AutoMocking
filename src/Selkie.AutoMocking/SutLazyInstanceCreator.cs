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

        private readonly IArgumentNullExceptionFinder _exceptionFinder;
        private readonly ICustomAttributeFinder _attributeFinder;

        public SutLazyInstanceCreator([NotNull] IArgumentNullExceptionFinder exceptionFinder,
            [NotNull] ICustomAttributeFinder attributeFinder)
        {
            Guard.ArgumentNotNull(exceptionFinder,
                                  nameof(exceptionFinder));
            Guard.ArgumentNotNull(attributeFinder,
                                  nameof(attributeFinder));

            _exceptionFinder = exceptionFinder;
            _attributeFinder = attributeFinder;
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
                ProcessAutoTestDataParameters(generator);

                var argument = CreateSut(generator, type);

                return argument;
            }
            catch (Exception e)
            {
                if (!_exceptionFinder.TryFindArgumentNullException(e, out var nullException))
                    throw;

                throw nullException;
            }
        }

        /// <summary>
        ///     Register instances for the given <see cref="AutoDataTestMethodAttribute"/>
        ///     parameters inside AutoFixture. This makes sure that the custom
        ///     attributes <see cref="BeNullAttribute"/> and <see cref="FreezeAttribute"/>
        ///     used for the parameters of the method are used.
        /// </summary>
        /// <param name="generator">
        ///     The generator is used to create the SUT.
        /// </param>
        private void ProcessAutoTestDataParameters(IArgumentsGenerator generator)
        {
            var methodType = typeof(AutoDataTestMethodAttribute);

            var parameterInfos = _attributeFinder.Find(methodType);

            foreach (var parameterInfo in parameterInfos)
            {
                generator.CreateOtherArgument(parameterInfo);
            }
        }

        /// <summary>
        ///     Create the SUT using the pre-registered parameters of the
        ///     <see cref="AutoDataTestMethodAttribute"/> method and create all
        ///     the missing additional parameters using AutoFixture.
        /// </summary>
        /// <param name="generator">
        ///     The generator is used to create the SUT.
        /// </param>
        /// <param name="type">
        ///     The type of the service under test to be created.
        /// </param>
        /// <returns></returns>
        private static object CreateSut(IArgumentsGenerator generator, Type type)
        {
            return generator.CreateArgument(type);
        }
    }
}