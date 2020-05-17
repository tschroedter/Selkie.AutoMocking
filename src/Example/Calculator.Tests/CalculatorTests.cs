using System;
using Calculator.Interfaces;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Selkie.AutoMocking;

namespace Calculator.Tests
{
    [AutoDataTestClass]
    public class CalculatorTests
    {
        private IAdd      _add;
        private ISubtract _subtract;

        [AutoDataTestMethod]
        public void Constructor_ForAddIsNull_Throws(Lazy<Calculator> sut,
                                                    [BeNull] IAdd    add)
        {
            // ReSharper disable once UnusedVariable
            Action action = () =>
                            {
                                var test = sut.Value;
                            };

            action.Should()
                  .Throw<ArgumentNullException>();
        }

        [AutoDataTestMethod]
        public void Add_ForNumbers_Adds(Calculator    sut,
                                        [Freeze] IAdd add)
        {
            add.Execute(1,
                        2)
               .Returns(3);

            sut.Add(1,
                    2)
               .Should()
               .Be(3);
        }

        [AutoDataTestMethod]
        public void Create_ForAddIsNull_Throws(Lazy<Calculator> sut,
                                               [BeNull] IAdd    add)
        {
            Action action = () =>
                            {
                                // ReSharper disable once UnusedVariable
                                var actual = sut.Value;
                            };

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("add");
        }

        [AutoDataTestMethod]
        public void Create_ForSubtractIsNull_Throws(Lazy<Calculator>   sut,
                                                    [BeNull] ISubtract subtract)
        {
            Action action = () =>
                            {
                                // ReSharper disable once UnusedVariable
                                var actual = sut.Value;
                            };

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("subtract");
        }

        [AutoDataTestMethod]
        public void Subtract_ForNumbers_Subtracts(Calculator         sut,
                                                  [Freeze] ISubtract subtract)
        {
            subtract?.Execute(1,
                              2)
                     .Returns(-1);

            sut.Subtract(1,
                         2)
               .Should()
               .Be(-1);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _add      = Substitute.For<IAdd>();
            _subtract = Substitute.For<ISubtract>();
        }

        [UsedImplicitly]
        private Calculator CreateSut()
        {
            return new Calculator(_add,
                                  _subtract);
        }
    }
}