using JetBrains.Annotations ;

namespace Selkie.AutoMocking.Net5.Tests.TestClasses
{
    public interface ISomething
    {
        [ UsedImplicitly ] ISomethingElse SomethingElse { get ; }
    }
}