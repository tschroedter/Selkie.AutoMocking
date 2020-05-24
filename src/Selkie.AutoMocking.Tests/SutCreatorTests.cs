using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Selkie.AutoMocking.Interfaces;
using Selkie.AutoMocking.Tests.TestClasses;

namespace Selkie.AutoMocking.Tests
{
    [TestClass]
    public class SutCreatorTests
    {
        private Something               _class;
        private ISutInstanceCreator     _creator;
        private IArgumentsGenerator     _generator;
        private Lazy<Something>         _lazyClass;
        private ISutLazyInstanceCreator _lazyCreator;
        private Type                    _typeClass;
        private Type                    _typeLazyClass;

        [TestInitialize]
        public void Initialize()
        {
            _creator       = Substitute.For<ISutInstanceCreator>();
            _lazyCreator   = Substitute.For<ISutLazyInstanceCreator>();
            _generator     = Substitute.For<IArgumentsGenerator>();
            _typeClass     = typeof(Something);
            _typeLazyClass = typeof(Lazy<Something>);
            _class         = new Something(new SomethingElse());
            _lazyClass     = new Lazy<Something>(() => new Something(new SomethingElse()));
        }

        [TestMethod]
        public void Constructor_ForCreatorIsNull_Throws()
        {
            _creator = null;

            Action action = () => { CreateSut(); };

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("creator");
        }

        [TestMethod]
        public void Constructor_ForLazyCreatorIsNull_Throws()
        {
            _lazyCreator = null;

            Action action = () => { CreateSut(); };

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("lazyCreator");
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
        public void Construct_ForTypeClass_Instance()
        {
            _creator.Construct(_generator,
                               _typeClass)
                    .Returns(_class);

            CreateSut().Construct(_generator,
                                  _typeClass)
                       .Should()
                       .Be(_class);
        }

        [TestMethod]
        public void Construct_ForTypeLazyClass_Instance()
        {
            _lazyCreator.Construct(_generator,
                                   _typeLazyClass.GenericTypeArguments
                                                 .First())
                        .Returns(_lazyClass);

            CreateSut().Construct(_generator,
                                  _typeLazyClass)
                       .Should()
                       .Be(_lazyClass);
        }

        private SutCreator CreateSut()
        {
            return new SutCreator(_creator,
                                  _lazyCreator);
        }
    }
}