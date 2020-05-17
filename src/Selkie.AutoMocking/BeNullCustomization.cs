using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using CompositeSpecimenBuilder = AutoFixture.Kernel.CompositeSpecimenBuilder;
using ISpecimenBuilder = AutoFixture.Kernel.ISpecimenBuilder;
using ISpecimenContext = AutoFixture.Kernel.ISpecimenContext;
using SpecimenBuilderNodeFactory = AutoFixture.Kernel.SpecimenBuilderNodeFactory;

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

            this.TargetType     = targetType;
            this.RegisteredType = registeredType;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> to freeze.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> to which the frozen <see cref="TargetType"/> value
        /// should be mapped to. Defaults to the same <see cref="Type"/> as <see cref="TargetType"/>.
        /// </summary>
        public Type RegisteredType { get; }

        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            var specimen = fixture.Create(this.TargetType, new SpecimenContext(fixture));
            var fixedBuilder = new NullBuilder(specimen);

            var types = new[]
                        {
                            this.TargetType,
                            this.RegisteredType
                        };

            var builder = new CompositeSpecimenBuilder(
                                                       from t in types
                                                       select SpecimenBuilderNodeFactory.CreateTypedNode(
                                                                                                         t, fixedBuilder) as ISpecimenBuilder);

            fixture.Customizations.Insert(0, builder);
        }
    }

    /// <summary>
    /// A <see cref="AutoFixture.Kernel.ISpecimenBuilder"/> that always returns the same specimen.
    /// </summary>
    public class NullBuilder : ISpecimenBuilder
    {
        private readonly object _specimen;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFixture.Kernel.FixedBuilder"/> class.
        /// </summary>
        /// <param name="specimen">
        /// The specimen to return from the <see cref="Create"/> method.
        /// </param>
        public NullBuilder(object specimen)
        {
            this._specimen = specimen;
        }

        /// <summary>
        /// Returns the same specimen every time.
        /// </summary>
        /// <param name="request">The request that describes what to create. Ignored.</param>
        /// <param name="context">
        /// A context that can be used to create other specimens. Ignored.
        /// </param>
        /// <returns>
        /// The specimen supplied to the instance in the constructor.
        /// </returns>
        /// <seealso cref="AutoFixture.Kernel.FixedBuilder(object)"/>
        public object Create(object request, ISpecimenContext context)
        {
            return null;
        }
    }
}