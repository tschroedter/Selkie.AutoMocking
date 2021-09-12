using System ;
using AutoFixture.Kernel ;
using FluentAssertions ;
using Microsoft.VisualStudio.TestTools.UnitTesting ;
using NSubstitute ;
using Selkie.AutoMocking.Net5.Tests.TestClasses ;

namespace Selkie.AutoMocking.Net5.Tests
{
    [ TestClass ]
    public class DoNotSetPropertyBuilderTests
    {
        private ISpecimenContext _context ;
        private object           _request ;
        private Type             _targetType ;

        [ TestInitialize ]
        public void Initialize ( )
        {
            _targetType = typeof ( Something ) ;

            _request = new object ( ) ;
            _context = Substitute.For < ISpecimenContext > ( ) ;
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
        public void Create_ForRequestIsNull_Throws ( )
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = ( ) => CreateSut ( ).Create ( null , _context ) ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "request" ) ;
        }

        [ TestMethod ]
        public void Create_ForContextIsNull_Throws ( )
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = ( ) => CreateSut ( ).Create ( _request , null ) ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "context" ) ;
        }

        [ TestMethod ]
        public void Create_ForRequestIsPropertyInfoAndDoesNotMatchTargetType_ReturnsNoSpecimen ( )
        {
            _targetType = typeof ( SomethingElse ) ;
            _request    = CreateForString ( ) ;

            CreateSut ( ).Create ( _request , _context )
                         .Should ( )
                         .BeOfType < NoSpecimen > ( ) ;
        }

        [ TestMethod ]
        public void Create_ForRequestIsPropertyInfoAndTargetType_ReturnsOmitSpecimen ( )
        {
            _request = CreateForString ( ) ;

            CreateSut ( ).Create ( _request , _context )
                         .Should ( )
                         .BeOfType < OmitSpecimen > ( ) ;
        }

        [ TestMethod ]
        public void Create_ForRequestIsNotPropertyInfo_ReturnsNoSpecimen ( )
        {
            _request = new object ( ) ;

            CreateSut ( ).Create ( _request , _context )
                         .Should ( )
                         .BeOfType < NoSpecimen > ( ) ;
        }

        private object CreateForString ( )
        {
            return typeof ( Something ).GetProperty ( "Name" ) ;
        }

        private DoNotSetPropertyBuilder CreateSut ( )
        {
            return new DoNotSetPropertyBuilder ( _targetType ) ;
        }
    }
}