using System;
using Calculator.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Calculator.Tests
{
    [TestClass]
    public class OldCalculatorTests
    {
        private IAdd      _add;
        private ISubtract _subtract;

        [TestMethod]
        public void Add_ForNumbers_Adds()
        {
            _add.Execute(1,
                         2)
                .Returns(3);

            CreateSut().Add(1,
                            2)
                       .Should()
                       .Be(3);
        }

        [TestMethod]
        public void Create_ForAddIsNull_Throws()
        {
            _add = null;

            Action action = () => CreateSut();

            action.Should()
                  .Throw<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be("add");
        }

        [TestMethod]
        public void Create_ForSubtractIsNull_Throws()
        {
            _subtract = null;

            Action action = () => CreateSut();

            action.Should()
                  .Throw<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be("subtract");
        }

        [TestMethod]
        public void Subtract_ForNumbers_Subtracts()
        {
            _subtract.Execute(1,
                              2)
                     .Returns(-1);

            CreateSut().Subtract(1,
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

        private Calculator CreateSut()
        {
            return new Calculator(_add,
                                  _subtract);
        }
    }
}