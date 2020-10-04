using System ;
using AutoFixture ;
using JetBrains.Annotations ;

namespace Selkie.AutoMocking
{
    public class DoNotSetPropertyCustomization : ICustomization // todo testing
    {
        private readonly Type _type ;

        public DoNotSetPropertyCustomization ( [ NotNull ] Type type )
        {
            Guard.ArgumentNotNull ( type , nameof ( type ) ) ;

            _type = type ;
        }

        public void Customize ( [ NotNull ] IFixture fixture )
        {
            Guard.ArgumentNotNull ( fixture , nameof ( fixture ) ) ;

            fixture.Customizations.Add ( new DoNotSetProperty ( _type ) ) ;
        }
    }
}