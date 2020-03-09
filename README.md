# Selkie.AutoMocking
The Selkie.AutoMocking project is an extension for MSTest2 which provides a [SUT Factory](http://blog.ploeh.dk/2009/02/13/SUTFactory.aspx). It tries to replicate the functionality of the package [AutoFixture.AutoNSubstitute](https://github.com/AutoFixture/AutoFixture) and make it available for MSTest2.

**Goal:** The goal is to make writing unit tests easier and more refactoring-safe.
<br>
**How is it done?** The SUT and all the required dependencies are automatically provided to the test method.
<br>
**How do I use it?** Like a normal MSTest2 Data Driven Test Method.
<br>
```csharp
[AutoDataTestMethod]
public void Add_ForNumbers_Adds(Calculator    sut,
                                [Freeze] IAdd add)
{
    add.Execute(1, 2)
       .Returns(3);

    sut.Add(1, 2)
       .Should()
       .Be(3);
}
```
**Freeze?** Freezing a parameter makes sure that the SUT uses the same instance ass the frozen parameter (see [AutoFixture Freeze](https://blog.ploeh.dk/2010/03/17/AutoFixtureFreeze/)).

# Example
## Calculator
Let do a very simple Calculator which will show us how to use the Selkie.AutoMocking package.
<br><br>
The Calculator will only...
* work for integers,
* adds two integers together and
* subtracts two integers from each other.

## Unit testing the Add method
The example below shows how to test the **Add** method using the 'old' and 'new' way.

### Old way...
```csharp
using Calculator.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Calculator.Tests
{
    [TestClass]
    public class OldCalculatorTests
    {
        private IFunctionAdd      _add;
        private IFunctionSubtract _subtract;

        [TestMethod]
        public void Add_ForNumbers_Adds()
        {
            _add.Execute(1, 2)
                .Returns(3);

            CreateSut().Add(1, 2)
                       .Should()
                       .Be(3);
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
```

### New way...
```csharp
using Calculator.Interfaces;
using FluentAssertions;
using NSubstitute;
using Selkie.AutoMocking;

namespace Calculator.Tests
{
    [AutoDataTestClass]
    public class CalculatorTests
    {
        [AutoDataTestMethod]
        public void Add_ForNumbers_Adds(Calculator    sut,
                                        [Freeze] IAdd add)
        {
            add.Execute(1, 2)
               .Returns(3);

            sut.Add(1, 2)
               .Should()
               .Be(3);
        }
    }
}
```
