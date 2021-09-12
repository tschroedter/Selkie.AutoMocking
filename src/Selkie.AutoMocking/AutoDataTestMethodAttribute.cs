using System.Collections.Generic ;
using System.Linq ;
using JetBrains.Annotations ;
using Microsoft.VisualStudio.TestTools.UnitTesting ;
using Selkie.AutoMocking.Interfaces ;


namespace Selkie.AutoMocking
{
    public sealed class AutoDataTestMethodAttribute : TestMethodAttribute
    {
        [ CanBeNull ] public TestMethodAttribute TestMethodAttribute { get ; }

        public AutoDataTestMethodAttribute ( )
        {
        }

        public AutoDataTestMethodAttribute ( [ NotNull ] TestMethodAttribute testMethodAttribute )
        {
            Guard.ArgumentNotNull ( testMethodAttribute ,
                                    nameof ( testMethodAttribute ) ) ;

            TestMethodAttribute = testMethodAttribute ;
        }

        [ NotNull ] internal IArgumentsGenerator Generator { get ; } = new ArgumentsGenerator ( ) ;

        public override TestResult [ ] Execute ( [ NotNull ] ITestMethod testMethod )
        {
            Guard.ArgumentNotNull ( testMethod ,
                                    nameof ( testMethod ) ) ;

            return Invoke ( testMethod ) ;
        }

        private TestResult [ ] Invoke ( ITestMethod testMethod )
        {
            if ( TestMethodAttribute != null ) return TestMethodAttribute.Execute ( testMethod ) ;

            IEnumerable < IParameterInfo >
                infos = testMethod.ParameterTypes.Select ( x => new ParameterInfo ( x ) ) ;

            var arguments = Generator.Create ( infos ) ;

            return new [ ]
                   {
                       testMethod.Invoke ( arguments )
                   } ;
        }
    }
}