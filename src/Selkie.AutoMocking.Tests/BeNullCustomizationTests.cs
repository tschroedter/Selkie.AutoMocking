using System ;
using System.Collections.Generic ;
using AutoFixture ;
using AutoFixture.Kernel ;
using FluentAssertions ;
using JetBrains.Annotations ;
using Microsoft.VisualStudio.TestTools.UnitTesting ;
using NSubstitute ;
using Selkie.AutoMocking.Tests.TestClasses ;

namespace Selkie.AutoMocking.Tests
{
    [TestClass]
    public class BeNullCustomizationTests
    {
        private Type                      _targetType ;
        private Type                      _registeredType ;
        private IFixture                  _fixture ;
        private List < ISpecimenBuilder > _builders ;

        [ TestInitialize ]
        public void Initialize ( )
        {
            _targetType     = typeof ( Something ) ;
            _registeredType = typeof ( ISomething ) ;

            _fixture                = Substitute.For < IFixture > ( ) ;

            _builders = new List<ISpecimenBuilder>();

            _fixture.Customizations
                    .Returns ( _builders );
        }

        [TestMethod]
        public void Constructor_ForWithSingleParameterAndTargetTypeIsNull_Throws()
        {
            Action action = () => CreateSut(null);

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("targetType");
        }

        [TestMethod]
        public void Constructor_ForTargetTypeIsNull_Throws()
        {
            _targetType = null;

            Action action = () => CreateSut();

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("targetType");
        }

        [TestMethod]
        public void Constructor_ForRegisteredTypeIsNull_Throws()
        {
            _registeredType = null;

            Action action = () => CreateSut();

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("registeredType");
        }

        [TestMethod]
        public void Constructor_ForRegisteredTypeIsNotAssignableFromTargetType_Throws()
        {
            _registeredType = typeof ( ISomethingElse ) ;

            Action action = () => CreateSut();

            action.Should()
                  .Throw<ArgumentException>()
                  .WithParameter("registeredType");
        }

        [TestMethod]
        public void Constructor_ForTargetType_SetsTargetType()
        {
            CreateSut()
               .TargetType
               .Should()
               .Be(_targetType);
        }

        [TestMethod]
        public void Constructor_ForRegisteredType_SetsRegisteredType()
        {
            CreateSut()
               .RegisteredType
               .Should()
               .Be(_registeredType);
        }

        [TestMethod]
        public void Customize_ForFixtureIsNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Action action = () => CreateSut().Customize ( null );

            action.Should()
                  .Throw<ArgumentNullException>()
                  .WithParameter("fixture");
        }

        [TestMethod]
        public void Customize_ForFixture_CallsCustomizations()
        {
            CreateSut().Customize(_fixture);

            _builders.Count
                     .Should ( )
                     .Be ( 1 ) ;
        }

        private BeNullCustomization CreateSut()
        {
            return new BeNullCustomization(_targetType,
                                           _registeredType);
        }

        [UsedImplicitly]
        private BeNullCustomization CreateSut(Type targetType)
        {
            return new BeNullCustomization(targetType);
        }
    }
}