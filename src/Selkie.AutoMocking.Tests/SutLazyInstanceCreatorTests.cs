using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Selkie.AutoMocking.Interfaces;
using Selkie.AutoMocking.Tests.TestClasses;

namespace Selkie.AutoMocking.Tests
{
    [TestClass]
    public class SutLazyInstanceCreatorTests
    {
        private const string ParameterName = "Name";

        private IArgumentNullExceptionFinder _exceptionFinder;
        private IArgumentsGenerator          _generator;
        private Something                    _instance;
        private Type                         _typeClass;
        private Type                         _typeInt;
        private ICustomAttributeFinder _attributeFinder;

        [TestInitialize]
        public void Initialize()
        {
            _exceptionFinder    = Substitute.For<IArgumentNullExceptionFinder>();
            _attributeFinder = Substitute.For<ICustomAttributeFinder>();
            _generator = Substitute.For<IArgumentsGenerator>();
            _typeClass = typeof(Something);
            _typeInt   = typeof(int);
            _instance  = new Something(new SomethingElse());
        }

        [TestMethod]
        public void Constructor_ForExceptionFinderIsNull_Throws()
        {
            _exceptionFinder = null;

            Action action = () => { CreateSut(); };

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("exceptionFinder");
        }

        [TestMethod]
        public void Constructor_ForAttributeFinderIsNull_Throws()
        {
            _attributeFinder = null;

            Action action = () => { CreateSut(); };

            action.Should()
                .Throw<ArgumentNullException>()
                .WithParameter("attributeFinder");
        }

        [TestMethod]
        public void Construct_ForGeneratorIsNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = () => { CreateSut().Construct(null, _typeClass); };

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("generator");
        }

        [TestMethod]
        public void Construct_ForTypeIsNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = () => { CreateSut().Construct(_generator, null); };

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("type");
        }

        [TestMethod]
        public void Construct_ForTypeLazyClass_Instance()
        {
            _generator.CreateArgument(_typeClass)
                      .Returns(_instance);

            var actual = CreateSut().Construct(_generator,
                                               _typeClass);

            using (new AssertionScope())
            {
                actual.Should()
                      .NotBeNull();

                var lazy = actual as Lazy<Something>;

                lazy.Should()
                    .NotBeNull();

                lazy?.IsValueCreated
                     .Should()
                     .BeFalse();

                var lazyValue = lazy?.Value;

                lazyValue?.Should()
                          .Be(_instance);
            }
        }

        [TestMethod]
        public void Construct_ForTypeLazyValueType_Instance()
        {
            _generator.CreateArgument(_typeInt)
                      .Returns(1);

            var actual = CreateSut().Construct(_generator,
                                               _typeInt);

            using (new AssertionScope())
            {
                actual.Should()
                      .NotBeNull();

                var lazy = actual as Lazy<int>;

                lazy.Should()
                    .NotBeNull();

                lazy?.IsValueCreated
                     .Should()
                     .BeFalse();

                var lazyValue = lazy?.Value;

                lazyValue?.Should()
                          .Be(1);
            }
        }

        [TestMethod]
        public void Construct_ForGeneratorThrows_Throws()
        {
            FinderReturns(false);
            GeneratorCreateThrows();

            var actual = CreateSut().Construct(_generator,
                                               _typeClass);

            Action action = () =>
                            {
                                var lazy = actual as Lazy<Something>;
                                // ReSharper disable once UnusedVariable
                                var test = lazy?.Value;
                            };

            action.Should()
                  .Throw<Exception>();
        }

        [TestMethod]
        public void Construct_ForGeneratorThrowsAndFinderReturnsTrue_ThrowsWithParameterName()
        {
            FinderReturns(true);
            GeneratorCreateThrows();

            var actual = CreateSut().Construct(_generator,
                                               _typeClass);

            Action action = () =>
                            {
                                var lazy = actual as Lazy<Something>;
                                // ReSharper disable once UnusedVariable
                                var test = lazy?.Value;
                            };

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter(ParameterName);
        }

        [TestMethod]
        public void Construct_ForTypeClass_InstanceWithPropertySetByDependency()
        {
            var sut = new SutLazyInstanceCreator(new ArgumentNullExceptionFinder(),
                                                 new CustomAttributeFinder());

            var construct = sut.Construct(new ArgumentsGenerator(),
                typeof(Lazy<Device>)) as Lazy<Device>;

            construct?.Value
                .PopulatedByDependency
                .Should()
                .NotBeNull();
        }

        [TestMethod]
        public void Construct_ForTypeClass_InstanceWithPropertyNotAutoPopulated()
        {
            var sut = new SutLazyInstanceCreator(new ArgumentNullExceptionFinder(),
                                                 new CustomAttributeFinder());

            var construct = sut.Construct(new ArgumentsGenerator(),
                typeof(Lazy<Device>)) as Lazy<Device>;

            construct?.Value
                .NotAutoPopulated
                .Should()
                .Be(Device.SomeText);
        }

        private void GeneratorCreateThrows()
        {
            _generator.When(x => x.CreateArgument(_typeClass))
                      .Do(_ => throw new Exception("Test"));
        }

        private void FinderReturns(bool value)
        {
            if (!value)
                _exceptionFinder.TryFindArgumentNullException(Arg.Any<Exception>(),
                                                     out Arg.Any<ArgumentNullException>())
                       .Returns(x =>
                                {
                                    x[1] = null;

                                    return false;
                                });
            else
                _exceptionFinder.TryFindArgumentNullException(Arg.Any<Exception>(),
                                                     out Arg.Any<ArgumentNullException>())
                       .Returns(x =>
                                {
                                    x[1] = new ArgumentNullException(ParameterName);

                                    return true;
                                });
        }

        private SutLazyInstanceCreator CreateSut()
        {
            return new SutLazyInstanceCreator(_exceptionFinder,
                                              _attributeFinder);
        }
    }
}