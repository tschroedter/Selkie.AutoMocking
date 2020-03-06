using JetBrains.Annotations;

namespace Selkie.AutoMocking.Tests.TestClasses
{
    public interface ISomething
    {
        [UsedImplicitly] ISomethingElse SomethingElse { get; }
    }
}