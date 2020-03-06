using System.Diagnostics.CodeAnalysis;

// ReSharper disable UnusedMember.Global

namespace Selkie.AutoMocking.Tests.TestClasses
{
    [ExcludeFromCodeCoverage]
    public class SomethingElse : ISomethingElse
    {
        public int    Number { get; set; }
        public string Text   { get; set; }
    }
}