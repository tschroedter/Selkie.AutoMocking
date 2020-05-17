using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Selkie.AutoMocking.Tests.TestClasses;

namespace Selkie.AutoMocking.Tests
{
    [AutoDataTestClass]
    public class AutoDataTestMethodAttributeTests
    {
        private const string MethodNameWithInterface = "WithInterface";

        private System.Reflection.ParameterInfo[] _parameterInfoString;
        private ITestMethod                       _testMethod;
        private TestMethodAttribute               _testMethodAttribute;

        [ExcludeFromCodeCoverage]
        [UsedImplicitly]
        public static System.Reflection.ParameterInfo[] WithInterface([Freeze] ISomething _)
        {
            return Array.Empty<System.Reflection.ParameterInfo>();
        }

        [TestMethod]
        public void Constructor_ForTestMethodAttributeIsNull_Throws()
        {
            Action action = () => CreateSut(null);

            action.Should()
                  .Throw<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be("testMethodAttribute");
        }

        [TestMethod]
        public void Constructor_Invoked_SetsGenerator()
        {
            CreateSut(_testMethodAttribute)
               .Generator
               .Should()
               .NotBeNull();
        }

        [TestMethod]
        public void Constructor_Invoked_SetsTestMethodAttribute()
        {
            CreateSut(_testMethodAttribute)
               .TestMethodAttribute
               .Should()
               .NotBeNull();
        }

        [TestMethod]
        public void DefaultConstructor_Invoked_SetsGenerator()
        {
            CreateSut()
               .Generator
               .Should()
               .NotBeNull();
        }

        [TestMethod]
        public void DefaultConstructor_Invoked_TestMethodAttributeIsNull()
        {
            CreateSut()
               .TestMethodAttribute
               .Should()
               .BeNull();
        }

        [TestMethod]
        public void Execute_ForTestMethodIsNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = () => CreateSut()
                               .Execute(null);

            action.Should()
                  .Throw<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be("testMethod");
        }

        [TestMethod]
        public void Execute_ForTestMethodWithoutParameters_Invoked()
        {
            CreateSut()
               .Execute(_testMethod);

            _testMethod.Received()
                       .Invoke(Arg.Any<object[]>());
        }

        [TestMethod]
        public void Execute_ForTestMethodWithParameters_Invoked()
        {
            _testMethod.ParameterTypes
                       .Returns(_parameterInfoString);

            CreateSut()
               .Execute(_testMethod);

            _testMethod.Received()
                       .Invoke(Arg.Is<object[]>(x => ExpectedObjects(x)));
        }

        [AutoDataTestMethod]
        public void Invoked_ForFirstParameter_ReturnsInstance(Something sut)
        {
            var test = typeof(Lazy<Something>).FullName;

            using (new AssertionScope())
            {
                sut.Should()
                   .NotBeNull("Instance should be created");

                sut?.GetType()
                    .Should()
                    .Be<Something>("Instance should be of given type");
            }
        }

        [AutoDataTestMethod]
        public void Invoked_ForLazySut_ReturnsInstance(
            Lazy<Something> sut)
        {
            using (new AssertionScope())
            {
                sut.Should()
                   .NotBeNull();

                sut.IsValueCreated
                   .Should()
                   .BeFalse();

                sut.Value
                   .Should()
                   .BeOfType<Something>();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _testMethodAttribute = new TestMethodAttribute();
            _testMethod          = Substitute.For<ITestMethod>();

            var methodInfo = GetType()
               .GetMethod(MethodNameWithInterface,
                          new[]
                          {
                              typeof(ISomething)
                          });

            if (methodInfo == null)
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                                            $"Can't find method '{MethodNameWithInterface}'");

                throw new Exception(message);
            }

            _parameterInfoString = methodInfo.GetParameters();
        }

        [ExcludeFromCodeCoverage]
        private static bool ExpectedObjects(IReadOnlyList<object> x)
        {
            if (x.Count != 1) return false;

            return x[0] is ISomething;
        }


        private AutoDataTestMethodAttribute CreateSut()
        {
            return new AutoDataTestMethodAttribute();
        }

        private AutoDataTestMethodAttribute CreateSut(TestMethodAttribute attribute)
        {
            return new AutoDataTestMethodAttribute(attribute);
        }
    }
}