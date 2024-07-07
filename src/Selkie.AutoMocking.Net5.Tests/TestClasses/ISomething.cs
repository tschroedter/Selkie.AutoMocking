using JetBrains.Annotations ;

namespace Selkie.AutoMocking.Net8.Tests.TestClasses
{
    public interface ISomething
    {
        [ UsedImplicitly ] ISomethingElse SomethingElse { get ; }
    }
}