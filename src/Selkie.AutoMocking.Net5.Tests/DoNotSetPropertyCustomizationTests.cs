using System ;
using System.Collections.Generic ;
using AutoFixture ;
using AutoFixture.Kernel ;
using FluentAssertions ;
using Microsoft.VisualStudio.TestTools.UnitTesting ;
using NSubstitute ;
using Selkie.AutoMocking.Net5.Tests.TestClasses ;

namespace Selkie.AutoMocking.Net5.Tests
{
    [ TestClass ]
    public class DoNotSetPropertyCustomizationTests
    {
        private List < ISpecimenBuilder > _builders ;
        private IFixture                  _fixture ;
        private Type                      _targetType ;

        [ TestInitialize ]
        public void Initialize ( )
        {
            _targetType = typeof ( Something ) ;

            _fixture = Substitute.For < IFixture > ( ) ;

            _builders = new List < ISpecimenBuilder > ( ) ;

            _fixture.Customizations
                    .Returns ( _builders ) ;
        }

        [ TestMethod ]
        public void Constructor_ForTargetTypeIsNull_Throws ( )
        {
            _targetType = null ;

            Action action = ( ) => CreateSut ( ) ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "targetType" ) ;
        }

        [ TestMethod ]
        public void Constructor_ForTargetType_SetsTargetType ( )
        {
            CreateSut ( )
               .TargetType
               .Should ( )
               .Be ( _targetType ) ;
        }

        [ TestMethod ]
        public void Customize_ForFixtureIsNull_Throws ( )
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = ( ) => CreateSut ( ).Customize ( null ) ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "fixture" ) ;
        }

        private DoNotSetPropertyCustomization CreateSut ( )
        {
            return new DoNotSetPropertyCustomization ( _targetType ) ;
        }
    }
}