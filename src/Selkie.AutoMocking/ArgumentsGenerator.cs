using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using JetBrains.Annotations;
using Selkie.AutoMocking.Interfaces;

namespace Selkie.AutoMocking
{
    public class ArgumentsGenerator : IArgumentsGenerator
    {
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

        private object CreateArgument(Type type,
                                      bool isFreeze)
        {
            if (isFreeze) Fixture.Customize(new FreezingCustomization(type));

            var parameter = Fixture.Create(type,
                                           new SpecimenContext(Fixture));

            if (parameter != null) return parameter;

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
                                               IsFreezeParameter(info));
            }

            return parameters;
        }

        private object CreateSutArgument(IParameterInfo info)
        {
            return CreateArgument(info.ParameterType,
                                  IsFreezeParameter(info));
        }
    }
}