using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Tests
{
    [TestClass]
    public class AddTests
    {
        [TestMethod]
        public void Execute_ForTwoNumbers_AddsNumbers()
        {
            CreateSut()
               .Execute(1,
                        2)
               .Should()
               .Be(3);
        }

        private Add CreateSut()
        {
            return new Add();
        }
    }
}