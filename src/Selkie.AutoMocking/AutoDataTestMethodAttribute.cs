using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selkie.AutoMocking.Interfaces;

namespace Selkie.AutoMocking;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AutoDataTestMethodAttribute : TestMethodAttribute
{
    public AutoDataTestMethodAttribute([CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0)
    {
    }

    public AutoDataTestMethodAttribute([NotNull] TestMethodAttribute testMethodAttribute,
        [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
    {
        Guard.ArgumentNotNull(testMethodAttribute,
            nameof(testMethodAttribute));
        TestMethodAttribute = testMethodAttribute;
    }

    [CanBeNull] public TestMethodAttribute TestMethodAttribute { get; }

    [NotNull] public IArgumentsGenerator Generator { get; } = new ArgumentsGenerator();

    public override Task<TestResult[]> ExecuteAsync(ITestMethod testMethod)
    {
        Guard.ArgumentNotNull(testMethod,
            nameof(testMethod));
        return Invoke(testMethod);
    }

    private async Task<TestResult[]> Invoke(ITestMethod testMethod)
    {
        if (TestMethodAttribute != null)
            return await TestMethodAttribute.ExecuteAsync(testMethod);

        IEnumerable<IParameterInfo> infos = testMethod.ParameterTypes.Select(x => new ParameterInfo(x));
        var arguments = Generator.Create(infos);

        return
        [
            await testMethod.InvokeAsync(arguments)
        ];
    }
}