using System.Diagnostics.CodeAnalysis ;
using NotNull = JetBrains.Annotations.NotNullAttribute ;

namespace Selkie.AutoMocking.Net5.Tests.TestClasses
{
    [ ExcludeFromCodeCoverage ]
    public class Something : ISomething
    {
        public Something ( [ @NotNull ] ISomethingElse something )
        {
            Guard.ArgumentNotNull ( something ,
                                    nameof ( something ) ) ;

            SomethingElse = something ;
            Name          = "SomethingElse.Name" ;
        }

        public string Name { get ; set ; }

        public ISomethingElse SomethingElse { get ; }
    }
}