using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selkie.AutoMocking.Net8.Tests;

[TestClass]
public class GuardTests
{
    public static void AssertException(Action action,
        Type type,
        string parameter)
    {
        using (new AssertionScope())
        {
            action.Should()
                .Throw<Exception>()
                .And.GetType()
                .Should()
                .Be(type);

            if (type == typeof(ArgumentException))
                action.Should()
                    .Throw<ArgumentException>()
                    .WithParameter(parameter);
        }
    }

    [TestMethod]
    [DynamicData(nameof(GuardTestData.NullEmptyOrWhitespace),
        typeof(GuardTestData))]
    public void ArgumentNotEmptyOrWhitespace_ForInvalidValues_Throws(string value,
        Type type)
    {
        AssertException(() => Guard.ArgumentNotEmptyOrWhitespace(value,
                "parameter"),
            type,
            "parameter");
    }

    [TestMethod]
    [DynamicData(nameof(GuardTestData.InstanceAndInteger),
        typeof(GuardTestData))]
    public void ArgumentNotEmptyOrWhitespace_ForValues_DoesNotThrows(object value)
    {
        var action = new Action(() => Guard.ArgumentNotEmptyOrWhitespace(value,
            "parameter"));

        action.Should()
            .NotThrow();
    }

    [TestMethod]
    [DynamicData(nameof(GuardTestData.InstanceAndInteger),
        typeof(GuardTestData))]
    public void ArgumentNotNull_ForValueNotNull_DoesNotThrows(object value)
    {
        var action = new Action(() => Guard.ArgumentNotNull(value,
            "parameter"));

        action.Should()
            .NotThrow();
    }

    [TestMethod]
    public void ArgumentNotNull_ForValueNull_Throws()
    {
        // ReSharper disable once AssignNullToNotNullAttribute
        var action = new Action(() => Guard.ArgumentNotNull(null,
            "parameter"));

        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameter("parameter");
    }

    [TestMethod]
    [DynamicData(nameof(GuardTestData.InstanceAndInteger),
        typeof(GuardTestData))]
    public void ArgumentNotNullOrEmpty_ForValues_DoesNotThrows(object value)
    {
        var action = new Action(() => Guard.ArgumentNotNullOrEmpty(value,
            "parameter"));

        action.Should()
            .NotThrow();
    }

    [TestMethod]
    [DynamicData(nameof(GuardTestData.NullOrEmpty),
        typeof(GuardTestData))]
    public void ArgumentNotNullOrEmpty_ForValues_Throws(string value,
        Type type)
    {
        AssertException(() => Guard.ArgumentNotNullOrEmpty(value,
                "parameter"),
            type,
            "parameter");
    }
}