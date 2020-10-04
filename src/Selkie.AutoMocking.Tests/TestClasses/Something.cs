using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Selkie.AutoMocking.Tests.TestClasses
{
    [ExcludeFromCodeCoverage]
    public class Something : ISomething
    {
        public Something([NotNull] ISomethingElse something)
        {
            Guard.ArgumentNotNull(something,
                                  nameof(something));

            SomethingElse = something;
            Name = "SomethingElse.Name";
        }

        public ISomethingElse SomethingElse { get; }

        public string Name { get; set; }
    }
}