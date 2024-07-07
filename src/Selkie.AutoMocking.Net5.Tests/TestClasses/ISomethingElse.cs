using JetBrains.Annotations ;

namespace Selkie.AutoMocking.Net8.Tests.TestClasses
{
    public interface ISomethingElse
    {
        [ UsedImplicitly ] int    Number { get ; set ; }
        [ UsedImplicitly ] string Text   { get ; set ; }
    }
}