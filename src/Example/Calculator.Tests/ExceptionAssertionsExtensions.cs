using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Specialized;
using JetBrains.Annotations;

namespace Calculator.Tests
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionAssertionsExtensions
    {
        [UsedImplicitly]
        public static AndConstraint<StringAssertions> WithParameter(
            this ExceptionAssertions<ArgumentException> assertions,
            string                                      parameter)
        {
            return assertions.And
                             .ParamName
                             .Should()
                             .Be(parameter);
        }

        [UsedImplicitly]
        public static AndConstraint<StringAssertions> WithParameter(
            this ExceptionAssertions<ArgumentNullException> assertions,
            string                                          parameter)
        {
            return assertions.And
                             .ParamName
                             .Should()
                             .Be(parameter);
        }
    }
}