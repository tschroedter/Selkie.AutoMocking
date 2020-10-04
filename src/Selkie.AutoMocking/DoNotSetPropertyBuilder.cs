using System ;
using System.Reflection ;
using AutoFixture.Kernel ;
using JetBrains.Annotations ;

namespace Selkie.AutoMocking
{
    public class DoNotSetPropertyBuilder : ISpecimenBuilder
    {
        public DoNotSetPropertyBuilder ( [ NotNull ] Type targetType )
        {
            Guard.ArgumentNotNull ( targetType , nameof ( targetType ) ) ;

            TargetType = targetType ;
        }

        public Type TargetType { get ; }

        public object Create ( [ NotNull ] object request ,
                               [ NotNull ] ISpecimenContext context )
        {
            Guard.ArgumentNotNull ( request , nameof ( request ) ) ;
            Guard.ArgumentNotNull ( context , nameof ( context ) ) ;

            var propertyInfo = request as PropertyInfo ;

            if ( propertyInfo?.ReflectedType != TargetType )
                return new NoSpecimen ( ) ;

            if ( IsProperty ( request ) )
                return new OmitSpecimen ( ) ;

            return new NoSpecimen ( ) ;
        }

        private static bool IsProperty ( object request )
        {
            return request is PropertyInfo ;
        }
    }
}