using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selkie.AutoMocking.Tests
{
    [TestClass]
    public class CustomAttributeDataTests
    {
        private const string MethodNameWithString = "WithString";

        private System.Reflection.CustomAttributeData _customAttributeData;

        [ExcludeFromCodeCoverage]
        [UsedImplicitly]
        public static System.Reflection.ParameterInfo[] WithString([Freeze] string _)
        {
            return Array.Empty<System.Reflection.ParameterInfo>();
        }

        [TestMethod]
        public void Constructor_ForCustomAttributeData_SetsAttributeType()
        {
            CreateSut()
               .AttributeType
               .Should()
               .Be(typeof(FreezeAttribute));
        }

        [TestMethod]
        public void Constructor_ForCustomAttributeDataIsNull_Throws()
        {
            _customAttributeData = null;

            Action action = () => CreateSut();

            action.Should()
                  .Throw<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be("customAttributeData");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var methodInfo = typeof(ParameterInfoTests).GetMethod(MethodNameWithString,
                                                                  new[]
                                                                  {
                                                                      typeof(string)
                                                                  });

            if (methodInfo == null)
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                                            $"Can't find method {MethodNameWithString}");

                throw new Exception(message);
            }

            var infos = methodInfo.GetParameters();
            var info  = infos.First();

            _customAttributeData = info.CustomAttributes.First();
        }

        private CustomAttributeData CreateSut()
        {
            return new CustomAttributeData(_customAttributeData);
        }
    }
}