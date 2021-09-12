using JetBrains.Annotations ;

namespace Selkie.AutoMocking.Net5.Tests.TestClasses
{
    public interface ISomethingElse
    {
        [ UsedImplicitly ] int    Number { get ; set ; }
        [ UsedImplicitly ] string Text   { get ; set ; }
    }
}