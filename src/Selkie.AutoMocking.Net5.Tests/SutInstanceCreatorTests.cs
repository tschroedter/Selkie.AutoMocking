using System ;
using FluentAssertions ;
using FluentAssertions.Execution ;
using Microsoft.VisualStudio.TestTools.UnitTesting ;
using NSubstitute ;
using Selkie.AutoMocking.Interfaces ;
using Selkie.AutoMocking.Net5.Tests.TestClasses ;

namespace Selkie.AutoMocking.Net5.Tests
{
    [ TestClass ]
    public class SutInstanceCreatorTests
    {
        private const string ParameterName = "Name" ;

        private IArgumentNullExceptionFinder _finder ;
        private IArgumentsGenerator          _generator ;
        private Something                    _instance ;
        private Type                         _typeClass ;
        private Type                         _typeInt ;

        [ TestInitialize ]
        public void Initialize ( )
        {
            _finder    = Substitute.For < IArgumentNullExceptionFinder > ( ) ;
            _generator = Substitute.For < IArgumentsGenerator > ( ) ;
            _typeClass = typeof ( Something ) ;
            _typeInt   = typeof ( int ) ;
            _instance  = new Something ( new SomethingElse ( ) ) ;
        }

        [ TestMethod ]
        public void Constructor_ForFinderIsNull_Throws ( )
        {
            _finder = null ;

            Action action = ( ) => { CreateSut ( ) ; } ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "finder" ) ;
        }

        [ TestMethod ]
        public void Construct_ForGeneratorIsNull_Throws ( )
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = ( ) => { CreateSut ( ).Construct ( null , _typeClass ) ; } ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "generator" ) ;
        }

        [ TestMethod ]
        public void Construct_ForTypeIsNull_Throws ( )
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = ( ) => { CreateSut ( ).Construct ( _generator , null ) ; } ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "type" ) ;
        }

        [ TestMethod ]
        public void Construct_ForTypeClass_Instance ( )
        {
            _generator.CreateArgument ( _typeClass )
                      .Returns ( _instance ) ;

            CreateSut ( ).Construct ( _generator ,
                                      _typeClass ).Should ( )
                         .Be ( _instance ) ;
        }

        [ TestMethod ]
        public void Construct_ForTypeLazyValueType_Instance ( )
        {
            _generator.CreateArgument ( _typeInt )
                      .Returns ( 1 ) ;

            var actual = CreateSut ( ).Construct ( _generator ,
                                                   _typeInt ) ;

            using ( new AssertionScope ( ) )
            {
                actual.Should ( )
                      .NotBeNull ( ) ;

                ( actual is int ).Should ( )
                                 .BeTrue ( ) ;

                ( ( int ) actual ).Should ( )
                                  .Be ( 1 ) ;
            }
        }

        [ TestMethod ]
        public void Construct_ForGeneratorThrowsFinderReturnsFalse_Throws ( )
        {
            FinderReturns ( false ) ;
            GeneratorCreateThrows ( ) ;

            Action action = ( ) =>
                            {
                                CreateSut ( ).Construct ( _generator ,
                                                          _typeClass ) ;
                            } ;

            action.Should ( )
                  .Throw < Exception > ( ) ;
        }

        [ TestMethod ]
        public void Construct_ForGeneratorThrowsAndFinderReturnsTrue_ThrowsArgumentNullException ( )
        {
            FinderReturns ( true ) ;
            GeneratorCreateThrows ( ) ;

            Action action = ( ) =>
                            {
                                CreateSut ( ).Construct ( _generator ,
                                                          _typeClass ) ;
                            } ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( ParameterName ) ;
        }

        private void GeneratorCreateThrows ( )
        {
            _generator.When ( x => x.CreateArgument ( _typeClass ) )
                      .Do ( _ => throw new Exception ( "Test" ) ) ;
        }

        private void FinderReturns ( bool value )
        {
            if ( ! value )
                _finder.TryFindArgumentNullException ( Arg.Any < Exception > ( ) ,
                                                       out Arg.Any < ArgumentNullException > ( ) )
                       .Returns ( x =>
                                  {
                                      x [ 1 ] = null ;

                                      return false ;
                                  } ) ;
            else
                _finder.TryFindArgumentNullException ( Arg.Any < Exception > ( ) ,
                                                       out Arg.Any < ArgumentNullException > ( ) )
                       .Returns ( x =>
                                  {
                                      x [ 1 ] = new ArgumentNullException ( ParameterName ) ;

                                      return true ;
                                  } ) ;
        }

        private SutInstanceCreator CreateSut ( )
        {
            return new SutInstanceCreator ( _finder ) ;
        }
    }
}