using System ;
using AutoFixture ;
using AutoFixture.Kernel ;
using FluentAssertions ;
using FluentAssertions.Execution ;
using Microsoft.VisualStudio.TestTools.UnitTesting ;
using NSubstitute ;
using Selkie.AutoMocking.Interfaces ;
using Selkie.AutoMocking.Net5.Tests.TestClasses ;

namespace Selkie.AutoMocking.Net5.Tests
{
    [ TestClass ]
    public class ArgumentsGeneratorTests
    {
        private ICustomAttributeData [ ] _customAttributesWithFreeze ;
        private IFixture                 _fixture ;
        private ICustomAttributeData     _freezeAttribute ;
        private IParameterInfo           _infoClass ;
        private IParameterInfo           _infoInt ;
        private IParameterInfo           _infoLazyClass ;
        private IParameterInfo           _infoLazyString ;
        private IParameterInfo           _infoSomething ;
        private IParameterInfo           _infoSomethingElseWithFreeze ;
        private IParameterInfo           _infoString ;
        private IParameterInfo           _infoStringFreeze ;
        private IParameterInfo           _infoSut ;
        private SutCreator               _sutCreator ;

        [ TestMethod ]
        public void Create_ForCanNotCreateType_Throws ( )
        {
            _fixture = Substitute.For < IFixture > ( ) ;

            _fixture.Create ( Arg.Any < Type > ( ) ,
                              Arg.Any < SpecimenContext > ( ) )
                    .Returns ( null ) ;

            IParameterInfo [ ] infos =
            {
                _infoString
            } ;

            Action action = ( ) => { CreateSut ( ).Create ( infos ) ; } ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( typeof ( string ).FullName ) ;
        }

        [ TestMethod ]
        public void Create_ForFixtureIsNull_Throws ( )
        {
            _fixture = null ;

            Action action = ( ) => CreateSut ( ) ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "fixture" ) ;
        }

        [ TestMethod ]
        public void Create_ForSutCreatorIsNull_Throws ( )
        {
            _sutCreator = null ;

            Action action = ( ) => CreateSut ( ) ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "sutCreator" ) ;
        }

        [ TestMethod ]
        public void Create_ForParameterInfoAndFreezeIsTrue_DoesNotAffectAlreadyCreatedParameters ( )
        {
            _infoInt.CustomAttributes.Returns ( _customAttributesWithFreeze ) ;

            IParameterInfo [ ] infos =
            {
                _infoSut ,
                _infoString ,
                _infoStringFreeze ,
                _infoString
            } ;

            using ( new AssertionScope ( ) )
            {
                var actual = CreateSut ( )
                   .Create ( infos ) ;

                actual.Length
                      .Should ( )
                      .Be ( 4 ) ;

                actual [ 1 ]
                   .Should ( )
                   .NotBeSameAs ( actual [ 2 ] ) ;

                actual [ 2 ]
                   .Should ( )
                   .BeSameAs ( actual [ 3 ] ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForParameterInfoArrayContainsIntegerAndStringAndClass_Instance ( )
        {
            IParameterInfo [ ] infos =
            {
                _infoInt ,
                _infoString ,
                _infoClass
            } ;

            var actual = CreateSut ( )
               .Create ( infos ) ;

            using ( new AssertionScope ( ) )
            {
                actual
                   .Length
                   .Should ( )
                   .Be ( 3 ,
                         "3 Parameters: integer, string and class" ) ;

                actual [ 0 ]
                   .Should ( )
                   .BeOfType < int > ( ) ;

                actual [ 1 ]
                   .Should ( )
                   .BeOfType < string > ( ) ;

                actual [ 2 ]
                   .Should ( )
                   .BeOfType < SomethingElse > ( ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForParameterInfoArrayContainsSingleClass_Instance ( )
        {
            IParameterInfo [ ] infos =
            {
                _infoClass
            } ;

            var actual = CreateSut ( )
               .Create ( infos ) ;

            using ( new AssertionScope ( ) )
            {
                actual
                   .Length
                   .Should ( )
                   .Be ( 1 ,
                         "1 Parameter: class" ) ;

                actual [ 0 ]
                   .Should ( )
                   .BeOfType < SomethingElse > ( ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForParameterInfoArrayContainsSingleClassDependingOnInterface_Instance ( )
        {
            IParameterInfo [ ] infos =
            {
                _infoSut
            } ;

            var actual = CreateSut ( )
               .Create ( infos ) ;

            using ( new AssertionScope ( ) )
            {
                actual
                   .Length
                   .Should ( )
                   .Be ( 1 ,
                         "1 Parameter: interface depending on other interface" ) ;

                var sut = actual [ 0 ] as Something ;

                sut.Should ( )
                   .NotBeNull ( ) ;

                // ReSharper disable once PossibleNullReferenceException
                sut.SomethingElse
                   .GetType ( )
                   .FullName
                   .Should ( )
                   .StartWith ( "Castle.Proxies.ObjectProxy" ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForParameterInfoArrayContainsSingleInteger_Instance ( )
        {
            IParameterInfo [ ] infos =
            {
                _infoInt
            } ;

            var actual = CreateSut ( )
               .Create ( infos ) ;

            using ( new AssertionScope ( ) )
            {
                actual
                   .Length
                   .Should ( )
                   .Be ( 1 ,
                         "1 Parameter: integer" ) ;

                actual [ 0 ]
                   .Should ( )
                   .BeOfType < int > ( ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForParameterInfoArrayContainsSingleInterface_Instance ( )
        {
            IParameterInfo [ ] infos =
            {
                _infoSomething
            } ;

            var actual = CreateSut ( )
               .Create ( infos ) ;

            using ( new AssertionScope ( ) )
            {
                actual
                   .Length
                   .Should ( )
                   .Be ( 1 ,
                         "1 Parameter: interface" ) ;

                actual [ 0 ]
                   .GetType ( )
                   .FullName
                   .Should ( )
                   .StartWith ( "Castle.Proxies.ObjectProxy" ) ;

                actual [ 0 ]
                   .Should ( )
                   .BeAssignableTo < ISomething > ( ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForParameterInfoArrayContainsSingleString_Instance ( )
        {
            IParameterInfo [ ] infos =
            {
                _infoString
            } ;

            var actual = CreateSut ( )
               .Create ( infos ) ;

            using ( new AssertionScope ( ) )
            {
                actual
                   .Length
                   .Should ( )
                   .Be ( 1 ,
                         "1 Parameter: string" ) ;

                actual [ 0 ]
                   .Should ( )
                   .BeOfType < string > ( ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForParameterInfoArrayIsEmpty_Empty ( )
        {
            var infos = Array.Empty < IParameterInfo > ( ) ;

            CreateSut ( )
               .Create ( infos )
               .Should ( )
               .BeEmpty ( ) ;
        }

        [ TestMethod ]
        public void Create_ForParameterInfoWithFreezeAttribute_ParameterAreTheSame ( )
        {
            _infoInt.CustomAttributes.Returns ( _customAttributesWithFreeze ) ;

            IParameterInfo [ ] infos =
            {
                _infoSut ,
                _infoStringFreeze ,
                _infoString
            } ;

            using ( new AssertionScope ( ) )
            {
                var actual = CreateSut ( )
                   .Create ( infos ) ;

                actual.Length
                      .Should ( )
                      .Be ( 3 ) ;

                actual [ 0 ]
                   .Should ( )
                   .NotBeNull ( ) ;

                actual [ 1 ]
                   .Should ( )
                   .BeSameAs ( actual [ 2 ] ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForParameterInfoWithFreezeAttribute_SutUsesFrozenInstances ( )
        {
            _infoInt.CustomAttributes.Returns ( _customAttributesWithFreeze ) ;

            IParameterInfo [ ] infos =
            {
                _infoSut ,
                _infoSomethingElseWithFreeze
            } ;

            using ( new AssertionScope ( ) )
            {
                var actual = CreateSut ( )
                   .Create ( infos ) ;

                actual.Length
                      .Should ( )
                      .Be ( 2 ) ;

                var something = actual [ 0 ] as Something ;

                something.Should ( )
                         .NotBeNull ( ) ;

                // ReSharper disable once PossibleNullReferenceException
                something
                   .SomethingElse
                   .Should ( )
                   .BeSameAs ( actual [ 1 ] ) ;

                ( actual [ 1 ] as ISomethingElse )
                   .Should ( )
                   .NotBeNull ( ) ;
            }
        }


        [ TestMethod ]
        public void Create_ForParameterInfoWithoutFreezeAttribute_ParameterAreDifferent ( )
        {
            _infoInt.CustomAttributes.Returns ( _customAttributesWithFreeze ) ;

            IParameterInfo [ ] infos =
            {
                _infoString ,
                _infoString
            } ;

            using ( new AssertionScope ( ) )
            {
                var actual = CreateSut ( )
                   .Create ( infos ) ;

                actual.Length
                      .Should ( )
                      .Be ( 2 ) ;

                actual [ 0 ]
                   .Should ( )
                   .NotBeSameAs ( actual [ 1 ] ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForLazyStringParameterInfo_Instance ( )
        {
            _infoInt.CustomAttributes.Returns ( _customAttributesWithFreeze ) ;

            IParameterInfo [ ] infos =
            {
                _infoLazyString
            } ;

            using ( new AssertionScope ( ) )
            {
                var actual = CreateSut ( )
                   .Create ( infos ) ;

                actual.Length
                      .Should ( )
                      .Be ( 1 ) ;

                var lazy = actual [ 0 ] as Lazy < string > ;

                lazy?.IsValueCreated
                     .Should ( )
                     .BeFalse ( ) ;

                var lazyValue = lazy?.Value ;

                lazyValue?.Should ( )
                          .NotBeNull ( ) ;
            }
        }

        [ TestMethod ]
        public void Create_ForLazySomethingParameterInfo_Instance ( )
        {
            _infoInt.CustomAttributes.Returns ( _customAttributesWithFreeze ) ;

            IParameterInfo [ ] infos =
            {
                _infoLazyClass
            } ;

            using ( new AssertionScope ( ) )
            {
                var sut = CreateSut ( ) ;

                var actual = sut.Create ( infos ) ;

                actual.Length
                      .Should ( )
                      .Be ( 1 ) ;

                var lazy = actual [ 0 ] as Lazy < Something > ;

                lazy.Should ( )
                    .NotBeNull ( ) ;

                lazy?.IsValueCreated
                     .Should ( )
                     .BeFalse ( ) ;

                var lazyValue = lazy?.Value ;

                lazyValue?.Should ( )
                          .NotBeNull ( ) ;
            }
        }

        [ TestInitialize ]
        public void TestInitialize ( )
        {
            _fixture = new Fixture ( ) ;
            _sutCreator = new SutCreator ( new SutInstanceCreator ( new ArgumentNullExceptionFinder ( ) ) ,
                                           new SutLazyInstanceCreator ( new ArgumentNullExceptionFinder ( ) ,
                                                                        new CustomAttributeFinder ( ) ) ) ;
            _freezeAttribute = Substitute.For < ICustomAttributeData > ( ) ;
            _freezeAttribute.AttributeType.Returns ( typeof ( FreezeAttribute ) ) ;

            _customAttributesWithFreeze = new [ ]
                                          {
                                              _freezeAttribute
                                          } ;

            _infoInt                     = CreateParameterInfo ( typeof ( int ) ) ;
            _infoLazyString              = CreateParameterInfo ( typeof ( Lazy < string > ) ) ;
            _infoString                  = CreateParameterInfo ( typeof ( string ) ) ;
            _infoClass                   = CreateParameterInfo ( typeof ( SomethingElse ) ) ;
            _infoSut                     = CreateParameterInfo ( typeof ( Something ) ) ;
            _infoLazyClass               = CreateParameterInfo ( typeof ( Lazy < Something > ) ) ;
            _infoSomething               = CreateParameterInfo ( typeof ( ISomething ) ) ;
            _infoSomethingElseWithFreeze = CreateParameterInfo ( typeof ( ISomethingElse ) ) ;
            _infoSomethingElseWithFreeze.CustomAttributes.Returns ( _customAttributesWithFreeze ) ;
            _infoStringFreeze = CreateParameterInfo ( typeof ( string ) ) ;
            _infoStringFreeze.CustomAttributes.Returns ( _customAttributesWithFreeze ) ;
        }

        private IParameterInfo CreateParameterInfo ( Type type )
        {
            var info = Substitute.For < IParameterInfo > ( ) ;
            info.CustomAttributes.Returns ( Array.Empty < ICustomAttributeData > ( ) ) ;
            info.ParameterType.Returns ( type ) ;

            return info ;
        }

        private ArgumentsGenerator CreateSut ( )
        {
            return new ArgumentsGenerator ( _fixture ,
                                            _sutCreator ) ;
        }
    }
}