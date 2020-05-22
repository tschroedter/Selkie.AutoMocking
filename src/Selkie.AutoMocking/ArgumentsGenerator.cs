using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using JetBrains.Annotations;
using Selkie.AutoMocking.Interfaces;

namespace Selkie.AutoMocking
{
    public class ArgumentsGenerator : IArgumentsGenerator
    {
        private static readonly MethodInfo FactoryMethod =
            typeof(ArgumentsGenerator).GetMethod(nameof(Factory), BindingFlags.Instance | BindingFlags.NonPublic);

        public ArgumentsGenerator()
            : this(new Fixture())
        {
        }

        internal ArgumentsGenerator([NotNull] IFixture fixture)
        {
            Guard.ArgumentNotNull(fixture,
                                  nameof(fixture));

            Fixture = fixture;

            Fixture.Customize(new AutoNSubstituteCustomization
                              {
                                  ConfigureMembers = true
                              });
        }

        public IFixture Fixture { get; }

        public object[] Create(IEnumerable<IParameterInfo> parameterInfos)
        {
            Guard.ArgumentNotNull(parameterInfos,
                                  nameof(parameterInfos));

            var infos = parameterInfos as IParameterInfo[] ?? parameterInfos.ToArray();

            return infos.Length == 0
                       ? Array.Empty<object>()
                       : CreateArguments(infos);
        }

        private static bool IsFreezeParameter(IParameterInfo info)
        {
            return info.CustomAttributes.Any(x => x.AttributeType == typeof(FreezeAttribute));
        }

        private static bool IsBeNullParameter(IParameterInfo info)
        {
            return info.CustomAttributes.Any(x => x.AttributeType == typeof(BeNullAttribute));
        }

        private object CreateArgument(Type type,
                                      bool isFreeze,
                                      bool isBeNull)
        {
            if (isFreeze) Fixture.Customize(new FreezingCustomization(type));

            if (isBeNull) Fixture.Customize(new BeNullCustomization(type));

            var parameter = Fixture.Create(type,
                                           new SpecimenContext(Fixture));

            if (parameter != null ||
                isBeNull)
                return parameter;

            var message = string.Format(CultureInfo.InvariantCulture,
                                        $"Failed to create type '{type.FullName}'");

            throw new ArgumentNullException(type.FullName,
                                            message);
        }

        private object[] CreateArguments(IParameterInfo[] infos)
        {
            var parameters = CreateOtherArguments(infos);

            parameters[0] = CreateSutArgument(infos[0]);

            return parameters;
        }

        private object[] CreateOtherArguments(IParameterInfo[] infos)
        {
            var parameters = new object[infos.Length];

            for (var i = 1; i < infos.Length; i++)
            {
                var info = infos[i];

                parameters[i] = CreateArgument(info.ParameterType,
                                               IsFreezeParameter(info),
                                               IsBeNullParameter(info));
            }

            return parameters;
        }

        private object CreateSutArgument(IParameterInfo info)
        {
            if (IsLazy(info)) return ConstructLazy(info.ParameterType.GenericTypeArguments.First());

            return CreateArgument(info.ParameterType,
                                  IsFreezeParameter(info),
                                  IsBeNullParameter(info));
        }

        private static bool IsLazy(IParameterInfo info)
        {
            return info.ParameterType.IsGenericType &&
                   info.ParameterType.GetGenericTypeDefinition() == typeof(Lazy<>);
        }

        public object ConstructLazy(Type desiredType)
        {
            var methodCall = Expression.Call(Expression.Constant(this),
                                             FactoryMethod,
                                             Expression.Constant(desiredType));
            var cast     = Expression.Convert(methodCall, desiredType);
            var lambda   = Expression.Lambda(cast).Compile();
            var lazyType = typeof(Lazy<>).MakeGenericType(desiredType);
            return Activator.CreateInstance(lazyType, lambda);
        }

        private object Factory(Type type)
        {
            try
            {
                return CreateArgument(type, false, false);
            }
            catch (Exception e)
            {
                var current = e;
                var last    = current;
                var count   = 0;

                while (current != null &&
                       count++ < 10)
                {
                    last    = current;
                    current = current.InnerException;
                }

                if (!(last is ArgumentException argumentException) ||
                    !argumentException.Message.StartsWith("Value cannot be null."))
                    throw last;

                Console.WriteLine("Creating ArgumentNullException with "             +
                                  $"parameter name '{argumentException.ParamName}' " +
                                  $"and message '{argumentException.Message}'.");

                throw new ArgumentNullException(argumentException.ParamName,
                                                argumentException.Message);
            }
        }
    }
}