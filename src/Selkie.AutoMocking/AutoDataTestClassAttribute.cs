using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selkie.AutoMocking
{
    public class AutoDataTestClassAttribute
        : TestClassAttribute
    {
        public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
        {
            return testMethodAttribute is AutoDataTestMethodAttribute
                       ? testMethodAttribute
                       : new AutoDataTestMethodAttribute(base.GetTestMethodAttribute(testMethodAttribute));
        }
    }
}