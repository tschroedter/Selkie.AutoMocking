using System ;
using AutoFixture ;
using JetBrains.Annotations ;

namespace Selkie.AutoMocking
{
    public class DoNotSetPropertyCustomization : ICustomization
    {
        public DoNotSetPropertyCustomization ( [ NotNull ] Type targetType )
        {
            Guard.ArgumentNotNull ( targetType , nameof ( targetType ) ) ;

            TargetType = targetType ;
        }

        public Type TargetType { get ; }

        public void Customize ( [ NotNull ] IFixture fixture )
        {
            Guard.ArgumentNotNull ( fixture , nameof ( fixture ) ) ;

            fixture.Customizations
                   .Add ( new DoNotSetPropertyBuilder ( TargetType ) ) ;
        }
    }
}