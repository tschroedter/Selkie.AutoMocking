# GitHub Copilot Instructions for Selkie.AutoMocking

## Project Overview
Selkie.AutoMocking is a .NET library that provides an extension for MSTest2, offering a SUT (System Under Test) Factory pattern. It replicates the functionality of AutoFixture.AutoNSubstitute for MSTest2, making unit testing easier and more refactoring-safe.

## Technology Stack
- **Framework**: .NET 8.0
- **Testing Framework**: MSTest2
- **Mocking Library**: NSubstitute
- **Auto-Mocking**: AutoFixture with AutoNSubstitute
- **Assertion Library**: FluentAssertions (in examples)
- **Language**: C# 12.0

## Code Style and Conventions

### General Guidelines
- Follow standard C# naming conventions (PascalCase for public members, _camelCase for private fields)
- Use nullable reference types appropriately
- Apply JetBrains.Annotations attributes where applicable ([NotNull], [CanBeNull])
- Use Guard clauses for constructor parameter validation
- Target .NET 8.0 features and idioms

### Testing Conventions
- Use `[AutoDataTestClass]` for test classes
- Use `[AutoDataTestMethod]` for test methods
- Use `[Freeze]` attribute to ensure SUT uses the same instance as the frozen parameter
- Use `[BeNull]` attribute when testing null parameter validation
- Use `Lazy<T>` to delay SUT creation when testing constructors
- Follow the naming pattern: `MethodName_ForCondition_ExpectedBehavior`

### Code Structure
- Place dependencies in constructor parameters with proper null checks
- Use readonly fields for injected dependencies
- Prefer interface-based dependencies for testability
- Keep methods focused and single-purpose

## When Generating Code

### Test Code
- Always generate tests using AutoDataTestMethod pattern
- Automatically inject SUT and dependencies as method parameters
- Use [Freeze] for dependencies that need to be configured in the test
- Don't create explicit Setup methods - let AutoMocking handle initialization
- Use FluentAssertions for assertions (`.Should().Be()`, `.Should().Throw<>()`, etc.)

### Production Code
- Include JetBrains annotations for null-safety
- Add Guard.ArgumentNotNull checks in constructors
- Follow dependency injection patterns
- Make classes and methods testable by default

### Example Test Pattern
```csharp
[AutoDataTestMethod]
public void MethodName_ForCondition_ExpectedResult(
    ClassName sut,
    [Freeze] IDependency dependency)
{
    // Arrange
    dependency.Method().Returns(expectedValue);
    
    // Act
    var result = sut.MethodUnderTest();
    
    // Assert
    result.Should().Be(expectedValue);
}
```

### Constructor Null Tests
```csharp
[AutoDataTestMethod]
public void Create_ForDependencyIsNull_Throws(
    Lazy<ClassName> sut,
    [BeNull] IDependency dependency)
{
    Action action = () =>
    {
        var actual = sut.Value;
    };
    
    action.Should()
          .Throw<ArgumentNullException>()
          .WithParameter("dependency");
}
```

## Best Practices
- Minimize use of setup/teardown methods - leverage AutoMocking instead
- Keep tests isolated and independent
- Use meaningful test names that describe the scenario and expected outcome
- Avoid magic numbers - use named constants or parameters
- Test one behavior per test method
- Mock external dependencies using NSubstitute's `.Returns()` method

## Documentation
- Add XML documentation comments for public APIs
- Include usage examples in README when adding new features
- Keep code comments minimal but meaningful
- Document non-obvious behaviors or workarounds

## Dependencies Management
- Keep NuGet packages up to date
- Ensure compatibility with latest MSTest2, NSubstitute, and AutoFixture versions
- Target the latest LTS .NET version (currently .NET 8.0)
