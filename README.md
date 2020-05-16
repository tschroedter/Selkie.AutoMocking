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
**AutoDataTestMethod?** The attribute tells MSTest to use the Selkie.AutoMocking extension of the DataTestMethod.
<br>
**Freeze?** Freezing a parameter makes sure that the SUT uses the same instance ass the frozen parameter (see [AutoFixture Freeze](https://blog.ploeh.dk/2010/03/17/AutoFixtureFreeze/)).

# Example
## Calculator
Let do a very simple Calculator which will show us how to use the Selkie.AutoMocking package.
<br><br>
The Calculator will only...
* work for integers and 
* adds two integers together.

_The complete source code can be found here: [Example](https://github.com/tschroedter/Selkie.AutoMocking/tree/master/src/Example)_

### The Calculator Class
The Calculator class depends on the IAdd interface which is injected in the constructor.

```csharp
public class Calculator : ICalculator
{
    private readonly IAdd _add;

    public Calculator([NotNull] IAdd add)
    {
        _add = add;
    }

    public int Add(int a, int b)
    {
        return _add.Execute(a, b);
    }
}
```

### The Add Class
The job of the Add class is just to add 2 integers together.

```csharp
public class Add : IAdd
{
    public int Execute(int a, int b)
    {
        return a + b;
    }
}
```

### Unit testing the Calculator's Add method
The example below shows how to test the **Add** method using the AutoDataTestMethod of the Selkie.AutoMocking package.

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

_Note: There isn't a method to initialize the test nor a CreateSut() method!_

## Adding subtraction to the Calculator 
Now we add the subtraction functionality to the Calculator class by adding a dependency in the constructor.

```csharp
public class Calculator : ICalculator
{
    private readonly IAdd      _add;
    private readonly ISubtract _subtract;

    public Calculator([NotNull] IAdd      add,
                      [NotNull] ISubtract subtract)
    {
        _add      = add;
        _subtract = subtract;
    }

    public int Add(int a, int b)
    {
        return _add.Execute(a, b);
    }

    public int Subtract(int a, int b)
    {
        return _subtract.Execute(a, b);
    }
}
```

### How is the unit test class affected?
No existing code needs to be modified, only the new test for the subtraction needs to be added!

```csharp
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

    [AutoDataTestMethod]
    public void Subtract_ForNumbers_Subtracts(Calculator         sut,
                                              [Freeze] ISubtract subtract)
    {
        subtract?.Execute(1, 2)
                 .Returns(-1);

        sut.Subtract(1, 2)
           .Should()
           .Be(-1);
    }
}
```
