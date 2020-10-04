using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selkie.AutoMocking.Tests
{
    [TestClass]
    public class ArgumentNullExceptionFinderTests
    {
        private const string ParameterName = "Name";
        private const string Message       = "Value cannot be null.";

        private ArgumentException     _argumentException;
        private ArgumentNullException _argumentNullException;
        private Exception             _exceptionWithNull;
        private Exception             _exceptionWithoutNull;

        [TestInitialize]
        public void Initialize()
        {
            _argumentNullException = new ArgumentNullException(ParameterName,
                                                               Message);

            _argumentException = new ArgumentException(Message,
                                                       ParameterName);

            _exceptionWithoutNull = new Exception("General",
                                                  new IndexOutOfRangeException("Index"));

            _exceptionWithNull = new Exception("General",
                                               _argumentException);
        }

        [TestMethod]
        public void TryFindArgumentNullException_ForExceptionIsNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = () =>
                            {
                                CreateSut().TryFindArgumentNullException(null,
                                                                         out _);
                            };

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("exception");
        }

        [TestMethod]
        public void TryFindArgumentNullException_ForMaxDepthIs0_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = () =>
                            {
                                CreateSut().TryFindArgumentNullException(_argumentNullException,
                                                                         out _,
                                                                         0);
                            };

            action.Should()
                  .Throw<ArgumentException>()
                  .WithParameter("maxDepth");
        }

        [TestMethod]
        public void TryFindArgumentNullException_ForExceptionIsArgumentNullException_ReturnsTrue()
        {
            CreateSut().TryFindArgumentNullException(_argumentNullException,
                                                     out _)
                       .Should()
                       .BeTrue();
        }

        [TestMethod]
        public void TryFindArgumentNullException_ForExceptionIsArgumentNullException_ReturnsOutArgumentNullException()
        {
            CreateSut().TryFindArgumentNullException(_argumentNullException,
                                                     out var nullException);

            nullException
               .Should()
               .Be(_argumentNullException);
        }

        [TestMethod]
        public void TryFindArgumentNullException_ForExceptionWithoutHiddenArgumentNullException_ReturnsFalse()
        {
            CreateSut().TryFindArgumentNullException(_exceptionWithoutNull,
                                                     out _)
                       .Should()
                       .BeFalse();
        }

        [TestMethod]
        public void TryFindArgumentNullException_ForExceptionWithoutHiddenArgumentNullException_ReturnsOutNull()
        {
            CreateSut().TryFindArgumentNullException(_exceptionWithoutNull,
                                                     out var nullException);

            nullException
               .Should()
               .BeNull();
        }

        [TestMethod]
        public void TryFindArgumentNullException_ForExceptionWithHiddenArgumentNullException_ReturnsTrue()
        {
            CreateSut().TryFindArgumentNullException(_exceptionWithNull,
                                                     out _)
                       .Should()
                       .BeTrue();
        }

        [TestMethod]
        public void TryFindArgumentNullException_ForExceptionWithHiddenArgumentNullException_ReturnsOutException()
        {
            CreateSut().TryFindArgumentNullException(_exceptionWithNull,
                                                     out var nullException);

            using (new AssertionScope())
            {
                nullException.ParamName
                             .Should()
                             .Be(_argumentException.ParamName);

                nullException.Message
                             .Should()
                             .Be(_argumentException.Message);
            }
        }

        private ArgumentNullExceptionFinder CreateSut()
        {
            return new ArgumentNullExceptionFinder();
        }
    }
}