using System;
using System.Collections.Generic;

namespace Selkie.AutoMocking.Net8.Tests
{
    public static class GuardTestData
    {
        public static IEnumerable<object[]> NullEmptyOrWhitespace()
        {
            yield return new object[] { null, typeof(ArgumentException) };
            yield return new object[] { "", typeof(ArgumentException) };
            yield return new object[] { "   ", typeof(ArgumentException) };
        }

        public static IEnumerable<object[]> InstanceAndInteger()
        {
            yield return new object[] { "valid value" };
            yield return new object[] { 123 };
        }

        public static IEnumerable<object[]> NullOrEmpty()
        {
            yield return new object[] { null, typeof(ArgumentException) };
            yield return new object[] { "", typeof(ArgumentException) };
        }
    }
}