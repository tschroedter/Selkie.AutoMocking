using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Tests
{
    [TestClass]
    public class SubtractTests
    {
        [TestMethod]
        public void Execute_ForTwoNumbers_AddsNumbers()
        {
            CreateSut()
               .Execute(1,
                        2)
               .Should()
               .Be(-1);
        }

        private Subtract CreateSut()
        {
            return new Subtract();
        }
    }
}