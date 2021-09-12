using System ;
using System.Diagnostics.CodeAnalysis ;
using System.Globalization ;
using System.Linq ;
using FluentAssertions ;
using JetBrains.Annotations ;
using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace Selkie.AutoMocking.Tests
{
    [ TestClass ]
    public class ParameterInfoTests
    {
        private const string MethodNameWithString = "WithString" ;

        private System.Reflection.ParameterInfo _parameterInfoString ;

        [ ExcludeFromCodeCoverage ]
        [ UsedImplicitly ]
        public static System.Reflection.ParameterInfo [ ] WithString ( [ Freeze ] string _ )
        {
            return Array.Empty < System.Reflection.ParameterInfo > ( ) ;
        }

        [ TestMethod ]
        public void Constructor_ForParameterInfo_SetsCustomAttributes ( )
        {
            CreateSut ( )
               .CustomAttributes
               .Count ( )
               .Should ( )
               .Be ( 1 ) ;
        }

        [ TestMethod ]
        public void Constructor_ForParameterInfo_SetsParameterType ( )
        {
            CreateSut ( )
               .ParameterType
               .Should ( )
               .Be ( typeof ( string ) ) ;
        }

        [ TestMethod ]
        public void Constructor_ForParameterInfoIsNull_Throws ( )
        {
            _parameterInfoString = null ;

            Action action = ( ) => CreateSut ( ) ;

            action.Should ( )
                  .Throw < ArgumentNullException > ( )
                  .WithParameter ( "parameterInfo" ) ;
        }

        [ TestInitialize ]
        public void TestInitialize ( )
        {
            var methodInfo = typeof ( ParameterInfoTests ).GetMethod ( MethodNameWithString ,
                                                                       new [ ]
                                                                       {
                                                                           typeof ( string )
                                                                       } ) ;

            if ( methodInfo == null )
            {
                var message = string.Format ( CultureInfo.InvariantCulture ,
                                              $"Can't find method {MethodNameWithString}" ) ;

                throw new Exception ( message ) ;
            }

            var infos = methodInfo.GetParameters ( ) ;

            _parameterInfoString = infos.First ( ) ;
        }

        private ParameterInfo CreateSut ( )
        {
            return new ParameterInfo ( _parameterInfoString ) ;
        }
    }
}