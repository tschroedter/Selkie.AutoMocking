using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

namespace Selkie.AutoMocking
{
    public class BeNullCustomization : ICustomization
    {
        public BeNullCustomization(Type targetType)
            : this(targetType, targetType)
        {
        }

        public BeNullCustomization(Type targetType, Type registeredType)
        {
            if (targetType     == null) throw new ArgumentNullException(nameof(targetType));
            if (registeredType == null) throw new ArgumentNullException(nameof(registeredType));

            if (!registeredType.GetTypeInfo().IsAssignableFrom(targetType))
            {
                var message = string.Format(
                                            CultureInfo.CurrentCulture,
                                            "The type '{0}' cannot be frozen as '{1}' because the two types are not compatible.",
                                            targetType,
                                            registeredType);
                throw new ArgumentException(message);
            }

            TargetType     = targetType;
            RegisteredType = registeredType;
        }

        /// <summary>
        ///     Gets the <see cref="Type" /> to freeze.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///     Gets the <see cref="Type" /> to which the frozen <see cref="TargetType" /> value
        ///     should be mapped to. Defaults to the same <see cref="Type" /> as <see cref="TargetType" />.
        /// </summary>
        public Type RegisteredType { get; }

        public void Customize(IFixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException(nameof(fixture));

            var fixedBuilder = new NullBuilder();

            var types = new[]
                        {
                            TargetType,
                            RegisteredType
                        };

            var builder = new CompositeSpecimenBuilder(
                                                       from t in types
                                                       select SpecimenBuilderNodeFactory.CreateTypedNode(
                                                                                                         t,
                                                                                                         fixedBuilder)
                                                                  as ISpecimenBuilder);

            fixture.Customizations.Insert(0, builder);
        }
    }
}