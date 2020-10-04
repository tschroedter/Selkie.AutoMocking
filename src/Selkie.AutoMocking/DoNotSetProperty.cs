using System ;
using System.Reflection ;
using AutoFixture.Kernel ;
using JetBrains.Annotations ;

namespace Selkie.AutoMocking
{
    public class DoNotSetProperty : ISpecimenBuilder // todo testing
    {
        private readonly Type _type ;

        public DoNotSetProperty ( [ NotNull ] Type type )
        {
            Guard.ArgumentNotNull ( type , nameof ( type ) ) ;

            _type = type ;
        }

        public object Create ( object request , ISpecimenContext context )
        {
            var propertyInfo = request as PropertyInfo ;

            if ( propertyInfo?.ReflectedType != _type )
                return new NoSpecimen ( ) ;

            if ( IsProperty ( request ) ) return new OmitSpecimen ( ) ;

            return new NoSpecimen ( ) ;
        }

        private static bool IsProperty ( object request )
        {
            return request is PropertyInfo ;
        }
    }
}