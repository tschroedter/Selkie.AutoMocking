using System;
using System.Collections.Generic;

namespace Selkie.AutoMocking.Net8.Tests
{
    public static class GuardTestData
    {
        public static IEnumerable<object[]> NullEmptyOrWhitespace()
        {
            // For null, expect ArgumentNullException
            yield return [null, typeof(ArgumentNullException)];

            // For empty string and whitespace, expect ArgumentException
            yield return ["", typeof(ArgumentException)];
            yield return ["   ", typeof(ArgumentException)];
        }

        public static IEnumerable<object[]> NullOrEmpty()
        {
            // For null, expect ArgumentNullException
            yield return [null, typeof(ArgumentNullException)];

            // For empty string, expect ArgumentException
            yield return ["", typeof(ArgumentException)];
        }

        public static IEnumerable<object[]> InstanceAndInteger()
        {
            // Valid cases: non-empty string, integer, and object instance.
            yield return ["valid"];
            yield return [1];
            yield return [new object()];
        }
    }
}