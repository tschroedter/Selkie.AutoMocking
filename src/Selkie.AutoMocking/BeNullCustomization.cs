using System ;
using System.Globalization ;
using System.Linq ;
using System.Reflection ;
using AutoFixture ;
using AutoFixture.Kernel ;
using JetBrains.Annotations ;

namespace Selkie.AutoMocking
{
    /// <summary>
    ///     Customization that supports setting a registered type to be always
    ///     null when resolved.
    /// </summary>
    public class BeNullCustomization
        : ICustomization
    {
        public BeNullCustomization ( Type targetType )
            : this ( targetType , targetType )
        {
        }

        /// <summary>
        ///     Set the given registered type to be always null.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="registeredType"></param>
        public BeNullCustomization ( [ NotNull ] Type targetType , [ NotNull ] Type registeredType )
        {
            Guard.ArgumentNotNull ( targetType ,     nameof ( targetType ) ) ;
            Guard.ArgumentNotNull ( registeredType , nameof ( registeredType ) ) ;

            if ( ! registeredType.GetTypeInfo ( ).IsAssignableFrom ( targetType ) )
            {
                var message = string.Format (
                                             CultureInfo.CurrentCulture ,
                                             "The type '{0}' cannot be set to null as '{1}' because the two types are not compatible." ,
                                             targetType ,
                                             registeredType ) ;

                throw new ArgumentException ( message,
                                              nameof(registeredType) ) ;
            }

            TargetType     = targetType ;
            RegisteredType = registeredType ;
        }

        /// <summary>
        ///     Gets the <see cref="Type" /> to freeze.
        /// </summary>
        public Type TargetType { get ; }

        /// <summary>
        ///     Gets the <see cref="Type" /> to which the frozen <see cref="TargetType" /> value
        ///     should be mapped to. Defaults to the same <see cref="Type" /> as <see cref="TargetType" />.
        /// </summary>
        public Type RegisteredType { get ; }

        /// <summary>
        ///     Add the customization for a NullBuilder to the given <see cref="IFixture" />.
        /// </summary>
        /// <param name="fixture">
        ///     The <see cref="IFixture" /> to be customized.
        /// </param>
        public void Customize ( [ NotNull ] IFixture fixture )
        {
            Guard.ArgumentNotNull ( fixture , nameof ( fixture ) ) ;

            var fixedBuilder = new NullBuilder ( ) ;

            var types = new [ ]
                        {
                            TargetType ,
                            RegisteredType
                        } ;

            var specimenBuilders = from t in types
                                   select SpecimenBuilderNodeFactory.CreateTypedNode ( t , fixedBuilder )
                                              as ISpecimenBuilder ;

            var builder = new CompositeSpecimenBuilder ( specimenBuilders ) ;

            fixture.Customizations
                   .Insert ( 0 ,
                             builder ) ;
        }
    }
}